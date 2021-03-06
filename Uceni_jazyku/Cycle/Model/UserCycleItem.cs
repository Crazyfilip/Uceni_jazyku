﻿using LanguageLearning.Language.Topic;
using System.Runtime.Serialization;

namespace LanguageLearning.Cycle.Model
{
    /// <summary>
    /// Class representing unit of user cycle's program
    /// Contain reference to language cycle, lesson in it and an information if lesson was finished
    /// </summary>
    [DataContract]
    public class UserCycleItem
    {
        [DataMember]
        public string LanguageTopicRef { get; init; }
        [DataMember]
        public virtual LanguageProgramItem LessonRef { get; init; }

        public UserCycleItem() { }

        /// <summary>
        /// Initialization of program item
        /// </summary>
        /// <param name="langTopicId">Reference to language topic</param>
        /// <param name="languageProgramItem">language item from topic</param>
        public UserCycleItem(string langTopicId, LanguageProgramItem languageProgramItem)
        {
            LanguageTopicRef = langTopicId;
            LessonRef = languageProgramItem;
        }
    }
}
