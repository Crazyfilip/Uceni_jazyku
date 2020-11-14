using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;
using Uceni_jazyku.Cycles;
using Uceni_jazyku.Cycles.UserCycles;

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
        UserCycle cycle;
        private readonly string activeCycleCacheFile = "cycles/service/active-cycle.xml";

        private void PrepareCachedActiveCycle()
        {
            cycle = new UserCycle().AssignUser("test").Activate();
            var serializer = new DataContractSerializer(typeof(UserCycle));
            using XmlWriter writer = XmlWriter.Create(activeCycleCacheFile);
            serializer.WriteObject(writer, cycle);
        }
        
        [TestInitialize]
        public void TestInitialization()
        {
            Directory.CreateDirectory("./cycles/service");
            database = new CycleDatabase();
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

        [TestMethod]
        public void TestGetUserCycleCreateNew()
        {
            Assert.AreEqual(0, database.GetCyclesCount());
            UserCycle result = service.GetUserCycle("test");
            Assert.AreEqual(UserCycleState.Active, result.State);
            Assert.AreEqual("test", result.Username);
            Assert.AreEqual(1, database.GetCyclesCount());
        }

        [TestMethod]
        public void TestGetUserCycleFromInactive()
        {
            cycle = new UserCycle().AssignUser("test").Activate().Inactivate();
            database.PutCycle(cycle);
            Assert.AreEqual(1, database.GetCyclesCount());
            Assert.AreEqual(UserCycleState.Inactive, cycle.State);

            UserCycle result = service.GetUserCycle("test");
            Assert.AreSame(cycle, result);
            Assert.AreEqual(UserCycleState.Active, result.State);
            Assert.AreEqual("test", result.Username);
            Assert.AreEqual(1, database.GetCyclesCount());
        }

        [TestMethod]
        public void TestGetActiveCycleNegative()
        {
            Assert.ThrowsException<FileNotFoundException>(() => service.GetActiveCycle());
        }

        [TestMethod]
        public void TestGetActiveCyclePositive()
        {
            PrepareCachedActiveCycle();
            UserCycle result = service.GetActiveCycle();
            Assert.AreEqual(cycle, result);
        }

        [TestMethod]
        public void TestGetNewCycle()
        {
            UserCycle result = service.GetNewCycle("test");
            Assert.IsTrue(database.IsInDatabase(result));
            Assert.AreEqual(UserCycleState.New, result.State);
            Assert.AreEqual("test", result.Username);
        }

        [TestMethod]
        public void TestActivateNewCycle()
        {
            cycle = new UserCycle().AssignUser("test");
            database.PutCycle(cycle);
            Assert.AreEqual(UserCycleState.New, cycle.State);

            UserCycle result = service.Activate(cycle);
            Assert.AreSame(cycle, result);
            Assert.AreEqual(UserCycleState.Active, result.State);
            Assert.IsTrue(database.IsInDatabase(result));
            Assert.IsTrue(File.Exists(activeCycleCacheFile));
            // TODO assert that program was assigned
        }

        [TestMethod]
        public void TestActivateInactiveCycle()
        {
            cycle = new UserCycle().AssignUser("test").Activate().Inactivate();
            database.PutCycle(cycle);
            Assert.AreEqual(UserCycleState.Inactive, cycle.State);

            UserCycle result = service.Activate(cycle);
            Assert.AreSame(cycle, result);
            Assert.AreEqual(UserCycleState.Active, result.State);
            Assert.IsTrue(database.IsInDatabase(result));
            Assert.IsTrue(File.Exists(activeCycleCacheFile));
        }

        [TestMethod]
        public void TestActivateNegative()
        {
            Assert.ThrowsException<ArgumentException>(() => service.Activate(new UserCycle()));
        }

        [TestMethod]
        public void TestInactivateCycle()
        {
            cycle = new UserCycle().AssignUser("test").Activate();
            database.PutCycle(cycle);
            Assert.AreEqual(UserCycleState.Active, cycle.State);

            UserCycle result = service.Inactivate(cycle);
            Assert.AreSame(cycle, result);
            Assert.AreEqual(UserCycleState.Inactive, result.State);
            Assert.IsTrue(database.IsInDatabase(result));
            Assert.IsFalse(File.Exists(activeCycleCacheFile));
        }

        [TestMethod]
        public void TestInactivateNegative()
        {
            Assert.ThrowsException<ArgumentException>(() => service.Inactivate(new UserCycle()));
        }

        [TestCleanup]
        public void TestCleanUp()
        {
            database = null;
            CycleService.DeallocateInstance();
            Directory.Delete("./cycles", true);
        }
    }
}
