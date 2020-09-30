using Microsoft.VisualStudio.TestTools.UnitTesting;
using Uceni_jazyku.Cycles;

namespace UnitTests
{
    /// <summary>
    /// Test for cycle factory
    /// </summary>
    [TestClass]
    public class CycleFactoryTests
    {
        CycleFactory factory;

        [TestInitialize]
        public void InitializeFactory()
        {
            factory = new CycleFactory();
        }

        /// <summary>
        /// Test of correct creation of UserActiveCycle
        /// </summary>
        [TestMethod]
        public void CreateUserActiveCycle()
        {
            AbstractCycle cycle = factory.CreateCycle(CycleType.UserActiveCycle, "test", 5);
            Assert.IsNotNull(cycle);
            Assert.IsTrue(cycle is UserActiveCycle);
            Assert.IsNull(cycle.CycleID);
            Assert.AreEqual("test", cycle.Username);
            Assert.AreEqual(cycle.RemainingEvents, 5);
        }

        /// <summary>
        /// Test of correct creation of UserInactiveCycle
        /// </summary>
        [TestMethod]
        public void CreateUserInactiveCycle()
        {
            AbstractCycle cycle = factory.CreateCycle(CycleType.UserInactiveCycle, "test", 5);
            Assert.IsNotNull(cycle);
            Assert.IsTrue(cycle is UserInactiveCycle);
            Assert.IsNull(cycle.CycleID);
            Assert.AreEqual("test", cycle.Username);
            Assert.AreEqual(cycle.RemainingEvents, 5);
        }

        /// <summary>
        /// Test of correct creation of UserNewCycle
        /// </summary>
        [TestMethod]
        public void CreateUserNewCycle()
        {
            AbstractCycle cycle = factory.CreateCycle(CycleType.UserNewCycle, "test", 5);
            Assert.IsNotNull(cycle);
            Assert.IsTrue(cycle is UserNewCycle);
            Assert.IsNull(cycle.CycleID);
            Assert.AreEqual("test", cycle.Username);
            Assert.IsNull(cycle.RemainingEvents);
        }

        /// <summary>
        /// Test of correct creation of UserFinishedCycle
        /// </summary>
        [TestMethod]
        public void CreateUserFinishedCycle()
        {
            AbstractCycle cycle = factory.CreateCycle(CycleType.UserFinishedCycle, "test", null);
            Assert.IsNotNull(cycle);
            Assert.IsTrue(cycle is UserFinishedCycle);
            Assert.IsNull(cycle.CycleID);
            Assert.AreEqual("test", cycle.Username);
            Assert.IsNull(cycle.RemainingEvents);
        }

        /// <summary>
        /// Test of correct creation of UnknownUserCycle
        /// </summary>
        [TestMethod]
        public void CreateUnknownUserCycle()
        {
            AbstractCycle cycle = factory.CreateCycle(CycleType.UnknownUserCycle, "ignore", null);
            Assert.IsNotNull(cycle);
            Assert.IsTrue(cycle is UnknownUserCycle);
            Assert.IsNull(cycle.CycleID);
            Assert.AreEqual("UnknownUser", cycle.Username);
            Assert.IsNull(cycle.RemainingEvents);
        }
    }
}
