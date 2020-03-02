using System;
using System.Windows.Forms;
using Uceni_jazyku.Cycles;

namespace User_interface
{
    public partial class UserMenu : Form
    {
        private UserActiveCycle userCycle;

        public UserMenu(UserActiveCycle userCycle)
        {
            InitializeComponent();
            this.userCycle = userCycle;
        }

        private void buttonLogout_Click(object sender, EventArgs e)
        {
            CycleService cycleService = CycleService.GetInstance();
            cycleService.Inactivate(userCycle);
            UnknownUserCycle uknownUser = (UnknownUserCycle) new CycleFactory().CreateCycle(CycleType.UnknownUserCycle, null, null);
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
