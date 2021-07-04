using System.Collections.Generic;
using LanguageLearning.Cycles.Program;

namespace LanguageLearning.Language
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
            if (PlannedLessons < Lessons.Count)
            {
                Lessons[PlannedLessons].Plan();
                return Lessons[PlannedLessons++];
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Mark first unfinished lesson as finished
        /// </summary>
        public void FinishLesson()
        {
            Lessons[FinishedLessons++].Finish();
        }

        /// <summary>
        /// Mark that topic was picked in course
        /// </summary>
        public virtual void TopicPicked()
        {
            Picked = true;
        }

        /// <summary>
        /// Test if all lessons are finished
        /// </summary>
        /// <returns>true if all lessons are finished otherwise false</returns>
        public bool FinishedAll()
        {
            return Lessons.TrueForAll(x => x.Finished);
        }

        /// <summary>
        /// Test if all lessons are planned
        /// </summary>
        /// <returns>true if all lessons are planned otherwise false</returns>
        public virtual bool PlannedAll()
        {
            return Lessons.TrueForAll(x => x.Planned);
        }
    }
}