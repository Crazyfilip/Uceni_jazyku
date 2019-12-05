using System;
using System.Collections.Generic;
using System.Text;

namespace Uceni_jazyku.User_sessions
{
    public enum SessionType
    {
        ActiveUserSession,
        InactiveUserSession
    }

    public class SessionFactory
    {
        public AbstractSession GetSession(SessionType type, string name, int numberOfEvents)
        {
            switch (type)
            {
                case SessionType.ActiveUserSession:
                    return new ActiveUserSessionFactory().CreateSession(name, numberOfEvents);
                case SessionType.InactiveUserSession:
                    return new InactiveUSerSessionFactory().CreateSession(name, numberOfEvents);
                default:
                    throw new ArgumentException("parametr type is not valid");
            }
        }

        protected virtual AbstractSession CreateSession(string name, int numberOfEvents) { return null; }
    }

    class ActiveUserSessionFactory : SessionFactory
    {
        protected override AbstractSession CreateSession(string name, int numberOfEvents)
        {
            return new ActiveUserSession(name, numberOfEvents);
        }
    }

    class InactiveUSerSessionFactory : SessionFactory
    {
        protected override AbstractSession CreateSession(string name, int numberOfEvents)
        {
            throw new NotImplementedException();
        }
    }
}
