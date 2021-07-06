using LanguageLearning.Cycle.Model;
using LanguageLearning.Language;
using LanguageLearning.Language.Topic;
using LanguageLearning.User.Model;
using LanguageLearning.User.Model.Impl;
using System;
using System.Collections.Generic;

namespace LanguageLearning.Planner.Impl
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

        public UserCycleItem GetNextLanguageLesson(string username)
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
            return new UserCycleItem(topic.TopicId, item);
        }

        public UserCycleItem GetNextLanguageLesson(string username, string topicId)
        {
            throw new NotImplementedException();
        }

        public List<UserCycleItem> GetNextUserCycleProgram(string username)
        {
            List<UserCycleItem> result = new();
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
