using System;
using System.Collections.Generic;
using System.Text;
using Uceni_jazyku.Cycles.Program;

namespace Uceni_jazyku.Planner
{
    public interface IProgramPlanner
    {
        List<UserProgramItem> getNextUserCycleProgram(string username);

        UserProgramItem getNextLanguageLesson(string username);
    }
}
