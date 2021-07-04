using log4net;
using System.Collections.Generic;
using System.Linq;
using LanguageLearning.Common;
using LanguageLearning.Cycles.UserCycles;

namespace LanguageLearning.Cycles
{
    /// <inheritdoc/>
    /// <summary>
    /// Repository represented as List which is saved to/loaded from a file
    /// </summary>
    public class CycleRepository : AbstractRepository<UserCycle>, ICycleRepository
    {

        private static ILog log = LogManager.GetLogger(typeof(CycleRepository));

        public CycleRepository() : this(null) {}

        public CycleRepository(Serializer<UserCycle> serializer)
        {
            this.serializer = serializer ?? new Serializer<UserCycle>() { Filepath = "./cycles/service/database.xml" };
        }

        /// <inheritdoc/>
        public UserCycle GetActiveCycle(string username, string courseID)
        {
            log.Info($"Getting active cycle for user {username}");
            data = serializer.Load();
            return data
                .Where(x => x.State == UserCycleState.Active && x.Username == username && x.CourseID == courseID)
                .FirstOrDefault();
        }

        /// <inheritdoc/>
        public UserCycle GetOldestUserInactiveCycle(string username, string courseId)
        {
            log.Info($"Getting oldest inactive cycle for user {username}");
            data = serializer.Load();
            List<UserCycle> queryResult = data
                .Where(x => x.Username == username && x.CourseID == courseId && x.State == UserCycleState.Inactive)
                .ToList();
            queryResult.Sort((x, y) => x.DateCreated.CompareTo(y.DateCreated));
            return queryResult.FirstOrDefault();
        }

        /// <inheritdoc/>
        public IncompleteUserCycle GetUserIncompleteCycle(string username, string courseId)
        {
            log.Info($"Getting incomplete cycle for user {username}");
            data = serializer.Load();
            return data
                .Where(x => x.Username == username && x.CourseID == courseId && x is IncompleteUserCycle)
                .Cast<IncompleteUserCycle>()
                .FirstOrDefault();
        }

        /// <inheritdoc/>
        public List<UserCycle> GetNotFinishedCycles(string username, string courseId)
        {
            log.Info($"Getting cycles which are not in finished state for user {username}");
            data = serializer.Load();
            List<UserCycle> result = data
                .Where(x => x.Username == username && x.CourseID == courseId && x.State != UserCycleState.Finished)
                .ToList();
            result.Sort((x,y) => x.DateCreated.CompareTo(y.DateCreated));
            return result;
        }
    }
}
