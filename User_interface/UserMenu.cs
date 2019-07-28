﻿using System;
using System.Windows.Forms;
using Uceni_jazyku.User_sessions;

namespace User_interface
{
    public partial class UserMenu : Form
    {
        private User_session userSession;

        public UserMenu()
        {
            InitializeComponent();
        }

        public UserMenu(User_session userSession)
        {
            InitializeComponent();
            this.userSession = userSession;
        }

        private void buttonLogout_Click(object sender, EventArgs e)
        {
            new LoginPage().Show();
            Close();
        }
        // event for session update

        private void buttonExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
