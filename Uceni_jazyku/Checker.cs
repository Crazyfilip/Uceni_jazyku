using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Uceni_jazyku
{
    class Checker : IChecker
    {
        private void Zpracuj(StreamReader vstup, StreamWriter vystup)
        {
            string zadani = vstup.ReadLine();
            zadani = zadani.Substring(zadani.IndexOf('>') + 1);
            zadani = zadani.Substring(0, zadani.IndexOf('<'));
            string ocek_odp = vstup.ReadLine();
            ocek_odp = ocek_odp.Substring(ocek_odp.IndexOf('>') + 1);
            ocek_odp = ocek_odp.Substring(0, ocek_odp.IndexOf('<'));
            string odp = vstup.ReadLine();
            odp = odp.Substring(odp.IndexOf('>') + 1);
            odp = odp.Substring(0, odp.IndexOf('<'));
            if (odp == ocek_odp)
            {
                vystup.WriteLine("    <vysledek>Spravne</vysledek>");
            }
            else
            {
                vystup.WriteLine("    <vysledek>Spatne</vysledek>");
            }
            vystup.WriteLine("    <Zadani>{0}</Zadani>", zadani);
            vystup.WriteLine("    <Ocekavana_odpoved>{0}</Ocekavana_odpoved>", ocek_odp);
            vystup.WriteLine("    <Odpoved>{0}</Odpoved>", odp);
        }
        public StreamReader Check(StreamReader test)
        {
            StreamWriter pom = new StreamWriter("evaluation.xml");
            pom.WriteLine("<eval>");
            string line;
            while ((line = test.ReadLine()) != null)
            {
                if (line == "  <Testovy_priklad>")
                {
                    pom.WriteLine("  <Testovy_priklad>");
                    Zpracuj(test, pom);
                    pom.WriteLine("  </Testovy_priklad>");
                }
            }
            pom.WriteLine("</eval>");
            pom.Flush();
            pom.Close();
            return new StreamReader("evaluation.xml");
        }
    }
}
