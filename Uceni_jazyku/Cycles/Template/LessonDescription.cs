using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uceni_jazyku.Cycles.Template
{
    public record LessonDescription
    {
        public LessonSource Source { get; init; }
        public ActivityType RecomendedActivity { get; init; }
    }
}
