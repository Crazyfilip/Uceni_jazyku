using Uceni_jazyku.Cycles;
using System;
using System.Security.Cryptography;
using log4net;
using Uceni_jazyku.Language;
using Uceni_jazyku.User;
using System.IO;

namespace Uceni_jazyku.User_database
{

    /// <summary>
    /// Class for managing user accounts
    /// Login and creating new users
    /// </summary>
    public class UserAccountService
    {
        private static ILog log = LogManager.GetLogger(typeof(UserAccountService));
        private readonly IUserAccountRepository userAccountRepository;
        private readonly IUserModelRepository userModelRepository;
        private readonly CycleService cycleService;
        private readonly LanguageCourseService languageCourseService;

        private readonly string path = "./users/loggedUser.txt";

        /// <summary>
        /// This constructor calls UserAccountService(IUserAccountRepository, CycleService) with null values
        /// </summary>
        public UserAccountService() : this(null, null, null, null) {}

        /// <summary>
        /// Constructor of UserAccountService. When IUserAccountRepository or CycleService are not provided (null values)
        /// it gets default instances for those.
        /// </summary>
        /// <param name="repository">object implementing IUserAccountRepository</param>
        /// <param name="cycleService">instance of CycleService</param>
        /// <param name="languageCourseService">instance of LanguageCourseService</param>
        public UserAccountService(IUserAccountRepository repository, CycleService cycleService, LanguageCourseService languageCourseService, IUserModelRepository userModelRepository)
        {
            this.userAccountRepository = repository ?? new UserAccountRepository();
            this.userModelRepository = userModelRepository ?? new UserModelRepository();
            this.cycleService = cycleService ?? CycleService.GetInstance();
            this.languageCourseService = languageCourseService ?? LanguageCourseService.GetInstance();
        }

        /// <summary>
        /// Login user if user exists and return user's cycle in that case
        /// 
        /// Save in special file with username in it as flag who is logged.
        /// </summary>
        /// <param name="username">username</param>
        /// <param name="password">password</param>
        /// <returns>User's cycle or null</returns>
        // TODO separete from cycle logic?
        public UserCycle Login(string username, string password)
        {
            log.Info($"Login attempt of user {username}");
            if (VerifyUser(username, password))
            {
                log.Debug("Login successful");
                File.WriteAllText(path, username);
                cycleService.SetActiveCourse(username, languageCourseService.GetActiveLanguageCourse(username));
                return cycleService.GetNextCycle(username);
            }
            else
            {
                log.Debug("Login failed");
                return null;
            }
        }

        /// <summary>
        /// Check if anyone is logged in based on the present of file where is saved who is logged.
        /// </summary>
        /// <returns>True if someone is logged</returns>
        public bool IsAnyoneLogged()
        {
            return File.Exists(path);
        }

        /// <summary>
        /// Getter of loggged user's name.
        /// </summary>
        /// <returns>name of logged user</returns>
        public string GetLoggedUser()
        {
            return File.ReadAllText(path);
        }

        /// <summary>
        /// Remove file with information who is logged.
        /// </summary>
        public void Logout()
        {
            File.Delete(path);
        }

        /// <summary>
        /// Verify if user with given credentials exists
        /// </summary>
        /// <param name="username">username</param>
        /// <param name="password">password</param>
        /// <returns>true when user exists</returns>
        private bool VerifyUser(string username, string password)
        {
            UserAccount userAccount = userAccountRepository.GetUserAccount(username);
            if (userAccount == null)
            {
                log.Debug($"No account with username: {username}");
                return false;
            }
            log.Debug($"Calculating hash for user {username}");
            var pbkdf2 = new Rfc2898DeriveBytes(password, Convert.FromBase64String(userAccount.salt), 1000);
            string loginCredential = Convert.ToBase64String(pbkdf2.GetBytes(20));
            log.Debug("Verifying if hash is correct");
            return userAccount.loginCredential == loginCredential;
        }

        /// <summary>
        /// Create user, unless user with same name already exists
        /// </summary>
        /// <param name="username">username</param>
        /// <param name="password">password</param>
        /// <returns>true if user is created, false when username is already used</returns>
        public bool CreateUser(string username, string password)
        {
            log.Info($"Verifying if there isn't already account with username {username}");
            UserAccount userAccount = userAccountRepository.GetUserAccount(username);
            if (userAccount != null)
            {
                log.Debug($"Account with username {username} already exists");
                return false;
            }
            log.Info($"Creating new user account with username {username}");

            userAccount = new UserAccount
            {
                username = username
            };

            log.Debug($"Calculating hash for user {username}");
            byte[] salt;
            new RNGCryptoServiceProvider().GetBytes(salt = new byte[16]);
            userAccount.salt = Convert.ToBase64String(salt);

            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 1000);
            userAccount.loginCredential = Convert.ToBase64String(pbkdf2.GetBytes(20));
            log.Debug("Registering user to repository");
            userAccountRepository.AddUserAccount(userAccount);

            // TODO will be in serapate method as in UI course is setup in second step
            LanguageCourse course = languageCourseService.GetLanguageCourseInstanceFromTemplate("template-default", username);
            cycleService.SetActiveCourse(username, course);
            CreateUserModel(username, course.CourseId);
            return true;
        }

        private void CreateUserModel(string username, string coursesId)
        {
            UserModel userModel = new UserModel() { Username = username, CourseId = coursesId, ModelId = Guid.NewGuid().ToString() };
            userModelRepository.InsertUserModel(userModel);
        }
    }
}
