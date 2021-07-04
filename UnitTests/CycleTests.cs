﻿using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using LanguageLearning.Cycles;
using LanguageLearning.Cycles.Program;
using LanguageLearning.Cycles.UserCycles;
using LanguageLearning.Language;

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
            cycleNew = new UserCycle() { Username = "test" };
            cycleActive = new UserCycle() { Username = "test" };
            cycleInactive = new UserCycle() { Username = "test" }.Inactivate();
            cycleActiveWithProgram = new UserCycle() { Username = "test", UserProgramItems = new List<UserProgramItem>() };

            LanguageProgramItem languageItem1 = new LanguageProgramItem("lesson1");
            LanguageProgramItem languageItem2 = new LanguageProgramItem("lesson2");
            LanguageProgramItem languageItem3 = new LanguageProgramItem("lesson3");

            LanguageTopic example = new LanguageTopic() { TopicId = "topic_id", Lessons = new List<LanguageProgramItem>() { languageItem1, languageItem2, languageItem3 } };
            item1 = new UserProgramItem(example.TopicId, example.PlanNextLesson());
            item2 = new UserProgramItem(example.TopicId, example.PlanNextLesson());
            item3 = new UserProgramItem(example.TopicId, example.PlanNextLesson());

            swappingList = new List<UserProgramItem>() { item1, item2 };
            cycleSwap = new UserCycle() { UserProgramItems = swappingList };
        }

        [TestMethod]
        public void TestUserCycleConstructorPositive()
        {
            // Test
            UserCycle cycle = new UserCycle();

            // Verify
            Assert.AreEqual(UserCycleState.Active, cycle.State);
            Assert.IsNotNull(cycle.UserProgramItems);
            Assert.AreEqual(0, cycle.UserProgramItems.Count);
            Assert.AreEqual(0, cycle.FinishedEvents);
            Assert.IsNull(cycle.Id);
            Assert.IsNull(cycle.Username);
        }

        [TestMethod]
        public void TestActivatePositive()
        {
            // Preverify
            Assert.AreEqual(UserCycleState.Inactive, cycleInactive.State);

            // Test
            UserCycle result = cycleInactive.Activate();

            // Verify
            Assert.AreSame(cycleInactive, result);
            Assert.AreEqual(UserCycleState.Active, cycleInactive.State);
        }

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
            IncorrectCycleStateException exception = Assert.ThrowsException<IncorrectCycleStateException>(() => cycleMock.Object.Activate());
            Assert.AreEqual("Cycle with state " + state + " cannot be activated", exception.Message);

            cycleMock.Verify(x => x.Activate(), Times.Once);
            cycleMock.Verify(x => x.State, Times.Exactly(2));

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
            IncorrectCycleStateException exception = Assert.ThrowsException<IncorrectCycleStateException>(() => cycleMock.Object.Inactivate());
            Assert.AreEqual("Cycle with state " + state + " cannot be inactivated", exception.Message);

            cycleMock.Verify(x => x.Inactivate(), Times.Once);
            cycleMock.Verify(x => x.State, Times.Exactly(2));

            cycleMock.VerifyNoOtherCalls();
        }

        [TestMethod]
        public void TestFinishPositive()
        {
            // Preverify
            Assert.AreEqual(UserCycleState.Active, cycleActiveWithProgram.State);

            // Test
            cycleActiveWithProgram.Finish();

            // Verify
            Assert.AreEqual(UserCycleState.Finished, cycleActiveWithProgram.State);
        }

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
            IncorrectCycleStateException exception = Assert.ThrowsException<IncorrectCycleStateException>(() => cycleMock.Object.Finish());
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
        public void TestSwapLessonPositive()
        {
            // Preverify
            CollectionAssert.AreEqual(new List<UserProgramItem>() { item1, item2 }, cycleSwap.UserProgramItems);

            // Test
            UserProgramItem result = cycleSwap.SwapLesson(item3);

            // Verify
            CollectionAssert.AreEqual(new List<UserProgramItem>() { item3, item1 }, cycleSwap.UserProgramItems);
            Assert.AreEqual(item2, result);
        }

        [TestMethod]
        public void TestAreAllFinishedPositive()
        {
            // Test
            bool result = cycleActiveWithProgram.AreAllFinished();

            // Verify
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void TestAreAllFinishedNegative()
        {
            // Test
            bool result = cycleSwap.AreAllFinished();

            // Verify
            Assert.IsFalse(result);
        }
    }
}
