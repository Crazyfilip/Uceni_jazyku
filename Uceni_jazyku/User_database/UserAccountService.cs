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
        private IUserAccountRepository userAccountRepository;
        private CycleService cycleService;

        public UserAccountService() : this(null, null) {}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="repository"></param>
        public UserAccountService(IUserAccountRepository repository, CycleService cycleService)
        {
            userAccountRepository = repository ?? new UserAccountRepository();
            this.cycleService = cycleService ?? CycleService.GetInstance();
        }

        /// <summary>
        /// Login user if user exists and return user's cycle in that case
        /// </summary>
        /// <param name="username">username</param>
        /// <param name="password">password</param>
        /// <returns>User's cycle or null</returns>
        public UserCycle Login(string username, string password)
        {
            if (VerifyUser(username, password))
            {
                return cycleService.GetUserCycle(username);
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
    }
}
