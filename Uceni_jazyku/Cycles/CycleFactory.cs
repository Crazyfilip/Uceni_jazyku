using System;
using System.Collections.Generic;
using LanguageLearning.Cycles.Program;
using LanguageLearning.Cycles.UserCycles;

namespace LanguageLearning.Cycles
{
    /// <inheritdoc/>
    /// CycleID is generated using System.Guid
    public class CycleFactory : ICycleFactory
    {
        /// <inheritdoc/>
        public UserCycle CreateCycle(string username, string courseId, List<UserProgramItem> program)
        {
            return new UserCycle()
            {
                Id = GetGUID(),
                DateCreated = DateTime.Now,
                Username = username,
                CourseID = courseId,
                UserProgramItems = program
            };
        }

        /// <inheritdoc/>
        public IncompleteUserCycle CreateIncompleteCycle(string username, string courseId, int limit)
        {
            return new IncompleteUserCycle(limit)
            { 
                Id = GetGUID(), 
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
