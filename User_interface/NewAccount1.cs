using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Uceni_jazyku.User_database;

namespace User_interface
{
    public partial class NewAccount1 : Form
    {
        public NewAccount1()
        {
            InitializeComponent();
        }

        private void buttonContinue_Click(object sender, EventArgs e)
        {
            if (User_account.CreateUser(textboxName.Text,textboxPassword.Text))
            {
                MessageBox.Show("Účet vytvořen. Nyní si nastavíte jazykový/-é kurz/-y");
                NewAccount2 newAccount2 = new NewAccount2();
                newAccount2.Show();
                newAccount2.username = textboxName.Text;
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
