using System;
using System.Collections.Generic;
using System.Text;
using Uceni_jazyku.Cycles.Program;

namespace Uceni_jazyku.Planner
{
    public class MockProgramPlanner : IProgramPlanner
    {


        public LanguageProgramItem getNextLanguageLesson(string username)
        {
            return new LanguageProgramItem("lekce x");
        }

        public List<UserProgramItem> getNextUserCycleProgram(string username)
        {
            List<UserProgramItem> result = new List<UserProgramItem>();
            result.Add(new UserProgramItem("test", getNextLanguageLesson(username)));
            result.Add(new UserProgramItem("test", getNextLanguageLesson(username)));
            return result;
        }
    }
}
