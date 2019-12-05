using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using Uceni_jazyku.User_sessions;

namespace UnitTests
{
    [TestClass]
    public class SessionServiceTests
    {
        SessionService service;

        [TestInitialize]
        public void TestInitialization()
        {
            Directory.CreateDirectory("./sessions/user-active");
            Directory.CreateDirectory("./sessions/service");
            service = SessionService.GetInstance();
        }

        [TestMethod]
        public void TestCreateActiveSession()
        {
            AbstractSession session = service.CreateSession(SessionType.ActiveUserSession,"test",3); // TODO adjust when session factory will be ready
            Assert.IsTrue(session is ActiveUserSession);
            Assert.AreEqual(session.Username,"test");
            Assert.AreEqual(session.RemainingEvents,3);
            Assert.AreNotEqual(session.SessionId, "");
            Assert.IsNotNull(session.SessionId);
        }

        [TestCleanup]
        public void TestCleanUp()
        {
            Directory.Delete("./sessions", true);
        }
    }
}
