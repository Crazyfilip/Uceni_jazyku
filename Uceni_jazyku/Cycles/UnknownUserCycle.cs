using System;
using System.Collections.Generic;
using System.Text;

namespace Uceni_jazyku.Cycles
{
  
    /// <summary>
    /// Technical cycle for part where application doesn't what user is using it
    /// So before login or new account creation
    /// </summary>
    public class UnknownUserCycle : UserCycle
    {
        public UnknownUserCycle()
        {
            Username = "UnknownUser";
            RemainingEvents = null;
        }

        protected override void Serialize(string filepath)
        {
            throw new NotSupportedException("UnknownUserCycle doesn't support serialization");
        }

        protected override AbstractCycle Deserialize(string filepath)
        {
            throw new NotSupportedException("UnknownUserCycle doesn't support serialization");
        }
    }
}
