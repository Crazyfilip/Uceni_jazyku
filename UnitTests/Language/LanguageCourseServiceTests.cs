using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Uceni_jazyku.Language;

namespace UnitTests.Language
{
    [TestClass]
    public class LanguageCourseServiceTests
    {
        Mock<ILanguageCourseRepository> languageCourseRepositoryMock;

        LanguageCourseService languageCourseService;

        [TestInitialize]
        public void Init()
        {
            Mock<LanguageCourse> mock = new();
            languageCourseRepositoryMock = new Mock<ILanguageCourseRepository>();
            languageCourseService = LanguageCourseService.GetInstance(languageCourseRepositoryMock.Object);
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
    }
}
