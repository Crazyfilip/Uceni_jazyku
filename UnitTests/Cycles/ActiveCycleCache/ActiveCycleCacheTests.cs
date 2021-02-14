using log4net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization;
using System.Xml;
using Uceni_jazyku.Cycles;

namespace UnitTests.Cycles
{
    [TestClass]
    public class ActiveCycleCacheTests
    {
        private static ActiveCycleCache cache;
        private static UserCycle cycleValid, cycleValid2, cycleInvalid;
        private static readonly string activeCycleCacheFile = "cycles/service/active-cycle.xml";
        private static readonly string cycleID = "testID";
        private static readonly Mock<ILog> log4netMock = new Mock<ILog>();
        private static bool isMockSetup = false;

        private static UserCycle Deserialize()
        {
            var serializer = new DataContractSerializer(typeof(UserCycle));
            using XmlReader reader = XmlReader.Create(activeCycleCacheFile);
            return (UserCycle)serializer.ReadObject(reader);
        }

        public static void MockInit()
        {
            if (!isMockSetup)
            {
                var field = typeof(ActiveCycleCache).GetField("log", BindingFlags.Static | BindingFlags.NonPublic);
                field.SetValue(null, log4netMock.Object);
                isMockSetup = true;
            }
        }

        public static void CommonInit()
        {
            // Init space
            Directory.CreateDirectory("./cycles/service");
            cache = new ActiveCycleCache();

            // Init mock
            log4netMock.Reset();
        }

        public static void CommonCleanUp()
        {
            Directory.Delete("./cycles", true);
        }

        [TestClass]
        public class FilledCacheTest
        {
            [ClassInitialize]
            public static void ClassInit(TestContext testContext)
            {
                MockInit();
            }

            [TestInitialize]
            public void CacheAndVarsInit()
            {
                CommonInit();

                // Init variables
                cache = new ActiveCycleCache();
                cycleValid = new UserCycle() { CycleID = cycleID, Username = "test1" }.Activate();
                cycleValid2 = new UserCycle() { CycleID = cycleID, Username = "test2" }.Activate();
                cycleInvalid = new UserCycle() { CycleID = cycleID};

                // Init cache
                var serializer = new DataContractSerializer(typeof(UserCycle));
                using XmlWriter writer = XmlWriter.Create(activeCycleCacheFile);
                serializer.WriteObject(writer, cycleValid);
            }
            [TestMethod]
            public void TestIsCacheFilledPositive()
            {
                // Test
                bool result = cache.IsCacheFilled();

                // Verify
                Assert.IsTrue(result);

                log4netMock.Verify(x => x.Info("Verifying content of cache"), Times.Once);

                log4netMock.VerifyNoOtherCalls();
            }

            [TestMethod]
            public void TestDropCachePositiveFilledCache()
            {
                // Preverify
                Assert.IsTrue(File.Exists(activeCycleCacheFile));

                // Test
                cache.DropCache();

                // Verify
                Assert.IsFalse(File.Exists(activeCycleCacheFile));

                log4netMock.Verify(x => x.Info("Clearing cache"), Times.Once);

                log4netMock.VerifyNoOtherCalls();
            }

            [TestMethod]
            public void TestInsertToCachePositive()
            {
                // Preverify
                Assert.AreEqual(cycleValid, Deserialize());

                // Test
                cache.InsertToCache(cycleValid2);

                // Verify
                Assert.IsTrue(File.Exists(activeCycleCacheFile));
                UserCycle result = Deserialize();
                Assert.AreNotEqual(cycleValid, result);
                Assert.AreEqual(cycleValid2, result);

                log4netMock.Verify(x => x.Info($"Inserting cycle {cycleID} to cache"), Times.Once);

                log4netMock.VerifyNoOtherCalls();
            }

            [TestMethod]
            public void TestInsertToCacheNegative()
            {
                // Test & Verify
                ArgumentException exception = Assert.ThrowsException<ArgumentException>(() => cache.InsertToCache(cycleInvalid));
                Assert.AreEqual("Cycle is in incorrect state to be saved in cache", exception.Message);
                Assert.IsTrue(File.Exists(activeCycleCacheFile));
                UserCycle result = Deserialize();
                Assert.AreEqual(cycleValid, result);

                log4netMock.Verify(x => x.Error($"Tried to insert cycle {cycleID} to cache with state New"), Times.Once);

                log4netMock.VerifyNoOtherCalls();
            }

            [TestMethod]
            public void TestGetFromCache()
            {
                // Test
                UserCycle result = cache.GetFromCache();

                // Verify
                Assert.AreEqual(cycleValid, result);

                log4netMock.Verify(x => x.Info("Getting active cycle from cache"), Times.Once);

                log4netMock.VerifyNoOtherCalls();
            }

            [TestCleanup]
            public void CleanUp()
            {
                CommonCleanUp();
            }
        }

        [TestClass]
        public class EmptyCacheTest
        {
            [ClassInitialize]
            public static void ClassInit(TestContext testContext)
            {
                MockInit();
            }

            [TestInitialize]
            public void Init()
            {
                CommonInit();

                // Init variables
                cache = new ActiveCycleCache();
                cycleValid = new UserCycle() { CycleID = cycleID, Username = "test" }.Activate();
                cycleInvalid = new UserCycle() { CycleID = cycleID };
            }

            [TestMethod]
            public void TestIsCacheFilledNegative()
            {
                // Test
                bool result = cache.IsCacheFilled();

                // Verify
                Assert.IsFalse(result);

                log4netMock.Verify(x => x.Info("Verifying content of cache"), Times.Once);

                log4netMock.VerifyNoOtherCalls();
            }

            [TestMethod]
            public void TestDropCachePositiveEmptyCache()
            {
                // Test
                cache.DropCache();

                // Verify
                // DropCache not throws exception

                log4netMock.Verify(x => x.Info("Clearing cache"), Times.Once);

                log4netMock.VerifyNoOtherCalls();
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

                log4netMock.Verify(x => x.Info($"Inserting cycle {cycleID} to cache"), Times.Once);

                log4netMock.VerifyNoOtherCalls();
            }

            [TestMethod]
            public void TestInsertToCacheNegative()
            {
                // Test & Verify
                ArgumentException exception = Assert.ThrowsException<ArgumentException>(() => cache.InsertToCache(cycleInvalid));
                Assert.AreEqual("Cycle is in incorrect state to be saved in cache", exception.Message);
                Assert.IsFalse(File.Exists(activeCycleCacheFile));

                log4netMock.Verify(x => x.Error($"Tried to insert cycle {cycleID} to cache with state New"), Times.Once);

                log4netMock.VerifyNoOtherCalls();
            }

            [TestMethod]
            public void TestGetFromCacheNegative()
            {
                // Test
                UserCycle result = cache.GetFromCache();

                // Verify
                Assert.IsNull(result);

                log4netMock.Verify(x => x.Info("There is no active cycle in cache"), Times.Once);

                log4netMock.VerifyNoOtherCalls();
            }

            [TestCleanup]
            public void CleanUp()
            {
                CommonCleanUp();
            }
        }
    }
}
