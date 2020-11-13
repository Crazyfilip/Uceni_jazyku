using Uceni_jazyku.Cycles;
using System.IO;
using System.Collections.Generic;
using System;
using System.Security.Cryptography;
using System.Xml;
using System.Runtime.Serialization;

namespace Uceni_jazyku.User_database
{

    /// <summary>
    /// Class for managing user accounts
    /// Login and creating new users
    /// </summary>
    public class UserAccountService
    {
        private static UserAccountService instance;
        private List<UserAccount> userDatabase = new List<UserAccount>();
        private string databasePath = "./users/accounts.txt";

        private UserAccountService()
        {
            if (File.Exists(databasePath))
            {
                var serializer = new DataContractSerializer(typeof(List<UserAccount>));
                using XmlReader reader = XmlReader.Create(databasePath);
                userDatabase = (List<UserAccount>)serializer.ReadObject(reader);
            }
            else
            {
                userDatabase = new List<UserAccount>();
            }
        }

        /// <summary>
        /// Getter of UserAccountService instance
        /// </summary>
        /// <returns>UserAccountService's instance</returns>
        public static UserAccountService GetInstance()
        {
            if (instance == null)
                instance = new UserAccountService();
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
            UserAccount userAccount = userDatabase.Find(x => x.username == username);
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
            UserAccount userAccount = userDatabase.Find(x => x.username == username);
            if (userAccount != null)
                return false;

            userAccount = new UserAccount();
            userAccount.username = username;

            byte[] salt;
            new RNGCryptoServiceProvider().GetBytes(salt = new byte[16]);
            userAccount.salt = Convert.ToBase64String(salt);

            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 1000);
            userAccount.loginCredential = Convert.ToBase64String(pbkdf2.GetBytes(20));

            userDatabase.Add(userAccount);
            SaveDatabase();
            return true;
        }

        private void SaveDatabase()
        {
            var serializer = new DataContractSerializer(typeof(List<UserAccount>));
            using XmlWriter writer = XmlWriter.Create(databasePath);
            serializer.WriteObject(writer, userDatabase);
        }

        public static void Deallocate()
        {
            instance = null;
        }
    }
}
