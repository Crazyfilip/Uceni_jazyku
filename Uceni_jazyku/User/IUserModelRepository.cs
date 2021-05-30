namespace Uceni_jazyku.User
{
    /// <summary>
    /// Repository for user models
    /// </summary>
    public interface IUserModelRepository
    {
        /// <summary>
        /// Getter of user model from repository
        /// </summary>
        /// <param name="username">username</param>
        /// <param name="courseId">language course id</param>
        /// <returns>User model</returns>
        public UserModel GetUserModel(string username, string courseId);

        /// <summary>
        /// Insert model to repository
        /// </summary>
        /// <param name="userModel">User model</param>
        public void InsertUserModel(UserModel userModel);
    }
}
