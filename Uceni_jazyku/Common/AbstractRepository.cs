using System.Collections.Generic;

namespace LanguageLearning.Common
{
    /// <summary>
    /// Abstract class for repositories.
    /// Providing implementation of CRUD operations.
    /// Key is assumed to be of type string.
    /// </summary>
    /// <typeparam name="T">type of data in repository</typeparam>
    public abstract class AbstractRepository<T> : IRepository<string, T> where T : IId
    {
        protected List<T> data;
        protected readonly string path;
        protected Serializer<T> serializer;

        /// <inheritdoc/>
        public void Create(T value)
        {
            data = serializer.Load();
            data.Add(value);
            serializer.Save(data);
        }

        /// <inheritdoc/>
        public void Delete(T value)
        {
            data = serializer.Load();
            data.Remove(value);
            serializer.Save(data);
        }

        /// <inheritdoc/>
        public T Get(string key)
        {
            data = serializer.Load();
            return data.Find(x => x.Id == key);
        }

        /// <inheritdoc/>
        public void Update(T value)
        {
            data = serializer.Load();
            int index = data.FindIndex(x => x.Id == value.Id);
            if (index != -1)
            {
                data[index] = value;
            }
            serializer.Save(data);
        }
    }
}
