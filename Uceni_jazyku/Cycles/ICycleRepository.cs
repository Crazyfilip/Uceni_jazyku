using System.Collections.Generic;
using LanguageLearning.Common;
using LanguageLearning.Cycles.UserCycles;

namespace LanguageLearning.Cycles
{
    /// <summary>
    /// Interface of operations with repository of cycles
    /// </summary>
    public interface ICycleRepository : IRepository<string, UserCycle>
    {
        /// <summary>
        /// Getter for active cycle
        /// </summary>
        /// <param name="username">user's name</param>
        /// <param name="courseID">language course id</param>
        /// <returns>User's active cycle</returns>
        UserCycle GetActiveCycle(string username, string courseID);

        /// <summary>
        /// Search for user's oldest inactive cycle
        /// </summary>
        /// <param name="username">user's name</param>
        /// <param name="courseId">language course id</param>
        /// <returns>Oldest user's inactive cycle</returns>
        UserCycle GetOldestUserInactiveCycle(string username, string courseId);

        /// <summary>
        /// Search for user's incomplete cycle
        /// </summary>
        /// <param name="username">user's name</param>
        /// <param name="courseId">language course id</param>
        /// <returns>user's incomplete cycle or null</returns>
        IncompleteUserCycle GetUserIncompleteCycle(string username, string courseId);

        /// <summary>
        /// Search for all user's cycles which are not in finished state
        /// </summary>
        /// <param name="username">user's name</param>
        /// <param name="courseId">language course id</param>
        /// <returns>List of not finished user's cycles</returns>
        List<UserCycle> GetNotFinishedCycles(string username, string courseId);
    }
}
