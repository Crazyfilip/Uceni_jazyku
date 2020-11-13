namespace Uceni_jazyku.Cycles.Program
{
    public class LanguageProgramItem : ProgramItem
    {
        public string Lesson { get; private set; }

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
    }
}