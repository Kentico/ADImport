using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

using CMS.DataEngine;
using CMS.Helpers;
using CMS.LicenseProvider;
using CMS.Base;

using WinAppFoundation;
using WinFormsFramework;

namespace ADImport
{
    /// <summary>
    /// Step2 - Database connection.
    /// </summary>
    public partial class Step2 : AbstractStep
    {
        #region "Private variables"

        private bool? connectionResult = null;
        private List<string> mSQLInstanceNames = null;

        #endregion


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


        /// <summary>
        /// Gets a sorted list containing instance names of available SQL servers.
        /// </summary>
        private List<string> SQLInstanceNames
        {
            get
            {
                if (mSQLInstanceNames == null)
                {
                    mSQLInstanceNames = SqlServerHelper.GetNeighbouringSQLInstanceNames();
                }
                return mSQLInstanceNames;
            }
        }

        #endregion


        #region "Constructors"

        /// <summary>
        /// Step2 constructor.
        /// </summary>
        public Step2()
            : this(null)
        {
        }


        /// <summary>
        /// Step2 constructor.
        /// </summary>
        public Step2(ADWizard wizard)
            : base(wizard)
        {
            InitializeComponent();
            SetupInputFields();
        }

        #endregion


        #region "Control events"

        private void Step2_ParentChanged(object sender, EventArgs e)
        {
            if (Parent != null)
            {
                // Preset step
                txtUsername.Text = ImportProfile.SQLServerUsername;
                txtPassword.Text = ImportProfile.SQLServerPassword;
                radUseTrustedConnection.Checked = ImportProfile.SQLUseTrustedConnection;
                radUseSQLAccount.Checked = !ImportProfile.SQLUseTrustedConnection;

                InitializeAutoCompleteAsync();
            }
        }


        private void radUseSQLAccount_CheckedChanged(object sender, EventArgs e)
        {
            SetupInputFields();
        }


        private void radUseTrustedConnection_CheckedChanged(object sender, EventArgs e)
        {
            SetupInputFields();
        }


        private void txtUsername_TextChanged(object sender, EventArgs e)
        {
            InvalidateDBs();
        }


        private void txtPassword_TextChanged(object sender, EventArgs e)
        {
            InvalidateDBs();
        }


        private async void btnEstablishConnection_Click(object sender, EventArgs e)
        {
            // Perform all necessary actions before validating connection
            if (PrepareConnectionValidation())
            {
                // Try to establish connection
                await ConnectToInstanceAsync();
            }
        }


        private void cmbSQLServerAddress_TextChanged(object sender, EventArgs e)
        {
            if ((ImportProfile.SQLServerAddress == null) || (ImportProfile.SQLServerAddress.ToLowerCSafe() != cmbSQLServerAddress.Text.ToLowerCSafe()))
            {
                ImportProfile.SQLServerAddress = cmbSQLServerAddress.Text;
                InvalidateDBs();
            }
            pnlDatabase.Enabled = !string.IsNullOrEmpty(cmbSQLServerAddress.Text);
        }


        private async void cmbDBName_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Validate database
            if ((connectionResult != null) && connectionResult.Value)
            {
                await ValidateDatabaseAsync();
            }
        }


