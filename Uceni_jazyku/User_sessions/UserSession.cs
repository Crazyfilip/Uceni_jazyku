using System;
using System.Collections.Generic;
using System.Text;

namespace Uceni_jazyku.User_sessions
{

    public class UserSession : AbstractSession
    {
        public UserSession() { }
        public UserSession(string username, int remainingEvents)
        {
            this.Username = username;
            this.RemainingEvents = remainingEvents;
        }

        public override void Update()
        {
            throw new NotImplementedException();
        }
    }
}
