﻿using LanguageLearning.Cycle.Model;
using LanguageLearning.Cycle.Template;
using LanguageLearning.Language;
using LanguageLearning.Language.Topic;
using LanguageLearning.Planner;
using LanguageLearning.Planner.Impl;
using LanguageLearning.User.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;

namespace UnitTests.Planner.Impl
{
    [TestClass]
    public class ProgramPlannerTests
    {
        private Mock<LanguageCourse> languageCourse;
        private Mock<IPlannerRepository> plannerRepository;
        private Mock<AbstractPlannerMemory> plannerMemory;
        private Mock<IUserModelRepository> userModelRepository;
        private Mock<UserModel> userModel;
        private ProgramPlanner programPlanner;


        [TestInitialize]
        public void Init()
        {
            languageCourse = new Mock<LanguageCourse>();
            languageCourse.Setup(x => x.Id).Returns("course_id");
            plannerMemory = new Mock<AbstractPlannerMemory>();
            plannerRepository = new Mock<IPlannerRepository>();
            plannerRepository.Setup(x => x.GetByCourseId("course_id")).Returns(plannerMemory.Object);
            userModel = new Mock<UserModel>();
            userModel.SetupGet(x => x.CycleTemplate).Returns(new List<LessonDescription>() { new LessonDescription() });
            userModelRepository = new Mock<IUserModelRepository>();
            userModelRepository.Setup(x => x.GetByCourseId("course_id")).Returns(userModel.Object);
            programPlanner = new ProgramPlanner(plannerRepository.Object, userModelRepository.Object);
            programPlanner.SetPlanner(languageCourse.Object, "test");

            plannerRepository.Reset();
            languageCourse.Reset();
        }

        [TestMethod]
        public void TestGetNextLanguageLessonPositiveNewTopic()
        {
            // Init
            Mock<LanguageProgramItem> item = new();

            Mock<LanguageTopic> topic = new();
            topic.Setup(x => x.PlanNextLesson()).Returns(item.Object);
            topic.Setup(x => x.PlannedAll()).Returns(true);
            topic.SetupGet(x => x.TopicId).Returns("test_id");
            plannerMemory.Setup(x => x.AnyUnfinishedTopic()).Returns(false);
            languageCourse.Setup(x => x.selectNextTopic()).Returns(topic.Object);

            // Test
            UserCycleItem result = programPlanner.GetNextLanguageLesson("test");

            // Verify
            Assert.AreEqual("test_id", result.LanguageTopicRef);
            Assert.AreEqual(item.Object, result.LessonRef);

            topic.Verify(x => x.PlanNextLesson(), Times.Once);
            topic.Verify(x => x.PlannedAll(), Times.Once);
            topic.Verify(x => x.TopicId, Times.Once);
            topic.Verify(x => x.TopicPicked(), Times.Once);
            plannerMemory.Verify(x => x.AnyUnfinishedTopic(), Times.Once);
            plannerMemory.Verify(x => x.GetNextTopic(), Times.Never);
            plannerRepository.Verify(x => x.Update(plannerMemory.Object), Times.Once);
            languageCourse.Verify(x => x.selectNextTopic(), Times.Once);

            item.VerifyNoOtherCalls();
            topic.VerifyNoOtherCalls();
            plannerMemory.VerifyNoOtherCalls();
            plannerRepository.VerifyNoOtherCalls();
            languageCourse.VerifyNoOtherCalls();
        }

        [TestMethod]
        public void TestGetNextLanguageLessonPositiveTopicFromMemory()
        {
            // Init
            Mock<LanguageProgramItem> item = new();

            Mock<LanguageTopic> topic = new();
            topic.Setup(x => x.PlanNextLesson()).Returns(item.Object);
            topic.Setup(x => x.PlannedAll()).Returns(true);
            topic.SetupGet(x => x.TopicId).Returns("test_id");
            plannerMemory.Setup(x => x.AnyUnfinishedTopic()).Returns(true);
            plannerMemory.Setup(x => x.GetNextTopic()).Returns(topic.Object);

            // Test
            UserCycleItem result = programPlanner.GetNextLanguageLesson("test");

            // Verify
            Assert.AreEqual("test_id", result.LanguageTopicRef);
            Assert.AreEqual(item.Object, result.LessonRef);

            topic.Verify(x => x.PlanNextLesson(), Times.Once);
            topic.Verify(x => x.PlannedAll(), Times.Once);
            topic.Verify(x => x.TopicId, Times.Once);
            plannerMemory.Verify(x => x.AnyUnfinishedTopic(), Times.Once);
            plannerMemory.Verify(x => x.GetNextTopic(), Times.Once);
            plannerRepository.Verify(x => x.Update(plannerMemory.Object), Times.Once);
            languageCourse.Verify(x => x.selectNextTopic(), Times.Never);

            item.VerifyNoOtherCalls();
            topic.VerifyNoOtherCalls();
            plannerMemory.VerifyNoOtherCalls();
            plannerRepository.VerifyNoOtherCalls();
            languageCourse.VerifyNoOtherCalls();
        }

