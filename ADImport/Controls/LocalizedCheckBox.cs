using System.Windows.Forms;

namespace ADImport
{
    /// <summary>
    /// CheckBox with localized text.
    /// </summary>
    public class LocalizedCheckBox : CheckBox
    {
        #region "Variables"

        /// <summary>
        /// Name of resource string used for text.
        /// </summary>
        private string mResourceString = null;

        #endregion


        #region "Properties"

        /// <summary>
        /// Name of a resource string used for text.
        /// </summary>
        public string ResourceString
        {
            get
            {
                return mResourceString;
            }
            set
            {
                mResourceString = value;
            }
        }


        /// <summary>
        /// Returns localized text.
        /// </summary>
        public override string Text
        {
            get
            {
                return ResHelper.GetString(string.IsNullOrEmpty(ResourceString) ? base.Text : ResourceString);
            }
            set
            {
                base.Text = value;
            }
        }

        #endregion
    }
}
