using log4net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml;
using Uceni_jazyku.Cycles.UserCycles;

namespace Uceni_jazyku.Cycles
{
    /// <inheritdoc/>
    /// <summary>
    /// Repository represented as List which is saved to/loaded from a file
    /// </summary>
    public class CycleRepository : ICycleRepository
    {
        /// <summary>
        /// Path to file where is stored collection of cycles
        /// </summary>
        private readonly string path = "./cycles/service/database.xml";

        /// <summary>
        /// List of all cycles, user and language ones.
        /// </summary>
        private List<AbstractCycle> database = new List<AbstractCycle>();

        private static readonly ILog log = LogManager.GetLogger(typeof(ActiveCycleCache));

        public CycleRepository()
        {
            if (File.Exists(path))
            {
                var serializer = new DataContractSerializer(typeof(List<AbstractCycle>));
                using XmlReader reader = XmlReader.Create(path);
                database = (List<AbstractCycle>)serializer.ReadObject(reader);
            }
        }

        public CycleRepository(List<AbstractCycle> cycles)
        {
            database = cycles;
            Save();
        }

        /// <summary>
        /// Save actual state database
        /// </summary>
        private void Save()
        {
            log.Debug("Saving repository to file");
            var serializer = new DataContractSerializer(typeof(List<AbstractCycle>));
            using XmlWriter writer = XmlWriter.Create(path);
            serializer.WriteObject(writer, database ?? new List<AbstractCycle>());
        }

        public void PutCycle(AbstractCycle cycle)
        {
            log.Info($"Adding cycle {cycle.CycleID} to repository");
            database.Add(cycle);
            Save();
        }

        // TODO this method is used for generating cycleID so once generating will change remove this
        [Obsolete]
        public int GetCyclesCount()
        {
            return database.Count;
        }

        public void UpdateCycle(AbstractCycle updatedCycle)
        {
            log.Info($"Updating cycle {updatedCycle.CycleID}");
            int index = database.FindIndex(x => x.CycleID == updatedCycle.CycleID);
            if (index != -1)
            {
                database[index] = updatedCycle;
                log.Debug($"Cycle {updatedCycle.CycleID} updated");
            }
            else
            {
                database.Add(updatedCycle);
                log.Debug($"Cycle {updatedCycle.CycleID} added");
            }
            Save();
        }

        /// <summary>
        /// getter of cycle number from its id
        /// </summary>
        /// <param name="cycle">cycle</param>
        /// <returns>cycle number</returns>
        // TODO delete this method, cycle should rather to have field like DateCreated based on which will be chosen oldest one
        [Obsolete]
        private int getCycleNumber(AbstractCycle cycle)
        {
            return int.Parse(cycle.CycleID.Substring(5)); // cycleID format is: cycle<number>
        }

        public UserCycle GetOldestUserInactiveCycle(string username)
        {
            log.Info($"Getting oldest inactive cycle for user {username}");
            var queryResult = database
                .Where(x => x is UserCycle)
                .Where(x =>
                {
                    UserCycle cycle = (UserCycle)x;
                    return cycle.Username == username && cycle.State == UserCycleState.Inactive;
                })
                .ToList();
            queryResult.Sort((x, y) => getCycleNumber(x).CompareTo(getCycleNumber(y)));
            return (queryResult.Count > 0) ? ((UserCycle) queryResult.First()) : null;
        }

        public IncompleteUserCycle GetUserIncompleteCycle(string username)
        {
            log.Info($"Getting incomplete cycle for user {username}");
            return (IncompleteUserCycle)database
                .Where(x => x is IncompleteUserCycle)
                .Where(x =>
                {
                    IncompleteUserCycle cycle = (IncompleteUserCycle)x;
                    return cycle.Username == username;
                }).FirstOrDefault();
        }
    }
}
