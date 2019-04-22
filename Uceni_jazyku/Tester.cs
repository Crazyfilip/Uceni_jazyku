using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Uceni_jazyku
{
    class Tester : ITester
    {
        string faults_file = "Test_Odpovedi.xml";

        private void OtestujPriklad(StreamReader s, StreamWriter vystup)
        {
            string line = s.ReadLine();
            line = line.Substring(line.IndexOf('>')+1);
            line = line.Substring(0, line.IndexOf('<'));
            vystup.WriteLine("    <Zadani>{0}</Zadani>",line);
            Console.WriteLine("Slovo v češtině: {0}", line);
            line = s.ReadLine();
            line = line.Substring(line.IndexOf('>') + 1);
            line = line.Substring(0, line.IndexOf('<'));
            vystup.WriteLine("    <Ocekavana_odpoved>{0}</Ocekavana_odpoved>", line);
            Console.Write("Překlad: ");
            string odpoved;
            odpoved = Console.ReadLine();
            vystup.WriteLine("    <Odpoved>{0}</Odpoved>", odpoved);
        }

        public void DoTest()
        {
            StreamWriter vystup = new StreamWriter(faults_file);
            IGenerator gen = new Generator();
            StreamReader reader = gen.Generate(null, null, new GenDataWords());
            String line;
            vystup.WriteLine("<Test>");
            while ((line = reader.ReadLine()) != null)
            {
                if (line == "  <Priklad>")
                {
                    vystup.WriteLine("  <Testovy_priklad>");
                    OtestujPriklad(reader, vystup);
                    vystup.WriteLine("  </Testovy_priklad>");
                }
            }
            vystup.WriteLine("</Test>");
            vystup.Flush();
            vystup.Dispose();
            IFeedback fb = new Feedback();
            fb.GetFeedBack(new StreamReader(faults_file),new Checker(), new StudentModel());
        }
    }
}
