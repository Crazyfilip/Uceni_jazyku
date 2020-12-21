using System;
using System.Collections.Generic;
using System.Text;
using Uceni_jazyku.Cycles.UserCycles;

namespace Uceni_jazyku.Cycles
{
    /// <summary>
    /// Interface of operations with repository of cycles
    /// </summary>
    interface ICycleRepository
    {

        /// <summary>
        /// Insert cycle to the repository
        /// </summary>
        /// <param name="cycle">inserted cycle</param>
        void PutCycle(AbstractCycle cycle);

        /// <summary>
        /// Update information about a cycle stored in the repository.
        /// </summary>
        /// <param name="updatedCycle"></param>
        void UpdateCycle(AbstractCycle updatedCycle);


        /// <summary>
        /// Getter for number of existing cycles
        /// </summary>
        /// <returns>number of cycles in the repository</returns>
        int GetCyclesCount();

        /// <summary>
        /// Search for user's oldest inactive cycle
        /// </summary>
        /// <param name="username">user's name</param>
        /// <returns>Oldest user's inactive cycle</returns>
        UserCycle GetOldestUserInactiveCycle(string username);

        /// <summary>
        /// Search for user's incomplete cycle
        /// </summary>
        /// <param name="username">user's name</param>
        /// <returns>user's incomplete cycle or null</returns>
        IncompleteUserCycle GetUserIncompleteCycle(string username);
    }
}
