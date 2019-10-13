using Uceni_jazyku.User_sessions;
using System.IO;

namespace Uceni_jazyku.User_database
{
    /// <summary>
    /// User account class
    /// </summary>
    public class User_account
    {
        /// <summary>
        /// Login user if user exists and return user's session in that case
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns>User's session or null</returns>
        public static User_session Login(string username, string password)
        {
            if (VerifyUser(username, password))
            {
                return User_session.CreateSession(username);
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
        /// <returns></returns>
        private static bool VerifyUser(string username, string password)
        {
            if (!File.Exists("./users/accounts.txt"))
                return false;
            using (StreamReader reader = new StreamReader("./users/accounts.txt"))
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
        public static bool CreateUser(string username, string password)
        {
            if (!File.Exists("./users/accounts.txt"))
            {
                using (StreamWriter writer = new StreamWriter("./users/accounts.txt"))
                {
                    writer.WriteLine(username + "|" + password); // TODO lepsi prace s password
                    return true;
                }
            }
            using (StreamReader reader = new StreamReader("./users/accounts.txt"))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    if (line.Split(new char[] { '|' })[0] == username)
                        return false;
                }
            }
            using (StreamWriter writer = new StreamWriter("./users/accounts.txt", append: true))
            {
                writer.WriteLine(username + "|" + password); // TODO lepsi prace s password
                return true;
            }
        }
    }
}
