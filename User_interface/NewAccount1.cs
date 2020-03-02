using System;
using System.Windows.Forms;
using Uceni_jazyku.Cycles;
using Uceni_jazyku.User_database;

namespace User_interface
{
    public partial class NewAccount1 : Form
    {
        UnknownUserCycle userCycle;

        public NewAccount1(UnknownUserCycle userCycle)
        {
            InitializeComponent();
            this.userCycle = userCycle;
        }

        private void buttonContinue_Click(object sender, EventArgs e)
        {
            UserAccountService userAccountService = UserAccountService.GetInstance();
            if (userAccountService.CreateUser(textboxName.Text, textboxPassword.Text))
            {
                MessageBox.Show("Účet vytvořen. Nyní si nastavíte jazykový/-é kurz/-y");
                UserNewCycle userNewCycle = CycleService.GetInstance().GetNewCycle(textboxName.Text);
                NewAccount2 newAccount2 = new NewAccount2(userNewCycle);
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
            new LoginPage(userCycle).Show();
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
