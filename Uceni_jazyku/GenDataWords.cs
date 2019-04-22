using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Uceni_jazyku
{
    class GenDataWords : IGenData
    {
        public StreamReader GetDataFile()
        {
            String output = "dataWords.xml";
            StreamWriter file = new StreamWriter(output);
            file.WriteLine("<Data>");
            StreamReader data = new StreamReader("dataWords.txt");
            string line;
            while ((line = data.ReadLine()) != null)
            {
                String[] pom = line.Split('-');
                file.WriteLine("  <Priklad>");
                file.WriteLine("    <Zdrojovy_jazyk>{0}</Zdrojovy_jazyk>", pom[1]);
                file.WriteLine("    <Cilovy_jazyk>{0}</Cilovy_jazyk>", pom[0]);
                file.WriteLine("  </Priklad>");
            }
            file.WriteLine("</Data>");
            file.Flush();
            file.Close();
            return new StreamReader(output);
        }
    }
}