        [DataRow(true)]
        [DataRow(false)]
        [DataTestMethod]

        public void TestGetNextLanguageLessonPositiveUnfinishedTopic(bool fromMemory)
        {
            // Init
            Mock<LanguageProgramItem> item = new();

            Mock<LanguageTopic> topic = new();
            topic.Setup(x => x.PlanNextLesson()).Returns(item.Object);
            topic.Setup(x => x.PlannedAll()).Returns(false);
            topic.SetupGet(x => x.TopicId).Returns("test_id");
            plannerMemory.Setup(x => x.AnyUnfinishedTopic()).Returns(fromMemory);
            if (fromMemory)
            {
                plannerMemory.Setup(x => x.GetNextTopic()).Returns(topic.Object);
            }
            else
            {
                languageCourse.Setup(x => x.selectNextTopic()).Returns(topic.Object);
            }

            // Test
            UserCycleItem result = programPlanner.GetNextLanguageLesson("test");

            // Verify
            Assert.AreEqual("test_id", result.LanguageTopicRef);
            Assert.AreEqual(item.Object, result.LessonRef);

            topic.Verify(x => x.PlanNextLesson(), Times.Once);
            topic.Verify(x => x.PlannedAll(), Times.Once);
            topic.Verify(x => x.TopicId, Times.Once);
            plannerMemory.Verify(x => x.AnyUnfinishedTopic(), Times.Once);
            plannerMemory.Verify(x => x.InsertTopic(topic.Object));
            plannerRepository.Verify(x => x.Update(plannerMemory.Object), Times.Once);
            if (fromMemory)
            {
                plannerMemory.Verify(x => x.GetNextTopic(), Times.Once);
                languageCourse.Verify(x => x.selectNextTopic(), Times.Never);
            }
            else
            {
                plannerMemory.Verify(x => x.GetNextTopic(), Times.Never);
                languageCourse.Verify(x => x.selectNextTopic(), Times.Once);
                topic.Verify(x => x.TopicPicked(), Times.Once);
            }

            item.VerifyNoOtherCalls();
            topic.VerifyNoOtherCalls();
            plannerMemory.VerifyNoOtherCalls();
            plannerRepository.VerifyNoOtherCalls();
            languageCourse.VerifyNoOtherCalls();
        }

        // TODO negative where no lesson is returned

        [DataRow(true)]
        [DataRow(false)]
        [DataTestMethod]
        public void TestGetNextUserCycleProgramPositive(bool fromMemory)
        {
            // Init
            Mock<LanguageProgramItem> item = new();

            Mock<LanguageTopic> topic = new();
            topic.Setup(x => x.PlanNextLesson()).Returns(item.Object);
            topic.Setup(x => x.PlannedAll()).Returns(false);
            topic.SetupGet(x => x.TopicId).Returns("test_id");
            plannerMemory.Setup(x => x.AnyUnfinishedTopic()).Returns(fromMemory);
            if (fromMemory)
            {
                plannerMemory.Setup(x => x.GetNextTopic()).Returns(topic.Object);
            }
            else
            {
                languageCourse.Setup(x => x.selectNextTopic()).Returns(topic.Object);
            }

            // Test
            List<UserCycleItem> result = programPlanner.GetNextUserCycleProgram("test");

            // Verify
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("test_id", result[0].LanguageTopicRef);
            Assert.AreEqual(item.Object, result[0].LessonRef);

            topic.Verify(x => x.PlanNextLesson(), Times.Once);
            topic.Verify(x => x.PlannedAll(), Times.Once);
            topic.Verify(x => x.TopicId, Times.Once);
            plannerMemory.Verify(x => x.AnyUnfinishedTopic(), Times.Once);
            plannerMemory.Verify(x => x.InsertTopic(topic.Object));
            plannerRepository.Verify(x => x.Update(plannerMemory.Object), Times.Once);
            if (fromMemory)
            {
                plannerMemory.Verify(x => x.GetNextTopic(), Times.Once);
                languageCourse.Verify(x => x.selectNextTopic(), Times.Never);
            }
            else
            {
                plannerMemory.Verify(x => x.GetNextTopic(), Times.Never);
                languageCourse.Verify(x => x.selectNextTopic(), Times.Once);
                topic.Verify(x => x.TopicPicked(), Times.Once);
            }

            item.VerifyNoOtherCalls();
            topic.VerifyNoOtherCalls();
            plannerMemory.VerifyNoOtherCalls();
            plannerRepository.VerifyNoOtherCalls();
            languageCourse.VerifyNoOtherCalls();
        }
    }
}
