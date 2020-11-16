using System.Runtime.Serialization;
using System.Xml;
using Uceni_jazyku.Cycles.LanguageCycles;
using Uceni_jazyku.Cycles.Program;
using Uceni_jazyku.Cycles.UserCycles;

namespace Uceni_jazyku.Cycles
{
    /// <summary>
    /// Abstract class for all cycles
    /// Cycle is internal object which drives learning process of user
    /// There are two types of cycles: user and language
    /// </summary>
    [KnownType(typeof(UserCycle))]
    [KnownType(typeof(LanguageCycle))]
    [KnownType(typeof(IncompleteUserCycle))]
    [DataContract]
    public abstract class AbstractCycle
    {
        [DataMember]
        public int FinishedEvents { get; protected set; }
        [DataMember]
        public string CycleID { get; set; }

        /// <summary>
        /// Update cycle when user did progress in learning => set first unfinished lesson as finished
        /// </summary>
        public abstract void Update(); // TODO different name?

        /// <summary>
        /// Get first unfinished lesson
        /// </summary>
        /// <returns>Lesson</returns>
        public abstract ProgramItem GetNext();
    }
}
