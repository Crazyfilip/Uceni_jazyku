using System;
using System.Windows.Forms;
using Uceni_jazyku.Cycles;

namespace User_interface
{
    public partial class NewAccount2 : Form
    {

        private UserCycle userCycle;
        public string username { get; set; }

        public NewAccount2(UserCycle userCycle)
        {
            InitializeComponent();
            this.userCycle = userCycle;
            this.username = userCycle.Username;
        }

        private void buttonFinish_Click(object sender, EventArgs e)
        {
            // TODO save user setting
            using (UserSettingsFinishedWindow window = new UserSettingsFinishedWindow())
            {
                DialogResult result = window.ShowDialog(this);
                if (result == DialogResult.Yes)
                {
                    // TODO vstupni test
                }
                else
                {
                    CycleService cycleService = CycleService.GetInstance();
                    new UserMenu(cycleService.Activate(userCycle)).Show();
                    Hide();
                }
            }
        }
    }
}
