using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using Uceni_jazyku.Cycles;
using Uceni_jazyku.Cycles.UserCycles;

namespace UnitTests
{
    [TestClass]
    public class CycleFactoryTests
    {
        CycleFactory CycleFactory;

        [TestInitialize]
        public void Init()
        {
            CycleFactory = new CycleFactory();
        }

        [TestMethod]
        public void TestCreateCyclePositive()
        {
            // Test
            UserCycle result = CycleFactory.CreateCycle();

            // Verify
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.CycleID);
            Assert.AreEqual(UserCycleState.UnknownUser, result.State);
            Assert.IsNotNull(result.DateCreated);
        }

        [TestMethod]
        public void TestCreateIncompleteCyclePositive()
        {
            // Test
            IncompleteUserCycle result = CycleFactory.CreateIncompleteCycle("test");

            // Verify
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.CycleID);
            Assert.AreEqual(UserCycleState.Inactive, result.State);
            Assert.AreEqual("test", result.Username);
            Assert.IsNotNull(result.DateCreated);
        }
    }
}
