using System;
using System.Windows.Forms;
using Uceni_jazyku.User_database;
using Uceni_jazyku.Cycles;

namespace User_interface
{
    public partial class LoginPage : Form
    {
        public LoginPage()
        {
            InitializeComponent();
        }

        private void languageSetting_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            new LanguageSettingPage().Show();
        }

        private void buttonExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void buttonLogin_Click(object sender, EventArgs e)
        {
            string username = textboxName.Text;
            string password = textboxPassword.Text;
            User_session userSession = User_account.Login(username, password);
            if (userSession != null)
            {
                MessageBox.Show("Uživatel přihlášen.");
                new UserMenu(userSession).Show();
                Hide();
            }
            else
                MessageBox.Show("Přihlášení neúspěšné. Špatné uživatelské údaje.");
        }

        private void linkNewAccount_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            new NewAccount1().Show();
            Hide();
        }
    }
}
