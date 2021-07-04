using System;
using System.Windows.Forms;
using LanguageLearning.Language;
using LanguageLearning.User_database;

namespace User_interface
{
    public partial class NewAccount2 : Form
    {
        LanguageCourseService languageCourseService = new LanguageCourseService();
        UserAccountService userAccountService = new UserAccountService();

        public string username { get; set; }

        public NewAccount2(string username)
        {
            InitializeComponent();
            this.username = username;
            languageCourseService.GetAvailableCourses().ForEach(x => comboBoxCourses.Items.Add(x.Id));
            comboBoxCourses.SelectedItem = "template-default";
        }

        private void buttonFinish_Click(object sender, EventArgs e)
        {
            userAccountService.SetUpAccount(username, comboBoxCourses.SelectedItem.ToString());
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
                    new UserMenu(username).Show();
                    Hide();
                }
            }
        }
    }
}
