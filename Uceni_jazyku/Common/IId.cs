namespace LanguageLearning.Common
{
    /// <summary>
    /// Interface providing Id field.
    /// Used for types used in AbstractRepository
    /// </summary>
    public interface IId
    {
        string Id { get; init; }
    }
}
