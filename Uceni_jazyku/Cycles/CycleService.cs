using log4net;
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
        private static readonly ILog log = LogManager.GetLogger(typeof(CycleService));

        private static CycleService instance;

        private readonly ICycleRepository CycleRepository;

        private readonly IActiveCycleCache ActiveCycleCache;

        public CycleService() : this(null, null){}

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
        public static CycleService GetInstance(ICycleRepository database, IActiveCycleCache cache)
        {
            if (instance == null)
                instance = new CycleService(database, cache);
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
            log.Info("Looking into cache if there is user active cycle present");
            return ActiveCycleCache.IsCacheFilled();
        }

        /// <summary>
        /// Get active cycle when application starts
        /// </summary>
        /// <returns>active cycle</returns>
        public UserCycle GetActiveCycle()
        {
            log.Info("Retriving user active cycle from cache");
            return ActiveCycleCache.GetFromCache();
        }

        /// <summary>
        /// Get active cycle for given user
        /// when login or when active cycle was finished
        /// </summary>
        /// <param name="username">username</param>
        /// <returns>active cycle for user</returns>
        public virtual UserCycle GetUserCycle(string username)
        {
            log.Info($"Getting cycle for user {username}");
            log.Trace($"Looking if there is existing inactive cycle for user {username}");
            UserCycle result = CycleRepository.GetOldestUserInactiveCycle(username);
            if (result != null)
            {
                log.Trace($"Obtained {result.CycleID}");
                 return Activate(result);
            }
            else
            {
                log.Trace($"No cycle found, new must be created and activated");
                return Activate(GetNewCycle(username));
            }
        }

        private string GenerateNewId()
        {
            log.Trace("Generating cycleID");
            return "cycle" + CycleRepository.GetCyclesCount();
        }
        /// <summary>
        /// Create new user cycle
        /// Register cycle and assign to it cycleID
        /// </summary>
        /// <returns>instance of UserNewCycle</returns>
        public UserCycle GetNewCycle(string username)
        {
            log.Info($"Creating new cycle for user {username}");
            UserCycle newCycle = new UserCycle
            {
                CycleID = GenerateNewId()
            }; 
            newCycle.AssignUser(username);
            CycleRepository.PutCycle(newCycle);
            log.Trace($"New cycle created with id {newCycle.CycleID}");
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
            log.Info($"Activating cycle {cycle.CycleID}");
            if (cycle.State == UserCycleState.New)
            {
                log.Info($"Assigning program to cycle {cycle.CycleID}");
                // TODO assign program from planner based on user model
                LanguageCycle example = LanguageCycle.LanguageCycleExample();
                RegisterCycle(example);
                cycle.AssignProgram(new List<Program.UserProgramItem>() { new Program.UserProgramItem(example.CycleID, example.PlanNext())});
            }
            try
            {
                cycle.Activate();
                CycleRepository.UpdateCycle(cycle);
                ActiveCycleCache.InsertToCache(cycle);
            }
            catch (IncorrectCycleStateException e)
            {
                log.Warn($"Cycle {cycle.CycleID} wasn't activated", e);
            }
            return cycle;
        }

        /// <summary>
        /// Inactive cycle, update it in a database and clear active cycle cache
        /// </summary>
        /// <param name="cycle">cycle to inactivate</param>
        /// <returns>updated cycle</returns>
        public UserCycle Inactivate(UserCycle cycle)
        {
            log.Info($"Inactivating cycle {cycle.CycleID}");
            try
            {
                cycle.Inactivate();
                CycleRepository.UpdateCycle(cycle);
                ActiveCycleCache.DropCache();
            } 
            catch (IncorrectCycleStateException e)
            {
                log.Warn($"Cycle {cycle.CycleID} wasn't inactivated", e);
            }
            return cycle;
        }

        /// <summary>
        /// Finish cycle, update it in a database and clear active cycle cache
        /// </summary>
        /// <param name="cycle">cycle to finish</param>
        public void Finish(UserCycle cycle)
        {
            log.Info($"Finishing cycle {cycle.CycleID}");
            try
            {
                cycle.Finish();
                CycleRepository.UpdateCycle(cycle);
                ActiveCycleCache.DropCache();
            }
            catch (IncorrectCycleStateException e)
            {
                log.Warn($"Cycle {cycle.CycleID} wasn't finished", e);
            }
            catch (Exception e)
            {
                log.Warn($"Cycle {cycle.CycleID} wasn't finished", e);
            }
        }

        /// <summary>
        /// Register cycle which was created in a different way then via GetNewCycle(username)
        /// </summary>
        /// <param name="cycle">cycle to register</param>
        public void RegisterCycle(AbstractCycle cycle)
        {
            log.Info("Registering cycle");
            cycle.CycleID = GenerateNewId();
            log.Trace($"Registered cycle got id {cycle.CycleID}");
            CycleRepository.PutCycle(cycle);
        }

        /// <summary>
        /// Insert lesson to cycle and place removed lesson from that cycle to new incomplete cycle
        /// </summary>
        /// <param name="cycle">cycle where to put lesson to swap</param>
        /// <param name="item">lesson to swap</param>
        public void SwapLesson(UserCycle cycle, UserProgramItem item)
        {
            log.Info($"Swapping lesson in cycle {cycle.CycleID}"); // TODO UserProgramItem should have also some id
            UserProgramItem swappedItem = cycle.SwapLesson(item);
            CycleRepository.UpdateCycle(cycle);
            if (cycle.State == UserCycleState.Active)
            {
                ActiveCycleCache.InsertToCache(cycle);
            }
            log.Info($"Placing swapped lesson to incomplete cycle");
            log.Trace($"Looking if there is incomplete user cycle for user {cycle.Username}");
            IncompleteUserCycle incompleteCycle = CycleRepository.GetUserIncompleteCycle(cycle.Username);
            if (incompleteCycle == null)
            {
                log.Trace("No incomplete user cycle found creating new");
                incompleteCycle = new IncompleteUserCycle(cycle.Username);
                RegisterCycle(incompleteCycle);
            }
            incompleteCycle.AddLesson(swappedItem);
            CycleRepository.UpdateCycle(incompleteCycle);
        }
    }
}
