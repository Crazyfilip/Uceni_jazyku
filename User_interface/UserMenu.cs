using System;
using System.Windows.Forms;
using Uceni_jazyku.Cycles;
using Uceni_jazyku.Cycles.Program;

namespace User_interface
{
    public partial class UserMenu : Form
    {
        private UserCycle userCycle;

        public UserMenu(UserCycle userCycle)
        {
            InitializeComponent();
            this.userCycle = userCycle;
            labelUser.Text = labelUser.Text.Replace("<username>", userCycle.Username);
            lessonLink.Text = lessonLink.Text.Replace("<lesson>", ((UserProgramItem)userCycle.GetNext()).LessonRef.Lesson);
        }

        private void buttonLogout_Click(object sender, EventArgs e)
        {
            CycleService cycleService = CycleService.GetInstance();
            cycleService.Inactivate(userCycle);
            // TODO get cycle in UnknownUser state from service
            UserCycle uknownUser = new UserCycle();
            new LoginPage(uknownUser).Show();
            Close();
        }
        // event for session update

        private void buttonExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void buttonShowPlan_Click(object sender, EventArgs e)
        {
            new PlanView(userCycle).Show();
        }
    }
}
