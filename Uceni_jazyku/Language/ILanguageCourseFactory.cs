using LanguageLearning.Language.Impl;
using System.Collections.Generic;

namespace LanguageLearning.Language
{
    /// <summary>
    /// Factory of language courses from templates.
    /// Can get instance of whole course or part if based on user's request
    /// </summary>
    public interface ILanguageCourseFactory
    {

        /// <summary>
        /// Create instance of whole language course
        /// </summary>
        /// <param name="template">template of language course</param>
        /// <param name="username">username</param>
        /// <returns>instance of language course</returns>
        LanguageCourse GetLanguageCourse(TemplateLanguageCourse template, string username);

        /// <summary>
        /// Create instance of partial language course based only on selected topics chosen by user
        /// </summary>
        /// <param name="template">template of language course</param>
        /// <param name="username">username</param>
        /// <param name="selectedTopics">selected topics</param>
        /// <returns>instance of partial language course</returns>
        LanguageCourse GetPartialLanguageCourse(TemplateLanguageCourse template, string username, ICollection<string> selectedTopics);

        /// <summary>
        /// Create instance of partial language course based on user's goals.
        /// Next to goals it should pick also topics which are needed for that goal.
        /// </summary>
        /// <param name="template">template of language course</param>
        /// <param name="username">username</param>
        /// <param name="goals">selected goals</param>
        /// <returns>instance of partial language course based on goals</returns>
        LanguageCourse GetGoalBasedLanguageCourse(TemplateLanguageCourse template, string username, ICollection<string> goals);
    }
}
