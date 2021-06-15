using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uceni_jazyku.Cycles.Template
{
    /// <summary>
    /// Lesson description is item of user cycle template
    /// describing how planner should pick lesson for use cycle instance
    /// </summary>
    public record LessonDescription
    {
        /// <summary>
        /// Instruction from where planner should pick lesson
        /// </summary>
        public LessonSource Source { get; init; }

        /// <summary>
        /// Instruction for which type of exercises should be mainly in lesson
        /// </summary>
        public ActivityType Activity { get; init; }
    }
}
