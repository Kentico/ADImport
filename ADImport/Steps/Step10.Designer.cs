namespace ADImport
{
    partial class Step10
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
            this.chkImportNow = new ADImport.LocalizedCheckBox();
            this.lblPath = new ADImport.LocalizedLabel();
            this.btnBrowse = new ADImport.LocalizedButton();
            this.txtPath = new System.Windows.Forms.TextBox();
            this.chkSaveImportProfile = new ADImport.LocalizedCheckBox();
            this.saveImportProfile = new System.Windows.Forms.SaveFileDialog();
            this.pnlSave = new System.Windows.Forms.Panel();
            this.lblMessage = new ADImport.LocalizedLabel();
            this.pnlStep10 = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.pnlSave.SuspendLayout();
            this.pnlStep10.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // chkImportNow
            // 
            this.chkImportNow.AutoSize = true;
            this.chkImportNow.Location = new System.Drawing.Point(3, 4);
            this.chkImportNow.Name = "chkImportNow";
            this.chkImportNow.ResourceString = "Step10_ImportNow";
            this.chkImportNow.Size = new System.Drawing.Size(78, 17);
            this.chkImportNow.TabIndex = 0;
            this.chkImportNow.Text = "Import now";
            this.chkImportNow.UseVisualStyleBackColor = true;
            this.chkImportNow.CheckedChanged += new System.EventHandler(this.chkImportNow_CheckedChanged);
            // 
            // lblPath
            // 
            this.lblPath.AutoSize = true;
            this.lblPath.Location = new System.Drawing.Point(5, 8);
            this.lblPath.Name = "lblPath";
            this.lblPath.ResourceString = "General_PathColon";
            this.lblPath.Size = new System.Drawing.Size(32, 13);
            this.lblPath.TabIndex = 1;
            this.lblPath.Text = "Path:";
            // 
            // btnBrowse
            // 
            this.btnBrowse.Location = new System.Drawing.Point(199, 3);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.ResourceString = "General_Browse";
            this.btnBrowse.Size = new System.Drawing.Size(75, 23);
            this.btnBrowse.TabIndex = 4;
            this.btnBrowse.Text = "Browse";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // txtPath
            // 
            this.txtPath.Location = new System.Drawing.Point(43, 5);
            this.txtPath.Name = "txtPath";
            this.txtPath.Size = new System.Drawing.Size(150, 20);
            this.txtPath.TabIndex = 3;
            // 
            // chkSaveImportProfile
            // 
            this.chkSaveImportProfile.AutoSize = true;
            this.chkSaveImportProfile.Location = new System.Drawing.Point(3, 27);
            this.chkSaveImportProfile.Name = "chkSaveImportProfile";
            this.chkSaveImportProfile.ResourceString = "Step10_SaveImportProfile";
            this.chkSaveImportProfile.Size = new System.Drawing.Size(141, 17);
            this.chkSaveImportProfile.TabIndex = 1;
            this.chkSaveImportProfile.Text = "Save import profile to file";
            this.chkSaveImportProfile.UseVisualStyleBackColor = true;
            this.chkSaveImportProfile.CheckedChanged += new System.EventHandler(this.chkSaveImportProfile_CheckedChanged);
            // 
            // saveImportProfile
            // 
            this.saveImportProfile.RestoreDirectory = true;
            // 
            // pnlSave
            // 
            this.pnlSave.Controls.Add(this.lblMessage);
            this.pnlSave.Controls.Add(this.btnBrowse);
            this.pnlSave.Controls.Add(this.txtPath);
            this.pnlSave.Controls.Add(this.lblPath);
            this.pnlSave.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlSave.Location = new System.Drawing.Point(0, 48);
            this.pnlSave.Name = "pnlSave";
            this.pnlSave.Size = new System.Drawing.Size(594, 265);
            this.pnlSave.TabIndex = 2;
            // 
            // lblMessage
            // 
            this.lblMessage.AutoSize = true;
            this.lblMessage.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblMessage.Location = new System.Drawing.Point(280, 8);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.ResourceString = null;
            this.lblMessage.Size = new System.Drawing.Size(50, 13);
            this.lblMessage.TabIndex = 7;
            this.lblMessage.Text = "Message";
            this.lblMessage.Visible = false;
            // 
            // pnlStep10
            // 
            this.pnlStep10.Controls.Add(this.pnlSave);
            this.pnlStep10.Controls.Add(this.panel1);
            this.pnlStep10.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlStep10.Location = new System.Drawing.Point(0, 0);
            this.pnlStep10.Name = "pnlStep10";
            this.pnlStep10.Size = new System.Drawing.Size(594, 313);
            this.pnlStep10.TabIndex = 5;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.chkSaveImportProfile);
            this.panel1.Controls.Add(this.chkImportNow);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(594, 48);
            this.panel1.TabIndex = 3;
            // 
            // Step10
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pnlStep10);
            this.Name = "Step10";
            this.ParentChanged += new System.EventHandler(this.Step10_ParentChanged);
            this.pnlSave.ResumeLayout(false);
            this.pnlSave.PerformLayout();
            this.pnlStep10.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private LocalizedCheckBox chkImportNow;
        private LocalizedLabel lblPath;
        private LocalizedButton btnBrowse;
        private System.Windows.Forms.TextBox txtPath;
        private LocalizedCheckBox chkSaveImportProfile;
        private System.Windows.Forms.SaveFileDialog saveImportProfile;
        private System.Windows.Forms.Panel pnlSave;
        private System.Windows.Forms.Panel pnlStep10;
        private LocalizedLabel lblMessage;
        private System.Windows.Forms.Panel panel1;
    }
}
