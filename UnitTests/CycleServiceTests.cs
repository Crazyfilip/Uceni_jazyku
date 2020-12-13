﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
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
            cacheMock.VerifyNoOtherCalls();
        }

        [TestMethod]
        public void TestGetUserCycleCreateNew()
        {
            // Init
            UserCycle userCycle = new UserCycle().AssignUser("test");
            userCycle.CycleID = "cycle1";
            databaseMock.Setup(x => x.GetOldestUserInactiveCycle("test")).Returns((UserCycle)null);
            databaseMock.Setup(x => x.UpdateCycle(userCycle)).Verifiable();

            // Test
            UserCycle result = service.GetUserCycle("test");

            // Verify
            Assert.AreEqual(UserCycleState.Active, result.State);
            Assert.AreEqual("test", result.Username);
            Assert.AreEqual(2, databaseMock.Object.GetCyclesCount()); // result + language cycle
            databaseMock.Verify(x => x.GetOldestUserInactiveCycle("test"), Times.Once);
           // databaseMock.Verify(x => x.PutCycle(userCycle), Times.Exactly(2));
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

            databaseMock.VerifyNoOtherCalls();
            cacheMock.VerifyNoOtherCalls();
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
            cacheMock.VerifyNoOtherCalls();
            cycleMock.VerifyNoOtherCalls();
        }

        [TestMethod]
        public void TestGetNewCycle()
        {
            UserCycle result = service.GetNewCycle("test");
            Assert.IsTrue(databaseMock.Object.IsInDatabase(result));
            Assert.AreEqual(UserCycleState.New, result.State);
            Assert.AreEqual("test", result.Username);
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
        }

        [TestMethod]
        public void TestInactivateCycle()
        {
            PrepareCachedActiveCycle();
            databaseMock.Object.PutCycle(cycle);
            Assert.AreEqual(UserCycleState.Active, cycle.State);
            Assert.IsTrue(File.Exists(activeCycleCacheFile));

            UserCycle result = service.Inactivate(cycle);
            Assert.AreSame(cycle, result);
            Assert.AreEqual(UserCycleState.Inactive, result.State);
            Assert.IsTrue(databaseMock.Object.IsInDatabase(result));
            Assert.IsFalse(File.Exists(activeCycleCacheFile));
        }

        [TestMethod]
        public void TestInactivateNegative()
        {
            Assert.ThrowsException<Exception>(() => service.Inactivate(new UserCycle()));
        }

        [TestMethod]
        public void TestFinishCycle()
        {
            PrepareCachedActiveCycle();
            LanguageCycle example = LanguageCycle.LanguageCycleExample();
            cycle.AssignProgram(new List<UserProgramItem>() { new UserProgramItem(example.CycleID, example.PlanNext()) });
            cycle.Update();
            databaseMock.Object.PutCycle(cycle);

            Assert.AreEqual(UserCycleState.Active, cycle.State);
            Assert.IsTrue(File.Exists(activeCycleCacheFile));

            service.Finish(cycle);

            Assert.AreEqual(UserCycleState.Finished, cycle.State);
            Assert.IsFalse(File.Exists(activeCycleCacheFile));
        }

        [TestMethod]
        public void TestFinishNegative()
        {
            Assert.ThrowsException<Exception>(() => service.Finish(new UserCycle()));
        }

        [TestMethod]
        public void TestRegisterCycle()
        {
            cycle = new UserCycle().AssignUser("test").Activate().Inactivate();
            Assert.IsNull(cycle.CycleID);
            Assert.IsFalse(databaseMock.Object.IsInDatabase(cycle));

            service.RegisterCycle(cycle);

            Assert.IsNotNull(cycle.CycleID);
            Assert.IsTrue(databaseMock.Object.IsInDatabase(cycle));
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
