using System;
using System.Data;
using System.Threading;
using System.Windows.Forms;

using CMS.Helpers;

using WinAppFoundation;

using WinFormsFramework;

namespace ADImport
{
    /// <summary>
    /// Step7 - User selection.
    /// </summary>
    public partial class Step7 : AbstractStep
    {
        #region "Constants"

        // Column identifiers
        private const string COLUMN_SELECTED = "chkSelected";
        private const string COLUMN_DISPLAYNAME = "txtDisplayName";
        private const string COLUMN_USERNAME = "txtUsername";
        private const string COLUMN_USERGUID = "txtUserGuid";

        #endregion


        #region "Constructors"

        /// <summary>
        /// Step7 constructor.
        /// </summary>
        public Step7()
            : this(null)
        {
        }

        /// <summary>
        /// Step7 constructor.
        /// </summary>
        public Step7(ADWizard wizard)
            : base(wizard)
        {
            InitializeComponent();
        }

        #endregion


        #region "Private variables"

        // Thread used for loading users
        private Thread userLoader = null;

        #endregion


        #region "Control events"

        private void Step7_ParentChanged(object sender, EventArgs e)
        {
            if (Parent != null)
            {
                grdUsers.Enabled = false;
                // Bind grid
                userLoader = new Thread(BindUsers);
                userLoader.IsBackground = true;
                userLoader.Start();
            }
        }


        private void btnFilter_Click(object sender, EventArgs e)
        {
            FilterGrid();
        }


        private void grdUsers_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (grdUsers.IsCurrentCellDirty)
            {
                // Commit cell change
                grdUsers.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }


        private void grdUsers_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (grdUsers.CurrentRow != null)
            {
                SetSelectedState(grdUsers.CurrentRow);
            }
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
            ThreadHelper.FinalizeThread(userLoader);
        }


        private void FilterGrid()
        {
            string pattern = DataSetHelper.EscapeLikeValue(txtFilter.Text);
            ((DataTable)grdUsers.DataSource).DefaultView.RowFilter = COLUMN_USERNAME + " LIKE '%" + pattern + "%' OR " + COLUMN_DISPLAYNAME + " LIKE '%" + pattern + "%'";
        }


        private void BindUsers()
        {
            // Set waiting state
            SetWait(true, ResHelper.GetString("Step7_LoadingUsers"));

            DataTable usersTable = null;
            if (DataHelper.DataSourceIsEmpty(grdUsers.DataSource))
            {
                // Create new datatable
                usersTable = new DataTable();

                // Create columns
                DataColumn selectedCol = new DataColumn(COLUMN_SELECTED, typeof(Boolean));
                DataColumn displayNameCol = new DataColumn(COLUMN_DISPLAYNAME, typeof(String));
                DataColumn userNameCol = new DataColumn(COLUMN_USERNAME, typeof(String));
                DataColumn userGuidCol = new DataColumn(COLUMN_USERGUID, typeof(Guid));

                // Add columns to datatable
                usersTable.Columns.Add(selectedCol);
                usersTable.Columns.Add(displayNameCol);
                usersTable.Columns.Add(userNameCol);
                usersTable.Columns.Add(userGuidCol);

                // Fill table with data
                foreach (IPrincipalObject user in ADProvider.GetAllUsers())
                {
                    // Create new row with the table schema
                    DataRow dr = usersTable.NewRow();

                    // Get user's identifier
                    object userIdentifier = user.Identifier;

                    // Get the display name of user
                    string displayName = !string.IsNullOrEmpty(user.DisplayName) ? user.DisplayName : user.Name;

                    // Add data to row
                    object[] dataRow = { user.IsSelected, displayName, user.GetCMSCodeName(true), userIdentifier };
                    dr.ItemArray = dataRow;

                    // Add row to table
                    usersTable.Rows.Add(dr);
                }

                // Apply sorting
                usersTable.DefaultView.Sort = COLUMN_DISPLAYNAME;
            }
            else
            {
                usersTable = (DataTable)grdUsers.DataSource;
                foreach (DataRow dr in usersTable.Rows)
                {
                    string userIdentifier = ValidationHelper.GetString(dr[COLUMN_USERGUID], string.Empty);
                    // Preselect users
                    bool selected = ImportProfile.Users.Contains(ADProvider.ConvertToObjectIdentifier(userIdentifier));
                    dr[COLUMN_SELECTED] = selected;
                    dr[COLUMN_USERNAME] = ADProvider.GetPrincipalObject(userIdentifier).GetCMSCodeName(true);
                }
            }

            using (InvokeHelper ih = new InvokeHelper(grdUsers))
            {
                ih.InvokeMethod(() => SetupGrid(usersTable));
            }

            using (InvokeHelper ih = new InvokeHelper(grdUsers))
            {
                ih.InvokeMethod(() => grdUsers.Enabled = true);
            }
            // Turn off wait state
            SetWait(false, string.Empty);
        }


        private void SetupGrid(DataTable usersTable)
        {
            // Bind table as a grid's datasource
            grdUsers.DataSource = usersTable;

            if (!DataHelper.DataSourceIsEmpty(grdUsers.DataSource))
            {
                // Adjust columns
                DataGridViewColumn columnSelected = grdUsers.Columns[COLUMN_SELECTED];
                if (columnSelected != null)
                {
                    columnSelected.HeaderText = ResHelper.GetString("General_Import");
                    columnSelected.AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;
                }
                DataGridViewColumn columnDisplayName = grdUsers.Columns[COLUMN_DISPLAYNAME];
                if (columnDisplayName != null)
                {
                    columnDisplayName.HeaderText = ResHelper.GetString("General_DisplayName");
                    columnDisplayName.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                    columnDisplayName.ReadOnly = true;
                }
                DataGridViewColumn columnUserName = grdUsers.Columns[COLUMN_USERNAME];
                if (columnUserName != null)
                {
                    columnUserName.HeaderText = ResHelper.GetString("General_CMSUsername");
                    columnUserName.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                    columnUserName.ReadOnly = true;
                }
                DataGridViewColumn columnUserGuid = grdUsers.Columns[COLUMN_USERGUID];
                if (columnUserGuid != null)
                {
                    columnUserGuid.Visible = false;
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
            Guid userGuid = (Guid)row.Cells[COLUMN_USERGUID].Value;
            bool selected = (bool)row.Cells[COLUMN_SELECTED].Value;
            // (Un)check all occurrences of principal
            ADProvider.SetSelectedPrincipalObject(userGuid, selected);
        }

        #endregion
    }
}