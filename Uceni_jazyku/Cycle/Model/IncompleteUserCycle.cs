using System.Runtime.Serialization;

namespace LanguageLearning.Cycle.Model
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

        public IncompleteUserCycle() { }

        public IncompleteUserCycle(int limit)
        {
            this.State = UserCycleState.Inactive;
            this.limit = limit;
        }

        /// <summary>
        /// Add lesson to beginning of the cycle
        /// </summary>
        /// <param name="item">lesson to add</param>
        public virtual void AddLesson(UserCycleItem item)
        {
            UserProgramItems.Insert(0, item);
        }

        // TODO when full then convert to normal UserCycle
    }
}
