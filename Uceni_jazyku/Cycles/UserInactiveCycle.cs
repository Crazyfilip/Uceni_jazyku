using System;
using System.Collections.Generic;
using System.Text;

namespace Uceni_jazyku.Cycles
{
    /// <summary>
    /// User inactive cycle
    /// </summary>
    public class UserInactiveCycle : UserCycle
    {
        /// <summary>
        /// Basic constructor. Mainly for serialization.
        /// </summary>
        public UserInactiveCycle() => path = "./cycles/user-inactive/";

        /// <summary>
        /// Main constructor.
        /// </summary>
        /// <param name="name">username</param>
        /// <param name="numberOfEvents">number of learning events to complete this cycle</param>
        public UserInactiveCycle(string name, int numberOfEvents)
        {
            Username = name;
            RemainingEvents = numberOfEvents;
            path = "./cycles/user-inactive/";
        }

        protected override void Serialize(string filepath) => base.Serialize(filepath + CycleID + ".xml");

        protected override AbstractCycle Deserialize(string filepath) => base.Deserialize(filepath + CycleID + ".xml");
    }
}
