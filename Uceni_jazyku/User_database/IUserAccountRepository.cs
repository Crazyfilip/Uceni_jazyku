using System;
using System.Collections.Generic;
using System.Text;
using Uceni_jazyku.Common;

namespace Uceni_jazyku.User_database
{
    /// <summary>
    /// Interface of operations with the repository of user accounts
    /// </summary>
    public interface IUserAccountRepository : IRepository<string, UserAccount>
    {
    }
}
