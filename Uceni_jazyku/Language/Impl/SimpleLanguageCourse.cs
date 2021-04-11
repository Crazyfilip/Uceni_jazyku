using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uceni_jazyku.Language.Impl
{
    /// <summary>
    /// Simple implementation of language course.
    /// Language topics are stored in list and their order is given by their order in list
    /// </summary>
    public class SimpleLanguageCourse : LanguageCourse
    {
        public SimpleLanguageCourse() { }

        // TODO načíst kurz podle CourseId
        public SimpleLanguageCourse(ICollection<LanguageTopic> topics)
        {
            this.CourseId = System.Guid.NewGuid().ToString();
            this.topics = topics;
        }

        public override List<LanguageTopic> getNextLayer()
        {
            return topics.Where(x => !x.Picked).Take(1).ToList();
        }

        public override LanguageTopic selectNextTopic()
        {
            return topics.Where(x => !x.Picked).FirstOrDefault();
        }
    }
}
