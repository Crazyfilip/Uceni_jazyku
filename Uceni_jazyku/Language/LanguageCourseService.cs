using System.Collections.Generic;
using LanguageLearning.Language.Impl;

namespace LanguageLearning.Language
{
    /// <summary>
    /// Service class for managing language courses
    /// </summary>
    public class LanguageCourseService
    {
        private readonly ILanguageCourseRepository languageCourseRepository;
        private ILanguageCourseFactory languageCourseFactory;

        private static LanguageCourseService instance;

        public LanguageCourseService() : this(null, null) { } 

        private LanguageCourseService(ILanguageCourseRepository languageCourseRepository, ILanguageCourseFactory languageCourseFactory)
        {
            this.languageCourseRepository = languageCourseRepository ?? new LanguageCourseRepository();
            this.languageCourseFactory = languageCourseFactory ?? new SimpleLanguageCourseFactory();
        }

        public static LanguageCourseService GetInstance()
        {
            if (instance == null)
            {
                instance = new LanguageCourseService(null, null);
            }
            return instance;
        }

        public static LanguageCourseService GetInstance(ILanguageCourseRepository languageCourseRepository, ILanguageCourseFactory languageCourseFactory)
        {
            if (instance == null)
            {
                instance = new LanguageCourseService(languageCourseRepository, languageCourseFactory);
            }
            return instance;
        }

        public static void deallocate()
        {
            instance = null;
        }
        
        /// <summary>
        /// Get user's active course
        /// </summary>
        /// <param name="username">username</param>
        /// <returns>Active language course</returns>
        public virtual LanguageCourse GetActiveLanguageCourse(string username)
        {
            return languageCourseRepository.GetActiveCourse(username);
        }

        /// <summary>
        /// Get new instance of language course based on template
        /// </summary>
        /// <param name="templateId">id of template</param>
        /// <param name="username">username</param>
        /// <returns></returns>
        public virtual LanguageCourse GetLanguageCourseInstanceFromTemplate(string templateId, string username)
        {
            TemplateLanguageCourse template = languageCourseRepository.GetTemplate(templateId);
            LanguageCourse course = languageCourseFactory.GetLanguageCourse(template, username);
            languageCourseRepository.Create(course);
            return course;
        }

        /// <summary>
        /// Get all available language courses which means collection of all templates.
        /// </summary>
        /// <returns>Available courses</returns>
        public virtual List<TemplateLanguageCourse> GetAvailableCourses()
        {
            return languageCourseRepository.GetAllTemplates();
        }
    }
}
