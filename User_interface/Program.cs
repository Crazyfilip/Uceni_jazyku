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
            
            User_session session;
            if (!cycleService.ActiveUserCycleExists())
            {
                // TODO new cycle type cycleService.CreateCycle(CycleType.UnknownUserCycle, "unknown");
                Application.Run(new LoginPage());
            }
            else
            {
                UserCycle userCycle = cycleService.GetActiveCycle();
                Application.Run(new WelcomePage(userCycle));
            }
        }

        private static void PrepareApp()
        {
            Directory.CreateDirectory("./users");
            Directory.CreateDirectory("./sessions/user-active");
        }
    }
}
