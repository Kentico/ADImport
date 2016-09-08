namespace ADImport
{
    /// <summary>
    /// Specifies type of import.
    /// </summary>
    public enum ImportType
    {
        /// <summary>
        /// Imports all objects.
        /// </summary>
        All = 0,

        /// <summary>
        /// Imports only selected objects.
        /// </summary>
        Selected = 1,

        /// <summary>
        /// New objects will be imported and selected objects will be imported/updated
        /// </summary>
        UpdateSelectedImportNew = 2
    }


    /// <summary>
    /// Specifies format of username.
    /// </summary>
    public enum CodenameFormat
    {
        /// <summary>
        /// Domain name \ SAM account name.
        /// </summary>
        DomainAndSAM = 0,

        /// <summary>
        /// SAM account name.
        /// </summary>
        SAM = 1,

        /// <summary>
        /// User principal name.
        /// </summary>
        UPN = 2,

        /// <summary>
        /// Guid.
        /// </summary>
        Guid = 3
    }


    /// <summary>
    /// Specifies type of selection using in group tree.
    /// </summary>
    public enum SelectionType
    {
        /// <summary>
        /// Selects first level of nodes.
        /// </summary>
        SelectAll = 0,

        /// <summary>
        /// Selects recursively all nodes.
        /// </summary>
        SelectAllRecursively = 1,

        /// <summary>
        /// Deselects first level of nodes.
        /// </summary>
        DeselectAll = 2,

        /// <summary>
        /// Deselects recursively all nodes.
        /// </summary>
        DeselectAllRecursively = 3
    }


    /// <summary>
    /// Specifies binding editor mode.
    /// </summary>
    public enum BindingEditorMode
    {
        /// <summary>
        /// Displays simple binding editor (excludes some fields, displays friendly strings, etc.)
        /// </summary>
        Simple,

        /// <summary>
        /// Displays advanced binding editor (displays all attributes from user's schema, doesn't show friendly names etc.)
        /// </summary>
        Advanced
    }
}