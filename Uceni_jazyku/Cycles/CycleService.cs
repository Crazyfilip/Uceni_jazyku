using System;
using System.Collections.Generic;
using System.IO;

namespace Uceni_jazyku.Cycles
{
    /// <summary>
    /// Cycle service class which handles creation, lifecycle and operations with cycles.
    /// Implemented as singleton
    /// </summary>
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

        /// <summary>
        /// Get instance of service
        /// </summary>
        /// <returns>instance <c>CycleService</c></returns>
        public static CycleService GetInstance()
        {
            if (instance == null)
                instance = new CycleService();
            return instance;
        }

        /// <summary>
        /// test presence of UserActiveCycle
        /// </summary>
        /// <returns>true if there is ActiveUserCycle present</returns>
        public bool UserActiveCycleExists()
        {
            return UserActiveCycle.CycleExists();
        }

        /// <summary>
        /// Getter of UserActiveCycle;
        /// </summary>
        /// <returns>active cycle</returns>
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

        /// <summary>
        /// Create cycle based on parameters
        /// </summary>
        /// <param name="type">cycle type</param>
        /// <param name="name">cycle name</param>
        /// <param name="numberOfEvents">number of events in cycle</param>
        /// <returns>cycle instance</returns>
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
