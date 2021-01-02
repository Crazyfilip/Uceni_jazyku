using log4net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;

namespace Uceni_jazyku.User_database
{
    /// <inheritdoc/>
    /// <summary>
    /// Repository represented as a list
    /// </summary>
    public class UserAccountRepository : IUserAccountRepository
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(UserAccountRepository));
        private List<UserAccount> userDatabase = new List<UserAccount>();
        private string databasePath = "./users/accounts.txt";

        /// <summary>
        /// constructor of UserAccountRepository
        /// Load data from file if exists otherwise initialize empty list
        /// </summary>
        public UserAccountRepository()
        {
            if (File.Exists(databasePath))
            {
                log.Trace("Loading data from file");
                var serializer = new DataContractSerializer(typeof(List<UserAccount>));
                using XmlReader reader = XmlReader.Create(databasePath);
                userDatabase = (List<UserAccount>)serializer.ReadObject(reader);
            }
            else
            {
                log.Trace("Initializing new collection");
                userDatabase = new List<UserAccount>();
            }
        }

        /// <summary>
        /// Initialize repository with existing data
        /// </summary>
        /// <param name="accounts">List of data</param>
        public UserAccountRepository(List<UserAccount> accounts)
        {
            userDatabase = accounts;
            SaveDatabase();
        }

        public void AddUserAccount(UserAccount userAccount)
        {
            log.Info("Adding user account to repository");
            userDatabase.Add(userAccount);
            SaveDatabase();
        }

        public UserAccount GetUserAccount(string username)
        {
            log.Info($"Looking for account with username: {username}");
            return userDatabase.Find(x => x.username == username);
        }

        private void SaveDatabase()
        {
            log.Trace("Saving repository to file");
            var serializer = new DataContractSerializer(typeof(List<UserAccount>));
            using XmlWriter writer = XmlWriter.Create(databasePath);
            serializer.WriteObject(writer, userDatabase);
        }
    }
}
