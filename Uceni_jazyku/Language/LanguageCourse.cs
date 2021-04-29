using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uceni_jazyku.Language
{
    /// <summary>
    /// Abstract form of language course
    /// Topics are stored in generic collection
    /// </summary>
    public abstract class LanguageCourse : ILanguageTopicSelectStrategy
    {
        public virtual string CourseId { get; init; }
        public string Username { get; init; }
        public bool Active { get; set; }

        public virtual ICollection<LanguageTopic> topics { get; protected set; }

        public abstract List<LanguageTopic> getNextLayer();

        /// <summary>
        /// Set in course that topic was picked by planner or user
        /// </summary>
        /// <param name="topicId">id of topic</param>
        public virtual void TopicPicked(string topicId)
        {
            LanguageTopic t = topics.Where(x => x.TopicId == topicId).FirstOrDefault();
            t?.TopicPicked();
        }

        public abstract LanguageTopic selectNextTopic();
    }
}
