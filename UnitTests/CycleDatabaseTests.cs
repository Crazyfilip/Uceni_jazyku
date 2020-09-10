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
        CycleFactory factory;

        [TestInitialize]
        public void Init()
        {
            Directory.CreateDirectory("./cycles/service");
            database = new CycleDatabase();
            factory = new CycleFactory();
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
            AbstractCycle cycle = factory.CreateCycle(CycleType.UserActiveCycle, "test",1);
            cycle.CycleID = "test_cycle_0";
            database.Load();
            database.PutCycle(cycle);
            Assert.IsTrue(database.IsInDatabase(cycle));
            Assert.AreEqual(1, database.GetCyclesCount());
        }

        [TestMethod]
        public void UpdateCycle()
        {
            AbstractCycle cycle1 = factory.CreateCycle(CycleType.UserNewCycle, "test");
            cycle1.CycleID = "test_cycle_0";
            database.Load();
            database.PutCycle(cycle1);
            Assert.IsTrue(database.IsInDatabase(cycle1));
            Assert.AreEqual(1, database.GetCyclesCount());

            AbstractCycle cycle2 = factory.CreateCycle(CycleType.UserActiveCycle, "test", 1);
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
            AbstractCycle cycle = factory.CreateCycle(CycleType.UserActiveCycle, "test", 1);
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
            AbstractCycle cycle_first = factory.CreateCycle(CycleType.UserInactiveCycle, "test", 1);
            cycle_first.CycleID = "cycle1";
            AbstractCycle cycle_second = factory.CreateCycle(CycleType.UserInactiveCycle, "test", 1);
            cycle_second.CycleID = "cycle2";
            database.Load();
            database.PutCycle(cycle_first);
            database.PutCycle(cycle_second);

            UserInactiveCycle cycle_result = database.GetOldestUserInactiveCycle("test");

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
