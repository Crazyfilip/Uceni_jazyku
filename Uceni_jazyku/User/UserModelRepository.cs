using System.Collections.Generic;
using Uceni_jazyku.Common;

namespace Uceni_jazyku.User
{
    /// <inheritdoc/>
    public class UserModelRepository : AbstractRepository<UserModel>, IUserModelRepository
    {
        public UserModelRepository() : this(null) {}

        public UserModelRepository(Serializer<UserModel> serializer)
        {
            this.serializer = serializer ?? new Serializer<UserModel>() { filepath = "./user-models/database.xml" };
        }

        /// <inheritdoc/>
        public UserModel GetByCourseId(string courseId)
        {
            data = serializer.Load();
            return data.Find(x => x.CourseId == courseId);
        }
    }
}
