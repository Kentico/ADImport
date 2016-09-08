namespace ADImport
{
    partial class Step3
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
            this.radUseCurrentUserAccount = new LocalizedRadioButton();
            this.lblPassword = new LocalizedLabel();
            this.lblUsername = new LocalizedLabel();
            this.txtUsername = new System.Windows.Forms.TextBox();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.radSpecifyCredentials = new LocalizedRadioButton();
            this.txtDomainController = new System.Windows.Forms.TextBox();
            this.lblDomainController = new LocalizedLabel();
            this.lblMessage = new LocalizedLabel();
            this.btnTestConnection = new LocalizedButton();
            this.SuspendLayout();
            // 
            // radUseCurrentUserAccount
            // 
            this.radUseCurrentUserAccount.AutoSize = true;
            this.radUseCurrentUserAccount.Checked = true;
            this.radUseCurrentUserAccount.Location = new System.Drawing.Point(11, 11);
            this.radUseCurrentUserAccount.Name = "radUseCurrentUserAccount";
            this.radUseCurrentUserAccount.Size = new System.Drawing.Size(285, 17);
            this.radUseCurrentUserAccount.TabIndex = 1;
            this.radUseCurrentUserAccount.TabStop = true;
            this.radUseCurrentUserAccount.ResourceString = "Step3_UseCurrentAccount";
            this.radUseCurrentUserAccount.UseVisualStyleBackColor = true;
            this.radUseCurrentUserAccount.CheckedChanged += new System.EventHandler(this.radUseCurrentUserAccount_CheckedChanged);
            // 
            // lblPassword
            // 
            this.lblPassword.AutoSize = true;
            this.lblPassword.Location = new System.Drawing.Point(158, 112);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new System.Drawing.Size(56, 13);
            this.lblPassword.TabIndex = 4;
            this.lblPassword.ResourceString = "Step3_Password";
            // 
            // lblUsername
            // 
            this.lblUsername.AutoSize = true;
            this.lblUsername.Location = new System.Drawing.Point(153, 86);
            this.lblUsername.Name = "lblUsername";
            this.lblUsername.Size = new System.Drawing.Size(61, 13);
            this.lblUsername.TabIndex = 5;
            this.lblUsername.ResourceString = "Step3_Username";
            // 
            // txtUsername
            // 
            this.txtUsername.Location = new System.Drawing.Point(220, 83);
            this.txtUsername.Name = "txtUsername";
            this.txtUsername.Size = new System.Drawing.Size(150, 20);
            this.txtUsername.TabIndex = 3;
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(220, 109);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Size = new System.Drawing.Size(150, 20);
            this.txtPassword.TabIndex = 4;
            this.txtPassword.UseSystemPasswordChar = true;
            // 
            // radSpecifyCredentials
            // 
            this.radSpecifyCredentials.AutoSize = true;
            this.radSpecifyCredentials.Location = new System.Drawing.Point(11, 34);
            this.radSpecifyCredentials.Name = "radSpecifyCredentials";
            this.radSpecifyCredentials.Size = new System.Drawing.Size(247, 17);
            this.radSpecifyCredentials.TabIndex = 1;
            this.radSpecifyCredentials.ResourceString = "Step3_SpecifyCredentials";
            this.radSpecifyCredentials.UseVisualStyleBackColor = true;
            this.radSpecifyCredentials.CheckedChanged += new System.EventHandler(this.radSpecifyCredentials_CheckedChanged);
            // 
            // txtDomainController
            // 
            this.txtDomainController.Location = new System.Drawing.Point(220, 57);
            this.txtDomainController.Name = "txtDomainController";
            this.txtDomainController.Size = new System.Drawing.Size(150, 20);
            this.txtDomainController.TabIndex = 2;
            // 
            // lblDomainController
            // 
            this.lblDomainController.AutoSize = true;
            this.lblDomainController.Location = new System.Drawing.Point(28, 60);
            this.lblDomainController.Name = "lblDomainController";
            this.lblDomainController.Size = new System.Drawing.Size(186, 13);
            this.lblDomainController.TabIndex = 5;
            this.lblDomainController.ResourceString = "Step3_DCName";
            // 
            // lblMessage
            // 
            this.lblMessage.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblMessage.Location = new System.Drawing.Point(28, 166);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new System.Drawing.Size(511, 68);
            this.lblMessage.TabIndex = 6;
            this.lblMessage.Text = "Message";
            this.lblMessage.Visible = false;
            // 
            // btnTestConnection
            // 
            this.btnTestConnection.Location = new System.Drawing.Point(270, 135);
            this.btnTestConnection.Name = "btnTestConnection";
            this.btnTestConnection.Size = new System.Drawing.Size(100, 23);
            this.btnTestConnection.TabIndex = 5;
            this.btnTestConnection.ResourceString = "General_TestConnection";
            this.btnTestConnection.UseVisualStyleBackColor = true;
            this.btnTestConnection.Click += new System.EventHandler(this.btnTestConnection_Click);
            // 
            // Step3
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnTestConnection);
            this.Controls.Add(this.lblMessage);
            this.Controls.Add(this.lblPassword);
            this.Controls.Add(this.lblDomainController);
            this.Controls.Add(this.lblUsername);
            this.Controls.Add(this.txtDomainController);
            this.Controls.Add(this.txtUsername);
            this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.radSpecifyCredentials);
            this.Controls.Add(this.radUseCurrentUserAccount);
            this.Name = "Step3";
            this.ParentChanged += new System.EventHandler(this.Step3_ParentChanged);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private LocalizedRadioButton radUseCurrentUserAccount;
        private LocalizedLabel lblPassword;
        private LocalizedLabel lblUsername;
        private System.Windows.Forms.TextBox txtUsername;
        private System.Windows.Forms.TextBox txtPassword;
        private LocalizedRadioButton radSpecifyCredentials;
        private System.Windows.Forms.TextBox txtDomainController;
        private LocalizedLabel lblDomainController;
        private LocalizedLabel lblMessage;
        private LocalizedButton btnTestConnection;

    }
}
