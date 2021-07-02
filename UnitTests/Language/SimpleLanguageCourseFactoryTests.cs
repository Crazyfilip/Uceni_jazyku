using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uceni_jazyku.Language;
using Uceni_jazyku.Language.Impl;

namespace UnitTests.Language
{
    [TestClass]
    public class SimpleLanguageCourseFactoryTests
    {
        SimpleLanguageCourseFactory simpleLanguageCourseFactory;
        LanguageTopic topic1, topic2, topic3;
        TemplateLanguageCourse templateLanguageCourse;
        ICollection<LanguageTopic> topics;

        [TestInitialize]
        public void Init()
        {
            topic1 = new LanguageTopic() { TopicId = "topic1" };
            topic2 = new LanguageTopic() { TopicId = "topic2" };
            topic3 = new LanguageTopic() { TopicId = "topic3" };
            topics = new List<LanguageTopic>() { topic1, topic2, topic3 };
            templateLanguageCourse = new TemplateLanguageCourse(topics) { Id = "template" };
            simpleLanguageCourseFactory = new SimpleLanguageCourseFactory();
        }

        [TestMethod]
        public void TestGetLanguageCoursePositive()
        {
            // Test
            LanguageCourse result = simpleLanguageCourseFactory.GetLanguageCourse(templateLanguageCourse, "test");

            // Verify
            Assert.IsTrue(result is SimpleLanguageCourse);
            Assert.AreEqual(topics, result.topics);
            Assert.AreEqual("test", result.Username);
            Assert.IsNotNull(result.Id);
            Assert.AreNotEqual("template", result.Id);
        }

        [TestMethod]
        public void TestGetPartialLanguageCoursePositive()
        {
            // Init
            ICollection<LanguageTopic> expectedTopics = new List<LanguageTopic> { topic2 };

            // Test
            LanguageCourse result = simpleLanguageCourseFactory.GetPartialLanguageCourse(templateLanguageCourse, "test", new List<string>() { "topic2" });

            // Verify
            Assert.IsTrue(result is SimpleLanguageCourse);
            Assert.AreNotEqual(topics, result.topics);
            Assert.IsTrue(expectedTopics.SequenceEqual(result.topics));
            Assert.AreEqual("test", result.Username);
            Assert.IsNotNull(result.Id);
            Assert.AreNotEqual("template", result.Id);
        }
    }
}
