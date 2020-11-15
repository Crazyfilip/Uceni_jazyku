using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;
using Uceni_jazyku.Cycles.LanguageCycles;
using Uceni_jazyku.Cycles.Program;
using Uceni_jazyku.Cycles.UserCycles;

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

        private readonly string activeCycleCacheFile = "cycles/service/active-cycle.xml";
        private CycleService(CycleDatabase database) {
            CycleDatabase = database ?? new CycleDatabase();
            CycleDatabase.Load();
        }

        /// <summary>
        /// Get instance of service
        /// </summary>
        /// <returns>instance <c>CycleService</c></returns>
        public static CycleService GetInstance()
        {
            if (instance == null)
                instance = new CycleService(null);
            return instance;
        }

        public static CycleService GetInstance(CycleDatabase database)
        {
            if (instance == null)
                instance = new CycleService(database);
            return instance;
        }

        public static void DeallocateInstance()
        {
            instance = null;
        }

        /// <summary>
        /// test presence of cached active cycle
        /// </summary>
        /// <returns>true if there is cached active cycle</returns>
        public bool UserActiveCycleExists()
        {
            return File.Exists(activeCycleCacheFile);
        }

        private void cacheActiveCycle(UserCycle cycle)
        {
            var serializer = new DataContractSerializer(typeof(UserCycle));
            using XmlWriter writer = XmlWriter.Create(activeCycleCacheFile);
            serializer.WriteObject(writer, cycle);
        }

        private void clearCachedActiveCycle()
        {
            File.Delete(activeCycleCacheFile);
        }

        /// <summary>
        /// Get active cycle for given user
        /// when login or when active cycle was finished
        /// </summary>
        /// <param name="username">username</param>
        /// <returns>active cycle for user</returns>
        public UserCycle GetUserCycle(string username)
        {
            UserCycle result = CycleDatabase.GetOldestUserInactiveCycle(username);
            if (result != null)
            {
                 return Activate(result);
            }
            else
            {
                return Activate(GetNewCycle(username));
            }
        }

        /// <summary>
        /// Get active cycle when application starts
        /// </summary>
        /// <returns>active cycle</returns>
        public UserCycle GetActiveCycle()
        {
            var serializer = new DataContractSerializer(typeof(UserCycle));
            using XmlReader reader = XmlReader.Create(activeCycleCacheFile);
            return (UserCycle)serializer.ReadObject(reader);
        }

        private string GenerateNewId()
        {
            return "cycle" + CycleDatabase.GetCyclesCount();
        }
        /// <summary>
        /// Create new user cycle
        /// Register cycle and assign to it cycleID
        /// </summary>
        /// <returns>instance of UserNewCycle</returns>
        public UserCycle GetNewCycle(string username)
        {
            UserCycle newCycle = new UserCycle
            {
                CycleID = GenerateNewId()
            }; 
            newCycle.AssignUser(username);
            CycleDatabase.PutCycle(newCycle);
            return newCycle;
        }

        public UserCycle Activate(UserCycle cycle)
        {
            if (cycle.State == UserCycles.UserCycleState.New)
            {
                // TODO assign program from planner based on user model
                LanguageCycle example = LanguageCycle.LanguageCycleExample();
                RegisterCycle(example);
                cycle.AssignProgram(new List<Program.UserProgramItem>() { new Program.UserProgramItem(example.CycleID, example.PlanNext())});
            }
            cycle.Activate();
            CycleDatabase.UpdateCycle(cycle);
            cacheActiveCycle(cycle);
            return cycle;
        }

        public UserCycle Inactivate(UserCycle cycle)
        {
            cycle.Inactivate();
            CycleDatabase.UpdateCycle(cycle);
            clearCachedActiveCycle();
            return cycle;
        }

        public void RegisterCycle(AbstractCycle cycle)
        {
            cycle.CycleID = GenerateNewId();
            CycleDatabase.PutCycle(cycle);
        }

        public void SwapLesson(UserCycle cycle, UserProgramItem item)
        {
            UserProgramItem swappedItem = cycle.SwapLesson(item);
            CycleDatabase.UpdateCycle(cycle);
            if (cycle.State == UserCycleState.Active)
            {
                clearCachedActiveCycle();
                cacheActiveCycle(cycle);
            }
            IncompleteUserCycle incompleteCycle = CycleDatabase.GetUserIncompleteCycle(cycle.Username);
            if (incompleteCycle == null)
            {
                incompleteCycle = new IncompleteUserCycle(cycle.Username);
                CycleDatabase.PutCycle(incompleteCycle);
            }
            incompleteCycle.AddLesson(swappedItem);
            CycleDatabase.UpdateCycle(incompleteCycle);
        }
    }
}
