using Uceni_jazyku.Language.Impl;

namespace Uceni_jazyku.Language
{
    public interface ILanguageCourseFactory
    {
        LanguageCourse GetLanguageCourse(TemplateLanguageCourse template, string username);
    }
}
