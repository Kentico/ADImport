using System;
using System.Collections.Generic;
using System.Data;
using System.Collections;
using System.Security;
using System.Security.Permissions;
using System.Threading.Tasks;
using System.Windows.Forms;

using CMS.Helpers;
using CMS.IO;
using CMS.Base;
using CMS.DataEngine;
using CMS.SiteProvider;

using WinAppFoundation;

namespace ADImport
{
    /// <summary>
    /// Step4 - Import settings.
    /// </summary>
    public partial class Step4 : AbstractStep
    {
        #region "Constants"

        // Column identifiers
        private const string COLUMN_SELECTED = "chkSelected";
        private const string COLUMN_SITENAME = "txtSitename";
        private const string COLUMN_CODENAME = "txtCodename";

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
        /// Step4 constructor.
        /// </summary>
        public Step4()
            : this(null)
        {
        }

        /// <summary>
        /// Step4 constructor.
        /// </summary>
        public Step4(ADWizard wizard)
            : base(wizard)
        {
            InitializeComponent();
        }

        #endregion


        #region "Control events"

        private void Step4_Load(object sender, EventArgs e)
        {
            // Localize step
            toolTipOnlySelectedAndNew.SetToolTip(radOnlySelectedAndNewGroups, ResHelper.GetString("Step4_UpdateSelectedImportNew"));
            toolTipOnlySelectedAndNew.SetToolTip(radOnlySelectedAndNewUsers, ResHelper.GetString("Step4_UpdateSelectedImportNew"));
            toolTipImportOnlyUsersFromSelectedRoles.SetToolTip(chkImportNewUsersOnlyFromSelectedRoles, ResHelper.GetString("Step4_ToolTipImportNewUsersOnlyFromSelectedRoles"));
        }


        private void Step4_ParentChanged(object sender, EventArgs e)
        {
            if (Parent != null)
            {
                // Bind sites
                BindSites();

                txtLogFile.Text = ImportProfile.LogPath;

                // Initialize radio buttons
                switch (ImportProfile.ImportUsersType)
                {
                    case ImportType.All:
                        radAllUsers.Checked = true;
                        break;

                    case ImportType.Selected:
                        radOnlySelectedUsers.Checked = true;
                        break;

                    case ImportType.UpdateSelectedImportNew:
                        radOnlySelectedAndNewUsers.Checked = true;
                        break;
                }
                switch (ImportProfile.ImportRolesType)
                {
                    case ImportType.All:
                        radAllGroups.Checked = true;
                        break;

                    case ImportType.Selected:
                        radOnlySelectedGroups.Checked = true;
                        break;

                    case ImportType.UpdateSelectedImportNew:
                        radOnlySelectedAndNewGroups.Checked = true;
                        break;
                }
                // Initialize checkboxes
                chkUpdateUserRoleData.Checked = ImportProfile.UpdateObjectData;
                chkImportNewUsersOnlyFromSelectedRoles.Checked = ImportProfile.ImportNewUsersOnlyFromSelectedRoles;
                chkImportOnlyUsersFromSelectedRoles.Checked = ImportProfile.ImportUsersOnlyFromSelectedRoles;
                chkUpdateMemberships.Checked = ImportProfile.UpdateMemberships;
                chkDeleteOldObjects.Checked = ImportProfile.DeleteNotExistingObjects;
                chkCreateLog.Checked = ImportProfile.LogImportProcess;

                // Initialize path selector
                SetupPathSelector();
            }
        }


        private void radAllUsers_CheckedChanged(object sender, EventArgs e)
        {
            // Update import profile
            SetUserImportType();
        }


        private void radOnlySelectedUsers_CheckedChanged(object sender, EventArgs e)
        {
            // Update import profile
            SetUserImportType();
        }


        private void radOnlySelectedAndNewUsers_CheckedChanged(object sender, EventArgs e)
        {
            // Update import profile
            SetUserImportType();
        }


        private void radAllGroups_CheckedChanged(object sender, EventArgs e)
        {
            // Update import profile
            SetRoleImportType();
        }


        private void radOnlySelectedGroups_CheckedChanged(object sender, EventArgs e)
        {
            // Update import profile
            SetRoleImportType();
        }


        private void radOnlySelectedAndNewGroups_CheckedChanged(object sender, EventArgs e)
        {
            // Update import profile
            SetRoleImportType();
        }


        private void grdSites_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (grdSites.CurrentRow != null)
            {
                SetSelectedState(grdSites.CurrentRow);
            }
        }


