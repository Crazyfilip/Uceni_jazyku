using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using Uceni_jazyku.Cycles;
using Uceni_jazyku.Cycles.Program;
using Uceni_jazyku.Cycles.UserCycles;
using System.Collections.Generic;
using Moq;
using log4net;
using System.Reflection;
using Uceni_jazyku.Planner;
using Uceni_jazyku.Language;

namespace UnitTests
{
    /// <summary>
    /// Tests for cycle service
    /// </summary>
    [TestClass]
    public class CycleServiceTests
    {
        CycleService service;
        Mock<ICycleRepository> databaseMock;
        Mock<IProgramPlanner> plannerMock;
        Mock<ICycleFactory> cycleFactoryMock;
        Mock<LanguageCourse> languageCourseMock;
        static readonly Mock<ILog> log4netMock = new Mock<ILog>();

        [ClassInitialize]
        public static void ClassInit(TestContext testContext)
        {
            var field = typeof(CycleService).GetField("log", BindingFlags.Static | BindingFlags.NonPublic);
            field.SetValue(null, log4netMock.Object);
        }

        [TestInitialize]
        public void TestInitialization()
        {
            Directory.CreateDirectory("./cycles/service");
            databaseMock = new Mock<ICycleRepository>();
            plannerMock = new Mock<IProgramPlanner>();
            cycleFactoryMock = new Mock<ICycleFactory>();
            languageCourseMock = new Mock<LanguageCourse>();
            languageCourseMock.SetupGet(x => x.CourseId).Returns("course_id");
            service = CycleService.GetInstance(databaseMock.Object, plannerMock.Object, cycleFactoryMock.Object);
            service.SetActiveCourse(languageCourseMock.Object);
            log4netMock.Reset();
        }

        [TestMethod]
        public void TestGetNextCyclePositiveCreateNew()
        {
            // Init
            Mock<UserCycle> cycleMock = new Mock<UserCycle>();
            cycleMock.SetupGet(x => x.CycleID).Returns("test_id");
            cycleMock.SetupGet(x => x.Username).Returns("test");
            Mock<List<UserProgramItem>> programMock = new Mock<List<UserProgramItem>>();
            databaseMock.Setup(x => x.GetOldestUserInactiveCycle("test", "course_id")).Returns((UserCycle)null);
            plannerMock.Setup(x => x.GetNextUserCycleProgram("test")).Returns(programMock.Object);
            cycleFactoryMock.Setup(x => x.CreateCycle("test", "course_id", programMock.Object)).Returns(cycleMock.Object);

            // Test
            UserCycle result = service.GetNextCycle("test");

            // Verify
            Assert.AreSame(cycleMock.Object, result);

            cycleMock.Verify(x => x.CycleID, Times.Once);

            databaseMock.Verify(x => x.GetOldestUserInactiveCycle("test", "course_id"), Times.Once);
            databaseMock.Verify(x => x.PutCycle(cycleMock.Object), Times.Once);
            plannerMock.Verify(x => x.GetNextUserCycleProgram("test"), Times.Once);

            log4netMock.Verify(x => x.Info("Getting cycle for user test"), Times.Once);
            log4netMock.Verify(x => x.Debug("Looking if there is existing inactive cycle for user test"), Times.Once);
            log4netMock.Verify(x => x.Debug("No cycle found, new must be created"), Times.Once);
            log4netMock.Verify(x => x.Info("Creating new cycle for user test"), Times.Once);
            log4netMock.Verify(x => x.Debug("New cycle created with id test_id"), Times.Once);

            databaseMock.VerifyNoOtherCalls();
            log4netMock.VerifyNoOtherCalls();
            plannerMock.VerifyNoOtherCalls();
            cycleMock.VerifyNoOtherCalls();
            cycleFactoryMock.VerifyNoOtherCalls();
        }

