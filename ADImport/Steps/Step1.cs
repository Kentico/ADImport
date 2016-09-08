using System;
using System.Threading.Tasks;
using System.Windows.Forms;

using CMS.IO;

namespace ADImport
{
    /// <summary>
    /// Step1 - Choosing installation profile.
    /// </summary>
    public partial class Step1 : AbstractStep
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
        /// Step1 constructor.
        /// </summary>
        public Step1()
            : this(null)
        {
        }


        /// <summary>
        /// Step1 constructor.
        /// </summary>
        public Step1(ADWizard wizard)
            : base(wizard)
        {
            InitializeComponent();
            SetupFileBrowser();
        }

        #endregion


        #region "Control events"

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(txtImportProfile.Text))
            {
                // Set initial directory
                openImportProfile.InitialDirectory = Application.StartupPath;
            }
            else
            {
                try
                {
                    FileInfo importProfile = FileInfo.New(txtImportProfile.Text);
                    // Try to set initial directory
                    openImportProfile.InitialDirectory = Directory.Exists(importProfile.DirectoryName) ? importProfile.DirectoryName : Application.StartupPath;

                    // Set initial filename
                    openImportProfile.FileName = importProfile.Name;
                }
                catch (Exception)
                {
                    // Set initial directory
                    openImportProfile.InitialDirectory = Application.StartupPath;
                }
            }

            // Set file filter
            openImportProfile.Filter = ImportProfile.FileFilter;

            // If dialog was confirmed
            if (openImportProfile.ShowDialog() == DialogResult.OK)
            {
                txtImportProfile.Text = openImportProfile.FileName;
            }
        }


        private void radNewImportProfile_CheckedChanged(object sender, EventArgs e)
        {
            SetupFileBrowser();
        }


        private void radExistingImportProfile_CheckedChanged(object sender, EventArgs e)
        {
            SetupFileBrowser();
        }

        #endregion


        #region "Methods"

        private void SetupFileBrowser()
        {
            pnlOpenProfile.Enabled = radExistingImportProfile.Checked;
        }


        /// <summary>
        /// Check if current step is valid.
        /// </summary>
        public override Task<bool> IsValid()
        {
            bool validationResult = radNewImportProfile.Checked;

            string permissionsCheckResult = ADProvider.CheckPermissions();
            if (!string.IsNullOrEmpty(permissionsCheckResult))
            {
                SetError(permissionsCheckResult);
                validationResult = false;
            }
            else
            {
                if (radExistingImportProfile.Checked)
                {
                    if (!string.IsNullOrEmpty(txtImportProfile.Text))
                    {
                        // Verify that file exists
                        if (!File.Exists(txtImportProfile.Text))
                        {
                            SetError("Error_FileNotFound");
                            validationResult = false;
                        }
                        else
                        {
                            validationResult = true;
                        }
                    }
                    else
                    {
                        SetError("Error_SelectPath");
                        validationResult = false;
                    }
                    // Setup import profile
                    if (validationResult)
                    {
                        string validationError = ImportProfile.InitializeImportProfile(txtImportProfile.Text);
                        if(!String.IsNullOrEmpty(validationError))
                        {
                            SetError(ResHelper.GetString("Error_ProfileIsNotValid") + Environment.NewLine + validationError);
                            validationResult = false;
                        }
                        else
                        {
                            lblMessage.Visible = false;
                        }
                    }
                }
                else
                {
                    lblMessage.Visible = false;
                }
            }

            return Task.FromResult(validationResult);
        }

        #endregion
    }
}