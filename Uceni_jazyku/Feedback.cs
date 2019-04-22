using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Uceni_jazyku
{
    class Feedback : IFeedback
    {
        int good;
        int bad;
        private void Zpracuj(StreamReader vstup)
        {
            string spravnost = vstup.ReadLine();
            spravnost = spravnost.Substring(spravnost.IndexOf('>') + 1);
            spravnost = spravnost.Substring(0, spravnost.IndexOf('<'));
            bool pom = spravnost == "Spravne";
            string zadani = vstup.ReadLine();
            zadani = zadani.Substring(zadani.IndexOf('>') + 1);
            zadani = zadani.Substring(0, zadani.IndexOf('<'));
            string ocek_odp = vstup.ReadLine();
            ocek_odp = ocek_odp.Substring(ocek_odp.IndexOf('>') + 1);
            ocek_odp = ocek_odp.Substring(0, ocek_odp.IndexOf('<'));
            string odp = vstup.ReadLine();
            odp = odp.Substring(odp.IndexOf('>') + 1);
            odp = odp.Substring(0, odp.IndexOf('<'));

            ////////////////////////////////////////////

            Console.WriteLine("Zadáno: {0}", zadani);
            Console.Write("Vaše odpověď je ");
            if (pom)
            {
                good++;
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("správně");
            }
            else
            {
                bad++;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("špatně");
            }
            Console.ResetColor();
            Console.WriteLine("Očekávaná odpověď: {0}", ocek_odp);
            Console.Write("Vaše odpověď: ");
            if (pom)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine(odp);
            }
            else
            {
                for (int i = 0; i < odp.Length; i++)
                {
                    if ((i < ocek_odp.Length) && (odp[i] == ocek_odp[i]))
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write(odp[i]);
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write(odp[i]);
                    }
                }
            }
            Console.ResetColor();
            Console.WriteLine();
        }

        public void GetFeedBack(StreamReader test, IChecker checker, IStudentModel model)
        {
            good = 0;
            bad = 0;
            StreamReader eval = checker.Check(test);
            string line;
            Console.Clear();
            while ((line = eval.ReadLine()) != null)
            {
                if (line == "  <Testovy_priklad>")
                {
                    Zpracuj(eval);
                    Console.WriteLine();
                }
            }
            Console.ReadLine();
            //////////////////////////////////////

            model.Update(good, bad, "Tester");
        }
    }
}
