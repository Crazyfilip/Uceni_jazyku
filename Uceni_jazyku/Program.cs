using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Uceni_jazyku.Cycles;
using System.Runtime.Serialization;
using System.Xml;
using Uceni_jazyku.Language;
using Uceni_jazyku.Language.Impl;
using Uceni_jazyku.Cycles.Program;

namespace Uceni_jazyku
{
    class Program
    {
        static void Main(string[] args)
        {
            LanguageTopic topic1 = new LanguageTopic() { TopicId = "a-id", Description = "A", Lessons = new List<LanguageProgramItem> { new LanguageProgramItem("A1"), new LanguageProgramItem("A2")}};
            LanguageTopic topic2 = new LanguageTopic() { TopicId = "b-id", Description = "B", Lessons = new List<LanguageProgramItem> { new LanguageProgramItem("B1"), new LanguageProgramItem("B2") } };
            TemplateLanguageCourse template = new TemplateLanguageCourse(new List<LanguageTopic>() { topic1, topic2 });
            Directory.CreateDirectory("./courses");
            var serializer = new DataContractSerializer(typeof(LanguageCourse));
            using XmlWriter writer = XmlWriter.Create("./courses/default.xml");
            serializer.WriteObject(writer, template);
        }
    }
}
