using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
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

        [TestInitialize]
        public void Init()
        {
            Directory.CreateDirectory("./users");
            userAccount1 = new UserAccount() { username = "test1", loginCredential = "test1", salt = "test1" };
            userAccount2 = new UserAccount() { username = "test2", loginCredential = "test2", salt = "test2" };
            accounts = new List<UserAccount>() { userAccount2 };
            userAccountRepository = new UserAccountRepository(accounts);
        }

        [TestMethod]
        public void TestAddUserAccountPostive()
        {
            // Test
            userAccountRepository.AddUserAccount(userAccount1);

            // Verify
            Assert.IsTrue(accounts.Contains(userAccount1));
        }

        [TestMethod]
        public void TestGetUserAccountPositive()
        {
            // Test
            UserAccount result = userAccountRepository.GetUserAccount("test2");

            // Verify
            Assert.AreEqual(userAccount2, result);
        }

        [TestMethod]
        public void TestGetUserAccountNegative()
        {
            // Test
            UserAccount result = userAccountRepository.GetUserAccount("non_existent");

            // Verify
            Assert.IsNull(result);
        }

        [TestCleanup]
        public void CleanUp()
        {
            Directory.Delete("./users", true);
        }
    }
}
