using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace Uceni_jazyku.Cycles
{
    /// <summary>
    /// Database of cycles
    /// In application as collection</c>
    /// Via xml-serialization saved to/loaded from file
    /// </summary>
    public class CycleDatabase
    {
        private readonly string path = "./cycles/service/database.xml";

        /// <summary>
        /// List of all cycles, user and language ones.
        /// </summary>
        private SortedList<String,AbstractCycle> database;

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
                database = new SortedList<String,AbstractCycle>();
            }
        }

        /// <summary>
        /// Insert cycle to database
        /// </summary>
        /// <param name="cycle">inserted cycle</param>
        public void PutCycle(AbstractCycle cycle)
        {
            database.Add(cycle.CycleID,cycle);
            Save();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int GetCyclesCount()
        {
            return database.Count;
        }

        /// <summary>
        /// Update cycle to its current state 
        /// </summary>
        /// <param name="cycleID"></param>
        /// <param name="updatedCycle"></param>
        public void UpdateCycle(string cycleID, AbstractCycle updatedCycle)
        {
            database[cycleID] = updatedCycle;
        }

        public bool IsInDatabase(AbstractCycle cycle)
        {
            if (!database.ContainsKey(cycle.CycleID))
                return false;
            return database.ContainsValue(cycle);
        }
    }
}
