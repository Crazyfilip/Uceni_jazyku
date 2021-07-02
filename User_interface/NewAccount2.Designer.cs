namespace User_interface
{
    partial class NewAccount2
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
            this.labelAddCourse = new System.Windows.Forms.Label();
            this.comboBoxCourses = new System.Windows.Forms.ComboBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.checkBoxVocabulary = new System.Windows.Forms.CheckBox();
            this.comboBoxCourseTypes = new System.Windows.Forms.ComboBox();
            this.labelCourseType = new System.Windows.Forms.Label();
            this.buttonFinish = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // labelAddCourse
            // 
            this.labelAddCourse.AutoSize = true;
            this.labelAddCourse.Location = new System.Drawing.Point(16, 48);
            this.labelAddCourse.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelAddCourse.Name = "labelAddCourse";
            this.labelAddCourse.Size = new System.Drawing.Size(79, 20);
            this.labelAddCourse.TabIndex = 1;
            this.labelAddCourse.Text = "Přidat kurz";
            // 
            // comboBoxCourses
            // 
            this.comboBoxCourses.FormattingEnabled = true;
            this.comboBoxCourses.Location = new System.Drawing.Point(101, 43);
            this.comboBoxCourses.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.comboBoxCourses.Name = "comboBoxCourses";
            this.comboBoxCourses.Size = new System.Drawing.Size(160, 28);
            this.comboBoxCourses.TabIndex = 2;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.checkBoxVocabulary);
            this.panel1.Controls.Add(this.comboBoxCourseTypes);
            this.panel1.Controls.Add(this.labelCourseType);
            this.panel1.Location = new System.Drawing.Point(20, 117);
            this.panel1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(412, 266);
            this.panel1.TabIndex = 3;
            // 
            // checkBoxVocabulary
            // 
            this.checkBoxVocabulary.AutoSize = true;
            this.checkBoxVocabulary.Location = new System.Drawing.Point(21, 80);
            this.checkBoxVocabulary.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.checkBoxVocabulary.Name = "checkBoxVocabulary";
            this.checkBoxVocabulary.Size = new System.Drawing.Size(85, 24);
            this.checkBoxVocabulary.TabIndex = 2;
            this.checkBoxVocabulary.Text = "Slovíčka";
            this.checkBoxVocabulary.UseVisualStyleBackColor = true;
            // 
            // comboBoxCourseTypes
            // 
            this.comboBoxCourseTypes.FormattingEnabled = true;
            this.comboBoxCourseTypes.Items.AddRange(new object[] {
            "Standardní",
            "Striktní",
            "Vícejazyčný"});
            this.comboBoxCourseTypes.Location = new System.Drawing.Point(99, 23);
            this.comboBoxCourseTypes.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.comboBoxCourseTypes.Name = "comboBoxCourseTypes";
            this.comboBoxCourseTypes.Size = new System.Drawing.Size(160, 28);
            this.comboBoxCourseTypes.TabIndex = 1;
            // 
            // labelCourseType
            // 
            this.labelCourseType.AutoSize = true;
            this.labelCourseType.Location = new System.Drawing.Point(17, 28);
            this.labelCourseType.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelCourseType.Name = "labelCourseType";
            this.labelCourseType.Size = new System.Drawing.Size(71, 20);
            this.labelCourseType.TabIndex = 0;
            this.labelCourseType.Text = "Typ kurzu";
            // 
            // buttonFinish
            // 
            this.buttonFinish.Location = new System.Drawing.Point(331, 409);
            this.buttonFinish.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.buttonFinish.Name = "buttonFinish";
            this.buttonFinish.Size = new System.Drawing.Size(100, 35);
            this.buttonFinish.TabIndex = 4;
            this.buttonFinish.Text = "Dokončit";
            this.buttonFinish.UseVisualStyleBackColor = true;
            this.buttonFinish.Click += new System.EventHandler(this.buttonFinish_Click);
            // 
            // NewAccount2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(559, 495);
            this.Controls.Add(this.buttonFinish);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.comboBoxCourses);
            this.Controls.Add(this.labelAddCourse);
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "NewAccount2";
            this.Text = "NewAccount2";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelAddCourse;
        private System.Windows.Forms.ComboBox comboBoxCourses;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.CheckBox checkBoxVocabulary;
        private System.Windows.Forms.ComboBox comboBoxCourseTypes;
        private System.Windows.Forms.Label labelCourseType;
        private System.Windows.Forms.Button buttonFinish;
    }
}