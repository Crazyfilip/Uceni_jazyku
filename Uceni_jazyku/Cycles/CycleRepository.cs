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

        private static ILog log = LogManager.GetLogger(typeof(ActiveCycleCache));

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

        public UserCycle GetOldestUserInactiveCycle(string username)
        {
            log.Info($"Getting oldest inactive cycle for user {username}");
            List<UserCycle> queryResult = database
                .Where(x => x is UserCycle)
                .Cast<UserCycle>()
                .Where(x => x.Username == username && x.State == UserCycleState.Inactive)
                .ToList();
            queryResult.Sort((x, y) => x.DateCreated.CompareTo(y.DateCreated));
            return queryResult.FirstOrDefault();
        }

        public IncompleteUserCycle GetUserIncompleteCycle(string username)
        {
            log.Info($"Getting incomplete cycle for user {username}");
            return database
                .Where(x => x is IncompleteUserCycle)
                .Cast<IncompleteUserCycle>()
                .Where(x => x.Username == username)
                .FirstOrDefault();
        }

        public List<UserCycle> GetNotFinishedCycles(string username)
        {
            log.Info($"Getting cycles which are not in finished state for user {username}");
            List<UserCycle> result = database
                .Where(x => x is UserCycle)
                .Cast<UserCycle>()
                .Where(x => x.Username == username && x.State != UserCycleState.Finished)
                .ToList();
            result.Sort((x,y) => x.DateCreated.CompareTo(y.DateCreated));
            return result;
        }
    }
}
