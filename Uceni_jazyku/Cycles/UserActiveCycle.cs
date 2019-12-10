using System;
using System.Collections.Generic;
using System.Text;

namespace Uceni_jazyku.Cycles
{
    public class UserActiveCycle : UserCycle
    {

        public UserActiveCycle() => path = "./sessions/user-active/session.txt";

        public UserActiveCycle(string name, int numberOfEvents)
        {
            Username = name;
            RemainingEvents = numberOfEvents;
            path = "./sessions/user-active/session.txt";
        }
    }
}
