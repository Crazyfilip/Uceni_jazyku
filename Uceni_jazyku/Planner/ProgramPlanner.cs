using System;
using System.Collections.Generic;
using System.Text;
using Uceni_jazyku.Cycles.Program;

namespace Uceni_jazyku.Planner
{
    /// <inheritdoc/>
    public class ProgramPlanner : IProgramPlanner
    {
        // TODO add dependencies
        // Language (+ ILanguageTopicSelectStrategy), PlannerHistory, UserModel

        public UserProgramItem getNextLanguageLesson(string username)
        {
            throw new NotImplementedException();
        }

        public List<UserProgramItem> getNextUserCycleProgram(string username)
        {
            throw new NotImplementedException();
        }
    }
}
