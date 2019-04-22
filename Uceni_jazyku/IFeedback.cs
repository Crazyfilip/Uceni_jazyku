using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Uceni_jazyku
{
    interface IFeedback
    {
        void GetFeedBack(StreamReader test, IChecker checker, IStudentModel model);
    }
}
