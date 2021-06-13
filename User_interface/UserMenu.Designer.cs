namespace User_interface
{
    partial class UserMenu
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
            this.buttonLogout = new System.Windows.Forms.Button();
            this.buttonExit = new System.Windows.Forms.Button();
            this.labelUser = new System.Windows.Forms.Label();
            this.Menu = new System.Windows.Forms.MenuStrip();
            this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem5 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem6 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem7 = new System.Windows.Forms.ToolStripMenuItem();
            this.lessonLink = new System.Windows.Forms.LinkLabel();
            this.label1 = new System.Windows.Forms.Label();
            this.buttonShowPlan = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.Menu.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonLogout
            // 
            this.buttonLogout.Location = new System.Drawing.Point(77, 312);
            this.buttonLogout.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.buttonLogout.Name = "buttonLogout";
            this.buttonLogout.Size = new System.Drawing.Size(100, 35);
            this.buttonLogout.TabIndex = 0;
            this.buttonLogout.Text = "Odhlásit";
            this.buttonLogout.UseVisualStyleBackColor = true;
            this.buttonLogout.Click += new System.EventHandler(this.buttonLogout_Click);
            // 
            // buttonExit
            // 
            this.buttonExit.Location = new System.Drawing.Point(185, 312);
            this.buttonExit.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.buttonExit.Name = "buttonExit";
            this.buttonExit.Size = new System.Drawing.Size(100, 35);
            this.buttonExit.TabIndex = 1;
            this.buttonExit.Text = "Ukončit";
            this.buttonExit.UseVisualStyleBackColor = true;
            this.buttonExit.Click += new System.EventHandler(this.buttonExit_Click);
            // 
            // labelUser
            // 
            this.labelUser.AutoSize = true;
            this.labelUser.Location = new System.Drawing.Point(38, 31);
            this.labelUser.Name = "labelUser";
            this.labelUser.Size = new System.Drawing.Size(153, 20);
            this.labelUser.TabIndex = 2;
            this.labelUser.Text = "Uživatel: <username>";
            // 
            // Menu
            // 
            this.Menu.Dock = System.Windows.Forms.DockStyle.None;
            this.Menu.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.Menu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem4});
            this.Menu.Location = new System.Drawing.Point(259, 31);
            this.Menu.Name = "Menu";
            this.Menu.Size = new System.Drawing.Size(68, 28);
            this.Menu.TabIndex = 4;
            this.Menu.Text = "menuStrip1";
            // 
            // toolStripMenuItem4
            // 
            this.toolStripMenuItem4.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem5,
            this.toolStripMenuItem6,
            this.toolStripMenuItem7});
            this.toolStripMenuItem4.Name = "toolStripMenuItem4";
            this.toolStripMenuItem4.Size = new System.Drawing.Size(60, 24);
            this.toolStripMenuItem4.Text = "Menu";
            // 
            // toolStripMenuItem5
            // 
            this.toolStripMenuItem5.Name = "toolStripMenuItem5";
            this.toolStripMenuItem5.Size = new System.Drawing.Size(143, 26);
            this.toolStripMenuItem5.Text = "test";
            // 
            // toolStripMenuItem6
            // 
            this.toolStripMenuItem6.Name = "toolStripMenuItem6";
            this.toolStripMenuItem6.Size = new System.Drawing.Size(143, 26);
            this.toolStripMenuItem6.Text = "settings";
            // 
            // toolStripMenuItem7
            // 
            this.toolStripMenuItem7.Name = "toolStripMenuItem7";
            this.toolStripMenuItem7.Size = new System.Drawing.Size(143, 26);
            this.toolStripMenuItem7.Text = "logout";
            // 
            // lessonLink
            // 
            this.lessonLink.AutoSize = true;
            this.lessonLink.Location = new System.Drawing.Point(77, 134);
            this.lessonLink.Name = "lessonLink";
            this.lessonLink.Size = new System.Drawing.Size(70, 20);
            this.lessonLink.TabIndex = 5;
            this.lessonLink.TabStop = true;
            this.lessonLink.Text = "<lesson>";
            this.lessonLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lessonLink_LinkClicked);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(77, 93);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(80, 20);
            this.label1.TabIndex = 6;
            this.label1.Text = "Další lekce";
            // 
            // buttonShowPlan
            // 
            this.buttonShowPlan.Location = new System.Drawing.Point(199, 93);
            this.buttonShowPlan.Name = "buttonShowPlan";
            this.buttonShowPlan.Size = new System.Drawing.Size(99, 61);
            this.buttonShowPlan.TabIndex = 7;
            this.buttonShowPlan.Text = "Ukázat plán";
            this.buttonShowPlan.UseVisualStyleBackColor = true;
            this.buttonShowPlan.Click += new System.EventHandler(this.buttonShowPlan_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(94, 217);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(132, 20);
            this.label2.TabIndex = 8;
            this.label2.Text = "TODO: panel kurzu";
            // 
            // UserMenu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(379, 402);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.buttonShowPlan);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lessonLink);
            this.Controls.Add(this.Menu);
            this.Controls.Add(this.labelUser);
            this.Controls.Add(this.buttonExit);
            this.Controls.Add(this.buttonLogout);
            this.MainMenuStrip = this.Menu;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "UserMenu";
            this.Text = "UserMenu";
            this.Menu.ResumeLayout(false);
            this.Menu.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonLogout;
        private System.Windows.Forms.Button buttonExit;
        private System.Windows.Forms.Label labelUser;
        private System.Windows.Forms.MenuStrip Menu;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem4;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem5;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem6;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem7;
        private System.Windows.Forms.LinkLabel lessonLink;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button buttonShowPlan;
        private System.Windows.Forms.Label label2;
    }
}