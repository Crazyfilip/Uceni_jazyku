using log4net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Reflection;
using LanguageLearning.Common;
using LanguageLearning.User_database;

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
        Mock<Serializer<UserAccount>> serializer;

        [ClassInitialize]
        public static void ClassInit(TestContext testContext)
        {
            var field = typeof(UserAccountRepository).GetField("log", BindingFlags.Static | BindingFlags.NonPublic);
            field.SetValue(null, log4netMock.Object);
        }

        [TestInitialize]
        public void Init()
        {
            serializer = new Mock<Serializer<UserAccount>>();
            userAccountRepository = new UserAccountRepository(serializer.Object);
            log4netMock.Reset();
        }

        [TestMethod]
        public void TestGetUserAccountPositive()
        {
            // Init
            Mock<UserAccount> account = new();
            account.SetupGet(x => x.username).Returns("test");
            List<UserAccount> accounts = new() { account.Object };
            serializer.Setup(x => x.Load()).Returns(accounts);

            // Test
            UserAccount result = userAccountRepository.GetByName("test");

            // Verify
            Assert.AreEqual(account.Object, result);

            log4netMock.Verify(x => x.Info("Looking for account with username: test"), Times.Once);

        }

        [TestMethod]
        public void TestGetUserAccountNegative()
        {
            // Init
            serializer.Setup(x => x.Load()).Returns(new List<UserAccount>());

            // Test
            UserAccount result = userAccountRepository.GetByName("non_existent");

            // Verify
            Assert.IsNull(result);

            log4netMock.Verify(x => x.Info("Looking for account with username: non_existent"), Times.Once);
        }

        [TestCleanup]
        public void CleanUp()
        {
            serializer.Verify(x => x.Load(), Times.Once);

            serializer.VerifyNoOtherCalls();
            log4netMock.VerifyNoOtherCalls();
        }
    }
}
