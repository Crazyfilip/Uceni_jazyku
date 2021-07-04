using LanguageLearning.Common;
using LanguageLearning.Planner;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;

namespace UnitTests.Planner.Impl
{
    [TestClass]
    public class PlannerRepositoryTests
    {
        PlannerRepository plannerRepository;
        List<AbstractPlannerMemory> plannerMemories;
        PlannerMemory memory;
        Mock<Serializer<AbstractPlannerMemory>> serializer;

        [TestInitialize]
        public void Init()
        {
            serializer = new Mock<Serializer<AbstractPlannerMemory>>();
            memory = new PlannerMemory() { CourseId = "test" };
            plannerMemories = new List<AbstractPlannerMemory>() { memory };
            plannerRepository = new PlannerRepository(serializer.Object);
        }

        [TestMethod]
        public void TestGetByCourseIdPositive()
        {
            // Init
            Mock<AbstractPlannerMemory> memory = new();
            memory.SetupGet(x => x.CourseId).Returns("test");
            List<AbstractPlannerMemory> plannerMemories = new() { memory.Object };
            serializer.Setup(x => x.Load()).Returns(plannerMemories);

            // Test
            AbstractPlannerMemory result = plannerRepository.GetByCourseId("test");

            // Verify
            Assert.AreEqual(memory.Object, result);

            memory.Verify(x => x.CourseId, Times.Once);
            serializer.Verify(x => x.Load(), Times.Once);

            memory.VerifyNoOtherCalls();
        }

        [TestMethod]
        public void TestGetByCourseIdNegative()
        {
            // Init
            serializer.Setup(x => x.Load()).Returns(new List<AbstractPlannerMemory>());

            // Test
            AbstractPlannerMemory result = plannerRepository.GetByCourseId("test");

            // Verify
            Assert.IsNull(result);

            serializer.Verify(x => x.Load(), Times.Once);
        }

        [TestCleanup]
        public void Finish()
        {
            serializer.VerifyNoOtherCalls();
        }
    }
}
