using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uceni_jazyku.Cycles.Template;

namespace Uceni_jazyku.User
{
    public class UserModel
    {
        public string Username { get; init; }
        public string CourseId { get; init; }
        public string ModelId { get; init; }

        public List<LessonDescription> CycleTemplate { get; protected set; }

        public UserModel()
        {
            CycleTemplate = new List<LessonDescription>() { new LessonDescription() { Source = LessonSource.ANY, RecomendedActivity = ActivityType.ANY }};
        }

        // TODO methods for information about user's knowledge
    }
}
