using LanguageLearning.Language;
using LanguageLearning.Language.Topic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;

namespace UnitTests.Language
{
    [TestClass]
    public class LanguageCourseTests
    {
        Mock<LanguageCourse> languageCourseMock;
        Mock<LanguageTopic> languageTopic1;
        Mock<LanguageTopic> languageTopic2;

        [TestInitialize]
        public void Init()
        {
            languageCourseMock = new Mock<LanguageCourse>();
            languageTopic1 = new Mock<LanguageTopic>();
            languageTopic2 = new Mock<LanguageTopic>();

            languageTopic1.SetupGet(x => x.TopicId).Returns("topic");
            languageTopic2.SetupGet(x => x.TopicId).Returns("another_topic");

            ICollection<LanguageTopic> topics = new List<LanguageTopic>() { languageTopic1.Object, languageTopic2.Object };
            languageCourseMock.SetupGet(x => x.topics).Returns(topics);
            languageCourseMock.Setup(x => x.TopicPicked(It.IsAny<string>())).CallBase();
        }

        [TestMethod]
        public void TestPickTopicPositiveExistingTopic()
        {
            // Test
            languageCourseMock.Object.TopicPicked("topic");

            // Verify
            languageTopic1.Verify(x => x.TopicId, Times.Once);
            languageTopic1.Verify(x => x.TopicPicked(), Times.Once);
            languageTopic2.Verify(x => x.TopicId, Times.Never);
            languageTopic2.Verify(x => x.TopicPicked(), Times.Never);
            languageCourseMock.Verify(x => x.TopicPicked("topic"), Times.Once);
            languageCourseMock.Verify(x => x.topics, Times.Once);

            languageTopic1.VerifyNoOtherCalls();
            languageTopic2.VerifyNoOtherCalls();
            languageCourseMock.VerifyNoOtherCalls();
        }

        [TestMethod]
        public void TestPickTopicPositiveNotExistingTopic()
        {
            // Test
            languageCourseMock.Object.TopicPicked("different_topic");

            // Verify
            languageTopic1.Verify(x => x.TopicId, Times.Once);
            languageTopic1.Verify(x => x.TopicPicked(), Times.Never);
            languageTopic2.Verify(x => x.TopicId, Times.Once);
            languageTopic2.Verify(x => x.TopicPicked(), Times.Never);
            languageCourseMock.Verify(x => x.TopicPicked("different_topic"), Times.Once);
            languageCourseMock.Verify(x => x.topics, Times.Once);

            languageTopic1.VerifyNoOtherCalls();
            languageTopic2.VerifyNoOtherCalls();
            languageCourseMock.VerifyNoOtherCalls();
        }
    }
}
