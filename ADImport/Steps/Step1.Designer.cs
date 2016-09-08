namespace ADImport
{
    partial class Step1
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.radNewImportProfile = new LocalizedRadioButton();
            this.radExistingImportProfile = new LocalizedRadioButton();
            this.openImportProfile = new System.Windows.Forms.OpenFileDialog();
            this.btnBrowse = new LocalizedButton();
            this.txtImportProfile = new System.Windows.Forms.TextBox();
            this.lblMessage = new ADImport.LocalizedLabel();
            this.lblPath = new ADImport.LocalizedLabel();
            this.pnlOpenProfile = new System.Windows.Forms.Panel();
            this.pnlOpenProfile.SuspendLayout();
            this.SuspendLayout();
            // 
            // radNewImportProfile
            // 
            this.radNewImportProfile.AutoSize = true;
            this.radNewImportProfile.Checked = true;
            this.radNewImportProfile.Location = new System.Drawing.Point(14, 11);
            this.radNewImportProfile.Name = "radNewImportProfile";
            this.radNewImportProfile.Size = new System.Drawing.Size(141, 17);
            this.radNewImportProfile.TabIndex = 0;
            this.radNewImportProfile.TabStop = true;
            this.radNewImportProfile.ResourceString = "Step1_NewImportProfile";
            this.radNewImportProfile.UseVisualStyleBackColor = true;
            this.radNewImportProfile.CheckedChanged += new System.EventHandler(this.radNewImportProfile_CheckedChanged);
            // 
            // radExistingImportProfile
            // 
            this.radExistingImportProfile.AutoSize = true;
            this.radExistingImportProfile.Location = new System.Drawing.Point(14, 34);
            this.radExistingImportProfile.Name = "radExistingImportProfile";
            this.radExistingImportProfile.Size = new System.Drawing.Size(159, 17);
            this.radExistingImportProfile.TabIndex = 0;
            this.radExistingImportProfile.ResourceString = "Step1_ExistingImportProfile";
            this.radExistingImportProfile.UseVisualStyleBackColor = true;
            this.radExistingImportProfile.CheckedChanged += new System.EventHandler(this.radExistingImportProfile_CheckedChanged);
            // 
            // openImportProfile
            // 
            this.openImportProfile.RestoreDirectory = true;
            // 
            // btnBrowse
            // 
            this.btnBrowse.Location = new System.Drawing.Point(199, 3);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(75, 23);
            this.btnBrowse.TabIndex = 3;
            this.btnBrowse.ResourceString = "General_Browse";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // txtImportProfile
            // 
            this.txtImportProfile.Location = new System.Drawing.Point(43, 6);
            this.txtImportProfile.Name = "txtImportProfile";
            this.txtImportProfile.Size = new System.Drawing.Size(150, 20);
            this.txtImportProfile.TabIndex = 2;
            // 
            // lblMessage
            // 
            this.lblMessage.AutoSize = true;
            this.lblMessage.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblMessage.Location = new System.Drawing.Point(30, 91);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.ResourceString = null;
            this.lblMessage.Size = new System.Drawing.Size(50, 13);
            this.lblMessage.TabIndex = 4;
            this.lblMessage.Text = "Message";
            this.lblMessage.Visible = false;
            // 
            // lblPath
            // 
            this.lblPath.AutoSize = true;
            this.lblPath.Location = new System.Drawing.Point(5, 9);
            this.lblPath.Name = "lblPath";
            this.lblPath.ResourceString = "General_PathColon";
            this.lblPath.Size = new System.Drawing.Size(32, 13);
            this.lblPath.TabIndex = 5;
            // 
            // pnlOpenProfile
            // 
            this.pnlOpenProfile.Controls.Add(this.btnBrowse);
            this.pnlOpenProfile.Controls.Add(this.lblPath);
            this.pnlOpenProfile.Controls.Add(this.txtImportProfile);
            this.pnlOpenProfile.Location = new System.Drawing.Point(25, 57);
            this.pnlOpenProfile.Name = "pnlOpenProfile";
            this.pnlOpenProfile.Size = new System.Drawing.Size(278, 31);
            this.pnlOpenProfile.TabIndex = 1;
            // 
            // Step1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.radNewImportProfile);
            this.Controls.Add(this.pnlOpenProfile);
            this.Controls.Add(this.radExistingImportProfile);
            this.Controls.Add(this.lblMessage);
            this.Name = "Step1";
            this.pnlOpenProfile.ResumeLayout(false);
            this.pnlOpenProfile.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private LocalizedRadioButton radNewImportProfile;
        private LocalizedRadioButton radExistingImportProfile;
        private System.Windows.Forms.OpenFileDialog openImportProfile;
        private LocalizedButton btnBrowse;
        private System.Windows.Forms.TextBox txtImportProfile;
        private LocalizedLabel lblMessage;
        private LocalizedLabel lblPath;
        private System.Windows.Forms.Panel pnlOpenProfile;
    }
}
