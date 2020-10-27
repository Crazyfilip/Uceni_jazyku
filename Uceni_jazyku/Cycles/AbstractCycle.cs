using System.IO;
using System.Xml.Serialization;
using Uceni_jazyku.Cycles.Program;

namespace Uceni_jazyku.Cycles
{
    /// <summary>
    /// Abstract class for all cycles
    /// Cycle is internal object which drives learning process of user
    /// There are two types of cycles: user and language
    /// </summary>
    [XmlInclude(typeof(UserActiveCycle))]
    [XmlInclude(typeof(UserFinishedCycle))]
    [XmlInclude(typeof(UserInactiveCycle))]
    [XmlInclude(typeof(UserNewCycle))]
    public abstract class AbstractCycle
    {

        public string Username { get; set; }
        public int? RemainingEvents { get; set; }
        public string CycleID { get; set; }

        public CycleProgram Program { get; set; }

        /// <summary>
        /// path to cycle's file
        /// </summary>
        protected string path;

        /// <summary>
        /// Update cycle when user did progress in learning
        /// </summary>
        public abstract void Update(); // TODO add appropiate argument describing what is updated 

        protected virtual void Serialize(string filepath)
        {
            XmlSerializer serializer = new XmlSerializer(this.GetType());
            using StreamWriter sw = new StreamWriter(filepath);
            serializer.Serialize(sw, this);
        }

        protected virtual AbstractCycle Deserialize(string filepath)
        {
            XmlSerializer serializer = new XmlSerializer(this.GetType());
            using StreamReader sr = new StreamReader(filepath);
            return (AbstractCycle)serializer.Deserialize(sr);
        }

        /// <summary>
        /// Save cycle to file
        /// </summary>
        public void SaveCycle()
        {
            Serialize(path);
        }

        /// <summary>
        /// Get cycle from file
        /// </summary>
        /// <returns></returns>
        public AbstractCycle GetCycle()
        {
            return Deserialize(path);
        }
    }
}
