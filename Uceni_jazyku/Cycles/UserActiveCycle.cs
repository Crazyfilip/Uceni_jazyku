using System;
using System.Collections.Generic;
using System.Text;

namespace Uceni_jazyku.Cycles
{
    public class UserActiveCycle : UserCycle
    {

        public UserActiveCycle() => path = "./cycles/user-active/activeCycle.txt";

        public UserActiveCycle(string name, int numberOfEvents)
        {
            Username = name;
            RemainingEvents = numberOfEvents;
            path = "./cycles/user-active/activeCycle.txt";
        }
    }
}
