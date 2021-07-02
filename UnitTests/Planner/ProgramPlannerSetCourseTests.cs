using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Uceni_jazyku.Language;
using Uceni_jazyku.Planner;
using Uceni_jazyku.User;

namespace UnitTests.Planner
{
    [TestClass]
    public class ProgramPlannerSetCourseTests
    {
        Mock<IUserModelRepository> userModelRepository;
        Mock<IPlannerRepository> plannerRepository;
        ProgramPlanner programPlanner;

        [TestInitialize]
        public void Init()
        {
            plannerRepository = new Mock<IPlannerRepository>();
            userModelRepository = new Mock<IUserModelRepository>();
            programPlanner = new ProgramPlanner(plannerRepository.Object, userModelRepository.Object);
        }

        [TestMethod]
        public void TestSetPlannerPositiveExistingMemory()
        {
            // Init
            Mock<LanguageCourse> languageCourse = new();
            Mock<PlannerMemory> plannerMemory = new();
            languageCourse.SetupGet(x => x.Id).Returns("course_id");
            plannerRepository.Setup(x => x.GetByCourseId("course_id")).Returns(plannerMemory.Object);

            // Test
            programPlanner.SetPlanner(languageCourse.Object, "test");

            // Verify
            languageCourse.Verify(x => x.Id, Times.Once);
            plannerRepository.Verify(x => x.GetByCourseId("course_id"), Times.Once);
            userModelRepository.Verify(x => x.GetByCourseId("course_id"), Times.Once);

            languageCourse.VerifyNoOtherCalls();
            plannerRepository.VerifyNoOtherCalls();
            plannerMemory.VerifyNoOtherCalls();
            userModelRepository.VerifyNoOtherCalls();
        }

        [TestMethod]
        public void TestSetPlannerPositiveNewMemory()
        {
            // Init
            Mock<LanguageCourse> languageCourse = new();
            languageCourse.SetupGet(x => x.Id).Returns("course_id");
            plannerRepository.Setup(x => x.GetByCourseId("course_id")).Returns((AbstractPlannerMemory)null);

            // Test
            programPlanner.SetPlanner(languageCourse.Object, "test");

            // Verify
            languageCourse.Verify(x => x.Id, Times.Once);
            plannerRepository.Verify(x => x.GetByCourseId("course_id"), Times.Once);
            plannerRepository.Verify(
                x => x.Create(It.Is<AbstractPlannerMemory>(x => x.CourseId == "course_id" && x.Id != null)), 
                Times.Once);
            userModelRepository.Verify(x => x.GetByCourseId("course_id"), Times.Once);

            languageCourse.VerifyNoOtherCalls();
            plannerRepository.VerifyNoOtherCalls();
            userModelRepository.VerifyNoOtherCalls();
        }
    }
}
