using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Uceni_jazyku
{
    class StudentModel : IStudentModel
    {
        public StreamReader GetStudentModel(string user_id)
        {
            //StreamReader database = new StreamReader("UsersDatabase.xml");
            //StreamWriter pom = new StreamWriter("user.xml");
            return new StreamReader("user.xml");

        }

        public void Update(int spravne, int spatne, string user) 
        {
            StreamReader pom = new StreamReader("user.xml");
            string line;
            line = pom.ReadLine();
            line = pom.ReadLine();
            line = pom.ReadLine();
            line = line.Substring(line.IndexOf('>') + 1);
            line = line.Substring(0, line.IndexOf('<'));
            int good = Int32.Parse(line) + spravne;
            line = pom.ReadLine();
            line = line.Substring(line.IndexOf('>') + 1);
            line = line.Substring(0, line.IndexOf('<'));
            int bad = Int32.Parse(line) + spatne;
            pom.Close();
            StreamWriter p = new StreamWriter("user.xml");
            p.WriteLine("<user>");
            p.WriteLine("  <name>{0}</name>",user);
            p.WriteLine("  <good_answers>{0}</good_answers>",good);
            p.WriteLine("  <bad_answers>{0}</bad_answers>",bad);
            p.WriteLine("</user>");
            p.Flush();
            p.Close();
        }
    }
}
