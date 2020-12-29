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
            service = CycleService.GetInstance(databaseMock.Object, cacheMock.Object);
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

            databaseMock.VerifyNoOtherCalls();
            cacheMock.VerifyNoOtherCalls();
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

            databaseMock.VerifyNoOtherCalls();
            cacheMock.VerifyNoOtherCalls();
        }

        [TestMethod]
        public void TestGetUserCyclePositiveCreateNew()
        {
            // Init
            UserCycle userCycle = new UserCycle().AssignUser("test"); // TODO should be outside of test
            userCycle.CycleID = "cycle1";
            databaseMock.Setup(x => x.GetOldestUserInactiveCycle("test")).Returns((UserCycle)null);
            databaseMock.Setup(x => x.UpdateCycle(userCycle)).Verifiable();

            // Test
            UserCycle result = service.GetUserCycle("test");

            // Verify
            Assert.AreEqual(UserCycleState.Active, result.State);
            Assert.AreEqual("test", result.Username);
            databaseMock.Verify(x => x.GetOldestUserInactiveCycle("test"), Times.Once);
            databaseMock.Verify(x => x.PutCycle(It.IsAny<AbstractCycle>()), Times.Exactly(2)); // result + language cycle, TODO language cycle will go away
            databaseMock.Verify(x => x.GetCyclesCount(), Times.Exactly(2));
            databaseMock.Verify(x => x.UpdateCycle(result), Times.Once);
            cacheMock.Verify(x => x.InsertToCache(result), Times.Once);

            databaseMock.VerifyNoOtherCalls();
            cacheMock.VerifyNoOtherCalls();
        }

        [TestMethod]
        public void TestGetUserCyclePositiveFromInactive()
        {
            // Init
            Mock<UserCycle> cycleMock = new Mock<UserCycle>();
            cycleMock.SetupGet(x => x.State).Returns(UserCycleState.Inactive);
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
            cycleMock.Verify(x => x.Activate(), Times.Once);
            databaseMock.Verify(x => x.UpdateCycle(cycleMock.Object), Times.Once);
            cacheMock.Verify(x => x.InsertToCache(cycleMock.Object), Times.Once);

            cacheMock.VerifyNoOtherCalls();
            databaseMock.VerifyNoOtherCalls();
            cycleMock.VerifyNoOtherCalls();
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

            databaseMock.VerifyNoOtherCalls();
            cacheMock.VerifyNoOtherCalls();
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

            cycleMock.VerifyNoOtherCalls();
            databaseMock.VerifyNoOtherCalls();
            cacheMock.VerifyNoOtherCalls();
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

            databaseMock.VerifyNoOtherCalls();
            cacheMock.VerifyNoOtherCalls();
        }

        [TestMethod]
        public void TestActivatePositiveNewCycle()
        {
            //Init
            Mock<UserCycle> cycleMock = new Mock<UserCycle>();
            cycleMock.SetupGet(x => x.State).Returns(UserCycleState.New);
            cycleMock.Setup(x => x.AssignProgram(It.IsAny<List<UserProgramItem>>())).Returns(cycleMock.Object);
            cycleMock.Setup(x => x.Activate()).Returns(cycleMock.Object);
            databaseMock.Setup(x => x.PutCycle(It.IsAny<AbstractCycle>())).Verifiable();
            databaseMock.Setup(x => x.UpdateCycle(cycleMock.Object)).Verifiable();
            cacheMock.Setup(x => x.InsertToCache(cycleMock.Object)).Verifiable();

            // Test
            UserCycle result = service.Activate(cycleMock.Object);

            // Verify
            Assert.AreSame(cycleMock.Object, result);

            cycleMock.Verify(x => x.State, Times.Once);
            cycleMock.Verify(x => x.AssignProgram(It.IsAny<List<UserProgramItem>>()), Times.Once);
            cycleMock.Verify(x => x.Activate(), Times.Once);
            databaseMock.Verify(x => x.GetCyclesCount(), Times.Once);
            databaseMock.Verify(x => x.PutCycle(It.IsAny<AbstractCycle>()), Times.Once);
            databaseMock.Verify(x => x.UpdateCycle(cycleMock.Object), Times.Once);
            cacheMock.Verify(x => x.InsertToCache(cycleMock.Object), Times.Once);

            cycleMock.VerifyNoOtherCalls();
            databaseMock.VerifyNoOtherCalls();
            cacheMock.VerifyNoOtherCalls();
        }

        [TestMethod]
        public void TestActivatePositiveInactiveCycle()
        {
            // Init
            Mock<UserCycle> cycleMock = new Mock<UserCycle>();
            cycleMock.SetupGet(x => x.State).Returns(UserCycleState.Inactive);
            cycleMock.Setup(x => x.Activate()).Returns(cycleMock.Object);
            databaseMock.Setup(x => x.UpdateCycle(cycleMock.Object)).Verifiable();
            cacheMock.Setup(x => x.InsertToCache(cycleMock.Object)).Verifiable();

            // Test
            UserCycle result = service.Activate(cycleMock.Object);

            // Verify
            Assert.AreSame(cycleMock.Object, result);

            cycleMock.Verify(x => x.State, Times.Once);
            cycleMock.Verify(x => x.Activate(), Times.Once);
            databaseMock.Verify(x => x.UpdateCycle(cycleMock.Object), Times.Once);
            cacheMock.Verify(x => x.InsertToCache(cycleMock.Object), Times.Once);

            cycleMock.VerifyNoOtherCalls();
            databaseMock.VerifyNoOtherCalls();
            cacheMock.VerifyNoOtherCalls();
        }

        [DataRow(UserCycleState.UnknownUser)]
        [DataRow(UserCycleState.Active)]
        [DataRow(UserCycleState.Finished)]
        [DataTestMethod]
        public void TestActivateNegative(UserCycleState incorrectState)
        {
            // Init
            Mock<UserCycle> cycleMock = new Mock<UserCycle>();
            cycleMock.SetupGet(x => x.State).Returns(incorrectState);
            cycleMock.Setup(x => x.Activate()).Throws<Exception>();

            // Test & Verify
            Assert.ThrowsException<Exception>(() => service.Activate(cycleMock.Object));

            cycleMock.Verify(x => x.State, Times.Once);
            cycleMock.Verify(x => x.Activate(), Times.Once);

            cycleMock.VerifyNoOtherCalls();
            databaseMock.VerifyNoOtherCalls();
            cacheMock.VerifyNoOtherCalls();
        }

        [TestMethod]
        public void TestInactivatePositive()
        {
            // Init
            Mock<UserCycle> cycleMock = new Mock<UserCycle>();
            cycleMock.Setup(x => x.Inactivate()).Returns(cycleMock.Object);
            databaseMock.Setup(x => x.UpdateCycle(cycleMock.Object)).Verifiable();
            cacheMock.Setup(x => x.DropCache()).Verifiable();

            // Test
            UserCycle result = service.Inactivate(cycleMock.Object);

            // Verify
            Assert.AreSame(cycleMock.Object, result);

            cycleMock.Verify(x => x.Inactivate(), Times.Once);
            databaseMock.Verify(x => x.UpdateCycle(cycleMock.Object), Times.Once);
            cacheMock.Verify(x => x.DropCache(), Times.Once);

            cycleMock.VerifyNoOtherCalls();
            databaseMock.VerifyNoOtherCalls();
            cacheMock.VerifyNoOtherCalls();
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
            log4netMock.Verify(x => x.Error($"Cycle {cycleId} wasn't inactivated", It.Is<Exception>(x => x is IncorrectCycleStateException)), Times.Once);

            cycleMock.VerifyNoOtherCalls();
            databaseMock.VerifyNoOtherCalls();
            cacheMock.VerifyNoOtherCalls();
            log4netMock.VerifyNoOtherCalls();
        }

        [TestMethod]
        public void TestFinishPositive()
        {
            // Init
            Mock<UserCycle> cycleMock = new Mock<UserCycle>();
            cycleMock.Setup(x => x.Finish()).Verifiable(); // may be call base
            cacheMock.Setup(x => x.DropCache()).Verifiable();
            databaseMock.Setup(x => x.UpdateCycle(cycleMock.Object)).Verifiable();

            // Test
            service.Finish(cycleMock.Object);

            // Verify
            cacheMock.Verify(x => x.DropCache(), Times.Once);
            cycleMock.Verify(x => x.Finish(), Times.Once);
            databaseMock.Verify(x => x.UpdateCycle(cycleMock.Object), Times.Once);

            cycleMock.VerifyNoOtherCalls();
            databaseMock.VerifyNoOtherCalls();
            cacheMock.VerifyNoOtherCalls();
        }

        [TestMethod]
        public void TestFinishNegative()
        {
            // Init
            Mock<UserCycle> cycleMock = new Mock<UserCycle>();
            cycleMock.Setup(x => x.Finish()).Throws<Exception>();

            // Test & Verify
            Assert.ThrowsException<Exception>(() => service.Finish(cycleMock.Object));

            cycleMock.Verify(x => x.Finish(), Times.Once);

            cycleMock.VerifyNoOtherCalls();
            databaseMock.VerifyNoOtherCalls();
            cacheMock.VerifyNoOtherCalls();
        }

        // TODO TestFinishUnfinishedLesson

        [TestMethod]
        public void TestRegisterCyclePositive()
        {
            // Init
            Mock<UserCycle> cycleMock = new Mock<UserCycle>();
            cycleMock.SetupSet(x => x.CycleID = It.IsAny<String>()).Verifiable();
            databaseMock.Setup(x => x.PutCycle(cycleMock.Object)).Verifiable();

            // Test
            service.RegisterCycle(cycleMock.Object);

            // Verify
            cycleMock.VerifySet(x => x.CycleID = It.IsAny<String>(), Times.Once);
            databaseMock.Verify(x => x.PutCycle(cycleMock.Object), Times.Once);
            databaseMock.Verify(x => x.GetCyclesCount(), Times.Once);

            cycleMock.VerifyNoOtherCalls();
            databaseMock.VerifyNoOtherCalls();
            cacheMock.VerifyNoOtherCalls();
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

            databaseMock.VerifyNoOtherCalls();
            cacheMock.VerifyNoOtherCalls();
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

            databaseMock.VerifyNoOtherCalls();
            cacheMock.VerifyNoOtherCalls();
        }

        [TestCleanup]
        public void TestCleanUp()
        {
            CycleService.DeallocateInstance();
            Directory.Delete("./cycles", true);
        }
    }
}
