using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Uceni_jazyku.Language;
using Uceni_jazyku.Planner;

namespace UnitTests.Planner
{
    [TestClass]
    public class ProgramPlannerSetCourseTests
    {
        Mock<IPlannerRepository> plannerRepository;
        ProgramPlanner programPlanner;

        [TestInitialize]
        public void Init()
        {
            plannerRepository = new Mock<IPlannerRepository>();
            programPlanner = new ProgramPlanner(plannerRepository.Object);
        }

        [TestMethod]
        public void TestSetCoursePositiveExistingMemory()
        {
            // Init
            Mock<LanguageCourse> languageCourse = new();
            Mock<PlannerMemory> plannerMemory = new();
            languageCourse.SetupGet(x => x.CourseId).Returns("course_id");
            plannerRepository.Setup(x => x.GetMemory("course_id")).Returns(plannerMemory.Object);

            // Test
            programPlanner.SetCourse(languageCourse.Object);

            // Verify
            languageCourse.Verify(x => x.CourseId, Times.Once);
            plannerRepository.Verify(x => x.GetMemory("course_id"), Times.Once);

            languageCourse.VerifyNoOtherCalls();
            plannerRepository.VerifyNoOtherCalls();
            plannerMemory.VerifyNoOtherCalls();
        }

        [TestMethod]
        public void TestSetCoursePositiveNewMemory()
        {
            // Init
            Mock<LanguageCourse> languageCourse = new();
            languageCourse.SetupGet(x => x.CourseId).Returns("course_id");
            plannerRepository.Setup(x => x.GetMemory("course_id")).Returns((AbstractPlannerMemory)null);

            // Test
            programPlanner.SetCourse(languageCourse.Object);

            // Verify
            languageCourse.Verify(x => x.CourseId, Times.Exactly(2));
            plannerRepository.Verify(x => x.GetMemory("course_id"), Times.Once);
            plannerRepository.Verify(
                x => x.InsertMemory(It.Is<AbstractPlannerMemory>(x => x.CourseId == "course_id" && x.MemoryId != null)), 
                Times.Once);

            languageCourse.VerifyNoOtherCalls();
            plannerRepository.VerifyNoOtherCalls();
        }
    }
}
