namespace WinAppFoundation
{
    /// <summary>
    /// Interface providing resource translations.
    /// </summary>
    public interface IResHelper
    {
        /// <summary>
        /// Gets localized string from resource file.
        /// </summary>
        /// <param name="stringName">Codename of string</param>
        /// <param name="args">An Object array containing zero or more objects to format</param>
        /// <returns>Localized text</returns>
        string GetString(string stringName, params object[] args);


        /// <summary>
        /// Changes language of supplied translations.
        /// </summary>
        /// <param name="cultureCode">Desired culture</param>
        void ChangeLanguage(string cultureCode);


        /// <summary>
        /// Adds an element to a collection of localized elements.
        /// </summary>
        /// <param name="localization">Localized element</param>
        void AddLocalization(ILocalizableItem localization);
    }
}
