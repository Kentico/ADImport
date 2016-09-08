using System;
using System.Security;
using System.Security.Permissions;
using System.Threading.Tasks;
using System.Windows.Forms;

using CMS.IO;
using CMS.Base;

using WinAppFoundation;

namespace ADImport
{
    /// <summary>
    /// Step10 - Import and/or save profile.
    /// </summary>
    public partial class Step10 : AbstractStep
    {
        #region "Constants"

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

        #endregion


        #region "Constructors"

        /// <summary>
        /// Step10 constructor.
        /// </summary>
        public Step10()
            : this(null)
        {
        }

        /// <summary>
        /// Step10 constructor.
        /// </summary>
        public Step10(ADWizard wizard)
            : base(wizard)
        {
            InitializeComponent();
        }

        #endregion


        #region "Control events"

        private void Step10_ParentChanged(object sender, EventArgs e)
        {
            if (Parent != null)
            {
                // Preselect values
                chkImportNow.Checked = ImportProfile.ImportNow;
                pnlSave.Enabled = chkSaveImportProfile.Checked;

                string profilePath = ImportProfile.ImportProfileFilename;
                if (string.IsNullOrEmpty(profilePath))
                {
                    profilePath = Application.StartupPath + "\\importProfile" + ImportProfile.PROFILE_EXTENSION;
                }
                txtPath.Text = profilePath;
                SetNextButton();

                // Handle events
                Wizard.StepLoadedEvent += Wizard_StepLoadedEvent;
            }
        }


        private void Wizard_StepLoadedEvent(object sender, EventArgs e)
        {
            if (IsStepActive)
            {
                SetNextButton();
            }
        }


        private void chkImportNow_CheckedChanged(object sender, EventArgs e)
        {
            SetNextButton();
            ImportProfile.ImportNow = chkImportNow.Checked;
        }


        private void chkSaveImportProfile_CheckedChanged(object sender, EventArgs e)
        {
            SetNextButton();
            pnlSave.Enabled = chkSaveImportProfile.Checked;
            ImportProfile.SaveImportProfile = chkSaveImportProfile.Checked;
        }


        private void btnBrowse_Click(object sender, EventArgs e)
        {
            try
            {
                FileInfo logFile = FileInfo.New(txtPath.Text);
                // Try to set initial directory
                saveImportProfile.InitialDirectory = logFile.DirectoryName;
                // Set initial filename
                saveImportProfile.FileName = logFile.Name;
            }
            catch (Exception)
            {
                // Set initial directory
                saveImportProfile.InitialDirectory = Application.StartupPath;
                // Set initial filename
                saveImportProfile.FileName = ImportProfile.ImportProfileFilename;
            }

            // Set file filter
            saveImportProfile.Filter = ImportProfile.FileFilter;

            // If dialog was confirmed
            if (saveImportProfile.ShowDialog() == DialogResult.OK)
            {
                txtPath.Text = saveImportProfile.FileName;
                ImportProfile.ImportProfileFilename = saveImportProfile.FileName;
            }
        }

        #endregion


        #region "Methods"

        /// <summary>
        /// Check if current step is valid.
        /// </summary>
        public override Task<bool> IsValid()
        {
            bool result = false;

            if (chkSaveImportProfile.Checked)
            {
                if (string.IsNullOrEmpty(txtPath.Text))
                {
                    SetError("Error_PathRequired");
                }
                else if (!txtPath.Text.EndsWithCSafe(ImportProfile.PROFILE_EXTENSION))
                {
                    SetError("Error_WrongExtensionXML");
                }
                else
                {
                    try
                    {
                        string validationResult = FileSystemHelper.ValidatePath(txtPath.Text);
                        if (!string.IsNullOrEmpty(validationResult))
                        {
                            SetError(validationResult);
                        }
                        else
                        {
                            FileInfo fi = FileInfo.New(txtPath.Text);

                            // If filename is not specified or filename is already a directory
                            if (string.IsNullOrEmpty(fi.Name))
                            {
                                SetError("Error_NoFilename");
                            }
                            else
                            {
                                if (((fi.Attributes & FileAttributes.Directory) == FileAttributes.Directory) && (Convert.ToInt32(fi.Attributes) != -1))
                                {
                                    SetError("Error_PathIsOccupied");
                                }
                                else
                                {
                                    // If directory doesn't exist
                                    if (fi.Directory != null)
                                    {
                                        if (!fi.Directory.Exists)
                                        {
                                            SetError("Error_PathNotExist");
                                        }
                                        else
                                        {
                                            // Check write permissions
                                            FileIOPermission fileIoPermission = new FileIOPermission(FileIOPermissionAccess.Write, fi.FullName);
                                            PermissionSet permSet = new PermissionSet(PermissionState.None);
                                            permSet.AddPermission(fileIoPermission);
                                            bool isGranted = permSet.IsSubsetOf(AppDomain.CurrentDomain.PermissionSet);

                                            if (!isGranted)
                                            {
                                                SetError("Error_NoWritePermissions");
                                            }
                                            else
                                            {
                                                result = true;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        SetError("Error_PathMissingDirectory");
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception)
                    {
                        SetError("Error_PathNotValid");
                        result = false;
                    }
                }
            }
            else
            {
                result = true;
            }

            lblMessage.Visible = !result;
            ImportProfile.ImportProfileFilename = txtPath.Text;
            return Task.FromResult(result);
        }


        /// <summary>
        /// Ensures that at least one action have to be selected before continue.
        /// </summary>
        protected bool SetNextButton()
        {
            bool result = chkImportNow.Checked || chkSaveImportProfile.Checked;
            Wizard.ButtonNext.Enabled = result;
            return result;
        }

        #endregion
    }
}