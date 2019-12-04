using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using Uceni_jazyku.User_sessions;

namespace UnitTests
{
    [TestClass]
    public class SessionTests
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
            AbstractSession session = service.CreateSession(); // TODO adjust when session factory will be ready
            Assert.IsTrue(session is ActiveUserSession);
        }

        [TestCleanup]
        public void TestCleanUp()
        {
            Directory.Delete("./sessions", true);
        }
    }
}
