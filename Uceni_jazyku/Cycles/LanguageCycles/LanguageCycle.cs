using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using Uceni_jazyku.Cycles.Program;

namespace Uceni_jazyku.Cycles.LanguageCycles
{
    [DataContract]
    public class LanguageCycle : AbstractCycle
    {
        [DataMember]
        public int PlannedItems { get; private set; }

        [DataMember]
        List<LanguageProgramItem> languageProgramItems = new List<LanguageProgramItem>();

        public LanguageCycle(List<string> lessons)
        {
            foreach (var lesson in lessons)
            {
                languageProgramItems.Add(new LanguageProgramItem(lesson));
            }
        }

        public override ProgramItem GetNext()
        {
            return languageProgramItems[FinishedEvents];
        }

        public override void Update()
        {
            languageProgramItems[FinishedEvents++].Finish();
        }

        public LanguageProgramItem PlanNext()
        {
            LanguageProgramItem item = languageProgramItems[PlannedItems++];
            item.Plan();
            return item;
        }

        public static LanguageCycle LanguageCycleExample() => new LanguageCycle(new List<string>() { "lekce1", "lekce2" });
    }
}