using System;
using System.Collections.Generic;
using System.Text;

namespace Uceni_jazyku.Cycles
{
    public class UserFinishedCycle : UserCycle
    {
        public UserFinishedCycle() => path = "./cycles/finished/user/";

        public UserFinishedCycle(string name)
        {
            Username = name;
            RemainingEvents = null;
            path = "./cycles/finished/user/";
        }
    }
}
