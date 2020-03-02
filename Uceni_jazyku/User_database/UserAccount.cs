using System;
using System.Collections.Generic;
using System.Text;

namespace Uceni_jazyku.User_database
{
    public class UserAccount
    {
        public string username { get; set; }
        public string loginCredential { get; set; }

        public string salt { get; set; }
    }
}
