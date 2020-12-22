using System;
using System.Windows.Forms;
using Uceni_jazyku.User_database;
using Uceni_jazyku.Cycles;

namespace User_interface
{
    public partial class LoginPage : Form
    {
        UserCycle userCycle;
        UserAccountService userAccountService;

        public LoginPage(UserCycle userCycle)
        {
            InitializeComponent();
            this.userCycle = userCycle;
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
                new UserMenu(userActiveCycle).Show();
                Hide();
            }
            else
                MessageBox.Show("Přihlášení neúspěšné. Špatné uživatelské údaje.");
        }

        private void LinkNewAccount_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            new NewAccount1(userCycle, userAccountService).Show();
            Hide();
        }
    }
}
