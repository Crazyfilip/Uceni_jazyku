using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;
using Uceni_jazyku.Cycles;
using Uceni_jazyku.Cycles.ActiveCycleCache;

namespace UnitTests.Cycles
{
    [TestClass]
    public class ActiveCycleCachFilledCacheTests
    {
        private ActiveCycleCache cache;
        private UserCycle cycleValid1, cycleValid2, cycleInvalid;
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
            // Init space
            Directory.CreateDirectory("./cycles/service");

            // Init variables
            cache = new ActiveCycleCache();
            cycleValid1 = new UserCycle().AssignUser("test1").Activate();
            cycleValid2 = new UserCycle().AssignUser("test2").Activate();
            cycleInvalid = new UserCycle();

            // Init cache
            var serializer = new DataContractSerializer(typeof(UserCycle));
            using XmlWriter writer = XmlWriter.Create(activeCycleCacheFile);
            serializer.WriteObject(writer, cycleValid1);
        }

        [TestMethod]
        public void TestIsCacheFilledPositive()
        {
            // Test
            bool result = cache.IsCacheFilled();

            // Verify
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void TestDropCache()
        {
            // Preverify
            Assert.IsTrue(File.Exists(activeCycleCacheFile));

            // Test
            cache.DropCache();

            // Verify
            Assert.IsFalse(File.Exists(activeCycleCacheFile));
        }

        [TestMethod]
        public void TestInsertToCachePositive()
        {
            // Preverify
            Assert.AreEqual(cycleValid1, Deserialize());

            // Test
            cache.InsertToCache(cycleValid2);

            // Verify
            Assert.IsTrue(File.Exists(activeCycleCacheFile));
            UserCycle result = Deserialize();
            Assert.AreNotEqual(cycleValid1, result);
            Assert.AreEqual(cycleValid2, result);
        }

        [TestMethod]
        public void TestInsertToCacheNegative()
        {
            // Test & Verify
            ArgumentException exception = Assert.ThrowsException<ArgumentException>(() => cache.InsertToCache(cycleInvalid));
            Assert.AreEqual("Cycle is in incorrect state to be saved in cache", exception.Message);
            Assert.IsTrue(File.Exists(activeCycleCacheFile));
            UserCycle result = Deserialize();
            Assert.AreEqual(cycleValid1, result);
        }

        [TestMethod]
        public void TestGetFromCache()
        {
            // Test
            UserCycle result = cache.GetFromCache();

            // Verify
            Assert.AreEqual(cycleValid1, result);
        }

        [TestCleanup]
        public void CleanUp()
        {
            Directory.Delete("./cycles", true);
        }
    }
}
