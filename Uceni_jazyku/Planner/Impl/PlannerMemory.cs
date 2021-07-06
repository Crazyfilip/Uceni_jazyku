using LanguageLearning.Language.Topic;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace LanguageLearning.Planner
{
    /// <inheritdoc/>
    /// <summary>
    /// Memory is represent via queue
    /// </summary>
    [DataContract]
    public class PlannerMemory : AbstractPlannerMemory
    {
        [DataMember]
        Queue<LanguageTopic> unfinishedTopics = new Queue<LanguageTopic>();

        /// <inheritdoc/>
        public override bool AnyUnfinishedTopic()
        {
            return unfinishedTopics.TryPeek(out _);
        }

        /// <inheritdoc/>
        public override LanguageTopic GetNextTopic()
        {
            return unfinishedTopics.Dequeue();
        }

        /// <inheritdoc/>
        public override void InsertTopic(LanguageTopic topic)
        {
            unfinishedTopics.Enqueue(topic);
        }
    }
}
