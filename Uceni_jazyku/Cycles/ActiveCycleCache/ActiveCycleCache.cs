using log4net;
using System;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;

namespace Uceni_jazyku.Cycles
{
    /// <inheritdoc/>
    public class ActiveCycleCache : IActiveCycleCache
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(ActiveCycleCache));
        private readonly string activeCycleCacheFile = "cycles/service/active-cycle.xml";

        public void DropCache()
        {
            log.Info("Clearing cache");
            File.Delete(activeCycleCacheFile);
        }

        public UserCycle GetFromCache()
        {
            if (File.Exists(activeCycleCacheFile))
            {
                log.Info("Getting active cycle from cache");
                var serializer = new DataContractSerializer(typeof(UserCycle));
                using XmlReader reader = XmlReader.Create(activeCycleCacheFile);
                return (UserCycle)serializer.ReadObject(reader);
            }
            else
            {
                log.Info("There is no active cycle in cache");
                return null;
            }
        }

        public void InsertToCache(UserCycle cycle)
        {
            if (cycle.State == UserCycles.UserCycleState.Active)
            {
                log.Info($"Inserting cycle {cycle.CycleID} to cache");
                var serializer = new DataContractSerializer(typeof(UserCycle));
                using XmlWriter writer = XmlWriter.Create(activeCycleCacheFile);
                serializer.WriteObject(writer, cycle);
            }
            else
            {
                log.Error($"Tried to insert cycle {cycle.CycleID} to cache with state {cycle.State}");
                throw new ArgumentException("Cycle is in incorrect state to be saved in cache");
            }
        }

        public bool IsCacheFilled()
        {
            log.Info("Verifying content of cache");
            return File.Exists(activeCycleCacheFile);
        }
    }
}
