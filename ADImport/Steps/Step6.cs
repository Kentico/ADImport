using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Windows.Forms;

using CMS.Helpers;

using WinFormsFramework;

namespace ADImport
{
    /// <summary>
    /// Step6 - Group tree.
    /// </summary>
    public partial class Step6 : AbstractStep
    {
        #region "Constants"

        // Column identifiers
        private const string COLUMN_SELECTED = "chkSelected";
        private const string COLUMN_DISPLAYNAME = "txtDisplayName";
        private const string COLUMN_USERNAME = "txtUsername";
        private const string COLUMN_USERGUID = "txtUserGuid";

        #endregion


        #region "Private variables"

        // Thread used for loading AD users and groups
        private Thread treeLoader = null;

        // Thread used for mass node (de)selection
        private Thread treeMassActions = null;

        // Thread used for group node selection
        private Thread treeSelector = null;

        // Determines whether importing of groups is enabled
        private bool importGroups = false;

        #endregion


        #region "Constructors"

        /// <summary>
        /// Step6 constructor.
        /// </summary>
        public Step6()
            : this(null)
        {
        }

        /// <summary>
        /// Step6 constructor.
        /// </summary>
        public Step6(ADWizard wizard)
            : base(wizard)
        {
            InitializeComponent();
        }

        #endregion


        #region "Control events"

        private void Step6_Load(object sender, EventArgs e)
        {
            // Bind tree
            BindTree();

            // Sort groups in tree
            treeGroups.Sort();
        }


        private void treeGroups_AfterCheck(object sender, TreeViewEventArgs e)
        {
            // Check all occurrences of principal
            CheckAllSiblings(e.Node);
        }


        private void treeGroups_AfterSelect(object sender, TreeViewEventArgs e)
        {
            // End previous tree operations
            ThreadHelper.FinalizeThread(treeSelector);

            // Set waiting state
            SetWait(true, ResHelper.GetString("Step6_LoadingADStruct"));

            // Clear all rows in grid
            if (grdUsers.DataSource != null)
            {
                grdUsers.DataSource = null;
            }
            else
            {
                grdUsers.Rows.Clear();
            }

            // Load users asynchronously
            treeSelector = new Thread(() => LoadGroupUsers(e.Node.Name));
            treeSelector.IsBackground = true;
            treeSelector.Start();
        }


        private void treeGroups_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            // If there is only temporary node
            if ((e.Node.Nodes.Count == 1) && (e.Node.Nodes[0].Name == ADProvider.TempNodeID))
            {
                // Delete temporary node
                e.Node.Nodes.Clear();

                // Expand node
                ADProvider.LazyExpandGroupNode(e.Node, HandleContextMenuClick);
            }
        }


        /// <summary>
        /// Handles context menu operations.
        /// </summary>
        public void HandleContextMenuClick(object sender, EventArgs e)
        {
            ToolStripMenuItem button = (ToolStripMenuItem)sender;
            string[] values = button.Name.Split('|');
            if (values.Length == 2)
            {
                SelectionType selectionType = (SelectionType)Enum.Parse(typeof(SelectionType), values[0]);
                Guid groupGuid = ValidationHelper.GetGuid(values[1], Guid.Empty);
                switch (selectionType)
                {
                    case SelectionType.SelectAll:
                        ADProvider.PreselectGroups(groupGuid, true, false);
                        break;

                    case SelectionType.SelectAllRecursively:
                        ADProvider.PreselectGroups(groupGuid, true, true);
                        break;

                    case SelectionType.DeselectAll:
                        ADProvider.PreselectGroups(groupGuid, false, false);
                        break;

                    case SelectionType.DeselectAllRecursively:
                        ADProvider.PreselectGroups(groupGuid, false, true);
                        break;
                }
            }
        }


