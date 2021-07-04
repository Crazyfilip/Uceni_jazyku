namespace LanguageLearning.Common
{
    /// <summary>
    /// Interface for abstraction of data layer
    /// </summary>
    /// <typeparam name="Key"></typeparam>
    /// <typeparam name="Value"></typeparam>
    public interface IRepository<Key, Value>
    {
        /// <summary>
        /// Create new value in repository
        /// </summary>
        /// <param name="value">Value to be created</param>
        void Create(Value value);

        /// <summary>
        /// Get value based on key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        Value Get(Key key);

        /// <summary>
        /// Update value in repository
        /// </summary>
        /// <param name="value">Value to be updated</param>
        void Update(Value value);

        /// <summary>
        /// Delete value from repository
        /// </summary>
        /// <param name="value">Value to be deleted</param>
        void Delete(Value value);
    }
}
