using System;
using System.Collections.Generic;
using Uceni_jazyku.Cycles.Program;
using Uceni_jazyku.Language;

namespace Uceni_jazyku.Planner
{
    /// <inheritdoc/>
    public class ProgramPlanner : IProgramPlanner
    {
        LanguageCourse languageCourse;
        // TODO add dependencies
        // PlannerHistory, UserModel, Feedback

        public ProgramPlanner() { }

        public UserProgramItem GetNextLanguageLesson(string username)
        {
            LanguageTopic topic = languageCourse.selectNextTopic(); // TODO with PlannerHistory can pick unfinished topic
            LanguageProgramItem item = topic.PlanNextLesson();
            return new UserProgramItem(topic.TopicId, item);
        }

        public UserProgramItem GetNextLanguageLesson(string username, string topicId)
        {
            throw new NotImplementedException();
        }

        public List<UserProgramItem> GetNextUserCycleProgram(string username)
        {
            throw new NotImplementedException();
        }

        public void SetCourse(LanguageCourse languageCourse)
        {
            this.languageCourse = languageCourse;
            // TODO save and reset planner
        }
    }
}
