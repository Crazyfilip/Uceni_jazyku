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
        private Mock<LanguageCourse> languageCourseMock;
        private ProgramPlanner programPlanner;

        [TestInitialize]
        public void Init()
        {
            languageCourseMock = new Mock<LanguageCourse>();
            programPlanner = new ProgramPlanner();
            programPlanner.SetCourse(languageCourseMock.Object);
        }

        [TestMethod]
        public void TestGetNextLanguageLessonPositive()
        {
            // Init
            Mock<LanguageProgramItem> itemMock = new();

            Mock<LanguageTopic> topicMock = new();
            topicMock.Setup(x => x.PlanNextLesson()).Returns(itemMock.Object);
            topicMock.SetupGet(x => x.TopicId).Returns("test_id");
            languageCourseMock.Setup(x => x.selectNextTopic()).Returns(topicMock.Object);

            // Test
            UserProgramItem result = programPlanner.GetNextLanguageLesson("test");

            // Verify
            Assert.AreEqual("test_id", result.LanguageTopicRef);
            Assert.AreEqual(itemMock.Object, result.LessonRef);
        }

        // TODO negative where no lesson is returned
    }
}
