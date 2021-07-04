using System;
using System.Windows.Forms;
using LanguageLearning.Cycles;
using LanguageLearning.User_database;

namespace User_interface
{
    public partial class UserMenu : Form
    {
        private readonly CycleService cycleService = CycleService.GetInstance();
        private UserCycle userCycle;
        private string username;

        public UserMenu(string username)
        {
            InitializeComponent();
            this.username = username;
            this.userCycle = cycleService.GetNextCycle(username);
            labelUser.Text = labelUser.Text.Replace("<username>", userCycle.Username);
            lessonLink.Text = lessonLink.Text.Replace("<lesson>", userCycle.GetNext().LessonRef.Lesson);
        }

        private void buttonLogout_Click(object sender, EventArgs e)
        {
            UserAccountService userAccountService = new UserAccountService();
            userAccountService.Logout();
            new LoginPage().Show();
            Close();
        }
        // event for session update

        private void buttonExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void buttonShowPlan_Click(object sender, EventArgs e)
        {
            new PlanView(username).Show();
        }

        private void lessonLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            new LessonWindow(userCycle).Show();
            Hide();
        }
    }
}
