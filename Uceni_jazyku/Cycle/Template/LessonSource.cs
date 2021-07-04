namespace LanguageLearning.Cycle.Template
{
    /// <summary>
    /// Enum capturing possible sources where planner can pick lesson for user cycle
    /// </summary>
    public enum LessonSource
    {
        /// <summary>Planner can use any available source</summary>
        ANY,
        /// <summary>Planner should pick lesson from new topic from course</summary>
        NEW_TOPIC,
        /// <summary>Planner should pick lesson from already picked topic</summary>
        UNFINISHED_TOPIC,
        /// <summary>Planner should pick lesson from feedback</summary>
        REVISION
    }
}
