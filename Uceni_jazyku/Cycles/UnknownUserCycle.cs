using System;
using System.Collections.Generic;
using System.Text;

namespace Uceni_jazyku.Cycles
{
  
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
