using System;
using System.Collections.Generic;
using System.Text;
using Uceni_jazyku.Cycles.Program;

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
        List<UserProgramItem> getNextUserCycleProgram(string username);

        /// <summary>
        /// Get following lesson for user
        /// </summary>
        /// <param name="username">username</param>
        /// <returns>Lesson</returns>
        UserProgramItem getNextLanguageLesson(string username);
    }
}
