using System;
using System.Runtime.Serialization;
using LanguageLearning.Cycles.Program;

namespace LanguageLearning.Cycles.UserCycles
{
    /// <summary>
    /// Incomplete user cycle
    /// Incomplete in a way that its program don't need to be fully initialized
    /// This type of cycle has always state inactive
    /// </summary>
    [DataContract]
    public class IncompleteUserCycle : UserCycle
    {
        public int limit { get; init; }

        public IncompleteUserCycle() {}

        public IncompleteUserCycle(int limit)
        {
            this.State = UserCycleState.Inactive;
            this.limit = limit;
        }

        /// <summary>
        /// Add lesson to beginning of the cycle
        /// </summary>
        /// <param name="item">lesson to add</param>
        public virtual void AddLesson(UserProgramItem item)
        {
            UserProgramItems.Insert(0, item);
        }

        // TODO when full then convert to normal UserCycle
    }
}
