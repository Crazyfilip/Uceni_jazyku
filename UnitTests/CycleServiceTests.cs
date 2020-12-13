using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;
using Uceni_jazyku.Cycles;
using Uceni_jazyku.Cycles.Program;
using Uceni_jazyku.Cycles.UserCycles;
using Uceni_jazyku.Cycles.LanguageCycles;
using System.Collections.Generic;
using System.Linq;
using Moq;

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
        UserCycle cycle;
        private readonly string activeCycleCacheFile = "cycles/service/active-cycle.xml";

        private void PrepareCachedActiveCycle()
        {
            cycle = new UserCycle().AssignUser("test").Activate();
            var serializer = new DataContractSerializer(typeof(UserCycle));
            using XmlWriter writer = XmlWriter.Create(activeCycleCacheFile);
            serializer.WriteObject(writer, cycle);
        }
        
        [TestInitialize]
        public void TestInitialization()
        {
            Directory.CreateDirectory("./cycles/service");
            databaseMock = new Mock<ICycleRepository>();
            cacheMock = new Mock<IActiveCycleCache>();
            service = CycleService.GetInstance(databaseMock.Object, cacheMock.Object);
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
        public void TestGetUserCycleCreateNew()
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
            databaseMock.Verify(x => x.UpdateCycle(result), Times.Once);
            cacheMock.Verify(x => x.InsertToCache(result), Times.Once);

            databaseMock.VerifyNoOtherCalls();
            cacheMock.VerifyNoOtherCalls();
        }

        [TestMethod]
        public void TestGetUserCycleFromInactive()
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
        public void TestGetNewCycle()
        {
            // Init
            databaseMock.Setup(x => x.PutCycle(It.IsAny<UserCycle>())).Verifiable(); 

            // Test
            UserCycle result = service.GetNewCycle("test");

            // Verify
            Assert.AreEqual(UserCycleState.New, result.State);
            Assert.AreEqual("test", result.Username);
            databaseMock.Verify(x => x.PutCycle(result), Times.Once);

            databaseMock.VerifyNoOtherCalls();
            cacheMock.VerifyNoOtherCalls();
        }

        [TestMethod]
        public void TestActivateNewCycle()
        {
            LanguageProgramItem languageItem = LanguageCycle.LanguageCycleExample().PlanNext();
            cycle = new UserCycle().AssignUser("test");
            databaseMock.Object.PutCycle(cycle);
            Assert.AreEqual(UserCycleState.New, cycle.State);

            UserCycle result = service.Activate(cycle);
            Assert.AreSame(cycle, result);
            Assert.AreEqual(UserCycleState.Active, result.State);
            Assert.IsTrue(databaseMock.Object.IsInDatabase(result));
            Assert.IsTrue(File.Exists(activeCycleCacheFile));

            UserProgramItem userItem = (UserProgramItem) result.GetNext();
            Assert.AreEqual(languageItem, userItem.LessonRef);
        }

        [TestMethod]
        public void TestActivateInactiveCycle()
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
            cycleMock.Setup(x => x.Activate()).CallBase();

            // Test & Verify
            Assert.ThrowsException<Exception>(() => service.Activate(cycleMock.Object));

            cycleMock.Verify(x => x.State, Times.Exactly(4));
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

        [DataRow(UserCycleState.UnknownUser)]
        [DataRow(UserCycleState.New)]
        [DataRow(UserCycleState.Inactive)]
        [DataRow(UserCycleState.Finished)]
        [DataTestMethod]
        public void TestInactivateNegative(UserCycleState incorrectState)
        {
            // Init
            Mock<UserCycle> cycleMock = new Mock<UserCycle>();
            cycleMock.SetupGet(x => x.State).Returns(incorrectState);
            cycleMock.Setup(x => x.Inactivate()).CallBase();

            // Test & Verify
            Assert.ThrowsException<Exception>(() => service.Inactivate(cycleMock.Object));

            cycleMock.Verify(x => x.State, Times.Exactly(2));
            cycleMock.Verify(x => x.Inactivate(), Times.Once);

            cycleMock.VerifyNoOtherCalls();
            databaseMock.VerifyNoOtherCalls();
            cacheMock.VerifyNoOtherCalls();
        }

        [TestMethod]
        public void TestFinishCorrectStateFinishedLessons()
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

        [DataRow(UserCycleState.UnknownUser)]
        [DataRow(UserCycleState.New)]
        [DataRow(UserCycleState.Inactive)]
        [DataRow(UserCycleState.Finished)]
        [DataTestMethod]
        public void TestFinishIncorrectState(UserCycleState incorrectState)
        {
            // Init
            Mock<UserCycle> cycleMock = new Mock<UserCycle>();
            cycleMock.SetupGet(x => x.State).Returns(incorrectState);
            cycleMock.Setup(x => x.Finish()).CallBase();

            // Test & Verify
            Assert.ThrowsException<Exception>(() => service.Finish(cycleMock.Object));

            cycleMock.Verify(x => x.State, Times.Exactly(2));
            cycleMock.Verify(x => x.Finish(), Times.Once);

            cycleMock.VerifyNoOtherCalls();
            databaseMock.VerifyNoOtherCalls();
            cacheMock.VerifyNoOtherCalls();
        }

        // TODO TestFinishUnfinishedLesson

        [TestMethod]
        public void TestRegisterCycle()
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

            cycleMock.VerifyNoOtherCalls();
            databaseMock.VerifyNoOtherCalls();
            cacheMock.VerifyNoOtherCalls();
        }

        [TestMethod]
        public void TestSwapLessonNewIncomplete()
        {
            LanguageCycle example = LanguageCycle.LanguageCycleExample();
            UserProgramItem item1 = new UserProgramItem(example.CycleID, example.PlanNext());
            UserProgramItem item2 = new UserProgramItem(example.CycleID, example.PlanNext());
            UserProgramItem item3 = new UserProgramItem(example.CycleID, example.PlanNext());
            cycle = new UserCycle().AssignUser("test").AssignProgram(new List<UserProgramItem>() { item1, item2 });
            databaseMock.Object.PutCycle(cycle);
            CollectionAssert.AreEqual(new List<UserProgramItem>() { item1, item2 }, cycle.UserProgramItems);

            service.SwapLesson(cycle, item3);

            CollectionAssert.AreEqual(new List<UserProgramItem>() { item3, item1 }, cycle.UserProgramItems);

            IncompleteUserCycle incompleteCycle = new IncompleteUserCycle("test");
            incompleteCycle.AddLesson(item2);
            incompleteCycle.CycleID = "cycle1";
            Assert.IsTrue(databaseMock.Object.IsInDatabase(incompleteCycle));
        }

        [TestMethod]
        public void TestSwapLessonExistingIncomplete()
        {
            LanguageCycle example = LanguageCycle.LanguageCycleExample();
            UserProgramItem item1 = new UserProgramItem(example.CycleID, example.PlanNext());
            UserProgramItem item2 = new UserProgramItem(example.CycleID, example.PlanNext());
            UserProgramItem item3 = new UserProgramItem(example.CycleID, example.PlanNext());
            UserProgramItem item4 = new UserProgramItem(example.CycleID, example.PlanNext());
            cycle = new UserCycle().AssignUser("test").AssignProgram(new List<UserProgramItem>() { item1, item2 });
            databaseMock.Object.PutCycle(cycle);
            CollectionAssert.AreEqual(new List<UserProgramItem>() { item1, item2 }, cycle.UserProgramItems);
            IncompleteUserCycle incompleteCycle = new IncompleteUserCycle("test");
            incompleteCycle.AddLesson(item4);
            databaseMock.Object.PutCycle(incompleteCycle);

            service.SwapLesson(cycle, item3);

            CollectionAssert.AreEqual(new List<UserProgramItem>() { item3, item1 }, cycle.UserProgramItems);
            CollectionAssert.AreEqual(new List<UserProgramItem>() { item2, item4 }, incompleteCycle.UserProgramItems);
        }

        [TestCleanup]
        public void TestCleanUp()
        {
            databaseMock = null;
            CycleService.DeallocateInstance();
            Directory.Delete("./cycles", true);
        }
    }
}
