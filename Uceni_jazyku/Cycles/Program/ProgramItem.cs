using System;
using System.Collections.Generic;
using System.Text;

namespace Uceni_jazyku.Cycles.Program
{
    public abstract class ProgramItem
    {
        public bool Finished { get; protected set; }

        /// <summary>
        /// Set Finished flag to true
        /// </summary>
        public void Finish()
        {
            Finished = true;
        }
    }
}
