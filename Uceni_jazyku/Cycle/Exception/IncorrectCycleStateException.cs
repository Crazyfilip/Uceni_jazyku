namespace LanguageLearning.Cycle.Exception
{
    /// <summary>
    /// Exception for case when trying to change state of cycle to incorrect one
    /// </summary>
    public class IncorrectCycleStateException : System.Exception
    {
        public IncorrectCycleStateException() { }

        public IncorrectCycleStateException(string message) : base(message) { }
    }
}
