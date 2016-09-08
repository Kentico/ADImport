namespace ADImport
{
    partial class Step8
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
            this.grdGroups = new System.Windows.Forms.DataGridView();
            this.lblFilter = new ADImport.LocalizedLabel();
            this.txtFilter = new System.Windows.Forms.TextBox();
            this.btnFilter = new ADImport.LocalizedButton();
            this.pnlFilter = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.grdGroups)).BeginInit();
            this.pnlFilter.SuspendLayout();
            this.SuspendLayout();
            // 
            // grdGroups
            // 
            this.grdGroups.AllowUserToAddRows = false;
            this.grdGroups.AllowUserToDeleteRows = false;
            this.grdGroups.AllowUserToResizeRows = false;
            this.grdGroups.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdGroups.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdGroups.Location = new System.Drawing.Point(5, 30);
            this.grdGroups.MultiSelect = false;
            this.grdGroups.Name = "grdGroups";
            this.grdGroups.RowHeadersVisible = false;
            this.grdGroups.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grdGroups.Size = new System.Drawing.Size(584, 278);
            this.grdGroups.TabIndex = 4;
            this.grdGroups.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.grdGroups_CellValueChanged);
            this.grdGroups.CurrentCellDirtyStateChanged += new System.EventHandler(this.grdGroups_CurrentCellDirtyStateChanged);
            // 
            // lblFilter
            // 
            this.lblFilter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblFilter.AutoSize = true;
            this.lblFilter.Location = new System.Drawing.Point(178, 8);
            this.lblFilter.Name = "lblFilter";
            this.lblFilter.ResourceString = "Step8_GroupName";
            this.lblFilter.Size = new System.Drawing.Size(164, 13);
            this.lblFilter.TabIndex = 1;
            // 
            // txtFilter
            // 
            this.txtFilter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtFilter.Location = new System.Drawing.Point(353, 5);
            this.txtFilter.Name = "txtFilter";
            this.txtFilter.Size = new System.Drawing.Size(150, 20);
            this.txtFilter.TabIndex = 2;
            this.txtFilter.Enter += new System.EventHandler(this.txtFilter_Enter);
            this.txtFilter.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtFilter_KeyPress);
            this.txtFilter.Leave += new System.EventHandler(this.txtFilter_Leave);
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
            // pnlFilter
            // 
            this.pnlFilter.Controls.Add(this.lblFilter);
            this.pnlFilter.Controls.Add(this.btnFilter);
            this.pnlFilter.Controls.Add(this.txtFilter);
            this.pnlFilter.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlFilter.Location = new System.Drawing.Point(5, 0);
            this.pnlFilter.Name = "pnlFilter";
            this.pnlFilter.Size = new System.Drawing.Size(584, 30);
            this.pnlFilter.TabIndex = 0;
            // 
            // Step8
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.grdGroups);
            this.Controls.Add(this.pnlFilter);
            this.Name = "Step8";
            this.Padding = new System.Windows.Forms.Padding(5, 0, 5, 5);
            this.ParentChanged += new System.EventHandler(this.Step8_ParentChanged);
            ((System.ComponentModel.ISupportInitialize)(this.grdGroups)).EndInit();
            this.pnlFilter.ResumeLayout(false);
            this.pnlFilter.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView grdGroups;
        private LocalizedLabel lblFilter;
        private System.Windows.Forms.TextBox txtFilter;
        private LocalizedButton btnFilter;
        private System.Windows.Forms.Panel pnlFilter;
    }
}
