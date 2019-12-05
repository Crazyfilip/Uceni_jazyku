using System;
using System.Collections.Generic;
using System.IO;

namespace Uceni_jazyku.User_sessions
{
    public class SessionService
    {
        private static SessionService instance;

        private readonly SessionDatabase database;
        private readonly SessionFactory factory;
        private SessionService() {
            database = new SessionDatabase();
            database.Load();
            factory = new SessionFactory();
        }

        public static SessionService GetInstance()
        {
            if (instance == null)
                instance = new SessionService();
            return instance;
        }

        private static readonly string activeSessionPath = "./sessions/user-active/session.txt";

        public bool ActiveUserSessionExists()
        {
            return File.Exists(activeSessionPath);
        }

        public UserSession GetActiveSession()
        {
            UserSession userSession = new ActiveUserSession();
            return (UserSession)userSession.GetSession();
        }

        private void RegisterSession(AbstractSession session)
        {
            session.SessionId = GenerateNewId();
            database.PutSession(session);
        }

        public AbstractSession CreateSession(SessionType type, string name, int numberOfEvents ) // TODO add parameters for session factory
        {
            AbstractSession session = factory.GetSession(type, name, numberOfEvents);
            RegisterSession(session);
            return session;
        }

        private string GenerateNewId()
        {
            return "session" + database.sessionsDatabase.Count;
        }
    }
}
