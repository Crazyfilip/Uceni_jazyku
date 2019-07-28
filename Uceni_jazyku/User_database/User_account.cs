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
    }
}
