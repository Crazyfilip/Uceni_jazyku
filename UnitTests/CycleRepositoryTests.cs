using log4net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Uceni_jazyku.Cycles;
using Uceni_jazyku.Cycles.UserCycles;

namespace UnitTests
{
    [TestClass]
    public class CycleRepositoryTests
    {
        ICycleRepository repository;
        List<UserCycle> cycles;

        UserCycle cycle;
        UserCycle cyclePreUpdate, cyclePostUpdate;
        UserCycle cycleInactive1, cycleInactive2;
        UserCycle cycleFinished;
        UserCycle cycleIncomplete;
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
            cycle = new UserCycle() { CycleID = "test" };
            cyclePreUpdate = new UserCycle() { CycleID = "test_id" };
            cyclePostUpdate = new UserCycle() { CycleID = "test_id", Username = "testUpdate" };
            cycleInactive1 = new UserCycle() { CourseID = "test_course", Username = "test", DateCreated = DateTime.Now }.Activate().Inactivate();
            cycleInactive2 = new UserCycle() { CourseID = "test_course", Username = "test", DateCreated = DateTime.Now.AddMinutes(1) }.Activate().Inactivate();
            cycleFinished = new UserCycle() { CourseID = "test_course", Username = "test", DateCreated = DateTime.Now.AddMinutes(-1) }.Activate().Finish();
            cycleIncomplete = new IncompleteUserCycle() { CourseID = "test_course", Username = "test", DateCreated = DateTime.Now.AddMinutes(2) };

            cycles = new List<UserCycle>() { cyclePreUpdate, cycleInactive1, cycleInactive2, cycleIncomplete, cycleFinished };


            Directory.CreateDirectory("./cycles/service");
            repository = new CycleRepository(cycles);

            log4netMock.Reset();
        }

        [TestMethod]
        public void TestPutCyclePositive()
        {
            // Preverify
            Assert.IsFalse(cycles.Contains(cycle));

            // Test
            repository.PutCycle(cycle);

            // Verify
            Assert.IsTrue(cycles.Contains(cycle));

            log4netMock.Verify(x => x.Info("Adding cycle test to repository"), Times.Once);
            log4netMock.Verify(x => x.Debug("Saving repository to file"), Times.Once);

            log4netMock.VerifyNoOtherCalls();
        }

        [TestMethod]
        public void TestUpdateCyclePositiveUpdating()
        {
            // Init
            int numberOfCycles = cycles.Count;

            // Preverify
            Assert.IsTrue(cycles.Contains(cyclePreUpdate));
            Assert.IsFalse(cycles.Contains(cyclePostUpdate));

            // Test
            repository.UpdateCycle(cyclePostUpdate);

            // Verify
            Assert.IsFalse(cycles.Contains(cyclePreUpdate));
            Assert.IsTrue(cycles.Contains(cyclePostUpdate));
            Assert.AreEqual(numberOfCycles, cycles.Count);

            log4netMock.Verify(x => x.Info("Updating cycle test_id"), Times.Once);
            log4netMock.Verify(x => x.Debug("Cycle test_id updated"), Times.Once);
            log4netMock.Verify(x => x.Debug("Saving repository to file"), Times.Once);

            log4netMock.VerifyNoOtherCalls();
        }

        [TestMethod]
        public void TestUpdateCyclePositiveAdding()
        {
            // Init
            int numberOfCycles = cycles.Count;

            // Preverify
            Assert.IsFalse(cycles.Contains(cycle));

            // Test
            repository.UpdateCycle(cycle);

            // Verify
            Assert.IsTrue(cycles.Contains(cycle));
            Assert.AreEqual(numberOfCycles + 1, cycles.Count);

            log4netMock.Verify(x => x.Info("Updating cycle test"), Times.Once);
            log4netMock.Verify(x => x.Debug("Cycle test added"), Times.Once);
            log4netMock.Verify(x => x.Debug("Saving repository to file"), Times.Once);

            log4netMock.VerifyNoOtherCalls();
        }

        [TestMethod]
        public void TestGetOldestUserInactiveCycleNegative()
        {
            // Test
            UserCycle result = repository.GetOldestUserInactiveCycle("testuser", "test_course");

            // Verify
            Assert.IsNull(result);

            log4netMock.Verify(x => x.Info("Getting oldest inactive cycle for user testuser"), Times.Once);

            log4netMock.VerifyNoOtherCalls();
        }

        [TestMethod]
        public void TestGetOldestUserInactiveCyclePositive()
        {
            // Test
            UserCycle result = repository.GetOldestUserInactiveCycle("test", "test_course");

            // Verify
            Assert.AreEqual(cycleInactive1, result);
            Assert.AreNotEqual(cycleInactive2, result);

            log4netMock.Verify(x => x.Info("Getting oldest inactive cycle for user test"), Times.Once);

            log4netMock.VerifyNoOtherCalls();
        }

        [TestMethod]
        public void TestGetUserIncompleteCycleNegative()
        {
            //Test
            IncompleteUserCycle result = repository.GetUserIncompleteCycle("testuser", "test_course");

            // Verify
            Assert.IsNull(result);

            log4netMock.Verify(x => x.Info("Getting incomplete cycle for user testuser"), Times.Once);

            log4netMock.VerifyNoOtherCalls();
        }

        [TestMethod]
        public void TestGetUserIncompleteCycleExisting()
        {
            // Test
            IncompleteUserCycle result = repository.GetUserIncompleteCycle("test", "test_course");

            // Verify
            Assert.AreEqual(cycleIncomplete, result);

            log4netMock.Verify(x => x.Info("Getting incomplete cycle for user test"), Times.Once);

            log4netMock.VerifyNoOtherCalls();
        }

        [TestMethod]
        public void TestGetNotFinishedCyclesPositive()
        {
            // Test
            List<UserCycle> result = repository.GetNotFinishedCycles("test", "test_course");

            // Verify
            CollectionAssert.AreEqual(new List<UserCycle>() { cycleInactive1, cycleInactive2, cycleIncomplete }, result);
            Assert.IsFalse(result.Contains(cycleFinished));

            log4netMock.Verify(x => x.Info("Getting cycles which are not in finished state for user test"), Times.Once);

            log4netMock.VerifyNoOtherCalls();
        }

        [TestMethod]
        public void TestGetNotFinishedCyclesNegative()
        {
            // Test
            List<UserCycle> result = repository.GetNotFinishedCycles("testtest", "test_course");

            // Verify
            CollectionAssert.AreEqual(new List<UserCycle>(), result);

            log4netMock.Verify(x => x.Info("Getting cycles which are not in finished state for user testtest"), Times.Once);

            log4netMock.VerifyNoOtherCalls();
        }

        [TestCleanup]
        public void CleanUp()
        {
            cycles = null;
            Directory.Delete("./cycles", true);
        }
    }
}