        [TestMethod]
        public void TestGetUserCyclePositiveFromInactive()
        {
            // Init
            Mock<UserCycle> cycleMock = new Mock<UserCycle>();
            cycleMock.SetupGet(x => x.State).Returns(UserCycleState.Inactive);
            cycleMock.SetupGet(x => x.CycleID).Returns("testId");
            cycleMock.Setup(x => x.Activate()).Returns(cycleMock.Object);
            databaseMock.Setup(x => x.GetOldestUserInactiveCycle("test", "course_id")).Returns(cycleMock.Object);
            databaseMock.Setup(x => x.UpdateCycle(cycleMock.Object)).Verifiable();

            // Test
            UserCycle result = service.GetNextCycle("test");

            // Verify
            Assert.AreSame(cycleMock.Object, result);

            databaseMock.Verify(x => x.GetOldestUserInactiveCycle("test", "course_id"), Times.Once);
            cycleMock.Verify(x => x.CycleID, Times.Exactly(2));
            cycleMock.Verify(x => x.Activate(), Times.Once);
            databaseMock.Verify(x => x.UpdateCycle(cycleMock.Object), Times.Once);
            log4netMock.Verify(x => x.Info("Getting cycle for user test"), Times.Once);
            log4netMock.Verify(x => x.Debug("Looking if there is existing inactive cycle for user test"), Times.Once);
            log4netMock.Verify(x => x.Debug("Obtained testId"), Times.Once);
            log4netMock.Verify(x => x.Info("Activating cycle testId"), Times.Once);

            databaseMock.VerifyNoOtherCalls();
            cycleMock.VerifyNoOtherCalls();
            log4netMock.VerifyNoOtherCalls();
        }

        [TestMethod]
        public void TestGetNewCyclePositive()
        {
            // Init
            Mock<UserCycle> cycleMock = new Mock<UserCycle>();
            cycleMock.SetupGet(x => x.CycleID).Returns("test_id");
            Mock<List<UserProgramItem>> programMock = new Mock<List<UserProgramItem>>();
            plannerMock.Setup(x => x.GetNextUserCycleProgram("test")).Returns(programMock.Object);
            cycleFactoryMock.Setup(x => x.CreateCycle("test", "course_id", programMock.Object)).Returns(cycleMock.Object);
            databaseMock.Setup(x => x.PutCycle(cycleMock.Object)).Verifiable();

            // Test
            UserCycle result = service.GetNewCycle("test");

            // Verify
            Assert.AreSame(cycleMock.Object, result);
            cycleMock.Verify(x => x.CycleID, Times.Once);
            cycleFactoryMock.Verify(x => x.CreateCycle("test", "course_id", programMock.Object), Times.Once);
            databaseMock.Verify(x => x.PutCycle(cycleMock.Object), Times.Once);
            log4netMock.Verify(x => x.Info("Creating new cycle for user test"), Times.Once);
            log4netMock.Verify(x => x.Debug("New cycle created with id test_id"), Times.Once);

            cycleMock.VerifyNoOtherCalls();
            databaseMock.VerifyNoOtherCalls();
            log4netMock.VerifyNoOtherCalls();
            cycleFactoryMock.VerifyNoOtherCalls();
        }

        [TestMethod]
        public void TestActivatePositive()
        {
            // Init
            string cycleId = "test";

            Mock<UserCycle> cycleMock = new Mock<UserCycle>();
            cycleMock.SetupGet(x => x.State).Returns(UserCycleState.Inactive);
            cycleMock.SetupGet(x => x.CycleID).Returns(cycleId);
            cycleMock.Setup(x => x.Activate()).Returns(cycleMock.Object);
            databaseMock.Setup(x => x.UpdateCycle(cycleMock.Object)).Verifiable();

            // Test
            UserCycle result = service.Activate(cycleMock.Object);

            // Verify
            Assert.AreSame(cycleMock.Object, result);

            cycleMock.Verify(x => x.CycleID, Times.Once);
            cycleMock.Verify(x => x.Activate(), Times.Once);
            databaseMock.Verify(x => x.UpdateCycle(cycleMock.Object), Times.Once);
            log4netMock.Verify(x => x.Info($"Activating cycle {cycleId}"), Times.Once);

            cycleMock.VerifyNoOtherCalls();
            databaseMock.VerifyNoOtherCalls();
            log4netMock.VerifyNoOtherCalls();
        }

