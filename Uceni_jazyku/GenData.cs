using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Uceni_jazyku
{
    class GenData : IGenData
    {
        private string GetWord(String[] x)  
        {
            string res = "";
            for (int i = 0; i < x.Length; i++)
            {
                if (x[i][0] == '[')
                {
                    x[i] = x[i].Remove(0, 1);
                    x[i] = x[i].Remove(x[i].Length - 1, 1);
                    res = x[i];
                    x[i] = "_______";
                    break;
                }
            }
            return res;
        }

        public StreamReader GetDataFile()
        {
            String output = "data.xml";
            StreamWriter file = new StreamWriter(output);
            file.WriteLine("<Data>");
            StreamReader data = new StreamReader("data.txt");
            string line;
            while ((line = data.ReadLine()) != null)
            {
                String[] pom = line.Split(' ');
                string word = GetWord(pom);
                file.WriteLine("  <Priklad>");
                file.Write("    <Veta>");
                for (int i = 0; i < pom.Length-1; i++)
                {
                    file.Write(pom[i] + " ");
                }
                file.WriteLine("{0}</Veta>", pom[pom.Length - 1]);
                file.WriteLine("    <Slovo>{0}</Slovo>", word);
                file.WriteLine("  </Priklad>");
            }
            file.WriteLine("</Data>");
            file.Flush();
            file.Dispose();
            return new StreamReader(output);
        }
    }
}
