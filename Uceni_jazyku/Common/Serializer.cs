
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;

namespace Uceni_jazyku.Common
{
    public class Serializer<T>
    {
        public string filepath { get; init; }

        public virtual List<T> Load()
        {
            if (File.Exists(filepath))
            {
                var serializer = new DataContractSerializer(typeof(List<T>));
                using XmlReader reader = XmlReader.Create(filepath);
                return (List<T>)serializer.ReadObject(reader);
            }
            else
            {
                return new List<T>();
            }
        }

        public virtual void Save(List<T> data)
        {
            var serializer = new DataContractSerializer(typeof(List<T>));
            using XmlWriter writer = XmlWriter.Create(filepath);
            serializer.WriteObject(writer, data ?? new List<T>());
        }
    }
}
