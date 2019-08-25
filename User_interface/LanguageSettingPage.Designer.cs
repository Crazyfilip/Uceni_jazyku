namespace User_interface
{
    partial class LanguageSettingPage
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
            this.groupboxAvailableLanguages = new System.Windows.Forms.GroupBox();
            this.panelLanguages = new System.Windows.Forms.Panel();
            this.buttonChangeLanguage = new System.Windows.Forms.Button();
            this.buttonBack = new System.Windows.Forms.Button();
            this.groupboxAvailableLanguages.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupboxAvailableLanguages
            // 
            this.groupboxAvailableLanguages.Controls.Add(this.panelLanguages);
            this.groupboxAvailableLanguages.Location = new System.Drawing.Point(47, 12);
            this.groupboxAvailableLanguages.Name = "groupboxAvailableLanguages";
            this.groupboxAvailableLanguages.Size = new System.Drawing.Size(200, 197);
            this.groupboxAvailableLanguages.TabIndex = 0;
            this.groupboxAvailableLanguages.TabStop = false;
            this.groupboxAvailableLanguages.Text = "Dostupné jazyky";
            // 
            // panelLanguages
            // 
            this.panelLanguages.AutoScroll = true;
            this.panelLanguages.Location = new System.Drawing.Point(0, 19);
            this.panelLanguages.Name = "panelLanguages";
            this.panelLanguages.Size = new System.Drawing.Size(200, 178);
            this.panelLanguages.TabIndex = 0;
            // 
            // buttonChangeLanguage
            // 
            this.buttonChangeLanguage.Location = new System.Drawing.Point(76, 227);
            this.buttonChangeLanguage.Name = "buttonChangeLanguage";
            this.buttonChangeLanguage.Size = new System.Drawing.Size(128, 23);
            this.buttonChangeLanguage.TabIndex = 1;
            this.buttonChangeLanguage.Text = "Změnit jazyk";
            this.buttonChangeLanguage.UseVisualStyleBackColor = true;
            // 
            // buttonBack
            // 
            this.buttonBack.Location = new System.Drawing.Point(97, 260);
            this.buttonBack.Name = "buttonBack";
            this.buttonBack.Size = new System.Drawing.Size(96, 23);
            this.buttonBack.TabIndex = 2;
            this.buttonBack.Text = "Zpět";
            this.buttonBack.UseVisualStyleBackColor = true;
            this.buttonBack.Click += new System.EventHandler(this.buttonBack_Click);
            // 
            // LanguageSettingPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(301, 311);
            this.Controls.Add(this.buttonBack);
            this.Controls.Add(this.buttonChangeLanguage);
            this.Controls.Add(this.groupboxAvailableLanguages);
            this.Name = "LanguageSettingPage";
            this.Text = "LanguageSettingPage";
            this.groupboxAvailableLanguages.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupboxAvailableLanguages;
        private System.Windows.Forms.Panel panelLanguages;
        private System.Windows.Forms.Button buttonChangeLanguage;
        private System.Windows.Forms.Button buttonBack;
    }
}