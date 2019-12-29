using System;
using System.Collections.Generic;
using System.Text;

namespace Uceni_jazyku.Cycles
{
    public enum CycleType
    {
        UserNewCycle,
        UserActiveCycle,
        UserInactiveCycle,
        UserFinishedCycle,
        UnknownUserCycle
    }

    public class CycleFactory
    {
        public AbstractCycle CreateCycle(CycleType type, string name)
        {
            return CreateCycle(type, name, null);
        }

        public AbstractCycle CreateCycle(CycleType type, string name, int? numberOfEvents)
        {
            switch (type)
            {
                case CycleType.UserActiveCycle:
                    return new UserActiveCycleFactory().CreateCycle(name, numberOfEvents);
                case CycleType.UserInactiveCycle:
                    return new UserInactiveCycleFactory().CreateCycle(name, numberOfEvents);
                case CycleType.UserNewCycle:
                    return new UserNewCycleFactory().CreateCycle(name);
                case CycleType.UserFinishedCycle:
                    return new UserFinishedCycleFactory().CreateCycle(name);
                case CycleType.UnknownUserCycle:
                    return new UnknownUserCycleFactory().CreateCycle();
                default:
                    throw new ArgumentException("parametr type is not valid");
            }
        }

        protected virtual AbstractCycle CreateCycle(string name, int? numberOfEvents) { throw new NotSupportedException(); }
        protected virtual AbstractCycle CreateCycle(string name) { throw new NotSupportedException(); }
        protected virtual AbstractCycle CreateCycle() { throw new NotSupportedException(); }
    }

    class UnknownUserCycleFactory : CycleFactory
    {
        protected override AbstractCycle CreateCycle()
        {
            return new UnknownUserCycle();
        }
    }

    class UserActiveCycleFactory : CycleFactory
    {
        protected override AbstractCycle CreateCycle(string name, int? numberOfEvents)
        {
            return new UserActiveCycle(name, numberOfEvents.Value);
        }
    }

    class UserInactiveCycleFactory : CycleFactory
    {
        protected override AbstractCycle CreateCycle(string name, int? numberOfEvents)
        {
            return new UserInactiveCycle(name, numberOfEvents.Value);
        }
    }

    class UserNewCycleFactory : CycleFactory
    {
        protected override AbstractCycle CreateCycle(string name)
        {
            return new UserNewCycle(name);
        }
    }

    class UserFinishedCycleFactory : CycleFactory
    {
        protected override AbstractCycle CreateCycle(string name)
        {
            return new UserFinishedCycle(name);
        }
    }
}
