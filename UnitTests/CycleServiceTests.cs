using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;
using Uceni_jazyku.Cycles;
using Uceni_jazyku.Cycles.Program;
using Uceni_jazyku.Cycles.UserCycles;
using Uceni_jazyku.Cycles.LanguageCycles;
using System.Collections.Generic;
using System.Linq;

namespace UnitTests
{
    /// <summary>
    /// Tests for cycle service
    /// </summary>
    [TestClass]
    public class CycleServiceTests
    {
        CycleService service;
        CycleDatabase database;
        UserCycle cycle;
        private readonly string activeCycleCacheFile = "cycles/service/active-cycle.xml";

        private void PrepareCachedActiveCycle()
        {
            cycle = new UserCycle().AssignUser("test").Activate();
            var serializer = new DataContractSerializer(typeof(UserCycle));
            using XmlWriter writer = XmlWriter.Create(activeCycleCacheFile);
            serializer.WriteObject(writer, cycle);
        }
        
        [TestInitialize]
        public void TestInitialization()
        {
            Directory.CreateDirectory("./cycles/service");
            database = new CycleDatabase();
            service = CycleService.GetInstance(database);
        }

        [TestMethod]
        public void TestActiveCycleExistsPositive()
        {
            PrepareCachedActiveCycle();
            bool result = service.UserActiveCycleExists();
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void TestActiveCycleExistsNegative()
        {
            bool result = service.UserActiveCycleExists();
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void TestGetUserCycleCreateNew()
        {
            Assert.AreEqual(0, database.GetCyclesCount());
            UserCycle result = service.GetUserCycle("test");
            Assert.AreEqual(UserCycleState.Active, result.State);
            Assert.AreEqual("test", result.Username);
            Assert.AreEqual(2, database.GetCyclesCount()); // result + language cycle
        }

        [TestMethod]
        public void TestGetUserCycleFromInactive()
        {
            cycle = new UserCycle().AssignUser("test").Activate().Inactivate();
            database.PutCycle(cycle);
            Assert.AreEqual(1, database.GetCyclesCount());
            Assert.AreEqual(UserCycleState.Inactive, cycle.State);

            UserCycle result = service.GetUserCycle("test");
            Assert.AreSame(cycle, result);
            Assert.AreEqual(UserCycleState.Active, result.State);
            Assert.AreEqual("test", result.Username);
            Assert.AreEqual(1, database.GetCyclesCount());
        }

        [TestMethod]
        public void TestGetActiveCycleNegative()
        {
            Assert.ThrowsException<FileNotFoundException>(() => service.GetActiveCycle());
        }

        [TestMethod]
        public void TestGetActiveCyclePositive()
        {
            PrepareCachedActiveCycle();
            UserCycle result = service.GetActiveCycle();
            Assert.AreEqual(cycle, result);
        }

        [TestMethod]
        public void TestGetNewCycle()
        {
            UserCycle result = service.GetNewCycle("test");
            Assert.IsTrue(database.IsInDatabase(result));
            Assert.AreEqual(UserCycleState.New, result.State);
            Assert.AreEqual("test", result.Username);
        }

        [TestMethod]
        public void TestActivateNewCycle()
        {
            LanguageProgramItem languageItem = LanguageCycle.LanguageCycleExample().PlanNext();
            cycle = new UserCycle().AssignUser("test");
            database.PutCycle(cycle);
            Assert.AreEqual(UserCycleState.New, cycle.State);

            UserCycle result = service.Activate(cycle);
            Assert.AreSame(cycle, result);
            Assert.AreEqual(UserCycleState.Active, result.State);
            Assert.IsTrue(database.IsInDatabase(result));
            Assert.IsTrue(File.Exists(activeCycleCacheFile));

            UserProgramItem userItem = (UserProgramItem) result.GetNext();
            Assert.AreEqual(languageItem, userItem.LessonRef);
        }

        [TestMethod]
        public void TestActivateInactiveCycle()
        {
            cycle = new UserCycle().AssignUser("test").Activate().Inactivate();
            database.PutCycle(cycle);
            Assert.AreEqual(UserCycleState.Inactive, cycle.State);

            UserCycle result = service.Activate(cycle);
            Assert.AreSame(cycle, result);
            Assert.AreEqual(UserCycleState.Active, result.State);
            Assert.IsTrue(database.IsInDatabase(result));
            Assert.IsTrue(File.Exists(activeCycleCacheFile));
        }

        [TestMethod]
        public void TestActivateNegative()
        {
            Assert.ThrowsException<Exception>(() => service.Activate(new UserCycle()));
        }

        [TestMethod]
        public void TestInactivateCycle()
        {
            PrepareCachedActiveCycle();
            database.PutCycle(cycle);
            Assert.AreEqual(UserCycleState.Active, cycle.State);
            Assert.IsTrue(File.Exists(activeCycleCacheFile));

            UserCycle result = service.Inactivate(cycle);
            Assert.AreSame(cycle, result);
            Assert.AreEqual(UserCycleState.Inactive, result.State);
            Assert.IsTrue(database.IsInDatabase(result));
            Assert.IsFalse(File.Exists(activeCycleCacheFile));
        }

        [TestMethod]
        public void TestInactivateNegative()
        {
            Assert.ThrowsException<Exception>(() => service.Inactivate(new UserCycle()));
        }

        [TestMethod]
        public void TestFinishCycle()
        {
            PrepareCachedActiveCycle();
            LanguageCycle example = LanguageCycle.LanguageCycleExample();
            cycle.AssignProgram(new List<UserProgramItem>() { new UserProgramItem(example.CycleID, example.PlanNext()) });
            cycle.Update();
            database.PutCycle(cycle);

            Assert.AreEqual(UserCycleState.Active, cycle.State);
            Assert.IsTrue(File.Exists(activeCycleCacheFile));

            service.Finish(cycle);

            Assert.AreEqual(UserCycleState.Finished, cycle.State);
            Assert.IsFalse(File.Exists(activeCycleCacheFile));
        }

        [TestMethod]
        public void TestFinishNegative()
        {
            Assert.ThrowsException<Exception>(() => service.Finish(new UserCycle()));
        }

        [TestMethod]
        public void TestRegisterCycle()
        {
            cycle = new UserCycle().AssignUser("test").Activate().Inactivate();
            Assert.IsNull(cycle.CycleID);
            Assert.IsFalse(database.IsInDatabase(cycle));

            service.RegisterCycle(cycle);

            Assert.IsNotNull(cycle.CycleID);
            Assert.IsTrue(database.IsInDatabase(cycle));
        }

        [TestMethod]
        public void TestSwapLessonNewIncomplete()
        {
            LanguageCycle example = LanguageCycle.LanguageCycleExample();
            UserProgramItem item1 = new UserProgramItem(example.CycleID, example.PlanNext());
            UserProgramItem item2 = new UserProgramItem(example.CycleID, example.PlanNext());
            UserProgramItem item3 = new UserProgramItem(example.CycleID, example.PlanNext());
            cycle = new UserCycle().AssignUser("test").AssignProgram(new List<UserProgramItem>() { item1, item2 });
            database.PutCycle(cycle);
            CollectionAssert.AreEqual(new List<UserProgramItem>() { item1, item2 }, cycle.UserProgramItems);

            service.SwapLesson(cycle, item3);

            CollectionAssert.AreEqual(new List<UserProgramItem>() { item3, item1 }, cycle.UserProgramItems);

            IncompleteUserCycle incompleteCycle = new IncompleteUserCycle("test");
            incompleteCycle.AddLesson(item2);
            incompleteCycle.CycleID = "cycle1";
            Assert.IsTrue(database.IsInDatabase(incompleteCycle));
        }

        [TestMethod]
        public void TestSwapLessonExistingIncomplete()
        {
            LanguageCycle example = LanguageCycle.LanguageCycleExample();
            UserProgramItem item1 = new UserProgramItem(example.CycleID, example.PlanNext());
            UserProgramItem item2 = new UserProgramItem(example.CycleID, example.PlanNext());
            UserProgramItem item3 = new UserProgramItem(example.CycleID, example.PlanNext());
            UserProgramItem item4 = new UserProgramItem(example.CycleID, example.PlanNext());
            cycle = new UserCycle().AssignUser("test").AssignProgram(new List<UserProgramItem>() { item1, item2 });
            database.PutCycle(cycle);
            CollectionAssert.AreEqual(new List<UserProgramItem>() { item1, item2 }, cycle.UserProgramItems);
            IncompleteUserCycle incompleteCycle = new IncompleteUserCycle("test");
            incompleteCycle.AddLesson(item4);
            database.PutCycle(incompleteCycle);

            service.SwapLesson(cycle, item3);

            CollectionAssert.AreEqual(new List<UserProgramItem>() { item3, item1 }, cycle.UserProgramItems);
            CollectionAssert.AreEqual(new List<UserProgramItem>() { item2, item4 }, incompleteCycle.UserProgramItems);
        }

        [TestCleanup]
        public void TestCleanUp()
        {
            database = null;
            CycleService.DeallocateInstance();
            Directory.Delete("./cycles", true);
        }
    }
}
