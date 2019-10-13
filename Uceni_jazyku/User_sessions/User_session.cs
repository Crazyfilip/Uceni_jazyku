using System.IO;

namespace Uceni_jazyku.User_sessions
{
    /// <summary>
    /// User session class
    /// </summary>
    public class User_session
    {
        public string username { get; private set;}
        public int remaining_logins { get; private set; }

        private static string activeSessionPath = "./sessions/user-active/session.txt";
            
        /// <summary>
        /// Check if active session exists and return it via out parameter if does
        /// </summary>
        /// <param name="session"></param>
        /// <returns>true if active session exists</returns>
        public static bool ActiveSessionExists(out User_session session)
        {
            bool exists = File.Exists(activeSessionPath);
            if (exists)
                session = GetActiveSession();
            else
                session = null;
            return exists;
        }

        /// <summary>
        /// Get active session
        /// </summary>
        /// <returns>Active session</returns>
        private static User_session GetActiveSession()
        {
            User_session session = new User_session();
            using (StreamReader reader = new StreamReader(activeSessionPath))
            {
                session.username = reader.ReadLine();
                session.remaining_logins = int.Parse(reader.ReadLine());
            }
            return session;
        }

        /// <summary>
        /// Create user's session (will be created as active session
        /// </summary>
        /// <param name="username">username</param>
        /// <returns>User's session</returns>
        public static User_session CreateSession(string username)
        {
            User_session userSession = new User_session();
            userSession.username = username;
            userSession.remaining_logins = 3;
            SaveSession(userSession);
            return userSession;
        }

        /// <summary>
        /// Save user's session as active session
        /// </summary>
        /// <param name="userSession">session to save</param>
        private static void SaveSession(User_session userSession)
        {
            using (StreamWriter writer = new StreamWriter(activeSessionPath))
            {
                writer.WriteLine(userSession.username);
                writer.WriteLine(userSession.remaining_logins);
            }
        }
    }
}