        [DataRow(UserCycleState.Active)]
        [DataRow(UserCycleState.Finished)]
        [DataTestMethod]
        public void TestActivateNegative(UserCycleState incorrectState)
        {
            // Init
            string cycleId = "test";

            Mock<UserCycle> cycleMock = new Mock<UserCycle>();
            cycleMock.SetupGet(x => x.State).Returns(incorrectState);
            cycleMock.SetupGet(x => x.CycleID).Returns(cycleId);
            cycleMock.Setup(x => x.Activate()).Throws<IncorrectCycleStateException>();

            // Test
            service.Activate(cycleMock.Object);

            // Verify
            cycleMock.Verify(x => x.CycleID, Times.Exactly(2));
            cycleMock.Verify(x => x.Activate(), Times.Once);
            log4netMock.Verify(x => x.Info("Activating cycle test"), Times.Once);
            log4netMock.Verify(x => x.Warn($"Cycle {cycleId} wasn't activated", It.Is<Exception>(x => x is IncorrectCycleStateException)), Times.Once);

            cycleMock.VerifyNoOtherCalls();
            databaseMock.VerifyNoOtherCalls();
            log4netMock.VerifyNoOtherCalls();
        }

        [TestMethod]
        public void TestInactivatePositive()
        {
            // Init
            string cycleId = "test";
            Mock<UserCycle> cycleMock = new Mock<UserCycle>();
            cycleMock.Setup(x => x.Inactivate()).Returns(cycleMock.Object);
            cycleMock.SetupGet(x => x.CycleID).Returns(cycleId);
            databaseMock.Setup(x => x.UpdateCycle(cycleMock.Object)).Verifiable();

            // Test
            UserCycle result = service.Inactivate(cycleMock.Object);

            // Verify
            Assert.AreSame(cycleMock.Object, result);

            cycleMock.Verify(x => x.Inactivate(), Times.Once);
            cycleMock.Verify(x => x.CycleID, Times.Once);
            databaseMock.Verify(x => x.UpdateCycle(cycleMock.Object), Times.Once);
            log4netMock.Verify(x => x.Info("Inactivating cycle test"), Times.Once);

            cycleMock.VerifyNoOtherCalls();
            databaseMock.VerifyNoOtherCalls();
            log4netMock.VerifyNoOtherCalls();
        }

        [TestMethod]
        public void TestInactivateNegative()
        {
            // Init
            string cycleId = "test";

            Mock<UserCycle> cycleMock = new Mock<UserCycle>();
            cycleMock.Setup(x => x.Inactivate()).Throws<IncorrectCycleStateException>();
            cycleMock.SetupGet(x => x.CycleID).Returns(cycleId);

            // Test 
            UserCycle result = service.Inactivate(cycleMock.Object);

            // Verify
            cycleMock.Verify(x => x.Inactivate(), Times.Once);
            cycleMock.Verify(x => x.CycleID, Times.Exactly(2));
            log4netMock.Verify(x => x.Info($"Inactivating cycle {cycleId}"), Times.Once);
            log4netMock.Verify(x => x.Warn($"Cycle {cycleId} wasn't inactivated", It.Is<Exception>(x => x is IncorrectCycleStateException)), Times.Once);

            cycleMock.VerifyNoOtherCalls();
            databaseMock.VerifyNoOtherCalls();
            log4netMock.VerifyNoOtherCalls();
        }

