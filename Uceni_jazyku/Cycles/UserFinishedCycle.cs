using System;
using System.Collections.Generic;
using System.Text;

namespace Uceni_jazyku.Cycles
{
    public class UserFinishedCycle : UserCycle
    {
        public UserFinishedCycle() => path = "./sessions/finished/user/";

        public UserFinishedCycle(string name, int numberOfEvents)
        {
            Username = name;
            RemainingEvents = numberOfEvents;
            path = "./sessions/user-inactive/session.txt";
        }
    }
}
