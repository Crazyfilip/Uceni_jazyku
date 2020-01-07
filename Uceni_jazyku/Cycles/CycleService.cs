using System;
using System.Collections.Generic;
using System.IO;

namespace Uceni_jazyku.Cycles
{
    /// <summary>
    /// Cycle service class which handles lifecycle and operations with cycles.
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
            UserCycle userCycle = new UserActiveCycle();
            return (UserCycle)userCycle.GetCycle();
        }

        private void RegisterCycle(AbstractCycle cycle)
        {
            cycle.CycleID = GenerateNewId();
            CycleDatabase.PutCycle(cycle);
            cycle.SaveCycle();
        }

        private string GenerateNewId()
        {
            return "cycle" + CycleDatabase.getCyclesCount();
        }

        /// <summary>
        /// Create new user cycle
        /// Register cycle and assign to it cycleID
        /// </summary>
        /// <returns>instance of UserNewCycle</returns>
        public UserNewCycle GetNewCycle(string username)
        {
            UserNewCycle newCycle = (UserNewCycle)factory.CreateCycle(CycleType.UserNewCycle, username);
            newCycle.CycleID = GenerateNewId();
            CycleDatabase.PutCycle(newCycle);
            return newCycle;
        }

        /// <summary>
        /// Convert UserNewCycle to UserActiveCycle.
        /// </summary>
        /// <param name="originCycle">new cycle which will be activated</param>
        /// <returns>UserActiveCycle</returns>
        // TODO add cycle program to cycle
        public UserActiveCycle Activate(UserNewCycle originCycle)
        {
            // TODO from user setting get length of cycle
            UserActiveCycle activeCycle = (UserActiveCycle)factory.CreateCycle(CycleType.UserActiveCycle, originCycle.Username, 3); // TODO not use 3 but length of cycle
            activeCycle.CycleID = originCycle.CycleID;
            // TODO assign cycle plan
            return activeCycle;
        }

        /// <summary>
        /// Convert UserInactiveCycle to UserActiveCycle
        /// </summary>
        /// <param name="originCycle">inactive cycle which will be activated</param>
        /// <returns>UserActiveCycle</returns>
        public UserActiveCycle Activate(UserInactiveCycle originCycle)
        {
            UserActiveCycle activeCycle = (UserActiveCycle)factory.CreateCycle(CycleType.UserActiveCycle, originCycle.Username, originCycle.RemainingEvents);
            activeCycle.CycleID = originCycle.CycleID;
            return activeCycle;
        }

        /// <summary>
        /// Convert UserActiveCycle to UserInactiveCycle
        /// </summary>
        /// <param name="originCycle">active cycle which will be inactivated</param>
        /// <returns>UserInactiveCycle</returns>
        public UserInactiveCycle Inactive(UserActiveCycle originCycle)
        {
            UserInactiveCycle inactiveCycle = (UserInactiveCycle)factory.CreateCycle(CycleType.UserInactiveCycle, originCycle.Username, originCycle.RemainingEvents);
            inactiveCycle.CycleID = originCycle.CycleID;
            return inactiveCycle;
        }

        /// <summary>
        /// Convert UserActiveCycle to UserFinishedCycle
        /// </summary>
        /// <param name="originCycle">active cycle which will be finished</param>
        /// <returns>UserFinishedCycle</returns>
        public UserFinishedCycle Finish(UserActiveCycle originCycle)
        {
            UserFinishedCycle finishedCycle = (UserFinishedCycle)factory.CreateCycle(CycleType.UserFinishedCycle, originCycle.Username);
            finishedCycle.CycleID = originCycle.CycleID;
            return finishedCycle;
        }
    }
}
