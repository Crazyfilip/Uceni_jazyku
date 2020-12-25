using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Uceni_jazyku.Cycles;
using Uceni_jazyku.Cycles.LanguageCycles;
using Uceni_jazyku.Cycles.Program;
using Uceni_jazyku.Cycles.UserCycles;

namespace UnitTests
{
    [TestClass]
    public class CycleTests
    {
        UserCycle cycle;
        UserCycle cycleNew, cycleActive, cycleInactive;
        UserCycle cycleActiveWithProgram, cycleSwap;
        List<UserProgramItem> userProgramItems, swappingList;
        UserProgramItem item1, item2, item3;

        [TestInitialize]
        public void Init()
        {
            userProgramItems = new List<UserProgramItem>();

            cycle = new UserCycle();
            cycleNew = new UserCycle().AssignUser("test");
            cycleActive = new UserCycle().AssignUser("test").Activate();
            cycleInactive = new UserCycle().AssignUser("test").Activate().Inactivate();
            cycleActiveWithProgram = new UserCycle().AssignUser("test").AssignProgram(new List<UserProgramItem>()).Activate();

            LanguageCycle example = LanguageCycle.LanguageCycleExample();
            item1 = new UserProgramItem(example.CycleID, example.PlanNext());
            item2 = new UserProgramItem(example.CycleID, example.PlanNext());
            item3 = new UserProgramItem(example.CycleID, example.PlanNext());

            swappingList = new List<UserProgramItem>() { item1, item2 };
            cycleSwap = new UserCycle().AssignProgram(swappingList);
        }

        [TestMethod]
        public void TestUserCycleInitialization()
        {
            // Test
            UserCycle cycle = new UserCycle();

            // Verify
            Assert.AreEqual(UserCycleState.UnknownUser, cycle.State);
            Assert.IsNotNull(cycle.UserProgramItems);
            Assert.AreEqual(0, cycle.UserProgramItems.Count);
            Assert.AreEqual(0, cycle.FinishedEvents);
            Assert.IsNull(cycle.CycleID);
            Assert.IsNull(cycle.Username);
        }

        [TestMethod]
        public void TestAssignUserPositive()
        {
            // Test
            UserCycle result = cycle.AssignUser("test");

            // Verify
            Assert.AreSame(cycle, result);
            Assert.AreEqual("test", result.Username);
            Assert.AreEqual(UserCycleState.New, result.State);
            Assert.IsTrue(result.IsUserAssigned);
        }

        [TestMethod]
        public void TestAssignUserNegative()
        {
            // Init
            Mock<UserCycle> cycleMock = new Mock<UserCycle>();
            cycleMock.Setup(x => x.AssignUser("user")).CallBase();
            cycleMock.SetupGet(x => x.IsUserAssigned).Returns(true);

            // Test & Verify
            Exception exception = Assert.ThrowsException<Exception>(() => cycleMock.Object.AssignUser("user"));
            Assert.AreEqual("username already assigned", exception.Message);

            cycleMock.Verify(x => x.AssignUser("user"), Times.Once);
            cycleMock.Verify(x => x.IsUserAssigned, Times.Once);
            
            cycleMock.VerifyNoOtherCalls();
        }

        [TestMethod]
        public void TestAssignProgramPositive()
        {
            // Test
            UserCycle result = cycle.AssignProgram(userProgramItems);

            // Verify
            Assert.AreSame(cycle, result);
            CollectionAssert.AreEqual(userProgramItems, result.UserProgramItems);
            Assert.IsTrue(result.IsProgramAssigned);
        }

        [TestMethod]
        public void TestAssignProgramNegative()
        {
            // Init
            Mock<UserCycle> cycleMock = new Mock<UserCycle>();
            cycleMock.Setup(x => x.AssignProgram(userProgramItems)).CallBase();
            cycleMock.SetupGet(x => x.IsProgramAssigned).Returns(true);

            // Test & Verify
            Exception exception = Assert.ThrowsException<Exception>(() => cycleMock.Object.AssignProgram(userProgramItems));
            Assert.AreEqual("cycle program already assigned", exception.Message);

            cycleMock.Verify(x => x.AssignProgram(userProgramItems), Times.Once);
            cycleMock.Verify(x => x.IsProgramAssigned, Times.Once);

            cycleMock.VerifyNoOtherCalls();
        }

        [TestMethod]
        public void TestActivatePositiveNew()
        {
            // Preverify
            Assert.AreEqual(UserCycleState.New, cycleNew.State);

            // Test
            UserCycle result = cycleNew.Activate();

            // Verify
            Assert.AreSame(cycleNew, result);
            Assert.AreEqual(UserCycleState.Active, cycleNew.State);
        }

        [TestMethod]
        public void TestActivatePositiveInactive()
        {
            // Preverify
            Assert.AreEqual(UserCycleState.Inactive, cycleInactive.State);

            // Test
            UserCycle result = cycleInactive.Activate();

            // Verify
            Assert.AreSame(cycleInactive, result);
            Assert.AreEqual(UserCycleState.Active, cycleInactive.State);
        }

