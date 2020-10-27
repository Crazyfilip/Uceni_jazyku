using System;
using System.Collections.Generic;
using System.Text;

namespace Uceni_jazyku.Cycles.Program
{
    public class CycleProgram
    {
        public List<CycleProgramItem> ProgramItems { get; private set; }

        public CycleProgram()
        {
            ProgramItems = new List<CycleProgramItem>();
        }
    }
}
