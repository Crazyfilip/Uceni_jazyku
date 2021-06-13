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
        IPlannerRepository plannerRepository;
        AbstractPlannerMemory plannerMemory;
        // TODO add dependencies
        // UserModel, Feedback

        public ProgramPlanner() : this(null) { }

        public ProgramPlanner(IPlannerRepository plannerRepository)
        {
            this.plannerRepository = plannerRepository ?? new PlannerRepository();
        }

        public UserProgramItem GetNextLanguageLesson(string username)
        {
            LanguageTopic topic;
            // TODO feedback
            if (plannerMemory.AnyUnfinishedTopic())
            {
                topic = plannerMemory.GetNextTopic();
            } 
            else
            {
                topic = languageCourse.selectNextTopic();
                // TODO persistent solution for setting picked
                topic.TopicPicked();
            }
            LanguageProgramItem item = topic.PlanNextLesson();
            if (!topic.PlannedAll())
            {
                plannerMemory.InsertTopic(topic);
            }
            plannerRepository.UpdateMemory(plannerMemory);
            return new UserProgramItem(topic.TopicId, item);
        }

        public UserProgramItem GetNextLanguageLesson(string username, string topicId)
        {
            throw new NotImplementedException();
        }

        public List<UserProgramItem> GetNextUserCycleProgram(string username)
        {
            List<UserProgramItem> result = new List<UserProgramItem>();
            // TODO from UserModel get length of cycle
            for (int i = 0; i < 1; i++)
            {
                result.Add(GetNextLanguageLesson(username));
            }
            return result;
        }

        public void SetCourse(LanguageCourse languageCourse)
        {
            this.languageCourse = languageCourse;
            plannerMemory = plannerRepository.GetMemory(languageCourse.CourseId) ?? InitMemory(languageCourse.CourseId);
        }

        private AbstractPlannerMemory InitMemory(string courseId)
        {
            AbstractPlannerMemory result = new PlannerMemory() { CourseId = courseId, MemoryId = Guid.NewGuid().ToString() };
            plannerRepository.InsertMemory(result);
            return result;
        }
    }
}
