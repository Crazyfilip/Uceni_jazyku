using System;
using System.Windows.Forms;
using Uceni_jazyku.Cycles;
using System.IO;
using log4net;
using System.Reflection;
using log4net.Config;

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
            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));

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
