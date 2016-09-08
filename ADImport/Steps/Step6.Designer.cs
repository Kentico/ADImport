namespace ADImport
{
    partial class Step6
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
            this.treeGroups = new System.Windows.Forms.TreeView();
            this.btnSelectAllGroups = new ADImport.LocalizedButton();
            this.btnDeselectAllGroups = new ADImport.LocalizedButton();
            this.lblUsersInGroup = new ADImport.LocalizedLabel();
            this.grdUsers = new System.Windows.Forms.DataGridView();
            this.btnSelectAllUsers = new ADImport.LocalizedButton();
            this.btnDeselectAllUsers = new ADImport.LocalizedButton();
            this.pnlButtons = new System.Windows.Forms.Panel();
            this.cntBody = new System.Windows.Forms.SplitContainer();
            this.lblGroups = new ADImport.LocalizedLabel();
            ((System.ComponentModel.ISupportInitialize)(this.grdUsers)).BeginInit();
            this.pnlButtons.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cntBody)).BeginInit();
            this.cntBody.Panel1.SuspendLayout();
            this.cntBody.Panel2.SuspendLayout();
            this.cntBody.SuspendLayout();
            this.SuspendLayout();
            // 
            // treeGroups
            // 
            this.treeGroups.CheckBoxes = true;
            this.treeGroups.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeGroups.Location = new System.Drawing.Point(0, 13);
            this.treeGroups.Name = "treeGroups";
            this.treeGroups.Size = new System.Drawing.Size(292, 266);
            this.treeGroups.TabIndex = 0;
            this.treeGroups.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.treeGroups_AfterCheck);
            this.treeGroups.BeforeExpand += new System.Windows.Forms.TreeViewCancelEventHandler(this.treeGroups_BeforeExpand);
            this.treeGroups.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeGroups_AfterSelect);
            // 
            // btnSelectAllGroups
            // 
            this.btnSelectAllGroups.Enabled = false;
            this.btnSelectAllGroups.Location = new System.Drawing.Point(0, 3);
            this.btnSelectAllGroups.Name = "btnSelectAllGroups";
            this.btnSelectAllGroups.ResourceString = "General_SelectAll";
            this.btnSelectAllGroups.Size = new System.Drawing.Size(75, 23);
            this.btnSelectAllGroups.TabIndex = 1;
            this.btnSelectAllGroups.Text = "Select all";
            this.btnSelectAllGroups.UseVisualStyleBackColor = true;
            this.btnSelectAllGroups.Click += new System.EventHandler(this.btnSelectAllGroups_Click);
            // 
            // btnDeselectAllGroups
            // 
            this.btnDeselectAllGroups.Enabled = false;
            this.btnDeselectAllGroups.Location = new System.Drawing.Point(81, 3);
            this.btnDeselectAllGroups.Name = "btnDeselectAllGroups";
            this.btnDeselectAllGroups.ResourceString = "General_DeselectAll";
            this.btnDeselectAllGroups.Size = new System.Drawing.Size(75, 23);
            this.btnDeselectAllGroups.TabIndex = 2;
            this.btnDeselectAllGroups.Text = "Deselect all";
            this.btnDeselectAllGroups.UseVisualStyleBackColor = true;
            this.btnDeselectAllGroups.Click += new System.EventHandler(this.btnDeselectAllGroups_Click);
            // 
            // lblUsersInGroup
            // 
            this.lblUsersInGroup.AutoSize = true;
            this.lblUsersInGroup.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblUsersInGroup.Location = new System.Drawing.Point(0, 0);
            this.lblUsersInGroup.Name = "lblUsersInGroup";
            this.lblUsersInGroup.ResourceString = "Step6_UsersInGroup";
            this.lblUsersInGroup.Size = new System.Drawing.Size(138, 13);
            this.lblUsersInGroup.TabIndex = 2;
            this.lblUsersInGroup.Text = "Select users to be imported:";
            // 
            // grdUsers
            // 
            this.grdUsers.AllowUserToAddRows = false;
            this.grdUsers.AllowUserToDeleteRows = false;
            this.grdUsers.AllowUserToResizeRows = false;
            this.grdUsers.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdUsers.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdUsers.Location = new System.Drawing.Point(0, 13);
            this.grdUsers.MultiSelect = false;
            this.grdUsers.Name = "grdUsers";
            this.grdUsers.RowHeadersVisible = false;
            this.grdUsers.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grdUsers.Size = new System.Drawing.Size(288, 266);
            this.grdUsers.TabIndex = 3;
            this.grdUsers.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.grdUsers_CellValueChanged);
            this.grdUsers.CurrentCellDirtyStateChanged += new System.EventHandler(this.grdUsers_CurrentCellDirtyStateChanged);
            // 
            // btnSelectAllUsers
            // 
            this.btnSelectAllUsers.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSelectAllUsers.Enabled = false;
            this.btnSelectAllUsers.Location = new System.Drawing.Point(428, 3);
            this.btnSelectAllUsers.Name = "btnSelectAllUsers";
            this.btnSelectAllUsers.ResourceString = "General_SelectAll";
            this.btnSelectAllUsers.Size = new System.Drawing.Size(75, 23);
            this.btnSelectAllUsers.TabIndex = 4;
            this.btnSelectAllUsers.Text = "Select all";
            this.btnSelectAllUsers.UseVisualStyleBackColor = true;
            this.btnSelectAllUsers.Click += new System.EventHandler(this.btnSelectAllUsers_Click);
            // 
            // btnDeselectAllUsers
            // 
            this.btnDeselectAllUsers.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDeselectAllUsers.Enabled = false;
            this.btnDeselectAllUsers.Location = new System.Drawing.Point(509, 3);
            this.btnDeselectAllUsers.Name = "btnDeselectAllUsers";
            this.btnDeselectAllUsers.ResourceString = "General_DeselectAll";
            this.btnDeselectAllUsers.Size = new System.Drawing.Size(75, 23);
            this.btnDeselectAllUsers.TabIndex = 5;
            this.btnDeselectAllUsers.Text = "Deselect all";
            this.btnDeselectAllUsers.UseVisualStyleBackColor = true;
            this.btnDeselectAllUsers.Click += new System.EventHandler(this.btnDeselectAllUsers_Click);
            // 
            // pnlButtons
            // 
            this.pnlButtons.Controls.Add(this.btnSelectAllUsers);
            this.pnlButtons.Controls.Add(this.btnDeselectAllUsers);
            this.pnlButtons.Controls.Add(this.btnDeselectAllGroups);
            this.pnlButtons.Controls.Add(this.btnSelectAllGroups);
            this.pnlButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlButtons.Location = new System.Drawing.Point(5, 284);
            this.pnlButtons.Name = "pnlButtons";
            this.pnlButtons.Size = new System.Drawing.Size(584, 29);
            this.pnlButtons.TabIndex = 4;
            // 
            // cntBody
            // 
            this.cntBody.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cntBody.Location = new System.Drawing.Point(5, 5);
            this.cntBody.Name = "cntBody";
            // 
            // cntBody.Panel1
            // 
            this.cntBody.Panel1.Controls.Add(this.treeGroups);
            this.cntBody.Panel1.Controls.Add(this.lblGroups);
            // 
            // cntBody.Panel2
            // 
            this.cntBody.Panel2.Controls.Add(this.grdUsers);
            this.cntBody.Panel2.Controls.Add(this.lblUsersInGroup);
            this.cntBody.Size = new System.Drawing.Size(584, 279);
            this.cntBody.SplitterDistance = 292;
            this.cntBody.TabIndex = 6;
            // 
            // lblGroups
            // 
            this.lblGroups.AutoSize = true;
            this.lblGroups.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblGroups.Location = new System.Drawing.Point(0, 0);
            this.lblGroups.Name = "lblGroups";
            this.lblGroups.ResourceString = "Step6_SelectGroups";
            this.lblGroups.Size = new System.Drawing.Size(145, 13);
            this.lblGroups.TabIndex = 1;
            this.lblGroups.Text = "Select groups to be imported:";
            // 
            // Step6
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.cntBody);
            this.Controls.Add(this.pnlButtons);
            this.Name = "Step6";
            this.Padding = new System.Windows.Forms.Padding(5, 5, 5, 0);
            this.Load += new System.EventHandler(this.Step6_Load);
            this.ParentChanged += new System.EventHandler(this.Step6_ParentChanged);
            ((System.ComponentModel.ISupportInitialize)(this.grdUsers)).EndInit();
            this.pnlButtons.ResumeLayout(false);
            this.cntBody.Panel1.ResumeLayout(false);
            this.cntBody.Panel1.PerformLayout();
            this.cntBody.Panel2.ResumeLayout(false);
            this.cntBody.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cntBody)).EndInit();
            this.cntBody.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TreeView treeGroups;
        private LocalizedButton btnSelectAllGroups;
        private LocalizedButton btnDeselectAllGroups;
        private LocalizedLabel lblUsersInGroup;
        private System.Windows.Forms.DataGridView grdUsers;
        private LocalizedButton btnSelectAllUsers;
        private LocalizedButton btnDeselectAllUsers;
        private System.Windows.Forms.Panel pnlButtons;
        private System.Windows.Forms.SplitContainer cntBody;
        private LocalizedLabel lblGroups;
    }
}
