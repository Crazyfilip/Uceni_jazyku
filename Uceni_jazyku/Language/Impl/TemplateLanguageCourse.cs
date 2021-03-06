﻿using LanguageLearning.Language.Topic;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace LanguageLearning.Language.Impl
{
    /// <summary>
    /// Template of language course.
    /// Not implementing ILanguageTopicSelectStrategy.
    /// Just contains collection of language topics
    /// </summary>
    [DataContract]
    public class TemplateLanguageCourse : LanguageCourse
    {
        // TODO metadata?!
        public TemplateLanguageCourse() { }

        public TemplateLanguageCourse(ICollection<LanguageTopic> topics)
        {
            this.topics = topics;
        }

        public override List<LanguageTopic> getNextLayer()
        {
            throw new NotImplementedException();
        }

        public override LanguageTopic selectNextTopic()
        {
            throw new NotImplementedException();
        }
    }
}
