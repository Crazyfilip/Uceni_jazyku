namespace User_interface
{
    partial class NewAccount1
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
            this.labelName = new System.Windows.Forms.Label();
            this.textboxName = new System.Windows.Forms.TextBox();
            this.labelPassword = new System.Windows.Forms.Label();
            this.textboxPassword = new System.Windows.Forms.TextBox();
            this.labelNativeLanguage = new System.Windows.Forms.Label();
            this.comboboxNatureLanguage = new System.Windows.Forms.ComboBox();
            this.buttonContinue = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // labelName
            // 
            this.labelName.AutoSize = true;
            this.labelName.Location = new System.Drawing.Point(24, 35);
            this.labelName.Name = "labelName";
            this.labelName.Size = new System.Drawing.Size(38, 13);
            this.labelName.TabIndex = 0;
            this.labelName.Text = "Jméno";
            // 
            // textboxName
            // 
            this.textboxName.Location = new System.Drawing.Point(84, 32);
            this.textboxName.Name = "textboxName";
            this.textboxName.Size = new System.Drawing.Size(121, 20);
            this.textboxName.TabIndex = 1;
            // 
            // labelPassword
            // 
            this.labelPassword.AutoSize = true;
            this.labelPassword.Location = new System.Drawing.Point(28, 63);
            this.labelPassword.Name = "labelPassword";
            this.labelPassword.Size = new System.Drawing.Size(34, 13);
            this.labelPassword.TabIndex = 2;
            this.labelPassword.Text = "Heslo";
            // 
            // textboxPassword
            // 
            this.textboxPassword.Location = new System.Drawing.Point(84, 60);
            this.textboxPassword.Name = "textboxPassword";
            this.textboxPassword.Size = new System.Drawing.Size(121, 20);
            this.textboxPassword.TabIndex = 3;
            // 
            // labelNativeLanguage
            // 
            this.labelNativeLanguage.AutoSize = true;
            this.labelNativeLanguage.Location = new System.Drawing.Point(12, 96);
            this.labelNativeLanguage.Name = "labelNativeLanguage";
            this.labelNativeLanguage.Size = new System.Drawing.Size(65, 13);
            this.labelNativeLanguage.TabIndex = 4;
            this.labelNativeLanguage.Text = "Rodný jazyk";
            // 
            // comboboxNatureLanguage
            // 
            this.comboboxNatureLanguage.FormattingEnabled = true;
            this.comboboxNatureLanguage.Items.AddRange(new object[] {
            "Čeština"});
            this.comboboxNatureLanguage.Location = new System.Drawing.Point(84, 96);
            this.comboboxNatureLanguage.Name = "comboboxNatureLanguage";
            this.comboboxNatureLanguage.Size = new System.Drawing.Size(121, 21);
            this.comboboxNatureLanguage.TabIndex = 5;
            this.comboboxNatureLanguage.Text = "Čeština";
            // 
            // buttonContinue
            // 
            this.buttonContinue.Location = new System.Drawing.Point(106, 190);
            this.buttonContinue.Name = "buttonContinue";
            this.buttonContinue.Size = new System.Drawing.Size(99, 23);
            this.buttonContinue.TabIndex = 6;
            this.buttonContinue.Text = "Tvorba účtu 1/2";
            this.buttonContinue.UseVisualStyleBackColor = true;
            this.buttonContinue.Click += new System.EventHandler(this.buttonContinue_Click);
            // 
            // NewAccount1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(234, 234);
            this.Controls.Add(this.buttonContinue);
            this.Controls.Add(this.comboboxNatureLanguage);
            this.Controls.Add(this.labelNativeLanguage);
            this.Controls.Add(this.textboxPassword);
            this.Controls.Add(this.labelPassword);
            this.Controls.Add(this.textboxName);
            this.Controls.Add(this.labelName);
            this.Name = "NewAccount1";
            this.Text = "NewAccount1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelName;
        private System.Windows.Forms.TextBox textboxName;
        private System.Windows.Forms.Label labelPassword;
        private System.Windows.Forms.TextBox textboxPassword;
        private System.Windows.Forms.Label labelNativeLanguage;
        private System.Windows.Forms.ComboBox comboboxNatureLanguage;
        private System.Windows.Forms.Button buttonContinue;
    }
}