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
    [KnownType(typeof(PlannerMemory))]
    public abstract class AbstractPlannerMemory
    {
        [DataMember]
        public string Username { get; init; }

        [DataMember]
        public string CourseId { get; init; }

        [DataMember]
        public string MemoryId { get; init; }

        public abstract LanguageTopic GetNext();

        public abstract void InsertTopic(LanguageTopic topic);

        public abstract bool AnyUnfinishedTopic();
    }
}
