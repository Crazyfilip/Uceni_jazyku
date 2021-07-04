using System;
using System.Windows.Forms;
using LanguageLearning.Cycles;

namespace User_interface
{
    public partial class LessonWindow : Form
    {
        private UserCycle userCycle;

        // TODO should not depend on UserCycle
        public LessonWindow(UserCycle userCycle)
        {
            this.userCycle = userCycle;
            InitializeComponent();
        }

        private void buttonFinish_Click(object sender, EventArgs e)
        {
            userCycle = CycleService.GetInstance().Update(userCycle);
            new UserMenu(userCycle.Username).Show();
            Hide();
        }
    }
}
