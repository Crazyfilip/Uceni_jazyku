using System;
using System.Collections.Generic;
using System.Text;
using Uceni_jazyku.Cycles.UserCycles;

namespace Uceni_jazyku.Cycles
{
    /// <summary>
    /// Interface of operations with repository of cycles
    /// </summary>
    public interface ICycleRepository
    {

        /// <summary>
        /// Insert cycle to the repository
        /// </summary>
        /// <param name="cycle">inserted cycle</param>
        void PutCycle(UserCycle cycle);

        /// <summary>
        /// Update information about a cycle stored in the repository.
        /// </summary>
        /// <param name="updatedCycle"></param>
        void UpdateCycle(UserCycle updatedCycle);

        /// <summary>
        /// Search for user's oldest inactive cycle
        /// </summary>
        /// <param name="username">user's name</param>
        /// <returns>Oldest user's inactive cycle</returns>
        UserCycle GetOldestUserInactiveCycle(string username, string courseId);

        /// <summary>
        /// Search for user's incomplete cycle
        /// </summary>
        /// <param name="username">user's name</param>
        /// <returns>user's incomplete cycle or null</returns>
        IncompleteUserCycle GetUserIncompleteCycle(string username, string courseId);

        /// <summary>
        /// Search for all user's cycles which are not in finished state
        /// </summary>
        /// <param name="username">user's name</param>
        /// <returns>List of not finished user's cycles</returns>
        List<UserCycle> GetNotFinishedCycles(string username, string courseId);
    }
}
