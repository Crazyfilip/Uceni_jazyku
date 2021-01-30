using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Uceni_jazyku.Cycles;

namespace User_interface
{
    public partial class PlanView : Form
    {
        readonly CycleService cycleService;
        UserCycle userCycle;

        public PlanView(UserCycle userCycle)
        {
            InitializeComponent();
            cycleService = CycleService.GetInstance();
            this.userCycle = userCycle;
        }

        private void listBoxPlannedLessons_SelectedValueChanged(object sender, EventArgs e)
        {
            // TODO should only change when some value is selected not just listbox is clicked
            // buttonStartSelected.Enabled = true;
        }

        private void buttonPlanNext_Click(object sender, EventArgs e)
        {
            string newLesson = cycleService.GetNextLesson(userCycle.Username);
            listBoxPlannedLessons.Items.Add(newLesson);
        }
    }
}
