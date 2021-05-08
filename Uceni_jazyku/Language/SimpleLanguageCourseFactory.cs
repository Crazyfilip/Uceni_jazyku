using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uceni_jazyku.Language.Impl;

namespace Uceni_jazyku.Language
{
    /// <inheritdoc/>>
    public class SimpleLanguageCourseFactory : ILanguageCourseFactory
    {
        /// <inheritdoc/>>
        public LanguageCourse GetLanguageCourse(TemplateLanguageCourse template, string username)
        {
            return new SimpleLanguageCourse(template.topics) { Username = username, CourseId = Guid.NewGuid().ToString(), Active = true };
        }

        /// <inheritdoc/>>
        public LanguageCourse GetLanguageCourse(TemplateLanguageCourse template, string username, ICollection<string> selectedTopics)
        {
            var courseTopics = template.topics.Where(x => selectedTopics.Contains(x.TopicId)).ToList();
            return new SimpleLanguageCourse(courseTopics) { Username = username, CourseId = Guid.NewGuid().ToString(), Active = true };
        }
    }
}
