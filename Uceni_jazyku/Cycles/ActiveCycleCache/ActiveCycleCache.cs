using System;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;

namespace Uceni_jazyku.Cycles
{
    public class ActiveCycleCache : IActiveCycleCache
    {
        private readonly string activeCycleCacheFile = "cycles/service/active-cycle.xml";

        public void DropCache()
        {
            File.Delete(activeCycleCacheFile);
        }

        public UserCycle GetFromCache()
        {
            if (File.Exists(activeCycleCacheFile))
            {
                var serializer = new DataContractSerializer(typeof(UserCycle));
                using XmlReader reader = XmlReader.Create(activeCycleCacheFile);
                return (UserCycle)serializer.ReadObject(reader);
            }
            else
                return null;
        }

        public void InsertToCache(UserCycle cycle)
        {
            if (cycle.State == UserCycles.UserCycleState.Active)
            {
                var serializer = new DataContractSerializer(typeof(UserCycle));
                using XmlWriter writer = XmlWriter.Create(activeCycleCacheFile);
                serializer.WriteObject(writer, cycle);
            }
            else
            {
                throw new ArgumentException("Cycle is in incorrect state to be saved in cache");
            }
        }

        public bool IsCacheFilled()
        {
            return File.Exists(activeCycleCacheFile);
        }
    }
}
