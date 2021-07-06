using LanguageLearning.Language.Topic;
using System.Collections.Generic;

namespace LanguageLearning.Language
{
    /// <summary>
    /// Interface for operations helping linearize language course.
    /// </summary>
    public interface ILanguageTopicSelectStrategy
    {
        /// <summary>
        /// Select next topic from language course like its topics would be linearly ordered
        /// </summary>
        /// <returns>LanguageTopic</returns>
        LanguageTopic selectNextTopic();

        /// <summary>
        /// Select possible next topics from language course like its topics would be partially ordered
        /// </summary>
        /// <returns>List of LanguageTopic</returns>
        List<LanguageTopic> getNextLayer();
    }
}