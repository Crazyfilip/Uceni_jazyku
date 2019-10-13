namespace User_interface
{
    partial class UserSettingsFinishedWindow
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
            this.label1 = new System.Windows.Forms.Label();
            this.buttonMenu = new System.Windows.Forms.Button();
            this.buttonEntryMenu = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(34, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(286, 26);
            this.label1.TabIndex = 0;
            this.label1.Text = "Nastavení kurzů uloženo. Nyní můžete udělat vstupní test,\r\n pokud máte již nějaké" +
    " znalosti ze zvolených kurzů.";
            // 
            // buttonMenu
            // 
            this.buttonMenu.DialogResult = System.Windows.Forms.DialogResult.No;
            this.buttonMenu.Location = new System.Drawing.Point(37, 117);
            this.buttonMenu.Name = "buttonMenu";
            this.buttonMenu.Size = new System.Drawing.Size(107, 23);
            this.buttonMenu.TabIndex = 1;
            this.buttonMenu.Text = "Menu";
            this.buttonMenu.UseVisualStyleBackColor = true;
            this.buttonMenu.Click += new System.EventHandler(this.buttonMenu_Click);
            // 
            // buttonEntryMenu
            // 
            this.buttonEntryMenu.DialogResult = System.Windows.Forms.DialogResult.Yes;
            this.buttonEntryMenu.Enabled = false;
            this.buttonEntryMenu.Location = new System.Drawing.Point(167, 117);
            this.buttonEntryMenu.Name = "buttonEntryMenu";
            this.buttonEntryMenu.Size = new System.Drawing.Size(128, 22);
            this.buttonEntryMenu.TabIndex = 2;
            this.buttonEntryMenu.Text = "Vstupní test";
            this.buttonEntryMenu.UseVisualStyleBackColor = true;
            this.buttonEntryMenu.Click += new System.EventHandler(this.button2_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(37, 69);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(138, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Chcete udělat vstupní test?";
            // 
            // UserSettingsFinishedWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(342, 182);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.buttonEntryMenu);
            this.Controls.Add(this.buttonMenu);
            this.Controls.Add(this.label1);
            this.Name = "UserSettingsFinishedWindow";
            this.Text = "UserSettingsFinishedWindow";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button buttonMenu;
        private System.Windows.Forms.Button buttonEntryMenu;
        private System.Windows.Forms.Label label2;
    }
}