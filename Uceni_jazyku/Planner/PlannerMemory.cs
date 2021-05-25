using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Uceni_jazyku.Language;

namespace Uceni_jazyku.Planner
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
