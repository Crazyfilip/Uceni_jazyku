using log4net;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;

namespace Uceni_jazyku.User_database
{
    /// <inheritdoc/>
    /// <summary>
    /// Repository represented as a list
    /// </summary>
    public class UserAccountRepository : IUserAccountRepository
    {
        private static ILog log = LogManager.GetLogger(typeof(UserAccountRepository));
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
                log.Debug("Loading data from file");
                var serializer = new DataContractSerializer(typeof(List<UserAccount>));
                using XmlReader reader = XmlReader.Create(databasePath);
                userDatabase = (List<UserAccount>)serializer.ReadObject(reader);
            }
            else
            {
                log.Debug("Initializing new collection");
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

        /// <inheritdoc/>
        public void Create(UserAccount userAccount)
        {
            log.Info("Adding user account to repository");
            userDatabase.Add(userAccount);
            SaveDatabase();
        }

        /// <inheritdoc/>
        public void Delete(UserAccount value)
        {
            userDatabase.Remove(value);
            SaveDatabase();
        }

        /// <inheritdoc/>
        public UserAccount Get(string username)
        {
            log.Info($"Looking for account with username: {username}");
            return userDatabase.Find(x => x.username == username);
        }

        /// <inheritdoc/>
        public void Update(UserAccount user)
        {
            int index = userDatabase.FindIndex(x => x.username == user.username);
            if (index != -1)
                userDatabase[index] = user;
            SaveDatabase();
        }

        private void SaveDatabase()
        {
            log.Debug("Saving repository to file");
            var serializer = new DataContractSerializer(typeof(List<UserAccount>));
            using XmlWriter writer = XmlWriter.Create(databasePath);
            serializer.WriteObject(writer, userDatabase);
        }
    }
}
