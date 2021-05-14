using System;
using System.Collections.Generic;
using System.Text;
using Uceni_jazyku.Cycles.Program;
using Uceni_jazyku.Language;

namespace Uceni_jazyku.Planner
{
    public class MockProgramPlanner : IProgramPlanner
    {


        public UserProgramItem GetNextLanguageLesson(string username)
        {
            return new UserProgramItem("test", new LanguageProgramItem("lekce x"));
        }

        public UserProgramItem GetNextLanguageLesson(string username, string topicId)
        {
            throw new NotImplementedException();
        }

        public List<UserProgramItem> GetNextUserCycleProgram(string username)
        {
            List<UserProgramItem> result = new List<UserProgramItem>();
            result.Add(GetNextLanguageLesson(username));
            result.Add(GetNextLanguageLesson(username));
            return result;
        }

        public void SetCourse(string username, LanguageCourse languageCourse)
        {
        }
    }
}
