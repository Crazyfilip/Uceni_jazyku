using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace Uceni_jazyku.Cycles
{
    /// <summary>
    /// Database of cycles
    /// In application as <c>List&lt;AbstractCycle&gt;</c>
    /// Via xml-serialization saved to/loaded from file
    /// </summary>
    public class CycleDatabase
    {
        private readonly string path = "./sessions/service/database.xml";

        /// <summary>
        /// List of all cycles, user and language ones.
        /// </summary>
        public List<AbstractCycle> database;

        /// <summary>
        /// Save actual state database
        /// </summary>
        public void Save()
        {
            XmlSerializer serializer = new XmlSerializer(this.GetType());
            using StreamWriter sw = new StreamWriter(path);
            serializer.Serialize(sw, this);
        }

        /// <summary>
        /// Load database. If database file doesn't exists then database will be empty list.
        /// </summary>
        public void Load()
        {
            XmlSerializer serializer = new XmlSerializer(this.GetType());
            if (File.Exists(path))
            {
                using StreamReader sr = new StreamReader(path);
                this.database = ((CycleDatabase)serializer.Deserialize(sr)).database;
            } 
            else
            {
                database = new List<AbstractCycle>();
            }
        }

        /// <summary>
        /// Insert cycle to database
        /// </summary>
        /// <param name="cycle">inserted cycle</param>
        public void PutSession(AbstractCycle cycle)
        {
            database.Add(cycle);
            Save();
            //sessionsDatabase.Sort() sort by id
        }

        /// <summary>
        /// Update cycle to its current state 
        /// </summary>
        /// <param name="sessionId"></param>
        /// <param name="updatedSession"></param>
        public void UpdateSession(string sessionId, AbstractCycle updatedSession)
        {
            //sessionsDatabase.Remove()
        }
    }
}
