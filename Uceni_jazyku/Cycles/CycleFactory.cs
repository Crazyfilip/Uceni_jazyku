using System;
using System.Collections.Generic;
using System.Text;

namespace Uceni_jazyku.Cycles
{
    public enum SessionType
    {
        ActiveUserSession,
        InactiveUserSession
    }

    public class CycleFactory
    {
        public AbstractCycle GetSession(SessionType type, string name, int numberOfEvents)
        {
            switch (type)
            {
                case SessionType.ActiveUserSession:
                    return new UserActiveSessionFactory().CreateSession(name, numberOfEvents);
                case SessionType.InactiveUserSession:
                    return new InactiveUSerSessionFactory().CreateSession(name, numberOfEvents);
                default:
                    throw new ArgumentException("parametr type is not valid");
            }
        }

        protected virtual AbstractCycle CreateSession(string name, int numberOfEvents) { throw new NotSupportedException(); }
    }

    class UserActiveSessionFactory : CycleFactory
    {
        protected override AbstractCycle CreateSession(string name, int numberOfEvents)
        {
            return new UserActiveCycle(name, numberOfEvents);
        }
    }

    class InactiveUSerSessionFactory : CycleFactory
    {
        protected override AbstractCycle CreateSession(string name, int numberOfEvents)
        {
            throw new NotImplementedException();
        }
    }
}
