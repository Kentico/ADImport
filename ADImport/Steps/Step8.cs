using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Windows.Forms;

using CMS.Helpers;

using WinAppFoundation;

using WinFormsFramework;

namespace ADImport
{
    /// <summary>
    /// Step8 - Group selection.
    /// </summary>
    public partial class Step8 : AbstractStep
    {
        #region "Constants"

        // Column identifiers
        private const string COLUMN_SELECTED = "chkSelected";
        private const string COLUMN_GROUPNAME = "txtGroupname";
        private const string COLUMN_CMSGROUPNAME = "txtCMSGroupname";
        private const string COLUMN_GROUPGUID = "txtGroupGuid";

        #endregion


        #region "Constructors"

        /// <summary>
        /// Step8 constructor.
        /// </summary>
        public Step8()
            : this(null)
        {
        }

        /// <summary>
        /// Step8 constructor.
        /// </summary>
        public Step8(ADWizard wizard)
            : base(wizard)
        {
            InitializeComponent();
        }

        #endregion


        #region "Private variables"

        // Thread used for loading groups
        private Thread groupLoader = null;

        #endregion


        #region "Control events"

        private void Step8_ParentChanged(object sender, EventArgs e)
        {
            if (Parent != null)
            {
                grdGroups.Enabled = false;
                // Bind grid
                groupLoader = new Thread(BindGroups)
                {
                    IsBackground = true
                };
                groupLoader.Start();
            }
        }


        private void grdGroups_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (grdGroups.IsCurrentCellDirty)
            {
                // Commit cell change
                grdGroups.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }


        private void grdGroups_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (grdGroups.CurrentRow != null)
            {
                SetSelectedState(grdGroups.CurrentRow);
            }
        }


        private void btnFilter_Click(object sender, EventArgs e)
        {
            FilterGrid();
        }


