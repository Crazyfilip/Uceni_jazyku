﻿using System;
using System.Runtime.Serialization;

namespace LanguageLearning.Language.Topic
{
    /// <summary>
    /// Class representing unit of language cycle's program
    /// Contain lesson description and an information if lesson was planned and finished
    /// </summary>
    [DataContract]
    public class LanguageProgramItem
    {
        [DataMember]
        public virtual string Lesson { get; private set; }

        [DataMember]
        public bool Planned { get; private set; }

        [DataMember]
        public virtual bool Finished { get; protected set; }

        public LanguageProgramItem() { }

        public LanguageProgramItem(string lessonDescription)
        {
            Lesson = lessonDescription;
            Finished = false;
        }

        public void Plan()
        {
            Planned = true;
        }

        /// <summary>
        /// Set Finished flag to true
        /// </summary>
        public virtual void Finish()
        {
            Finished = true;
        }

        public override bool Equals(object obj)
        {
            if (Object.ReferenceEquals(obj, null)) return false;
            if (Object.ReferenceEquals(this, obj)) return true;
            if (this.GetType() != obj.GetType()) return false;

            LanguageProgramItem item = (LanguageProgramItem)obj;
            return this.Lesson == item.Lesson
                && this.Finished == item.Finished
                && this.Planned == item.Planned;
        }

        public override int GetHashCode()
        {
            return this.Lesson.GetHashCode()
                + this.Finished.GetHashCode()
                + this.Planned.GetHashCode();
        }
    }
}