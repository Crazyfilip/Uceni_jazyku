using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uceni_jazyku.Language;

namespace Uceni_jazyku.Planner
{
    public interface IPlannerRepository
    {
        AbstractPlannerMemory GetMemory(string username, string course_id);

        void UpdateMemory(AbstractPlannerMemory plannerMemory);

        void InsertMemory(AbstractPlannerMemory plannerMemory);
    }
}
