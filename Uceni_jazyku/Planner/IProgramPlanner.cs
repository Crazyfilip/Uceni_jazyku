using System;
using System.Collections.Generic;
using System.Text;
using Uceni_jazyku.Cycles.Program;
using Uceni_jazyku.Language;

namespace Uceni_jazyku.Planner
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
        /// Set from what language course are lessons picked
        /// Save current state of planner and reset planner for new course
        /// </summary>
        /// <param name="languageCourse">LanguageCourse</param>
        void SetCourse(string username, LanguageCourse languageCourse);
    }
}
