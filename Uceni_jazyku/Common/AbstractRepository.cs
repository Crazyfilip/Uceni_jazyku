using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uceni_jazyku.Common
{
    public abstract class AbstractRepository<T> : IRepository<string, T> where T : IId
    {
        protected List<T> data;
        protected readonly string path;
        protected Serializer<T> serializer;

        public void Create(T value)
        {
            data = serializer.Load();
            data.Add(value);
            serializer.Save(data);
        }

        public void Delete(T value)
        {
            data = serializer.Load();
            data.Remove(value);
            serializer.Save(data);
        }

        public T Get(string key)
        {
            data = serializer.Load();
            return data.Find(x => x.Id == key);
        }

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
