using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Uceni_jazyku.Cycles
{
    /// <summary>
    /// User active cycle
    /// Second step in UserCycle lifecycle
    /// At most one active cycle for user can be present
    /// Represents cycle based on which user is currently doing lessons
    /// </summary>
    public class UserActiveCycle : UserCycle
    {
        /// <summary>
        /// place where active cycle will be saved
        /// </summary>
        private static readonly string filepath = "./cycles/user-active/activeCycle.txt";

        /// <summary>
        /// Basic constructor. Mainly for serialization.
        /// </summary>
        public UserActiveCycle() => path = filepath;

        /// <summary>
        /// Main constructor.
        /// </summary>
        /// <param name="name">username</param>
        /// <param name="numberOfEvents">number of learning events to complete this cycle</param>
        public UserActiveCycle(string name, int numberOfEvents)
        {
            Username = name;
            RemainingEvents = numberOfEvents;
            path = filepath;
        }

        /// <summary>
        /// Test if file for active cycle exists
        /// </summary>
        /// <returns>true if file with active cycle exists</returns>
        public static bool CycleExists()
        {
            return File.Exists(filepath);
        }
    }
}
