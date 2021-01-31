using System;
using System.Collections.Generic;
using System.Text;
using Uceni_jazyku.Cycles.UserCycles;

namespace Uceni_jazyku.Cycles
{
    public class CycleFactory : ICycleFactory
    {
        public UserCycle createCycle()
        {
            return new UserCycle() { CycleID = getGUID() };
        }

        public IncompleteUserCycle createIncompleteCycle(string username)
        {
            return new IncompleteUserCycle(username) { CycleID = getGUID() };
        }

        private string getGUID()
        {
            return System.Guid.NewGuid().ToString();
        }
    }
}
