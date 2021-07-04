using LanguageLearning.Cycle.Model;
using LanguageLearning.Language;
using LanguageLearning.Language.Topic;
using System;
using System.Collections.Generic;

namespace LanguageLearning.Planner
{
    public class MockProgramPlanner : IProgramPlanner
    {


        public UserCycleItem GetNextLanguageLesson(string username)
        {
            return new UserCycleItem("test", new LanguageProgramItem("lekce x"));
        }

        public UserCycleItem GetNextLanguageLesson(string username, string topicId)
        {
            throw new NotImplementedException();
        }

        public List<UserCycleItem> GetNextUserCycleProgram(string username)
        {
            List<UserCycleItem> result = new List<UserCycleItem>();
            result.Add(GetNextLanguageLesson(username));
            result.Add(GetNextLanguageLesson(username));
            return result;
        }

        public void SetPlanner(LanguageCourse languageCourse, string username)
        {
        }
    }
}
