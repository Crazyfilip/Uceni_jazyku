using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using Uceni_jazyku.Cycles;
using Uceni_jazyku.User_database;

namespace UnitTests
{
    [TestClass]
    public class UserAccountsTests
    {
        UserAccountService accountService;

        [TestInitialize]
        public void Init()
        {
            Directory.CreateDirectory("./users");
            Directory.CreateDirectory("./cycles/service");
            Directory.CreateDirectory("./cycles/user-new");
            Directory.CreateDirectory("./cycles/user-active");
            accountService = UserAccountService.GetInstance();
        }

        [TestMethod]
        public void CreateAndLoginTest()
        {
            Assert.IsTrue(accountService.CreateUser("test", "test"));
            UserActiveCycle cycle = accountService.Login("test", "test");
            Assert.IsNotNull(cycle);
            Assert.AreEqual("test", cycle.Username);
        }

        [TestMethod]
        public void WhenLoginFailed_thenReturnNull()
        {
            Assert.IsNull(accountService.Login("fail", "fail"));
        }

        [TestMethod]
        public void CantCreateUserWithExistingName()
        {
            Assert.IsTrue(accountService.CreateUser("test", "test"));
            Assert.IsFalse(accountService.CreateUser("test", "password"));
        }

        [TestCleanup]
        public void CleanUp()
        {
            Directory.Delete("./cycles", true);
            Directory.Delete("./users", true);
            UserAccountService.Deallocate();
        }
    }
}
