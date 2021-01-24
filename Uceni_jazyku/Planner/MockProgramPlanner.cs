using System;
using System.Collections.Generic;
using System.Text;
using Uceni_jazyku.Cycles.Program;

namespace Uceni_jazyku.Planner
{
    public class MockProgramPlanner : IProgramPlanner
    {


        public UserProgramItem getNextLanguageLesson(string username)
        {
            return new UserProgramItem("test", new LanguageProgramItem("lekce x"));
        }

        public List<UserProgramItem> getNextUserCycleProgram(string username)
        {
            List<UserProgramItem> result = new List<UserProgramItem>();
            result.Add(getNextLanguageLesson(username));
            result.Add(getNextLanguageLesson(username));
            return result;
        }
    }
}
