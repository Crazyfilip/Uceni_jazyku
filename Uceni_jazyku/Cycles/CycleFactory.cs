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
        public UserCycle CreateCycle(string username)
        {
            return new UserCycle() 
            { 
                CycleID = GetGUID(), 
                DateCreated = DateTime.Now, 
                Username = username 
            };
        }

        public IncompleteUserCycle CreateIncompleteCycle(string username, int limit)
        {
            return new IncompleteUserCycle(limit)
            { 
                CycleID = GetGUID(), 
                DateCreated = DateTime.Now, 
                Username = username
            };
        }

        private string GetGUID()
        {
            return System.Guid.NewGuid().ToString();
        }
    }
}
