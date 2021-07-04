namespace LanguageLearning.Cycle.Model
{
    /// <summary>
    /// Enum of possible states for user cycle
    /// </summary>
    public enum UserCycleState
    {
        /// <summary>Cycle from which are picked lessons</summary>
        Active,
        /// <summary>Cycles for further planned lessons</summary>
        Inactive,
        /// <summary>User cycle with all lessons finished</summary>
        Finished
    }
}
