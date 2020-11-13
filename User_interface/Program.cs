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
            CycleService cycleService = CycleService.GetInstance();
            if (cycleService.UserActiveCycleExists())
            {
                UserCycle userCycle = cycleService.GetActiveCycle();
                Application.Run(new WelcomePage(userCycle));
            }
            else
            {
                // TODO get cycle in UnknownUser state from service
                UserCycle userCycle = new UserCycle();
                Application.Run(new LoginPage(userCycle));
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
