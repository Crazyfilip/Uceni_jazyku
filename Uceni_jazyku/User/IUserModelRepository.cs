using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uceni_jazyku.User
{
    public interface IUserModelRepository
    {
        public UserModel GetUserModel(string username, string courseId);

        public void InsertUserModel(UserModel userModel);
    }
}
