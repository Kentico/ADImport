using System.Windows.Forms;

namespace ADImport
{
    partial class Step11
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
            this.listLog = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // listLog
            // 
            this.listLog.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.listLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listLog.Location = new System.Drawing.Point(0, 0);
            this.listLog.Name = "listLog";
            this.listLog.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.listLog.Size = new System.Drawing.Size(594, 313);
            this.listLog.TabIndex = 0;
            // 
            // Step11
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.listLog);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "Step11";
            this.Load += new System.EventHandler(this.Step11_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox listLog;

    }
}
