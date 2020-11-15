using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Uceni_jazyku.Cycles.Program
{
    /// <summary>
    /// Class representing unit of user's cycle program
    /// </summary>
    [DataContract]
    public class UserProgramItem : ProgramItem
    {
        [DataMember]
        public string LanguageCycleRef { get; private set; }
        [DataMember]
        public LanguageProgramItem LessonRef { get; private set; }

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
    }
}