        [DataRow(UserCycleState.UnknownUser)]
        [DataRow(UserCycleState.Active)]
        [DataRow(UserCycleState.Finished)]
        [DataTestMethod]
        public void TestActivateNegative(UserCycleState state)
        {
            // Init
            Mock<UserCycle> cycleMock = new Mock<UserCycle>();
            cycleMock.Setup(x => x.Activate()).CallBase();
            cycleMock.SetupGet(x => x.State).Returns(state);

            // Test & Verify
            Exception exception = Assert.ThrowsException<Exception>(() => cycleMock.Object.Activate());
            Assert.AreEqual("Cycle with state " + state + " cannot be activated", exception.Message);

            cycleMock.Verify(x => x.Activate(), Times.Once);
            cycleMock.Verify(x => x.State, Times.Exactly(3));

            cycleMock.VerifyNoOtherCalls();
        }

        [TestMethod]
        public void TestInactivatePositive()
        {
            // Preverify
            Assert.AreEqual(UserCycleState.Active, cycleActive.State);

            // Test
            UserCycle result = cycleActive.Inactivate();

            // Verify
            Assert.AreSame(cycleActive, result);
            Assert.AreEqual(UserCycleState.Inactive, cycleActive.State);
        }

        [DataRow(UserCycleState.UnknownUser)]
        [DataRow(UserCycleState.New)]
        [DataRow(UserCycleState.Inactive)]
        [DataRow(UserCycleState.Finished)]
        [DataTestMethod]
        public void TestInactivateNegative(UserCycleState state)
        {
            // Init
            Mock<UserCycle> cycleMock = new Mock<UserCycle>();
            cycleMock.Setup(x => x.Inactivate()).CallBase();
            cycleMock.SetupGet(x => x.State).Returns(state);

            // Test & Verify
            Exception exception = Assert.ThrowsException<Exception>(() => cycleMock.Object.Inactivate());
            Assert.AreEqual("Cycle with state " + state + " cannot be inactivated", exception.Message);

            cycleMock.Verify(x => x.Inactivate(), Times.Once);
            cycleMock.Verify(x => x.State, Times.Exactly(2));

            cycleMock.VerifyNoOtherCalls();
        }

        [TestMethod]
        public void TestFinish()
        {
            // Preverify
            Assert.AreEqual(UserCycleState.Active, cycleActiveWithProgram.State);

            // Test
            cycleActiveWithProgram.Finish();

            // Verify
            Assert.AreEqual(UserCycleState.Finished, cycleActiveWithProgram.State);
        }

        [DataRow(UserCycleState.UnknownUser)]
        [DataRow(UserCycleState.New)]
        [DataRow(UserCycleState.Inactive)]
        [DataRow(UserCycleState.Finished)]
        [DataTestMethod]
        public void TestFinishNegativeIncorrectState(UserCycleState state)
        {
            // Init
            Mock<UserCycle> cycleMock = new Mock<UserCycle>();
            cycleMock.Setup(x => x.Finish()).CallBase();
            cycleMock.SetupGet(x => x.State).Returns(state);

            // Test & Verify
            Exception exception = Assert.ThrowsException<Exception>(() => cycleMock.Object.Finish());
            Assert.AreEqual("Cycle with state " + state + " cannot be finished", exception.Message);

            cycleMock.Verify(x => x.Finish(), Times.Once);
            cycleMock.Verify(x => x.State, Times.Exactly(2));

            cycleMock.VerifyNoOtherCalls();
        }

        [TestMethod]
        public void TestFinishNegativeUnfinishedLesson()
        {
            // Init
            Mock<LanguageProgramItem> languageItemMock = new Mock<LanguageProgramItem>();
            languageItemMock.SetupGet(x => x.Finished).Returns(false);

            Mock<UserProgramItem> userItemMock = new Mock<UserProgramItem>();
            userItemMock.SetupGet(x => x.LessonRef).Returns(languageItemMock.Object);

            List<UserProgramItem> listMock = new List<UserProgramItem>() { userItemMock.Object };
            Mock<UserCycle> cycleMock = new Mock<UserCycle>();
            cycleMock.Setup(x => x.Finish()).CallBase();
            cycleMock.SetupGet(x => x.State).Returns(UserCycleState.Active);
            cycleMock.SetupGet(x => x.UserProgramItems).Returns(listMock);

            // Test & Verify
            Exception exception = Assert.ThrowsException<Exception>(() => cycleMock.Object.Finish());
            Assert.AreEqual("Cycle doesn't have finished all lesson so can't be finished", exception.Message);

            cycleMock.Verify(x => x.Finish(), Times.Once);
            cycleMock.Verify(x => x.State, Times.Once);
            cycleMock.Verify(x => x.UserProgramItems, Times.Once);
            userItemMock.Verify(x => x.LessonRef, Times.Once);
            languageItemMock.Verify(x => x.Finished, Times.Once);

            cycleMock.VerifyNoOtherCalls();
            userItemMock.VerifyNoOtherCalls();
            languageItemMock.VerifyNoOtherCalls();
        }

        [TestMethod]
        public void TestSwapLesson()
        {
            // Preverify
            CollectionAssert.AreEqual(new List<UserProgramItem>() { item1, item2 }, cycleSwap.UserProgramItems);

            // Test
            UserProgramItem result = cycleSwap.SwapLesson(item3);

            // Verify
            CollectionAssert.AreEqual(new List<UserProgramItem>() { item3, item1 }, cycleSwap.UserProgramItems);
            Assert.AreEqual(item2, result);
        }
    }
}
