using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uceni_jazyku.Language;

namespace Uceni_jazyku.Planner
{
    /// <summary>
    /// Repository for program planner memories
    /// </summary>
    public interface IPlannerRepository
    {
        /// <summary>
        /// Get planner memory for given language course
        /// </summary>
        /// <param name="course_id">id of language course</param>
        /// <returns>planner memory</returns>
        AbstractPlannerMemory GetMemory(string course_id);

        /// <summary>
        /// Put new planner memory to repository
        /// </summary>
        /// <param name="plannerMemory">PlannerMemory</param>
        void UpdateMemory(AbstractPlannerMemory plannerMemory);

        /// <summary>
        /// Update existing planner memory record in repository
        /// </summary>
        /// <param name="plannerMemory">PlannerMemory</param>
        void InsertMemory(AbstractPlannerMemory plannerMemory);
    }
}
