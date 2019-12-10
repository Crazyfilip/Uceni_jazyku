using System;
using System.Collections.Generic;
using System.Text;

namespace Uceni_jazyku.Cycles
{
    public enum CycleType
    {
        ActiveUserCycle,
        InactiveUserCycle
    }

    public class CycleFactory
    {
        public AbstractCycle CreateCycle(CycleType type, string name, int numberOfEvents)
        {
            switch (type)
            {
                case CycleType.ActiveUserCycle:
                    return new UserActiveCycleFactory().CreateCycle(name, numberOfEvents);
                case CycleType.InactiveUserCycle:
                    return new InactiveUSerCycleFactory().CreateCycle(name, numberOfEvents);
                default:
                    throw new ArgumentException("parametr type is not valid");
            }
        }

        protected virtual AbstractCycle CreateCycle(string name, int numberOfEvents) { throw new NotSupportedException(); }
    }

    class UserActiveCycleFactory : CycleFactory
    {
        protected override AbstractCycle CreateCycle(string name, int numberOfEvents)
        {
            return new UserActiveCycle(name, numberOfEvents);
        }
    }

    class InactiveUSerCycleFactory : CycleFactory
    {
        protected override AbstractCycle CreateCycle(string name, int numberOfEvents)
        {
            throw new NotImplementedException();
        }
    }
}
