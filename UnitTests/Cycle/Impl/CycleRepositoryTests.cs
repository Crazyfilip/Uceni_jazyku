using LanguageLearning.Common;
using LanguageLearning.Cycle;
using LanguageLearning.Cycle.Impl;
using LanguageLearning.Cycle.Model;
using log4net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace UnitTests.Cycle.Impl
{
    [TestClass]
    public class CycleRepositoryTests
    {
        ICycleRepository repository;
        Mock<Serializer<UserCycle>> serializer;
        static readonly Mock<ILog> log4netMock = new Mock<ILog>();

        [ClassInitialize]
        public static void ClassInit(TestContext testContext)
        {
            var field = typeof(CycleRepository).GetField("log", BindingFlags.Static | BindingFlags.NonPublic);
            field.SetValue(null, log4netMock.Object);
        }

        [TestInitialize]
        public void Init()
        {
            serializer = new Mock<Serializer<UserCycle>>();
            repository = new CycleRepository(serializer.Object);
            log4netMock.Reset();
        }

        [TestMethod]
        public void TestGetOldestUserInactiveCycleNegative()
        {
            // Init
            serializer.Setup(x => x.Load()).Returns(new List<UserCycle>());

            // Test
            UserCycle result = repository.GetOldestUserInactiveCycle("testuser", "test_course");

            // Verify
            Assert.IsNull(result);

            log4netMock.Verify(x => x.Info("Getting oldest inactive cycle for user testuser"), Times.Once);

        }

        [TestMethod]
        public void TestGetOldestUserInactiveCyclePositive()
        {
            // Init
            Mock<UserCycle> cycle1 = new();
            cycle1.SetupGet(x => x.CourseID).Returns("test_course");
            cycle1.SetupGet(x => x.Username).Returns("test");
            cycle1.SetupGet(x => x.State).Returns(UserCycleState.Inactive);
            cycle1.Setup(x => x.DateCreated).Returns(DateTime.Now);
            Mock<UserCycle> cycle2 = new();
            cycle2.SetupGet(x => x.CourseID).Returns("test_course");
            cycle2.SetupGet(x => x.Username).Returns("test");
            cycle2.SetupGet(x => x.State).Returns(UserCycleState.Inactive);
            cycle2.Setup(x => x.DateCreated).Returns(DateTime.Now.AddMinutes(5));
            List<UserCycle> cycles = new List<UserCycle>() { cycle1.Object, cycle2.Object };
            serializer.Setup(x => x.Load()).Returns(cycles);

            // Test
            UserCycle result = repository.GetOldestUserInactiveCycle("test", "test_course");

            // Verify
            Assert.AreEqual(cycle1.Object, result);
            Assert.AreNotEqual(cycle2.Object, result);

            log4netMock.Verify(x => x.Info("Getting oldest inactive cycle for user test"), Times.Once);
        }

        [TestMethod]
        public void TestGetUserIncompleteCycleNegative()
        {
            // Init
            serializer.Setup(x => x.Load()).Returns(new List<UserCycle>());

            //Test
            IncompleteUserCycle result = repository.GetUserIncompleteCycle("testuser", "test_course");

            // Verify
            Assert.IsNull(result);

            log4netMock.Verify(x => x.Info("Getting incomplete cycle for user testuser"), Times.Once);
        }

        [TestMethod]
        public void TestGetUserIncompleteCycleExisting()
        {
            // Init
            Mock<IncompleteUserCycle> cycle = new();
            cycle.SetupGet(x => x.CourseID).Returns("test_course");
            cycle.SetupGet(x => x.Username).Returns("test");
            List<UserCycle> cycles = new List<UserCycle>() { cycle.Object };
            serializer.Setup(x => x.Load()).Returns(cycles);

            // Test
            IncompleteUserCycle result = repository.GetUserIncompleteCycle("test", "test_course");

            // Verify
            Assert.AreEqual(cycle.Object, result);

            log4netMock.Verify(x => x.Info("Getting incomplete cycle for user test"), Times.Once);
        }

        [TestMethod]
        public void TestGetNotFinishedCyclesPositive()
        {
            // Init
            Mock<UserCycle> cycle1 = new();
            cycle1.SetupGet(x => x.CourseID).Returns("test_course");
            cycle1.SetupGet(x => x.Username).Returns("test");
            cycle1.SetupGet(x => x.State).Returns(UserCycleState.Finished);
            cycle1.Setup(x => x.DateCreated).Returns(DateTime.Now);
            Mock<UserCycle> cycle2 = new();
            cycle2.SetupGet(x => x.CourseID).Returns("test_course");
            cycle2.SetupGet(x => x.Username).Returns("test");
            cycle2.SetupGet(x => x.State).Returns(UserCycleState.Inactive);
            cycle2.Setup(x => x.DateCreated).Returns(DateTime.Now.AddMinutes(5));
            List<UserCycle> cycles = new List<UserCycle>() { cycle1.Object, cycle2.Object };
            serializer.Setup(x => x.Load()).Returns(cycles);

            // Test
            List<UserCycle> result = repository.GetNotFinishedCycles("test", "test_course");

            // Verify
            CollectionAssert.AreEqual(new List<UserCycle>() { cycle2.Object }, result);
            CollectionAssert.DoesNotContain(result, cycle1.Object);

            log4netMock.Verify(x => x.Info("Getting cycles which are not in finished state for user test"), Times.Once);
        }

        [TestMethod]
        public void TestGetNotFinishedCyclesNegative()
        {
            // Init
            serializer.Setup(x => x.Load()).Returns(new List<UserCycle>());

            // Test
            List<UserCycle> result = repository.GetNotFinishedCycles("testtest", "test_course");

            // Verify
            CollectionAssert.AreEqual(new List<UserCycle>(), result);

            log4netMock.Verify(x => x.Info("Getting cycles which are not in finished state for user testtest"), Times.Once);
        }

        [TestCleanup]
        public void Finish()
        {
            serializer.Verify(x => x.Load(), Times.Once);

            log4netMock.VerifyNoOtherCalls();
            serializer.VerifyNoOtherCalls();
        }
    }
}
