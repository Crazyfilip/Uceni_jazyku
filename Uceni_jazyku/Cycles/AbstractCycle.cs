using System.IO;
using System.Xml.Serialization;

namespace Uceni_jazyku.Cycles
{

    [XmlInclude(typeof(UserActiveCycle))]
    public abstract class AbstractCycle
    {

        public string Username { get; set; }
        public int? RemainingEvents { get; set; }
        public string CycleID { get; set; }

        protected string path;

        public abstract void Update();

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

        public void SaveCycle()
        {
            Serialize(path);
        }

        public AbstractCycle GetCycle()
        {
            return Deserialize(path);
        }
    }
}
