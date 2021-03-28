using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uceni_jazyku.Language.Impl
{
    public class SimpleLanguageCourse : LanguageCourse
    {
        public SimpleLanguageCourse() { }

        // TODO načíst kurz podle CourseId
        public SimpleLanguageCourse(ICollection<LanguageTopic> topics)
        {
            this.CourseId = System.Guid.NewGuid().ToString();
            this.topics = topics;
            this.pickedTopics = new Dictionary<LanguageTopic, bool>();
            foreach (LanguageTopic item in topics)
            {
                pickedTopics.Add(item, false);
            }
        }

        public override List<LanguageTopic> getNextLayer()
        {
            return topics.Where(x => !pickedTopics.ContainsKey(x)).Take(1).ToList();
        }

        public override LanguageTopic selectNextTopic()
        {
            return topics.Where(x => !pickedTopics.ContainsKey(x)).FirstOrDefault();
        }
    }
}
