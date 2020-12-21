using Uceni_jazyku.Cycles;
using System;
using System.Security.Cryptography;

namespace Uceni_jazyku.User_database
{

    /// <summary>
    /// Class for managing user accounts
    /// Login and creating new users
    /// </summary>
    public class UserAccountService
    {
        private static UserAccountService instance;
        private IUserAccountRepository userAccountRepository;

        private UserAccountService(IUserAccountRepository repository)
        {
            userAccountRepository = repository ?? new UserAccountRepository();
        }

        /// <summary>
        /// Getter of UserAccountService instance
        /// </summary>
        /// <returns>UserAccountService's instance</returns>
        public static UserAccountService GetInstance()
        {
            if (instance == null)
                instance = new UserAccountService(null);
            return instance;
        }

        /// <summary>
        /// Getter of UserAccountService instance
        /// </summary>
        /// <param name="repository">UserAccountRepository to be used in repository</param>
        /// <returns>UserAccountService's instance</returns>
        public static UserAccountService GetInstance(IUserAccountRepository repository)
        {
            if (instance == null)
                instance = new UserAccountService(repository);
            return instance;
        }

        /// <summary>
        /// Login user if user exists and return user's cycle in that case
        /// </summary>
        /// <param name="username">username</param>
        /// <param name="password">password</param>
        /// <returns>User's cycle or null</returns>
        public UserCycle Login(string username, string password)
        {
            CycleService service = CycleService.GetInstance();
            if (VerifyUser(username, password))
            {
                return service.GetUserCycle(username);
            }
            else
            {
                return null;
            }
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
                return false;

            var pbkdf2 = new Rfc2898DeriveBytes(password, Convert.FromBase64String(userAccount.salt), 1000);
            string loginCredential = Convert.ToBase64String(pbkdf2.GetBytes(20));

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
            UserAccount userAccount = userAccountRepository.GetUserAccount(username);
            if (userAccount != null)
                return false;

            userAccount = new UserAccount();
            userAccount.username = username;

            byte[] salt;
            new RNGCryptoServiceProvider().GetBytes(salt = new byte[16]);
            userAccount.salt = Convert.ToBase64String(salt);

            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 1000);
            userAccount.loginCredential = Convert.ToBase64String(pbkdf2.GetBytes(20));

            userAccountRepository.AddUserAccount(userAccount);
            return true;
        }

        public static void Deallocate()
        {
            instance = null;
        }
    }
}
