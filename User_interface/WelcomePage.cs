using log4net;
using System;
using System.Windows.Forms;
using Uceni_jazyku.Cycles;
using Uceni_jazyku.Cycles.Program;
using Uceni_jazyku.Language;
using Uceni_jazyku.User_database;

namespace User_interface
{
    public partial class WelcomePage : Form
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(WelcomePage));
        private UserCycle userCycle;
        private CycleService cycleService = CycleService.GetInstance();
        
        public WelcomePage()
        {
            InitializeComponent();
        }

        public WelcomePage(UserCycle cycle)
        {
            log.Info("test");
            InitializeComponent();
            userCycle = cycle;
            labelWelcome.Text = labelWelcome.Text.Replace("<username>", cycle.Username);
            lessonLink.Text = lessonLink.Text.Replace("<lesson>", ((UserProgramItem)cycle.GetNext()).LessonRef.Lesson);
        }

        private void buttonExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void linkContinue_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            new UserMenu(userCycle).Show();
            Hide();
        }

        private void linkDifferentUser_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            UserAccountService userAccountService = new UserAccountService();
            userAccountService.Logout();
            // TODO remove cycle argument for login page
            UserCycle unknownUserCycle = new UserCycle();
            new LoginPage(unknownUserCycle).Show();
            Hide();
        }

        private void linkLanguageSettings_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            new LanguageSettingPage().Show();
        }

        private void lessonLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            new LessonWindow(userCycle).Show();
            Hide();
        }
    }
}