        [TestMethod]
        public void TestFinishPositive()
        {
            // Init
            string cycleId = "test";

            Mock<UserCycle> cycleMock = new Mock<UserCycle>();
            cycleMock.Setup(x => x.Finish()).Verifiable();
            cycleMock.SetupGet(x => x.CycleID).Returns(cycleId);
            databaseMock.Setup(x => x.UpdateCycle(cycleMock.Object)).Verifiable();

            // Test
            service.Finish(cycleMock.Object);

            // Verify
            cycleMock.Verify(x => x.Finish(), Times.Once);
            cycleMock.Verify(x => x.CycleID, Times.Once);
            databaseMock.Verify(x => x.UpdateCycle(cycleMock.Object), Times.Once);
            log4netMock.Verify(x => x.Info("Finishing cycle test"), Times.Once);

            cycleMock.VerifyNoOtherCalls();
            databaseMock.VerifyNoOtherCalls();
            log4netMock.VerifyNoOtherCalls();
        }

        [TestMethod]
        public void TestFinishNegative()
        {
            // Init
            Mock<UserCycle> cycleMock = new Mock<UserCycle>();
            cycleMock.Setup(x => x.Finish()).Throws<IncorrectCycleStateException>();
            cycleMock.SetupGet(x => x.CycleID).Returns("cycle0");

            // Test
           service.Finish(cycleMock.Object);

            // Verify
            cycleMock.Verify(x => x.Finish(), Times.Once);
            cycleMock.Verify(x => x.CycleID, Times.Exactly(2));
            log4netMock.Verify(x => x.Info("Finishing cycle cycle0"), Times.Once);
            log4netMock.Verify(x => x.Warn("Cycle cycle0 wasn't finished", It.Is<Exception>(x => x is IncorrectCycleStateException)), Times.Once);

            cycleMock.VerifyNoOtherCalls();
            databaseMock.VerifyNoOtherCalls();
            log4netMock.VerifyNoOtherCalls();
        }

        // TODO TestFinishUnfinishedLesson

        [TestMethod]
        public void TestSwapLessonPositiveNewIncomplete()
        {
            // Init
            Mock<UserCycle> cycleMock = new Mock<UserCycle>();
            Mock<IncompleteUserCycle> incompleteCycleMock = new Mock<IncompleteUserCycle>();
            Mock<LanguageProgramItem> languageItem1 = new Mock<LanguageProgramItem>();
            Mock<LanguageProgramItem> languageItem2 = new Mock<LanguageProgramItem>();
            Mock<UserProgramItem> userItemMock1 = new Mock<UserProgramItem>();
            Mock<UserProgramItem> userItemMock2 = new Mock<UserProgramItem>();

            userItemMock1.SetupGet(x => x.LessonRef).Returns(languageItem1.Object);
            languageItem1.SetupGet(x => x.Lesson).Returns("l1");
            userItemMock2.SetupGet(x => x.LessonRef).Returns(languageItem2.Object);
            languageItem2.SetupGet(x => x.Lesson).Returns("l2");
            cycleMock.SetupGet(x => x.Username).Returns("test");
            cycleMock.SetupGet(x => x.CycleID).Returns("test_id");
            cycleMock.Setup(x => x.SwapLesson(userItemMock2.Object)).Returns(userItemMock1.Object);
            incompleteCycleMock.SetupGet(x => x.CycleID).Returns("test_id_2");

            databaseMock.Setup(x => x.GetUserIncompleteCycle("test", "course_id")).Returns((IncompleteUserCycle)null);
            cycleFactoryMock.Setup(x => x.CreateIncompleteCycle("test", "course_id", 0)).Returns(incompleteCycleMock.Object);

            // Test
            service.SwapLesson(cycleMock.Object, userItemMock2.Object);

            // Verify
            languageItem1.Verify(x => x.Lesson, Times.Once);
            languageItem2.Verify(x => x.Lesson, Times.Once);
            userItemMock1.Verify(x => x.LessonRef, Times.Once);
            userItemMock2.Verify(x => x.LessonRef, Times.Once);
            languageItem1.VerifyNoOtherCalls();
            languageItem2.VerifyNoOtherCalls();
            userItemMock1.VerifyNoOtherCalls();
            userItemMock2.VerifyNoOtherCalls();

            cycleMock.Verify(x => x.CycleID, Times.Once);
            cycleMock.Verify(x => x.Username, Times.Once);
            cycleMock.Verify(x => x.SwapLesson(userItemMock2.Object), Times.Once);
            cycleMock.VerifyNoOtherCalls();

            incompleteCycleMock.Verify(x => x.CycleID, Times.Once);
            incompleteCycleMock.Verify(x => x.AddLesson(userItemMock1.Object), Times.Once);
            incompleteCycleMock.VerifyNoOtherCalls();

            cycleFactoryMock.Verify(x => x.CreateIncompleteCycle("test", "course_id", 0), Times.Once);
            cycleFactoryMock.VerifyNoOtherCalls();
            
            databaseMock.Verify(x => x.GetUserIncompleteCycle("test", "course_id"), Times.Once);
            databaseMock.Verify(x => x.PutCycle(incompleteCycleMock.Object), Times.Once);
            databaseMock.Verify(x => x.UpdateCycle(cycleMock.Object), Times.Once);
            databaseMock.Verify(x => x.UpdateCycle(incompleteCycleMock.Object), Times.Once);
            log4netMock.Verify(x => x.Info("Swapping lesson l2 to cycle test_id"), Times.Once);
            log4netMock.Verify(x => x.Debug("Looking if there is incomplete user cycle for user test"), Times.Once);
            log4netMock.Verify(x => x.Debug("No incomplete user cycle found creating new"), Times.Once);
            log4netMock.Verify(x => x.Info("Placing swapped lesson l1 to incomplete cycle test_id_2"), Times.Once);

            databaseMock.VerifyNoOtherCalls();
            log4netMock.VerifyNoOtherCalls();
        }

