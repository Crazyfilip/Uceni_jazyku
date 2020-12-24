using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.IO;
using System.Security.Cryptography;
using Uceni_jazyku.Cycles;
using Uceni_jazyku.User_database;

namespace UnitTests
{
    [TestClass]
    public class UserAccountsTests
    {
        UserAccountService accountService;
        Mock<IUserAccountRepository> repositoryMock;
        Mock<UserAccount> accountMock;
        Mock<CycleService> cycleServiceMock;
        String saltMock;

        [TestInitialize]
        public void Init()
        {
            accountMock = new Mock<UserAccount>();
            repositoryMock = new Mock<IUserAccountRepository>();
            cycleServiceMock = new Mock<CycleService>();
            accountService = new UserAccountService(repositoryMock.Object, cycleServiceMock.Object);

            byte[] salt;
            new RNGCryptoServiceProvider().GetBytes(salt = new byte[16]);
            saltMock = Convert.ToBase64String(salt);
        }

        [TestMethod]
        public void TestCreateUserPositive()
        {
            // Init
            repositoryMock.Setup(x => x.GetUserAccount("test")).Returns((UserAccount)null);
            repositoryMock.Setup(x => x.AddUserAccount(It.IsAny<UserAccount>())).Verifiable();

            // Test
            bool result = accountService.CreateUser("test", "test");

            // Verify
            Assert.IsTrue(result);

            repositoryMock.Verify(x => x.GetUserAccount("test"), Times.Once);
            repositoryMock.Verify(x => x.AddUserAccount(It.IsAny<UserAccount>()), Times.Once);

            repositoryMock.VerifyNoOtherCalls();
        }

        [TestMethod]
        public void TestCreateUserNegative()
        {
            // Init
            repositoryMock.Setup(x => x.GetUserAccount("test")).Returns(accountMock.Object);

            // Test
            bool result = accountService.CreateUser("test", "test");

            // Verify
            Assert.IsFalse(result);

            repositoryMock.Verify(x => x.GetUserAccount("test"), Times.Once);

            repositoryMock.VerifyNoOtherCalls();
        }

        [TestMethod]
        public void TestLoginPositive()
        {
            // Init
            Mock<UserCycle> cycleMock = new Mock<UserCycle>();

            repositoryMock.Setup(x => x.GetUserAccount("test")).Returns(accountMock.Object);
            accountMock.SetupGet(x => x.salt).Returns("CcDMwpJNSEIIBxhizhYGBw==");
            accountMock.SetupGet(x => x.loginCredential).Returns("2RN00+i/w/sl/0AmqkWTUOpOtig=");
            cycleServiceMock.Setup(x => x.GetUserCycle("test")).Returns(cycleMock.Object);

            // Test
            UserCycle result = accountService.Login("test", "test");

            // Verify
            Assert.AreEqual(cycleMock.Object, result);

            repositoryMock.Verify(x => x.GetUserAccount("test"), Times.Once);
            accountMock.Verify(x => x.salt, Times.Once);
            accountMock.Verify(x => x.loginCredential, Times.Once);
            cycleServiceMock.Verify(x => x.GetUserCycle("test"), Times.Once);

            repositoryMock.VerifyNoOtherCalls();
            accountMock.VerifyNoOtherCalls();
            cycleServiceMock.VerifyNoOtherCalls();
        }


        [TestMethod]
        public void TestLoginNotExistingAccount()
        {
            // Init
            repositoryMock.Setup(x => x.GetUserAccount("fail")).Returns((UserAccount)null);

            // Test
            UserCycle result = accountService.Login("fail", "fail");

            // Verify
            Assert.IsNull(result);

            repositoryMock.Verify(x => x.GetUserAccount("fail"), Times.Once);

            repositoryMock.VerifyNoOtherCalls();
            cycleServiceMock.VerifyNoOtherCalls();
        }

        [TestMethod]
        public void TestLoginIncorrectCredentials()
        {
            // Init
            repositoryMock.Setup(x => x.GetUserAccount("test")).Returns(accountMock.Object);
            accountMock.SetupGet(x => x.salt).Returns(saltMock);
            accountMock.SetupGet(x => x.loginCredential).Returns("incorrect");

            // Test
            UserCycle result = accountService.Login("test", "test");

            // Verify
            Assert.IsNull(result);

            repositoryMock.Verify(x => x.GetUserAccount("test"), Times.Once);
            accountMock.Verify(x => x.salt, Times.Once);
            accountMock.Verify(x => x.loginCredential, Times.Once);

            repositoryMock.VerifyNoOtherCalls();
            accountMock.VerifyNoOtherCalls();
            cycleServiceMock.VerifyNoOtherCalls();
        }
    }
}
