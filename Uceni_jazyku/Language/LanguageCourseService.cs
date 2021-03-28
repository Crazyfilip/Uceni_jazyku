using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uceni_jazyku.Language
{
    public class LanguageCourseService
    {
        private readonly ILanguageCourseRepository languageCourseRepository;

        private static LanguageCourseService instance;

        public LanguageCourseService() : this(null) { } 

        private LanguageCourseService(ILanguageCourseRepository languageCourseRepository)
        {
            this.languageCourseRepository = languageCourseRepository ?? new LanguageCourseRepository();
        }

        public static LanguageCourseService GetInstance()
        {
            if (instance == null)
            {
                instance = new LanguageCourseService(null);
            }
            return instance;
        }

        public static LanguageCourseService GetInstance(ILanguageCourseRepository languageCourseRepository)
        {
            if (instance == null)
            {
                instance = new LanguageCourseService(languageCourseRepository);
            }
            return instance;
        }
        
        public virtual LanguageCourse GetActiveLanguageCourse(string username)
        {
            return languageCourseRepository.GetActiveCourse(username);
        }
    }
}
