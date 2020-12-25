using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.IO;
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

        [TestInitialize]
        public void Init()
        {
            cycle = new UserCycle();
            cyclePreUpdate = new UserCycle() { CycleID = "cycle0" };
            cyclePostUpdate = new UserCycle() { CycleID = "cycle0" };
            cyclePostUpdate.AssignUser("test");
            cycleInactive1 = new UserCycle().AssignUser("test").Activate().Inactivate();
            cycleInactive1.CycleID = "cycle1"; // TODO refactor when CycleID will be changed
            cycleInactive2 = new UserCycle().AssignUser("test").Activate().Inactivate();
            cycleInactive2.CycleID = "cycle2"; // TODO refactor when CycleID will be changed
            cycleIncomplete = new IncompleteUserCycle("test") { CycleID = "cycle3" };

            cycles = new List<AbstractCycle>() { cyclePreUpdate, cycleInactive1, cycleInactive2, cycleIncomplete };


            Directory.CreateDirectory("./cycles/service");
            repository = new CycleRepository(cycles);

        }

        [TestMethod]
        public void TestPutCyclePositive()
        {
            // Test
            repository.PutCycle(cycle);

            // Verify
            Assert.IsTrue(cycles.Contains(cycle));
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
        }

        [TestMethod]
        public void TestGetOldestUserInactiveCycleNegative()
        {
            // Test
            UserCycle result = repository.GetOldestUserInactiveCycle("testuser");

            // Verify
            Assert.IsNull(result);
        }

        [TestMethod]
        public void TestGetOldestUserInactiveCyclePositive()
        {
            // Test
            UserCycle result = repository.GetOldestUserInactiveCycle("test");

            // Verify
            Assert.AreEqual(cycleInactive1, result);
            Assert.AreNotEqual(cycleInactive2, result);
        }

        [TestMethod]
        public void TestGetUserIncompleteCycleNegative()
        {
            //Test
            IncompleteUserCycle result = repository.GetUserIncompleteCycle("testuser");

            // Verify
            Assert.IsNull(result);
        }

        [TestMethod]
        public void TestGetUserIncompleteCycleExisting()
        {
            // Test
            IncompleteUserCycle result = repository.GetUserIncompleteCycle("test");

            // Verify
            Assert.AreEqual(cycleIncomplete, result);
        }

        [TestCleanup]
        public void CleanUp()
        {
            cycles = null;
            Directory.Delete("./cycles", true);
        }
    }
}
