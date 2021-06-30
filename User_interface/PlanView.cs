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
        private readonly CycleService cycleService = CycleService.GetInstance();
        private string username;

        public PlanView(string username)
        {
            InitializeComponent();
            this.username = username;
            cycleService.GetPlannedUnfinishedLessons(username).ForEach(x => listBoxPlannedLessons.Items.Add(x.LessonRef.Lesson));
        }

        private void listBoxPlannedLessons_SelectedValueChanged(object sender, EventArgs e)
        {
            // TODO should only change when some value is selected not just listbox is clicked
            // buttonStartSelected.Enabled = true;
        }

        private void buttonPlanNext_Click(object sender, EventArgs e)
        {
            string newLesson = cycleService.GetNextLesson(username);
            listBoxPlannedLessons.Items.Add(newLesson);
        }
    }
}
