using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml;

namespace Uceni_jazyku.User
{
    /// <inheritdoc/>
    public class UserModelRepository : IUserModelRepository
    {
        List<UserModel> database = new List<UserModel>();
        private string path = "./user-models/database.xml";

        public UserModelRepository()
        {
            if (File.Exists(path))
            {
                var serializer = new DataContractSerializer(typeof(List<UserModel>));
                using XmlReader reader = XmlReader.Create(path);
                database = (List<UserModel>)serializer.ReadObject(reader);
            }
        }

        private void Save()
        {
            var serializer = new DataContractSerializer(typeof(List<UserModel>));
            using XmlWriter writer = XmlWriter.Create(path);
            serializer.WriteObject(writer, database ?? new List<UserModel>());
        }

        /// <inheritdoc/>
        public UserModel GetUserModel(string username, string courseId)
        {
            return database
                .Where(x => x.Username == username && x.CourseId == courseId)
                .FirstOrDefault();
        }

        /// <inheritdoc/>
        public void InsertUserModel(UserModel userModel)
        {
            database.Add(userModel);
            Save();
        }
    }
}
