using System;
using System.Collections.Generic;
using System.Text;
using Uceni_jazyku.Cycles.UserCycles;

namespace Uceni_jazyku.Cycles
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
        /// <returns>instance of user cycle</returns>
        UserCycle CreateCycle();

        /// <summary>
        /// Create incomplete user cycle with cycleId and DateCreated
        /// </summary>
        /// <param name="username">username</param>
        /// <returns>instance of incomplete user cycle</returns>
        IncompleteUserCycle CreateIncompleteCycle(string username);
    }
}
