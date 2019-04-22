using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Uceni_jazyku
{
    interface IStudentModel
    {
        StreamReader GetStudentModel(string user_id);
        void Update(int spravne, int spatne, string user); // pro jednoduchost
    }
}
