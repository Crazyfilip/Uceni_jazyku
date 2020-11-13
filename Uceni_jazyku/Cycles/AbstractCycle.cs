using System.Runtime.Serialization;
using System.Xml;
using Uceni_jazyku.Cycles.Program;

namespace Uceni_jazyku.Cycles
{
    /// <summary>
    /// Abstract class for all cycles
    /// Cycle is internal object which drives learning process of user
    /// There are two types of cycles: user and language
    /// </summary>
    [KnownType(typeof(UserCycle))]
    [DataContract]
    public abstract class AbstractCycle
    {
        [DataMember]
        public int FinishedEvents { get; protected set; }
        [DataMember]
        public string CycleID { get; set; }

        /// <summary>
        /// path to cycle's file
        /// </summary>
        protected string path;

        /// <summary>
        /// Update cycle when user did progress in learning
        /// </summary>
        public abstract void Update(); // TODO add appropiate argument describing what is updated 

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public abstract ProgramItem GetNext();
    }
}
