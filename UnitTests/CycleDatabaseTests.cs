using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using Uceni_jazyku.Cycles;
using Uceni_jazyku.Cycles.UserCycles;

namespace UnitTests
{
    [TestClass]
    public class CycleDatabaseTests
    {
        CycleRepository database;

        [TestInitialize]
        public void Init()
        {
            Directory.CreateDirectory("./cycles/service");
            database = new CycleRepository();
        }

        [TestMethod]
        public void PutToDatabase()
        {
            UserCycle cycle = new UserCycle().AssignUser("test");
            cycle.CycleID = "test_cycle_0";
            database.PutCycle(cycle);
            Assert.IsTrue(database.IsInDatabase(cycle));
            Assert.AreEqual(1, database.GetCyclesCount());
        }

        [TestMethod]
        public void UpdateCycle()
        {
            UserCycle cycle1 = new UserCycle().AssignUser("test");
            cycle1.CycleID = "test_cycle_0";
            database.PutCycle(cycle1);
            Assert.IsTrue(database.IsInDatabase(cycle1));
            Assert.AreEqual(1, database.GetCyclesCount());

            UserCycle cycle2 = new UserCycle().AssignUser("test").Activate();
            cycle2.CycleID = "test_cycle_0";
            database.UpdateCycle(cycle2);
            Assert.IsFalse(database.IsInDatabase(cycle1));
            Assert.IsTrue(database.IsInDatabase(cycle2));
            Assert.AreEqual(1, database.GetCyclesCount());
        }

        [TestMethod]
        public void WhenNoInactiveCycle_ThenOldestReturnNull()
        {
            Assert.IsNull(database.GetOldestUserInactiveCycle("testuser"));
        }

        [TestMethod]
        public void WhenInactiveCyclePresents_ThenReturnOldest()
        {
            UserCycle cycle_first = new UserCycle().AssignUser("test").Activate().Inactivate();
            cycle_first.CycleID = "cycle1";
            UserCycle cycle_second = new UserCycle().AssignUser("test").Activate().Inactivate();
            cycle_second.CycleID = "cycle2";
            database.PutCycle(cycle_first);
            database.PutCycle(cycle_second);

            UserCycle cycle_result = database.GetOldestUserInactiveCycle("test");

            Assert.AreEqual(cycle_first, cycle_result);
            Assert.AreNotEqual(cycle_second, cycle_result);
        }

        [TestMethod]
        public void TestGetUserIncompleteCycleNull()
        {
            Assert.IsNull(database.GetUserIncompleteCycle("test"));
        }

        [TestMethod]
        public void TestGetUserIncompleteCycleExisting()
        {
            IncompleteUserCycle cycle = new IncompleteUserCycle("test");
            database.PutCycle(cycle);

            IncompleteUserCycle result = database.GetUserIncompleteCycle("test");
            Assert.AreEqual(cycle, result);
        }

        [TestCleanup]
        public void CleanUp()
        {
            database = null;
            Directory.Delete("./cycles", true);
        }
    }
}
