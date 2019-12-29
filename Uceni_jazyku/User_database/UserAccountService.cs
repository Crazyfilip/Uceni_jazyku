using Uceni_jazyku.Cycles;
using System.IO;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Uceni_jazyku.User_database
{

    /// <summary>
    /// User account class
    /// </summary>
    public class UserAccountService
    {
        private static UserAccountService instance;
        private List<UserAccount> userDatabase;
        private string databasePath = "./users/accounts.txt";

        public static UserAccountService GetInstance()
        {
            if (instance == null)
                instance = new UserAccountService();
            return instance;
        }

        /// <summary>
        /// Login user if user exists and return user's cycle in that case
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
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
            if (!File.Exists(databasePath))
                return false;
            using (StreamReader reader = new StreamReader(databasePath))
            {
                string credentials = username + "|" + password; // TODO lepsi prace s heslem
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    if (line == credentials)
                        return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Create user, unless user with same name already exists
        /// </summary>
        /// <param name="username">username</param>
        /// <param name="password">password</param>
        /// <returns>true if user is created, false when username is already used</returns>
        public bool CreateUser(string username, string password)
        {
            if (!File.Exists(databasePath))
            {
                using (StreamWriter writer = new StreamWriter(databasePath))
                {
                    writer.WriteLine(username + "|" + password); // TODO lepsi prace s password
                    return true;
                }
            }
            using (StreamReader reader = new StreamReader(databasePath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    if (line.Split(new char[] { '|' })[0] == username)
                        return false;
                }
            }
            using (StreamWriter writer = new StreamWriter(databasePath, append: true))
            {
                writer.WriteLine(username + "|" + password); // TODO lepsi prace s password
                return true;
            }
        }

        private void SaveDatabase()
        {
            XmlSerializer serializer = new XmlSerializer(userDatabase.GetType());
            using StreamWriter sw = new StreamWriter(databasePath);
            serializer.Serialize(sw, userDatabase);
        }
    }
}
