using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uceni_jazyku.Language
{
    public interface ILanguageCourseRepository
    {
        /// <summary>
        /// Get user's active language course
        /// </summary>
        /// <param name="username">username</param>
        /// <returns>Language course</returns>
        LanguageCourse GetActiveCourse(string username);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        List<LanguageCourse> GetInactiveLanguageCourses(string username);
    }
}
