using Microsoft.VisualStudio.TestTools.UnitTesting;
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

        [TestCleanup]
        public void CleanUp()
        {
            database = null;
            Directory.Delete("./cycles", true);
        }
    }
}