        private void grdSites_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (grdSites.IsCurrentCellDirty)
            {
                // Commit cell change
                grdSites.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }


        private void btnBrowse_Click(object sender, EventArgs e)
        {
            try
            {
                FileInfo logFile = FileInfo.New(txtLogFile.Text);
                // Try to set initial directory
                saveLogFile.InitialDirectory = logFile.DirectoryName;
                // Set initial filename
                saveLogFile.FileName = logFile.Name;
            }
            catch (Exception)
            {
                // Set initial directory
                saveLogFile.InitialDirectory = Application.StartupPath;
                // Set initial filename
                saveLogFile.FileName = ImportProfile.LogPath;
            }


            // Set file filter
            saveLogFile.Filter = ResHelper.GetString("General_LOGFiles") + " (*.log)|*.log";

            // If dialog was confirmed
            if (saveLogFile.ShowDialog() == DialogResult.OK)
            {
                txtLogFile.Text = saveLogFile.FileName;
            }
        }


        private void chkCreateLog_CheckedChanged(object sender, EventArgs e)
        {
            // Update import profile
            ImportProfile.LogImportProcess = chkCreateLog.Checked;
            SetupPathSelector();
        }


        private void chkUpdateUserRoleData_CheckedChanged(object sender, EventArgs e)
        {
            // Update import profile
            ImportProfile.UpdateObjectData = chkUpdateUserRoleData.Checked;
        }


        private void chkUpdateMemberships_CheckedChanged(object sender, EventArgs e)
        {
            // Update import profile
            ImportProfile.UpdateMemberships = chkUpdateMemberships.Checked;
        }


        private void chkDeleteOldObjects_CheckedChanged(object sender, EventArgs e)
        {
            // Update import profile
            ImportProfile.DeleteNotExistingObjects = chkDeleteOldObjects.Checked;
        }


        private void chkImportNewUsersOnlyFromSelectedRoles_CheckedChanged(object sender, EventArgs e)
        {
            // Update import profile
            ImportProfile.ImportNewUsersOnlyFromSelectedRoles = chkImportNewUsersOnlyFromSelectedRoles.Checked;
        }

        private void chkImportOnlyUsersFromSelectedRoles_CheckedChanged(object sender, EventArgs e)
        {
            // Update import profile
            ImportProfile.ImportUsersOnlyFromSelectedRoles = chkImportOnlyUsersFromSelectedRoles.Checked;
        }

        #endregion


        #region "Methods"

