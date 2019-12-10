using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Uceni_jazyku.Cycles;

namespace Uceni_jazyku
{
    class Program
    {
        static void Main(string[] args)
        {
            Directory.CreateDirectory("./sessions/user-active");
            AbstractCycle session = new UserActiveCycle();
            session.SaveSession();
            UserCycle session2 = CycleService.GetInstance().GetActiveSession();
            //LoadData load = new LoadData();
            //load.LoadDataToDatabase("lekce1_němčina.txt");
            //IGenerator gen = new Generator();
            //StreamReader pom = gen.Generate(null, null, new GenDataWords());
            //ITester test = new Tester();
            //test.DoTest();
            
        }
    }
}
