using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using Uceni_jazyku.Cycles;

namespace UnitTests
{
    [TestClass]
    public class SessionServiceTests
    {
        CycleService service;

        [TestInitialize]
        public void TestInitialization()
        {
            Directory.CreateDirectory("./sessions/user-active");
            Directory.CreateDirectory("./sessions/service");
            service = CycleService.GetInstance();
        }

        [TestMethod]
        public void TestCreateActiveSession()
        {
            AbstractCycle session = service.CreateSession(SessionType.ActiveUserSession,"test",3); // TODO adjust when session factory will be ready
            Assert.IsTrue(session is UserActiveCycle);
            Assert.AreEqual(session.Username,"test");
            Assert.AreEqual(session.RemainingEvents,3);
            Assert.AreNotEqual(session.CycleID, "");
            Assert.IsNotNull(session.CycleID);
        }

        [TestCleanup]
        public void TestCleanUp()
        {
            Directory.Delete("./sessions", true);
        }
    }
}
