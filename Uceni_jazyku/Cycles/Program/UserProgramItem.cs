using System;
using System.Collections.Generic;
using System.Text;

namespace Uceni_jazyku.Cycles.Program
{
    /// <summary>
    /// Class representing unit of user's cycle program
    /// </summary>
    public class UserProgramItem
    {
        public string LanguageCycleRef { get; private set; }
        public LanguageProgramItem LessonRef { get; private set; }
        public bool Finished { get; private set; }

        /// <summary>
        /// Initialization of program item
        /// </summary>
        /// <param name="langCycleId">Reference </param>
        public UserProgramItem(string langCycleId, LanguageProgramItem languageProgramItem)
        {
            LanguageCycleRef = langCycleId;
            LessonRef = languageProgramItem;
            Finished = false;
        }

        /// <summary>
        /// Set Finished flag to true
        /// </summary>
        public void Finish()
        {
            Finished = true;
        }
    }
}
