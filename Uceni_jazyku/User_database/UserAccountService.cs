﻿using Uceni_jazyku.Cycles;
using System;
using System.Security.Cryptography;
using log4net;

namespace Uceni_jazyku.User_database
{

    /// <summary>
    /// Class for managing user accounts
    /// Login and creating new users
    /// </summary>
    public class UserAccountService
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(UserAccountService));
        private IUserAccountRepository userAccountRepository;
        private CycleService cycleService;

        /// <summary>
        /// This constructor calls UserAccountService(IUserAccountRepository, CycleService) with null values
        /// </summary>
        public UserAccountService() : this(null, null) {}

        /// <summary>
        /// Constructor of UserAccountService. When IUserAccountRepository or CycleService are not provided (null values)
        /// it gets default instances for those.
        /// </summary>
        /// <param name="repository">object implementing IUserAccountRepository</param>
        /// <param name="cycleService">instance of CycleService</param>
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
            log.Info($"Login attempt of user {username}");
            if (VerifyUser(username, password))
            {
                log.Debug("Login successful");
                return cycleService.GetUserCycle(username);
            }
            else
            {
                log.Debug("Login failed");
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

            userAccount = new UserAccount();
            userAccount.username = username;

            log.Debug($"Calculating hash for user {username}");
            byte[] salt;
            new RNGCryptoServiceProvider().GetBytes(salt = new byte[16]);
            userAccount.salt = Convert.ToBase64String(salt);

            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 1000);
            userAccount.loginCredential = Convert.ToBase64String(pbkdf2.GetBytes(20));
            log.Debug("Registering user to repository");
            userAccountRepository.AddUserAccount(userAccount);
            return true;
        }
    }
}
