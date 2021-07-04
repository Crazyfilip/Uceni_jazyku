using LanguageLearning.Language.Topic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;

namespace UnitTests.Language.Topic
{
    [TestClass]
    public class LanguageTopicTests
    {
        LanguageTopic topic1, topic2;
        Mock<LanguageProgramItem> itemMock1;

        [TestInitialize]
        public void Init()
        {
            itemMock1 = new Mock<LanguageProgramItem>();
            topic1 = new LanguageTopic() { Lessons = new List<LanguageProgramItem>() { itemMock1.Object } };
            topic2 = new LanguageTopic() { Lessons = new List<LanguageProgramItem>() };
        }

        [TestMethod]
        public void TestPlanNextLessonPositive()
        {
            // Init
            int originalNumberOfPlanned = topic1.PlannedLessons;

            // Test
            LanguageProgramItem result = topic1.PlanNextLesson();

            // Verify
            Assert.AreEqual(itemMock1.Object, result);
            Assert.AreEqual(originalNumberOfPlanned + 1, topic1.PlannedLessons);
            itemMock1.VerifyNoOtherCalls();
        }

        [TestMethod]
        public void TestPlanNextLessonNegative()
        {
            // Init
            int originalNumberOfPlanned = topic1.PlannedLessons;

            // Test
            LanguageProgramItem result = topic2.PlanNextLesson();

            // Verify
            Assert.IsNull(result);
            Assert.AreEqual(originalNumberOfPlanned, topic1.PlannedLessons);
            itemMock1.VerifyNoOtherCalls();
        }

        [TestMethod]
        public void TestFinishLessonPositive()
        {
            // Init
            int originalNumberOfFinished = topic1.FinishedLessons;
            itemMock1.Setup(x => x.Finish()).Verifiable();

            // Test
            topic1.FinishLesson();

            // Verify
            Assert.AreEqual(originalNumberOfFinished + 1, topic1.FinishedLessons);
            itemMock1.Verify(x => x.Finish(), Times.Once);
            itemMock1.VerifyNoOtherCalls();
        }
    }
}
