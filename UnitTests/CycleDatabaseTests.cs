using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using Uceni_jazyku.Cycles;

namespace UnitTests
{
    [TestClass]
    public class CycleDatabaseTests
    {
        CycleDatabase database;

        [TestInitialize]
        public void Init()
        {
            Directory.CreateDirectory("./cycles/service");
            database = new CycleDatabase();
        }

        [TestMethod]
        public void TestSave()
        {
            Assert.IsFalse(File.Exists("./cycles/service/database.xml"));
            database.Save();
            Assert.IsTrue(File.Exists("./cycles/service/database.xml"));
        }

        [TestMethod]
        public void LoadEmptyDatabase()
        {
            database.Load();
            Assert.AreEqual(0, database.GetCyclesCount());
        }

        [TestMethod]
        public void PutToDatabase()
        {
            UserCycle cycle = new UserCycle().AssignUser("test");
            cycle.CycleID = "test_cycle_0";
            database.Load();
            database.PutCycle(cycle);
            Assert.IsTrue(database.IsInDatabase(cycle));
            Assert.AreEqual(1, database.GetCyclesCount());
        }

        [TestMethod]
        public void UpdateCycle()
        {
            UserCycle cycle1 = new UserCycle().AssignUser("test");
            cycle1.CycleID = "test_cycle_0";
            database.Load();
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
        public void UpdateCycleNullDatabase()
        {
            Exception ex = Assert.ThrowsException<Exception>(() => database.UpdateCycle(null));
            Assert.AreEqual("invalid state of database", ex.Message);
        }

        [TestMethod]
        public void CycleNumberTest()
        {
            UserCycle cycle = new UserCycle().AssignUser("test").Activate();
            int cycleNumber = 8548;
            cycle.CycleID = "cycle" + cycleNumber;
            database.Load();
            database.PutCycle(cycle);
            int result = database.getCycleNumber(cycle);
            Assert.AreEqual(cycleNumber, result);
        }

        [TestMethod]
        public void WhenInvalidStateOfDatabase_ThenOldestThrowsException()
        {
            Exception ex = Assert.ThrowsException<Exception>(() => database.GetOldestUserInactiveCycle("test"));
            Assert.AreEqual("invalid state of database", ex.Message);
        }

        [TestMethod]
        public void WhenNoInactiveCycle_ThenOldestReturnNull()
        {
            database.Load();
            Assert.IsNull(database.GetOldestUserInactiveCycle("testuser"));
        }

        [TestMethod]
        public void WhenInactiveCyclePresents_ThenReturnOldest()
        {
            UserCycle cycle_first = new UserCycle().AssignUser("test").Activate().Inactivate();
            cycle_first.CycleID = "cycle1";
            UserCycle cycle_second = new UserCycle().AssignUser("test").Activate().Inactivate();
            cycle_second.CycleID = "cycle2";
            database.Load();
            database.PutCycle(cycle_first);
            database.PutCycle(cycle_second);

            UserCycle cycle_result = database.GetOldestUserInactiveCycle("test");

            Assert.AreEqual(cycle_first, cycle_result);
            Assert.AreNotEqual(cycle_second, cycle_result);
        }

        [TestCleanup]
        public void CleanUp()
        {
            database = null;
            Directory.Delete("./cycles", true);
        }
    }
}
