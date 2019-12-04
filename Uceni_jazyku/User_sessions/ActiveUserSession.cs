using System;
using System.Collections.Generic;
using System.Text;

namespace Uceni_jazyku.User_sessions
{
    public class ActiveUserSession : UserSession
    {
        public ActiveUserSession() : base("test", 3) // TODO refactor
        {
            this.path = "./sessions/user-active/session.txt";
        }
    }
}
