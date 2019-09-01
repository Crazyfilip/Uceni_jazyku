using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Uceni_jazyku.User_sessions;
using System.IO;

namespace UnitTests
{
    

    [TestClass]
    public class UnitTest1
    {
        [TestInitialize]
        public void TestInitialization()
        {
            Directory.CreateDirectory("./sessions/user-active");
        }

        [TestMethod]
        public void TestSessionCreation()
        {
            User_session user_session;
            user_session = User_session.CreateSession("UnitTest_TestSessionCreation");
            Assert.AreEqual("UnitTest_TestSessionCreation", user_session.username);
            Assert.AreEqual(3, user_session.remaining_logins);
        }

        [TestMethod]
        public void TestSessionExistence()
        {
            User_session user_session;
            bool result = User_session.UserSessionExists(out user_session);
            Assert.IsFalse(result);
            Assert.IsNull(user_session);
            User_session.CreateSession("unittest_testsessionexistence");
            result = User_session.UserSessionExists(out user_session);
            Assert.IsTrue(result);
            Assert.IsNotNull(user_session);
        }

        [TestCleanup]
        public void TestCleanUp()
        {
            Directory.Delete("./sessions", true);
        }
    }
}
