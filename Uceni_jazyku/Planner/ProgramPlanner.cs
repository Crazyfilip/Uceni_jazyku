using System;
using System.Collections.Generic;
using Uceni_jazyku.Cycles.Program;
using Uceni_jazyku.Language;
using Uceni_jazyku.User;

namespace Uceni_jazyku.Planner
{
    /// <inheritdoc/>
    public class ProgramPlanner : IProgramPlanner
    {
        private readonly IPlannerRepository plannerRepository;
        private readonly IUserModelRepository userModelRepository;

        private LanguageCourse languageCourse;
        private AbstractPlannerMemory plannerMemory;
        private UserModel userModel;
        // TODO add dependencies
        // Feedback

        public ProgramPlanner() : this(null, null) { }

        public ProgramPlanner(IPlannerRepository plannerRepository, IUserModelRepository userModelRepository)
        {
            this.plannerRepository = plannerRepository ?? new PlannerRepository();
            this.userModelRepository = userModelRepository ?? new UserModelRepository();
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
            for (int i = 0; i < userModel.CycleTemplate.Count; i++)
            {
                result.Add(GetNextLanguageLesson(username));
            }
            return result;
        }

        public void SetPlanner(LanguageCourse languageCourse, string username)
        {
            this.languageCourse = languageCourse;
            plannerMemory = plannerRepository.GetMemory(languageCourse.CourseId) ?? InitMemory(languageCourse.CourseId);
            userModel = userModelRepository.GetUserModel(username, languageCourse.CourseId);
        }

        private AbstractPlannerMemory InitMemory(string courseId)
        {
            AbstractPlannerMemory result = new PlannerMemory() { CourseId = courseId, MemoryId = Guid.NewGuid().ToString() };
            plannerRepository.InsertMemory(result);
            return result;
        }
    }
}
