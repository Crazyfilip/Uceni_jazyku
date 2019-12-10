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
        public void CreateActiveSession()
        {
            AbstractCycle session = factory.CreateCycle(CycleType.ActiveUserCycle, "test", 5);
            Assert.IsNotNull(session);
            Assert.IsTrue(session is UserActiveCycle);
            Assert.IsNull(session.CycleID);
            Assert.AreEqual(session.Username, "test");
            Assert.AreEqual(session.RemainingEvents, 5);
        }
    }
}
