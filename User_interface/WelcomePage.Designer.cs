namespace User_interface
{
    partial class WelcomePage
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
            this.linkLanguageSettings = new System.Windows.Forms.LinkLabel();
            this.labelWelcome = new System.Windows.Forms.Label();
            this.buttonExit = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.linkContinue = new System.Windows.Forms.LinkLabel();
            this.linkDifferentUser = new System.Windows.Forms.LinkLabel();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // linkLanguageSettings
            // 
            this.linkLanguageSettings.AutoSize = true;
            this.linkLanguageSettings.Location = new System.Drawing.Point(12, 67);
            this.linkLanguageSettings.Name = "linkLanguageSettings";
            this.linkLanguageSettings.Size = new System.Drawing.Size(103, 13);
            this.linkLanguageSettings.TabIndex = 0;
            this.linkLanguageSettings.TabStop = true;
            this.linkLanguageSettings.Text = "Jazykové nastavení";
            this.linkLanguageSettings.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLanguageSettings_LinkClicked);
            // 
            // labelWelcome
            // 
            this.labelWelcome.AutoSize = true;
            this.labelWelcome.Location = new System.Drawing.Point(128, 117);
            this.labelWelcome.Name = "labelWelcome";
            this.labelWelcome.Size = new System.Drawing.Size(99, 13);
            this.labelWelcome.TabIndex = 1;
            this.labelWelcome.Text = "Vítejte <username>";
            // 
            // buttonExit
            // 
            this.buttonExit.Location = new System.Drawing.Point(258, 273);
            this.buttonExit.Name = "buttonExit";
            this.buttonExit.Size = new System.Drawing.Size(75, 23);
            this.buttonExit.TabIndex = 2;
            this.buttonExit.Text = "Ukončit";
            this.buttonExit.UseVisualStyleBackColor = true;
            this.buttonExit.Click += new System.EventHandler(this.buttonExit_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::User_interface.Properties.Resources.cz_flag;
            this.pictureBox1.Location = new System.Drawing.Point(15, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(100, 50);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 3;
            this.pictureBox1.TabStop = false;
            // 
            // linkContinue
            // 
            this.linkContinue.AutoSize = true;
            this.linkContinue.Location = new System.Drawing.Point(131, 134);
            this.linkContinue.Name = "linkContinue";
            this.linkContinue.Size = new System.Drawing.Size(62, 13);
            this.linkContinue.TabIndex = 4;
            this.linkContinue.TabStop = true;
            this.linkContinue.Text = "Pokračovat";
            this.linkContinue.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkContinue_LinkClicked);
            // 
            // linkDifferentUser
            // 
            this.linkDifferentUser.AutoSize = true;
            this.linkDifferentUser.Location = new System.Drawing.Point(12, 278);
            this.linkDifferentUser.Name = "linkDifferentUser";
            this.linkDifferentUser.Size = new System.Drawing.Size(82, 13);
            this.linkDifferentUser.TabIndex = 5;
            this.linkDifferentUser.TabStop = true;
            this.linkDifferentUser.Text = "Jsem někdo jiný";
            this.linkDifferentUser.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkDifferentUser_LinkClicked);
            // 
            // WelcomePage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(344, 310);
            this.Controls.Add(this.linkDifferentUser);
            this.Controls.Add(this.linkContinue);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.buttonExit);
            this.Controls.Add(this.labelWelcome);
            this.Controls.Add(this.linkLanguageSettings);
            this.Name = "WelcomePage";
            this.Text = "WelcomePage";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.LinkLabel linkLanguageSettings;
        private System.Windows.Forms.Label labelWelcome;
        private System.Windows.Forms.Button buttonExit;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.LinkLabel linkContinue;
        private System.Windows.Forms.LinkLabel linkDifferentUser;
    }
}