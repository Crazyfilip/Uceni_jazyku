using System;

namespace LanguageLearning.Cycle
{
    /// <summary>
    /// Exception for case when trying to change state of cycle to incorrect one
    /// </summary>
    public class IncorrectCycleStateException : Exception
    {
        public IncorrectCycleStateException() { }

        public IncorrectCycleStateException(string message) : base(message) { }
    }
}
