using System;
using System.Windows.Forms;
using Uceni_jazyku.Cycles;
using System.IO;

namespace User_interface
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            PrepareApp();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            UserCycle userCycle = CycleService.GetInstance().GetActiveCycle();
            if (userCycle != null)
            {
                Application.Run(new WelcomePage(userCycle));
            }
            else
            {
                Application.Run(new LoginPage(new UserCycle()));
            }
        }

        // TODO better solution
        private static void PrepareApp()
        {
            Directory.CreateDirectory("./users");
            Directory.CreateDirectory("./cycles/service");
        }
    }
}
