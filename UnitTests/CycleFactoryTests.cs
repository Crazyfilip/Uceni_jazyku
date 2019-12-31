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
            AbstractCycle session = factory.CreateCycle(CycleType.UserActiveCycle, "test", 5);
            Assert.IsNotNull(session);
            Assert.IsTrue(session is UserActiveCycle);
            Assert.IsNull(session.CycleID);
            Assert.AreEqual(session.Username, "test");
            Assert.AreEqual(session.RemainingEvents, 5);
        }

        /// <summary>
        /// Test of correct creation of UserInactiveCycle
        /// </summary>
        [TestMethod]
        public void CreateUserInactiveCycle()
        {
            AbstractCycle session = factory.CreateCycle(CycleType.UserInactiveCycle, "test", 5);
            Assert.IsNotNull(session);
            Assert.IsTrue(session is UserInactiveCycle);
            Assert.IsNull(session.CycleID);
            Assert.AreEqual(session.Username, "test");
            Assert.AreEqual(session.RemainingEvents, 5);
        }

        /// <summary>
        /// Test of correct creation of UserNewCycle
        /// </summary>
        [TestMethod]
        public void CreateUserNewCycle()
        {
            AbstractCycle session = factory.CreateCycle(CycleType.UserNewCycle, "test", 5);
            Assert.IsNotNull(session);
            Assert.IsTrue(session is UserNewCycle);
            Assert.IsNull(session.CycleID);
            Assert.AreEqual(session.Username, "test");
            Assert.IsNull(session.RemainingEvents);
        }

        /// <summary>
        /// Test of correct creation of UserFinishedCycle
        /// </summary>
        [TestMethod]
        public void CreateUserFinishedCycle()
        {
            AbstractCycle session = factory.CreateCycle(CycleType.UserFinishedCycle, "test", null);
            Assert.IsNotNull(session);
            Assert.IsTrue(session is UserFinishedCycle);
            Assert.IsNull(session.CycleID);
            Assert.AreEqual(session.Username, "test");
            Assert.IsNull(session.RemainingEvents);
        }
    }
}
