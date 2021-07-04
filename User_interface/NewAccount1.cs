using System;
using System.Windows.Forms;
using LanguageLearning.User_database;

namespace User_interface
{
    public partial class NewAccount1 : Form
    {
        UserAccountService userAccountService;

        public NewAccount1(UserAccountService userAccountService)
        {
            InitializeComponent();
            this.userAccountService = userAccountService;
        }

        private void buttonContinue_Click(object sender, EventArgs e)
        {
            if (userAccountService.CreateUser(textboxName.Text, textboxPassword.Text))
            {
                MessageBox.Show("Účet vytvořen. Nyní si nastavíte jazykový/-é kurz/-y");
                NewAccount2 newAccount2 = new NewAccount2(textboxName.Text);
                newAccount2.Show();
                Hide();
            }
            else
            {
                MessageBox.Show($"Uživatel se jménem: {textboxName.Text} už existuje, zvolte jiné jméno");
            }
        }

        private void NewAccount1_FormClosed(object sender, FormClosedEventArgs e)
        {
            new LoginPage().Show();
        }

        private void textboxName_TextChanged(object sender, EventArgs e)
        {
            buttonContinue.Enabled = (textboxName.Text != "") && (textboxPassword.Text != "");
        }

        private void textboxPassword_TextChanged(object sender, EventArgs e)
        {
            buttonContinue.Enabled = (textboxName.Text != "") && (textboxPassword.Text != "");
        }
    }
}
