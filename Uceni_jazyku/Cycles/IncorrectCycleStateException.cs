using System;
using System.Collections.Generic;
using System.Text;

namespace Uceni_jazyku.Cycles
{
    public class IncorrectCycleStateException : Exception
    {
        public IncorrectCycleStateException() { }

        public IncorrectCycleStateException(string message) : base(message) { }
    }
}
