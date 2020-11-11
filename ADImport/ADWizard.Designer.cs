using System.Windows.Forms.Integration;
namespace ADImport
{
    partial class ADWizard
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ADWizard));
            this.host = new System.Windows.Forms.Integration.ElementHost();
            this.headerControl = new WPFFramework.Controls.Viewers.Header();
            this.pnlBody = new System.Windows.Forms.Panel();
            this.pnlAction = new System.Windows.Forms.Panel();
            this.footerButtons = new WinFormsFramework.WizardFooterButtons();
            this.progressBarWaiting = new System.Windows.Forms.ProgressBar();
            this.lblState = new ADImport.LocalizedLabel();
            this.pnlSeparator = new System.Windows.Forms.Panel();
            this.pnlAction.SuspendLayout();
            this.SuspendLayout();
            // 
            // host
            // 
            this.host.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(37)))), ((int)(((byte)(36)))));
            this.host.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.host.Dock = System.Windows.Forms.DockStyle.Top;
            this.host.ForeColor = System.Drawing.SystemColors.ControlText;
            this.host.Location = new System.Drawing.Point(0, 0);
            this.host.Margin = new System.Windows.Forms.Padding(0);
            this.host.Name = "host";
            this.host.Size = new System.Drawing.Size(746, 80);
            this.host.TabIndex = 3;
            this.host.TabStop = false;
            this.host.Child = this.headerControl;
            // 
            // pnlBody
            // 
            this.pnlBody.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlBody.Location = new System.Drawing.Point(0, 80);
            this.pnlBody.Name = "pnlBody";
            this.pnlBody.Size = new System.Drawing.Size(746, 402);
            this.pnlBody.TabIndex = 2;
            // 
            // pnlAction
            // 
            this.pnlAction.Controls.Add(this.footerButtons);
            this.pnlAction.Controls.Add(this.progressBarWaiting);
            this.pnlAction.Controls.Add(this.lblState);
            this.pnlAction.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlAction.Location = new System.Drawing.Point(0, 482);
            this.pnlAction.Name = "pnlAction";
            this.pnlAction.Padding = new System.Windows.Forms.Padding(3);
            this.pnlAction.Size = new System.Drawing.Size(746, 34);
            this.pnlAction.TabIndex = 9;
            // 
            // footerButtons
            // 
            this.footerButtons.BackColor = System.Drawing.Color.Transparent;
            this.footerButtons.Dock = System.Windows.Forms.DockStyle.Right;
            this.footerButtons.Location = new System.Drawing.Point(452, 3);
            this.footerButtons.Margin = new System.Windows.Forms.Padding(0);
            this.footerButtons.Name = "footerButtons";
            this.footerButtons.Size = new System.Drawing.Size(291, 28);
            this.footerButtons.TabIndex = 11;
            // 
            // progressBarWaiting
            // 
            this.progressBarWaiting.Location = new System.Drawing.Point(6, 18);
            this.progressBarWaiting.Name = "progressBarWaiting";
            this.progressBarWaiting.Size = new System.Drawing.Size(208, 13);
            this.progressBarWaiting.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            this.progressBarWaiting.TabIndex = 10;
            this.progressBarWaiting.Visible = false;
            // 
            // lblState
            // 
            this.lblState.AutoSize = true;
            this.lblState.Location = new System.Drawing.Point(3, 3);
            this.lblState.Name = "lblState";
            this.lblState.ResourceString = null;
            this.lblState.Size = new System.Drawing.Size(32, 13);
            this.lblState.TabIndex = 9;
            this.lblState.Text = "State";
            this.lblState.Visible = false;
            // 
            // pnlSeparator
            // 
            this.pnlSeparator.BackColor = System.Drawing.Color.Black;
            this.pnlSeparator.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlSeparator.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlSeparator.Location = new System.Drawing.Point(0, 481);
            this.pnlSeparator.Name = "pnlSeparator";
            this.pnlSeparator.Size = new System.Drawing.Size(746, 1);
            this.pnlSeparator.TabIndex = 0;
            // 
            // ADWizard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(746, 516);
            this.Controls.Add(this.pnlSeparator);
            this.Controls.Add(this.pnlBody);
            this.Controls.Add(this.pnlAction);
            this.Controls.Add(this.host);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(762, 457);
            this.Name = "ADWizard";
            this.Text = "Kentico Xperience Active Directory Import Utility";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ADWizard_FormClosing);
            this.Load += new System.EventHandler(this.ADWizard_Load);
            this.pnlAction.ResumeLayout(false);
            this.pnlAction.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlBody;
        private System.Windows.Forms.Panel pnlAction;
        private LocalizedLabel lblState;
        private System.Windows.Forms.ProgressBar progressBarWaiting;
        private WPFFramework.Controls.Viewers.Header headerControl;
        private WinFormsFramework.WizardFooterButtons footerButtons;
        private System.Windows.Forms.Panel pnlSeparator;
        private ElementHost host;
    }
}