using System.Collections.Generic;
using Uceni_jazyku.Common;

namespace Uceni_jazyku.Planner
{
    /// <inheritdoc/>
    public class PlannerRepository : AbstractRepository<AbstractPlannerMemory>, IPlannerRepository
    {
        public PlannerRepository() : this(null) {}

        public PlannerRepository(Serializer<AbstractPlannerMemory> serializer)
        {
            this.serializer = serializer ?? new Serializer<AbstractPlannerMemory>() { Filepath = "./planners/database.xml" };
        }

        /// <inheritdoc/>
        public AbstractPlannerMemory GetByCourseId(string courseId)
        {
            data = serializer.Load();
            return data.Find(x => x.CourseId == courseId);
        }
    } 
}
