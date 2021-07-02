using System.Collections.Generic;
using Uceni_jazyku.Common;
using Uceni_jazyku.Language.Impl;

namespace Uceni_jazyku.Language
{
    /// <summary>
    /// Repository for language courses
    /// </summary>
    public interface ILanguageCourseRepository : IRepository<string, LanguageCourse>
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
        /// Get all templates for language courses
        /// </summary>
        /// <returns>Collection of template courses</returns>
        List<TemplateLanguageCourse> GetAllTemplates();
    }
}
