using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml.Serialization;

namespace Uceni_jazyku.User_sessions
{

    [XmlInclude(typeof(ActiveUserSession))]
    public abstract class AbstractSession
    {

        public string Username { get; set; }
        public int RemainingEvents { get; set; }
        public string SessionId { get; set; }

        protected string path;
        protected EventArgs e = null;

        public abstract void Update();

        protected virtual void Serialize(string filepath)
        {
            XmlSerializer serializer = new XmlSerializer(this.GetType());
            using StreamWriter sw = new StreamWriter(filepath);
            serializer.Serialize(sw, this);
        }

        protected virtual AbstractSession Deserialize(string filepath)
        {
            XmlSerializer serializer = new XmlSerializer(this.GetType());
            using StreamReader sr = new StreamReader(filepath);
            return (AbstractSession)serializer.Deserialize(sr);
        }

        public void SaveSession()
        {
            Serialize(path);
        }

        public AbstractSession GetSession()
        {
            return Deserialize(path);
        }
    }
}
