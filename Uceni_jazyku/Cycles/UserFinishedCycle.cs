using System;
using System.Collections.Generic;
using System.Text;

namespace Uceni_jazyku.Cycles
{
    public class UserFinishedCycle : UserCycle
    {
        public UserFinishedCycle() => path = "./cycles/finished/user/";

        public UserFinishedCycle(string name, int numberOfEvents)
        {
            Username = name;
            RemainingEvents = numberOfEvents;
            path = "./cycles/finished/user/";
        }
    }
}
