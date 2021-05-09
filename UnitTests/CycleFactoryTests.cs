﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using Uceni_jazyku.Cycles;
using Uceni_jazyku.Cycles.UserCycles;

namespace UnitTests
{
    [TestClass]
    public class CycleFactoryTests
    {
        CycleFactory CycleFactory;

        [TestInitialize]
        public void Init()
        {
            CycleFactory = new CycleFactory();
        }

        [TestMethod]
        public void TestCreateCyclePositive()
        {
            // Test
            UserCycle result = CycleFactory.CreateCycle("test", "test_course");

            // Verify
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.CycleID);
            Assert.IsNotNull(result.DateCreated);
            Assert.AreEqual(UserCycleState.New, result.State);
            Assert.AreEqual("test", result.Username);
            Assert.AreEqual("test_course", result.CourseID);
        }

        [TestMethod]
        public void TestCreateIncompleteCyclePositive()
        {
            // Test
            IncompleteUserCycle result = CycleFactory.CreateIncompleteCycle("test", "test_course", 0);

            // Verify
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.CycleID);
            Assert.IsNotNull(result.DateCreated);
            Assert.AreEqual(UserCycleState.Inactive, result.State);
            Assert.AreEqual("test", result.Username);
            Assert.AreEqual("test_course", result.CourseID);
            Assert.AreEqual(0, result.limit);
        }
    }
}