using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using Uceni_jazyku.Cycles.Program;

namespace Uceni_jazyku.Cycles.UserCycles
{
    [DataContract]
    public class IncompleteUserCycle : UserCycle
    {
        public IncompleteUserCycle(string username)
        {
            this.Username = username;
            this.State = UserCycleState.Inactive;
            isUserAssigned = true;
        }

        public void AddLesson(UserProgramItem item)
        {
            userProgramItems.Insert(0, item);
        }

        // TODO when full then convert to normal UserCycle
    }
}
