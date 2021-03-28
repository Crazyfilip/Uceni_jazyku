using System.Collections.Generic;

namespace Uceni_jazyku.Language
{
    public interface ILanguageTopicSelectStrategy
    {
        LanguageTopic selectNextTopic();
        List<LanguageTopic> getNextLayer();
    }
}