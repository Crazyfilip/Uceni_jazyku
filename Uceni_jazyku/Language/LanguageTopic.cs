using System.Collections.Generic;
using Uceni_jazyku.Cycles.Program;

namespace Uceni_jazyku.Language
{
    public class LanguageTopic
    {
        public virtual string TopicId { get; init; }
        public string Description { get; init; }
        public List<LanguageProgramItem> Lessons { get; init; }

        public int PlannedLessons { get; private set; }
        public int FinishedLessons { get; private set; }

        public virtual LanguageProgramItem PlanNextLesson()
        {
            return PlannedLessons < Lessons.Count ? Lessons[PlannedLessons++] : null;
        }

        public void FinishLesson()
        {
            Lessons[FinishedLessons++].Finish();
        }
    }
}