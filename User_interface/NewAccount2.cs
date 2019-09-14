using System;
using System.Windows.Forms;
using Uceni_jazyku.User_sessions;

namespace User_interface
{
    public partial class NewAccount2 : Form
    {
        public string username { get; set; }

        public NewAccount2()
        {
            InitializeComponent();
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
                    new UserMenu().Show();
                    User_session.CreateSession(username);
                    Hide();
                }
            }
        }
    }
}
