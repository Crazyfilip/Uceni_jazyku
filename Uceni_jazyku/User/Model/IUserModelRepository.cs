using LanguageLearning.Common;

namespace LanguageLearning.User.Model
{
    /// <summary>
    /// Repository for user models
    /// </summary>
    public interface IUserModelRepository : IRepository<string, UserModel>
    {
        /// <summary>
        /// Get user model for language course
        /// </summary>
        /// <param name="courseId">id of course</param>
        /// <returns>UserModel</returns>
        UserModel GetByCourseId(string courseId);
    }
}
