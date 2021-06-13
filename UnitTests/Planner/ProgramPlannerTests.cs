using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uceni_jazyku.Cycles.Program;
using Uceni_jazyku.Language;
using Uceni_jazyku.Planner;

namespace UnitTests.Planner
{
    [TestClass]
    public class ProgramPlannerTests
    {
        private Mock<LanguageCourse> languageCourse;
        private Mock<IPlannerRepository> plannerRepository;
        private Mock<AbstractPlannerMemory> plannerMemory;
        private ProgramPlanner programPlanner;

        [TestInitialize]
        public void Init()
        {
            languageCourse = new Mock<LanguageCourse>();
            languageCourse.Setup(x => x.CourseId).Returns("course_id");
            plannerMemory = new Mock<AbstractPlannerMemory>();
            plannerRepository = new Mock<IPlannerRepository>();
            plannerRepository.Setup(x => x.GetMemory("course_id")).Returns(plannerMemory.Object);
            programPlanner = new ProgramPlanner(plannerRepository.Object);
            programPlanner.SetCourse(languageCourse.Object);

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
            UserProgramItem result = programPlanner.GetNextLanguageLesson("test");

            // Verify
            Assert.AreEqual("test_id", result.LanguageTopicRef);
            Assert.AreEqual(item.Object, result.LessonRef);

            topic.Verify(x => x.PlanNextLesson(), Times.Once);
            topic.Verify(x => x.PlannedAll(), Times.Once);
            topic.Verify(x => x.TopicId, Times.Once);
            topic.Verify(x => x.TopicPicked(), Times.Once);
            plannerMemory.Verify(x => x.AnyUnfinishedTopic(), Times.Once);
            plannerMemory.Verify(x => x.GetNextTopic(), Times.Never);
            plannerRepository.Verify(x => x.UpdateMemory(plannerMemory.Object), Times.Once);
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
            UserProgramItem result = programPlanner.GetNextLanguageLesson("test");

            // Verify
            Assert.AreEqual("test_id", result.LanguageTopicRef);
            Assert.AreEqual(item.Object, result.LessonRef);

            topic.Verify(x => x.PlanNextLesson(), Times.Once);
            topic.Verify(x => x.PlannedAll(), Times.Once);
            topic.Verify(x => x.TopicId, Times.Once);
            plannerMemory.Verify(x => x.AnyUnfinishedTopic(), Times.Once);
            plannerMemory.Verify(x => x.GetNextTopic(), Times.Once);
            plannerRepository.Verify(x => x.UpdateMemory(plannerMemory.Object), Times.Once);
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
            UserProgramItem result = programPlanner.GetNextLanguageLesson("test");

            // Verify
            Assert.AreEqual("test_id", result.LanguageTopicRef);
            Assert.AreEqual(item.Object, result.LessonRef);

            topic.Verify(x => x.PlanNextLesson(), Times.Once);
            topic.Verify(x => x.PlannedAll(), Times.Once);
            topic.Verify(x => x.TopicId, Times.Once);
            plannerMemory.Verify(x => x.AnyUnfinishedTopic(), Times.Once);
            plannerMemory.Verify(x => x.InsertTopic(topic.Object));
            plannerRepository.Verify(x => x.UpdateMemory(plannerMemory.Object), Times.Once);
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
            List<UserProgramItem> result = programPlanner.GetNextUserCycleProgram("test");

            // Verify
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("test_id", result[0].LanguageTopicRef);
            Assert.AreEqual(item.Object, result[0].LessonRef);

            topic.Verify(x => x.PlanNextLesson(), Times.Once);
            topic.Verify(x => x.PlannedAll(), Times.Once);
            topic.Verify(x => x.TopicId, Times.Once);
            plannerMemory.Verify(x => x.AnyUnfinishedTopic(), Times.Once);
            plannerMemory.Verify(x => x.InsertTopic(topic.Object));
            plannerRepository.Verify(x => x.UpdateMemory(plannerMemory.Object), Times.Once);
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
