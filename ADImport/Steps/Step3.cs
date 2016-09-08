using System;
using System.Threading.Tasks;
using System.Windows.Forms;

using WinAppFoundation;

using WinFormsFramework;

namespace ADImport
{
    /// <summary>
    /// Step3 - AD connection.
    /// </summary>
    public partial class Step3 : AbstractStep
    {
        #region "Properties"

        /// <summary>
        /// Label used for displaying info/warning/error messages.
        /// </summary>
        public override Label DefaultMessageLabel
        {
            get
            {
                return lblMessage;
            }
        }

        #endregion


        #region "Constructors"

        /// <summary>
        /// Step3 constructor.
        /// </summary>
        public Step3()
            : this(null)
        {
        }

        /// <summary>
        /// Step3 constructor.
        /// </summary>
        public Step3(ADWizard wizard)
            : base(wizard)
        {
            InitializeComponent();
        }

        #endregion


        #region "Control events"

        private void Step3_ParentChanged(object sender, EventArgs e)
        {
            if (Parent != null)
            {
                // Setup input fields
                SetupInputFields();

                // Preset step
                radUseCurrentUserAccount.Checked = ImportProfile.ADUseCurrentUserAccount;
                radSpecifyCredentials.Checked = !ImportProfile.ADUseCurrentUserAccount;
                txtDomainController.Text = ImportProfile.ADControllerAddress;
                txtUsername.Text = ImportProfile.ADUsername;
                txtPassword.Text = ImportProfile.ADPassword;
            }
        }


        private void radUseCurrentUserAccount_CheckedChanged(object sender, EventArgs e)
        {
            SetupInputFields();
        }


        private void radSpecifyCredentials_CheckedChanged(object sender, EventArgs e)
        {
            SetupInputFields();
        }


        private async void btnTestConnection_Click(object sender, EventArgs e)
        {
            // Perform all necessary actions before validating connection
            if (PrepareConnectionValidation())
            {
                await VerifyADConnectionAsync();
            }
        }

        #endregion


        #region "Methods"

        /// <summary>
        /// Check if current step is valid.
        /// </summary>
        public override async Task<bool> IsValid()
        {
            // Perform all necessary actions before validating connection
            if (PrepareConnectionValidation())
            {
                bool result = await VerifyADConnectionAsync();
                // Return validation result
                return result;
            }
            else
            {
                return false;
            }
        }


        /// <summary>
        /// Performs all necessary actions before validating connection.
        /// </summary>
        private bool PrepareConnectionValidation()
        {
            // Setup import profile
            ImportProfile.ADControllerAddress = txtDomainController.Text;
            ImportProfile.ADUsername = txtUsername.Text;
            ImportProfile.ADPassword = txtPassword.Text;
            ImportProfile.ADUseCurrentUserAccount = radUseCurrentUserAccount.Checked;

            if (((txtUsername.Text == string.Empty) || (txtDomainController.Text == string.Empty)) && radSpecifyCredentials.Checked)
            {
                SetError("Step3_SpecifyConnection");
                return false;
            }
            else
            {
                lblMessage.Visible = false;

                // Setup UI
                Wizard.SetWaitState(true, ResHelper.GetString("Step3_TryingToConnect"));
                Enabled = false;
                Wizard.ButtonNext.Enabled = false;
                return true;
            }
        }


        /// <summary>
        /// Asynchronously verifies connection credentials and performs proper action.
        /// </summary>
        private Task<bool> VerifyADConnectionAsync()
        {
            return Task.Run(() => VerifyADConnection());
        }


        /// <summary>
        /// Verifies connection credentials and performs proper action.
        /// </summary>
        private Task<bool> VerifyADConnection()
        {
            TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();
            try
            {
                ADProvider.InitializeDomainContext();
                SetMessage("Step3_ConnectionSuccessful");
                tcs.SetResult(true);
            }
            catch (LocalUserAccountException)
            {
                DisableCurrentUserAccount();
                SetError(ResHelper.GetString("Error_ConnectingAD") + AbstractResHelper.LINE_BREAK + ResHelper.GetString("Error_LocalUserAccountException"));
            }
            catch (DCConnectionException)
            {
                SetError(ResHelper.GetString("Error_ConnectingAD") + AbstractResHelper.LINE_BREAK + ResHelper.GetString("Error_LocalUserAccountException"));
            }
            catch (Exception ex)
            {
                SetError(ResHelper.GetString("Error_ConnectingAD") + AbstractResHelper.LINE_BREAK + ex.Message);
            }
            finally
            {
                Wizard.SetWaitState(false, string.Empty);
                Wizard.SetControlEnabled(Wizard.ButtonNext, true);
                using (InvokeHelper ih = new InvokeHelper(this))
                {
                    ih.InvokeMethod(() => Enabled = true);
                }
            }
            return tcs.Task;
        }


        /// <summary>
        /// Disables current user account option.
        /// </summary>
        private void DisableCurrentUserAccount()
        {
            using (InvokeHelper ih = new InvokeHelper(radUseCurrentUserAccount))
            {
                ih.InvokeMethod(() => radUseCurrentUserAccount.Checked = false);

                using (InvokeHelper ih1 = new InvokeHelper(radSpecifyCredentials))
                {
                    ih1.InvokeMethod(() => radSpecifyCredentials.Checked = true);
                }

                ih.InvokeMethod(() => radUseCurrentUserAccount.Enabled = false);
            }
            ImportProfile.ADUseCurrentUserAccount = false;
        }


        /// <summary>
        /// Sets up controls depending on connection type.
        /// </summary>
        private void SetupInputFields()
        {
            bool enabled = radSpecifyCredentials.Checked;
            txtDomainController.Enabled = enabled;
            txtUsername.Enabled = enabled;
            txtPassword.Enabled = enabled;
            lblDomainController.Enabled = enabled;
            lblUsername.Enabled = enabled;
            lblPassword.Enabled = enabled;
        }

        #endregion
    }
}