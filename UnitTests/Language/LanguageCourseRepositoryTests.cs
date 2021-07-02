using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.IO;
using Uceni_jazyku.Common;
using Uceni_jazyku.Language;
using Uceni_jazyku.Language.Impl;

namespace UnitTests.Language
{
    [TestClass]
    public class LanguageCourseRepositoryTests
    {
        LanguageCourseRepository languageCourseRepository;
        readonly LanguageCourse languageCourseActive1_1 = new SimpleLanguageCourse() { Active = true, Id = "active_id1", Username = "test" };
        readonly LanguageCourse languageCourseActive2_1 = new SimpleLanguageCourse() { Active = true, Id = "active_id2", Username = "tester" };
        readonly LanguageCourse languageCourseInactive1_1 = new SimpleLanguageCourse() { Active = false, Id = "inactive_id3", Username = "test" };
        readonly LanguageCourse languageCourseInactive1_2 = new SimpleLanguageCourse() { Active = false, Id = "inactive_id4", Username = "test" };
        readonly LanguageCourse templateLanguageCourse = new TemplateLanguageCourse(new List<LanguageTopic>()) { Id = "template_id" };
        readonly LanguageCourse testLanguageCourse = new SimpleLanguageCourse() { Id = "test" };
        List<LanguageCourse> languageCourses;
        Mock<Serializer<LanguageCourse>> serializer;

        [TestInitialize]
        public void Init()
        {
            serializer = new Mock<Serializer<LanguageCourse>>();
            languageCourseRepository = new LanguageCourseRepository(serializer.Object);
        }

        [TestMethod]
        public void TestGetActiveCoursePositive()
        {
            // Init
            Mock<LanguageCourse> course = new();
            course.SetupGet(x => x.Active).Returns(true);
            course.SetupGet(x => x.Username).Returns("test");
            List<LanguageCourse> courses = new() { course.Object };
            serializer.Setup(x => x.Load()).Returns(courses);

            // Test
            LanguageCourse result = languageCourseRepository.GetActiveCourse("test");

            // Verify
            Assert.AreEqual(course.Object, result);
        }

        [TestMethod]
        public void TestGetActiveCourseNegative()
        {
            // Init
            serializer.Setup(x => x.Load()).Returns(new List<LanguageCourse>());

            // Test
            LanguageCourse result = languageCourseRepository.GetActiveCourse("not_user");

            // Verify
            Assert.IsNull(result);
        }

        [TestMethod]
        public void TestGetInactiveLanguageCoursesPositive()
        {
            // Init
            Mock<LanguageCourse> course = new();
            course.SetupGet(x => x.Active).Returns(false);
            course.SetupGet(x => x.Username).Returns("test");
            List<LanguageCourse> courses = new() { course.Object };
            serializer.Setup(x => x.Load()).Returns(courses);

            // Test
            List<LanguageCourse> result = languageCourseRepository.GetInactiveLanguageCourses("test");

            // Verify
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(course.Object, result[0]);
        }

        [TestMethod]
        public void TestGetInactiveLanguageCoursesNegative()
        {
            // Init
            serializer.Setup(x => x.Load()).Returns(new List<LanguageCourse>());

            // Test
            List<LanguageCourse> result = languageCourseRepository.GetInactiveLanguageCourses("not_user");

            // Verify
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        public void TestGetTemplatePositive()
        {
            // Init
            Mock<TemplateLanguageCourse> template = new();
            template.SetupGet(x => x.Id).Returns("template_id");
            List<LanguageCourse> courses = new List<LanguageCourse>() { template.Object };
            serializer.Setup(x => x.Load()).Returns(courses);

            // Test
            TemplateLanguageCourse result = languageCourseRepository.GetTemplate("template_id");

            // Verify
            Assert.AreEqual(template.Object, result);
        }

        [TestMethod]
        public void TestGetTemplateNegative()
        {
            // Init
            serializer.Setup(x => x.Load()).Returns(new List<LanguageCourse>());

            // Test
            TemplateLanguageCourse result = languageCourseRepository.GetTemplate("no_template");

            // Verify
            Assert.IsNull(result);
        }

        [TestCleanup]
        public void Finish()
        {
            serializer.Verify(x => x.Load(), Times.Once);
            serializer.VerifyNoOtherCalls();
        }
    }
}
