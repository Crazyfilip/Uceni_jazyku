using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;
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
        CycleDatabase database;
        private readonly string activeCycleCacheFile = "cycles/service/active-cycle.xml";

        private void PrepareCachedActiveCycle()
        {
            UserCycle cycle = new UserCycle().AssignUser("test").Activate();
            var serializer = new DataContractSerializer(typeof(UserCycle));
            using XmlWriter writer = XmlWriter.Create(activeCycleCacheFile);
            serializer.WriteObject(writer, cycle);
        }
        
        [TestInitialize]
        public void TestInitialization()
        {
            database = new CycleDatabase();
            Directory.CreateDirectory("./cycles/service");
            service = CycleService.GetInstance(database);
        }

        [TestMethod]
        public void TestActiveCycleExistsPositive()
        {
            PrepareCachedActiveCycle();
            bool result = service.UserActiveCycleExists();
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void TestActiveCycleExistsNegative()
        {
            bool result = service.UserActiveCycleExists();
            Assert.IsFalse(result);
        }

        //[TestMethod]
        //public void TestLifecycleOfUserCycle()
        //{
        //    UserNewCycle newCycle = service.GetNewCycle("test");

        //    Assert.IsTrue(UserNewCycle.CycleExists(newCycle.CycleID));
        //    Assert.IsFalse(UserActiveCycle.CycleExists());

        //    UserActiveCycle activeCycle = service.Activate(newCycle);

        //    Assert.IsFalse(UserNewCycle.CycleExists(newCycle.CycleID));
        //    Assert.IsTrue(UserActiveCycle.CycleExists());
        //    Assert.AreEqual(newCycle.CycleID, activeCycle.CycleID);
        //    Assert.AreEqual(newCycle.Username, activeCycle.Username);
        //    Assert.IsNotNull(activeCycle.FinishedEvents);

        //    UserInactiveCycle inactiveCycle = service.Inactivate(activeCycle);

        //    Assert.IsFalse(UserActiveCycle.CycleExists());
        //    Assert.IsTrue(UserInactiveCycle.CycleExists(inactiveCycle.CycleID));
        //    Assert.AreEqual(activeCycle.CycleID, inactiveCycle.CycleID);
        //    Assert.AreEqual(activeCycle.Username, inactiveCycle.Username);
        //    Assert.IsNotNull(inactiveCycle.FinishedEvents);
        //    Assert.AreEqual(activeCycle.FinishedEvents, inactiveCycle.FinishedEvents);

        //    UserActiveCycle activeCycle1 = service.Activate(inactiveCycle);

        //    Assert.IsFalse(UserInactiveCycle.CycleExists(inactiveCycle.CycleID));
        //    Assert.IsTrue(UserActiveCycle.CycleExists());
        //    Assert.AreEqual(inactiveCycle.CycleID, activeCycle1.CycleID);
        //    Assert.AreEqual(inactiveCycle.Username, activeCycle1.Username);
        //    Assert.IsNotNull(activeCycle1.FinishedEvents);
        //    Assert.AreEqual(inactiveCycle.FinishedEvents, activeCycle1.FinishedEvents);

        //    UserFinishedCycle finishedCycle = service.Finish(activeCycle1);

        //    Assert.IsFalse(UserActiveCycle.CycleExists());
        //    Assert.IsTrue(UserFinishedCycle.CycleExists(finishedCycle.CycleID));
        //    Assert.AreEqual(activeCycle1.CycleID, finishedCycle.CycleID);
        //    Assert.AreEqual(activeCycle1.Username, finishedCycle.Username);
        //}

        [TestCleanup]
        public void TestCleanUp()
        {
            Directory.Delete("./cycles", true);
        }
    }
}
