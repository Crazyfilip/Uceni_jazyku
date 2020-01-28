using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Uceni_jazyku.Cycles
{
    /// <summary>
    /// User finished cycle
    /// </summary>
    public class UserFinishedCycle : UserCycle
    {
        /// <summary>
        /// Basic constructor. Mainly for serialization.
        /// </summary>
        public UserFinishedCycle() => path = "./cycles/user-finished/";

        /// <summary>
        /// Main constructor.
        /// </summary>
        /// <param name="name">username</param>
        public UserFinishedCycle(string name)
        {
            Username = name;
            RemainingEvents = null;
            path = "./cycles/user-finished/";
        }

        protected override void Serialize(string filepath) => base.Serialize(filepath + CycleID + ".xml");

        protected override AbstractCycle Deserialize(string filepath) => base.Deserialize(filepath + CycleID + ".xml");


        public static bool CycleExists(string cycleID)
        {
            return File.Exists("./cycles/user-finished/" + cycleID + ".xml");
        }
    }
}
