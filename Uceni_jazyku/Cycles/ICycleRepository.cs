using System;
using System.Collections.Generic;
using System.Text;
using Uceni_jazyku.Cycles.UserCycles;

namespace Uceni_jazyku.Cycles
{
    interface ICycleRepository
    {
        void PutCycle(AbstractCycle cycle);

        void UpdateCycle(AbstractCycle updatedCycle);

        int GetCyclesCount();

        UserCycle GetOldestUserInactiveCycle(string username);

        IncompleteUserCycle GetUserIncompleteCycle(string username);
    }
}
