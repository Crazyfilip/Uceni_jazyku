using System;
using System.Windows.Forms;
using Uceni_jazyku.User_database;
using Uceni_jazyku.Cycles;

namespace User_interface
{
    public partial class LoginPage : Form
    {
        UserAccountService userAccountService;

        public LoginPage()
        {
            InitializeComponent();
            userAccountService = new UserAccountService();
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
            UserCycle userActiveCycle = userAccountService.Login(username, password);
            if (userActiveCycle != null)
            {
                MessageBox.Show("Uživatel přihlášen.");
                new UserMenu(username).Show();
                Hide();
            }
            else
                MessageBox.Show("Přihlášení neúspěšné. Špatné uživatelské údaje.");
        }

        private void LinkNewAccount_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            new NewAccount1(userAccountService).Show();
            Hide();
        }
    }
}
