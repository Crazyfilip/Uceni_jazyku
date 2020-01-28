using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using Uceni_jazyku.Cycles;

namespace UnitTests
{
    /// <summary>
    /// Tests for cycle service
    /// </summary>
    [TestClass]
    public class CycleServiceTests
    {
        CycleService service;
        CycleFactory factory;
        CycleDatabase database;

        [TestInitialize]
        public void TestInitialization()
        {
            Directory.CreateDirectory("./cycles/user-active");
            Directory.CreateDirectory("./cycles/user-finished");
            Directory.CreateDirectory("./cycles/user-inactive");
            Directory.CreateDirectory("./cycles/user-new");
            Directory.CreateDirectory("./cycles/service");
            service = CycleService.GetInstance();
            factory = new CycleFactory();
            database = new CycleDatabase();
            database.Load();
        }

        [TestMethod]
        public void TestActiveCycleExistsPositive()
        {
            AbstractCycle cycle = factory.CreateCycle(CycleType.UserActiveCycle, "test", 3);
            cycle.SaveCycle();
            bool result = service.UserActiveCycleExists();
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void TestActiveCycleExistsNegative()
        {
            bool result = service.UserActiveCycleExists();
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void TestLifecycleOfUserCycle()
        {
            UserNewCycle newCycle = service.GetNewCycle("test");

            Assert.IsTrue(UserNewCycle.CycleExists(newCycle.CycleID));
            Assert.IsFalse(UserActiveCycle.CycleExists());
            Assert.IsNull(newCycle.RemainingEvents);

            UserActiveCycle activeCycle = service.Activate(newCycle);

            Assert.IsFalse(UserNewCycle.CycleExists(newCycle.CycleID));
            Assert.IsTrue(UserActiveCycle.CycleExists());
            Assert.AreEqual(newCycle.CycleID, activeCycle.CycleID);
            Assert.AreEqual(newCycle.Username, activeCycle.Username);
            Assert.IsNotNull(activeCycle.RemainingEvents);

            UserInactiveCycle inactiveCycle = service.Inactivate(activeCycle);

            Assert.IsFalse(UserActiveCycle.CycleExists());
            Assert.IsTrue(UserInactiveCycle.CycleExists(inactiveCycle.CycleID));
            Assert.AreEqual(activeCycle.CycleID, inactiveCycle.CycleID);
            Assert.AreEqual(activeCycle.Username, inactiveCycle.Username);
            Assert.IsNotNull(inactiveCycle.RemainingEvents);
            Assert.AreEqual(activeCycle.RemainingEvents, inactiveCycle.RemainingEvents);

            UserActiveCycle activeCycle1 = service.Activate(inactiveCycle);

            Assert.IsFalse(UserInactiveCycle.CycleExists(inactiveCycle.CycleID));
            Assert.IsTrue(UserActiveCycle.CycleExists());
            Assert.AreEqual(inactiveCycle.CycleID, activeCycle1.CycleID);
            Assert.AreEqual(inactiveCycle.Username, activeCycle1.Username);
            Assert.IsNotNull(activeCycle1.RemainingEvents);
            Assert.AreEqual(inactiveCycle.RemainingEvents, activeCycle1.RemainingEvents);

            activeCycle1.RemainingEvents = 0;
            UserFinishedCycle finishedCycle = service.Finish(activeCycle1);

            Assert.IsFalse(UserActiveCycle.CycleExists());
            Assert.IsTrue(UserFinishedCycle.CycleExists(finishedCycle.CycleID));
            Assert.AreEqual(activeCycle1.CycleID, finishedCycle.CycleID);
            Assert.AreEqual(activeCycle1.Username, finishedCycle.Username);
            Assert.IsNull(finishedCycle.RemainingEvents);
        }

        [TestCleanup]
        public void TestCleanUp()
        {
            Directory.Delete("./cycles", true);
        }
    }
}
