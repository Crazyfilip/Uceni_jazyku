﻿using LanguageLearning.Common;
using LanguageLearning.Cycle.Exception;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace LanguageLearning.Cycle.Model
{
    /// <summary>
    /// Representation of the user's cycles
    /// User cycle reflects user's activity in learning and application
    /// Has role in internal process of adapting to user's progress
    /// </summary>
    [KnownType(typeof(UserCycle))]
    [KnownType(typeof(IncompleteUserCycle))]
    [DataContract]
    public class UserCycle : IId
    {
        [DataMember]
        public virtual string Id { get; init; }

        [DataMember]
        public virtual string CourseID { get; init; }

        [DataMember]
        public int FinishedEvents { get; protected set; }

        [DataMember]
        public virtual string Username { get; init; }

        [DataMember]
        public virtual UserCycleState State { get; protected set; }

        [DataMember]
        public virtual List<UserCycleItem> UserProgramItems { get; init; } = new List<UserCycleItem>();

        [DataMember]
        public virtual DateTime DateCreated { get; init; }

        private static readonly ILog log = LogManager.GetLogger(typeof(UserCycle));

        public UserCycle() { }

        /// <summary>
        /// Update cycle when user did progress in learning => set first unfinished lesson as finished
        /// </summary>
        public virtual void Update() // TODO different name?
        {
            UserCycleItem item = UserProgramItems[FinishedEvents++];
            item.LessonRef.Finish();
        }

        /// <summary>
        /// Get first unfinished lesson
        /// </summary>
        /// <returns>Lesson</returns>
        public virtual UserCycleItem GetNext()
        {
            return UserProgramItems[FinishedEvents];
        }

        /// <summary>
        /// Update cycle to active state if possible otherwise throw exception
        /// Only inactive cycle is possible to activate
        /// </summary>
        /// <returns>this instance</returns>
        /// <exception cref="IncorrectCycleStateException">when cycle is not in correct state for activating</exception>
        public virtual UserCycle Activate()
        {
            if (State == UserCycleState.Inactive)
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
                    throw new NotFinishedCycleException("Cycle doesn't have finished all lesson so can't be finished"); // TODO replace with more suitable exception type
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
        public virtual UserCycleItem SwapLesson(UserCycleItem newLesson)
        {
            UserCycleItem item = UserProgramItems.Last();
            UserProgramItems.Insert(FinishedEvents, newLesson);
            UserProgramItems.RemoveAt(UserProgramItems.Count - 1);
            return item;
        }

        /// <summary>
        /// Test if all lessons in cycle are finished
        /// </summary>
        /// <returns>true if all lessons are finished</returns>
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
            bool result = (this.Id == cycle.Id)
                && (this.FinishedEvents == cycle.FinishedEvents)
                && (this.State == cycle.State)
                && (this.Username == cycle.Username)
                && (this.DateCreated == cycle.DateCreated)
                && (this.UserProgramItems.SequenceEqual(cycle.UserProgramItems));
            return result;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode()
                + FinishedEvents.GetHashCode()
                + State.GetHashCode()
                + Username.GetHashCode()
                + UserProgramItems.Sum(x => x.GetHashCode());
        }
    }
}
