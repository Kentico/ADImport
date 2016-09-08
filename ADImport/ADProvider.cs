using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.DirectoryServices.ActiveDirectory;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Security.Principal;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

using WinAppFoundation;

using WinFormsFramework;

namespace ADImport
{
    /// <summary>
    /// Covers operations above Active Directory.
    /// </summary>
    public class ADProvider : IPrincipalProvider
    {
        #region "Constants"

        /// <summary>
        /// Constant used for identify user.
        /// </summary>
        public const string USER = "user";

        /// <summary>
        /// Constant used for identify group.
        /// </summary>
        public const string GROUP = "group";

        /// <summary>
        /// Name of property for retrieving NETBIOS name.
        /// </summary>
        private const string ATTR_NETBIOS_NAME = "NETBIOSName";

        /// <summary>
        /// Name of property for distinguished name.
        /// </summary>
        private const string ATTR_DISTINGUISHED_NAME = "distinguishedName";

        /// <summary>
        /// Name of property for object GUID.
        /// </summary>
        private const string ATTR_OBJECT_GUID = "objectGuid";

        /// <summary>
        /// Class of the AD object.
        /// </summary>
        private const string ATTR_OBJECT_CLASS = "objectClass";

        /// <summary>
        /// Name of property for object category. For example person in user object class.
        /// </summary>
        private const string ATTR_OBJECT_CATEGORY = "objectCategory";

        /// <summary>
        /// Name of property for group memberships.
        /// </summary>
        private const string ATTR_MEMBERSHIPS = "MemberOf";

        /// <summary>
        /// Name of property for group members.
        /// </summary>
        private const string ATTR_MEMBERS = "member";

        /// <summary>
        /// Name of property for retrieving distinguished name of the naming context for the object.
        /// </summary>
        private const string ATTR_NC_NAME = "nCName";

        /// <summary>
        /// Name of property for retrieving object's SID.
        /// </summary>
        private const string ATTR_OBJECT_SID = "objectSid";

        /// <summary>
        /// Name of property for primary group ID.
        /// </summary>
        private const string ATTR_PRIMARY_GROUP_ID = "primaryGroupID";


        #region "RootDSE attributes"

        /// <summary>
        /// Name of the configuration partition on the domain controller.
        /// </summary>
        private const string ATTR_CONFIGURATION_NAMING_CONTEXT = "configurationNamingContext";

        #endregion

        #endregion


        #region "Private variables"

        /// <summary>
        /// Domain context.
        /// </summary>
        private static PrincipalContext mPrincipalContext = null;

        /// <summary>
        /// Domain controller context.
        /// </summary>
        private static DirectoryContext mDirectoryContext = null;

        /// <summary>
        /// Collection containing principal objects.
        /// </summary>
        private static Dictionary<object, IPrincipalObject> mPrincipalObjects = null;

        /// <summary>
        /// Variable used for identifying temporary nodes (for expanding tree).
        /// </summary>
        private string mTempNodeID = null;

        /// <summary>
        /// Logs with potential exceptions.
        /// </summary>
        private ExceptionLog mExceptionLog = null;

        /// <summary>
        /// Domain NetBIOS name.
        /// </summary>
        private string mDomainNetBIOSName = null;

        /// <summary>
        /// Holds all group principals in AD.
        /// </summary>
        private List<Principal> mAllGroupPrincipals = null;

        /// <summary>
        /// Holds all user principals in AD.
        /// </summary>
        private List<Principal> mAllUserPrincipals = null;

        /// <summary>
        /// Principal context of current user.
        /// </summary>
        private PrincipalContext mCurrentUserPrincipalContext = null;

        private static Regex mRidRegex = null;

        #endregion


        #region "Private properties"

        /// <summary>
        /// Regular expression to match RID in SID.
        /// </summary>
        private static Regex RidRegex
        {
            get
            {
                return mRidRegex ?? (mRidRegex = new Regex(@"^S.*-(\d+)$"));
            }
        }


        /// <summary>
        /// Collection containing IPrincipalObjects.
        /// </summary>
        private static Dictionary<object, IPrincipalObject> PrincipalObjects
        {
            get
            {
                return mPrincipalObjects ?? (mPrincipalObjects = new Dictionary<object, IPrincipalObject>());
            }
        }


        /// <summary>
        /// Domain context.
        /// </summary>
        private PrincipalContext PrincipalContext
        {
            get
            {
                if (mPrincipalContext == null)
                {
                    InitializeDomainContext();
                }
                return mPrincipalContext;
            }
            set
            {
                mPrincipalContext = value;
            }
        }


        /// <summary>
        /// Domain controller context.
        /// </summary>
        private DirectoryContext DirectoryContext
        {
            get
            {
                if (mDirectoryContext == null)
                {
                    if (ImportProfile.ADUseCurrentUserAccount)
                    {
                        mDirectoryContext = new DirectoryContext(DirectoryContextType.DirectoryServer, PrincipalContext.ConnectedServer);
                    }
                    else
                    {
                        mDirectoryContext = new DirectoryContext(DirectoryContextType.DirectoryServer, PrincipalContext.ConnectedServer, ImportProfile.ADUsername, ImportProfile.ADPassword);
                    }
                }
                return mDirectoryContext;
            }
        }


        /// <summary>
        /// Schema of active directory.
        /// </summary>
        ActiveDirectorySchema ActiveDirectorySchema
        {
            get
            {
                return ActiveDirectorySchema.GetSchema(DirectoryContext);
            }
        }


