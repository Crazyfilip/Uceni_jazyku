using Uceni_jazyku.Common;

namespace Uceni_jazyku.Planner
{
    /// <summary>
    /// Repository for program planner memories
    /// </summary>
    public interface IPlannerRepository : IRepository<string, AbstractPlannerMemory>
    {
        AbstractPlannerMemory GetByCourseId(string courseId);
    }
}
