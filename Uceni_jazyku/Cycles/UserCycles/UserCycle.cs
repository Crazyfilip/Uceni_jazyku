using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Runtime.Serialization;
using System.Text;
using System.Xml.Xsl;
using Uceni_jazyku.Cycles.Program;
using Uceni_jazyku.Cycles.UserCycles;

namespace Uceni_jazyku.Cycles
{
    /// <summary>
    /// Abstract class for user cycles.
    /// User cycle reflects user's activity in learning and application
    /// Has role in internal process of adapting to user's progress
    /// </summary>
    [DataContract]
    public class UserCycle : AbstractCycle
    {
        [DataMember]
        public string Username { get; private set; }

        [DataMember]
        public UserCycleState State { get; private set; }

        [DataMember]
        List<UserProgramItem> userProgramItems = new List<UserProgramItem>();

        [DataMember]
        private bool isUserAssigned = false;

        [DataMember]
        private bool isProgramAssigned = false;

        public UserCycle() => State = UserCycleState.UnknownUser;

        public override void Update()
        {
            userProgramItems[FinishedEvents++].Finish();
        }

        public override ProgramItem GetNext()
        {
            return userProgramItems[FinishedEvents];
        }

        public UserCycle AssignUser(string name)
        {
            if (!isUserAssigned)
            {
                Username = name;
                State = UserCycleState.New;
                isUserAssigned = true;
                return this;
            }
            else
            {
                throw new Exception("username already assigned");
            }
        }

        public UserCycle AssignProgram(List<UserProgramItem> userProgramItems)
        {
            if (!isProgramAssigned)
            {
                this.userProgramItems = userProgramItems;
                isProgramAssigned = true;
                return this;
            }
            else
            {
                throw new Exception("cycle program already assigned");
            }
        }


        public UserCycle Activate()
        {
            if (State == UserCycleState.New || State == UserCycleState.Inactive)
            {
                State = UserCycleState.Active;
                return this;
            }
            else
                throw new ArgumentException("Cycle with state " + State + " cannot be activated");
        }

        public UserCycle Inactivate()
        {
            if (State == UserCycleState.Active)
            { 
                State = UserCycleState.Inactive;
                return this;
            }
            else
                throw new ArgumentException("Cycle with state " + State + " cannot be inactivated");
        }

        public UserCycle Finish()
        {
            if (State == UserCycleState.Active)
            {
                if (userProgramItems.TrueForAll(x => x.LessonRef.Finished))
                {
                    State = UserCycleState.Finished;
                    return this;
                }
                else
                    throw new ArgumentException("Cycle doesn't have finished all lesson so can't be finished");
            }
            else
                throw new ArgumentException("Cycle with state " + State + " cannot be finished");
        }
    }
}