        [TestMethod]
        public void TestSwapLessonPositiveExistingIncomplete()
        {
            // Init
            Mock<IncompleteUserCycle> incompleteCycleMock = new Mock<IncompleteUserCycle>();
            Mock<UserCycle> cycleMock = new Mock<UserCycle>();
            Mock<LanguageProgramItem> languageItem1 = new Mock<LanguageProgramItem>();
            Mock<LanguageProgramItem> languageItem2 = new Mock<LanguageProgramItem>();
            Mock<UserProgramItem> userItemMock1 = new Mock<UserProgramItem>();
            Mock<UserProgramItem> userItemMock2 = new Mock<UserProgramItem>();

            cycleMock.SetupGet(x => x.CycleID).Returns("test_id");
            cycleMock.SetupGet(x => x.Username).Returns("test");
            cycleMock.Setup(x => x.SwapLesson(userItemMock2.Object)).Returns(userItemMock1.Object);
            incompleteCycleMock.SetupGet(x => x.CycleID).Returns("test_id_2");
            userItemMock1.SetupGet(x => x.LessonRef).Returns(languageItem1.Object);
            languageItem1.SetupGet(x => x.Lesson).Returns("l1");
            userItemMock2.SetupGet(x => x.LessonRef).Returns(languageItem2.Object);
            languageItem2.SetupGet(x => x.Lesson).Returns("l2");
            databaseMock.Setup(x => x.GetUserIncompleteCycle("test", "course_id")).Returns(incompleteCycleMock.Object);

            // Test
            service.SwapLesson(cycleMock.Object, userItemMock2.Object);

            // Verify
            languageItem1.Verify(x => x.Lesson, Times.Once);
            languageItem2.Verify(x => x.Lesson, Times.Once);
            userItemMock1.Verify(x => x.LessonRef, Times.Once);
            userItemMock2.Verify(x => x.LessonRef, Times.Once);
            languageItem1.VerifyNoOtherCalls();
            languageItem2.VerifyNoOtherCalls();
            userItemMock1.VerifyNoOtherCalls();
            userItemMock2.VerifyNoOtherCalls();

            cycleMock.Verify(x => x.CycleID, Times.Once);
            cycleMock.Verify(x => x.Username, Times.Once);
            cycleMock.Verify(x => x.SwapLesson(userItemMock2.Object), Times.Once);
            cycleMock.VerifyNoOtherCalls();

            incompleteCycleMock.Verify(x => x.CycleID, Times.Once);
            incompleteCycleMock.Verify(x => x.AddLesson(userItemMock1.Object), Times.Once);
            incompleteCycleMock.VerifyNoOtherCalls();

            databaseMock.Verify(x => x.UpdateCycle(cycleMock.Object), Times.Once);
            databaseMock.Verify(x => x.GetUserIncompleteCycle("test", "course_id"), Times.Once);
            databaseMock.Verify(x => x.UpdateCycle(incompleteCycleMock.Object), Times.Once);
            log4netMock.Verify(x => x.Info("Swapping lesson l2 to cycle test_id"), Times.Once);
            log4netMock.Verify(x => x.Debug("Looking if there is incomplete user cycle for user test"), Times.Once);
            log4netMock.Verify(x => x.Info("Placing swapped lesson l1 to incomplete cycle test_id_2"), Times.Once);

            cycleMock.Reset(); incompleteCycleMock.Reset(); // due to interactions among mocks

            databaseMock.VerifyNoOtherCalls();
            log4netMock.VerifyNoOtherCalls();
        }

