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
                UserActiveCycle userCycle = cycleService.GetActiveCycle();
                Application.Run(new WelcomePage(userCycle));
            }
            else
            {
                UnknownUserCycle userCycle = (UnknownUserCycle) new CycleFactory().CreateCycle(CycleType.UnknownUserCycle, null, null);
                Application.Run(new LoginPage(userCycle));
            }
        }

        // TODO better solution
        private static void PrepareApp()
        {
            Directory.CreateDirectory("./users");
            Directory.CreateDirectory("./cycles/user-active");
            Directory.CreateDirectory("./cycles/user-finished");
            Directory.CreateDirectory("./cycles/user-inactive");
            Directory.CreateDirectory("./cycles/user-new");
            Directory.CreateDirectory("./cycles/service");
        }
    }
}
