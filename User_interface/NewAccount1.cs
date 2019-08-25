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
                // TODO druhe okno vytvareni uctu
            }
            else
            {
                MessageBox.Show($"Uživatel se jménem: {textboxName.Text} už existuje, zvolte jiné jméno");
            }
        }
    }
}
