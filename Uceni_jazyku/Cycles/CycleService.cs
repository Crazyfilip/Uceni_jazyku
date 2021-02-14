using log4net;
using System;
using System.Collections.Generic;
using Uceni_jazyku.Cycles.LanguageCycles;
using Uceni_jazyku.Cycles.Program;
using Uceni_jazyku.Cycles.UserCycles;
using Uceni_jazyku.Planner;

namespace Uceni_jazyku.Cycles
{
    /// <summary>
    /// Cycle service is class for managing cycles.
    /// With cycles should be manipulated only via this service to keep their state consistent.
    /// Implemented as singleton
    /// </summary>
    public class CycleService
    {
        private static ILog log = LogManager.GetLogger(typeof(CycleService));

        private static CycleService instance;

        private readonly ICycleRepository CycleRepository;

        private readonly IActiveCycleCache ActiveCycleCache;

        private readonly IProgramPlanner ProgramPlanner;

        private readonly ICycleFactory CycleFactory;

        public CycleService() : this(null, null, null, null){}

        private CycleService(ICycleRepository database, IActiveCycleCache cache, IProgramPlanner planner, ICycleFactory cycleFactory) {
            CycleRepository = database ?? new CycleRepository();
            ActiveCycleCache = cache ?? new ActiveCycleCache();
            CycleFactory = cycleFactory ?? new CycleFactory();
            // TODO replace mock planner by real implementation
            ProgramPlanner = planner ?? new MockProgramPlanner();
        }

        /// <summary>
        /// Get instance of service. 
        /// For database instance will be used the default one if service instance wasn't initialized yet.
        /// </summary>
        /// <returns>instance <c>CycleService</c></returns>
        public static CycleService GetInstance()
        {
            if (instance == null)
                instance = new CycleService(null, null, null, null);
            return instance;
        }

        /// <summary>
        /// Get instance of service. For database instance will be used the provided one if service instance wasn't initialized yet.
        /// </summary>
        /// <param name="database">database</param>
        /// <param name="cache">cache</param>
        /// <param name="planner">planner</param>
        /// <param name="cycleFactory">cycle factory</param>
        /// <returns>instance <c>CycleService</c></returns>
        public static CycleService GetInstance(ICycleRepository database, IActiveCycleCache cache, IProgramPlanner planner, ICycleFactory cycleFactory)
        {
            if (instance == null)
                instance = new CycleService(database, cache, planner, cycleFactory);
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
            log.Debug($"Looking if there is existing inactive cycle for user {username}");
            UserCycle result = CycleRepository.GetOldestUserInactiveCycle(username);
            if (result != null)
            {
                log.Debug($"Obtained {result.CycleID}");
                 return Activate(result);
            }
            else
            {
                log.Debug($"No cycle found, new must be created and activated");
                return Activate(GetNewCycle(username));
            }
        }

        /// <summary>
        /// Create new user cycle
        /// Register cycle and assign to it cycleID
        /// </summary>
        /// <returns>instance of UserNewCycle</returns>
        public UserCycle GetNewCycle(string username)
        {
            log.Info($"Creating new cycle for user {username}");
            UserCycle newCycle = CycleFactory.CreateCycle(username);
            CycleRepository.PutCycle(newCycle);
            log.Debug($"New cycle created with id {newCycle.CycleID}");
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
                log.Debug("Obtaining cycle program");
                List<UserProgramItem> program = ProgramPlanner.getNextUserCycleProgram(cycle.Username);
                log.Info($"Assigning program to cycle {cycle.CycleID}");
                cycle.AssignProgram(program);
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
        /// Insert lesson to cycle and place removed lesson from that cycle to new incomplete cycle
        /// </summary>
        /// <param name="cycle">cycle where to put lesson to swap</param>
        /// <param name="item">lesson to swap</param>
        public void SwapLesson(UserCycle cycle, UserProgramItem item)
        {
            log.Info($"Swapping lesson {item.LessonRef.Lesson} to cycle {cycle.CycleID}"); // TODO UserProgramItem should have also some id
            UserProgramItem swappedItem = cycle.SwapLesson(item);
            CycleRepository.UpdateCycle(cycle);
            if (cycle.State == UserCycleState.Active)
            {
                ActiveCycleCache.InsertToCache(cycle);
            }
            IncompleteUserCycle incompleteCycle = GetIncompleteUserCycle(cycle.Username);
            log.Info($"Placing swapped lesson {swappedItem.LessonRef.Lesson} to incomplete cycle {incompleteCycle.CycleID}");
            incompleteCycle.AddLesson(swappedItem);
            CycleRepository.UpdateCycle(incompleteCycle);
        }

        /// <summary>
        /// Get from planner next lesson for user.
        /// It will put lesson to incomplete cycle and return its description.
        /// </summary>
        /// <param name="username">username</param>
        /// <returns>Lesson description</returns>
        public string GetNextLesson(string username)
        {
            log.Info("Getting next planned lesson");
            UserProgramItem item = ProgramPlanner.getNextLanguageLesson(username);
            IncompleteUserCycle incompleteCycle = GetIncompleteUserCycle(username);
            log.Debug($"Placing lesson {item.LessonRef.Lesson} to cycle {incompleteCycle.CycleID}");
            incompleteCycle.AddLesson(item);
            CycleRepository.UpdateCycle(incompleteCycle);
            return item.LessonRef.Lesson;
        }

        private IncompleteUserCycle GetIncompleteUserCycle(string username)
        {
            log.Debug($"Looking if there is incomplete user cycle for user {username}");
            IncompleteUserCycle incompleteCycle = CycleRepository.GetUserIncompleteCycle(username);
            if (incompleteCycle == null)
            {
                log.Debug("No incomplete user cycle found creating new");
                incompleteCycle = CycleFactory.CreateIncompleteCycle(username, 0); // TODO set limit properly
                CycleRepository.PutCycle(incompleteCycle);
            }
            return incompleteCycle;
        }
    }
}
