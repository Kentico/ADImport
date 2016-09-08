namespace ADImport
{
    partial class Step4
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
            this.lblImportUsers = new ADImport.LocalizedLabel();
            this.radAllUsers = new ADImport.LocalizedRadioButton();
            this.chkUpdateUserRoleData = new ADImport.LocalizedCheckBox();
            this.lblSelectSites = new ADImport.LocalizedLabel();
            this.radOnlySelectedUsers = new ADImport.LocalizedRadioButton();
            this.radOnlySelectedAndNewUsers = new ADImport.LocalizedRadioButton();
            this.radAllGroups = new ADImport.LocalizedRadioButton();
            this.radOnlySelectedGroups = new ADImport.LocalizedRadioButton();
            this.radOnlySelectedAndNewGroups = new ADImport.LocalizedRadioButton();
            this.chkUpdateMemberships = new ADImport.LocalizedCheckBox();
            this.chkDeleteOldObjects = new ADImport.LocalizedCheckBox();
            this.chkCreateLog = new ADImport.LocalizedCheckBox();
            this.lblImportGroups = new ADImport.LocalizedLabel();
            this.pnlImportUsers = new System.Windows.Forms.Panel();
            this.pnlOptions = new System.Windows.Forms.Panel();
            this.chkImportOnlyUsersFromSelectedRoles = new ADImport.LocalizedCheckBox();
            this.chkImportNewUsersOnlyFromSelectedRoles = new ADImport.LocalizedCheckBox();
            this.pnlSaveLogFile = new System.Windows.Forms.Panel();
            this.lblMessage = new ADImport.LocalizedLabel();
            this.btnBrowse = new ADImport.LocalizedButton();
            this.lblPath = new ADImport.LocalizedLabel();
            this.txtLogFile = new System.Windows.Forms.TextBox();
            this.grdSites = new System.Windows.Forms.DataGridView();
            this.pnlImportGroups = new System.Windows.Forms.Panel();
            this.saveLogFile = new System.Windows.Forms.SaveFileDialog();
            this.toolTipOnlySelectedAndNew = new System.Windows.Forms.ToolTip(this.components);
            this.toolTipImportOnlyUsersFromSelectedRoles = new System.Windows.Forms.ToolTip(this.components);
            this.pnlSites = new System.Windows.Forms.Panel();
            this.pnlRadios = new System.Windows.Forms.Panel();
            this.pnlAllOptions = new System.Windows.Forms.Panel();
            this.pnlImportUsers.SuspendLayout();
            this.pnlOptions.SuspendLayout();
            this.pnlSaveLogFile.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdSites)).BeginInit();
            this.pnlImportGroups.SuspendLayout();
            this.pnlSites.SuspendLayout();
            this.pnlRadios.SuspendLayout();
            this.pnlAllOptions.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblImportUsers
            // 
            this.lblImportUsers.AutoSize = true;
            this.lblImportUsers.Location = new System.Drawing.Point(13, 9);
            this.lblImportUsers.Name = "lblImportUsers";
            this.lblImportUsers.ResourceString = "Step4_ImportUsers";
            this.lblImportUsers.Size = new System.Drawing.Size(67, 13);
            this.lblImportUsers.TabIndex = 0;
            this.lblImportUsers.Text = "Import users:";
            // 
            // radAllUsers
            // 
            this.radAllUsers.AutoSize = true;
            this.radAllUsers.Location = new System.Drawing.Point(3, 3);
            this.radAllUsers.Name = "radAllUsers";
            this.radAllUsers.ResourceString = "Step4_AllUsers";
            this.radAllUsers.Size = new System.Drawing.Size(64, 17);
            this.radAllUsers.TabIndex = 2;
            this.radAllUsers.TabStop = true;
            this.radAllUsers.Text = "All users";
            this.radAllUsers.UseVisualStyleBackColor = true;
            this.radAllUsers.CheckedChanged += new System.EventHandler(this.radAllUsers_CheckedChanged);
            // 
            // chkUpdateUserRoleData
            // 
            this.chkUpdateUserRoleData.AutoSize = true;
            this.chkUpdateUserRoleData.Location = new System.Drawing.Point(86, 52);
            this.chkUpdateUserRoleData.Name = "chkUpdateUserRoleData";
            this.chkUpdateUserRoleData.ResourceString = "Step4_UpdateData";
            this.chkUpdateUserRoleData.Size = new System.Drawing.Size(209, 17);
            this.chkUpdateUserRoleData.TabIndex = 10;
            this.chkUpdateUserRoleData.Text = "Update data of existing users and roles";
            this.chkUpdateUserRoleData.UseVisualStyleBackColor = true;
            this.chkUpdateUserRoleData.CheckedChanged += new System.EventHandler(this.chkUpdateUserRoleData_CheckedChanged);
            // 
            // lblSelectSites
            // 
            this.lblSelectSites.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblSelectSites.Location = new System.Drawing.Point(0, 0);
            this.lblSelectSites.Margin = new System.Windows.Forms.Padding(0);
            this.lblSelectSites.Name = "lblSelectSites";
            this.lblSelectSites.Padding = new System.Windows.Forms.Padding(15, 0, 0, 0);
            this.lblSelectSites.ResourceString = "Step4_SelectSites";
            this.lblSelectSites.Size = new System.Drawing.Size(80, 72);
            this.lblSelectSites.TabIndex = 0;
            this.lblSelectSites.Text = "Select sites:";
            // 
            // radOnlySelectedUsers
            // 
            this.radOnlySelectedUsers.AutoSize = true;
            this.radOnlySelectedUsers.Location = new System.Drawing.Point(80, 3);
            this.radOnlySelectedUsers.Name = "radOnlySelectedUsers";
            this.radOnlySelectedUsers.ResourceString = "Step4_OnlySelected";
            this.radOnlySelectedUsers.Size = new System.Drawing.Size(89, 17);
            this.radOnlySelectedUsers.TabIndex = 3;
            this.radOnlySelectedUsers.TabStop = true;
            this.radOnlySelectedUsers.Text = "Only selected";
            this.radOnlySelectedUsers.UseVisualStyleBackColor = true;
            this.radOnlySelectedUsers.CheckedChanged += new System.EventHandler(this.radOnlySelectedUsers_CheckedChanged);
            // 
            // radOnlySelectedAndNewUsers
            // 
            this.radOnlySelectedAndNewUsers.AutoSize = true;
            this.radOnlySelectedAndNewUsers.Location = new System.Drawing.Point(175, 3);
            this.radOnlySelectedAndNewUsers.Name = "radOnlySelectedAndNewUsers";
            this.radOnlySelectedAndNewUsers.ResourceString = "Step4_UpdateSelectedImportNewUsers";
            this.radOnlySelectedAndNewUsers.Size = new System.Drawing.Size(219, 17);
            this.radOnlySelectedAndNewUsers.TabIndex = 4;
            this.radOnlySelectedAndNewUsers.TabStop = true;
            this.radOnlySelectedAndNewUsers.Text = "Update selected and import all new users";
            this.radOnlySelectedAndNewUsers.UseVisualStyleBackColor = true;
            this.radOnlySelectedAndNewUsers.CheckedChanged += new System.EventHandler(this.radOnlySelectedAndNewUsers_CheckedChanged);
            // 
            // radAllGroups
            // 
            this.radAllGroups.AutoSize = true;
            this.radAllGroups.Location = new System.Drawing.Point(3, 2);
            this.radAllGroups.Name = "radAllGroups";
            this.radAllGroups.ResourceString = "Step4_AllGroups";
            this.radAllGroups.Size = new System.Drawing.Size(71, 17);
            this.radAllGroups.TabIndex = 6;
            this.radAllGroups.TabStop = true;
            this.radAllGroups.Text = "All groups";
            this.radAllGroups.UseVisualStyleBackColor = true;
            this.radAllGroups.CheckedChanged += new System.EventHandler(this.radAllGroups_CheckedChanged);
            // 
            // radOnlySelectedGroups
            // 
            this.radOnlySelectedGroups.AutoSize = true;
            this.radOnlySelectedGroups.Location = new System.Drawing.Point(80, 2);
            this.radOnlySelectedGroups.Name = "radOnlySelectedGroups";
            this.radOnlySelectedGroups.ResourceString = "Step4_OnlySelected";
            this.radOnlySelectedGroups.Size = new System.Drawing.Size(89, 17);
            this.radOnlySelectedGroups.TabIndex = 7;
            this.radOnlySelectedGroups.TabStop = true;
            this.radOnlySelectedGroups.Text = "Only selected";
            this.radOnlySelectedGroups.UseVisualStyleBackColor = true;
            this.radOnlySelectedGroups.CheckedChanged += new System.EventHandler(this.radOnlySelectedGroups_CheckedChanged);
            // 
            // radOnlySelectedAndNewGroups
            // 
            this.radOnlySelectedAndNewGroups.AutoSize = true;
            this.radOnlySelectedAndNewGroups.Location = new System.Drawing.Point(175, 2);
            this.radOnlySelectedAndNewGroups.Name = "radOnlySelectedAndNewGroups";
            this.radOnlySelectedAndNewGroups.ResourceString = "Step4_UpdateSelectedImportNewGroups";
            this.radOnlySelectedAndNewGroups.Size = new System.Drawing.Size(226, 17);
            this.radOnlySelectedAndNewGroups.TabIndex = 8;
            this.radOnlySelectedAndNewGroups.TabStop = true;
            this.radOnlySelectedAndNewGroups.Text = "Update selected and import all new groups";
            this.radOnlySelectedAndNewGroups.UseVisualStyleBackColor = true;
            this.radOnlySelectedAndNewGroups.CheckedChanged += new System.EventHandler(this.radOnlySelectedAndNewGroups_CheckedChanged);
            // 
            // chkUpdateMemberships
            // 
            this.chkUpdateMemberships.AutoSize = true;
            this.chkUpdateMemberships.Location = new System.Drawing.Point(86, 98);
            this.chkUpdateMemberships.Name = "chkUpdateMemberships";
            this.chkUpdateMemberships.ResourceString = "Step4_UpdateMemberships";
            this.chkUpdateMemberships.Size = new System.Drawing.Size(122, 17);
            this.chkUpdateMemberships.TabIndex = 11;
            this.chkUpdateMemberships.Text = "Assign users to roles";
            this.chkUpdateMemberships.UseVisualStyleBackColor = true;
            this.chkUpdateMemberships.CheckedChanged += new System.EventHandler(this.chkUpdateMemberships_CheckedChanged);
            // 
            // chkDeleteOldObjects
            // 
            this.chkDeleteOldObjects.AutoSize = true;
            this.chkDeleteOldObjects.Location = new System.Drawing.Point(86, 75);
            this.chkDeleteOldObjects.Name = "chkDeleteOldObjects";
            this.chkDeleteOldObjects.ResourceString = "Step4_DeleteOldObjects";
            this.chkDeleteOldObjects.Size = new System.Drawing.Size(316, 17);
            this.chkDeleteOldObjects.TabIndex = 13;
            this.chkDeleteOldObjects.Text = "Delete users and roles that do not exist in the Active Directory";
            this.chkDeleteOldObjects.UseVisualStyleBackColor = true;
            this.chkDeleteOldObjects.CheckedChanged += new System.EventHandler(this.chkDeleteOldObjects_CheckedChanged);
            // 
            // chkCreateLog
            // 
            this.chkCreateLog.AutoSize = true;
            this.chkCreateLog.Location = new System.Drawing.Point(86, 124);
            this.chkCreateLog.Name = "chkCreateLog";
            this.chkCreateLog.ResourceString = "Step4_CreateLog";
            this.chkCreateLog.Size = new System.Drawing.Size(143, 17);
            this.chkCreateLog.TabIndex = 14;
            this.chkCreateLog.Text = "Log import process to file";
            this.chkCreateLog.UseVisualStyleBackColor = true;
            this.chkCreateLog.CheckedChanged += new System.EventHandler(this.chkCreateLog_CheckedChanged);
            // 
            // lblImportGroups
            // 
            this.lblImportGroups.AutoSize = true;
            this.lblImportGroups.Location = new System.Drawing.Point(6, 34);
            this.lblImportGroups.Name = "lblImportGroups";
            this.lblImportGroups.ResourceString = "Step4_ImportGroups";
            this.lblImportGroups.Size = new System.Drawing.Size(74, 13);
            this.lblImportGroups.TabIndex = 0;
            this.lblImportGroups.Text = "Import groups:";
            // 
            // pnlImportUsers
            // 
            this.pnlImportUsers.Controls.Add(this.radAllUsers);
            this.pnlImportUsers.Controls.Add(this.radOnlySelectedUsers);
            this.pnlImportUsers.Controls.Add(this.radOnlySelectedAndNewUsers);
            this.pnlImportUsers.Location = new System.Drawing.Point(83, 4);
            this.pnlImportUsers.Name = "pnlImportUsers";
            this.pnlImportUsers.Size = new System.Drawing.Size(403, 22);
            this.pnlImportUsers.TabIndex = 1;
            // 
            // pnlOptions
            // 
            this.pnlOptions.Controls.Add(this.chkImportOnlyUsersFromSelectedRoles);
            this.pnlOptions.Controls.Add(this.chkImportNewUsersOnlyFromSelectedRoles);
            this.pnlOptions.Controls.Add(this.pnlSaveLogFile);
            this.pnlOptions.Controls.Add(this.chkUpdateMemberships);
            this.pnlOptions.Controls.Add(this.chkUpdateUserRoleData);
            this.pnlOptions.Controls.Add(this.chkDeleteOldObjects);
            this.pnlOptions.Controls.Add(this.chkCreateLog);
            this.pnlOptions.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlOptions.Location = new System.Drawing.Point(0, 57);
            this.pnlOptions.Name = "pnlOptions";
            this.pnlOptions.Size = new System.Drawing.Size(584, 174);
            this.pnlOptions.TabIndex = 9;
            // 
            // chkImportOnlyUsersFromSelectedRoles
            // 
            this.chkImportOnlyUsersFromSelectedRoles.AutoSize = true;
            this.chkImportOnlyUsersFromSelectedRoles.Location = new System.Drawing.Point(86, 29);
            this.chkImportOnlyUsersFromSelectedRoles.Name = "chkImportOnlyUsersFromSelectedRoles";
            this.chkImportOnlyUsersFromSelectedRoles.ResourceString = "Step4_ImportAllUsersOnlyFromSelectedRoles";
            this.chkImportOnlyUsersFromSelectedRoles.Size = new System.Drawing.Size(305, 17);
            this.chkImportOnlyUsersFromSelectedRoles.TabIndex = 16;
            this.chkImportOnlyUsersFromSelectedRoles.Text = "Import all users from selected groups and ignore other users";
            this.chkImportOnlyUsersFromSelectedRoles.UseVisualStyleBackColor = true;
            this.chkImportOnlyUsersFromSelectedRoles.CheckedChanged += new System.EventHandler(this.chkImportOnlyUsersFromSelectedRoles_CheckedChanged);
            // 
            // chkImportNewUsersOnlyFromSelectedRoles
            // 
            this.chkImportNewUsersOnlyFromSelectedRoles.AutoSize = true;
            this.chkImportNewUsersOnlyFromSelectedRoles.Location = new System.Drawing.Point(86, 6);
            this.chkImportNewUsersOnlyFromSelectedRoles.Name = "chkImportNewUsersOnlyFromSelectedRoles";
            this.chkImportNewUsersOnlyFromSelectedRoles.ResourceString = "Step4_ImportNewUsersOnlyFromSelectedRoles";
            this.chkImportNewUsersOnlyFromSelectedRoles.Size = new System.Drawing.Size(229, 17);
            this.chkImportNewUsersOnlyFromSelectedRoles.TabIndex = 12;
            this.chkImportNewUsersOnlyFromSelectedRoles.Text = "Import new users only from selected groups";
            this.chkImportNewUsersOnlyFromSelectedRoles.UseVisualStyleBackColor = true;
            this.chkImportNewUsersOnlyFromSelectedRoles.CheckedChanged += new System.EventHandler(this.chkImportNewUsersOnlyFromSelectedRoles_CheckedChanged);
            // 
            // pnlSaveLogFile
            // 
            this.pnlSaveLogFile.Controls.Add(this.lblMessage);
            this.pnlSaveLogFile.Controls.Add(this.btnBrowse);
            this.pnlSaveLogFile.Controls.Add(this.lblPath);
            this.pnlSaveLogFile.Controls.Add(this.txtLogFile);
            this.pnlSaveLogFile.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlSaveLogFile.Location = new System.Drawing.Point(0, 141);
            this.pnlSaveLogFile.Name = "pnlSaveLogFile";
            this.pnlSaveLogFile.Size = new System.Drawing.Size(584, 33);
            this.pnlSaveLogFile.TabIndex = 15;
            // 
            // lblMessage
            // 
            this.lblMessage.AutoSize = true;
            this.lblMessage.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblMessage.Location = new System.Drawing.Point(373, 9);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.ResourceString = null;
            this.lblMessage.Size = new System.Drawing.Size(50, 13);
            this.lblMessage.TabIndex = 6;
            this.lblMessage.Text = "Message";
            this.lblMessage.Visible = false;
            // 
            // btnBrowse
            // 
            this.btnBrowse.Location = new System.Drawing.Point(292, 4);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.ResourceString = "General_Browse";
            this.btnBrowse.Size = new System.Drawing.Size(75, 23);
            this.btnBrowse.TabIndex = 17;
            this.btnBrowse.Text = "Browse";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // lblPath
            // 
            this.lblPath.AutoSize = true;
            this.lblPath.Location = new System.Drawing.Point(47, 9);
            this.lblPath.Name = "lblPath";
            this.lblPath.ResourceString = "General_PathColon";
            this.lblPath.Size = new System.Drawing.Size(32, 13);
            this.lblPath.TabIndex = 5;
            this.lblPath.Text = "Path:";
            // 
            // txtLogFile
            // 
            this.txtLogFile.Location = new System.Drawing.Point(86, 6);
            this.txtLogFile.Name = "txtLogFile";
            this.txtLogFile.Size = new System.Drawing.Size(200, 20);
            this.txtLogFile.TabIndex = 16;
            // 
            // grdSites
            // 
            this.grdSites.AllowUserToAddRows = false;
            this.grdSites.AllowUserToDeleteRows = false;
            this.grdSites.AllowUserToResizeRows = false;
            this.grdSites.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdSites.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdSites.Location = new System.Drawing.Point(80, 0);
            this.grdSites.MultiSelect = false;
            this.grdSites.Name = "grdSites";
            this.grdSites.RowHeadersVisible = false;
            this.grdSites.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grdSites.Size = new System.Drawing.Size(504, 72);
            this.grdSites.TabIndex = 18;
            this.grdSites.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.grdSites_CellValueChanged);
            this.grdSites.CurrentCellDirtyStateChanged += new System.EventHandler(this.grdSites_CurrentCellDirtyStateChanged);
            // 
            // pnlImportGroups
            // 
            this.pnlImportGroups.Controls.Add(this.radAllGroups);
            this.pnlImportGroups.Controls.Add(this.radOnlySelectedAndNewGroups);
            this.pnlImportGroups.Controls.Add(this.radOnlySelectedGroups);
            this.pnlImportGroups.Location = new System.Drawing.Point(83, 30);
            this.pnlImportGroups.Name = "pnlImportGroups";
            this.pnlImportGroups.Size = new System.Drawing.Size(403, 22);
            this.pnlImportGroups.TabIndex = 5;
            // 
            // pnlSites
            // 
            this.pnlSites.Controls.Add(this.grdSites);
            this.pnlSites.Controls.Add(this.lblSelectSites);
            this.pnlSites.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlSites.Location = new System.Drawing.Point(5, 236);
            this.pnlSites.Name = "pnlSites";
            this.pnlSites.Size = new System.Drawing.Size(584, 72);
            this.pnlSites.TabIndex = 19;
            // 
            // pnlRadios
            // 
            this.pnlRadios.Controls.Add(this.lblImportUsers);
            this.pnlRadios.Controls.Add(this.lblImportGroups);
            this.pnlRadios.Controls.Add(this.pnlImportGroups);
            this.pnlRadios.Controls.Add(this.pnlImportUsers);
            this.pnlRadios.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlRadios.Location = new System.Drawing.Point(0, 0);
            this.pnlRadios.Name = "pnlRadios";
            this.pnlRadios.Size = new System.Drawing.Size(584, 57);
            this.pnlRadios.TabIndex = 20;
            // 
            // pnlAllOptions
            // 
            this.pnlAllOptions.Controls.Add(this.pnlOptions);
            this.pnlAllOptions.Controls.Add(this.pnlRadios);
            this.pnlAllOptions.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlAllOptions.Location = new System.Drawing.Point(5, 5);
            this.pnlAllOptions.Name = "pnlAllOptions";
            this.pnlAllOptions.Size = new System.Drawing.Size(584, 231);
            this.pnlAllOptions.TabIndex = 21;
            // 
            // Step4
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pnlSites);
            this.Controls.Add(this.pnlAllOptions);
            this.Name = "Step4";
            this.Padding = new System.Windows.Forms.Padding(5);
            this.Load += new System.EventHandler(this.Step4_Load);
            this.ParentChanged += new System.EventHandler(this.Step4_ParentChanged);
            this.pnlImportUsers.ResumeLayout(false);
            this.pnlImportUsers.PerformLayout();
            this.pnlOptions.ResumeLayout(false);
            this.pnlOptions.PerformLayout();
            this.pnlSaveLogFile.ResumeLayout(false);
            this.pnlSaveLogFile.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdSites)).EndInit();
            this.pnlImportGroups.ResumeLayout(false);
            this.pnlImportGroups.PerformLayout();
            this.pnlSites.ResumeLayout(false);
            this.pnlRadios.ResumeLayout(false);
            this.pnlRadios.PerformLayout();
            this.pnlAllOptions.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private LocalizedLabel lblImportUsers;
        private LocalizedRadioButton radAllUsers;
        private LocalizedCheckBox chkUpdateUserRoleData;
        private LocalizedLabel lblSelectSites;
        private LocalizedRadioButton radOnlySelectedUsers;
        private LocalizedRadioButton radOnlySelectedAndNewUsers;
        private LocalizedRadioButton radAllGroups;
        private LocalizedRadioButton radOnlySelectedGroups;
        private LocalizedRadioButton radOnlySelectedAndNewGroups;
        private LocalizedCheckBox chkUpdateMemberships;
        private LocalizedCheckBox chkDeleteOldObjects;
        private LocalizedCheckBox chkCreateLog;
        private LocalizedLabel lblImportGroups;
        private System.Windows.Forms.Panel pnlImportUsers;
        private System.Windows.Forms.Panel pnlOptions;
        private System.Windows.Forms.DataGridView grdSites;
        private System.Windows.Forms.Panel pnlImportGroups;
        private System.Windows.Forms.Panel pnlSaveLogFile;
        private LocalizedButton btnBrowse;
        private LocalizedLabel lblPath;
        private System.Windows.Forms.TextBox txtLogFile;
        private System.Windows.Forms.SaveFileDialog saveLogFile;
        private LocalizedLabel lblMessage;
        private System.Windows.Forms.ToolTip toolTipOnlySelectedAndNew;
        private LocalizedCheckBox chkImportNewUsersOnlyFromSelectedRoles;
        private System.Windows.Forms.ToolTip toolTipImportOnlyUsersFromSelectedRoles;
        private System.Windows.Forms.Panel pnlSites;
        private System.Windows.Forms.Panel pnlRadios;
        private System.Windows.Forms.Panel pnlAllOptions;
        private LocalizedCheckBox chkImportOnlyUsersFromSelectedRoles;
    }
}
