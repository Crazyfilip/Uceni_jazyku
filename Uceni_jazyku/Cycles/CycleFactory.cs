using System;
using System.Collections.Generic;
using System.Text;

namespace Uceni_jazyku.Cycles
{
    /// <summary>
    /// List of cycle types
    /// </summary>
    public enum CycleType
    {
        UserNewCycle,
        UserActiveCycle,
        UserInactiveCycle,
        UserFinishedCycle,
        UnknownUserCycle
    }

    /// <summary>
    /// Factory class to create correct cycle
    /// </summary>
    public class CycleFactory
    {
        /// <summary>
        /// Factory method when given type and name
        /// </summary>
        /// <param name="type">cycle type</param>
        /// <param name="name">name for cycle</param>
        /// <returns>instance of cycle</returns>
        public AbstractCycle CreateCycle(CycleType type, string name)
        {
            return CreateCycle(type, name, null);
        }

        /// <summary>
        /// Factory method when given type, name and number of events in cycle
        /// </summary>
        /// <param name="type">cycle type</param>
        /// <param name="name">name of cycle</param>
        /// <param name="numberOfEvents">number of events in cycle</param>
        /// <returns>instance of cycle</returns>
        public AbstractCycle CreateCycle(CycleType type, string name, int? numberOfEvents)
        {
            return type switch
            {
                CycleType.UserActiveCycle => new UserActiveCycleFactory().CreateCycle(name, numberOfEvents),
                CycleType.UserInactiveCycle => new UserInactiveCycleFactory().CreateCycle(name, numberOfEvents),
                CycleType.UserNewCycle => new UserNewCycleFactory().CreateCycle(name),
                CycleType.UserFinishedCycle => new UserFinishedCycleFactory().CreateCycle(name),
                CycleType.UnknownUserCycle => new UnknownUserCycleFactory().CreateCycle(),
                _ => throw new ArgumentException("parametr type is not valid"),
            };
        }

        protected virtual AbstractCycle CreateCycle(string name, int? numberOfEvents) { throw new NotSupportedException(); }
        protected virtual AbstractCycle CreateCycle(string name) { throw new NotSupportedException(); }
        protected virtual AbstractCycle CreateCycle() { throw new NotSupportedException(); }
    }

    /// <summary>
    /// Factory subclass handling creation of UnknownUserCycle
    /// </summary>
    class UnknownUserCycleFactory : CycleFactory
    {
        protected override AbstractCycle CreateCycle()
        {
            return new UnknownUserCycle();
        }
    }

    /// <summary>
    /// Factory subclass handling creation of UserActiveCycle
    /// </summary>
    class UserActiveCycleFactory : CycleFactory
    {
        protected override AbstractCycle CreateCycle(string name, int? numberOfEvents)
        {
            return new UserActiveCycle(name, numberOfEvents.Value);
        }
    }


    /// <summary>
    /// Factory subclass handling creation of UserInactiveCycle
    /// </summary>
    class UserInactiveCycleFactory : CycleFactory
    {
        protected override AbstractCycle CreateCycle(string name, int? numberOfEvents)
        {
            return new UserInactiveCycle(name, numberOfEvents.Value);
        }
    }

    /// <summary>
    /// Factory subclass handling creation of UserNewCycle
    /// </summary>
    class UserNewCycleFactory : CycleFactory
    {
        protected override AbstractCycle CreateCycle(string name)
        {
            return new UserNewCycle(name);
        }
    }

    /// <summary>
    /// Factory subclass handling creation of UserFinishedCycle
    /// </summary>
    class UserFinishedCycleFactory : CycleFactory
    {
        protected override AbstractCycle CreateCycle(string name)
        {
            return new UserFinishedCycle(name);
        }
    }
}
