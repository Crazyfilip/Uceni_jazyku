namespace Uceni_jazyku.Cycles
{
    /// <summary>
    /// Interface for operation with cache for user's active cycle
    /// </summary>
    interface IActiveCycleCache
    {
        /// <summary>
        /// Empty the cache
        /// </summary>
        void DropCache();

        /// <summary>
        /// Retrieve an active cycle from the cache
        /// </summary>
        /// <returns>Cached active cycle or null</returns>
        UserCycle GetFromCache();

        /// <summary>
        /// Insert or update a cycle in the cache
        /// </summary>
        /// <param name="cycle"></param>
        /// <exception cref="System.ArgumentException">if cycle is not in active state</exception>
        void InsertToCache(UserCycle cycle);

        /// <summary>
        /// Test if there is a cycle in the cache
        /// </summary>
        /// <returns>true if cache is filled</returns>
        bool IsCacheFilled();
    }
}
