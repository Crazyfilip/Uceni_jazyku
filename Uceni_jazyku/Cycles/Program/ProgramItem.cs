using System.Runtime.Serialization;

namespace LanguageLearning.Cycles.Program
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
