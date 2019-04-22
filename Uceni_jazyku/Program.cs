using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Uceni_jazyku
{
    class Program
    {
        static void Main(string[] args)
        {
            LoadData load = new LoadData();
            load.LoadDataToDatabase("lekce1_němčina.txt");
            //IGenerator gen = new Generator();
            //StreamReader pom = gen.Generate(null, null, new GenDataWords());
            //ITester test = new Tester();
            //test.DoTest();
            
        }
    }
}
