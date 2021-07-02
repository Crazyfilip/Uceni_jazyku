using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Uceni_jazyku.Common;
using Uceni_jazyku.Language;

namespace Uceni_jazyku.Planner
{
    /// <summary>
    /// Abstract representation of planner memory.
    /// 
    /// Memory serve as storage for topics which are not fully planned.
    /// Specific representation of this storage is not forced.
    /// </summary>
    [DataContract]
    [KnownType(typeof(PlannerMemory))]
    public abstract class AbstractPlannerMemory : IId
    {
        [DataMember]
        public virtual string CourseId { get; init; }

        [DataMember]
        public string Id { get; init; }

        /// <summary>
        /// Get topic from memory from which will be picked next lesson
        /// </summary>
        /// <returns>LanguageTopic</returns>
        public abstract LanguageTopic GetNextTopic();

        /// <summary>
        /// Put topic to memory
        /// </summary>
        /// <param name="topic">LanguageTopic</param>
        public abstract void InsertTopic(LanguageTopic topic);

        /// <summary>
        /// Test if in memory is present any topic
        /// </summary>
        /// <returns>True if some topic is in memory, false when empty</returns>
        public abstract bool AnyUnfinishedTopic();
    }
}
