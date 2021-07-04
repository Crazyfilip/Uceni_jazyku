using System;
using System.Collections.Generic;
using LanguageLearning.Cycles.Program;
using LanguageLearning.Language;
using LanguageLearning.User;

namespace LanguageLearning.Planner
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
            plannerRepository.Update(plannerMemory);
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
            string courseId = languageCourse.Id;
            plannerMemory = plannerRepository.GetByCourseId(courseId) ?? InitMemory(courseId);
            userModel = userModelRepository.GetByCourseId(courseId);
        }

        private AbstractPlannerMemory InitMemory(string courseId)
        {
            AbstractPlannerMemory result = new PlannerMemory() { CourseId = courseId, Id = Guid.NewGuid().ToString() };
            plannerRepository.Create(result);
            return result;
        }
    }
}
