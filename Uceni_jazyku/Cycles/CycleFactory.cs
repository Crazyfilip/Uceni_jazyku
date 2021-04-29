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
        public UserCycle CreateCycle(string username, string courseId)
        {
            return new UserCycle() 
            { 
                CycleID = GetGUID(), 
                DateCreated = DateTime.Now, 
                Username = username,
                CourseID = courseId
            };
        }

        public IncompleteUserCycle CreateIncompleteCycle(string username, string courseId, int limit)
        {
            return new IncompleteUserCycle(limit)
            { 
                CycleID = GetGUID(), 
                DateCreated = DateTime.Now, 
                Username = username,
                CourseID = courseId
            };
        }

        private string GetGUID()
        {
            return System.Guid.NewGuid().ToString();
        }
    }
}
