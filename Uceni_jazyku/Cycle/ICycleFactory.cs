using LanguageLearning.Cycle.Model;
using System.Collections.Generic;

namespace LanguageLearning.Cycle
{
    /// <summary>
    /// Interface for operations of creation of user cycles
    /// It takes care of generating CycleID
    /// </summary>
    public interface ICycleFactory
    {
        /// <summary>
        /// Create user cycle in initial state, cycleId and DateCreated
        /// </summary>
        /// <param name="username">username</param>
        /// <param name="courseId">language course id</param>
        /// <param name="program">lessons for cycle</param>
        /// <returns>instance of user cycle</returns>
        UserCycle CreateCycle(string username, string courseId, List<UserCycleItem> program);

        /// <summary>
        /// Create incomplete user cycle with cycleId and DateCreated
        /// </summary>
        /// <param name="username">username</param>
        /// <param name="courseId">language course id</param>
        /// <param name="limit">number of lessons in cycle</param>
        /// <returns>instance of incomplete user cycle</returns>
        IncompleteUserCycle CreateIncompleteCycle(string username, string courseId, int limit);
    }
}
