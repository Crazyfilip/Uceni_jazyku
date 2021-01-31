using System;
using System.Collections.Generic;
using System.Text;
using Uceni_jazyku.Cycles.UserCycles;

namespace Uceni_jazyku.Cycles
{
    /// <inheritdoc/>
    /// CycleID is generated using System.Guid
    public class CycleFactory : ICycleFactory
    {
        public UserCycle CreateCycle()
        {
            return new UserCycle() { CycleID = getGUID() };
        }

        public IncompleteUserCycle CreateIncompleteCycle(string username)
        {
            return new IncompleteUserCycle(username) { CycleID = getGUID() };
        }

        private string getGUID()
        {
            return System.Guid.NewGuid().ToString();
        }
    }
}
