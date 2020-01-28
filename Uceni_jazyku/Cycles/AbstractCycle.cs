using System.IO;
using System.Xml.Serialization;

namespace Uceni_jazyku.Cycles
{
    /// <summary>
    /// Abstract class for all language and user cycles
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
