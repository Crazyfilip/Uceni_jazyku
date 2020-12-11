namespace Uceni_jazyku.Cycles
{
    /// <summary>
    /// 
    /// </summary>
    interface IActiveCycleCache
    {
        /// <summary>
        /// Empty cache
        /// </summary>
        void DropCache();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        UserCycle GetFromCache();

        /// <summary>
        /// Insert cycle to cache
        /// </summary>
        /// <param name="cycle"></param>
        void InsertToCache(UserCycle cycle);

        /// <summary>
        /// Test if cache is used
        /// </summary>
        /// <returns>true if cache is filled</returns>
        bool IsCacheFilled();
    }
}
