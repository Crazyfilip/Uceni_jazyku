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
        UserProgramItem GetNextLanguageLesson(string username, string topicId);

        void SetCourse(LanguageCourse languageCourse);
    }
}
