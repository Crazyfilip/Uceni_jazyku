using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uceni_jazyku.Language.Impl;

namespace Uceni_jazyku.Language
{
    public class SimpleLanguageCourseFactory : ILanguageCourseFactory
    {
        public LanguageCourse GetLanguageCourse(TemplateLanguageCourse template, string username)
        {
            return new SimpleLanguageCourse(template.topics) { Username = username, CourseId = Guid.NewGuid().ToString(), Active = true };
        }
    }
}
