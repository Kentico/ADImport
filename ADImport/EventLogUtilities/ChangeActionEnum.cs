namespace ADImport
{
    /// <summary>
    /// Enumeration describing change action applied to an item (user or role) in CMS
    /// </summary>
    internal enum ChangeActionEnum
    {
        /// <summary>
        /// Item was created/newly added
        /// </summary>
        Created,
        

        /// <summary>
        /// Item was modified (existed before and was altered)
        /// </summary>
        Updated,
        

        /// <summary>
        /// Item was removed (existed before and does not anymore)
        /// </summary>
        Deleted
    }
}