        private void txtFilter_KeyPress(object sender, KeyPressEventArgs e)
        {
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
        /// Ends local threads.
        /// </summary>
        public override void EndThreads()
        {
            ThreadHelper.FinalizeThread(groupLoader);
        }


        private void FilterGrid()
        {
            string pattern = DataSetHelper.EscapeLikeValue(txtFilter.Text);
            ((DataTable)grdGroups.DataSource).DefaultView.RowFilter = String.Format("{0} LIKE '%{1}%' OR {2} LIKE '%{1}%'", COLUMN_GROUPNAME, pattern, COLUMN_CMSGROUPNAME);
        }


        private void BindGroups()
        {
            // Set waiting state
            SetWait(true, ResHelper.GetString("Step8_LoadingGroups"));

            DataTable groupsTable = null;
            if (DataHelper.DataSourceIsEmpty(grdGroups.DataSource))
            {
                // Create new datatable
                groupsTable = new DataTable();

                // Create columns
                DataColumn selectedCol = new DataColumn(COLUMN_SELECTED, typeof(Boolean));
                DataColumn groupNameCol = new DataColumn(COLUMN_GROUPNAME, typeof(String));
                DataColumn groupCMSNameCol = new DataColumn(COLUMN_CMSGROUPNAME, typeof(String));
                DataColumn groupGuidCol = new DataColumn(COLUMN_GROUPGUID, typeof(Guid));

                // Add columns to datatable
                groupsTable.Columns.Add(selectedCol);
                groupsTable.Columns.Add(groupNameCol);
                groupsTable.Columns.Add(groupCMSNameCol);
                groupsTable.Columns.Add(groupGuidCol);

                var possibleConflicts = new HashSet<string>();
                bool askForGuid = true;

                // Fill table with data
                foreach (IPrincipalObject group in ADProvider.GetAllGroups())
                {
                    // Look for possible code name conflicts and ask user if he wants to replace them with GUID
                    string codeName = group.GetCMSCodeName(true);

                    if (ImportProfile.RoleCodeNameFormat != CodenameFormat.Guid)
                    {
                        if (askForGuid && !possibleConflicts.Add(codeName))
                        {
                            if (MessageBox.Show(ResHelper.GetString("Step8_ConflictsText"), ResHelper.GetString("Step8_ConflictsCaption"), MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                            {
                                ImportProfile.RoleCodeNameFormat = CodenameFormat.Guid;
                            }

                            // Ask user only once
                            askForGuid = false;
                        }
                    }

                    // Create new row with the table schema
                    DataRow dr = groupsTable.NewRow();

                    // Add data to row
                    object[] dataRow = { group.IsSelected, group.Name, codeName, group.Identifier };
                    dr.ItemArray = dataRow;

                    // Add row to table
                    groupsTable.Rows.Add(dr);
                }

                // Apply sorting
                groupsTable.DefaultView.Sort = COLUMN_GROUPNAME;
            }
            else
            {
                groupsTable = (DataTable)grdGroups.DataSource;
                foreach (DataRow dr in groupsTable.Rows)
                {
                    string groupIdentifier = ValidationHelper.GetString(dr[COLUMN_GROUPGUID], string.Empty);
                    // Preselect users
                    bool selected = ImportProfile.Groups.Contains(ADProvider.ConvertToObjectIdentifier(groupIdentifier));
                    dr[COLUMN_SELECTED] = selected;
                    dr[COLUMN_CMSGROUPNAME] = ADProvider.GetPrincipalObject(groupIdentifier).GetCMSCodeName(true);
                }
            }

            using (InvokeHelper ih = new InvokeHelper(grdGroups))
            {
                ih.InvokeMethod(() => SetupGrid(groupsTable));
            }

            using (InvokeHelper ih = new InvokeHelper(grdGroups))
            {
                ih.InvokeMethod(() => grdGroups.Enabled = true);
            }

            // Turn off wait state
            SetWait(false, string.Empty);
        }


        private void SetupGrid(DataTable groupsTable)
        {
            // Bind table as a grid's datasource
            grdGroups.DataSource = groupsTable;
            if (!DataHelper.DataSourceIsEmpty(grdGroups.DataSource))
            {
                // Adjust columns
                DataGridViewColumn columnSelected = grdGroups.Columns[COLUMN_SELECTED];
                if (columnSelected != null)
                {
                    columnSelected.HeaderText = ResHelper.GetString("General_Import");
                    columnSelected.AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;
                }
                DataGridViewColumn columnGroupName = grdGroups.Columns[COLUMN_GROUPNAME];
                if (columnGroupName != null)
                {
                    columnGroupName.HeaderText = ResHelper.GetString("General_Groupname");
                    columnGroupName.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                    columnGroupName.ReadOnly = true;
                }
                DataGridViewColumn columnCMSGroupName = grdGroups.Columns[COLUMN_CMSGROUPNAME];
                if (columnCMSGroupName != null)
                {
                    columnCMSGroupName.HeaderText = ResHelper.GetString("General_CMSRolename");
                    columnCMSGroupName.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                    columnCMSGroupName.ReadOnly = true;
                    columnCMSGroupName.Visible = ImportProfile.RoleCodeNameFormat != CodenameFormat.Guid;
                }
                DataGridViewColumn columnGroupGuid = grdGroups.Columns[COLUMN_GROUPGUID];
                if (columnGroupGuid != null)
                {
                    columnGroupGuid.HeaderText = ResHelper.GetString("General_CMSRolename");
                    columnGroupGuid.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                    columnGroupGuid.ReadOnly = true;
                    columnGroupGuid.Visible = ImportProfile.RoleCodeNameFormat == CodenameFormat.Guid;
                }
            }
        }


        private void SetWait(bool wait, string message)
        {
            Wizard.SetWaitState(wait, message);

            using (InvokeHelper ih = new InvokeHelper(pnlFilter))
            {
                ih.InvokeMethod(() => pnlFilter.Enabled = !wait);
            }
        }


        private void SetSelectedState(DataGridViewRow row)
        {
            Guid groupGuid = (Guid)row.Cells[COLUMN_GROUPGUID].Value;
            bool selected = (bool)row.Cells[COLUMN_SELECTED].Value;
            // Check / decheck all occurrences of principal
            ADProvider.SetSelectedPrincipalObject(groupGuid, selected);
        }

        #endregion
    }
}