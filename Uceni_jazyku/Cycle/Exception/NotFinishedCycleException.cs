namespace LanguageLearning.Cycle.Exception
{
    public class NotFinishedCycleException : System.Exception
    {
        public NotFinishedCycleException() { }

        public NotFinishedCycleException(string message) : base(message) { }
    }
}