        [TestMethod]
        public void TestGetNextLessonPositiveNewIncomplete()
        {
            // Init
            Mock<LanguageProgramItem> languageItemMock = new Mock<LanguageProgramItem>();
            languageItemMock.SetupGet(x => x.Lesson).Returns("testLesson");
            Mock<UserProgramItem> userItemMock = new Mock<UserProgramItem>();
            userItemMock.SetupGet(x => x.LessonRef).Returns(languageItemMock.Object);
            Mock<IncompleteUserCycle> cycleMock = new Mock<IncompleteUserCycle>();
            cycleMock.SetupGet(x => x.CycleID).Returns("test_id");
            cycleFactoryMock.Setup(x => x.CreateIncompleteCycle("test", "course_id", 0)).Returns(cycleMock.Object);

            plannerMock.Setup(x => x.GetNextLanguageLesson("test")).Returns(userItemMock.Object);
            databaseMock.Setup(x => x.GetUserIncompleteCycle("test", "course_id")).Returns((IncompleteUserCycle)null);

            // test
            string result = service.GetNextLesson("test");

            // verify
            Assert.AreEqual("testLesson", result);

            languageItemMock.Verify(x => x.Lesson, Times.Exactly(2));
            cycleMock.Verify(x => x.CycleID, Times.Once);
            cycleMock.Verify(x => x.AddLesson(userItemMock.Object), Times.Once);
            databaseMock.Verify(x => x.GetUserIncompleteCycle("test", "course_id"), Times.Once);
            databaseMock.Verify(x => x.PutCycle(cycleMock.Object), Times.Once);
            databaseMock.Verify(x => x.UpdateCycle(cycleMock.Object), Times.Once);
            log4netMock.Verify(x => x.Info("Getting next planned lesson"), Times.Once);
            log4netMock.Verify(x => x.Debug("Looking if there is incomplete user cycle for user test"), Times.Once);
            log4netMock.Verify(x => x.Debug("No incomplete user cycle found creating new"), Times.Once);
            log4netMock.Verify(x => x.Debug("Placing lesson testLesson to cycle test_id"), Times.Once);
            plannerMock.Verify(x => x.GetNextLanguageLesson("test"), Times.Once);
            cycleFactoryMock.Verify(x => x.CreateIncompleteCycle("test", "course_id", 0), Times.Once);

            languageItemMock.VerifyNoOtherCalls();
            userItemMock.VerifyNoOtherCalls();
            databaseMock.VerifyNoOtherCalls();
            log4netMock.VerifyNoOtherCalls();
            plannerMock.VerifyNoOtherCalls();
            cycleMock.VerifyNoOtherCalls();
            cycleFactoryMock.VerifyNoOtherCalls();
        }

