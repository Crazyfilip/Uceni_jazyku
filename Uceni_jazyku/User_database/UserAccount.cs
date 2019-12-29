using System;
using System.Collections.Generic;
using System.Text;

namespace Uceni_jazyku.User_database
{
    class UserAccount
    {
        string username { get; set; }
        string loginCredential { get; set; }

        string salt { get; set; }
    }
}
