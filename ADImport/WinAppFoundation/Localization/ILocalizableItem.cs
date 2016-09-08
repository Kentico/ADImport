namespace WinAppFoundation
{
    /// <summary>
    /// Represents one item containing localizable string. E.g. part of UI like Label or Button.
    /// </summary>
    public interface ILocalizableItem
    {
        /// <summary>
        /// Updates the value of the localized object.
        /// </summary>
        void UpdateTargetValue();


        /// <summary>
        /// Returns whether localization is still being used in the UI.
        /// </summary>
        /// <remarks>Used for garbage collection.</remarks>
        bool IsAlive
        {
            get;
        }
    }
}
