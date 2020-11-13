using System;
using System.Windows.Forms;
using Uceni_jazyku.Cycles;

namespace User_interface
{
    public partial class UserMenu : Form
    {
        private UserCycle userCycle;

        public UserMenu(UserCycle userCycle)
        {
            InitializeComponent();
            this.userCycle = userCycle;
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
    }
}
