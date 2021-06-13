﻿using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Uceni_jazyku.Cycles.Program;
using Uceni_jazyku.Cycles.UserCycles;

namespace Uceni_jazyku.Cycles
{
    /// <summary>
    /// Representation of the user's cycles
    /// User cycle reflects user's activity in learning and application
    /// Has role in internal process of adapting to user's progress
    /// </summary>
    [KnownType(typeof(UserCycle))]
    [KnownType(typeof(IncompleteUserCycle))]
    [DataContract]
    public class UserCycle
    {
        [DataMember]
        public virtual string CycleID { get; init; }

        [DataMember]
        public virtual string CourseID { get; init; }

        [DataMember]
        public int FinishedEvents { get; protected set; }

        [DataMember]
        public virtual string Username { get; init; }

        [DataMember]
        public virtual UserCycleState State { get; protected set; }

        [DataMember]
        public virtual List<UserProgramItem> UserProgramItems { get; protected set; } = new List<UserProgramItem>();

        [DataMember]
        public virtual bool IsProgramAssigned { get; protected set; }

        [DataMember]
        public virtual DateTime DateCreated { get; init; }

        private static readonly ILog log = LogManager.GetLogger(typeof(UserCycle));

        public UserCycle() { }

        /// <summary>
        /// Update cycle when user did progress in learning => set first unfinished lesson as finished
        /// </summary>
        public virtual void Update() // TODO different name?
        {
            UserProgramItem item = UserProgramItems[FinishedEvents++];
            item.LessonRef.Finish();
        }

        /// <summary>
        /// Get first unfinished lesson
        /// </summary>
        /// <returns>Lesson</returns>
        public virtual ProgramItem GetNext()
        {
            return UserProgramItems[FinishedEvents];
        }

        /// <summary>
        /// Assign program to cycle if it wasn's assigned yet otherwise throw exception
        /// </summary>
        /// <param name="userProgramItems"></param>
        /// <returns>this instance</returns>
        /// <exception cref="Exception">when program is already assigned</exception>
        public virtual UserCycle AssignProgram(List<UserProgramItem> userProgramItems)
        {
            if (!IsProgramAssigned)
            {
                this.UserProgramItems = userProgramItems;
                IsProgramAssigned = true;
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
        /// <exception cref="IncorrectCycleStateException">when cycle is not in correct state for activating</exception>
        public virtual UserCycle Activate()
        {
            if (State == UserCycleState.New || State == UserCycleState.Inactive)
            {
                State = UserCycleState.Active;
                return this;
            }
            else
                throw new IncorrectCycleStateException("Cycle with state " + State + " cannot be activated");
        }

        /// <summary>
        /// Update cycle to inactive state if possible otherwise throw exception
        /// Only active cycle is possible to inactivate
        /// </summary>
        /// <returns>this instance</returns>
        /// <exception cref="IncorrectCycleStateException">when cycle is not in correct state for inactivating</exception>
        public virtual UserCycle Inactivate()
        {
            if (State == UserCycleState.Active)
            { 
                State = UserCycleState.Inactive;
                return this;
            }
            else
                throw new IncorrectCycleStateException("Cycle with state " + State + " cannot be inactivated");
        }

        /// <summary>
        /// Update cycle to finished state if possible otherwise throw exception
        /// Only active cycle with all lessons finished is possible to finish
        /// </summary>
        /// <returns>this instance</returns>
        /// <exception cref="Exception">when not all lessons are finished</exception>
        /// <exception cref="IncorrectCycleStateException">when cycle is not in correct state for finishing</exception>
        public virtual UserCycle Finish()
        {
            if (State == UserCycleState.Active)
            {
                if (UserProgramItems.TrueForAll(x => x.LessonRef.Finished))
                {
                    State = UserCycleState.Finished;
                    return this;
                }
                else
                    throw new Exception("Cycle doesn't have finished all lesson so can't be finished"); // TODO replace with more suitable exception type
            }
            else
                throw new IncorrectCycleStateException("Cycle with state " + State + " cannot be finished");
        }

        /// <summary>
        /// Place new program item on place of first unfinished lesson in cycle.
        /// Remove last program item
        /// </summary>
        /// <param name="newLesson"></param>
        /// <returns>Last item of the program</returns>
        public virtual UserProgramItem SwapLesson(UserProgramItem newLesson)
        {
            UserProgramItem item = UserProgramItems.Last();
            UserProgramItems.Insert(FinishedEvents, newLesson);
            UserProgramItems.RemoveAt(UserProgramItems.Count-1);
            return item;
        }

        public virtual bool AreAllFinished()
        {
            return UserProgramItems.TrueForAll(x => x.LessonRef.Finished);
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
                && (this.IsProgramAssigned == cycle.IsProgramAssigned)
                && (this.DateCreated == cycle.DateCreated)
                && (this.UserProgramItems.SequenceEqual(cycle.UserProgramItems));
            return result;
        }

        public override int GetHashCode()
        {
            return CycleID.GetHashCode()
                + FinishedEvents.GetHashCode()
                + State.GetHashCode()
                + Username.GetHashCode()
                + IsProgramAssigned.GetHashCode()
                + UserProgramItems.Sum(x => x.GetHashCode());
        }
    }
}
