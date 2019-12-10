using System;
using System.Collections.Generic;
using System.IO;

namespace Uceni_jazyku.Cycles
{
    public class CycleService
    {
        private static CycleService instance;

        private readonly CycleDatabase database;
        private readonly CycleFactory factory;
        private CycleService() {
            database = new CycleDatabase();
            database.Load();
            factory = new CycleFactory();
        }

        public static CycleService GetInstance()
        {
            if (instance == null)
                instance = new CycleService();
            return instance;
        }

        private static readonly string activeSessionPath = "./sessions/user-active/session.txt";

        public bool ActiveUserSessionExists()
        {
            return File.Exists(activeSessionPath);
        }

        public UserCycle GetActiveSession()
        {
            UserCycle userSession = new UserActiveCycle();
            return (UserCycle)userSession.GetSession();
        }

        private void RegisterSession(AbstractCycle session)
        {
            session.CycleID = GenerateNewId();
            database.PutSession(session);
        }

        public AbstractCycle CreateSession(SessionType type, string name, int numberOfEvents ) // TODO add parameters for session factory
        {
            AbstractCycle session = factory.GetSession(type, name, numberOfEvents);
            RegisterSession(session);
            return session;
        }

        private string GenerateNewId()
        {
            return "session" + database.sessionsDatabase.Count;
        }
    }
}
