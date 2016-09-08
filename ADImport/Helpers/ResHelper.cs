using WinAppFoundation;

namespace ADImport
{
    /// <summary>
    /// Class providing resource translations.
    /// </summary>
    internal class ResHelper : AbstractResHelper
    {
        #region "Private variables"

        private static IResHelper mCurrentResHelper = null;

        #endregion


        #region "Properties"

        /// <summary>
        /// Current resource helper object.
        /// </summary>
        public static IResHelper CurrentResHelper
        {
            get
            {
                return mCurrentResHelper ?? (mCurrentResHelper = new ResHelper());
            }
        }

        #endregion


        #region "Constructors"

        /// <summary>
        /// Default constructor.
        /// </summary>
        public ResHelper()
            : base(typeof(Resources.Localization))
        {

        }

        #endregion


        #region "Public methods"

        /// <summary>
        /// Gets localized string from resource file.
        /// </summary>
        /// <param name="stringName">Codename of string</param>
        /// <param name="args">An Object array containing zero or more objects to format</param>
        /// <returns>Localized text</returns>
        public static new string GetString(string stringName, params object[] args)
        {
            return CurrentResHelper.GetString(stringName, args);
        }

        #endregion
    }
}