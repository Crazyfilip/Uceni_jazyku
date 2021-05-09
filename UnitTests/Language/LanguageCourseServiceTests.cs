using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Uceni_jazyku.Language;
using Uceni_jazyku.Language.Impl;

namespace UnitTests.Language
{
    [TestClass]
    public class LanguageCourseServiceTests
    {
        Mock<ILanguageCourseRepository> languageCourseRepositoryMock;
        Mock<ILanguageCourseFactory> languageCourseFactoryMock;

        LanguageCourseService languageCourseService;

        [TestInitialize]
        public void Init()
        {
            languageCourseRepositoryMock = new Mock<ILanguageCourseRepository>();
            languageCourseFactoryMock = new Mock<ILanguageCourseFactory>();
            languageCourseService = LanguageCourseService.GetInstance(languageCourseRepositoryMock.Object, languageCourseFactoryMock.Object);
        }

        [TestMethod]
        public void TestGetActiveLanguageCoursePositive()
        {
            // Init
            Mock<LanguageCourse> languageCourseMock = new();
            languageCourseRepositoryMock.Setup(x => x.GetActiveCourse("test")).Returns(languageCourseMock.Object);

            // Test
            LanguageCourse result = languageCourseService.GetActiveLanguageCourse("test");

            // Verify
            Assert.AreEqual(languageCourseMock.Object, result);

            languageCourseRepositoryMock.Verify(x => x.GetActiveCourse("test"), Times.Once);

            languageCourseMock.VerifyNoOtherCalls();
            languageCourseRepositoryMock.VerifyNoOtherCalls();
        }

        // TODO TestGetActiveLanguageCourseNegative

        [TestMethod]
        public void TestGetLanguageCourseInstanceFromTemplatePositive()
        {
            // Init
            Mock<TemplateLanguageCourse> templateMock = new();
            Mock<LanguageCourse> languageCourseMock = new();

            languageCourseRepositoryMock.Setup(x => x.GetTemplate("template_id")).Returns(templateMock.Object);
            languageCourseFactoryMock.Setup(x => x.GetLanguageCourse(templateMock.Object, "test")).Returns(languageCourseMock.Object);

            // Test
            LanguageCourse result = languageCourseService.GetLanguageCourseInstanceFromTemplate("template_id", "test");

            // Verify
            Assert.AreEqual(languageCourseMock.Object, result);

            languageCourseRepositoryMock.Verify(x => x.GetTemplate("template_id"), Times.Once);
            languageCourseRepositoryMock.Verify(x => x.InsertCourse(languageCourseMock.Object), Times.Once);
            languageCourseFactoryMock.Verify(x => x.GetLanguageCourse(templateMock.Object, "test"), Times.Once);

            templateMock.VerifyNoOtherCalls();
            languageCourseMock.VerifyNoOtherCalls();
            languageCourseRepositoryMock.VerifyNoOtherCalls();
            languageCourseFactoryMock.VerifyNoOtherCalls();
        }

        [TestCleanup]
        public void CleanUp()
        {
            LanguageCourseService.deallocate();
        }
    }
}
