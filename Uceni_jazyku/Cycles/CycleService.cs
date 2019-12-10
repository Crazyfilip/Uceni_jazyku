using System;
using System.Collections.Generic;
using System.IO;

namespace Uceni_jazyku.Cycles
{
    public class CycleService
    {
        private static CycleService instance;

        private readonly CycleDatabase CycleDatabase;
        private readonly CycleFactory factory;
        private CycleService() {
            CycleDatabase = new CycleDatabase();
            CycleDatabase.Load();
            factory = new CycleFactory();
        }

        public static CycleService GetInstance()
        {
            if (instance == null)
                instance = new CycleService();
            return instance;
        }

        private static readonly string activeSessionPath = "./sessions/user-active/session.txt";

        public bool ActiveUserCycleExists()
        {
            return File.Exists(activeSessionPath);
        }

        public UserCycle GetActiveCycle()
        {
            UserCycle userSession = new UserActiveCycle();
            return (UserCycle)userSession.GetCycle();
        }

        private void RegisterCycle(AbstractCycle cycle)
        {
            cycle.CycleID = GenerateNewId();
            CycleDatabase.PutSession(cycle);
        }

        public AbstractCycle CreateCycle(CycleType type, string name, int numberOfEvents )
        {
            AbstractCycle cycle = factory.CreateCycle(type, name, numberOfEvents);
            RegisterCycle(cycle);
            return cycle;
        }

        private string GenerateNewId()
        {
            return "cycle" + CycleDatabase.database.Count;
        }
    }
}