        /// <summary>
        /// Gets all groups in active directory.
        /// </summary>
        /// <returns>All AD groups</returns>
        private IEnumerable<Principal> AllGroupPrincipals
        {
            get
            {
                if (mAllGroupPrincipals == null)
                {
                    // Search active directory for all groups
                    try
                    {
                        GroupPrincipal groupPrincipal = new GroupPrincipal(PrincipalContext);
                        groupPrincipal.Name = "*";
                        using (PrincipalSearcher searcher = new PrincipalSearcher(groupPrincipal))
                        {
                            try
                            {
                                mAllGroupPrincipals = new List<Principal>(searcher.FindAll());
                            }
                            catch (Exception x)
                            {
                                if (x.Message.Contains("1355"))
                                {
                                    mAllGroupPrincipals = new List<Principal>(searcher.FindAll());
                                }
                                else
                                {
                                    throw;
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        ExceptionLog.AddException(ex);
                        // Alternative gaining of groups
                        mAllGroupPrincipals = new List<Principal>();
                        try
                        {
                            using (DirectorySearcher searcher = new DirectorySearcher(DomainRoot, string.Format("({0}={1})", ATTR_OBJECT_CLASS, GROUP)))
                            {
                                searcher.PageSize = int.MaxValue;
                                searcher.SearchScope = SearchScope.Subtree;
                                using (SearchResultCollection searchResults = searcher.FindAll())
                                {
                                    if (searchResults.Count != 0)
                                    {
                                        foreach (SearchResult singleResult in searchResults)
                                        {
                                            Principal principal = Principal.FindByIdentity(PrincipalContext, IdentityType.DistinguishedName, singleResult.Properties[ATTR_DISTINGUISHED_NAME][0].ToString());
                                            if (principal != null)
                                            {
                                                mAllGroupPrincipals.Add(principal);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        catch (Exception exception)
                        {
                            ExceptionLog.AddException(exception);
                        }
                    }
                }
                return mAllGroupPrincipals;
            }
        }


        /// <summary>
        /// Gets all users in active directory.
        /// </summary>
        /// <returns>All AD users</returns>
        private IEnumerable<Principal> AllUserPrincipals
        {
            get
            {
                if (mAllUserPrincipals == null)
                {
                    // Search active directory for all users
                    try
                    {
                        UserPrincipal userPrincipal = new UserPrincipal(PrincipalContext);
                        userPrincipal.Name = "*";
                        using (PrincipalSearcher searcher = new PrincipalSearcher(userPrincipal))
                        {
                            try
                            {
                                mAllUserPrincipals = new List<Principal>(searcher.FindAll());
                            }
                            catch (Exception x)
                            {
                                if (x.Message.Contains("1355"))
                                {
                                    mAllUserPrincipals = new List<Principal>(searcher.FindAll());
                                }
                                else
                                {
                                    throw;
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        ExceptionLog.AddException(ex);
                        // Alternative gaining of users
                        mAllUserPrincipals = new List<Principal>();
                        try
                        {
                            using (DirectorySearcher searcher = new DirectorySearcher(DomainRoot, string.Format("(&({0}={1})({2}=person))", ATTR_OBJECT_CLASS, USER, ATTR_OBJECT_CATEGORY)))
                            {
                                searcher.PageSize = int.MaxValue;
                                searcher.SearchScope = SearchScope.Subtree;
                                using (SearchResultCollection searchResults = searcher.FindAll())
                                {
                                    if (searchResults.Count != 0)
                                    {
                                        foreach (SearchResult singleResult in searchResults)
                                        {
                                            Principal principal = Principal.FindByIdentity(PrincipalContext, IdentityType.DistinguishedName, singleResult.Properties[ATTR_DISTINGUISHED_NAME][0].ToString());
                                            if (principal != null)
                                            {
                                                mAllUserPrincipals.Add(principal);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        catch (Exception exception)
                        {
                            ExceptionLog.AddException(exception);
                        }
                    }
                }
                return mAllUserPrincipals;
            }
        }

        #endregion


        #region "Public properties"


        /// <summary>
        /// Property used for identifying empty AD attributes.
        /// </summary>
        public string NoneAttribute
        {
            get
            {
                return "none";
            }
        }


        /// <summary>
        /// Constant used for identify user.
        /// </summary>
        public string UserIdentifier
        {
            get
            {
                return USER;
            }
        }


        /// <summary>
        /// Constant used for identify group.
        /// </summary>
        public string GroupIdentifier
        {
            get
            {
                return GROUP;
            }
        }


        /// <summary>
        /// Property used for identifying temporary nodes (for expanding tree).
        /// </summary>
        public string TempNodeID
        {
            get
            {
                if (mTempNodeID == null)
                {
                    Guid tempGuid = Guid.NewGuid();
                    mTempNodeID = tempGuid.ToString();
                }
                return mTempNodeID;
            }
        }


        /// <summary>
        /// Gets log with exceptions.
        /// </summary>
        public ExceptionLog ExceptionLog
        {
            get
            {
                return mExceptionLog ?? (mExceptionLog = new ExceptionLog());
            }
        }


        /// <summary>
        /// Root of the directory data tree on a directory server.
        /// </summary>
        private DirectoryEntry RootDSE
        {
            get
            {
                return GetDirectoryEntry(GetLdapPath("rootDSE"));
            }
        }


        /// <summary>
        /// Principal context of current user.
        /// </summary>
        private PrincipalContext CurrentUserPrincipalContext
        {
            get
            {
                return mCurrentUserPrincipalContext ?? (mCurrentUserPrincipalContext = new PrincipalContext(ContextType.Domain));
            }
        }


        /// <summary>
        /// Domain object (root of the domain).
        /// </summary>
        private DirectoryEntry DomainRoot
        {
            get
            {
                string ldapPath = null;
                if (ImportProfile.ADUseCurrentUserAccount)
                {
                    ldapPath = GetLdapPath(null, CurrentUserPrincipalContext.ConnectedServer);
                }
                else
                {
                    ldapPath = GetLdapPath(null);
                }
                return GetDirectoryEntry(ldapPath);
            }
        }


        /// <summary>
        /// Distinguished name of domain root. (Used e.g. as container name.)
        /// </summary>
        private string RootContainerName
        {
            get
            {
                if (DomainRoot != null)
                {
                    object rootContainerNameObj = DomainRoot.Properties[ATTR_DISTINGUISHED_NAME].Value;
                    return rootContainerNameObj as string;
                }
                return null;
            }
        }


        /// <summary>
        /// Returns domain NetBIOS name.
        /// </summary>
        /// <returns>Domain NetBIOS name</returns>
        public string DomainNetBiosName
        {
            get
            {
                if (mDomainNetBIOSName == null)
                {
                    SearchResultCollection forestPartitionList = null;
                    try
                    {
                        // Retrieve the configuration naming context from RootDSE
                        using (DirectoryEntry configSearchRoot = GetDirectoryEntry(GetLdapPath(RootDSE.Properties[ATTR_CONFIGURATION_NAMING_CONTEXT].Value.ToString())))
                        {
                            using (DirectorySearcher configSearch = new DirectorySearcher(configSearchRoot, string.Format("({0}=*)", ATTR_NETBIOS_NAME)))
                            {
                                configSearch.PageSize = int.MaxValue;
                                configSearch.PropertiesToLoad.Add(ATTR_NETBIOS_NAME);
                                configSearch.PropertiesToLoad.Add(ATTR_NC_NAME);

                                forestPartitionList = configSearch.FindAll();
                            }
                        }

                        if (forestPartitionList.Count == 0)
                        {
                            throw new Exception("[ADProvider.DomainNetBIOSName] : NetBIOS name not found.");
                        }
                        else
                        {
                            // Loop through each returned domain in the result collection
                            foreach (SearchResult domainPartition in forestPartitionList)
                            {
                                ResultPropertyValueCollection netBIOSNameProp = domainPartition.Properties[ATTR_NETBIOS_NAME];
                                ResultPropertyValueCollection nCNameProp = domainPartition.Properties[ATTR_NC_NAME];
                                if ((netBIOSNameProp.Count > 0) && (nCNameProp.Count > 0))
                                {
                                    // Check whether the naming context corresponds
                                    if (nCNameProp[0].ToString() == PrincipalContext.Container)
                                    {
                                        // Get NetBIOS name
                                        mDomainNetBIOSName = netBIOSNameProp[0].ToString();
                                        break;
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception exception)
                    {
                        if (forestPartitionList != null)
                        {
                            foreach (SearchResult domainPartition in forestPartitionList)
                            {
                                string path = domainPartition.Path;
                                ResultPropertyValueCollection netBIOSNameProp = domainPartition.Properties[ATTR_NETBIOS_NAME];
                                if (!exception.Data.Contains(path) && (netBIOSNameProp.Count > 0))
                                {
                                    exception.Data.Add(path, netBIOSNameProp[0].ToString());
                                }
                            }
                        }
                        ExceptionLog.AddException(exception);
                    }
                }
                return mDomainNetBIOSName;
            }
        }

        #endregion


        #region "Permission check"

        /// <summary>
        /// Checks permissions which are needed for proceeding.
        /// </summary>
        /// <returns>Null if all needed permissions are granted</returns>
        public string CheckPermissions()
        {
            string toReturn = null;

            if (!IsUserAdministrator())
            {
                toReturn = ResHelper.GetString("Error_NotAdministrator");
            }
            return toReturn;
        }


        /// <summary>
        /// Determines whether application is running under administrator account.
        /// </summary>
        /// <returns>TRUE if user is application is running under administrator account</returns>
        public bool IsUserAdministrator()
        {
            bool isAdmin = false;
            try
            {
                // Get user application is running under
                WindowsIdentity user = WindowsIdentity.GetCurrent();
                if (user != null)
                {
                    // Get principal object
                    WindowsPrincipal principal = new WindowsPrincipal(user);
                    // Check whether principal belongs to Administrators
                    isAdmin = principal.IsInRole(WindowsBuiltInRole.Administrator);
                }
            }
            catch (Exception exception)
            {
                ExceptionLog.AddException(exception);
                isAdmin = false;
            }
            return isAdmin;
        }

        #endregion


        #region "AD connection"

        /// <summary>
        /// Initializes domain context.
        /// </summary>
        public void InitializeDomainContext()
        {
            if (ImportProfile.ADUseCurrentUserAccount)
            {
                // Check whether local account is domain account
                if (UserPrincipal.Current.ContextType != ContextType.Domain)
                {
                    throw new LocalUserAccountException();
                }
                // Initializes context using current account
                PrincipalContext = new PrincipalContext(ContextType.Domain, CurrentUserPrincipalContext.ConnectedServer, RootContainerName);
            }
            else
            {
                // Initialize context using specified credentials
                PrincipalContext = new PrincipalContext(ContextType.Domain, ImportProfile.ADControllerAddress, RootContainerName, ImportProfile.ADUsername, ImportProfile.ADPassword);

                // Touch connected server to find out whether was successfully initialized
                if (PrincipalContext.ConnectedServer == null)
                {
                    throw new DCConnectionException();
                }
            }
        }


        /// <summary>
        /// Resets the context.
        /// </summary>
        public void ClearContext()
        {
            if (mPrincipalContext != null)
            {
                mPrincipalContext.Dispose();
                mPrincipalContext = null;
            }

            mDirectoryContext = null;

            if (mPrincipalObjects != null)
            {
                mPrincipalObjects.DisposeAll();
                mPrincipalObjects.Clear();
            }


            mTempNodeID = null;
            mExceptionLog = null;
            mDomainNetBIOSName = null;

            if (mAllGroupPrincipals != null)
            {
                mAllGroupPrincipals.DisposeAll();
                mAllGroupPrincipals = null;
            }

            if (mAllUserPrincipals != null)
            {
                mAllUserPrincipals.DisposeAll();
                mAllUserPrincipals = null;
            }

            mCurrentUserPrincipalContext = null;
        }

        #endregion


        #region "Principal collection operations"

        /// <summary>
        /// Adds TreeNode to collection with principal objects to specific principal object.
        /// </summary>
        /// <param name="guid">Unique identifier of principal object</param>
        /// <param name="principal">According principal object</param>
        /// <param name="selected">Determines whether to select object</param>
        private void AddPrincipal(object guid, Principal principal, bool? selected)
        {
            IPrincipalObject principalObject = null;
            if (PrincipalObjects.ContainsKey(guid))
            {
                // Update existing object
                principalObject = PrincipalObjects[guid];
                principalObject.SetPrincipal(principal);
            }
            else
            {
                // Add new object to collection
                principalObject = new ADObject(this);
                principalObject.SetPrincipal(principal);

                if (!PrincipalObjects.ContainsKey(guid))
                {
                    PrincipalObjects.Add(guid, principalObject);
                }
            }
            if (selected != null)
            {
                SetSelectedPrincipalObject(principalObject, selected.Value);
            }
        }


        /// <summary>
        /// Determines selected state of principal based on import settings and adds it to a collection.
        /// </summary>
        /// <param name="principal">Principal to process</param>
        private void SelectPrincipalAndAdd(Principal principal)
        {
            if (PrincipalObjects.ContainsKey(principal.Guid))
            {
                return;
            }

            bool selected = false;

            // Preselect objects
            switch (principal.StructuralObjectClass)
            {
                case USER:
                    selected = IsSelected(principal.Guid, ImportProfile.ImportUsersType, ImportProfile.Users, CMSImport.UserExists);
                    break;

                case GROUP:
                    selected = IsSelected(principal.Guid, ImportProfile.ImportRolesType, ImportProfile.Groups, CMSImport.RoleExists);
                    break;
            }

            AddPrincipal(principal.Guid, principal, selected);
        }


        private static bool IsSelected(Guid? principalGuid, ImportType importType, IEnumerable<object> selectedObjects, Func<Guid, bool> existsFunc)
        {
            bool selected = false;

            if ((importType == ImportType.All) || selectedObjects.Contains(principalGuid))
            {
                selected = true;
            }
            else if ((importType == ImportType.UpdateSelectedImportNew) && (principalGuid != null))
            {
                // Preselect new users/roles
                selected = !existsFunc(principalGuid.Value);
            }

            return selected;
        }


        /// <summary>
        /// Loads all users to collection.
        /// </summary>
        public void LoadAllUsers()
        {
            foreach (IPrincipalObject principalObject in GetAllUsers())
            {
            }
        }


        /// <summary>
        /// Loads all groups to collection.
        /// </summary>
        public void LoadAllGroups()
        {
            foreach (IPrincipalObject principalObject in GetAllGroups())
            {
            }
        }


        /// <summary>
        /// Sets selected state of all objects depending on given Import type.
        /// </summary>
        /// <param name="importType">Type of import</param>
        /// <param name="structuralObjectClass">'GROUP' or 'USER'</param>
        public void SetSelectedStates(ImportType importType, string structuralObjectClass)
        {
            if (!string.IsNullOrEmpty(structuralObjectClass))
            {
                foreach (IPrincipalObject principalObject in PrincipalObjects.Values)
                {
                    Principal innerPrincipal = (Principal)principalObject.GetPrincipal();
                    if (innerPrincipal.StructuralObjectClass == structuralObjectClass)
                    {
                        switch (importType)
                        {
                            case ImportType.All:
                                SetSelectedPrincipalObject(principalObject, true);
                                break;

                            case ImportType.Selected:
                            case ImportType.UpdateSelectedImportNew:
                                SetSelectedPrincipalObject(principalObject, false);
                                break;
                        }
                    }
                }
            }
        }

        #endregion


        #region "TreeView & TreeNode operations"

        /// <summary>
        /// Initialize tree view of the AD tree. In case of cycles in AD, choose only one node of the cycle as the group root.
        /// Initializes only root group nodes, which can be later lazily expanded.
        /// All IPrincipalObjects are created (even if the group is not present in the tree) and selected status is set.
        /// </summary>
        /// <param name="treeView">Tree view to add all group "root nodes" to.</param>
        /// <param name="contextMenuClick">Context menu click handler.</param>
        public void LazyAddRootGroupNodes(TreeView treeView, EventHandler contextMenuClick)
        {
            List<IPrincipalObject> allGroups = GetAllGroups().ToList();

            // Get nodes with no parents
            List<IPrincipalObject> roots = allGroups.Where(g => !g.Groups.Any()).ToList();

            // Get nodes which are in a AD subgraph with no root (subgraph contains only cycles).
            List<IPrincipalObject> cycles = allGroups.Except(roots).Where(g => g.Groups.All(cycle => roots.All(y => (Guid)y.Identifier != cycle))).ToList();

            // Add cycles to UI tree by choosing an artificial root
            while (cycles.Any())
            {
                IPrincipalObject artificialRoot = cycles.First();

                roots.Add(artificialRoot);
                cycles.Remove(artificialRoot);

                // Subtract all descendants of the artificial root
                cycles.RemoveAll(c => c.Groups.Contains((Guid)artificialRoot.Identifier));
            }

            // Add root groups to the UI
            using (InvokeHelper ih = new InvokeHelper(treeView))
            {
                ih.InvokeMethod(delegate
                {
                    treeView.BeginUpdate();

                    foreach (IPrincipalObject rootGroup in roots)
                    {
                        treeView.Nodes.Add(CreateNodeForPrincipal(rootGroup, contextMenuClick));

                    }

                    treeView.EndUpdate();
                });
            }
        }


        /// <summary>
        /// Expand temporary node and add all child groups. Always expand only one level of children.
        /// </summary>
        /// <param name="groupNode">Node of group to be expanded. Before expansion this node has only one child node (temporary node).</param>
        /// <param name="contextMenuClick">Context menu click handler.</param>
        public void LazyExpandGroupNode(TreeNode groupNode, EventHandler contextMenuClick)
        {
            // Get node to expand
            IPrincipalObject groupPrincipal = GetPrincipalObject(groupNode.Name);

            // Add child groups
            foreach (Principal memberPrincipalObject in GetMembersOf((GroupPrincipal)groupPrincipal.GetPrincipal(), GROUP))
            {
                if (memberPrincipalObject.Guid != null)
                {
                    IPrincipalObject subGroupPrincipal = GetPrincipalObject(memberPrincipalObject.Guid);

                    // Check for cycles among parents
                    bool isAlreadyPresent = false;
                    TreeNode nodeIterator = groupNode;

                    while (nodeIterator != null)
                    {
                        isAlreadyPresent |= string.Equals(nodeIterator.Name, subGroupPrincipal.Identifier.ToString(), StringComparison.InvariantCultureIgnoreCase);
                        nodeIterator = nodeIterator.Parent;
                    }

                    TreeNode subGroupNode = CreateNodeForPrincipal(subGroupPrincipal, contextMenuClick, !isAlreadyPresent);

                    if (isAlreadyPresent)
                    {
                        subGroupNode.ForeColor = SystemColors.GrayText;
                    }

                    // Add subnode to the tree
                    using (InvokeHelper ih = new InvokeHelper(groupNode.TreeView))
                    {
                        ih.InvokeMethod(() => groupNode.Nodes.Add(subGroupNode));
                    }

                    SetSelectedPrincipalObject(subGroupPrincipal, subGroupPrincipal.IsSelected);
                }
            }
        }


        /// <summary>
        /// Create tree node associated with given principal. 
        /// </summary>
        /// <param name="principal">Group principal to create tree node for.</param>
        /// <param name="contextMenuClick">Context menu click handler.</param>
        /// <param name="createTempNode">Also create temporary node for lazy tree expansion.</param>
        private TreeNode CreateNodeForPrincipal(IPrincipalObject principal, EventHandler contextMenuClick, bool createTempNode = true)
        {
            // Create subnode
            TreeNode subNode = new TreeNode(principal.Name)
            {
                Name = principal.Identifier.ToString()
            };
            AddContextMenu(subNode, contextMenuClick);

            if (createTempNode)
            {
                // Create temporary node for lazy tree expansion
                TreeNode tempNode = new TreeNode(TempNodeID)
                {
                    Name = TempNodeID
                };
                subNode.Nodes.Add(tempNode);
            }

            // Associate node with the principal
            principal.AddTreeNode(subNode);

            return subNode;
        }


        /// <summary>
        /// Add context menu to given tree node. Also assign given click handler.
        /// </summary>
        /// <param name="node">Node to enhance with context menu.</param>
        /// <param name="contextMenuClick">Click handler</param>
        private void AddContextMenu(TreeNode node, EventHandler contextMenuClick)
        {
            ComponentResourceManager resources = new ComponentResourceManager(typeof(AbstractStep));
            ContextMenuStrip menuStrip = new ContextMenuStrip();

            // Add select all button
            var selectMenuItem = new ToolStripMenuItem
            {
                Name = Enum.GetName(typeof (SelectionType), SelectionType.SelectAll) + "|" + node.Name,
                Text = ResHelper.GetString("General_SelectAll"),
                Image = (Image)resources.GetObject("select")
            };
            selectMenuItem.Click += contextMenuClick;
            menuStrip.Items.Add(selectMenuItem);

            // Add select all recursively button
            var selectMenuItemRecur = new ToolStripMenuItem
            {
                Name = Enum.GetName(typeof (SelectionType), SelectionType.SelectAllRecursively) + "|" + node.Name,
                Text = ResHelper.GetString("General_SelectAllRecursively"),
                Image = (Image)resources.GetObject("select")
            };
            selectMenuItemRecur.Click += contextMenuClick;
            menuStrip.Items.Add(selectMenuItemRecur);

            // Add deselect all button
            var deselectMenuItem = new ToolStripMenuItem
            {
                Name = Enum.GetName(typeof (SelectionType), SelectionType.DeselectAll) + "|" + node.Name,
                Text = ResHelper.GetString("General_DeselectAll"),
                Image = (Image)resources.GetObject("deselect")
            };
            deselectMenuItem.Click += contextMenuClick;
            menuStrip.Items.Add(deselectMenuItem);

            // Add deselect all recursively button
            var deselectMenuItemRecur = new ToolStripMenuItem
            {
                Name = Enum.GetName(typeof (SelectionType), SelectionType.DeselectAllRecursively) + "|" + node.Name,
                Text = ResHelper.GetString("General_DeselectAllRecursively"),
                Image = (Image)resources.GetObject("deselect")
            };
            deselectMenuItemRecur.Click += contextMenuClick;
            menuStrip.Items.Add(deselectMenuItemRecur);

            // Assign menu to treenode
            node.ContextMenuStrip = menuStrip;
        }


        /// <summary>
        /// Preselects nodes in tree.
        /// </summary>
        /// <param name="groupIdentifier">Identifier of group (null = all groups)</param>
        /// <param name="select">Determines whether to select or deselect</param>
        /// <param name="nest">Determines whether to select all</param>
        public void PreselectGroups(object groupIdentifier, bool select, bool nest)
        {
            // If there is no groups specified
            if (groupIdentifier == null)
            {
                // Preselect all groups
                foreach (IPrincipalObject group in GetAllGroups())
                {
                    GroupPrincipal groupPrincipal = (GroupPrincipal)group.GetPrincipal();
                    PreselectGroups(groupPrincipal.Guid, select, false);
                }
            }
            else
            {
                // Get group by identifier
                IPrincipalObject principal = GetPrincipalObject(groupIdentifier);
                if (principal != null)
                {
                    // Preselect group
                    SetSelectedPrincipalObject(principal.Identifier, select);

                    GroupPrincipal group = (GroupPrincipal)principal.GetPrincipal();

                    if (group != null)
                    {
                        // Get all members of group
                        foreach (Principal member in GetMembersOf(group, GROUP, nest))
                        {
                            // Preselect first level
                            SetSelectedPrincipalObject(member.Guid, select);
                        }
                    }
                }
            }
        }


        /// <summary>
        /// Sets visibility of context menu.
        /// </summary>
        /// <param name="enabled">Determines whether to allow using context menu</param>
        public void SetContextMenuEnabledState(bool enabled)
        {
            foreach (IPrincipalObject principalObject in PrincipalObjects.Values)
            {
                principalObject.SetContextMenuEnabledState(enabled);
            }
        }

        #endregion


        #region "AD objects operations"

        /// <summary>
        /// Gets collection of strings representing attributes of user class in active directory.
        /// </summary>
        /// <returns>Collection of strings representing attributes of user class in active directory.</returns>
        public IEnumerable<string> GetUserAttributes()
        {
            ActiveDirectorySchemaClass userClass = ActiveDirectorySchema.FindClass(USER);
            ReadOnlyActiveDirectorySchemaPropertyCollection userProps = userClass.GetAllProperties();
            foreach (ActiveDirectorySchemaProperty property in userProps)
            {
                yield return property.Name;
            }
        }


        /// <summary>
        /// Creates DirectoryEntry out of the given path with respect to authentication.
        /// </summary>
        /// <param name="path">Path to use</param>
        /// <returns>DirectoryEntry corresponding to the given path.</returns>
        public DirectoryEntry GetDirectoryEntry(string path)
        {
            DirectoryEntry entry = null;
            if (ImportProfile.ADUseCurrentUserAccount)
            {
                entry = new DirectoryEntry(path);
            }
            else
            {
                entry = new DirectoryEntry(path, ImportProfile.ADUsername, ImportProfile.ADPassword);
            }
            return entry;
        }


        /// <summary>
        /// Completes path with respect to authentication.
        /// </summary>
        /// <param name="path">Path to make complete</param>
        /// <returns>Complete LDAP path</returns>
        public string GetLdapPath(string path)
        {
            return GetLdapPath(path, ImportProfile.ADUseCurrentUserAccount ? null : ImportProfile.ADControllerAddress);
        }


        /// <summary>
        /// Completes path with respect to authentication.
        /// </summary>
        /// <param name="guid">Guid to insert to path.</param>
        /// <returns>Complete LDAP path</returns>
        public string GetGuidLdapPath(Guid guid)
        {
            return GetLdapPath(string.Format("<GUID={0}>", guid));
        }


        /// <summary>
        /// Completes path with respect to authentication.
        /// </summary>
        /// <param name="sid">SID to insert to path.</param>
        /// <returns>Complete LDAP path</returns>
        public string GetSidLdapPath(SecurityIdentifier sid)
        {
            return GetLdapPath(string.Format("<SID={0}>", sid));
        }


        /// <summary>
        /// Completes path with respect to authentication.
        /// </summary>
        /// <param name="path">Path to make complete</param>
        /// <param name="controllerAddress">Server address</param>
        /// <returns>Complete LDAP path</returns>
        public string GetLdapPath(string path, string controllerAddress)
        {
            string completePath = "LDAP://";
            if (!string.IsNullOrEmpty(controllerAddress))
            {
                completePath += controllerAddress;
                if (!string.IsNullOrEmpty(path))
                {
                    completePath += "/";
                }
            }
            completePath += path;
            return completePath;
        }


        /// <summary>
        /// Gets members of the given group (users or groups).
        /// </summary>
        /// <param name="groupPrincipal">Group to examine</param>
        /// <param name="structureObjectClass">Class of objects to retrieve (user/group).</param>
        /// <param name="recursive">Whether to nest recursively</param>
        /// <returns>Collection of member objects</returns>
        private IEnumerable<Principal> GetMembersOf(GroupPrincipal groupPrincipal, string structureObjectClass, bool recursive = false)
        {
            List<Principal> members = new List<Principal>();

            Queue<GroupPrincipal> queue = new Queue<GroupPrincipal>();
            queue.Enqueue(groupPrincipal);

            do
            {
                GroupPrincipal group = queue.Dequeue();

                if (group.Guid.HasValue)
                {
                    DirectoryEntry groupEntry = GetDirectoryEntry(group.Guid.Value, ATTR_MEMBERS);

                    // If a group is primary to some of its users, default search would not find these users
                    members.AddRange(GetPrimaryGroupUsers(groupEntry));

                    // Go through members of given group
                    foreach (Object memberIdentity in GetArrayProperty(groupEntry,ATTR_MEMBERS))
                    {
                        try
                        {
                            Principal member = Principal.FindByIdentity(PrincipalContext, IdentityType.DistinguishedName, memberIdentity.ToString());
                            if (member != null)
                            {
                                // Add transitive child members
                                if ((member.StructuralObjectClass == GROUP) && recursive)
                                {
                                    GroupPrincipal childGroup = member as GroupPrincipal;
                                    if (!queue.Union(members).Contains(childGroup))
                                    {
                                        queue.Enqueue(childGroup);
                                    }
                                }

                                members.Add(member);
                            }
                        }
                        catch (Exception ex)
                        {
                            ExceptionLog.AddException(ex);
                        }
                    }
                }
            } while (queue.Any());

            return members.Where(m => m.StructuralObjectClass == structureObjectClass).Distinct();
        }        


        /// <summary>
        /// Gets users participating in given group.
        /// </summary>
        /// <param name="group">Group to examine</param>
        public List<IPrincipalObject> GetUsersOf(IPrincipalObject group)
        {
            List<IPrincipalObject> users = new List<IPrincipalObject>();
            GroupPrincipal groupPrincipal = GroupPrincipal.FindByIdentity(PrincipalContext, IdentityType.Guid, group.Identifier.ToString());

            IEnumerable<Principal> principals = GetMembersOf(groupPrincipal, USER, true);
            if (principals != null)
            {
                foreach (Principal principal in principals.Where(p => (p.Guid != null)))
                {
                    SelectPrincipalAndAdd(principal);
                    IPrincipalObject principalObject = PrincipalObjects[principal.Guid];

                    if (!users.Contains(principalObject))
                    {
                        users.Add(principalObject);
                    }
                }
            }

            return users;
        }


        /// <summary>
        /// Gets all groups in which <paramref name="participant"/> participates.
        /// </summary>
        /// <param name="participant">Distinguished name of object</param>
        public IEnumerable<Guid> GetMemberships(Principal participant)
        {
            if ((participant != null) && (participant.Guid != null))
            {
                ISet<Guid> memberships = new HashSet<Guid>();

                // Find all memberships using BFS (to avoid cycles in the directory) on the MemberOf attribute. This attribute stores one level of memberships.
                // Only use this technique if no members retrieved from previous search (previous search suffers from higher privileges requirements).
                Queue<DirectoryEntry> queue = new Queue<DirectoryEntry>();

                DirectoryEntry participantsEntry = GetDirectoryEntry(participant.Guid.Value);
                queue.Enqueue(participantsEntry);

                if (participant.StructuralObjectClass == USER)
                {
                    // Add primary group, which is mandatory
                    var primaryGroup = GetPrimaryGroup(participantsEntry);

                    if (primaryGroup == null)
                    {
                        throw new ArgumentException("Cannot retrieve the primary group of user " + participant.Name, "participant");
                    }

                    queue.Enqueue(primaryGroup);
                }

                do
                {
                    // Get top entry
                    DirectoryEntry entry = queue.Dequeue();
                    Guid entryGuid = GetEntryGuidFromEntry(entry);

                    if (entryGuid == Guid.Empty)
                    {
                        continue;
                    }

                    if (memberships.Contains(entryGuid))
                    {
                        // Skip already visited groups
                        continue;
                    }

                    if (participant.Guid != entryGuid)
                    {
                        // Do not yield in the first iteration
                        memberships.Add(entryGuid);
                    }

                    // Get immediate parents of current node
                    foreach (DirectoryEntry parent in GetArrayProperty(entry, ATTR_MEMBERSHIPS).Cast<string>().Select(p => GetGroupDirectoryEntry(p, ATTR_OBJECT_GUID, ATTR_MEMBERSHIPS)))
                    {
                        if (parent != null)
                        {
                            Guid parentGuid = GetEntryGuidFromEntry(parent);
                            if ((parentGuid != Guid.Empty) && (participant.Guid != parentGuid))
                            {
                                queue.Enqueue(parent);
                            }
                        }
                    }

                } while (queue.Any());

                return memberships.ToList();
            }

            return null;
        }


        /// <summary>
        /// Returns <see cref="System.Guid"/> value for entry object GUID attribute.
        /// </summary>
        /// <param name="entry">Directory entry</param>
        private static Guid GetEntryGuidFromEntry(DirectoryEntry entry)
        {
            byte[] entryGuidBytes = entry.Properties[ATTR_OBJECT_GUID][0] as byte[];
            Guid entryGuid = (entryGuidBytes == null) ? Guid.Empty : new Guid(entryGuidBytes);
            return entryGuid;
        }


        /// <summary>
        /// Translates distinguished names to GUIDs.
        /// </summary>
        /// <param name="distinguishedNames">Collection of distinguished names</param>
        /// <returns>Object GUIDs</returns>
        public List<Guid> GetGuids(IEnumerable<string> distinguishedNames)
        {
            List<Guid> result = new List<Guid>();
            if (distinguishedNames.Any())
            {
                string query = "(|" + distinguishedNames.Aggregate<string, string>(null, (current, s) => current + ("(" + ATTR_DISTINGUISHED_NAME + "=" + s + ")")) + ")";

                using (DirectorySearcher searcher = new DirectorySearcher(DomainRoot, query))
                {
                    searcher.PageSize = int.MaxValue;
                    searcher.PropertiesToLoad.Add(ATTR_OBJECT_GUID);
                    using (SearchResultCollection guidCollection = searcher.FindAll())
                    {
                        foreach (SearchResult searchResult in guidCollection.Cast<SearchResult>())
                        {
                            byte[] byteGuid = searchResult.Properties[ATTR_OBJECT_GUID][0] as byte[];
                            result.Add(new Guid(byteGuid));
                        }
                    }
                }
            }
            return result;
        }



        /// <summary>
        /// Gets list of groups in active directory.
        /// </summary>
        /// <returns>List of AD groups</returns>
        public IEnumerable<IPrincipalObject> GetAllGroups()
        {
            foreach (GroupPrincipal group in AllGroupPrincipals)
            {
                SelectPrincipalAndAdd(group);
                yield return PrincipalObjects[@group.Guid];
            }
        }


        /// <summary>
        /// Gets list of users in active directory.
        /// </summary>
        /// <returns>List of AD users</returns>
        public IEnumerable<IPrincipalObject> GetAllUsers()
        {
            foreach (UserPrincipal user in AllUserPrincipals)
            {
                SelectPrincipalAndAdd(user);
                yield return PrincipalObjects[user.Guid];
            }
        }


        /// <summary>
        /// Finds out whether object exists.
        /// </summary>
        /// <param name="objectIdentifier">Identifier of an object</param>
        /// <returns>TRUE if exists</returns>
        public bool Exists(object objectIdentifier)
        {
            Principal principal = Principal.FindByIdentity(PrincipalContext, IdentityType.Guid, objectIdentifier.ToString());
            return (principal != null);
        }


        /// <summary>
        /// Selects or deselects principal object.
        /// </summary>
        /// <param name="principalKey">Key of principal object</param>
        /// <param name="selected">Indicates whether to select or deselect</param>
        public void SetSelectedPrincipalObject(object principalKey, bool selected)
        {
            // Try to get principal
            IPrincipalObject principal = GetPrincipalObject(principalKey);

            // Set selected
            SetSelectedPrincipalObject(principal, selected);
        }


        /// <summary>
        /// Selects or deselects principal object.
        /// </summary>
        /// <param name="principalObject">Principal object</param>
        /// <param name="selected">Indicates whether to select or deselect</param>
        public void SetSelectedPrincipalObject(IPrincipalObject principalObject, bool selected)
        {
            // Set selected
            if (principalObject != null)
            {
                principalObject.IsSelected = selected;
            }
        }


        /// <summary>
        /// Gets principal object based on given key.
        /// </summary>
        /// <param name="principalKey">Key of principal object</param>
        /// <returns>Matching principal object</returns>
        public IPrincipalObject GetPrincipalObject(object principalKey)
        {
            try
            {
                if (!PrincipalObjects.ContainsKey(principalKey))
                {
                    Principal principal = Principal.FindByIdentity(PrincipalContext, IdentityType.Guid, principalKey.ToString());
                    if (principal != null)
                    {
                        AddPrincipal(principalKey, principal, null);
                    }
                }

                if (PrincipalObjects.ContainsKey(principalKey))
                {
                    // Return principal based on identifier
                    return PrincipalObjects[principalKey];
                }
            }
            catch (Exception exception)
            {
                exception.Data.Add(principalKey.ToString(), principalKey);
                ExceptionLog.AddException(exception);
            }

            return null;
        }

        #endregion


        #region "Parsing AD attributes"

        /// <summary>
        /// Parses SID from attribute.
        /// </summary>
        /// <param name="attribute">AD attribute</param>
        /// <returns>String representation of SID</returns>
        public string GetSID(object attribute)
        {
            SecurityIdentifier sid = new SecurityIdentifier((byte[])attribute, 0);
            return sid.ToString();
        }


        /// <summary>
        /// Parses date from attribute.
        /// </summary>
        /// <param name="attribute">AD attribute</param>
        /// <returns>String representing time</returns>
        public string GetTimeFromInterval(object attribute)
        {
            // Parse time interval from IADsLargeInteger
            int lowPart = Convert.ToInt32(attribute.GetType().InvokeMember("LowPart", BindingFlags.GetProperty, null, attribute, null));
            int highPart = Convert.ToInt32(attribute.GetType().InvokeMember("HighPart", BindingFlags.GetProperty, null, attribute, null));
            Int64 interval = ((long)highPart << 32) + lowPart;
            return DateTime.FromFileTime(interval).ToString();
        }

        #endregion


        #region "Helper methods"


        /// <summary>
        /// Gets correct type of identifier.
        /// </summary>
        /// <param name="identifier">Identifier to convert</param>
        /// <returns>Identifier of desired type</returns>
        public object ConvertToObjectIdentifier(object identifier)
        {
            return new Guid(identifier.ToString());
        }


        /// <summary>
        /// Gets Active Directory object by GUID and returns its Directory Entry with given attributes (other attributes may not be loaded).
        /// </summary>
        /// <param name="guid">Unique identifier of AD object</param>
        /// <param name="propertiesToLoad">AD properties to load. Other properties may not be refreshed or loaded properly.</param>
        private DirectoryEntry GetDirectoryEntry(Guid guid, params string[] propertiesToLoad)
        {
            if (guid != Guid.Empty)
            {
                var result = GetDirectoryEntry(GetGuidLdapPath(guid));
                result.RefreshCache(propertiesToLoad);

                return result;
            }

            return null;
        }


        /// <summary>
        /// Gets Active Directory object by querying LDAP with given LDAP filter. Resolve specific attributes (other attributes may not be loaded).
        /// </summary>
        /// <param name="ldapFilter">LDAP filter to select object with.</param>
        /// <param name="propertiesToLoad">AD properties to load. Other properties may not be refreshed or loaded properly.</param>
        private DirectoryEntry GetDirectoryEntry(string ldapFilter, params string[] propertiesToLoad)
        {
            using (var searcher = new DirectorySearcher(DomainRoot, ldapFilter, propertiesToLoad))
            {
                var result = searcher.FindOne();

                if (result != null)
                {
                    return result.GetDirectoryEntry();
                }
            }

            return null;
        }


        /// <summary>
        /// Get users primary group, which is not listed in memberOf attribute.
        /// </summary>
        /// <param name="user">User to get the group from.</param>
        private DirectoryEntry GetPrimaryGroup(DirectoryEntry user)
        {
            // Get user SID
            user.RefreshCache(new[] { ATTR_OBJECT_SID, ATTR_PRIMARY_GROUP_ID });
            byte[] userSid = user.Properties[ATTR_OBJECT_SID][0] as byte[];

            if ((userSid != null) && user.Properties.Contains(ATTR_PRIMARY_GROUP_ID))
            {
                // Get users primary group RID
                byte[] groupRid = BitConverter.GetBytes((int)user.Properties[ATTR_PRIMARY_GROUP_ID].Value);

                // Forge primary group SID by replacing RID
                for (int i = 0; i < groupRid.Length; i++)
                {
                    userSid.SetValue(groupRid[i], (userSid.Length - groupRid.Length) + i);
                }

                return GetDirectoryEntry(GetSidLdapPath(new SecurityIdentifier(userSid, 0)));
            }

            return null;
        }


        /// <summary>
        /// Gets all users of given primary group. Primary groups are not stored in the members attribute.
        /// </summary>
        /// <param name="groupEntry">Group to examine</param>
        private IEnumerable<Principal> GetPrimaryGroupUsers(DirectoryEntry groupEntry)
        {
            List<Principal> userList = new List<Principal>();

            if (groupEntry != null)
            {
                // Get SID
                SecurityIdentifier secId = new SecurityIdentifier(groupEntry.Properties[ATTR_OBJECT_SID][0] as byte[], 0);

                // Get RID
                string rid = RidRegex.Match(secId.Value).Groups[1].Value;

                // Search for users that has a particular primary group
                using (DirectorySearcher dsLookForUsers = new DirectorySearcher(DomainRoot))
                {
                    dsLookForUsers.PageSize = int.MaxValue;
                    dsLookForUsers.Filter = string.Format("({0}={1})", ATTR_PRIMARY_GROUP_ID, rid);
                    dsLookForUsers.SearchScope = SearchScope.Subtree;
                    dsLookForUsers.PropertiesToLoad.Add(ATTR_DISTINGUISHED_NAME);

                    using (SearchResultCollection srcUsers = dsLookForUsers.FindAll())
                    {
                        foreach (SearchResult user in srcUsers)
                        {
                            // Get principal by distinguished name
                            string dn = user.Properties[ATTR_DISTINGUISHED_NAME][0].ToString();
                            Principal principal = Principal.FindByIdentity(PrincipalContext, IdentityType.DistinguishedName, dn);
                            if (principal != null)
                            {
                                userList.Add(principal);
                            }
                        }
                    }
                }
            }
            return userList;
        }


        /// <summary>
        /// Finds Active Directory group by Distinguished Name and returns its Directory Entry with given attributes (other attributes may not be loaded).
        /// </summary>
        /// <param name="distinguishedName">Distinguished name of the group.</param>
        /// <param name="propertiesToLoad">AD properties to load. Other properties may not be refreshed or loaded properly.</param>
        private DirectoryEntry GetGroupDirectoryEntry(string distinguishedName, params string[] propertiesToLoad)
        {
            if (!string.IsNullOrEmpty(distinguishedName))
            {
                return GetDirectoryEntry(String.Format("(&({0}={1})({2}={3}))", ATTR_OBJECT_CLASS, GROUP, ATTR_DISTINGUISHED_NAME, distinguishedName), propertiesToLoad);
            }

            return null;
        }


        /// <summary>
        /// Gets all items of an array LDAP properties. Such properties can be large and the server doesn't send all items at once.
        /// </summary>
        /// <param name="entry">Entry to get the property from</param>
        /// <param name="propertyName">An LDAP property name. The type of the property must be an array</param>
        private IEnumerable<object> GetArrayProperty(DirectoryEntry entry, string propertyName)
        {
            int start = 0;
            const int windowSize = 1000;

            bool reachedEnd = false;

            List<object> result = new List<object>();

            if (entry != null)
            {
                while (!reachedEnd)
                {
                    // Get items from current interval
                    entry.RefreshCache(new[] { string.Format("{0};range={1}-{2}", propertyName, start, start + windowSize) });

                    if (entry.Properties.Contains(propertyName))
                    {
                        var refreshedProperty = entry.Properties[propertyName].Value;

                        if (refreshedProperty.GetType().IsArray)
                        {
                            // Check if the array contains more items
                            var temporary = (object[])refreshedProperty;
                            reachedEnd = temporary.Length < windowSize + 1;

                            result.AddRange(temporary);

                            // Move to the next interval, but always include last item from previous search, because if start of the range is larger than the array length, the search throws an exception
                            start += windowSize;
                        }
                        else
                        {
                            result.Add(refreshedProperty);
                            reachedEnd = true;
                        }
                    }
                    else
                    {
                        // Current Directory Entry does not contain given property
                        reachedEnd = true;
                    }
                }
            }

            return result.Distinct().ToList();
        }

        #endregion
    }
}