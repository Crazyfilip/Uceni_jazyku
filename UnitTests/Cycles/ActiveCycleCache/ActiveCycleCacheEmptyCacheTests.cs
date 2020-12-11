using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;
using Uceni_jazyku.Cycles;
using Uceni_jazyku.Cycles.ActiveCycleCache;

namespace UnitTests.Cycles
{
    [TestClass]
    public class ActiveCycleCacheEmptyCacheTests
    {
        private ActiveCycleCache cache;
        private UserCycle cycleValid, cycleInvalid;
        private readonly string activeCycleCacheFile = "cycles/service/active-cycle.xml";

        private UserCycle Deserialize()
        {
            var serializer = new DataContractSerializer(typeof(UserCycle));
            using XmlReader reader = XmlReader.Create(activeCycleCacheFile);
            return (UserCycle)serializer.ReadObject(reader);
        }

        [TestInitialize]
        public void Init()
        {
            Directory.CreateDirectory("./cycles/service");
            cache = new ActiveCycleCache();
            cycleValid = new UserCycle().AssignUser("test").Activate();
            cycleInvalid = new UserCycle();
        }

        [TestMethod]
        public void TestIsCacheFilledNegative()
        {
            // Test
            bool result = cache.IsCacheFilled();

            // Verify
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void TestDropCache()
        {
            // Test
            cache.DropCache();

            // Verify
            // DropCache not throws exception
        }

        [TestMethod]
        public void TestInsertToCachePositive()
        {
            // Preverify
            Assert.IsFalse(File.Exists(activeCycleCacheFile));

            // Test
            cache.InsertToCache(cycleValid);

            // Verify
            Assert.IsTrue(File.Exists(activeCycleCacheFile));
            UserCycle result = Deserialize();
            Assert.AreEqual(cycleValid, result);
        }

        [TestMethod]
        public void TestInsertToCacheNegative()
        {
            // Test & Verify
            ArgumentException exception = Assert.ThrowsException<ArgumentException>(() => cache.InsertToCache(cycleInvalid));
            Assert.AreEqual("Cycle is in incorrect state to be saved in cache", exception.Message);
            Assert.IsFalse(File.Exists(activeCycleCacheFile));
        }

        [TestMethod]
        public void TestGetFromCache()
        {
            // Test
            UserCycle result = cache.GetFromCache();

            // Verify
            Assert.IsNull(result);
        }

        [TestCleanup]
        public void CleanUp()
        {
            Directory.Delete("./cycles", true);
        }

    }
}