        private void btnSelectAllUsers_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in grdUsers.Rows)
            {
                row.Cells[COLUMN_SELECTED].Value = true;
            }
        }


        private void btnDeselectAllUsers_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in grdUsers.Rows)
            {
                row.Cells[COLUMN_SELECTED].Value = false;
            }
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
            if (e.RowIndex > -1)
            {
                SetSelectedState(grdUsers.Rows[e.RowIndex]);
            }
        }


        private void btnSelectAllGroups_Click(object sender, EventArgs e)
        {
            SetWait(true, ResHelper.GetString("Step6_SelectingNodes"));
            treeMassActions = new Thread(SelectAllGroups);
            treeMassActions.IsBackground = true;
            treeMassActions.Start(true);
        }


        private void btnDeselectAllGroups_Click(object sender, EventArgs e)
        {
            SetWait(true, ResHelper.GetString("Step6_DeselectingNodes"));
            treeMassActions = new Thread(SelectAllGroups);
            treeMassActions.IsBackground = true;
            treeMassActions.Start(false);
        }


        private void Step6_ParentChanged(object sender, EventArgs e)
        {
            if (Parent != null)
            {
                // Deselect node
                treeGroups.SelectedNode = null;

                // Unbind data source
                grdUsers.DataSource = null;

                // Determine whether the group importing will be allowed
                importGroups = (ImportProfile.Sites.Count != 0);

                // Setup controls
                SetupSelectionControls();

                // Show or hide checkboxes
                treeGroups.CheckBoxes = importGroups;
            }
        }

        #endregion


        #region "Methods"

        /// <summary>
        /// Ends local threads.
        /// </summary>
        public override void EndThreads()
        {
            ThreadHelper.FinalizeThread(treeLoader);
            ThreadHelper.FinalizeThread(treeMassActions);
            ThreadHelper.FinalizeThread(treeSelector);
        }


        private void SelectAllGroups(object selected)
        {
            ADProvider.PreselectGroups(null, (bool)selected, true);
            SetWait(false, string.Empty);
        }


        private void SetWait(bool wait, string message)
        {
            Wizard.SetWaitState(wait, message);

            // Enable/disable mass actions buttons
            using (InvokeHelper ih = new InvokeHelper(pnlButtons))
            {
                ih.InvokeMethod(() => pnlButtons.Enabled = !wait);
            }

            // Enable/disable tree with groups
            using (InvokeHelper ih = new InvokeHelper(treeGroups))
            {
                ih.InvokeMethod(() =>
                {
                    treeGroups.Enabled = !wait;

                    if (!wait)
                    {
                        // Return focus to previously selected node
                        treeGroups.Focus();
                    }
                });
            }
        }


        private void CheckAllSiblings(TreeNode node)
        {
            // Get identifier
            Guid? identifier = new Guid(node.Name);

            // Get checked state
            bool checkedState = node.Checked;

            // Check / uncheck all occurrences of principal
            ADProvider.SetSelectedPrincipalObject(identifier, checkedState);
        }


        private void SetSelectedState(DataGridViewRow row)
        {
            // Get GUID from row
            Guid userGuid = (Guid)row.Cells[COLUMN_USERGUID].Value;

            // Get selected state from row
            bool selected = (bool)row.Cells[COLUMN_SELECTED].Value;

            // Set selected state of principal
            ADProvider.SetSelectedPrincipalObject(userGuid, selected);
        }


        private void SetupSelectionControls()
        {
            using (InvokeHelper ih = new InvokeHelper(treeGroups))
            {
                ih.InvokeMethod(delegate
                {
                    // Enable or disable context menus and buttons
                    ADProvider.SetContextMenuEnabledState(importGroups);
                    btnSelectAllGroups.Enabled = importGroups;
                    btnDeselectAllGroups.Enabled = importGroups;
                });
            }
        }


        private void LoadNodeStructure()
        {
            // Set waiting state
            Wizard.SetWaitState(true, ResHelper.GetString("Step6_LoadingADStruct"));

            // Load node structure into the tree view
            ADProvider.LazyAddRootGroupNodes(treeGroups, HandleContextMenuClick);

            // Setup controls
            SetupSelectionControls();

            SetWait(false, string.Empty);
        }


        /// <summary>
        /// Loads users of given group.
        /// </summary>
        /// <param name="principalKey">Identifier (GUID) of group to load users from.</param>
        private void LoadGroupUsers(object principalKey)
        {
            DataTable usersTable = new DataTable();
            usersTable.Columns.Add(new DataColumn(COLUMN_SELECTED, typeof(Boolean)));
            usersTable.Columns.Add(new DataColumn(COLUMN_DISPLAYNAME, typeof(String)));
            usersTable.Columns.Add(new DataColumn(COLUMN_USERNAME, typeof(String)));
            usersTable.Columns.Add(new DataColumn(COLUMN_USERGUID, typeof(Guid)));

            // Get group based on node selected in tree
            IPrincipalObject group = ADProvider.GetPrincipalObject(principalKey);
            if (group != null)
            {
                // Get members of group
                List<IPrincipalObject> users = ADProvider.GetUsersOf(group);
                foreach (IPrincipalObject user in users)
                {
                    // Get user's identifier
                    object userIdentifier = user.Identifier;

                    // Get the display name of user
                    string displayName = !string.IsNullOrEmpty(user.DisplayName) ? user.DisplayName : user.Name;

                    // Create new row with the table schema
                    DataRow dr = usersTable.NewRow();

                    // Add row to table
                    usersTable.Rows.Add(dr);

                    // Add data to row
                    object[] dataRow = { user.IsSelected, displayName, user.GetCMSCodeName(true), userIdentifier };
                    dr.ItemArray = dataRow;
                }

                // Apply sorting
                usersTable.DefaultView.Sort = COLUMN_DISPLAYNAME;

                // Bind table as a grid's data source
                using (InvokeHelper ih = new InvokeHelper(grdUsers))
                {
                    ih.InvokeMethod(delegate
                    {
                        grdUsers.DataSource = usersTable;

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

                        // Set enabled state
                        btnSelectAllUsers.Enabled = (grdUsers.Rows.Count > 0);
                        btnDeselectAllUsers.Enabled = (grdUsers.Rows.Count > 0);
                    });
                }
            }

            // Do not interrupt mass actions
            if ((treeMassActions == null) || (!treeMassActions.IsAlive))
            {
                SetWait(false, string.Empty);
            }
        }


        /// <summary>
        /// Binds tree with groups.
        /// </summary>
        private void BindTree()
        {
            // Clear all nodes
            treeGroups.Nodes.Clear();

            // Clear all rows in grid
            grdUsers.Rows.Clear();

            // Disable buttons
            pnlButtons.Enabled = false;

            // Start loading tree structure
            treeLoader = new Thread(LoadNodeStructure);
            treeLoader.IsBackground = true;
            treeLoader.Start();
        }

        #endregion
    }
}