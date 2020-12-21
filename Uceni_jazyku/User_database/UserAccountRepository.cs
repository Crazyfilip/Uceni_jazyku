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
            userDatabase.Add(userAccount);
            SaveDatabase();
        }

        public UserAccount GetUserAccount(string username)
        {
            return userDatabase.Find(x => x.username == username);
        }

        private void SaveDatabase()
        {
            var serializer = new DataContractSerializer(typeof(List<UserAccount>));
            using XmlWriter writer = XmlWriter.Create(databasePath);
            serializer.WriteObject(writer, userDatabase);
        }
    }
}
