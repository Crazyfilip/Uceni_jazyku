﻿using System;
using System.Collections.Generic;
using System.Text;
using LanguageLearning.Common;

namespace LanguageLearning.User_database
{
    /// <summary>
    /// Interface of operations with the repository of user accounts
    /// </summary>
    public interface IUserAccountRepository : IRepository<string, UserAccount>
    {
        /// <summary>
        /// Get user account by user's name
        /// </summary>
        /// <param name="username">username</param>
        /// <returns>UserAccount</returns>
        UserAccount GetByName(string username);
    }
}
