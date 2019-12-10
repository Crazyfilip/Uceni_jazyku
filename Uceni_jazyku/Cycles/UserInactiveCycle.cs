using System;
using System.Collections.Generic;
using System.Text;

namespace Uceni_jazyku.Cycles
{
    class UserInactiveCycle : UserCycle
    {
        public UserInactiveCycle() => path = "./cycles/user-inactive/";

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
