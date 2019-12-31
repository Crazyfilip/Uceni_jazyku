using System;
using System.Collections.Generic;
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
        public UserNewCycle() { }

        /// <summary>
        /// Main constructor.
        /// </summary>
        /// <param name="name">username</param>
        public UserNewCycle(string name)
        {
            Username = name;
            RemainingEvents = null;
        }
    }
}
