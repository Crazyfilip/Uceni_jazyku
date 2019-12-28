using System;
using System.Collections.Generic;
using System.Text;

namespace Uceni_jazyku.Cycles
{
    public class UserNewCycle : UserCycle
    {
        public UserNewCycle() { }

        public UserNewCycle(string name)
        {
            Username = name;
            RemainingEvents = null;
        }
    }
}
