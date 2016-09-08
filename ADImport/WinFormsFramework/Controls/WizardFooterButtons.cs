using System;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WinFormsFramework
{
    /// <summary>
    /// Control wrapping the footer buttons usually used by wizard forms.
    /// </summary>
    public partial class WizardFooterButtons : UserControl
    {
        #region "Properties"

        /// <summary>
        /// Button back
        /// </summary>
        public Button ButtonBack
        {
            get
            {
                return btnBack;
            }
        }


        /// <summary>
        /// Button next
        /// </summary>
        public Button ButtonNext
        {
            get
            {
                return btnNext;
            }
        }


        /// <summary>
        /// Button close, mutually exclusive with cancel button.
        /// </summary>
        public Button ButtonClose
        {
            get
            {
                return btnClose;
            }
        }


        /// <summary>
        /// Button cancel, mutually exclusive with close button.
        /// </summary>
        public Button ButtonCancel
        {
            get
            {
                return btnCancel;
            }
        }

        #endregion


        #region "Constructors"

        /// <summary>
        /// Initializes the control.
        /// </summary>
        public WizardFooterButtons()
        {
            InitializeComponent();
        }

        #endregion


        #region "Methods"

        /// <summary>
        /// Displays cancel button and hides close button.
        /// </summary>
        /// <param name="allowCancel">Indicates whether cancel button should be enabled</param>
        public void AsyncActionStart(bool allowCancel)
        {
            btnBack.Enabled = false;
            btnNext.Enabled = false;

            if (allowCancel)
            {
                btnCancel.Visible = true;
                btnClose.Visible = false;
            }
        }


        /// <summary>
        /// Displays close button and hides cancel button.
        /// </summary>
        public void AsyncActionFinish()
        {
            btnBack.Enabled = true;
            btnNext.Enabled = true;

            btnCancel.Visible = false;
            btnClose.Visible = true;
        }

        #endregion
    }
}
