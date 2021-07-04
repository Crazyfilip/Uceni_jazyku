using LanguageLearning.Cycle.Model;
using LanguageLearning.User;
using System;
using System.Windows.Forms;

namespace User_interface
{
    public partial class LoginPage : Form
    {
        UserService userAccountService;

        public LoginPage()
        {
            InitializeComponent();
            userAccountService = new UserService();
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
