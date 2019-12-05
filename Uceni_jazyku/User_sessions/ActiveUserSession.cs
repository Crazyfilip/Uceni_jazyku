using System;
using System.Collections.Generic;
using System.Text;

namespace Uceni_jazyku.User_sessions
{
    public class ActiveUserSession : UserSession
    {

        public ActiveUserSession() => path = "./sessions/user-active/session.txt";

        public ActiveUserSession(string name, int numberOfEvents)
        {
            Username = name;
            RemainingEvents = numberOfEvents;
            path = "./sessions/user-active/session.txt";
        }
    }
}
