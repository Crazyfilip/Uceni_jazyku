using System.Collections.Generic;
using LanguageLearning.Cycles.Program;
using LanguageLearning.Language;

namespace LanguageLearning.Planner
{
    /// <summary>
    /// Interface for operations of planning programs for user cycles
    /// </summary>
    public interface IProgramPlanner
    {
        /// <summary>
        /// Get full program for user cycle
        /// </summary>
        /// <param name="username">username</param>
        /// <returns>List of lessons (program)</returns>
        List<UserProgramItem> GetNextUserCycleProgram(string username);

        /// <summary>
        /// Get following lesson for user
        /// </summary>
        /// <param name="username">username</param>
        /// <returns>Lesson</returns>
        UserProgramItem GetNextLanguageLesson(string username);

        /// <summary>
        /// Get following lesson from language topic chosen by user
        /// </summary>
        /// <param name="username">username</param>
        /// <param name="topicId">language topic id</param>
        /// <returns></returns>
        UserProgramItem GetNextLanguageLesson(string username, string topicId);

        /// <summary>
        /// Set up planner fom given language course and user
        /// </summary>
        /// <param name="languageCourse">LanguageCourse</param>
        /// <param name="username">username</param>
        void SetPlanner(LanguageCourse languageCourse, string username);
    }
}
