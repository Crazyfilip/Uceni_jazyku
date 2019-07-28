﻿using System;
using System.Windows.Forms;
using Uceni_jazyku.User_sessions;

namespace User_interface
{
    public partial class WelcomePage : Form
    {
        private User_session userSession;
        
        public WelcomePage()
        {
            InitializeComponent();
        }

        public WelcomePage(User_session session)
        {
            InitializeComponent();
            userSession = session;
            labelWelcome.Text = labelWelcome.Text.Replace("<username>",session.username);
        }

        private void buttonExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void linkContinue_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            new UserMenu(userSession).Show();
            Hide();
        }

        private void linkDifferentUser_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            // TODO inactivate session
            new LoginPage().Show();
            Hide();
        }
    }
}