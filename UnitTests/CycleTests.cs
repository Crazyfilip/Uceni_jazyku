using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Uceni_jazyku.Cycles;
using Uceni_jazyku.Cycles.LanguageCycles;
using Uceni_jazyku.Cycles.Program;
using Uceni_jazyku.Cycles.UserCycles;

namespace UnitTests
{
    [TestClass]
    public class CycleTests
    {
        [TestMethod]
        public void TestUserCycleInitialization()
        {
            UserCycle cycle = new UserCycle();
            Assert.AreEqual(UserCycleState.UnknownUser, cycle.State);
            Assert.IsNotNull(cycle.UserProgramItems);
            Assert.AreEqual(0, cycle.UserProgramItems.Count);
            Assert.AreEqual(0, cycle.FinishedEvents);
            Assert.IsNull(cycle.CycleID);
            Assert.IsNull(cycle.Username);
        }

        [TestMethod]
        public void TestAssignUser()
        {
            UserCycle cycle = new UserCycle().AssignUser("test");
            Assert.AreEqual(UserCycleState.New, cycle.State);
            Assert.AreEqual("test", cycle.Username);

            Exception exception = Assert.ThrowsException<Exception>(() => cycle.AssignUser("user"));
            Assert.AreEqual("username already assigned", exception.Message);
        }

        [TestMethod]
        public void TestAssignProgram()
        {
            List<UserProgramItem> userProgramItems = new List<UserProgramItem>();
            UserCycle cycle = new UserCycle().AssignProgram(userProgramItems);

            CollectionAssert.AreEqual(userProgramItems, cycle.UserProgramItems);

            Exception exception = Assert.ThrowsException<Exception>(() => cycle.AssignProgram(userProgramItems));
            Assert.AreEqual("cycle program already assigned", exception.Message);
        }

        [TestMethod]
        public void TestActivateNew()
        {
            UserCycle cycle = new UserCycle().AssignUser("test");
            Assert.AreEqual(UserCycleState.New, cycle.State);

            cycle.Activate();

            Assert.AreEqual(UserCycleState.Active, cycle.State);
        }

        [TestMethod]
        public void TestActivateInactive()
        {
            UserCycle cycle = new UserCycle().AssignUser("test").Activate().Inactivate();
            Assert.AreEqual(UserCycleState.Inactive, cycle.State);

            cycle.Activate();

            Assert.AreEqual(UserCycleState.Active, cycle.State);
        }

        [TestMethod]
        public void TestActivateNegative()
        {
            Exception exception = Assert.ThrowsException<Exception>(() => new UserCycle().Activate());
            Assert.AreEqual("Cycle with state " + UserCycleState.UnknownUser + " cannot be activated", exception.Message);
        }

        [TestMethod]
        public void TestInactivate()
        {
            UserCycle cycle = new UserCycle().AssignUser("test").Activate();
            Assert.AreEqual(UserCycleState.Active, cycle.State);

            cycle.Inactivate();

            Assert.AreEqual(UserCycleState.Inactive, cycle.State);
        }

        [TestMethod]
        public void TestInactivateNegative()
        {
            Exception exception = Assert.ThrowsException<Exception>(() => new UserCycle().Inactivate());
            Assert.AreEqual("Cycle with state " + UserCycleState.UnknownUser + " cannot be inactivated", exception.Message);
        }

        [TestMethod]
        public void TestFinish()
        {
            UserCycle cycle = new UserCycle().AssignUser("test").Activate();
            cycle.AssignProgram(new List<UserProgramItem>()); // replace with mocked data
            Assert.AreEqual(UserCycleState.Active, cycle.State);

            cycle.Finish();

            Assert.AreEqual(UserCycleState.Finished, cycle.State);
        }

        [TestMethod]
        public void TestFinishNegativeFromIncorrectState()
        {
            Exception exception = Assert.ThrowsException<Exception>(() => new UserCycle().Finish());
            Assert.AreEqual("Cycle with state " + UserCycleState.UnknownUser + " cannot be finished", exception.Message);
        }

        [TestMethod]
        public void TestFinishNegativeUnfinishedLesson()
        {
            LanguageCycle example = LanguageCycle.LanguageCycleExample();
            UserCycle cycle = new UserCycle().AssignUser("test").AssignProgram(new List<UserProgramItem>() { new UserProgramItem(example.CycleID, example.PlanNext()) }).Activate();

            Exception exception = Assert.ThrowsException<Exception>(() => cycle.Finish());
            Assert.AreEqual("Cycle doesn't have finished all lesson so can't be finished", exception.Message);
        }

        [TestMethod]
        public void TestSwapLesson()
        {
            LanguageCycle example = LanguageCycle.LanguageCycleExample();
            UserProgramItem item1 = new UserProgramItem(example.CycleID, example.PlanNext());
            UserProgramItem item2 = new UserProgramItem(example.CycleID, example.PlanNext());
            UserProgramItem item3 = new UserProgramItem(example.CycleID, example.PlanNext());
            UserCycle cycle = new UserCycle().AssignUser("test").AssignProgram(new List<UserProgramItem>() { item1, item2 });
            CollectionAssert.AreEqual(new List<UserProgramItem>() { item1, item2 }, cycle.UserProgramItems);

            UserProgramItem result = cycle.SwapLesson(item3);

            CollectionAssert.AreEqual(new List<UserProgramItem>() { item3, item1 }, cycle.UserProgramItems);
            Assert.AreEqual(item2, result);
        }
    }
}
