namespace ADImport
{
    partial class Step5
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.grpUsers = new ADImport.LocalizedGroupBox();
            this.grdUserProperties = new System.Windows.Forms.DataGridView();
            this.txtTarget = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cmbSource = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.pnlUserFormat = new System.Windows.Forms.Panel();
            this.chkAllAttributes = new ADImport.LocalizedCheckBox();
            this.lblCMSEditor = new ADImport.LocalizedLabel();
            this.cmbUsernameFromat = new System.Windows.Forms.ComboBox();
            this.lblUsernameFormat = new ADImport.LocalizedLabel();
            this.chkCMSEditor = new ADImport.LocalizedCheckBox();
            this.grpRoles = new ADImport.LocalizedGroupBox();
            this.lblImportRoleDescription = new ADImport.LocalizedLabel();
            this.cmbRoleCodeNameFormat = new System.Windows.Forms.ComboBox();
            this.cmbRoleDisplayNameFormat = new System.Windows.Forms.ComboBox();
            this.lblRoleCodeNameFormat = new ADImport.LocalizedLabel();
            this.chkImportRoleDescription = new ADImport.LocalizedCheckBox();
            this.lblRoleDisplayNameFormat = new ADImport.LocalizedLabel();
            this.toolTipSimple = new System.Windows.Forms.ToolTip(this.components);
            this.toolTipAdvanced = new System.Windows.Forms.ToolTip(this.components);
            this.grpUsers.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdUserProperties)).BeginInit();
            this.pnlUserFormat.SuspendLayout();
            this.grpRoles.SuspendLayout();
            this.SuspendLayout();
            // 
            // grpUsers
            // 
            this.grpUsers.AutoSize = true;
            this.grpUsers.Controls.Add(this.grdUserProperties);
            this.grpUsers.Controls.Add(this.pnlUserFormat);
            this.grpUsers.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpUsers.Location = new System.Drawing.Point(5, 5);
            this.grpUsers.Name = "grpUsers";
            this.grpUsers.ResourceString = "General_Users";
            this.grpUsers.Size = new System.Drawing.Size(584, 198);
            this.grpUsers.TabIndex = 0;
            this.grpUsers.TabStop = false;
            this.grpUsers.Text = "Users";
            // 
            // grdUserProperties
            // 
            this.grdUserProperties.AllowUserToAddRows = false;
            this.grdUserProperties.AllowUserToDeleteRows = false;
            this.grdUserProperties.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.grdUserProperties.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.grdUserProperties.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdUserProperties.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.txtTarget,
            this.cmbSource});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.grdUserProperties.DefaultCellStyle = dataGridViewCellStyle2;
            this.grdUserProperties.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdUserProperties.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.grdUserProperties.Location = new System.Drawing.Point(3, 69);
            this.grdUserProperties.MultiSelect = false;
            this.grdUserProperties.Name = "grdUserProperties";
            this.grdUserProperties.RowHeadersVisible = false;
            this.grdUserProperties.Size = new System.Drawing.Size(578, 126);
            this.grdUserProperties.TabIndex = 8;
            this.grdUserProperties.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.grdUserProperties_CellValueChanged);
            this.grdUserProperties.EditingControlShowing += new System.Windows.Forms.DataGridViewEditingControlShowingEventHandler(this.grdUserProperties_EditingControlShowing);
            // 
            // txtTarget
            // 
            this.txtTarget.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.txtTarget.HeaderText = "Target";
            this.txtTarget.Name = "txtTarget";
            // 
            // cmbSource
            // 
            this.cmbSource.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.cmbSource.HeaderText = "Source";
            this.cmbSource.Name = "cmbSource";
            // 
            // pnlUserFormat
            // 
            this.pnlUserFormat.AutoSize = true;
            this.pnlUserFormat.Controls.Add(this.chkAllAttributes);
            this.pnlUserFormat.Controls.Add(this.lblCMSEditor);
            this.pnlUserFormat.Controls.Add(this.cmbUsernameFromat);
            this.pnlUserFormat.Controls.Add(this.lblUsernameFormat);
            this.pnlUserFormat.Controls.Add(this.chkCMSEditor);
            this.pnlUserFormat.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlUserFormat.Location = new System.Drawing.Point(3, 16);
            this.pnlUserFormat.Name = "pnlUserFormat";
            this.pnlUserFormat.Padding = new System.Windows.Forms.Padding(3);
            this.pnlUserFormat.Size = new System.Drawing.Size(578, 53);
            this.pnlUserFormat.TabIndex = 0;
            // 
            // chkAllAttributes
            // 
            this.chkAllAttributes.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.chkAllAttributes.AutoSize = true;
            this.chkAllAttributes.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkAllAttributes.Location = new System.Drawing.Point(460, 30);
            this.chkAllAttributes.Name = "chkAllAttributes";
            this.chkAllAttributes.ResourceString = "Step5_ShowAllAttrs";
            this.chkAllAttributes.Size = new System.Drawing.Size(112, 17);
            this.chkAllAttributes.TabIndex = 8;
            this.chkAllAttributes.Text = "Show all attributes";
            this.chkAllAttributes.UseVisualStyleBackColor = true;
            this.chkAllAttributes.CheckedChanged += new System.EventHandler(this.chkAllAttributes_CheckedChanged);
            // 
            // lblCMSEditor
            // 
            this.lblCMSEditor.AutoSize = true;
            this.lblCMSEditor.Location = new System.Drawing.Point(9, 31);
            this.lblCMSEditor.Name = "lblCMSEditor";
            this.lblCMSEditor.ResourceString = "Step5_UserCMSEditor";
            this.lblCMSEditor.Size = new System.Drawing.Size(193, 13);
            this.lblCMSEditor.TabIndex = 3;
            this.lblCMSEditor.Text = "Configure new users as Kentico editors:";
            // 
            // cmbUsernameFromat
            // 
            this.cmbUsernameFromat.DisplayMember = "DisplayMember";
            this.cmbUsernameFromat.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbUsernameFromat.FormattingEnabled = true;
            this.cmbUsernameFromat.Location = new System.Drawing.Point(208, 4);
            this.cmbUsernameFromat.Name = "cmbUsernameFromat";
            this.cmbUsernameFromat.Size = new System.Drawing.Size(200, 21);
            this.cmbUsernameFromat.TabIndex = 2;
            this.cmbUsernameFromat.ValueMember = "ValueMember";
            this.cmbUsernameFromat.SelectedIndexChanged += new System.EventHandler(this.cmbUsernameFromat_SelectedIndexChanged);
            // 
            // lblUsernameFormat
            // 
            this.lblUsernameFormat.AutoSize = true;
            this.lblUsernameFormat.Location = new System.Drawing.Point(109, 7);
            this.lblUsernameFormat.Name = "lblUsernameFormat";
            this.lblUsernameFormat.ResourceString = "Step5_UsernameFormat";
            this.lblUsernameFormat.Size = new System.Drawing.Size(93, 13);
            this.lblUsernameFormat.TabIndex = 1;
            this.lblUsernameFormat.Text = "User name format:";
            // 
            // chkCMSEditor
            // 
            this.chkCMSEditor.AutoSize = true;
            this.chkCMSEditor.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkCMSEditor.Location = new System.Drawing.Point(208, 31);
            this.chkCMSEditor.Name = "chkCMSEditor";
            this.chkCMSEditor.ResourceString = null;
            this.chkCMSEditor.Size = new System.Drawing.Size(15, 14);
            this.chkCMSEditor.TabIndex = 4;
            this.chkCMSEditor.UseVisualStyleBackColor = true;
            this.chkCMSEditor.CheckedChanged += new System.EventHandler(this.chkCMSEditor_CheckedChanged);
            // 
            // grpRoles
            // 
            this.grpRoles.AutoSize = true;
            this.grpRoles.Controls.Add(this.lblImportRoleDescription);
            this.grpRoles.Controls.Add(this.cmbRoleCodeNameFormat);
            this.grpRoles.Controls.Add(this.cmbRoleDisplayNameFormat);
            this.grpRoles.Controls.Add(this.lblRoleCodeNameFormat);
            this.grpRoles.Controls.Add(this.chkImportRoleDescription);
            this.grpRoles.Controls.Add(this.lblRoleDisplayNameFormat);
            this.grpRoles.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.grpRoles.Location = new System.Drawing.Point(5, 203);
            this.grpRoles.Name = "grpRoles";
            this.grpRoles.ResourceString = "General_Roles";
            this.grpRoles.Size = new System.Drawing.Size(584, 105);
            this.grpRoles.TabIndex = 6;
            this.grpRoles.TabStop = false;
            this.grpRoles.Text = "Roles";
            // 
            // lblImportRoleDescription
            // 
            this.lblImportRoleDescription.AutoSize = true;
            this.lblImportRoleDescription.Location = new System.Drawing.Point(99, 72);
            this.lblImportRoleDescription.Name = "lblImportRoleDescription";
            this.lblImportRoleDescription.ResourceString = "Step5_ImportRoleDesc";
            this.lblImportRoleDescription.Size = new System.Drawing.Size(93, 13);
            this.lblImportRoleDescription.TabIndex = 13;
            this.lblImportRoleDescription.Text = "Import description:";
            // 
            // cmbRoleCodeNameFormat
            // 
            this.cmbRoleCodeNameFormat.DisplayMember = "DisplayMember";
            this.cmbRoleCodeNameFormat.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbRoleCodeNameFormat.FormattingEnabled = true;
            this.cmbRoleCodeNameFormat.Location = new System.Drawing.Point(198, 45);
            this.cmbRoleCodeNameFormat.Name = "cmbRoleCodeNameFormat";
            this.cmbRoleCodeNameFormat.Size = new System.Drawing.Size(200, 21);
            this.cmbRoleCodeNameFormat.TabIndex = 12;
            this.cmbRoleCodeNameFormat.ValueMember = "ValueMember";
            this.cmbRoleCodeNameFormat.SelectedIndexChanged += new System.EventHandler(this.cmbRoleCodeNameFormat_SelectedIndexChanged);
            // 
            // cmbRoleDisplayNameFormat
            // 
            this.cmbRoleDisplayNameFormat.DisplayMember = "DisplayMember";
            this.cmbRoleDisplayNameFormat.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbRoleDisplayNameFormat.FormattingEnabled = true;
            this.cmbRoleDisplayNameFormat.Location = new System.Drawing.Point(198, 18);
            this.cmbRoleDisplayNameFormat.Name = "cmbRoleDisplayNameFormat";
            this.cmbRoleDisplayNameFormat.Size = new System.Drawing.Size(200, 21);
            this.cmbRoleDisplayNameFormat.TabIndex = 10;
            this.cmbRoleDisplayNameFormat.ValueMember = "ValueMember";
            this.cmbRoleDisplayNameFormat.SelectedIndexChanged += new System.EventHandler(this.cmbRoleDisplayNameFormat_SelectedIndexChanged);
            // 
            // lblRoleCodeNameFormat
            // 
            this.lblRoleCodeNameFormat.AutoSize = true;
            this.lblRoleCodeNameFormat.Location = new System.Drawing.Point(72, 48);
            this.lblRoleCodeNameFormat.Name = "lblRoleCodeNameFormat";
            this.lblRoleCodeNameFormat.ResourceString = "Step5_RoleCodeNameFormat";
            this.lblRoleCodeNameFormat.Size = new System.Drawing.Size(120, 13);
            this.lblRoleCodeNameFormat.TabIndex = 11;
            this.lblRoleCodeNameFormat.Text = "Role code name format:";
            // 
            // chkImportRoleDescription
            // 
            this.chkImportRoleDescription.AutoSize = true;
            this.chkImportRoleDescription.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkImportRoleDescription.Location = new System.Drawing.Point(198, 72);
            this.chkImportRoleDescription.Name = "chkImportRoleDescription";
            this.chkImportRoleDescription.ResourceString = null;
            this.chkImportRoleDescription.Size = new System.Drawing.Size(15, 14);
            this.chkImportRoleDescription.TabIndex = 14;
            this.chkImportRoleDescription.UseVisualStyleBackColor = true;
            this.chkImportRoleDescription.CheckedChanged += new System.EventHandler(this.chkImportRoleDescription_CheckedChanged);
            // 
            // lblRoleDisplayNameFormat
            // 
            this.lblRoleDisplayNameFormat.AutoSize = true;
            this.lblRoleDisplayNameFormat.Location = new System.Drawing.Point(64, 21);
            this.lblRoleDisplayNameFormat.Name = "lblRoleDisplayNameFormat";
            this.lblRoleDisplayNameFormat.ResourceString = "Step5_RoleDisplayNameFormat";
            this.lblRoleDisplayNameFormat.Size = new System.Drawing.Size(128, 13);
            this.lblRoleDisplayNameFormat.TabIndex = 9;
            this.lblRoleDisplayNameFormat.Text = "Role display name format:";
            // 
            // Step5
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.grpUsers);
            this.Controls.Add(this.grpRoles);
            this.Name = "Step5";
            this.Padding = new System.Windows.Forms.Padding(5);
            this.Load += new System.EventHandler(this.Step5_Load);
            this.ParentChanged += new System.EventHandler(this.Step5_ParentChanged);
            this.grpUsers.ResumeLayout(false);
            this.grpUsers.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdUserProperties)).EndInit();
            this.pnlUserFormat.ResumeLayout(false);
            this.pnlUserFormat.PerformLayout();
            this.grpRoles.ResumeLayout(false);
            this.grpRoles.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private LocalizedGroupBox grpUsers;
        private LocalizedGroupBox grpRoles;
        private System.Windows.Forms.ComboBox cmbUsernameFromat;
        private LocalizedCheckBox chkCMSEditor;
        private LocalizedLabel lblUsernameFormat;
        private System.Windows.Forms.DataGridView grdUserProperties;
        private System.Windows.Forms.ComboBox cmbRoleDisplayNameFormat;
        private LocalizedCheckBox chkImportRoleDescription;
        private LocalizedLabel lblRoleDisplayNameFormat;
        private System.Windows.Forms.DataGridViewTextBoxColumn txtTarget;
        private System.Windows.Forms.DataGridViewComboBoxColumn cmbSource;
        private System.Windows.Forms.Panel pnlUserFormat;
        private LocalizedLabel lblCMSEditor;
        private LocalizedLabel lblImportRoleDescription;
        private System.Windows.Forms.ComboBox cmbRoleCodeNameFormat;
        private LocalizedLabel lblRoleCodeNameFormat;
        private System.Windows.Forms.ToolTip toolTipSimple;
        private System.Windows.Forms.ToolTip toolTipAdvanced;
        private LocalizedCheckBox chkAllAttributes;
    }
}