        [TestMethod]
        public void TestGetNextLessonPositiveExistingIncomplete()
        {
            // Init
            Mock<LanguageProgramItem> languageItemMock = new Mock<LanguageProgramItem>();
            languageItemMock.SetupGet(x => x.Lesson).Returns("testLesson");
            Mock<UserProgramItem> userItemMock = new Mock<UserProgramItem>();
            userItemMock.SetupGet(x => x.LessonRef).Returns(languageItemMock.Object);
            Mock<IncompleteUserCycle> cycleMock = new Mock<IncompleteUserCycle>();
            cycleMock.SetupGet(x => x.CycleID).Returns("cycle0");

            plannerMock.Setup(x => x.GetNextLanguageLesson("test")).Returns(userItemMock.Object);
            databaseMock.Setup(x => x.GetUserIncompleteCycle("test", "course_id")).Returns(cycleMock.Object);

            // test
            string result = service.GetNextLesson("test");

            // verify
            Assert.AreEqual("testLesson", result);

            languageItemMock.Verify(x => x.Lesson, Times.Exactly(2));
            databaseMock.Verify(x => x.GetUserIncompleteCycle("test", "course_id"), Times.Once);
            databaseMock.Verify(x => x.UpdateCycle(cycleMock.Object), Times.Once);
            log4netMock.Verify(x => x.Info("Getting next planned lesson"), Times.Once);
            log4netMock.Verify(x => x.Debug("Looking if there is incomplete user cycle for user test"), Times.Once);
            log4netMock.Verify(x => x.Debug("Placing lesson testLesson to cycle cycle0"), Times.Once);
            cycleMock.Verify(x => x.CycleID, Times.Once);
            cycleMock.Verify(x => x.AddLesson(userItemMock.Object), Times.Once);
            plannerMock.Verify(x => x.GetNextLanguageLesson("test"), Times.Once);

            languageItemMock.VerifyNoOtherCalls();
            userItemMock.VerifyNoOtherCalls();
            cycleMock.VerifyNoOtherCalls();
            databaseMock.VerifyNoOtherCalls();
            log4netMock.VerifyNoOtherCalls();
            plannerMock.VerifyNoOtherCalls();
        }

        [TestMethod]
        public void TestGetPlannedUnfinishedLessonsPositive()
        {
            // Init
            Mock<LanguageProgramItem> languageItem1 = new();
            Mock<LanguageProgramItem> languageItem2 = new();
            languageItem1.SetupGet(x => x.Finished).Returns(true);
            languageItem2.SetupGet(x => x.Finished).Returns(false);

            Mock<UserProgramItem> userItem1 = new();
            Mock<UserProgramItem> userItem2 = new();
            userItem1.SetupGet(x => x.LessonRef).Returns(languageItem1.Object);
            userItem2.SetupGet(x => x.LessonRef).Returns(languageItem2.Object);

            Mock<UserCycle> cycleMock = new();
            cycleMock.SetupGet(x => x.UserProgramItems).Returns(new List<UserProgramItem>() { userItem1.Object, userItem2.Object });
            databaseMock.Setup(x => x.GetNotFinishedCycles("test", "course_id")).Returns(new List<UserCycle>() { cycleMock.Object });

            // Test
            List<UserProgramItem> result = service.GetPlannedUnfinishedLessons("test");

            // Verify
            CollectionAssert.AreEqual(new List<UserProgramItem>() { userItem2.Object }, result);

            databaseMock.Verify(x => x.GetNotFinishedCycles("test", "course_id"), Times.Once);
            languageItem1.Verify(x => x.Finished, Times.Once);
            languageItem2.Verify(x => x.Finished, Times.Once);
            userItem1.Verify(x => x.LessonRef, Times.Once);
            userItem2.Verify(x => x.LessonRef, Times.Once);
            cycleMock.Verify(x => x.UserProgramItems, Times.Once);
            log4netMock.Verify(x => x.Info("Getting unfinished planned lessons for user test"), Times.Once);

            languageItem1.VerifyNoOtherCalls();
            languageItem2.VerifyNoOtherCalls();
            userItem1.VerifyNoOtherCalls();
            userItem2.VerifyNoOtherCalls();
            cycleMock.VerifyNoOtherCalls();
            databaseMock.VerifyNoOtherCalls();
            log4netMock.VerifyNoOtherCalls();
        }

