using System;
using System.Runtime.Serialization;

namespace Uceni_jazyku.Cycles.Program
{
    /// <summary>
    /// Class representing unit of language cycle's program
    /// Contain lesson description and an information if lesson was planned and finished
    /// </summary>
    [DataContract]
    public class LanguageProgramItem : ProgramItem
    {
        [DataMember]
        public string Lesson { get; private set; }

        [DataMember]
        public bool Planned { get; private set; }

        public LanguageProgramItem(string lessonDescription)
        {
            Lesson = lessonDescription;
            Finished = false;
        }

        public void Plan()
        {
            Planned = true;
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