using System;
using System.Collections.Generic;
using System.Text;

namespace Uceni_jazyku.Cycles
{

    public class UserCycle : AbstractCycle
    {
        public UserCycle() { }
        public UserCycle(string username, int remainingEvents)
        {
            this.Username = username;
            this.RemainingEvents = remainingEvents;
        }

        public override void Update()
        {
            throw new NotImplementedException();
        }
    }
}