        private void cmbDBName_TextChanged(object sender, EventArgs e)
        {
            ImportProfile.SQLServerDatabase = cmbDBName.Text;
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
                // Try to establish connection
                await ConnectToInstanceAsync();
                bool validationResult = false;
                if ((connectionResult != null) && connectionResult.Value)
                {
                    validationResult = await ValidateDatabaseAsync();
                }
                // Return the validation result
                return validationResult;
            }
            else
            {
                return false;
            }
        }


        /// <summary>
        /// Clears combobox with databases
        /// </summary>
        private void InvalidateDBs()
        {
            cmbDBName.Items.Clear();
        }


        /// <summary>
        /// Validate input fields.
        /// </summary>
        /// <returns>TRUE if fields are valid</returns>
        private bool ValidateFields()
        {
            if (string.IsNullOrEmpty(cmbSQLServerAddress.Text))
            {
                SetError("Step2_SpecifyServerAddress");
                return false;
            }
            if (radUseSQLAccount.Checked)
            {
                if (string.IsNullOrEmpty(txtUsername.Text))
                {
                    SetError("Step2_SpecifyUsername");
                    return false;
                }
            }
            return true;
        }

        #endregion


        #region "Async methods"

        /// <summary>
        /// Asynchronously initializes autocomplete.
        /// </summary>
        private async void InitializeAutoCompleteAsync()
        {
            await Task.Run(() => InitializeAutoComplete());
        }


        private void InitializeAutoComplete()
        {
            // Change message
            Wizard.SetWaitState(true, ResHelper.GetString("Step2_LoadingInstances"));
            string[] instances = SQLInstanceNames.ToArray();
            AutoCompleteStringCollection autoCompleteStringCollection = new AutoCompleteStringCollection();
            autoCompleteStringCollection.AddRange(instances);

            using (InvokeHelper ih = new InvokeHelper(cmbSQLServerAddress))
            {
                ih.InvokeMethod(() => cmbSQLServerAddress.AutoCompleteCustomSource = autoCompleteStringCollection);
                ih.InvokeMethod(() => cmbSQLServerAddress.Items.Clear());
                ih.InvokeMethod(() => cmbSQLServerAddress.Items.AddRange(instances));
                ih.InvokeMethod(() => cmbSQLServerAddress.Text = ImportProfile.SQLServerAddress);
            }

            if (!string.IsNullOrEmpty(ImportProfile.SQLServerDatabase))
            {
                using (InvokeHelper ih = new InvokeHelper(cmbDBName))
                {
                    ih.InvokeMethod(delegate
                                        {
                                            if (cmbDBName.Items.Contains(ImportProfile.SQLServerDatabase))
                                            {
                                                cmbDBName.SelectedItem = ImportProfile.SQLServerDatabase;
                                            }
                                            else
                                            {
                                                cmbDBName.Text = ImportProfile.SQLServerDatabase;
                                            }
                                        });
                }
            }
            Wizard.SetWaitState(false, string.Empty);
        }


        /// <summary>
        /// Asynchronously connects to selected SQL instance.
        /// </summary>
        private Task ConnectToInstanceAsync()
        {
            return Task.Run(() => ConnectToInstance());
        }


        /// <summary>
        /// Connects to selected SQL instance.
        /// </summary>
        private void ConnectToInstance()
        {
            try
            {
                string[] databases = SqlServerHelper.GetDatabaseNames(CMSImport.BaseConnectionString).ToArray();
                using (InvokeHelper ih = new InvokeHelper(cmbDBName))
                {
                    ih.InvokeMethod(InvalidateDBs);
                    ih.InvokeMethod(() => cmbDBName.Items.AddRange(databases));
                    ih.InvokeMethod(() => cmbDBName.Enabled = true);
                    if (!string.IsNullOrEmpty(ImportProfile.SQLServerDatabase))
                    {
                        ih.InvokeMethod(delegate
                                            {
                                                if (cmbDBName.Items.Contains(ImportProfile.SQLServerDatabase))
                                                {
                                                    cmbDBName.SelectedItem = ImportProfile.SQLServerDatabase;
                                                }
                                            });
                    }
                }

                SetMessage("Step2_ConnectionSuccessful");
                connectionResult = true;
            }
            catch (Exception ex)
            {
                connectionResult = false;
                SetError(ResHelper.GetString("Step2_ErrorConnectingDB") + AbstractResHelper.LINE_BREAK + ex.Message);
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
        }


        /// <summary>
        /// Asynchronously verifies connection credentials, database version, license.
        /// </summary>
        private Task<bool> ValidateDatabaseAsync()
        {
            return Task.Run(() => ValidateDatabase());
        }


        /// <summary>
        /// Verifies connection credentials, database version, license.
        /// </summary>
        private Task<bool> ValidateDatabase()
        {
            TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();
            bool validationResult = false;
            try
            {
                // Change message
                Wizard.SetWaitState(null, ResHelper.GetString("Step2_CheckingLicense"));

                using (InvokeHelper ih = new InvokeHelper(cmbDBName))
                {
                    ih.InvokeMethod(() => ImportProfile.SQLServerDatabase = cmbDBName.Text);
                }

                if (!string.IsNullOrEmpty(ImportProfile.SQLServerDatabase))
                {
                    var projectVersion = GetDatabaseVersion(CMSImport.ConnectionString);

                    if (String.IsNullOrEmpty(projectVersion))
                    {
                        SetError("Step2_OtherDB");
                    }
                    else
                    {
                        Wizard.SetWaitState(null, ResHelper.GetString("Step2_CheckingVersion"));

                        // Version has to correspond with DLL version
                        if (projectVersion.EqualsCSafe(ADWizard.SupportedVersion))
                        {
                            // Set new connection string
                            CMSImport.ConfigureApplicationSettings();

                            // Initialize application to enable usage of providers
                            CMSImport.CMSInit();

                            validationResult = true;
                            SetMessage("Step2_ConnectionToDBSuccessfull");
                        }
                        else
                        {
                            SetError(ResHelper.GetString("Step2_WrongVersion", ADWizard.SupportedVersion, projectVersion));
                        }
                    }
                }
                else
                {
                    SetError("Step2_SelectDatabase");
                }
            }
            catch (SqlException)
            {
                validationResult = false;
                SetError("Step2_OtherDB");
            }
            tcs.SetResult(validationResult);
            return tcs.Task;
        }


        private static string GetDatabaseVersion(string connectionString)
        {
            // Try to open connection
            SqlConnection sc = new SqlConnection(connectionString);

            // Find out whether CMS version is correct
            SqlCommand getVersion = new SqlCommand("SELECT [KeyValue] FROM [CMS_SettingsKey] WHERE [KeyName] = 'CMSDBVersion'", sc);

            using (SqlDataAdapter sda = new SqlDataAdapter(getVersion))
            {
                var ds = new DataSet();
                sda.Fill(ds);

                // Get current project version
                string projectVersion = null;
                if (!DataHelper.DataSourceIsEmpty(ds))
                {
                    projectVersion = ValidationHelper.GetString(ds.Tables[0].Rows[0]["KeyValue"], String.Empty);
                }

                return projectVersion;
            }
        }

        #endregion


        #region "Methods"

        /// <summary>
        /// Performs all necessary actions before validating connection.
        /// </summary>
        /// <returns>True if connecting should continue</returns>
        private bool PrepareConnectionValidation()
        {
            // Setup import profile
            ImportProfile.SQLServerAddress = cmbSQLServerAddress.Text;
            ImportProfile.SQLServerUsername = txtUsername.Text;
            ImportProfile.SQLServerPassword = txtPassword.Text;
            ImportProfile.SQLUseTrustedConnection = radUseTrustedConnection.Checked;

            if (ValidateFields())
            {
                // Change app.config
                CMSImport.ConfigureApplicationSettings();

                // Setup UI
                Wizard.SetWaitState(true, ResHelper.GetString("Step2_TryingToConnect"));
                Enabled = false;
                Wizard.ButtonNext.Enabled = false;
                return true;
            }
            else
            {
                return false;
            }
        }


        /// <summary>
        /// Enables / disables appropriate input fields
        /// </summary>
        private void SetupInputFields()
        {
            bool enabled = radUseSQLAccount.Checked;
            txtUsername.Enabled = enabled;
            txtPassword.Enabled = enabled;
            lblUsername.Enabled = enabled;
            lblPassword.Enabled = enabled;
            InvalidateDBs();
        }

        #endregion
    }
}