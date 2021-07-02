
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;

namespace Uceni_jazyku.Common
{
    /// <summary>
    /// Common implementation of data serialization used in repository objects.
    /// </summary>
    /// <typeparam name="T">Type of serialized data</typeparam>
    public class Serializer<T>
    {
        /// <summary>
        /// Path to file to which will be data serialized.
        /// </summary>
        public string Filepath { get; init; }

        /// <summary>
        /// Deserialize data from file if exists.
        /// </summary>
        /// <returns>Collection of data or empty collection if file doesn't exists</returns>
        public virtual List<T> Load()
        {
            if (File.Exists(Filepath))
            {
                var serializer = new DataContractSerializer(typeof(List<T>));
                using XmlReader reader = XmlReader.Create(Filepath);
                return (List<T>)serializer.ReadObject(reader);
            }
            else
            {
                return new List<T>();
            }
        }

        /// <summary>
        /// Serialize data to file. If null is provided then it will serialize as empty collection.
        /// </summary>
        /// <param name="data">Data for serialization</param>
        public virtual void Save(List<T> data)
        {
            var serializer = new DataContractSerializer(typeof(List<T>));
            using XmlWriter writer = XmlWriter.Create(Filepath);
            serializer.WriteObject(writer, data ?? new List<T>());
        }
    }
}
