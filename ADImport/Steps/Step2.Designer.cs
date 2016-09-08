namespace ADImport
{
    partial class Step2
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
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.lblSQLServerAddress = new LocalizedLabel();
            this.lblDBName = new LocalizedLabel();
            this.lblUsername = new LocalizedLabel();
            this.radUseSQLAccount = new LocalizedRadioButton();
            this.radUseTrustedConnection = new LocalizedRadioButton();
            this.lblPassword = new LocalizedLabel();
            this.txtUsername = new System.Windows.Forms.TextBox();
            this.lblMessage = new LocalizedLabel();
            this.btnEstablishConnection = new LocalizedButton();
            this.cmbSQLServerAddress = new System.Windows.Forms.ComboBox();
            this.cmbDBName = new System.Windows.Forms.ComboBox();
            this.pnlDatabase = new System.Windows.Forms.Panel();
            this.pnlDatabase.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(162, 71);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Size = new System.Drawing.Size(150, 20);
            this.txtPassword.TabIndex = 4;
            this.txtPassword.UseSystemPasswordChar = true;
            this.txtPassword.TextChanged += new System.EventHandler(this.txtPassword_TextChanged);
            // 
            // lblSQLServerAddress
            // 
            this.lblSQLServerAddress.AutoSize = true;
            this.lblSQLServerAddress.Location = new System.Drawing.Point(11, 11);
            this.lblSQLServerAddress.Name = "lblSQLServerAddress";
            this.lblSQLServerAddress.Size = new System.Drawing.Size(159, 13);
            this.lblSQLServerAddress.TabIndex = 1;
            this.lblSQLServerAddress.ResourceString = "Step2_ServerName";
            // 
            // lblDBName
            // 
            this.lblDBName.AutoSize = true;
            this.lblDBName.Location = new System.Drawing.Point(-3, 129);
            this.lblDBName.Name = "lblDBName";
            this.lblDBName.Size = new System.Drawing.Size(85, 13);
            this.lblDBName.TabIndex = 1;
            this.lblDBName.ResourceString = "Step2_Database";
            // 
            // lblUsername
            // 
            this.lblUsername.AutoSize = true;
            this.lblUsername.Location = new System.Drawing.Point(95, 48);
            this.lblUsername.Name = "lblUsername";
            this.lblUsername.Size = new System.Drawing.Size(61, 13);
            this.lblUsername.TabIndex = 1;
            this.lblUsername.ResourceString = "Step2_Username";
            // 
            // radUseSQLAccount
            // 
            this.radUseSQLAccount.AutoSize = true;
            this.radUseSQLAccount.Checked = true;
            this.radUseSQLAccount.Location = new System.Drawing.Point(0, 23);
            this.radUseSQLAccount.Name = "radUseSQLAccount";
            this.radUseSQLAccount.Size = new System.Drawing.Size(144, 17);
            this.radUseSQLAccount.TabIndex = 2;
            this.radUseSQLAccount.TabStop = true;
            this.radUseSQLAccount.ResourceString = "Step2_UseAccountName";
            this.radUseSQLAccount.UseVisualStyleBackColor = true;
            this.radUseSQLAccount.CheckedChanged += new System.EventHandler(this.radUseSQLAccount_CheckedChanged);
            // 
            // radUseTrustedConnection
            // 
            this.radUseTrustedConnection.AutoSize = true;
            this.radUseTrustedConnection.Location = new System.Drawing.Point(0, 0);
            this.radUseTrustedConnection.Name = "radUseTrustedConnection";
            this.radUseTrustedConnection.Size = new System.Drawing.Size(208, 17);
            this.radUseTrustedConnection.TabIndex = 2;
            this.radUseTrustedConnection.ResourceString = "Step2_UseTrustedConnection";
            this.radUseTrustedConnection.UseVisualStyleBackColor = true;
            this.radUseTrustedConnection.CheckedChanged += new System.EventHandler(this.radUseTrustedConnection_CheckedChanged);
            // 
            // lblPassword
            // 
            this.lblPassword.AutoSize = true;
            this.lblPassword.Location = new System.Drawing.Point(100, 74);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new System.Drawing.Size(56, 13);
            this.lblPassword.TabIndex = 1;
            this.lblPassword.ResourceString = "Step2_Password";
            // 
            // txtUsername
            // 
            this.txtUsername.Location = new System.Drawing.Point(162, 45);
            this.txtUsername.Name = "txtUsername";
            this.txtUsername.Size = new System.Drawing.Size(150, 20);
            this.txtUsername.TabIndex = 3;
            this.txtUsername.TextChanged += new System.EventHandler(this.txtUsername_TextChanged);
            // 
            // lblMessage
            // 
            this.lblMessage.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblMessage.Location = new System.Drawing.Point(11, 195);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new System.Drawing.Size(567, 75);
            this.lblMessage.TabIndex = 5;
            this.lblMessage.Text = "Message";
            this.lblMessage.Visible = false;
            // 
            // btnEstablishConnection
            // 
            this.btnEstablishConnection.Location = new System.Drawing.Point(183, 97);
            this.btnEstablishConnection.Name = "btnEstablishConnection";
            this.btnEstablishConnection.Size = new System.Drawing.Size(129, 23);
            this.btnEstablishConnection.TabIndex = 5;
            this.btnEstablishConnection.ResourceString = "Step2_EstablishConnection";
            this.btnEstablishConnection.UseVisualStyleBackColor = true;
            this.btnEstablishConnection.Click += new System.EventHandler(this.btnEstablishConnection_Click);
            // 
            // cmbSQLServerAddress
            // 
            this.cmbSQLServerAddress.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append;
            this.cmbSQLServerAddress.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.cmbSQLServerAddress.FormattingEnabled = true;
            this.cmbSQLServerAddress.Location = new System.Drawing.Point(176, 7);
            this.cmbSQLServerAddress.Name = "cmbSQLServerAddress";
            this.cmbSQLServerAddress.Size = new System.Drawing.Size(150, 21);
            this.cmbSQLServerAddress.TabIndex = 6;
            this.cmbSQLServerAddress.TextChanged += new System.EventHandler(this.cmbSQLServerAddress_TextChanged);
            // 
            // cmbDBName
            // 
            this.cmbDBName.Enabled = false;
            this.cmbDBName.FormattingEnabled = true;
            this.cmbDBName.Location = new System.Drawing.Point(162, 126);
            this.cmbDBName.Name = "cmbDBName";
            this.cmbDBName.Size = new System.Drawing.Size(150, 21);
            this.cmbDBName.TabIndex = 7;
            this.cmbDBName.SelectedIndexChanged += new System.EventHandler(this.cmbDBName_SelectedIndexChanged);
            this.cmbDBName.TextChanged += new System.EventHandler(this.cmbDBName_TextChanged);
            // 
            // pnlDatabase
            // 
            this.pnlDatabase.Controls.Add(this.cmbDBName);
            this.pnlDatabase.Controls.Add(this.txtPassword);
            this.pnlDatabase.Controls.Add(this.txtUsername);
            this.pnlDatabase.Controls.Add(this.btnEstablishConnection);
            this.pnlDatabase.Controls.Add(this.lblDBName);
            this.pnlDatabase.Controls.Add(this.lblUsername);
            this.pnlDatabase.Controls.Add(this.radUseTrustedConnection);
            this.pnlDatabase.Controls.Add(this.lblPassword);
            this.pnlDatabase.Controls.Add(this.radUseSQLAccount);
            this.pnlDatabase.Enabled = false;
            this.pnlDatabase.Location = new System.Drawing.Point(14, 34);
            this.pnlDatabase.Name = "pnlDatabase";
            this.pnlDatabase.Size = new System.Drawing.Size(321, 158);
            this.pnlDatabase.TabIndex = 8;
            // 
            // Step2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pnlDatabase);
            this.Controls.Add(this.cmbSQLServerAddress);
            this.Controls.Add(this.lblMessage);
            this.Controls.Add(this.lblSQLServerAddress);
            this.Name = "Step2";
            this.ParentChanged += new System.EventHandler(this.Step2_ParentChanged);
            this.pnlDatabase.ResumeLayout(false);
            this.pnlDatabase.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtPassword;
        private LocalizedLabel lblSQLServerAddress;
        private LocalizedLabel lblDBName;
        private LocalizedLabel lblUsername;
        private LocalizedRadioButton radUseSQLAccount;
        private LocalizedRadioButton radUseTrustedConnection;
        private LocalizedLabel lblPassword;
        private System.Windows.Forms.TextBox txtUsername;
        private LocalizedLabel lblMessage;
        private LocalizedButton btnEstablishConnection;
        private System.Windows.Forms.ComboBox cmbSQLServerAddress;
        private System.Windows.Forms.ComboBox cmbDBName;
        private System.Windows.Forms.Panel pnlDatabase;
    }
}
