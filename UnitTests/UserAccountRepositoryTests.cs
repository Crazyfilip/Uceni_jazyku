using log4net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using Uceni_jazyku.User_database;

namespace UnitTests
{

    [TestClass]
    public class UserAccountRepositoryTests
    {
        private List<UserAccount> accounts;
        private UserAccountRepository userAccountRepository;
        private UserAccount userAccount1;
        private UserAccount userAccount2;
        static readonly Mock<ILog> log4netMock = new Mock<ILog>();

        [ClassInitialize]
        public static void ClassInit(TestContext testContext)
        {
            var field = typeof(UserAccountRepository).GetField("log", BindingFlags.Static | BindingFlags.NonPublic);
            field.SetValue(null, log4netMock.Object);
        }

        [TestInitialize]
        public void Init()
        {
            Directory.CreateDirectory("./users");
            userAccount1 = new UserAccount() { username = "test1", loginCredential = "test1", salt = "test1" };
            userAccount2 = new UserAccount() { username = "test2", loginCredential = "test2", salt = "test2" };
            accounts = new List<UserAccount>() { userAccount2 };
            userAccountRepository = new UserAccountRepository(accounts);
            log4netMock.Reset();
        }

        [TestMethod]
        public void TestAddUserAccountPostive()
        {
            // Test
            userAccountRepository.Create(userAccount1);

            // Verify
            Assert.IsTrue(accounts.Contains(userAccount1));

            log4netMock.Verify(x => x.Info("Adding user account to repository"), Times.Once);
            log4netMock.Verify(x => x.Debug("Saving repository to file"), Times.Once);

            log4netMock.VerifyNoOtherCalls();
        }

        [TestMethod]
        public void TestGetUserAccountPositive()
        {
            // Test
            UserAccount result = userAccountRepository.Get("test2");

            // Verify
            Assert.AreEqual(userAccount2, result);

            log4netMock.Verify(x => x.Info("Looking for account with username: test2"), Times.Once);

            log4netMock.VerifyNoOtherCalls();
        }

        [TestMethod]
        public void TestGetUserAccountNegative()
        {
            // Test
            UserAccount result = userAccountRepository.Get("non_existent");

            // Verify
            Assert.IsNull(result);

            log4netMock.Verify(x => x.Info("Looking for account with username: non_existent"), Times.Once);

            log4netMock.VerifyNoOtherCalls();
        }

        [TestCleanup]
        public void CleanUp()
        {
            Directory.Delete("./users", true);
        }
    }
}
