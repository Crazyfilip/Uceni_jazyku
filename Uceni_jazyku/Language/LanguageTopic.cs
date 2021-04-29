using System.Collections.Generic;
using Uceni_jazyku.Cycles.Program;

namespace Uceni_jazyku.Language
{
    /// <summary>
    /// Class representing topic of group of lessons
    /// </summary>
    public class LanguageTopic
    {
        public virtual string TopicId { get; init; }
        public string Description { get; init; }
        public List<LanguageProgramItem> Lessons { get; init; }

        public int PlannedLessons { get; private set; }
        public int FinishedLessons { get; private set; }

        public virtual bool Picked { get; private set; }

        /// <summary>
        /// Get next unplanned lesson, if no lesson is available then return null
        /// </summary>
        /// <returns>Next lesson</returns>
        public virtual LanguageProgramItem PlanNextLesson()
        {
            return PlannedLessons < Lessons.Count ? Lessons[PlannedLessons++] : null;
        }

        /// <summary>
        /// Mark first unfinished lesson as finished
        /// </summary>
        public void FinishLesson()
        {
            Lessons[FinishedLessons++].Finish();
        }

        public virtual void TopicPicked()
        {
            Picked = true;
        }
    }
}