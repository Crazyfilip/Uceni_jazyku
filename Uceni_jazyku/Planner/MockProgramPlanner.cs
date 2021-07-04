using System;
using System.Collections.Generic;
using LanguageLearning.Cycles.Program;
using LanguageLearning.Language;

namespace LanguageLearning.Planner
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

        public void SetPlanner(LanguageCourse languageCourse, string username)
        {
        }
    }
}
