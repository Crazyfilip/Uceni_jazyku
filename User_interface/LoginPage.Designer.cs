﻿namespace User_interface
{
    partial class LoginPage
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.linkLanguageSettings = new System.Windows.Forms.LinkLabel();
            this.nameLabel = new System.Windows.Forms.Label();
            this.passwordLabel = new System.Windows.Forms.Label();
            this.textboxName = new System.Windows.Forms.TextBox();
            this.textboxPassword = new System.Windows.Forms.TextBox();
            this.buttonLogin = new System.Windows.Forms.Button();
            this.linkNewAccount = new System.Windows.Forms.LinkLabel();
            this.buttonExit = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::User_interface.Properties.Resources.cz_flag;
            this.pictureBox1.Location = new System.Drawing.Point(13, 3);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(103, 57);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // linkLanguageSettings
            // 
            this.linkLanguageSettings.AutoSize = true;
            this.linkLanguageSettings.Location = new System.Drawing.Point(13, 63);
            this.linkLanguageSettings.Name = "linkLanguageSettings";
            this.linkLanguageSettings.Size = new System.Drawing.Size(103, 13);
            this.linkLanguageSettings.TabIndex = 1;
            this.linkLanguageSettings.TabStop = true;
            this.linkLanguageSettings.Text = "Jazykové nastavení";
            this.linkLanguageSettings.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.languageSetting_LinkClicked);
            // 
            // nameLabel
            // 
            this.nameLabel.AutoSize = true;
            this.nameLabel.Location = new System.Drawing.Point(56, 129);
            this.nameLabel.Name = "nameLabel";
            this.nameLabel.Size = new System.Drawing.Size(38, 13);
            this.nameLabel.TabIndex = 2;
            this.nameLabel.Text = "Jméno";
            // 
            // passwordLabel
            // 
            this.passwordLabel.AutoSize = true;
            this.passwordLabel.Location = new System.Drawing.Point(56, 155);
            this.passwordLabel.Name = "passwordLabel";
            this.passwordLabel.Size = new System.Drawing.Size(34, 13);
            this.passwordLabel.TabIndex = 3;
            this.passwordLabel.Text = "Heslo";
            // 
            // textboxName
            // 
            this.textboxName.Location = new System.Drawing.Point(100, 122);
            this.textboxName.Name = "textboxName";
            this.textboxName.Size = new System.Drawing.Size(145, 20);
            this.textboxName.TabIndex = 4;
            // 
            // textboxPassword
            // 
            this.textboxPassword.Location = new System.Drawing.Point(100, 155);
            this.textboxPassword.Name = "textboxPassword";
            this.textboxPassword.Size = new System.Drawing.Size(145, 20);
            this.textboxPassword.TabIndex = 5;
            // 
            // buttonLogin
            // 
            this.buttonLogin.Location = new System.Drawing.Point(100, 192);
            this.buttonLogin.Name = "buttonLogin";
            this.buttonLogin.Size = new System.Drawing.Size(145, 23);
            this.buttonLogin.TabIndex = 6;
            this.buttonLogin.Text = "Přihlásit";
            this.buttonLogin.UseVisualStyleBackColor = true;
            this.buttonLogin.Click += new System.EventHandler(this.buttonLogin_Click);
            // 
            // linkNewAccount
            // 
            this.linkNewAccount.AutoSize = true;
            this.linkNewAccount.Location = new System.Drawing.Point(147, 228);
            this.linkNewAccount.Name = "linkNewAccount";
            this.linkNewAccount.Size = new System.Drawing.Size(56, 13);
            this.linkNewAccount.TabIndex = 7;
            this.linkNewAccount.TabStop = true;
            this.linkNewAccount.Text = "Nový účet";
            this.linkNewAccount.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LinkNewAccount_LinkClicked);
            // 
            // buttonExit
            // 
            this.buttonExit.Location = new System.Drawing.Point(228, 289);
            this.buttonExit.Name = "buttonExit";
            this.buttonExit.Size = new System.Drawing.Size(75, 23);
            this.buttonExit.TabIndex = 8;
            this.buttonExit.Text = "Ukončit";
            this.buttonExit.UseVisualStyleBackColor = true;
            this.buttonExit.Click += new System.EventHandler(this.buttonExit_Click);
            // 
            // LoginPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(343, 354);
            this.Controls.Add(this.buttonExit);
            this.Controls.Add(this.linkNewAccount);
            this.Controls.Add(this.buttonLogin);
            this.Controls.Add(this.textboxPassword);
            this.Controls.Add(this.textboxName);
            this.Controls.Add(this.passwordLabel);
            this.Controls.Add(this.nameLabel);
            this.Controls.Add(this.linkLanguageSettings);
            this.Controls.Add(this.pictureBox1);
            this.Name = "LoginPage";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.LinkLabel linkLanguageSettings;
        private System.Windows.Forms.Label nameLabel;
        private System.Windows.Forms.Label passwordLabel;
        private System.Windows.Forms.TextBox textboxName;
        private System.Windows.Forms.TextBox textboxPassword;
        private System.Windows.Forms.Button buttonLogin;
        private System.Windows.Forms.LinkLabel linkNewAccount;
        private System.Windows.Forms.Button buttonExit;
    }
}

