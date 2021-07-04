using LanguageLearning.Common;

namespace LanguageLearning.Planner
{
    /// <summary>
    /// Repository for program planner memories
    /// </summary>
    public interface IPlannerRepository : IRepository<string, AbstractPlannerMemory>
    {
        AbstractPlannerMemory GetByCourseId(string courseId);
    }
}
