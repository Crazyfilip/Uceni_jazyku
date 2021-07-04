using LanguageLearning.Common;

namespace LanguageLearning.User.Account
{
    /// <summary>
    /// user account class
    /// User's password is not stored just calculated hash
    /// </summary>
    public class UserAccount : IId
    {
        public virtual string username { get; set; }
        public virtual string loginCredential { get; set; }

        public virtual string salt { get; set; }
        public string Id { get; init; }
    }
}
