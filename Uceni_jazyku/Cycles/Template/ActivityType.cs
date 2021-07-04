namespace LanguageLearning.Cycles.Template
{
    /// <summary>
    /// Enum capturing possible activities
    /// For user cycle it captures main activity which should be lesson focused
    /// but in lesson can be used other activities as well
    /// </summary>
    public enum ActivityType
    {
        /// <summary>Any activity type for exercise can be used</summary>
        ANY,
        /// <summary>Exercises for translating should be preferred</summary>
        TRANSLATION,
        /// <summary>Exercises with filling the gap should be preferred</summary>
        FILL_THE_GAP
    }
}
