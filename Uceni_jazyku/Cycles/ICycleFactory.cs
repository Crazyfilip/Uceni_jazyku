using System;
using System.Collections.Generic;
using System.Text;
using Uceni_jazyku.Cycles.UserCycles;

namespace Uceni_jazyku.Cycles
{
    /// <summary>
    /// 
    /// </summary>
    public interface ICycleFactory
    {
        UserCycle createCycle();

        IncompleteUserCycle createIncompleteCycle(string username);
    }
}
