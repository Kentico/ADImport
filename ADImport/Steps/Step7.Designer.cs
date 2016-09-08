namespace ADImport
{
    partial class Step7
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
            this.btnFilter = new ADImport.LocalizedButton();
            this.txtFilter = new System.Windows.Forms.TextBox();
            this.lblFilter = new ADImport.LocalizedLabel();
            this.grdUsers = new System.Windows.Forms.DataGridView();
            this.pnlFilter = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.grdUsers)).BeginInit();
            this.pnlFilter.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnFilter
            // 
            this.btnFilter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnFilter.Location = new System.Drawing.Point(509, 3);
            this.btnFilter.Name = "btnFilter";
            this.btnFilter.ResourceString = "General_Filter";
            this.btnFilter.Size = new System.Drawing.Size(75, 23);
            this.btnFilter.TabIndex = 3;
            this.btnFilter.UseVisualStyleBackColor = true;
            this.btnFilter.Click += new System.EventHandler(this.btnFilter_Click);
            // 
            // txtFilter
            // 
            this.txtFilter.AcceptsReturn = true;
            this.txtFilter.AcceptsTab = true;
            this.txtFilter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtFilter.Location = new System.Drawing.Point(353, 4);
            this.txtFilter.Name = "txtFilter";
            this.txtFilter.Size = new System.Drawing.Size(150, 20);
            this.txtFilter.TabIndex = 2;
            this.txtFilter.Enter += new System.EventHandler(this.txtFilter_Enter);
            this.txtFilter.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtFilter_KeyPress);
            this.txtFilter.Leave += new System.EventHandler(this.txtFilter_Leave);
            // 
            // lblFilter
            // 
            this.lblFilter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblFilter.AutoSize = true;
            this.lblFilter.Location = new System.Drawing.Point(176, 7);
            this.lblFilter.Name = "lblFilter";
            this.lblFilter.ResourceString = "Step7_DisplayUserName";
            this.lblFilter.Size = new System.Drawing.Size(169, 13);
            this.lblFilter.TabIndex = 1;
            // 
            // grdUsers
            // 
            this.grdUsers.AllowUserToAddRows = false;
            this.grdUsers.AllowUserToDeleteRows = false;
            this.grdUsers.AllowUserToResizeRows = false;
            this.grdUsers.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdUsers.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdUsers.Location = new System.Drawing.Point(5, 30);
            this.grdUsers.MultiSelect = false;
            this.grdUsers.Name = "grdUsers";
            this.grdUsers.RowHeadersVisible = false;
            this.grdUsers.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grdUsers.Size = new System.Drawing.Size(584, 278);
            this.grdUsers.TabIndex = 4;
            this.grdUsers.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.grdUsers_CellValueChanged);
            this.grdUsers.CurrentCellDirtyStateChanged += new System.EventHandler(this.grdUsers_CurrentCellDirtyStateChanged);
            // 
            // pnlFilter
            // 
            this.pnlFilter.Controls.Add(this.btnFilter);
            this.pnlFilter.Controls.Add(this.txtFilter);
            this.pnlFilter.Controls.Add(this.lblFilter);
            this.pnlFilter.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlFilter.Location = new System.Drawing.Point(5, 0);
            this.pnlFilter.Name = "pnlFilter";
            this.pnlFilter.Size = new System.Drawing.Size(584, 30);
            this.pnlFilter.TabIndex = 0;
            // 
            // Step7
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.grdUsers);
            this.Controls.Add(this.pnlFilter);
            this.Name = "Step7";
            this.Padding = new System.Windows.Forms.Padding(5, 0, 5, 5);
            this.ParentChanged += new System.EventHandler(this.Step7_ParentChanged);
            ((System.ComponentModel.ISupportInitialize)(this.grdUsers)).EndInit();
            this.pnlFilter.ResumeLayout(false);
            this.pnlFilter.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private LocalizedButton btnFilter;
        private System.Windows.Forms.TextBox txtFilter;
        private LocalizedLabel lblFilter;
        private System.Windows.Forms.DataGridView grdUsers;
        private System.Windows.Forms.Panel pnlFilter;
    }
}
