using System;
using System.Collections.Generic;
using System.Text;

namespace Uceni_jazyku.User_database
{
    /// <summary>
    /// Interface of operations with the repository of user accounts
    /// </summary>
    public interface IUserAccountRepository
    {
        /// <summary>
        /// Get user account based on username
        /// </summary>
        /// <param name="username">username</param>
        /// <returns>UserAccount</returns>
        UserAccount GetUserAccount(string username);

        /// <summary>
        /// Add user account to the repository
        /// </summary>
        /// <param name="userAccount">Account to add</param>
        void AddUserAccount(UserAccount userAccount);
    }
}
