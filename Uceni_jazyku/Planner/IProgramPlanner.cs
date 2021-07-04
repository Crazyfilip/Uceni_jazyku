using LanguageLearning.Cycle.Model;
using LanguageLearning.Language;
using System.Collections.Generic;

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
        List<UserCycleItem> GetNextUserCycleProgram(string username);

        /// <summary>
        /// Get following lesson for user
        /// </summary>
        /// <param name="username">username</param>
        /// <returns>Lesson</returns>
        UserCycleItem GetNextLanguageLesson(string username);

        /// <summary>
        /// Get following lesson from language topic chosen by user
        /// </summary>
        /// <param name="username">username</param>
        /// <param name="topicId">language topic id</param>
        /// <returns></returns>
        UserCycleItem GetNextLanguageLesson(string username, string topicId);

        /// <summary>
        /// Set up planner fom given language course and user
        /// </summary>
        /// <param name="languageCourse">LanguageCourse</param>
        /// <param name="username">username</param>
        void SetPlanner(LanguageCourse languageCourse, string username);
    }
}
