using LanguageLearning.Language.Impl;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LanguageLearning.Language
{
    /// <inheritdoc/>>
    /// Factory create instances of type SingleLanguageCourse
    public class SimpleLanguageCourseFactory : ILanguageCourseFactory
    {
        /// <inheritdoc/>
        public LanguageCourse GetGoalBasedLanguageCourse(TemplateLanguageCourse template, string username, ICollection<string> goals)
        {
            // TODO depends on topics to be List
            throw new NotImplementedException();
        }

        /// <inheritdoc/>>
        public LanguageCourse GetLanguageCourse(TemplateLanguageCourse template, string username)
        {
            return new SimpleLanguageCourse(template.topics) { Username = username, Id = Guid.NewGuid().ToString(), Active = true };
        }

        /// <inheritdoc/>>
        /// Just selected topics
        public LanguageCourse GetPartialLanguageCourse(TemplateLanguageCourse template, string username, ICollection<string> selectedTopics)
        {
            var courseTopics = template.topics.Where(x => selectedTopics.Contains(x.TopicId)).ToList();
            return new SimpleLanguageCourse(courseTopics) { Username = username, Id = Guid.NewGuid().ToString(), Active = true };
        }
    }
}
