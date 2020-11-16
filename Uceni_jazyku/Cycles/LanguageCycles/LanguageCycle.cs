using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using Uceni_jazyku.Cycles.Program;

namespace Uceni_jazyku.Cycles.LanguageCycles
{
    /// <summary>
    /// Representation of language cycle
    /// </summary>
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

        /// <summary>
        /// Pick first unplanned program item of cycle and mark it as planned
        /// </summary>
        /// <returns>Planned item</returns>
        public LanguageProgramItem PlanNext()
        {
            LanguageProgramItem item = languageProgramItems[PlannedItems++];
            item.Plan();
            return item;
        }

        /// <summary>
        /// Test example
        /// </summary>
        /// <returns>Instance of test example</returns>
        // TODO remove when no longer needed
        public static LanguageCycle LanguageCycleExample() => new LanguageCycle(new List<string>() { "lekce1", "lekce2" });
    }
}