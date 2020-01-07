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

        [TestInitialize]
        public void TestInitialization()
        {
            Directory.CreateDirectory("./cycles/user-active");
            Directory.CreateDirectory("./cycles/service");
            service = CycleService.GetInstance();
            factory = new CycleFactory();
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

        [TestCleanup]
        public void TestCleanUp()
        {
            Directory.Delete("./cycles", true);
        }
    }
}
