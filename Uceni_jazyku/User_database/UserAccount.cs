using System;
using System.Collections.Generic;
using System.Text;

namespace Uceni_jazyku.User_database
{
    /// <summary>
    /// user account class
    /// User's password is not stored just calculated hash
    /// </summary>
    public class UserAccount
    {
        public virtual string username { get; set; }
        public virtual string loginCredential { get; set; }

        public virtual string salt { get; set; }
    }
}
