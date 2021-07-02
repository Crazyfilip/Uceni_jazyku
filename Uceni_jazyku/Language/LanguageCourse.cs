using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Uceni_jazyku.Common;
using Uceni_jazyku.Language.Impl;

namespace Uceni_jazyku.Language
{
    /// <summary>
    /// Abstract form of language course
    /// Topics are stored in generic collection
    /// </summary>
    [DataContract]
    [KnownType(typeof(TemplateLanguageCourse))]
    [KnownType(typeof(SimpleLanguageCourse))]
    public abstract class LanguageCourse : ILanguageTopicSelectStrategy, IId
    {
        [DataMember]
        public virtual string Id { get; init; }

        [DataMember]
        public virtual string Username { get; init; }

        [DataMember]
        public virtual bool Active { get; set; }

        [DataMember]
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
