using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Uceni_jazyku.Cycles
{
    /// <summary>
    /// User new cycle.
    /// First step in UserCycle lifecycle
    /// Reflects cycle's initialization and assigning of program
    /// </summary>
    public class UserNewCycle : UserCycle
    {
        /// <summary>
        /// place where new cycles will be saved
        /// </summary>
        private static readonly string dirPath = "./cycles/user-new/";

        /// <summary>
        /// Basic constructor. Mainly for serialization.
        /// </summary>
        public UserNewCycle() => path = dirPath;

        /// <summary>
        /// Main constructor.
        /// </summary>
        /// <param name="name">username</param>
        public UserNewCycle(string name)
        {
            Username = name;
            RemainingEvents = null;
            path = dirPath;
        }

        /// <summary>
        /// Test if file for new cycle exists
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
