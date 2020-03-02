using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
        private List<AbstractCycle> database;

        /// <summary>
        /// Save actual state database
        /// </summary>
        public void Save()
        {
            if (database == null)
                database = new List<AbstractCycle>();
            XmlSerializer serializer = new XmlSerializer(database.GetType());
            using StreamWriter sw = new StreamWriter(path);
            serializer.Serialize(sw, database);
        }

        /// <summary>
        /// Load database. If database file doesn't exists then database will be empty list.
        /// </summary>
        public void Load()
        {
            database = new List<AbstractCycle>();
            XmlSerializer serializer = new XmlSerializer(database.GetType());
            if (File.Exists(path))
            {
                using StreamReader sr = new StreamReader(path);
                database = (List<AbstractCycle>)serializer.Deserialize(sr);
            }
        }

        /// <summary>
        /// Insert cycle to database
        /// </summary>
        /// <param name="cycle">inserted cycle</param>
        public void PutCycle(AbstractCycle cycle)
        {
            database.Add(cycle);
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
        public void UpdateCycle(AbstractCycle updatedCycle)
        {
            int index = database.FindIndex(x => x.CycleID == updatedCycle.CycleID);
            database[index] = updatedCycle;
            Save();
        }

        public bool IsInDatabase(AbstractCycle cycle)
        {
            return database.Contains(cycle);
        }

        public int getCycleNumber(AbstractCycle cycle)
        {
            return int.Parse(cycle.CycleID.Substring(5));
        }

        public UserInactiveCycle GetOldestUserInactiveCycle(string username)
        {
            var queryResult = database
                .Where(x => x.Username == username)
                .Where(x => x is UserInactiveCycle)
                .ToList();
            queryResult.Sort((x, y) => getCycleNumber(x).CompareTo(getCycleNumber(y)));
            return queryResult.Count > 0 ? (UserInactiveCycle) queryResult.First() : null;
        }
    }
}
