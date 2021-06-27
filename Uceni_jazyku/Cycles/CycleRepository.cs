using log4net;
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
        private List<UserCycle> database = new List<UserCycle>();

        private static ILog log = LogManager.GetLogger(typeof(CycleRepository));

        public CycleRepository()
        {
            if (File.Exists(path))
            {
                var serializer = new DataContractSerializer(typeof(List<UserCycle>));
                using XmlReader reader = XmlReader.Create(path);
                database = (List<UserCycle>)serializer.ReadObject(reader);
            }
        }

        public CycleRepository(List<UserCycle> cycles)
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
            var serializer = new DataContractSerializer(typeof(List<UserCycle>));
            using XmlWriter writer = XmlWriter.Create(path);
            serializer.WriteObject(writer, database ?? new List<UserCycle>());
        }

        #region CRUD operations
        /// <inheritdoc/>
        public void Create(UserCycle cycle)
        {
            log.Info($"Adding cycle with ID {cycle.CycleID}");
            database.Add(cycle);
            Save();
        }

        /// <inheritdoc/>
        public UserCycle Get(string key)
        {
            log.Info($"Getting cycle with id {key}");
            return database
                .Where(x => x.CycleID == key)
                .FirstOrDefault();
        }

        /// <inheritdoc/>
        public void Delete(UserCycle value)
        {
            log.Info($"Removing cycle with id {value.CycleID}");
            database.Remove(value);
            Save();
        }

        /// <inheritdoc/>
        public void Update(UserCycle value)
        {
            log.Info($"Updating cycle {value.CycleID}");
            int index = database.FindIndex(x => x.CycleID == value.CycleID);
            if (index != -1)
            {
                database[index] = value;
                log.Debug($"Cycle {value.CycleID} updated");
            }
            else
            {
                database.Add(value);
                log.Debug($"Cycle {value.CycleID} added");
            }
            Save();
        }
        #endregion

        /// <inheritdoc/>
        public UserCycle GetActiveCycle(string username, string courseID)
        {
            log.Info($"Getting active cycle for user {username}");
            return database
                .Where(x => x.State == UserCycleState.Active && x.Username == username && x.CourseID == courseID)
                .FirstOrDefault();
        }

        /// <inheritdoc/>
        public UserCycle GetOldestUserInactiveCycle(string username, string courseId)
        {
            log.Info($"Getting oldest inactive cycle for user {username}");
            List<UserCycle> queryResult = database
                .Where(x => x.Username == username && x.CourseID == courseId && x.State == UserCycleState.Inactive)
                .ToList();
            queryResult.Sort((x, y) => x.DateCreated.CompareTo(y.DateCreated));
            return queryResult.FirstOrDefault();
        }

        /// <inheritdoc/>
        public IncompleteUserCycle GetUserIncompleteCycle(string username, string courseId)
        {
            log.Info($"Getting incomplete cycle for user {username}");
            return database
                .Where(x => x.Username == username && x.CourseID == courseId && x is IncompleteUserCycle)
                .Cast<IncompleteUserCycle>()
                .FirstOrDefault();
        }

        /// <inheritdoc/>
        public List<UserCycle> GetNotFinishedCycles(string username, string courseId)
        {
            log.Info($"Getting cycles which are not in finished state for user {username}");
            List<UserCycle> result = database
                .Where(x => x.Username == username && x.CourseID == courseId && x.State != UserCycleState.Finished)
                .ToList();
            result.Sort((x,y) => x.DateCreated.CompareTo(y.DateCreated));
            return result;
        }
    }
}
