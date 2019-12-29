using System;
using System.Windows.Forms;
using Uceni_jazyku.Cycles;

namespace User_interface
{
    public partial class WelcomePage : Form
    {
        private UserCycle userCycle;
        
        public WelcomePage()
        {
            InitializeComponent();
        }

        public WelcomePage(UserCycle cycle)
        {
            InitializeComponent();
            userCycle = cycle;
            labelWelcome.Text = labelWelcome.Text.Replace("<username>",cycle.Username);
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
            // TODO inactivate cycle
            CycleService cycleService = CycleService.GetInstance();
            userCycle = (UserCycle) cycleService.CreateCycle(CycleType.UnknownUserCycle, null, null);
            new LoginPage(userCycle).Show();
            Hide();
        }

        private void linkLanguageSettings_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            new LanguageSettingPage().Show();
        }
    }
}
