using System.Windows;

namespace WPFFramework.Controls.Viewers
{
    /// <summary>
    /// Interaction logic for Header.xaml
    /// </summary>
    public partial class Header
    {
        #region "Properties"

        /// <summary>
        /// Indicates whether subtitle should be visible.
        /// </summary>
        public Visibility SubtitleVisibility
        {
            get
            {
                return lblSubtitle.Visibility;
            }
            set
            {
                lblSubtitle.Visibility = value;
            }
        }

        #endregion


        #region "Constrcutors"

        /// <summary>
        /// Default constructor.
        /// </summary>
        public Header()
        {
            InitializeComponent();
        }

        #endregion


        #region "Methods"

        /// <summary>
        /// Sets title text.
        /// </summary>
        /// <param name="title">Title</param>
        public void SetTitle(string title)
        {
            lblTitle.Content = title;
        }


        /// <summary>
        /// Sets subtitle text.
        /// </summary>
        /// <param name="subtitle">Subtitle</param>
        public void SetSubtitle(string subtitle)
        {
            lblSubtitle.Content = subtitle;
        }

        #endregion
    }
}
