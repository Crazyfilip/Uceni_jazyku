using LanguageLearning.Language.Impl;
using LanguageLearning.Language.Topic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;

namespace UnitTests.Language.Impl
{
    [TestClass]
    public class SimpleLanguageCourseTests
    {
        SimpleLanguageCourse simpleLanguageCourse;
        Mock<LanguageTopic> languageTopic1;
        Mock<LanguageTopic> languageTopic2;
        Mock<LanguageTopic> languageTopic3;

        [TestInitialize]
        public void Init()
        {
            languageTopic1 = new Mock<LanguageTopic>();
            languageTopic1.SetupGet(x => x.Picked).Returns(true);
            languageTopic2 = new Mock<LanguageTopic>();
            languageTopic2.SetupGet(x => x.Picked).Returns(false);
            languageTopic3 = new Mock<LanguageTopic>();
            languageTopic3.SetupGet(x => x.Picked).Returns(false);
            simpleLanguageCourse = new SimpleLanguageCourse(new List<LanguageTopic>() { languageTopic1.Object, languageTopic2.Object, languageTopic3.Object });
        }

        [TestMethod]
        public void TestSelectNextTopicPositive()
        {
            // Test
            LanguageTopic result = simpleLanguageCourse.selectNextTopic();

            // Verify
            Assert.AreEqual(languageTopic2.Object, result);
            Assert.AreNotEqual(languageTopic1.Object, result);

            languageTopic1.Verify(x => x.Picked, Times.Once);
            languageTopic2.Verify(x => x.Picked, Times.Once);
            languageTopic3.Verify(x => x.Picked, Times.Never);

            languageTopic1.VerifyNoOtherCalls();
            languageTopic2.VerifyNoOtherCalls();
            languageTopic3.VerifyNoOtherCalls();
        }

        [TestMethod]
        public void TestGetNextLayercPositive()
        {
            // Test
            List<LanguageTopic> result = simpleLanguageCourse.getNextLayer();

            // Verify
            Assert.AreEqual(1, result.Count);

            LanguageTopic languageTopic = result[0];
            Assert.AreEqual(languageTopic2.Object, languageTopic);
            Assert.AreNotEqual(languageTopic1.Object, languageTopic);

            languageTopic1.Verify(x => x.Picked, Times.Once);
            languageTopic2.Verify(x => x.Picked, Times.Once);
            languageTopic3.Verify(x => x.Picked, Times.Never);

            languageTopic1.VerifyNoOtherCalls();
            languageTopic2.VerifyNoOtherCalls();
            languageTopic3.VerifyNoOtherCalls();
        }
    }
}
