using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using Uceni_jazyku.Cycles;
using Uceni_jazyku.Cycles.Program;
using Uceni_jazyku.Cycles.UserCycles;
using Uceni_jazyku.Cycles.LanguageCycles;
using System.Collections.Generic;
using Moq;
using log4net;
using System.Reflection;
using Uceni_jazyku.Planner;

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
        Mock<IActiveCycleCache> cacheMock;
        Mock<IProgramPlanner> plannerMock;
        UserCycle cycle1, cycle2;
        UserProgramItem item1, item2, item3, item4;
        IncompleteUserCycle newIncomplete, existingIncomplete;
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
            cacheMock = new Mock<IActiveCycleCache>();
            plannerMock = new Mock<IProgramPlanner>();
            service = CycleService.GetInstance(databaseMock.Object, cacheMock.Object, plannerMock.Object);
            log4netMock.Reset();

            LanguageCycle example = LanguageCycle.LanguageCycleExample();
            item1 = new UserProgramItem(example.CycleID, example.PlanNext());
            item2 = new UserProgramItem(example.CycleID, example.PlanNext());
            item3 = new UserProgramItem(example.CycleID, example.PlanNext());
            item4 = new UserProgramItem(example.CycleID, example.PlanNext());

            newIncomplete = new IncompleteUserCycle("test");
            newIncomplete.AddLesson(item2);
            newIncomplete.CycleID = "cycle0";
            existingIncomplete = new IncompleteUserCycle("test");
            existingIncomplete.AddLesson(item4);
            cycle1 = new UserCycle().AssignUser("test").AssignProgram(new List<UserProgramItem>() { item1, item2 }).Activate();
            cycle2 = new UserCycle().AssignUser("test").AssignProgram(new List<UserProgramItem>() { item1, item2 });
        }

        [TestMethod]
        public void TestActiveCycleExistsPositive()
        {
            // Init
            cacheMock.Setup(x => x.IsCacheFilled()).Returns(true);

            // Test
            bool result = service.UserActiveCycleExists();

            // Verify
            Assert.IsTrue(result);

            cacheMock.Verify(x => x.IsCacheFilled(), Times.Once);
            log4netMock.Verify(x => x.Info("Looking into cache if there is user active cycle present"), Times.Once);

            databaseMock.VerifyNoOtherCalls();
            cacheMock.VerifyNoOtherCalls();
            log4netMock.VerifyNoOtherCalls();
        }

        [TestMethod]
        public void TestActiveCycleExistsNegative()
        {
            // Init
            cacheMock.Setup(x => x.IsCacheFilled()).Returns(false);

            // Test
            bool result = service.UserActiveCycleExists();

            // Verify
            Assert.IsFalse(result);

            cacheMock.Verify(x => x.IsCacheFilled(), Times.Once);
            log4netMock.Verify(x => x.Info("Looking into cache if there is user active cycle present"), Times.Once);

            databaseMock.VerifyNoOtherCalls();
            cacheMock.VerifyNoOtherCalls();
            log4netMock.VerifyNoOtherCalls();
        }

        [TestMethod]
        public void TestGetUserCyclePositiveCreateNew()
        {
            // Init
            UserCycle userCycle = new UserCycle().AssignUser("test"); // TODO should be outside of test
            userCycle.CycleID = "cycle1";
            Mock<List<UserProgramItem>> programMock = new Mock<List<UserProgramItem>>();
            databaseMock.Setup(x => x.GetOldestUserInactiveCycle("test")).Returns((UserCycle)null);
            databaseMock.Setup(x => x.UpdateCycle(userCycle)).Verifiable();
            plannerMock.Setup(x => x.getNextUserCycleProgram("testUser")).Returns(programMock.Object);

            // Test
            UserCycle result = service.GetUserCycle("test");

            // Verify
            Assert.AreEqual(UserCycleState.Active, result.State);
            Assert.AreEqual("test", result.Username);
            // Assert.AreSame(programMock.Object, userCycle.UserProgramItems); // TODO should be testable
            databaseMock.Verify(x => x.GetOldestUserInactiveCycle("test"), Times.Once);
            databaseMock.Verify(x => x.PutCycle(It.IsAny<AbstractCycle>()), Times.Once);
            databaseMock.Verify(x => x.GetCyclesCount(), Times.Once);
            databaseMock.Verify(x => x.UpdateCycle(result), Times.Once);
            cacheMock.Verify(x => x.InsertToCache(result), Times.Once);
            plannerMock.Verify(x => x.getNextUserCycleProgram("test"), Times.Once);

            log4netMock.Verify(x => x.Info("Getting cycle for user test"), Times.Once);
            log4netMock.Verify(x => x.Debug("Looking if there is existing inactive cycle for user test"), Times.Once);
            log4netMock.Verify(x => x.Debug("No cycle found, new must be created and activated"), Times.Once);
            log4netMock.Verify(x => x.Info("Creating new cycle for user test"), Times.Once);
            log4netMock.Verify(x => x.Debug("Generating cycleID"), Times.Once);
            log4netMock.Verify(x => x.Debug("New cycle created with id cycle0"), Times.Once);
            log4netMock.Verify(x => x.Info("Activating cycle cycle0"), Times.Once);
            log4netMock.Verify(x => x.Debug("Obtaining cycle program"), Times.Once);
            log4netMock.Verify(x => x.Info("Assigning program to cycle cycle0"), Times.Once);

            databaseMock.VerifyNoOtherCalls();
            cacheMock.VerifyNoOtherCalls();
            log4netMock.VerifyNoOtherCalls();
            plannerMock.VerifyNoOtherCalls();
        }

        [TestMethod]
        public void TestGetUserCyclePositiveFromInactive()
        {
            // Init
            Mock<UserCycle> cycleMock = new Mock<UserCycle>();
            cycleMock.SetupGet(x => x.State).Returns(UserCycleState.Inactive);
            cycleMock.SetupGet(x => x.CycleID).Returns("testId");
            cycleMock.Setup(x => x.Activate()).Returns(cycleMock.Object);
            databaseMock.Setup(x => x.GetOldestUserInactiveCycle("test")).Returns(cycleMock.Object);
            databaseMock.Setup(x => x.UpdateCycle(cycleMock.Object)).Verifiable();
            cacheMock.Setup(x => x.InsertToCache(cycleMock.Object)).Verifiable();

            // Test
            UserCycle result = service.GetUserCycle("test");

            // Verify
            Assert.AreSame(cycleMock.Object, result);

            databaseMock.Verify(x => x.GetOldestUserInactiveCycle("test"), Times.Once);
            cycleMock.Verify(x => x.State, Times.Once);
            cycleMock.Verify(x => x.CycleID, Times.Exactly(2));
            cycleMock.Verify(x => x.Activate(), Times.Once);
            databaseMock.Verify(x => x.UpdateCycle(cycleMock.Object), Times.Once);
            cacheMock.Verify(x => x.InsertToCache(cycleMock.Object), Times.Once);
            log4netMock.Verify(x => x.Info("Getting cycle for user test"), Times.Once);
            log4netMock.Verify(x => x.Debug("Looking if there is existing inactive cycle for user test"), Times.Once);
            log4netMock.Verify(x => x.Debug("Obtained testId"), Times.Once);
            log4netMock.Verify(x => x.Info("Activating cycle testId"), Times.Once);

            cacheMock.VerifyNoOtherCalls();
            databaseMock.VerifyNoOtherCalls();
            cycleMock.VerifyNoOtherCalls();
            log4netMock.VerifyNoOtherCalls();
        }

        [TestMethod]
        public void TestGetActiveCycleNegative()
        {
            // Init
            cacheMock.Setup(x => x.GetFromCache()).Returns((UserCycle)null);

            // Test
            UserCycle result = service.GetActiveCycle();

            // Verify
            Assert.IsNull(result);
            cacheMock.Verify(x => x.GetFromCache(), Times.Once);
            log4netMock.Verify(x => x.Info("Retriving user active cycle from cache"), Times.Once);

            databaseMock.VerifyNoOtherCalls();
            cacheMock.VerifyNoOtherCalls();
            log4netMock.VerifyNoOtherCalls();
        }

        [TestMethod]
        public void TestGetActiveCyclePositive()
        {
            // Init
            Mock<UserCycle> cycleMock = new Mock<UserCycle>();
            cacheMock.Setup(x => x.GetFromCache()).Returns(cycleMock.Object);

            // Test
            UserCycle result = service.GetActiveCycle();

            // Verify
            Assert.AreEqual(cycleMock.Object, result);
            cacheMock.Verify(x => x.GetFromCache(), Times.Once);
            log4netMock.Verify(x => x.Info("Retriving user active cycle from cache"), Times.Once);

            cycleMock.VerifyNoOtherCalls();
            databaseMock.VerifyNoOtherCalls();
            cacheMock.VerifyNoOtherCalls();
            log4netMock.VerifyNoOtherCalls();
        }

        [TestMethod]
        public void TestGetNewCyclePositive()
        {
            // Init
            databaseMock.Setup(x => x.PutCycle(It.IsAny<UserCycle>())).Verifiable(); 

            // Test
            UserCycle result = service.GetNewCycle("test");

            // Verify
            Assert.AreEqual(UserCycleState.New, result.State);
            Assert.AreEqual("test", result.Username);
            databaseMock.Verify(x => x.PutCycle(result), Times.Once);
            databaseMock.Verify(x => x.GetCyclesCount(), Times.Once);
            log4netMock.Verify(x => x.Info("Creating new cycle for user test"), Times.Once);
            log4netMock.Verify(x => x.Debug("Generating cycleID"), Times.Once);
            log4netMock.Verify(x => x.Debug("New cycle created with id cycle0"), Times.Once);

            databaseMock.VerifyNoOtherCalls();
            cacheMock.VerifyNoOtherCalls();
            log4netMock.VerifyNoOtherCalls();
        }

        [TestMethod]
        public void TestActivatePositiveNewCycle()
        {
            //Init
            string cycleId = "test";

            Mock<List<UserProgramItem>> programMock = new Mock<List<UserProgramItem>>();
            Mock<UserCycle> cycleMock = new Mock<UserCycle>();
            cycleMock.SetupGet(x => x.State).Returns(UserCycleState.New);
            cycleMock.SetupGet(x => x.CycleID).Returns(cycleId);
            cycleMock.SetupGet(x => x.Username).Returns("testUser");
            cycleMock.Setup(x => x.AssignProgram(programMock.Object)).Returns(cycleMock.Object);
            cycleMock.Setup(x => x.Activate()).Returns(cycleMock.Object);
            databaseMock.Setup(x => x.PutCycle(It.IsAny<AbstractCycle>())).Verifiable();
            databaseMock.Setup(x => x.UpdateCycle(cycleMock.Object)).Verifiable();
            cacheMock.Setup(x => x.InsertToCache(cycleMock.Object)).Verifiable();
            plannerMock.Setup(x => x.getNextUserCycleProgram("testUser")).Returns(programMock.Object);

            // Test
            UserCycle result = service.Activate(cycleMock.Object);

            // Verify
            Assert.AreSame(cycleMock.Object, result);

            cycleMock.Verify(x => x.State, Times.Once);
            cycleMock.Verify(x => x.CycleID, Times.Exactly(2));
            cycleMock.Verify(x => x.AssignProgram(programMock.Object), Times.Once);
            cycleMock.Verify(x => x.Activate(), Times.Once);
            cycleMock.Verify(x => x.Username, Times.Once);
            databaseMock.Verify(x => x.UpdateCycle(cycleMock.Object), Times.Once);
            cacheMock.Verify(x => x.InsertToCache(cycleMock.Object), Times.Once);
            plannerMock.Verify(x => x.getNextUserCycleProgram("testUser"), Times.Once);
            log4netMock.Verify(x => x.Info($"Activating cycle {cycleId}"), Times.Once);
            log4netMock.Verify(x => x.Debug($"Obtaining cycle program"), Times.Once);
            log4netMock.Verify(x => x.Info($"Assigning program to cycle {cycleId}"), Times.Once);

            cycleMock.VerifyNoOtherCalls();
            databaseMock.VerifyNoOtherCalls();
            cacheMock.VerifyNoOtherCalls();
            plannerMock.VerifyNoOtherCalls();
            log4netMock.VerifyNoOtherCalls();
        }

        [TestMethod]
        public void TestActivatePositiveInactiveCycle()
        {
            // Init
            string cycleId = "test";

            Mock<UserCycle> cycleMock = new Mock<UserCycle>();
            cycleMock.SetupGet(x => x.State).Returns(UserCycleState.Inactive);
            cycleMock.SetupGet(x => x.CycleID).Returns(cycleId);
            cycleMock.Setup(x => x.Activate()).Returns(cycleMock.Object);
            databaseMock.Setup(x => x.UpdateCycle(cycleMock.Object)).Verifiable();
            cacheMock.Setup(x => x.InsertToCache(cycleMock.Object)).Verifiable();

            // Test
            UserCycle result = service.Activate(cycleMock.Object);

            // Verify
            Assert.AreSame(cycleMock.Object, result);

            cycleMock.Verify(x => x.State, Times.Once);
            cycleMock.Verify(x => x.CycleID, Times.Once);
            cycleMock.Verify(x => x.Activate(), Times.Once);
            databaseMock.Verify(x => x.UpdateCycle(cycleMock.Object), Times.Once);
            cacheMock.Verify(x => x.InsertToCache(cycleMock.Object), Times.Once);
            log4netMock.Verify(x => x.Info($"Activating cycle {cycleId}"), Times.Once);

            cycleMock.VerifyNoOtherCalls();
            databaseMock.VerifyNoOtherCalls();
            cacheMock.VerifyNoOtherCalls();
            log4netMock.VerifyNoOtherCalls();
        }

        [DataRow(UserCycleState.UnknownUser)]
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
            cycleMock.Verify(x => x.State, Times.Once);
            cycleMock.Verify(x => x.CycleID, Times.Exactly(2));
            cycleMock.Verify(x => x.Activate(), Times.Once);
            log4netMock.Verify(x => x.Info("Activating cycle test"), Times.Once);
            log4netMock.Verify(x => x.Warn($"Cycle {cycleId} wasn't activated", It.Is<Exception>(x => x is IncorrectCycleStateException)), Times.Once);

            cycleMock.VerifyNoOtherCalls();
            databaseMock.VerifyNoOtherCalls();
            cacheMock.VerifyNoOtherCalls();
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
            cacheMock.Setup(x => x.DropCache()).Verifiable();

            // Test
            UserCycle result = service.Inactivate(cycleMock.Object);

            // Verify
            Assert.AreSame(cycleMock.Object, result);

            cycleMock.Verify(x => x.Inactivate(), Times.Once);
            cycleMock.Verify(x => x.CycleID, Times.Once);
            databaseMock.Verify(x => x.UpdateCycle(cycleMock.Object), Times.Once);
            cacheMock.Verify(x => x.DropCache(), Times.Once);
            log4netMock.Verify(x => x.Info("Inactivating cycle test"), Times.Once);

            cycleMock.VerifyNoOtherCalls();
            databaseMock.VerifyNoOtherCalls();
            cacheMock.VerifyNoOtherCalls();
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
            cacheMock.VerifyNoOtherCalls();
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
            cacheMock.Setup(x => x.DropCache()).Verifiable();
            databaseMock.Setup(x => x.UpdateCycle(cycleMock.Object)).Verifiable();

            // Test
            service.Finish(cycleMock.Object);

            // Verify
            cacheMock.Verify(x => x.DropCache(), Times.Once);
            cycleMock.Verify(x => x.Finish(), Times.Once);
            cycleMock.Verify(x => x.CycleID, Times.Once);
            databaseMock.Verify(x => x.UpdateCycle(cycleMock.Object), Times.Once);
            log4netMock.Verify(x => x.Info("Finishing cycle test"), Times.Once);

            cycleMock.VerifyNoOtherCalls();
            databaseMock.VerifyNoOtherCalls();
            cacheMock.VerifyNoOtherCalls();
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
            cacheMock.VerifyNoOtherCalls();
            log4netMock.VerifyNoOtherCalls();
        }

        // TODO TestFinishUnfinishedLesson

        [TestMethod]
        public void TestRegisterCyclePositive()
        {
            // Init
            Mock<UserCycle> cycleMock = new Mock<UserCycle>();
            cycleMock.SetupSet(x => x.CycleID = "cycle0").Verifiable();
            cycleMock.Setup(x => x.CycleID).Returns("cycle0");
            databaseMock.Setup(x => x.GetCyclesCount()).Returns(0);
            databaseMock.Setup(x => x.PutCycle(cycleMock.Object)).Verifiable();

            // Test
            service.RegisterCycle(cycleMock.Object);

            // Verify
            cycleMock.VerifySet(x => x.CycleID = "cycle0", Times.Once);
            cycleMock.Verify(x => x.CycleID, Times.Once);
            databaseMock.Verify(x => x.PutCycle(cycleMock.Object), Times.Once);
            databaseMock.Verify(x => x.GetCyclesCount(), Times.Once);
            log4netMock.Verify(x => x.Info("Registering cycle"), Times.Once);
            log4netMock.Verify(x => x.Debug("Generating cycleID"), Times.Once);
            log4netMock.Verify(x => x.Debug("Registered cycle got id cycle0"), Times.Once);

            cycleMock.VerifyNoOtherCalls();
            databaseMock.VerifyNoOtherCalls();
            cacheMock.VerifyNoOtherCalls();
            log4netMock.VerifyNoOtherCalls();
        }

        [TestMethod]
        public void TestSwapLessonPositiveNewIncomplete()
        {
            // Init
            databaseMock.Setup(x => x.GetUserIncompleteCycle("test")).Returns((IncompleteUserCycle)null);
            databaseMock.Setup(x => x.GetCyclesCount()).Returns(0);
            databaseMock.Setup(x => x.PutCycle(newIncomplete)).Verifiable();
            databaseMock.Setup(x => x.UpdateCycle(cycle1)).Verifiable();
            databaseMock.Setup(x => x.UpdateCycle(newIncomplete)).Verifiable();
            cacheMock.Setup(x => x.InsertToCache(cycle1)).Verifiable();

            // Preverify
            CollectionAssert.AreEqual(new List<UserProgramItem>() { item1, item2 }, cycle1.UserProgramItems);

            // Test
            service.SwapLesson(cycle1, item3);

            // Verify
            CollectionAssert.AreEqual(new List<UserProgramItem>() { item3, item1 }, cycle1.UserProgramItems);
            CollectionAssert.AreEqual(new List<UserProgramItem>() { item2 }, newIncomplete.UserProgramItems);

            databaseMock.Verify(x => x.GetUserIncompleteCycle("test"), Times.Once);
            databaseMock.Verify(x => x.GetCyclesCount(), Times.Once);
            databaseMock.Verify(x => x.PutCycle(newIncomplete), Times.Once);
            databaseMock.Verify(x => x.UpdateCycle(cycle1), Times.Once);
            databaseMock.Verify(x => x.UpdateCycle(newIncomplete), Times.Once);
            cacheMock.Verify(x => x.InsertToCache(cycle1), Times.Once);
            log4netMock.Verify(x => x.Info("Swapping lesson in cycle "), Times.Once);
            log4netMock.Verify(x => x.Info("Placing swapped lesson to incomplete cycle"), Times.Once);
            log4netMock.Verify(x => x.Debug("Looking if there is incomplete user cycle for user test"), Times.Once);
            log4netMock.Verify(x => x.Debug("No incomplete user cycle found creating new"), Times.Once);
            log4netMock.Verify(x => x.Info("Registering cycle"), Times.Once);
            log4netMock.Verify(x => x.Debug("Generating cycleID"), Times.Once);
            log4netMock.Verify(x => x.Debug("Registered cycle got id cycle0"), Times.Once);

            databaseMock.VerifyNoOtherCalls();
            cacheMock.VerifyNoOtherCalls();
            log4netMock.VerifyNoOtherCalls();
        }

        [TestMethod]
        public void TestSwapLessonPositiveExistingIncomplete()
        {
            // Init
            databaseMock.Setup(x => x.GetUserIncompleteCycle("test")).Returns(existingIncomplete);
            databaseMock.Setup(x => x.UpdateCycle(cycle2)).Verifiable();
            databaseMock.Setup(x => x.UpdateCycle(newIncomplete)).Verifiable();

            // Preverify
            CollectionAssert.AreEqual(new List<UserProgramItem>() { item1, item2 }, cycle2.UserProgramItems);
            CollectionAssert.AreEqual(new List<UserProgramItem>() { item4 }, existingIncomplete.UserProgramItems);

            // Test
            service.SwapLesson(cycle2, item3);

            // Verify
            CollectionAssert.AreEqual(new List<UserProgramItem>() { item3, item1 }, cycle2.UserProgramItems);
            CollectionAssert.AreEqual(new List<UserProgramItem>() { item2, item4 }, existingIncomplete.UserProgramItems);

            databaseMock.Verify(x => x.GetUserIncompleteCycle("test"), Times.Once);
            databaseMock.Verify(x => x.UpdateCycle(cycle2), Times.Once);
            databaseMock.Verify(x => x.UpdateCycle(existingIncomplete), Times.Once);
            log4netMock.Verify(x => x.Info("Swapping lesson in cycle "), Times.Once);
            log4netMock.Verify(x => x.Info("Placing swapped lesson to incomplete cycle"), Times.Once);
            log4netMock.Verify(x => x.Debug("Looking if there is incomplete user cycle for user test"), Times.Once);

            databaseMock.VerifyNoOtherCalls();
            cacheMock.VerifyNoOtherCalls();
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

            plannerMock.Setup(x => x.getNextLanguageLesson("test")).Returns(userItemMock.Object);
            databaseMock.Setup(x => x.GetUserIncompleteCycle("test")).Returns((IncompleteUserCycle)null);
            databaseMock.Setup(x => x.GetCyclesCount()).Returns(0);

            // test
            string result = service.GetNextLesson("test");

            // verify
            Assert.AreEqual("testLesson", result);

            languageItemMock.Verify(x => x.Lesson, Times.Exactly(2));
            databaseMock.Verify(x => x.GetUserIncompleteCycle("test"), Times.Once);
            databaseMock.Verify(x => x.GetCyclesCount(), Times.Once);
            databaseMock.Verify(x => x.PutCycle(It.IsAny<IncompleteUserCycle>()), Times.Once);
            databaseMock.Verify(x => x.UpdateCycle(It.IsAny<IncompleteUserCycle>()), Times.Once);
            log4netMock.Verify(x => x.Info("Getting next planned lesson"), Times.Once);
            log4netMock.Verify(x => x.Debug("Looking if there is incomplete user cycle for user test"), Times.Once);
            log4netMock.Verify(x => x.Debug("No incomplete user cycle found creating new"), Times.Once);
            log4netMock.Verify(x => x.Info("Registering cycle"), Times.Once);
            log4netMock.Verify(x => x.Debug("Generating cycleID"), Times.Once);
            log4netMock.Verify(x => x.Debug("Registered cycle got id cycle0"), Times.Once);
            log4netMock.Verify(x => x.Debug("Placing lesson testLesson to cycle cycle0"), Times.Once);
            plannerMock.Verify(x => x.getNextLanguageLesson("test"), Times.Once);

            languageItemMock.VerifyNoOtherCalls();
            userItemMock.VerifyNoOtherCalls();
            databaseMock.VerifyNoOtherCalls();
            cacheMock.VerifyNoOtherCalls();
            log4netMock.VerifyNoOtherCalls();
            plannerMock.VerifyNoOtherCalls();
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

            plannerMock.Setup(x => x.getNextLanguageLesson("test")).Returns(userItemMock.Object);
            databaseMock.Setup(x => x.GetUserIncompleteCycle("test")).Returns(cycleMock.Object);

            // test
            string result = service.GetNextLesson("test");

            // verify
            Assert.AreEqual("testLesson", result);

            languageItemMock.Verify(x => x.Lesson, Times.Exactly(2));
            databaseMock.Verify(x => x.GetUserIncompleteCycle("test"), Times.Once);
            databaseMock.Verify(x => x.UpdateCycle(cycleMock.Object), Times.Once);
            log4netMock.Verify(x => x.Info("Getting next planned lesson"), Times.Once);
            log4netMock.Verify(x => x.Debug("Looking if there is incomplete user cycle for user test"), Times.Once);
            log4netMock.Verify(x => x.Debug("Placing lesson testLesson to cycle cycle0"), Times.Once);
            cycleMock.Verify(x => x.CycleID, Times.Once);
            cycleMock.Verify(x => x.AddLesson(userItemMock.Object), Times.Once);
            plannerMock.Verify(x => x.getNextLanguageLesson("test"), Times.Once);

            languageItemMock.VerifyNoOtherCalls();
            userItemMock.VerifyNoOtherCalls();
            cycleMock.VerifyNoOtherCalls();
            databaseMock.VerifyNoOtherCalls();
            cacheMock.VerifyNoOtherCalls();
            log4netMock.VerifyNoOtherCalls();
            plannerMock.VerifyNoOtherCalls();
        }

        [TestCleanup]
        public void TestCleanUp()
        {
            CycleService.DeallocateInstance();
            Directory.Delete("./cycles", true);
        }
    }
}
