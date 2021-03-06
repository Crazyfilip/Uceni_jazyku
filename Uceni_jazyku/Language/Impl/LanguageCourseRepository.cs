﻿using LanguageLearning.Common;
using System.Collections.Generic;
using System.Linq;

namespace LanguageLearning.Language.Impl
{
    public class LanguageCourseRepository : AbstractRepository<LanguageCourse>, ILanguageCourseRepository
    {

        public LanguageCourseRepository() : this(null) { }

        public LanguageCourseRepository(Serializer<LanguageCourse> serializer)
        {
            this.serializer = serializer ?? new Serializer<LanguageCourse>() { Filepath = "./courses/service/database.xml" };
        }

        /// <inheritdoc/>
        public LanguageCourse GetActiveCourse(string username)
        {
            data = serializer.Load();
            return data
                .Where(x => x.Username == username && x.Active)
                .SingleOrDefault();
        }

        /// <inheritdoc/>
        public List<TemplateLanguageCourse> GetAllTemplates()
        {
            data = serializer.Load();
            return data
                .Where(x => x is TemplateLanguageCourse)
                .Cast<TemplateLanguageCourse>()
                .ToList();
        }

        /// <inheritdoc/>
        public List<LanguageCourse> GetInactiveLanguageCourses(string username)
        {
            data = serializer.Load();
            return data
                .Where(x => x.Username == username && !x.Active)
                .ToList();
        }

        /// <inheritdoc/>
        public TemplateLanguageCourse GetTemplate(string templateId)
        {
            data = serializer.Load();
            return data
                .Where(x => x.Id == templateId && x is TemplateLanguageCourse)
                .Cast<TemplateLanguageCourse>()
                .FirstOrDefault();
        }
    }
}