        [TestMethod]
        public void TestGetPlannedUnfinishedLessonsNegative()
        {
            // Init
            databaseMock.Setup(x => x.GetNotFinishedCycles("test", "course_id")).Returns(new List<UserCycle> { });

            // Test
            List<UserProgramItem> result = service.GetPlannedUnfinishedLessons("test");

            // Verify
            CollectionAssert.AreEqual(new List<UserProgramItem> {}, result);

            databaseMock.Verify(x => x.GetNotFinishedCycles("test", "course_id"), Times.Once);
            log4netMock.Verify(x => x.Info("Getting unfinished planned lessons for user test"), Times.Once);

            databaseMock.VerifyNoOtherCalls();
            log4netMock.VerifyNoOtherCalls();

        }

        [TestMethod]
        public void TestSetActiveCoursePositiveNoCacheReset()
        {
            // Init
            Mock<LanguageCourse> languageCourse = new();

            // Test
            service.SetActiveCourse("test", languageCourse.Object, false);

            // Verify
            Assert.AreEqual(languageCourse.Object, service.ActiveCourse);
            plannerMock.Verify(x => x.SetPlanner(languageCourse.Object, "test"), Times.Once);

            languageCourse.VerifyNoOtherCalls();
            plannerMock.VerifyNoOtherCalls();
        }

        [TestMethod]
        public void TestSetActiveCoursePositiveWithCacheReset()
        {
            // Init
            Mock<UserCycle> cycleMock = new();
            cycleMock.SetupGet(x => x.State).Returns(UserCycleState.Inactive);
            Mock<LanguageCourse> languageCourse = new();
            languageCourse.SetupGet(x => x.CourseId).Returns("course_id");
            databaseMock.Setup(x => x.GetOldestUserInactiveCycle("test", "course_id")).Returns(cycleMock.Object);

            // Test
            service.SetActiveCourse("test", languageCourse.Object, true);

            // Verify
            Assert.AreEqual(languageCourse.Object, service.ActiveCourse);
            plannerMock.Verify(x => x.SetPlanner(languageCourse.Object, "test"), Times.Once);
            cycleMock.Verify(x => x.CycleID, Times.Exactly(2));
            cycleMock.Verify(x => x.Activate(), Times.Once);
            languageCourse.Verify(x => x.CourseId, Times.Once);
            databaseMock.Verify(x => x.UpdateCycle(cycleMock.Object), Times.Once);

            cycleMock.VerifyNoOtherCalls();
            languageCourse.VerifyNoOtherCalls();
            plannerMock.VerifyNoOtherCalls();
            databaseMock.VerifyNoOtherCalls();
        }

        [TestMethod]
        public void TestUpdatePositiveUnfinished()
        {
            // Init
            Mock<UserCycle> userCycle = new();
            userCycle.Setup(x => x.AreAllFinished()).Returns(false);

            // Test
            UserCycle result = service.Update(userCycle.Object);

            // Verify
            Assert.AreSame(userCycle.Object, result);

            userCycle.Verify(x => x.Update(), Times.Once);
            userCycle.Verify(x => x.AreAllFinished(), Times.Once);
            databaseMock.Verify(x => x.UpdateCycle(userCycle.Object), Times.Once);

            userCycle.VerifyNoOtherCalls();
            databaseMock.VerifyNoOtherCalls();
        }

        // TODO TestUpdatePositiveFinished

        [TestCleanup]
        public void TestCleanUp()
        {
            CycleService.DeallocateInstance();
            Directory.Delete("./cycles", true);
        }
    }
}
