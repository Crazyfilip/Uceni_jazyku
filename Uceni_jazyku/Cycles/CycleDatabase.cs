using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace Uceni_jazyku.Cycles
{
    /// <summary>
    /// Database of cycles
    /// 
    /// Via xml-serialization saved to/loaded from file
    /// </summary>
    public class CycleDatabase
    {
        /// <summary>
        /// Path to file where is stored collection of cycles
        /// </summary>
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
            XmlSerializer serializer = new XmlSerializer(typeof(List<AbstractCycle>));
            using StreamWriter sw = new StreamWriter(path);
            serializer.Serialize(sw, database ?? new List<AbstractCycle>());
        }

        /// <summary>
        /// Load database. If database file doesn't exists then database will be empty list.
        /// </summary>
        public void Load()
        {
            if (File.Exists(path)) {
                XmlSerializer serializer = new XmlSerializer(typeof(List<AbstractCycle>));
                using StreamReader sr = new StreamReader(path);
                database = (List<AbstractCycle>)serializer.Deserialize(sr);
                return;
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
        public void PutCycle(AbstractCycle cycle)
        {
            (database ??= new List<AbstractCycle>()).Add(cycle);
            Save();
        }

        /// <summary>
        /// getter for number of existing cycles
        /// </summary>
        /// <returns>number of cycles in database</returns>
        public int GetCyclesCount()
        {
            return database?.Count ?? 0;
        }

        /// <summary>
        /// Update information about a cycle stored in the database.
        /// </summary>
        /// <param name="cycleID"></param>
        /// <param name="updatedCycle"></param>
        public void UpdateCycle(AbstractCycle updatedCycle)
        {
            int index = database?.FindIndex(x => x.CycleID == updatedCycle.CycleID) ?? throw new Exception("invalid state of database");
            database[index] = updatedCycle;
            Save();
        }

        /// <summary>
        /// Test if given cycle is present in database
        /// </summary>
        /// <param name="cycle">tested cycle</param>
        /// <returns>true if cycle is present otherwise false</returns>
        public bool IsInDatabase(AbstractCycle cycle)
        {
            return database.Contains(cycle);
        }

        /// <summary>
        /// getter of cycle number from its id
        /// </summary>
        /// <param name="cycle">cycle</param>
        /// <returns>cycle number</returns>
        public int getCycleNumber(AbstractCycle cycle)
        {
            return int.Parse(cycle.CycleID.Substring(5)); // cycleID format is: cycle<number>
        }

        /// <summary>
        /// Search for user's oldest inactive cycle
        /// </summary>
        /// <param name="username">user's name</param>
        /// <returns>Oldest user's inactive cycle</returns>
        public UserInactiveCycle GetOldestUserInactiveCycle(string username)
        {
            var queryResult = database
                ?.Where(x => x.Username == username)
                .Where(x => x is UserInactiveCycle)
                .ToList()
                ?? throw new Exception("invalid state of database");
            queryResult.Sort((x, y) => getCycleNumber(x).CompareTo(getCycleNumber(y)));
            return queryResult.Count > 0 ? (UserInactiveCycle) queryResult.First() : null;
        }
    }
}
