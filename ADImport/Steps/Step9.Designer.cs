namespace ADImport
{
    partial class Step9
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
            this.lblSite = new ADImport.LocalizedLabel();
            this.cmbSites = new System.Windows.Forms.ComboBox();
            this.lblRoles = new ADImport.LocalizedLabel();
            this.grdRoles = new System.Windows.Forms.DataGridView();
            this.pnlRoles = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblFilter = new ADImport.LocalizedLabel();
            this.txtFilter = new System.Windows.Forms.TextBox();
            this.btnFilter = new ADImport.LocalizedButton();
            ((System.ComponentModel.ISupportInitialize)(this.grdRoles)).BeginInit();
            this.pnlRoles.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblSite
            // 
            this.lblSite.AutoSize = true;
            this.lblSite.Location = new System.Drawing.Point(49, 6);
            this.lblSite.Name = "lblSite";
            this.lblSite.ResourceString = "Step9_Site";
            this.lblSite.Size = new System.Drawing.Size(28, 13);
            this.lblSite.TabIndex = 1;
            this.lblSite.Text = "Site:";
            // 
            // cmbSites
            // 
            this.cmbSites.DisplayMember = "SiteDisplayName";
            this.cmbSites.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSites.FormattingEnabled = true;
            this.cmbSites.Location = new System.Drawing.Point(82, 3);
            this.cmbSites.Name = "cmbSites";
            this.cmbSites.Size = new System.Drawing.Size(200, 21);
            this.cmbSites.TabIndex = 2;
            this.cmbSites.ValueMember = "SiteName";
            this.cmbSites.SelectedIndexChanged += new System.EventHandler(this.cmbSites_SelectedIndexChanged);
            // 
            // lblRoles
            // 
            this.lblRoles.AutoSize = true;
            this.lblRoles.Location = new System.Drawing.Point(12, 3);
            this.lblRoles.Name = "lblRoles";
            this.lblRoles.ResourceString = "Step9_SelectRoles";
            this.lblRoles.Size = new System.Drawing.Size(65, 13);
            this.lblRoles.TabIndex = 7;
            this.lblRoles.Text = "Select roles:";
            // 
            // grdRoles
            // 
            this.grdRoles.AllowUserToAddRows = false;
            this.grdRoles.AllowUserToDeleteRows = false;
            this.grdRoles.AllowUserToResizeRows = false;
            this.grdRoles.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdRoles.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdRoles.Location = new System.Drawing.Point(82, 27);
            this.grdRoles.Name = "grdRoles";
            this.grdRoles.RowHeadersVisible = false;
            this.grdRoles.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grdRoles.Size = new System.Drawing.Size(507, 281);
            this.grdRoles.TabIndex = 8;
            this.grdRoles.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.grdRoles_CellValueChanged);
            this.grdRoles.CurrentCellDirtyStateChanged += new System.EventHandler(this.grdRoles_CurrentCellDirtyStateChanged);
            // 
            // pnlRoles
            // 
            this.pnlRoles.Controls.Add(this.lblRoles);
            this.pnlRoles.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlRoles.Location = new System.Drawing.Point(0, 27);
            this.pnlRoles.Name = "pnlRoles";
            this.pnlRoles.Size = new System.Drawing.Size(82, 281);
            this.pnlRoles.TabIndex = 6;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.lblFilter);
            this.panel1.Controls.Add(this.txtFilter);
            this.panel1.Controls.Add(this.btnFilter);
            this.panel1.Controls.Add(this.cmbSites);
            this.panel1.Controls.Add(this.lblSite);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(589, 27);
            this.panel1.TabIndex = 0;
            // 
            // lblFilter
            // 
            this.lblFilter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblFilter.AutoSize = true;
            this.lblFilter.Location = new System.Drawing.Point(291, 6);
            this.lblFilter.Name = "lblFilter";
            this.lblFilter.ResourceString = "General_Rolename";
            this.lblFilter.Size = new System.Drawing.Size(61, 13);
            this.lblFilter.TabIndex = 3;
            this.lblFilter.Text = "Role name:";
            // 
            // txtFilter
            // 
            this.txtFilter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtFilter.Location = new System.Drawing.Point(358, 3);
            this.txtFilter.Name = "txtFilter";
            this.txtFilter.Size = new System.Drawing.Size(150, 20);
            this.txtFilter.TabIndex = 4;
            this.txtFilter.Enter += new System.EventHandler(this.txtFilter_Enter);
            this.txtFilter.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtFilter_KeyPress);
            this.txtFilter.Leave += new System.EventHandler(this.txtFilter_Leave);
            // 
            // btnFilter
            // 
            this.btnFilter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnFilter.Location = new System.Drawing.Point(514, 1);
            this.btnFilter.Name = "btnFilter";
            this.btnFilter.ResourceString = "General_Filter";
            this.btnFilter.Size = new System.Drawing.Size(75, 23);
            this.btnFilter.TabIndex = 5;
            this.btnFilter.Text = "Filter";
            this.btnFilter.UseVisualStyleBackColor = true;
            this.btnFilter.Click += new System.EventHandler(this.btnFilter_Click);
            // 
            // Step9
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.grdRoles);
            this.Controls.Add(this.pnlRoles);
            this.Controls.Add(this.panel1);
            this.Name = "Step9";
            this.Padding = new System.Windows.Forms.Padding(0, 0, 5, 5);
            this.ParentChanged += new System.EventHandler(this.Step9_ParentChanged);
            ((System.ComponentModel.ISupportInitialize)(this.grdRoles)).EndInit();
            this.pnlRoles.ResumeLayout(false);
            this.pnlRoles.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private LocalizedLabel lblSite;
        private System.Windows.Forms.ComboBox cmbSites;
        private LocalizedLabel lblRoles;
        private System.Windows.Forms.DataGridView grdRoles;
        private System.Windows.Forms.Panel pnlRoles;
        private System.Windows.Forms.Panel panel1;
        private LocalizedButton btnFilter;
        private System.Windows.Forms.TextBox txtFilter;
        private LocalizedLabel lblFilter;
    }
}
