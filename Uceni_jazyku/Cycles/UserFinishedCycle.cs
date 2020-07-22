using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Uceni_jazyku.Cycles
{
    /// <summary>
    /// User finished cycle
    /// Last step of UserCycle lifecycle
    /// Represents cycle for which user finished its program
    /// Has role in adapting mechanism
    /// </summary>
    public class UserFinishedCycle : UserCycle
    {
        /// <summary>
        /// place where inactive cycles will be saved
        /// </summary>
        private static readonly string dirPath = "./cycles/user-finished/";

        /// <summary>
        /// Basic constructor. Mainly for serialization.
        /// </summary>
        public UserFinishedCycle() => path = dirPath;

        /// <summary>
        /// Main constructor.
        /// </summary>
        /// <param name="name">username</param>
        public UserFinishedCycle(string name)
        {
            Username = name;
            RemainingEvents = null;
            path = dirPath;
        }

        protected override void Serialize(string filepath) => base.Serialize(filepath + CycleID + ".xml");

        protected override AbstractCycle Deserialize(string filepath) => base.Deserialize(filepath + CycleID + ".xml");

        /// <summary>
        /// Test if file for finished cycle exists
        /// </summary>
        /// <param name="cycleId">cycle's id</param>
        /// <returns>true if exists</returns>
        public static bool CycleExists(string cycleID)
        {
            return File.Exists(dirPath + cycleID + ".xml");
        }
    }
}
