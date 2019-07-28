using System.IO;

namespace Uceni_jazyku.User_sessions
{
    public class User_session
    {
        public string username { get; private set;}
        public int remaining_logins { get; private set; }

        private static string activeSessionPath = "./sessions/user-active/session.txt";
            
        public static bool UserSessionExists(out User_session session)
        {
            bool exists = File.Exists(activeSessionPath);
            if (exists)
                session = GetActiveSession();
            else
                session = null;
            return exists;
        }

        private static User_session GetActiveSession()
        {
            StreamReader reader = new StreamReader(activeSessionPath);
            User_session session = new User_session();
            session.username = reader.ReadLine();
            session.remaining_logins = int.Parse(reader.ReadLine());
            reader.Close();
            return session;
        }

        public static User_session CreateSession(string username)
        {
            User_session userSession = new User_session();
            userSession.username = username;
            userSession.remaining_logins = 3;
            SaveSession(userSession);
            return userSession;
        }

        private static void SaveSession(User_session userSession)
        {
            StreamWriter writer = new StreamWriter(activeSessionPath);
            writer.WriteLine(userSession.username);
            writer.WriteLine(userSession.remaining_logins);
            writer.Flush();
            writer.Close();
        }
    }
}
