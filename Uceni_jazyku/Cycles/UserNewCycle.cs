using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Uceni_jazyku.Cycles
{
    /// <summary>
    /// User new cycle
    /// </summary>
    public class UserNewCycle : UserCycle
    {
        /// <summary>
        /// Basic constructor. Mainly for serialization.
        /// </summary>
        public UserNewCycle() => path = "./cycles/user-new/";

        /// <summary>
        /// Main constructor.
        /// </summary>
        /// <param name="name">username</param>
        public UserNewCycle(string name)
        {
            Username = name;
            RemainingEvents = null;
            path = "./cycles/user-new/";
        }

        public static bool CycleExists(string cycleId)
        {
            return File.Exists("./cycles/user-new/" + cycleId + ".xml");
        }

        protected override void Serialize(string filepath) => base.Serialize(filepath + CycleID + ".xml");

        protected override AbstractCycle Deserialize(string filepath) => base.Deserialize(filepath + CycleID + ".xml");

        protected override void DeleteCycleFile(string filepath)
        {
            base.DeleteCycleFile(filepath + CycleID + ".xml");
        }
    }
}
