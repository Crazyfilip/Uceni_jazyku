using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace Uceni_jazyku.User_sessions
{
    public class SessionDatabase
    {
        private readonly string path = "./sessions/service/database.xml";

        public List<AbstractSession> sessionsDatabase;

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
                this.sessionsDatabase = ((SessionDatabase)serializer.Deserialize(sr)).sessionsDatabase;
            } 
            else
            {
                sessionsDatabase = new List<AbstractSession>();
            }
        }

        public void PutSession(AbstractSession session)
        {
            sessionsDatabase.Add(session);
            Save();
            //sessionsDatabase.Sort() sort by id
        }

        public void UpdateSession(string sessionId, AbstractSession updatedSession)
        {
            //sessionsDatabase.Remove()
        }
    }
}