        /// <summary>
        /// Check if current step is valid.
        /// </summary>
        public override Task<bool> IsValid()
        {
            var result = false;
            if (chkCreateLog.Checked)
            {
                if (txtLogFile.Text == string.Empty)
                {
                    SetError("Error_PathRequired");
                }
                else
                {
                    try
                    {
                        string validationResult = FileSystemHelper.ValidatePath(txtLogFile.Text);
                        if (!string.IsNullOrEmpty(validationResult))
                        {
                            SetError(validationResult);
                        }
                        else
                        {
                            FileInfo fi = FileInfo.New(txtLogFile.Text);

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
            ImportProfile.LogPath = txtLogFile.Text;
            return Task.FromResult(result);
        }


        private void SetupPathSelector()
        {
            pnlSaveLogFile.Enabled = chkCreateLog.Checked;
        }


        private void BindSites()
        {
            // Create new datatable
            DataTable sitesTable = new DataTable();

            // Create columns
            DataColumn selectedCol = new DataColumn(COLUMN_SELECTED, typeof(Boolean));
            DataColumn siteNameCol = new DataColumn(COLUMN_SITENAME, typeof(String));
            DataColumn codeNameCol = new DataColumn(COLUMN_CODENAME, typeof(String));

            // Add columns to datatable
            sitesTable.Columns.Add(selectedCol);
            sitesTable.Columns.Add(siteNameCol);
            sitesTable.Columns.Add(codeNameCol);

            // Bind DataGridView to CMS sites
            DataSet sites = SiteInfoProvider.GetSites()
                .OrderBy("SiteDisplayName")
                .Columns("SiteDisplayName, SiteName");

            bool sitesAvailable = !DataHelper.DataSourceIsEmpty(sites);

            pnlImportGroups.Enabled = sitesAvailable;
            if (sitesAvailable)
            {
                // Remove non-existing sites
                IEnumerator sitesEnumerator = ImportProfile.Sites.GetEnumerator();
                List<string> removeSites = new List<string>();
                while (sitesEnumerator.MoveNext())
                {
                    // Get site
                    if (sitesEnumerator.Current != null)
                    {
                        KeyValuePair<string, List<Guid>> importedSite = (KeyValuePair<string, List<Guid>>)sitesEnumerator.Current;

                        // Clear hash tables
                        AbstractProvider.ClearHashtables(SiteInfo.OBJECT_TYPE, true);
                        
                        // Get info object
                        SiteInfo site = SiteInfoProvider.GetSiteInfo(importedSite.Key);

                        // If site is not present
                        if (site == null)
                        {
                            // Add to removal list
                            removeSites.Add(importedSite.Key);
                        }
                    }
                }
                foreach (string siteToRemove in removeSites)
                {
                    ImportProfile.Sites.Remove(siteToRemove.ToLowerCSafe());
                }
                foreach (DataRow site in sites.Tables[0].Rows)
                {
                    // Create new row with the table schema
                    DataRow dr = sitesTable.NewRow();

                    // Preselect sites
                    bool checkSite = (ImportProfile.Sites.ContainsKey(site["SiteName"].ToString().ToLowerCSafe()));

                    // Localize display name
                    string siteDisplayName = CMS.Helpers.ResHelper.LocalizeString(site["SiteDisplayName"].ToString());

                    // Add data to row
                    object[] dataRow = { checkSite, siteDisplayName, site["SiteName"] };
                    dr.ItemArray = dataRow;

                    // Add row to table
                    sitesTable.Rows.Add(dr);
                }
                // Bind table as a grid's datasource
                grdSites.DataSource = sitesTable;

                // Adjust columns
                DataGridViewColumn columnSelected = grdSites.Columns[COLUMN_SELECTED];
                if (columnSelected != null)
                {
                    columnSelected.HeaderText = ResHelper.GetString("General_Import");
                    columnSelected.AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;
                }
                DataGridViewColumn columnSiteName = grdSites.Columns[COLUMN_SITENAME];
                if (columnSiteName != null)
                {
                    columnSiteName.HeaderText = ResHelper.GetString("General_Sitename");
                    columnSiteName.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                    columnSiteName.ReadOnly = true;
                }
                DataGridViewColumn columnCodeName = grdSites.Columns[COLUMN_CODENAME];
                if (columnCodeName != null)
                {
                    columnCodeName.Visible = false;
                }
            }
            else
            {
                // No sites to import
                ImportProfile.Sites.Clear();
            }
        }


        /// <summary>
        /// Update user import type in import profile
        /// </summary>
        private void SetUserImportType()
        {
            if (radAllUsers.Checked)
            {
                ImportProfile.ImportUsersType = ImportType.All;
            }
            if (radOnlySelectedUsers.Checked)
            {
                ImportProfile.ImportUsersType = ImportType.Selected;
            }
            if (radOnlySelectedAndNewUsers.Checked)
            {
                ImportProfile.ImportUsersType = ImportType.UpdateSelectedImportNew;
            }
            ADProvider.SetSelectedStates(ImportProfile.ImportUsersType, ADProvider.UserIdentifier);
        }


        /// <summary>
        /// Update role import type in import profile
        /// </summary>
        private void SetRoleImportType()
        {
            if (radAllGroups.Checked)
            {
                ImportProfile.ImportRolesType = ImportType.All;
            }
            if (radOnlySelectedGroups.Checked)
            {
                ImportProfile.ImportRolesType = ImportType.Selected;
            }
            if (radOnlySelectedAndNewGroups.Checked)
            {
                ImportProfile.ImportRolesType = ImportType.UpdateSelectedImportNew;
            }
            ADProvider.SetSelectedStates(ImportProfile.ImportRolesType, ADProvider.GroupIdentifier);
        }


        private static void SetSelectedState(DataGridViewRow row)
        {
            // Get selected state
            bool selected = (bool)row.Cells[COLUMN_SELECTED].Value;

            // Get site name
            string siteName = row.Cells[COLUMN_CODENAME].Value.ToString().ToLowerCSafe();

            // If import profile contains site
            if (ImportProfile.Sites.ContainsKey(siteName))
            {
                // If site is to be deselected
                if (!selected)
                {
                    // Remove site from import profile
                    ImportProfile.Sites.Remove(siteName);
                }
            }
            else
            {
                // If site is to be selected
                if (selected)
                {
                    // Add site to import profile
                    ImportProfile.Sites.Add(siteName, new List<Guid>());
                }
            }
        }

        #endregion
    }
}