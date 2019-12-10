using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace Uceni_jazyku.Cycles
{
    public class CycleDatabase
    {
        private readonly string path = "./sessions/service/database.xml";

        public List<AbstractCycle> sessionsDatabase;

        public void Save()
        {
            XmlSerializer serializer = new XmlSerializer(this.GetType());
            using StreamWriter sw = new StreamWriter(path);
            serializer.Serialize(sw, this);
        }

        public void Load()
        {
            XmlSerializer serializer = new XmlSerializer(this.GetType());
            if (File.Exists(path))
            {
                using StreamReader sr = new StreamReader(path);
                this.sessionsDatabase = ((CycleDatabase)serializer.Deserialize(sr)).sessionsDatabase;
            } 
            else
            {
                sessionsDatabase = new List<AbstractCycle>();
            }
        }

        public void PutSession(AbstractCycle session)
        {
            sessionsDatabase.Add(session);
            Save();
            //sessionsDatabase.Sort() sort by id
        }

        public void UpdateSession(string sessionId, AbstractCycle updatedSession)
        {
            //sessionsDatabase.Remove()
        }
    }
}
