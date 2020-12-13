using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Runtime.Serialization;
using System.Text;
using System.Xml.Xsl;
using Uceni_jazyku.Cycles.Program;
using Uceni_jazyku.Cycles.UserCycles;

namespace Uceni_jazyku.Cycles
{
    /// <summary>
    /// Representation of the user's cycles
    /// User cycle reflects user's activity in learning and application
    /// Has role in internal process of adapting to user's progress
    /// </summary>
    [DataContract]
    public class UserCycle : AbstractCycle
    {
        [DataMember]
        public string Username { get; protected set; }

        [DataMember]
        public virtual UserCycleState State { get; protected set; }

        [DataMember]
        public List<UserProgramItem> UserProgramItems { get; protected set; } = new List<UserProgramItem>();

        [DataMember]
        protected bool isUserAssigned;

        [DataMember]
        protected bool isProgramAssigned;

        public UserCycle() { }

        public override void Update()
        {
            UserProgramItem item = UserProgramItems[FinishedEvents++];
            item.Finish();
            item.LessonRef.Finish();
        }

        public override ProgramItem GetNext()
        {
            return UserProgramItems[FinishedEvents];
        }

        /// <summary>
        /// Assign user to cycle if wasn't assigned yet otherwise throw exception.
        /// Also set state of cycle to new
        /// </summary>
        /// <param name="name">user's name</param>
        /// <returns>this instance</returns>
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

        /// <summary>
        /// Assign program to cycle if it wasn's assigned yet otherwise throw exception
        /// </summary>
        /// <param name="userProgramItems"></param>
        /// <returns></returns>
        public UserCycle AssignProgram(List<UserProgramItem> userProgramItems)
        {
            if (!isProgramAssigned)
            {
                this.UserProgramItems = userProgramItems;
                isProgramAssigned = true;
                return this;
            }
            else
            {
                throw new Exception("cycle program already assigned");
            }
        }

        /// <summary>
        /// Update cycle to active state if possible otherwise throw exception
        /// Only new or inactive cycle is possible to activate
        /// </summary>
        /// <returns>this instance</returns>
        public virtual UserCycle Activate()
        {
            if (State == UserCycleState.New || State == UserCycleState.Inactive)
            {
                State = UserCycleState.Active;
                return this;
            }
            else
                throw new Exception("Cycle with state " + State + " cannot be activated");
        }

        /// <summary>
        /// Update cycle to inactive state if possible otherwise throw exception
        /// Only active cycle is possible to inactivate
        /// </summary>
        /// <returns>this instance</returns>
        public virtual UserCycle Inactivate()
        {
            if (State == UserCycleState.Active)
            { 
                State = UserCycleState.Inactive;
                return this;
            }
            else
                throw new Exception("Cycle with state " + State + " cannot be inactivated");
        }

        /// <summary>
        /// Update cycle to finished state if possible otherwise throw exception
        /// Only active cycle with all lessons finished is possible to finish
        /// </summary>
        /// <returns>this instance</returns>
        public virtual void Finish()
        {
            if (State == UserCycleState.Active)
            {
                if (UserProgramItems.TrueForAll(x => x.LessonRef.Finished))
                {
                    State = UserCycleState.Finished;
                }
                else
                    throw new Exception("Cycle doesn't have finished all lesson so can't be finished");
            }
            else
                throw new Exception("Cycle with state " + State + " cannot be finished");
        }

        /// <summary>
        /// Place new program item on place of first unfinished lesson in cycle.
        /// Remove last program item
        /// </summary>
        /// <param name="newLesson"></param>
        /// <returns>Last item of the program</returns>
        public UserProgramItem SwapLesson(UserProgramItem newLesson)
        {
            UserProgramItem item = UserProgramItems.Last();
            UserProgramItems.Insert(FinishedEvents, newLesson);
            UserProgramItems.RemoveAt(UserProgramItems.Count-1);
            return item;
        }

        public override bool Equals(object obj)
        {
            if (Object.ReferenceEquals(obj, null)) return false;
            if (Object.ReferenceEquals(this, obj)) return true;
            if (this.GetType() != obj.GetType()) return false;

            UserCycle cycle = (UserCycle)obj;
            bool result = (this.CycleID == cycle.CycleID)
                && (this.FinishedEvents == cycle.FinishedEvents)
                && (this.State == cycle.State)
                && (this.Username == cycle.Username)
                && (this.isUserAssigned == cycle.isUserAssigned)
                && (this.isProgramAssigned == cycle.isProgramAssigned)
                && (this.UserProgramItems.SequenceEqual(cycle.UserProgramItems));
            return result;
        }

        public override int GetHashCode()
        {
            return CycleID.GetHashCode()
                + FinishedEvents.GetHashCode()
                + State.GetHashCode()
                + Username.GetHashCode()
                + isUserAssigned.GetHashCode()
                + isProgramAssigned.GetHashCode()
                + UserProgramItems.Sum(x => x.GetHashCode());
        }
    }
}
