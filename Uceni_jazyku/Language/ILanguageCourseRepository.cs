using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uceni_jazyku.Language.Impl;

namespace Uceni_jazyku.Language
{
    /// <summary>
    /// Repository for language courses
    /// </summary>
    public interface ILanguageCourseRepository
    {
        /// <summary>
        /// Get user's active language course
        /// </summary>
        /// <param name="username">username</param>
        /// <returns>Language course</returns>
        LanguageCourse GetActiveCourse(string username);

        /// <summary>
        /// Get all user's inactive language courses
        /// </summary>
        /// <param name="username">username</param>
        /// <returns>List of language courses</returns>
        List<LanguageCourse> GetInactiveLanguageCourses(string username);

        /// <summary>
        /// Get template language course
        /// </summary>
        /// <param name="templateId">id of template</param>
        /// <returns>template course</returns>
        TemplateLanguageCourse GetTemplate(string templateId);

        /// <summary>
        /// Inserting course to database
        /// </summary>
        /// <param name="languageCourse">LanguageCourse</param>
        void InsertCourse(LanguageCourse languageCourse);
    }
}
