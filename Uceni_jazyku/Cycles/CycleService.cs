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
    /// Cycle service is class for managing cycles.
    /// With cycles should be manipulated only via this service to keep their state consistent.
    /// Implemented as singleton
    /// </summary>
    public class CycleService
    {
        private static CycleService instance;

        private readonly ICycleRepository CycleRepository;

        private readonly IActiveCycleCache ActiveCycleCache;
        private CycleService(ICycleRepository database, IActiveCycleCache cache) {
            CycleRepository = database ?? new CycleRepository();
            ActiveCycleCache = cache ?? new ActiveCycleCache();
        }

        /// <summary>
        /// Get instance of service. 
        /// For database instance will be used the default one if service instance wasn't initialized yet.
        /// </summary>
        /// <returns>instance <c>CycleService</c></returns>
        public static CycleService GetInstance()
        {
            if (instance == null)
                instance = new CycleService(null, null);
            return instance;
        }

        /// <summary>
        /// Get instance of service. For database instance will be used the provided one if service instance wasn't initialized yet.
        /// </summary>
        /// <param name="database"></param>
        /// <returns>instance <c>CycleService</c></returns>
        public static CycleService GetInstance(ICycleRepository database)
        {
            if (instance == null)
                instance = new CycleService(database, null);
            return instance;
        }

        /// <summary>
        /// Method for deallocating singleton instance
        /// </summary>
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
            return ActiveCycleCache.IsCacheFilled();
        }

        /// <summary>
        /// Get active cycle when application starts
        /// </summary>
        /// <returns>active cycle</returns>
        public UserCycle GetActiveCycle()
        {
            return ActiveCycleCache.GetFromCache();
        }

        /// <summary>
        /// Get active cycle for given user
        /// when login or when active cycle was finished
        /// </summary>
        /// <param name="username">username</param>
        /// <returns>active cycle for user</returns>
        public UserCycle GetUserCycle(string username)
        {
            UserCycle result = CycleRepository.GetOldestUserInactiveCycle(username);
            if (result != null)
            {
                 return Activate(result);
            }
            else
            {
                return Activate(GetNewCycle(username));
            }
        }

        private string GenerateNewId()
        {
            return "cycle" + CycleRepository.GetCyclesCount();
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
            CycleRepository.PutCycle(newCycle);
            return newCycle;
        }

        /// <summary>
        /// Activate cycle, update it in a database and set active cycle cache
        /// If cycle was in state new then assign program to cycle
        /// </summary>
        /// <param name="cycle">cycle to activate</param>
        /// <returns>updated cycle</returns>
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
            CycleRepository.UpdateCycle(cycle);
            ActiveCycleCache.InsertToCache(cycle);
            return cycle;
        }

        /// <summary>
        /// Inactive cycle, update it in a database and clear active cycle cache
        /// </summary>
        /// <param name="cycle">cycle to inactivate</param>
        /// <returns>updated cycle</returns>
        public UserCycle Inactivate(UserCycle cycle)
        {
            cycle.Inactivate();
            CycleRepository.UpdateCycle(cycle);
            ActiveCycleCache.DropCache();
            return cycle;
        }

        /// <summary>
        /// Finish cycle, update it in a database and clear active cycle cache
        /// </summary>
        /// <param name="cycle">cycle to finish</param>
        public void Finish(UserCycle cycle)
        {
            cycle.Finish();
            CycleRepository.UpdateCycle(cycle);
            ActiveCycleCache.DropCache();
        }

        /// <summary>
        /// Register cycle which was created in a different way then via GetNewCycle(username)
        /// </summary>
        /// <param name="cycle">cycle to register</param>
        public void RegisterCycle(AbstractCycle cycle)
        {
            cycle.CycleID = GenerateNewId();
            CycleRepository.PutCycle(cycle);
        }

        /// <summary>
        /// Insert lesson to cycle and place removed lesson from that cycle to new incomplete cycle
        /// </summary>
        /// <param name="cycle">cycle where to put lesson to swap</param>
        /// <param name="item">lesson to swap</param>
        public void SwapLesson(UserCycle cycle, UserProgramItem item)
        {
            UserProgramItem swappedItem = cycle.SwapLesson(item);
            CycleRepository.UpdateCycle(cycle);
            if (cycle.State == UserCycleState.Active)
            {
                ActiveCycleCache.InsertToCache(cycle);
            }
            IncompleteUserCycle incompleteCycle = CycleRepository.GetUserIncompleteCycle(cycle.Username);
            if (incompleteCycle == null)
            {
                incompleteCycle = new IncompleteUserCycle(cycle.Username);
                RegisterCycle(incompleteCycle);
            }
            incompleteCycle.AddLesson(swappedItem);
            CycleRepository.UpdateCycle(incompleteCycle);
        }
    }
}
