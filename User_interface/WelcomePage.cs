using log4net;
using System;
using System.Windows.Forms;
using Uceni_jazyku.Cycles;
using Uceni_jazyku.User_database;

namespace User_interface
{
    public partial class WelcomePage : Form
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(WelcomePage));
        private string username;
        private UserCycle userCycle;
        private readonly CycleService cycleService = CycleService.GetInstance();
        
        public WelcomePage()
        {
            InitializeComponent();
        }

        public WelcomePage(string username)
        {
            InitializeComponent();
            this.username = username;
            userCycle = cycleService.GetNextCycle(username);
            labelWelcome.Text = labelWelcome.Text.Replace("<username>", username);
            lessonLink.Text = lessonLink.Text.Replace("<lesson>", userCycle.GetNext().LessonRef.Lesson);
        }

        private void buttonExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void linkContinue_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            new UserMenu(username).Show();
            Hide();
        }

        private void linkDifferentUser_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            UserAccountService userAccountService = new UserAccountService();
            userAccountService.Logout();
            new LoginPage().Show();
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
