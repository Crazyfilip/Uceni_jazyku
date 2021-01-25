
namespace User_interface
{
    partial class PlanView
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
            this.listBoxPlannedLessons = new System.Windows.Forms.ListBox();
            this.buttonPlanNext = new System.Windows.Forms.Button();
            this.buttonStartSelected = new System.Windows.Forms.Button();
            this.labelPlannedLessons = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // listBoxPlannedLessons
            // 
            this.listBoxPlannedLessons.FormattingEnabled = true;
            this.listBoxPlannedLessons.ItemHeight = 20;
            this.listBoxPlannedLessons.Location = new System.Drawing.Point(56, 43);
            this.listBoxPlannedLessons.Name = "listBoxPlannedLessons";
            this.listBoxPlannedLessons.Size = new System.Drawing.Size(153, 244);
            this.listBoxPlannedLessons.TabIndex = 0;
            this.listBoxPlannedLessons.SelectedValueChanged += new System.EventHandler(this.listBoxPlannedLessons_SelectedValueChanged);
            // 
            // buttonPlanNext
            // 
            this.buttonPlanNext.Location = new System.Drawing.Point(255, 96);
            this.buttonPlanNext.Name = "buttonPlanNext";
            this.buttonPlanNext.Size = new System.Drawing.Size(153, 29);
            this.buttonPlanNext.TabIndex = 1;
            this.buttonPlanNext.Text = "Get next lesson";
            this.buttonPlanNext.UseVisualStyleBackColor = true;
            this.buttonPlanNext.Click += new System.EventHandler(this.buttonPlanNext_Click);
            // 
            // buttonStartSelected
            // 
            this.buttonStartSelected.Enabled = false;
            this.buttonStartSelected.Location = new System.Drawing.Point(255, 168);
            this.buttonStartSelected.Name = "buttonStartSelected";
            this.buttonStartSelected.Size = new System.Drawing.Size(153, 29);
            this.buttonStartSelected.TabIndex = 2;
            this.buttonStartSelected.Text = "Do selected lesson";
            this.buttonStartSelected.UseVisualStyleBackColor = true;
            // 
            // labelPlannedLessons
            // 
            this.labelPlannedLessons.AutoSize = true;
            this.labelPlannedLessons.Location = new System.Drawing.Point(56, 13);
            this.labelPlannedLessons.Name = "labelPlannedLessons";
            this.labelPlannedLessons.Size = new System.Drawing.Size(157, 20);
            this.labelPlannedLessons.TabIndex = 3;
            this.labelPlannedLessons.Text = "TODO Planned lessons";
            // 
            // PlanView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(445, 333);
            this.Controls.Add(this.labelPlannedLessons);
            this.Controls.Add(this.buttonStartSelected);
            this.Controls.Add(this.buttonPlanNext);
            this.Controls.Add(this.listBoxPlannedLessons);
            this.Name = "PlanView";
            this.Text = "PlanView";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox listBoxPlannedLessons;
        private System.Windows.Forms.Button buttonPlanNext;
        private System.Windows.Forms.Button buttonStartSelected;
        private System.Windows.Forms.Label labelPlannedLessons;
    }
}