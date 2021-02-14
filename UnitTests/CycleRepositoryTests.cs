using log4net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
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
        List<AbstractCycle> cycles;

        UserCycle cycle;
        UserCycle cyclePreUpdate, cyclePostUpdate;
        UserCycle cycleInactive1, cycleInactive2;
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
            cyclePostUpdate = new UserCycle() { CycleID = "test_id", Username = "test" };
            cycleInactive1 = new UserCycle() { Username = "test" }.Activate().Inactivate();
            cycleInactive2 = new UserCycle() { Username = "test" }.Activate().Inactivate();
            cycleIncomplete = new IncompleteUserCycle() { Username = "test" };

            cycles = new List<AbstractCycle>() { cyclePreUpdate, cycleInactive1, cycleInactive2, cycleIncomplete };


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
            UserCycle result = repository.GetOldestUserInactiveCycle("testuser");

            // Verify
            Assert.IsNull(result);

            log4netMock.Verify(x => x.Info("Getting oldest inactive cycle for user testuser"), Times.Once);

            log4netMock.VerifyNoOtherCalls();
        }

        [TestMethod]
        public void TestGetOldestUserInactiveCyclePositive()
        {
            // Test
            UserCycle result = repository.GetOldestUserInactiveCycle("test");

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
            IncompleteUserCycle result = repository.GetUserIncompleteCycle("testuser");

            // Verify
            Assert.IsNull(result);

            log4netMock.Verify(x => x.Info("Getting incomplete cycle for user testuser"), Times.Once);

            log4netMock.VerifyNoOtherCalls();
        }

        [TestMethod]
        public void TestGetUserIncompleteCycleExisting()
        {
            // Test
            IncompleteUserCycle result = repository.GetUserIncompleteCycle("test");

            // Verify
            Assert.AreEqual(cycleIncomplete, result);

            log4netMock.Verify(x => x.Info("Getting incomplete cycle for user test"), Times.Once);

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
