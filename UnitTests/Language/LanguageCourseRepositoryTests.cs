using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using Uceni_jazyku.Language;
using Uceni_jazyku.Language.Impl;

namespace UnitTests.Language
{
    [TestClass]
    public class LanguageCourseRepositoryTests
    {
        LanguageCourseRepository languageCourseRepository;
        readonly LanguageCourse languageCourseActive1_1 = new SimpleLanguageCourse() { Active = true, CourseId = "active_id1", Username = "test" };
        readonly LanguageCourse languageCourseActive2_1 = new SimpleLanguageCourse() { Active = true, CourseId = "active_id2", Username = "tester" };
        readonly LanguageCourse languageCourseInactive1_1 = new SimpleLanguageCourse() { Active = false, CourseId = "inactive_id3", Username = "test" };
        readonly LanguageCourse languageCourseInactive1_2 = new SimpleLanguageCourse() { Active = false, CourseId = "inactive_id4", Username = "test" };

        [TestInitialize]
        public void Init()
        {
            List<LanguageCourse> languageCourses = new List<LanguageCourse>() { languageCourseActive1_1, languageCourseActive2_1, languageCourseInactive1_1, languageCourseInactive1_2 };

            languageCourseRepository = new LanguageCourseRepository(languageCourses);
        }

        [TestMethod]
        public void TestGetActiveCoursePositive()
        {
            // Test
            LanguageCourse result = languageCourseRepository.GetActiveCourse("test");

            // Verify
            Assert.AreEqual(languageCourseActive1_1, result);
            Assert.AreNotEqual(languageCourseActive2_1, result);
        }

        [TestMethod]
        public void TestGetActiveCourseNegative()
        {
            // Test
            LanguageCourse result = languageCourseRepository.GetActiveCourse("not_user");

            // Verify
            Assert.IsNull(result);
        }

        [TestMethod]
        public void TestGetInactiveLanguageCoursesPositive()
        {
            // Test
            List<LanguageCourse> result = languageCourseRepository.GetInactiveLanguageCourses("test");

            // Verify
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count == 2);
            Assert.AreEqual(languageCourseInactive1_1, result[0]);
            Assert.AreEqual(languageCourseInactive1_2, result[1]);
        }

        [TestMethod]
        public void TestGetInactiveLanguageCoursesNegative()
        {
            // Test
            List<LanguageCourse> result = languageCourseRepository.GetInactiveLanguageCourses("not_user");

            // Verify
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count == 0);
        }
    }
}
