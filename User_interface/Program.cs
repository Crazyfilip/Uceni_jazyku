using System;
using System.Windows.Forms;
using Uceni_jazyku.User_sessions;
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
            User_session session;
            if (!User_session.UserSessionExists(out session))
                Application.Run(new LoginPage());
            else
            {
                
                Application.Run(new WelcomePage(session));
            }
        }

        private static void PrepareApp()
        {
            Directory.CreateDirectory("./users");
            Directory.CreateDirectory("./sessions/user-active");
        }
    }
}
