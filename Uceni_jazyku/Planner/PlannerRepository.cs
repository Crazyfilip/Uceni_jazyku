using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Uceni_jazyku.Language;

namespace Uceni_jazyku.Planner
{
    /// <inheritdoc/>
    public class PlannerRepository : IPlannerRepository
    {
        private List<AbstractPlannerMemory> plannerMemories = new List<AbstractPlannerMemory>();
        private string path = "./planners/database.xml";

        public PlannerRepository() 
        {
            if (File.Exists(path))
            {
                var serializer = new DataContractSerializer(typeof(List<AbstractPlannerMemory>));
                using XmlReader reader = XmlReader.Create(path);
                plannerMemories = (List<AbstractPlannerMemory>)serializer.ReadObject(reader);
            }
        }

        public PlannerRepository(List<AbstractPlannerMemory> plannerMemories)
        {
            this.plannerMemories = plannerMemories;
            Save();
        }

        private void Save()
        {
            var serializer = new DataContractSerializer(typeof(List<AbstractPlannerMemory>));
            using XmlWriter writer = XmlWriter.Create(path);
            serializer.WriteObject(writer, plannerMemories ?? new List<AbstractPlannerMemory>());
        }

        /// <inheritdoc/>
        public AbstractPlannerMemory Get(string course_id)
        {
            return plannerMemories
                .Where(x => x.CourseId == course_id)
                .FirstOrDefault();
        }

        /// <inheritdoc/>
        public void Create(AbstractPlannerMemory plannerMemory)
        {
            plannerMemories.Add(plannerMemory);
            Save();
        }

        /// <inheritdoc/>
        public void Update(AbstractPlannerMemory plannerMemory)
        {
            int index = plannerMemories.FindIndex(x => x.MemoryId == plannerMemory.MemoryId);
            if (index != -1)
            {
                plannerMemories[index] = plannerMemory;
            }
            Save();
        }

        /// <inheritdoc/>
        public void Delete(AbstractPlannerMemory plannerMemory)
        {
            plannerMemories.Remove(plannerMemory);
            Save();
        }
    } 
}
