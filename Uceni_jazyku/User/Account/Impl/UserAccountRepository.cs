using LanguageLearning.Common;
using log4net;

namespace LanguageLearning.User.Account.Impl
{
    /// <inheritdoc/>
    /// <summary>
    /// Repository represented as a list
    /// </summary>
    public class UserAccountRepository : AbstractRepository<UserAccount>, IUserAccountRepository
    {
        private static ILog log = LogManager.GetLogger(typeof(UserAccountRepository));

        /// <summary>
        /// constructor of UserAccountRepository
        /// Load data from file if exists otherwise initialize empty list
        /// </summary>
        public UserAccountRepository() : this(null) { }

        /// <summary>
        /// Initialize repository with existing data
        /// </summary>
        /// <param name="accounts">List of data</param>
        public UserAccountRepository(Serializer<UserAccount> serializer)
        {
            this.serializer = serializer ?? new Serializer<UserAccount>() { Filepath = "./users/accounts.xml" };
        }

        /// <inheritdoc/>
        public UserAccount GetByName(string username)
        {
            log.Info($"Looking for account with username: {username}");
            data = serializer.Load();
            return data.Find(x => x.username == username);
        }
    }
}
