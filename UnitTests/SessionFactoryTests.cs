using Microsoft.VisualStudio.TestTools.UnitTesting;
using Uceni_jazyku.Cycles;

namespace UnitTests
{
    [TestClass]
    public class SessionFactoryTests
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
            AbstractCycle session = factory.GetSession(SessionType.ActiveUserSession, "test", 5);
            Assert.IsNotNull(session);
            Assert.IsTrue(session is UserActiveCycle);
            Assert.IsNull(session.CycleID);
            Assert.AreEqual(session.Username, "test");
            Assert.AreEqual(session.RemainingEvents, 5);
        }
    }
}
