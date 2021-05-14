using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Uceni_jazyku.Language;

namespace Uceni_jazyku.Planner
{
    [DataContract]
    public class PlannerMemory : AbstractPlannerMemory
    {
        [DataMember]
        Queue<LanguageTopic> unfinishedTopics = new Queue<LanguageTopic>();

        public override bool AnyUnfinishedTopic()
        {
            return unfinishedTopics.TryPeek(out _);
        }

        public override LanguageTopic GetNext()
        {
            return unfinishedTopics.Dequeue();
        }

        public override void InsertTopic(LanguageTopic topic)
        {
            unfinishedTopics.Enqueue(topic);
        }
    }
}
