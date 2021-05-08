using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Uceni_jazyku.Language.Impl;

namespace Uceni_jazyku.Language
{
    public class LanguageCourseRepository : ILanguageCourseRepository
    {
        /// <summary>
        /// Path to file where is stored collection of cycles
        /// </summary>
        private readonly string path = "./courses/service/database.xml";
        private readonly ICollection<LanguageCourse> languageCourses = new List<LanguageCourse>();


        public LanguageCourseRepository()
        {
            if (File.Exists(path))
            {
                var serializer = new DataContractSerializer(typeof(List<LanguageCourse>));
                using XmlReader reader = XmlReader.Create(path);
                languageCourses = (List<LanguageCourse>)serializer.ReadObject(reader);
            }
        }

        private void Save()
        {
            var serializer = new DataContractSerializer(typeof(List<LanguageCourse>));
            using XmlWriter writer = XmlWriter.Create(path);
            serializer.WriteObject(writer, languageCourses ?? new List<LanguageCourse>());
        }

        public LanguageCourseRepository(ICollection<LanguageCourse> languageCourses)
        {
            this.languageCourses = languageCourses;
        }

        public LanguageCourse GetActiveCourse(string username)
        {
            return languageCourses
                .Where(x => x.Username == username && x.Active)
                .SingleOrDefault();
        }

        public List<LanguageCourse> GetInactiveLanguageCourses(string username)
        {
            return languageCourses
                .Where(x => x.Username == username && !x.Active)
                .ToList();
        }

        public TemplateLanguageCourse GetTemplate(string templateId)
        {
            return languageCourses
                .Where(x => x.CourseId == templateId)
                .Cast<TemplateLanguageCourse>()
                .FirstOrDefault();
        }

        public void InsertCourse(LanguageCourse languageCourse)
        {
            languageCourses.Add(languageCourse);
            Save();
        }
    }
}
