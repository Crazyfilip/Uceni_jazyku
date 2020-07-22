using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Uceni_jazyku.Cycles
{
    /// <summary>
    /// User inactive cycle
    /// Represents cycle based on which user is currently not do lessons
    /// and which has not finished its program
    /// </summary>
    public class UserInactiveCycle : UserCycle
    {
        /// <summary>
        /// place where inactive cycles will be saved
        /// </summary>
        private static readonly string dirPath = "./cycles/user-inactive/";

        /// <summary>
        /// Basic constructor. Mainly for serialization.
        /// </summary>
        public UserInactiveCycle() => path = dirPath;

        /// <summary>
        /// Main constructor.
        /// </summary>
        /// <param name="name">username</param>
        /// <param name="numberOfEvents">number of learning events to complete this cycle</param>
        public UserInactiveCycle(string name, int numberOfEvents)
        {
            Username = name;
            RemainingEvents = numberOfEvents;
            path = dirPath;
        }

        /// <summary>
        /// Test if file for inactive cycle exists
        /// </summary>
        /// <param name="cycleId">cycle's id</param>
        /// <returns>true if exists</returns>
        public static bool CycleExists(string cycleId)
        {
            return File.Exists(dirPath + cycleId + ".xml");
        }

        protected override void Serialize(string filepath) => base.Serialize(filepath + CycleID + ".xml");

        protected override AbstractCycle Deserialize(string filepath) => base.Deserialize(filepath + CycleID + ".xml");

        protected override void DeleteCycleFile(string filepath)
        {
            base.DeleteCycleFile(filepath + CycleID + ".xml");
        }
    }
}
