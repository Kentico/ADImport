using System;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
using System.Windows.Forms;

namespace ADImport
{
    /// <summary>
    /// Interface for objects providing principals and operations above them.
    /// </summary>
    public interface IPrincipalProvider
    {
        #region "Properties"

        /// <summary>
        /// Property used for identifying temporary nodes (for expanding tree).
        /// </summary>
        string TempNodeID
        {
            get;
        }


        /// <summary>
        /// Property used for identifying empty AD attributes.
        /// </summary>
        string NoneAttribute
        {
            get;
        }


        /// <summary>
        /// Constant used for identify user.
        /// </summary>
        string UserIdentifier
        {
            get;
        }


        /// <summary>
        /// Constant used for identify group.
        /// </summary>
        string GroupIdentifier
        {
            get;
        }


        /// <summary>
        /// Gets log with potential exceptions.
        /// </summary>
        ExceptionLog ExceptionLog
        {
            get;
        }


        /// <summary>
        /// Returns domain NetBIOS name.
        /// </summary>
        /// <returns>Domain NetBIOS name</returns>
        string DomainNetBiosName
        {
            get;
        }

        #endregion


        #region "Domain connection methods"

        /// <summary>
        /// Initializes domain context.
        /// </summary>
        void InitializeDomainContext();


        /// <summary>
        /// Resets the context.
        /// </summary>
        void ClearContext();

        #endregion


        #region "Permission check"

        /// <summary>
        /// Checks permissions which are needed for proceeding.
        /// </summary>
        /// <returns>Null if all needed permissions are granted</returns>
        string CheckPermissions();

        #endregion


        #region "Principal operations"

        /// <summary>
        /// Gets users participating in given group.
        /// </summary>
        /// <param name="group">Group to examine</param>
        List<IPrincipalObject> GetUsersOf(IPrincipalObject group);


        /// <summary>
        /// Gets distinguished names of all groups in which the object participates.
        /// </summary>
        /// <param name="participant">Distinguished name of object to examine</param>
        IEnumerable<Guid> GetMemberships(Principal participant);


        /// <summary>
        /// Translates distinguished names to GUIDs.
        /// </summary>
        /// <param name="distinguishedNames">Collection of distinguished names</param>
        /// <returns>Object GUIDs</returns>
        List<Guid> GetGuids(IEnumerable<string> distinguishedNames);


        /// <summary>
        /// Gets list of groups in active directory.
        /// </summary>
        /// <returns>List of AD groups</returns>
        IEnumerable<IPrincipalObject> GetAllGroups();


        /// <summary>
        /// Gets list of users in active directory.
        /// </summary>
        /// <returns>List of AD users</returns>
        IEnumerable<IPrincipalObject> GetAllUsers();


        /// <summary>
        /// Finds out whether object exists.
        /// </summary>
        /// <param name="objectIdentifier">Identifier of an object</param>
        /// <returns>TRUE if exists</returns>
        bool Exists(object objectIdentifier);


        /// <summary>
        /// Selects or deselects principal object.
        /// </summary>
        /// <param name="principalKey">Key of principal object</param>
        /// <param name="selected">Indicates whether to select or deselect</param>
        void SetSelectedPrincipalObject(object principalKey, bool selected);


        /// <summary>
        /// Gets principal object based on given key.
        /// </summary>
        /// <param name="principalKey">Key of principal object</param>
        /// <returns>Matching principal object</returns>
        IPrincipalObject GetPrincipalObject(object principalKey);

        #endregion


        #region "Principal collection operations"

        /// <summary>
        /// Sets selected state of all objects depending on given Import type.
        /// </summary>
        /// <param name="importType">Type of import</param>
        /// <param name="structuralObjectClass">'GROUP' or 'USER'</param>
        void SetSelectedStates(ImportType importType, string structuralObjectClass);


        /// <summary>
        /// Loads all users to collection.
        /// </summary>
        void LoadAllUsers();


        /// <summary>
        /// Loads all groups to collection.
        /// </summary>
        void LoadAllGroups();

        #endregion


        #region "TreeView & TreeNode operations"
        
        /// <summary>
        /// Expand temporary node and add all child groups. Always expand only one level of children.
        /// </summary>
        /// <param name="groupNode">Node of group to be expanded. Before expansion this node has only one child node (temporary node).</param>
        /// <param name="contextMenuClick">Context menu click handler.</param>
        void LazyExpandGroupNode(TreeNode groupNode, EventHandler contextMenuClick);


        /// <summary>
        /// Initialize tree view of the AD tree. In case of cycles in AD, choose only one node of the cycle as the root.
        /// Initializes only root group nodes, which can be later lazily expanded.
        /// All IPrincipalObjects are created (even if the group is not present in the tree) and selected status is set.
        /// </summary>
        /// <param name="treeView">Tree view to add all group "root nodes" to.</param>
        /// <param name="contextMenuClick">Context menu click handler.</param>
        void LazyAddRootGroupNodes(TreeView treeView, EventHandler contextMenuClick);


        /// <summary>
        /// Preselects nodes in tree.
        /// </summary>
        /// <param name="groupIdentifier">Identifier of group (null = all groups)</param>
        /// <param name="select">Determines whether to select or deselect</param>
        /// <param name="nest">Determines whether to select all</param>
        void PreselectGroups(object groupIdentifier, bool select, bool nest);


        /// <summary>
        /// Sets visibility of context menu.
        /// </summary>
        /// <param name="enabled">Determines whether to allow using context menu</param>
        void SetContextMenuEnabledState(bool enabled);

        #endregion


        #region "Class attributes"

        /// <summary>
        /// Parses SID from attribute.
        /// </summary>
        /// <param name="attribute">AD attribute</param>
        /// <returns>String representation of SID</returns>
        string GetSID(object attribute);


        /// <summary>
        /// Parses date from attribute.
        /// </summary>
        /// <param name="attribute">AD attribute</param>
        /// <returns>String representing time</returns>
        string GetTimeFromInterval(object attribute);


        /// <summary>
        /// Gets collection of strings representing attributes of user class in active directory.
        /// </summary>
        /// <returns>Collection of strings representing attributes of user class in active directory.</returns>
        IEnumerable<string> GetUserAttributes();

        #endregion


        #region "Helper methods"


        /// <summary>
        /// Gets correct type of identifier.
        /// </summary>
        /// <param name="identifier">Identifier to convert</param>
        /// <returns>Identifier of desired type</returns>
        object ConvertToObjectIdentifier(object identifier);

        #endregion
    }
}