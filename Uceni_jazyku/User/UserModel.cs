using System.Collections.Generic;
using LanguageLearning.Common;
using LanguageLearning.Cycles.Template;

namespace LanguageLearning.User
{
    public class UserModel : IId
    {
        public string Username { get; init; }
        public string CourseId { get; init; }
        public string Id { get; init; }

        public virtual List<LessonDescription> CycleTemplate { get; protected set; }

        public UserModel()
        {
            CycleTemplate = new List<LessonDescription>() { new LessonDescription() { Source = LessonSource.ANY, Activity = ActivityType.ANY }};
        }

        // TODO methods for information about user's knowledge
    }
}
