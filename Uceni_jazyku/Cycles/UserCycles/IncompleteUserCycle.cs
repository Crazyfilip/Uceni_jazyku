using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using Uceni_jazyku.Cycles.Program;

namespace Uceni_jazyku.Cycles.UserCycles
{
    /// <summary>
    /// Incomplete user cycle
    /// Incomplete in a way that its program don't need to be fully initialized
    /// This type of cycle has always state inactive
    /// </summary>
    [DataContract]
    public class IncompleteUserCycle : UserCycle
    {
        public IncompleteUserCycle(string username)
        {
            this.Username = username;
            this.State = UserCycleState.Inactive;
            IsUserAssigned = true;
        }

        /// <summary>
        /// Add lesson to beginning of the cycle
        /// </summary>
        /// <param name="item">lesson to add</param>
        public void AddLesson(UserProgramItem item)
        {
            UserProgramItems.Insert(0, item);
        }

        // TODO when full then convert to normal UserCycle
    }
}
