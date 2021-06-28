using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uceni_jazyku.Planner;

namespace UnitTests.Planner
{
    [TestClass]
    public class PlannerRepositoryTests
    {
        PlannerRepository plannerRepository;
        List<AbstractPlannerMemory> plannerMemories;
        PlannerMemory memory;

        [TestInitialize]
        public void Init()
        {
            Directory.CreateDirectory("./planners");

            memory = new PlannerMemory() { CourseId = "course_id", MemoryId = "memory_id"};
            plannerMemories = new List<AbstractPlannerMemory>() { memory };
            plannerRepository = new PlannerRepository(plannerMemories);
        }

        [TestMethod]
        public void TestGetMemoryPositive()
        {
            // Test
            AbstractPlannerMemory result = plannerRepository.Get("course_id");

            // Verify
            Assert.IsNotNull(result);
            Assert.AreEqual(memory, result);
        }
        [TestMethod]
        public void TestGetMemoryNegative()
        {
            // Test
            AbstractPlannerMemory result = plannerRepository.Get("invalid");

            // Verify
            Assert.IsNull(result);
        }

        [TestMethod]
        public void TestInsertMemoryPositive()
        {
            // Init
            AbstractPlannerMemory memoryAdd = new PlannerMemory() { CourseId = "course2_id" };

            // Preverify
            Assert.AreEqual(1, plannerMemories.Count);
            Assert.IsFalse(plannerMemories.Contains(memoryAdd));

            // Test
            plannerRepository.Create(memoryAdd);

            // Verify
            Assert.AreEqual(2, plannerMemories.Count);
            Assert.IsTrue(plannerMemories.Contains(memoryAdd));
        }

        [TestMethod]
        public void TestUpdateMemoryPositive()
        {
            // Init
            AbstractPlannerMemory memoryUpdate = new PlannerMemory() { MemoryId = "memory_id" };

            // Preverify
            Assert.AreEqual(1, plannerMemories.Count);
            Assert.IsTrue(plannerMemories.Contains(memory));
            Assert.IsFalse(plannerMemories.Contains(memoryUpdate));

            // Test
            plannerRepository.Update(memoryUpdate);

            // Verify
            Assert.AreEqual(1, plannerMemories.Count);
            Assert.IsFalse(plannerMemories.Contains(memory));
            Assert.IsTrue(plannerMemories.Contains(memoryUpdate));
        }

        [TestCleanup]
        public void CleanUp()
        {
            Directory.Delete("./planners", true);
        }
    }
}
