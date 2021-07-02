using log4net;
using System;
using System.Collections.Generic;
using Uceni_jazyku.Cycles.Program;
using Uceni_jazyku.Cycles.UserCycles;
using Uceni_jazyku.Language;
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

        public LanguageCourse ActiveCourse { get; private set; }

        private readonly ICycleRepository CycleRepository;

        private readonly IProgramPlanner ProgramPlanner;

        private readonly ICycleFactory CycleFactory;

        #region Initialization
        public CycleService() : this(null, null, null){}

        private CycleService(ICycleRepository database, IProgramPlanner planner, ICycleFactory cycleFactory) {
            CycleRepository = database ?? new CycleRepository();
            CycleFactory = cycleFactory ?? new CycleFactory();
            ProgramPlanner = planner ?? new ProgramPlanner();
        }

        /// <summary>
        /// Get instance of service. 
        /// For database instance will be used the default one if service instance wasn't initialized yet.
        /// </summary>
        /// <returns>instance <c>CycleService</c></returns>
        public static CycleService GetInstance()
        {
            if (instance == null)
                instance = new CycleService(null, null, null);
            return instance;
        }

        /// <summary>
        /// Get instance of service. For database instance will be used the provided one if service instance wasn't initialized yet.
        /// </summary>
        /// <param name="database">database</param>
        /// <param name="planner">planner</param>
        /// <param name="cycleFactory">cycle factory</param>
        /// <returns>instance <c>CycleService</c></returns>
        public static CycleService GetInstance(ICycleRepository database, IProgramPlanner planner, ICycleFactory cycleFactory)
        {
            if (instance == null)
                instance = new CycleService(database, planner, cycleFactory);
            return instance;
        }

        /// <summary>
        /// Method for deallocating singleton instance
        /// </summary>
        public static void DeallocateInstance()
        {
            instance = null;
        }

        #endregion

        #region Creating and updating user cycles

        /// <summary>
        /// Get active cycle for given user
        /// when login or when active cycle was finished
        /// </summary>
        /// <param name="username">username</param>
        /// <returns>active cycle for user</returns>
        public virtual UserCycle GetNextCycle(string username)
        {
            log.Info($"Getting cycle for user {username}");
            UserCycle result = CycleRepository.GetActiveCycle(username, ActiveCourse.Id);
            if (result != null) return result;

            log.Debug($"Looking if there is existing inactive cycle for user {username}");
            result = CycleRepository.GetOldestUserInactiveCycle(username, ActiveCourse.Id);
            if (result != null)
            {
                log.Debug($"Obtained {result.Id}");
                 return Activate(result);
            }
            else
            {
                log.Debug($"No cycle found, new must be created");
                return GetNewCycle(username);
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
            List<UserProgramItem> program = ProgramPlanner.GetNextUserCycleProgram(username);
            UserCycle newCycle = CycleFactory.CreateCycle(username, ActiveCourse.Id, program);
            CycleRepository.Create(newCycle);
            log.Debug($"New cycle created with id {newCycle.Id}");
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
            log.Info($"Activating cycle {cycle.Id}");
            try
            {
                cycle.Activate();
                CycleRepository.Update(cycle);
            }
            catch (IncorrectCycleStateException e)
            {
                log.Warn($"Cycle {cycle.Id} wasn't activated", e);
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
            log.Info($"Inactivating cycle {cycle.Id}");
            try
            {
                cycle.Inactivate();
                CycleRepository.Update(cycle);
            } 
            catch (IncorrectCycleStateException e)
            {
                log.Warn($"Cycle {cycle.Id} wasn't inactivated", e);
            }
            return cycle;
        }

        /// <summary>
        /// Finish cycle, update it in a database and clear active cycle cache
        /// </summary>
        /// <param name="cycle">cycle to finish</param>
        public void Finish(UserCycle cycle)
        {
            log.Info($"Finishing cycle {cycle.Id}");
            try
            {
                cycle.Finish();
                CycleRepository.Update(cycle);
            }
            catch (IncorrectCycleStateException e)
            {
                log.Warn($"Cycle {cycle.Id} wasn't finished", e);
            }
            catch (Exception e)
            {
                log.Warn($"Cycle {cycle.Id} wasn't finished", e);
            }
        }

        /// <summary>
        /// Update cycle and switch to new cycle if all lesson are finished
        /// </summary>
        /// <param name="cycle">Cycle to update</param>
        /// <returns>current cycle or new one</returns>
        public UserCycle Update(UserCycle cycle)
        {
            cycle.Update();
            if (cycle.AreAllFinished())
            {
                Finish(cycle);
                return GetNextCycle(cycle.Username);
            } 
            else
            {
                CycleRepository.Update(cycle);
                return cycle;
            }
        }
        #endregion

        #region Others methods

        // Only for testing 
        public void SetActiveCourse(LanguageCourse languageCourse)
        {
            ActiveCourse = languageCourse;
        }

        /// <summary>
        /// Set as active language course provided course, reset planner and reset active cycle if needed
        /// </summary>
        /// <param name="username">username</param>
        /// <param name="languageCourse">LanguageCourse</param>
        public virtual void SetActiveCourse(string username, LanguageCourse languageCourse)
        {
            ActiveCourse = languageCourse;
            ProgramPlanner.SetPlanner(languageCourse, username);
        }
        
        /// <summary>
        /// Insert lesson to cycle and place removed lesson from that cycle to new incomplete cycle
        /// </summary>
        /// <param name="cycle">cycle where to put lesson to swap</param>
        /// <param name="item">lesson to swap</param>
        public void SwapLesson(UserCycle cycle, UserProgramItem item)
        {
            log.Info($"Swapping lesson {item.LessonRef.Lesson} to cycle {cycle.Id}"); // TODO UserProgramItem should have also some id
            UserProgramItem swappedItem = cycle.SwapLesson(item);
            CycleRepository.Update(cycle);
            IncompleteUserCycle incompleteCycle = GetIncompleteUserCycle(cycle.Username);
            log.Info($"Placing swapped lesson {swappedItem.LessonRef.Lesson} to incomplete cycle {incompleteCycle.Id}");
            incompleteCycle.AddLesson(swappedItem);
            CycleRepository.Update(incompleteCycle);
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
            UserProgramItem item = ProgramPlanner.GetNextLanguageLesson(username);
            IncompleteUserCycle incompleteCycle = GetIncompleteUserCycle(username);
            log.Debug($"Placing lesson {item.LessonRef.Lesson} to cycle {incompleteCycle.Id}");
            incompleteCycle.AddLesson(item);
            CycleRepository.Update(incompleteCycle);
            return item.LessonRef.Lesson;
        }

        /// <summary>
        /// Get lesson from LanguageTopic chosen by user
        /// </summary>
        /// <param name="username">username</param>
        /// <param name="topicId">language topic id</param>
        /// <returns>lesson</returns>
        // TODO add tests when implemented call from UI
        public string GetNextLesson(string username, string topicId)
        {
            log.Info($"Getting next lesson from topic {topicId}");
            UserProgramItem item = ProgramPlanner.GetNextLanguageLesson(username, topicId);
            // TODO swap lesson to active cycle
            return item.LessonRef.Lesson;
        }

        private IncompleteUserCycle GetIncompleteUserCycle(string username)
        {
            log.Debug($"Looking if there is incomplete user cycle for user {username}");
            IncompleteUserCycle incompleteCycle = CycleRepository.GetUserIncompleteCycle(username, ActiveCourse.Id);
            if (incompleteCycle == null)
            {
                log.Debug("No incomplete user cycle found creating new");
                incompleteCycle = CycleFactory.CreateIncompleteCycle(username, ActiveCourse.Id, 0); // TODO set limit properly
                CycleRepository.Create(incompleteCycle);
            }
            return incompleteCycle;
        }

        /// <summary>
        /// Retrieve all unfinished planned lesson for user
        /// </summary>
        /// <param name="username">user's name</param>
        /// <returns>List of lessons</returns>
        public List<UserProgramItem> GetPlannedUnfinishedLessons(string username)
        {
            log.Info($"Getting unfinished planned lessons for user {username}");
            List<UserCycle> userCycles = CycleRepository.GetNotFinishedCycles(username, ActiveCourse.Id);
            List<UserProgramItem> result = new();
            userCycles
                .ForEach(x => x.UserProgramItems
                    .ForEach(y =>
                    {
                        if (!y.LessonRef.Finished)
                            result.Add(y);
                    }));
            return result;
        }
        #endregion
    }
}
