using LanguageLearning.Common;

namespace LanguageLearning.User.Model.Impl
{
    /// <inheritdoc/>
    public class UserModelRepository : AbstractRepository<UserModel>, IUserModelRepository
    {
        public UserModelRepository() : this(null) { }

        public UserModelRepository(Serializer<UserModel> serializer)
        {
            this.serializer = serializer ?? new Serializer<UserModel>() { Filepath = "./user-models/database.xml" };
        }

        /// <inheritdoc/>
        public UserModel GetByCourseId(string courseId)
        {
            data = serializer.Load();
            return data.Find(x => x.CourseId == courseId);
        }
    }
}
