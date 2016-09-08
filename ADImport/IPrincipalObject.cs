using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace ADImport
{
    /// <summary>
    /// Interface for principal objects.
    /// </summary>
    public interface IPrincipalObject : IDisposable
    {
        #region "Properties"

        /// <summary>
        /// Gets unique identifier of principal object.
        /// </summary>
        /// <returns>Identifier</returns>
        object Identifier
        {
            get;
        }


        /// <summary>
        /// Gets or sets whether principal object is selected.
        /// </summary>
        /// <returns>TRUE if object is selected</returns>
        bool IsSelected
        {
            get;
            set;
        }


        /// <summary>
        /// Gets name of principal object.
        /// </summary>
        /// <returns>Name</returns>
        string Name
        {
            get;
        }


        /// <summary>
        /// Gets display name of principal object.
        /// </summary>
        /// <returns>Display name</returns>
        string DisplayName
        {
            get;
        }


        /// <summary>
        /// Gets description of principal object.
        /// </summary>
        /// <returns>Description</returns>
        string Description
        {
            get;
        }


        /// <summary>
        /// Gets whether principal object is enabled or disabled.
        /// </summary>
        /// <returns>TRUE if enabled</returns>
        bool Enabled
        {
            get;
        }


        /// <summary>
        /// Collection containing identifiers of groups where object participates.
        /// </summary>
        List<Guid> Groups
        {
            get;
        }

        #endregion


        #region "Methods"

        /// <summary>
        /// Adds TreeNode to collection of related nodes.
        /// </summary>
        /// <param name="node">Node to add</param>
        void AddTreeNode(TreeNode node);


        /// <summary>
        /// Sets visibility of context menu.
        /// </summary>
        /// <param name="enabled">Determines whether to allow using context menu</param>
        void SetContextMenuEnabledState(bool enabled);


        /// <summary>
        /// Sets principal object.
        /// </summary>
        /// <param name="principal">Object containing principal</param>
        void SetPrincipal(object principal);


        /// <summary>
        /// Gets principal object.
        /// </summary>
        /// <returns>Principal object</returns>
        object GetPrincipal();


        /// <summary>
        /// Gets account name of principal object for import to CMS.
        /// </summary>
        /// <param name="safe">Determines whether to get safe form of the name</param>
        /// <returns>Code name for CMS</returns>
        string GetCMSCodeName(bool safe);


        /// <summary>
        /// Gets display name of in correct format.
        /// </summary>
        /// <returns>Display name in format defined by ImportProfile</returns>
        string GetCMSDisplayName();


        /// <summary>
        /// Gets properties of inner object.
        /// </summary>
        object GetProperty(string name);


        /// <summary>
        /// Finds out whether object participates in given role.
        /// </summary>
        /// <returns>TRUE if object belongs to role</returns>
        bool IsPrincipalInGroup(Guid groupIdentifier);

        #endregion
    }
}