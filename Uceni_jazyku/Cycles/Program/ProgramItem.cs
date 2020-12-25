using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Uceni_jazyku.Cycles.Program
{
    /// <summary>
    /// Abstract class for program of cycle
    /// </summary>
    [DataContract]
    [KnownType(typeof(UserProgramItem))]
    [KnownType(typeof(LanguageProgramItem))]
    public abstract class ProgramItem
    {
    }
}
