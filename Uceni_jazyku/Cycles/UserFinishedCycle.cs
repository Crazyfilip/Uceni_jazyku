using System;
using System.Collections.Generic;
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
        public UserFinishedCycle() => path = "./cycles/finished/user/";

        /// <summary>
        /// Main constructor.
        /// </summary>
        /// <param name="name">username</param>
        public UserFinishedCycle(string name)
        {
            Username = name;
            RemainingEvents = null;
            path = "./cycles/finished/user/";
        }
    }
}
