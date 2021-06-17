using log4net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Reflection;
using System.Security.Cryptography;
using Uceni_jazyku.Cycles;
using Uceni_jazyku.Language;
using Uceni_jazyku.User;
using Uceni_jazyku.User_database;

namespace UnitTests
{
    [TestClass]
    public class UserAccountServiceTests
    {
        UserAccountService accountService;
        Mock<IUserAccountRepository> repositoryMock;
        Mock<UserAccount> accountMock;
        Mock<CycleService> cycleServiceMock;
        Mock<LanguageCourseService> languageCourseServiceMock;
        Mock<IUserModelRepository> userModelRepositoryMock;
        string saltMock;

        static readonly Mock<ILog> log4netMock = new Mock<ILog>();

        [ClassInitialize]
        public static void ClassInit(TestContext testContext)
        {
            var field = typeof(UserAccountService).GetField("log", BindingFlags.Static | BindingFlags.NonPublic);
            field.SetValue(null, log4netMock.Object);
        }

        [TestInitialize]
        public void Init()
        {
            accountMock = new Mock<UserAccount>();
            repositoryMock = new Mock<IUserAccountRepository>();
            cycleServiceMock = new Mock<CycleService>();
            languageCourseServiceMock = new Mock<LanguageCourseService>();
            userModelRepositoryMock = new Mock<IUserModelRepository>();
            accountService = new UserAccountService(
                repositoryMock.Object, 
                cycleServiceMock.Object, 
                languageCourseServiceMock.Object, 
                userModelRepositoryMock.Object);

            byte[] salt;
            new RNGCryptoServiceProvider().GetBytes(salt = new byte[16]);
            saltMock = Convert.ToBase64String(salt);
            log4netMock.Reset();
        }

        [TestMethod]
        public void TestCreateUserPositive()
        {
            // Init
            repositoryMock.Setup(x => x.GetUserAccount("test")).Returns((UserAccount)null);
            repositoryMock.Setup(x => x.AddUserAccount(It.IsAny<UserAccount>())).Verifiable();
            Mock<LanguageCourse> languageCourse = new();
            languageCourse.SetupGet(x => x.CourseId).Returns("course_id");
            languageCourseServiceMock.Setup(x => x.GetLanguageCourseInstanceFromTemplate("template-default", "test")).Returns(languageCourse.Object);

            // Test
            bool result = accountService.CreateUser("test", "test");

            // Verify
            Assert.IsTrue(result);

            repositoryMock.Verify(x => x.GetUserAccount("test"), Times.Once);
            repositoryMock.Verify(x => x.AddUserAccount(It.IsAny<UserAccount>()), Times.Once);
            log4netMock.Verify(x => x.Info("Verifying if there isn't already account with username test"), Times.Once);
            log4netMock.Verify(x => x.Info("Creating new user account with username test"), Times.Once);
            log4netMock.Verify(x => x.Debug("Calculating hash for user test"), Times.Once);
            log4netMock.Verify(x => x.Debug("Registering user to repository"), Times.Once);

            repositoryMock.VerifyNoOtherCalls();
            log4netMock.VerifyNoOtherCalls();
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
            log4netMock.Verify(x => x.Info("Verifying if there isn't already account with username test"), Times.Once);
            log4netMock.Verify(x => x.Debug("Account with username test already exists"), Times.Once);

            repositoryMock.VerifyNoOtherCalls();
            log4netMock.VerifyNoOtherCalls();
        }

        [TestMethod]
        public void TestLoginPositive()
        {
            // Init
            Mock<UserCycle> cycleMock = new Mock<UserCycle>();
            Mock<LanguageCourse> courseMock = new Mock<LanguageCourse>();

            repositoryMock.Setup(x => x.GetUserAccount("test")).Returns(accountMock.Object);
            accountMock.SetupGet(x => x.salt).Returns("CcDMwpJNSEIIBxhizhYGBw==");
            accountMock.SetupGet(x => x.loginCredential).Returns("2RN00+i/w/sl/0AmqkWTUOpOtig=");

            languageCourseServiceMock.Setup(x => x.GetActiveLanguageCourse("test")).Returns(courseMock.Object);

            cycleServiceMock.Setup(x => x.GetNextCycle("test")).Returns(cycleMock.Object);
            cycleServiceMock.Setup(x => x.SetActiveCourse("test", courseMock.Object, false)).Verifiable();

            // Test
            UserCycle result = accountService.Login("test", "test");

            // Verify
            Assert.AreEqual(cycleMock.Object, result);

            repositoryMock.Verify(x => x.GetUserAccount("test"), Times.Once);
            accountMock.Verify(x => x.salt, Times.Once);
            accountMock.Verify(x => x.loginCredential, Times.Once);
            cycleServiceMock.Verify(x => x.GetNextCycle("test"), Times.Once);
            cycleServiceMock.Verify(x => x.SetActiveCourse("test", courseMock.Object, false), Times.Once);
            log4netMock.Verify(x => x.Info("Login attempt of user test"), Times.Once);
            log4netMock.Verify(x => x.Debug("Calculating hash for user test"), Times.Once);
            log4netMock.Verify(x => x.Debug("Verifying if hash is correct"), Times.Once);
            log4netMock.Verify(x => x.Debug("Login successful"), Times.Once);

            repositoryMock.VerifyNoOtherCalls();
            accountMock.VerifyNoOtherCalls();
            cycleServiceMock.VerifyNoOtherCalls();
            log4netMock.VerifyNoOtherCalls();
        }


        [TestMethod]
        public void TestLoginNegativeNotExistingAccount()
        {
            // Init
            repositoryMock.Setup(x => x.GetUserAccount("fail")).Returns((UserAccount)null);

            // Test
            UserCycle result = accountService.Login("fail", "fail");

            // Verify
            Assert.IsNull(result);

            repositoryMock.Verify(x => x.GetUserAccount("fail"), Times.Once);
            log4netMock.Verify(x => x.Info("Login attempt of user fail"), Times.Once);
            log4netMock.Verify(x => x.Debug("No account with username: fail"), Times.Once);
            log4netMock.Verify(x => x.Debug("Login failed"), Times.Once);

            repositoryMock.VerifyNoOtherCalls();
            cycleServiceMock.VerifyNoOtherCalls();
            log4netMock.VerifyNoOtherCalls();
        }

        [TestMethod]
        public void TestLoginNegativeIncorrectCredentials()
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
            log4netMock.Verify(x => x.Info("Login attempt of user test"), Times.Once);
            log4netMock.Verify(x => x.Debug("Calculating hash for user test"), Times.Once);
            log4netMock.Verify(x => x.Debug("Verifying if hash is correct"), Times.Once);
            log4netMock.Verify(x => x.Debug("Login failed"), Times.Once);

            repositoryMock.VerifyNoOtherCalls();
            accountMock.VerifyNoOtherCalls();
            cycleServiceMock.VerifyNoOtherCalls();
            log4netMock.VerifyNoOtherCalls();
        }
    }
}
