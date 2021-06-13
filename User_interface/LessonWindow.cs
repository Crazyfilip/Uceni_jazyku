using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Uceni_jazyku.Cycles;

namespace User_interface
{
    public partial class LessonWindow : Form
    {
        private UserCycle userCycle;

        public LessonWindow(UserCycle userCycle)
        {
            this.userCycle = userCycle;
            InitializeComponent();
        }

        private void buttonFinish_Click(object sender, EventArgs e)
        {
            userCycle = CycleService.GetInstance().Update(userCycle);
            new UserMenu(userCycle).Show();
            Hide();
        }
    }
}
