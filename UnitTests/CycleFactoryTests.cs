using Microsoft.VisualStudio.TestTools.UnitTesting;
using Uceni_jazyku.Cycles;

namespace UnitTests
{
    [TestClass]
    public class CycleFactoryTests
    {
        CycleFactory factory;

        [TestInitialize]
        public void InitializeFactory()
        {
            factory = new CycleFactory();
        }

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

        [TestMethod]
        public void CreateUserNewCycle()
        {
            AbstractCycle session = factory.CreateCycle(CycleType.UserInactiveCycle, "test", 5);
            Assert.IsNotNull(session);
            Assert.IsTrue(session is UserNewCycle);
            Assert.IsNull(session.CycleID);
            Assert.AreEqual(session.Username, "test");
            Assert.IsNull(session.RemainingEvents);
        }

        [TestMethod]
        public void CreateUserFinishedCycle()
        {
            AbstractCycle session = factory.CreateCycle(CycleType.UserInactiveCycle, "test", 5);
            Assert.IsNotNull(session);
            Assert.IsTrue(session is UserFinishedCycle);
            Assert.IsNull(session.CycleID);
            Assert.AreEqual(session.Username, "test");
            Assert.IsNull(session.RemainingEvents);
        }
    }
}
