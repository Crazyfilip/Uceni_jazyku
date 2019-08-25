using Uceni_jazyku.User_sessions;
using System.IO;

namespace Uceni_jazyku.User_database
{
    public class User_account
    {
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

        private static bool VerifyUser(string username, string password)
        {
            if (!File.Exists("./users/accounts.txt"))
                return false;
            StreamReader reader = new StreamReader("./users/accounts.txt");
            string credentials = username + "|" + password;
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                if (line == credentials)
                    return true;
            }
            reader.Close();
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns>true if user is created, false when username is already used</returns>
        public static bool CreateUser(string username, string password)
        {
            if (!File.Exists("./users/accounts.txt"))
            {
                StreamWriter writer = File.CreateText("./users/accounts.txt");
                writer.WriteLine(username + "|" + password);
                writer.Flush();
                writer.Close();
                return true;
            }
            StreamReader reader = new StreamReader("./users/accounts.txt");
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                if (line.Split(new char[]{'|'})[0] == username)
                    return false;
            }
            reader.Close();
            StreamWriter writer2 = new StreamWriter("./users/accounts.txt", append: true);
            writer2.WriteLine(username + "|" + password);
            writer2.Flush();
            writer2.Close();
            return true;
        }
    }
}
