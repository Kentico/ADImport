using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

using CMS.Helpers;
using CMS.Base;
using CMS.SiteProvider;
using CMS.Membership;

using WinAppFoundation;

namespace ADImport
{
    /// <summary>
    /// Step9 - CMS Role selection.
    /// </summary>
    public partial class Step9 : AbstractStep
    {
        #region "Constants"

        // Column identifiers
        private const string COLUMN_SELECTED = "chkSelected";
        private const string COLUMN_ROLENAME = "txtRolename";
        private const string COLUMN_ROLEGUID = "txtRoleGUID";

        #endregion


        #region "Constructors"

        /// <summary>
        /// Step9 constructor.
        /// </summary>
        public Step9()
            : this(null)
        {
        }


        /// <summary>
        /// Step9 constructor.
        /// </summary>
        public Step9(ADWizard wizard)
            : base(wizard)
        {
            InitializeComponent();
        }

        #endregion


        #region "Control events"

        private void Step9_ParentChanged(object sender, EventArgs e)
        {
            if (Parent != null)
            {
                // Bind sites
                BindSites();
            }
        }


        private void cmbSites_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Bind roles
            BindRoles();
        }


        private void grdRoles_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (grdRoles.CurrentRow != null)
            {
                SetSelectedState(grdRoles.CurrentRow);
            }
        }


        private void grdRoles_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (grdRoles.IsCurrentCellDirty)
            {
                // Commit cell change
                grdRoles.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }


        private void btnFilter_Click(object sender, EventArgs e)
        {
            FilterGrid();
        }


        private void txtFilter_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Handle filtering
            if (e.KeyChar == (char)Keys.Return)
            {
                FilterGrid();
                e.Handled = true;
            }
        }


        private void txtFilter_Enter(object sender, EventArgs e)
        {
            Wizard.AcceptButton = null;
        }


        private void txtFilter_Leave(object sender, EventArgs e)
        {
            Wizard.AcceptButton = Wizard.ButtonNext;
        }

        #endregion


        #region "Methods"

        /// <summary>
        /// Binds site combobox
        /// </summary>
        private void BindSites()
        {
            if (ImportProfile.Sites.Count > 0)
            {
                DataSet sites = SiteInfoProvider.GetSites()
                    .WhereIn("SiteName", ImportProfile.Sites.Keys)
                    .OrderBy("SiteDisplayName")
                    .Columns("SiteID, SiteName, SiteDisplayName, SiteGUID");
                
                if (!DataHelper.DataSourceIsEmpty(sites))
                {
                    foreach (DataRow site in sites.Tables[0].Rows)
                    {
                        // Localize display names
                        site["SiteDisplayName"] = CMS.Helpers.ResHelper.LocalizeString(site["SiteDisplayName"].ToString());
                    }
                    cmbSites.DataSource = sites.Tables[0];
                }
            }
        }


        private void SetSelectedState(DataGridViewRow row)
        {
            // Get selected state
            bool selected = (bool)row.Cells[COLUMN_SELECTED].Value;

            // Get guid of role
            Guid roleGuid = (Guid)row.Cells[COLUMN_ROLEGUID].Value;

            // Get site key
            string siteKey = cmbSites.SelectedValue.ToString().ToLowerCSafe();

            // If role is to be selected
            if (selected)
            {
                // If import profile doesn't contain site
                if (!ImportProfile.Sites.ContainsKey(siteKey))
                {
                    // Add site to import profile
                    ImportProfile.Sites.Add(siteKey, new List<Guid>());
                }

                // If site doesn't containt role
                if (!ImportProfile.Sites[siteKey].Contains(roleGuid))
                {
                    // Add role to site
                    ImportProfile.Sites[siteKey].Add(roleGuid);
                }
            }
            else
            {
                // If import profile contains site
                if (ImportProfile.Sites.ContainsKey(siteKey))
                {
                    // Remove role from site
                    ImportProfile.Sites[siteKey].Remove(roleGuid);
                }
            }
        }


        private void FilterGrid()
        {
            string pattern = DataSetHelper.EscapeLikeValue(txtFilter.Text);
            ((DataTable)grdRoles.DataSource).DefaultView.RowFilter = String.Format("{0} LIKE '%{1}%'", COLUMN_ROLENAME, pattern);
        }


        private void BindRoles()
        {
            // Create new datatable
            DataTable rolesTable = new DataTable();

            // Create columns in the datatable
            rolesTable.Columns.Add(new DataColumn(COLUMN_SELECTED, typeof(Boolean)));
            rolesTable.Columns.Add(new DataColumn(COLUMN_ROLENAME, typeof(String)));
            rolesTable.Columns.Add(new DataColumn(COLUMN_ROLEGUID, typeof(Guid)));

            if (cmbSites.SelectedValue != null)
            {
                SiteInfo si = SiteInfoProvider.GetSiteInfo(cmbSites.SelectedValue.ToString());
                if (si != null)
                {
                    DataSet roles = RoleInfoProvider.GetAllRoles(si.SiteID, false, false);
                    if (!DataHelper.DataSourceIsEmpty(roles))
                    {
                        foreach (DataRow role in roles.Tables[0].Rows)
                        {
                            // Determine preselection state
                            string siteName = si.SiteName.ToLowerCSafe();
                            Guid roleGuid = ValidationHelper.GetGuid(role["RoleGuid"], Guid.Empty);
                            bool checkRole = ImportProfile.Sites.ContainsKey(siteName) && ImportProfile.Sites[siteName].Contains(roleGuid);

                            // Create data for the row
                            object[] dataRow = { checkRole, role["RoleDisplayName"], roleGuid };

                            // Create new row with the table schema
                            DataRow dr = rolesTable.NewRow();
                            dr.ItemArray = dataRow;

                            // Add row to table
                            rolesTable.Rows.Add(dr);
                        }
                    }

                    // Apply sorting
                    rolesTable.DefaultView.Sort = COLUMN_ROLENAME;

                    // Bind table as a grid's datasource
                    grdRoles.DataSource = rolesTable;

                    if (!DataHelper.DataSourceIsEmpty(grdRoles.DataSource))
                    {
                        // Adjust columns
                        DataGridViewColumn columnSelected = grdRoles.Columns[COLUMN_SELECTED];
                        if (columnSelected != null)
                        {
                            columnSelected.HeaderText = ResHelper.GetString("General_Assign");
                            columnSelected.AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;
                        }

                        DataGridViewColumn columnRoleName = grdRoles.Columns[COLUMN_ROLENAME];
                        if (columnRoleName != null)
                        {
                            columnRoleName.HeaderText = ResHelper.GetString("General_Role");
                            columnRoleName.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                            columnRoleName.ReadOnly = true;
                        }

                        DataGridViewColumn columnRoleGuid = grdRoles.Columns[COLUMN_ROLEGUID];
                        if (columnRoleGuid != null)
                        {
                            columnRoleGuid.Visible = false;
                        }
                    }
                }
            }
        }

        #endregion
    }
}