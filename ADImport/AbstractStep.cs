using System.Drawing;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using WinFormsFramework;

namespace ADImport
{
    /// <summary>
    /// Base class for 'Step' controls. Provides common functionality.
    /// </summary>
    public partial class AbstractStep : UserControl
    {
        #region "Variables"

        private ADWizard mWizard = null;

        #endregion


        #region "Properties"

        /// <summary>
        /// Step name.
        /// </summary>
        public virtual string StepLabel
        {
            get
            {
                return string.Empty;
            }
        }


        /// <summary>
        /// Step description.
        /// </summary>
        public virtual string StepDescription
        {
            get
            {
                return string.Empty;
            }
        }


        /// <summary>
        /// Label used for displaying info/warning/error messages.
        /// </summary>
        public virtual Label DefaultMessageLabel
        {
            get
            {
                return null;
            }
        }


        /// <summary>
        /// Wizard form.
        /// </summary>
        public ADWizard Wizard
        {
            get
            {
                return mWizard;
            }
            set
            {
                mWizard = value;
            }
        }


        /// <summary>
        /// Directory service provider.
        /// </summary>
        public IPrincipalProvider ADProvider
        {
            get
            {
                return Wizard.PrincipalProvider;
            }
        }


        /// <summary>
        /// Gets whether this step is currently loaded in wizard.
        /// </summary>
        public bool IsStepActive
        {
            get
            {
                return Wizard.IsStepActive(this);
            }
        }


        /// <summary>
        /// Indicates whether step allows canceling its asynchronous action.
        /// </summary>
        public virtual bool AllowCancel
        {
            get
            {
                return false;
            }
        }

        #endregion


        #region "Constructors"

        /// <summary>
        /// Constructor used for correct display of Visual Designer.
        /// </summary>
        public AbstractStep()
        {
            InitializeComponent();
        }


        /// <summary>
        /// Constructor.
        /// </summary>
        public AbstractStep(ADWizard wizard)
            : this()
        {
            Wizard = wizard;
        }

        #endregion


        #region "Methods"

        /// <summary>
        /// Check if current step is valid.
        /// </summary>
        public virtual Task<bool> IsValid()
        {
            return Task.FromResult(true);
        }


        /// <summary>
        /// Ends all step threads.
        /// </summary>
        public virtual void EndThreads()
        {
        }


        /// <summary>
        /// Sets error message.
        /// </summary>
        /// <param name="message">Error message</param>
        public void SetError(string message)
        {
            SetError(DefaultMessageLabel, message);
        }


        /// <summary>
        /// Sets error message.
        /// </summary>
        /// <param name="label">Label to use</param>
        /// <param name="message">Error message</param>
        public void SetError(Label label, string message)
        {
            using (InvokeHelper ih = new InvokeHelper(label))
            {
                ih.InvokeMethod(() => SetMessageInternal(label, message, true));
            }
        }


        /// <summary>
        /// Sets information message.
        /// </summary>
        /// <param name="message">Information message</param>
        public void SetMessage(string message)
        {
            SetMessage(DefaultMessageLabel, message);
        }


        /// <summary>
        /// Sets information message.
        /// </summary>
        /// <param name="label">Label to use</param>
        /// <param name="message">Information message</param>
        public void SetMessage(Label label, string message)
        {
            using (InvokeHelper ih = new InvokeHelper(label))
            {
                ih.InvokeMethod(() => SetMessageInternal(label, message, false));
            }
        }


        /// <summary>
        /// Sets message.
        /// </summary>
        /// <param name="label">Label to use</param>
        /// <param name="message">Error message</param>
        /// <param name="isError">Indicates whether message is error</param>
        private void SetMessageInternal(Label label, string message, bool isError)
        {
            label.Visible = true;
            label.Text = ResHelper.GetString(message);
            label.ForeColor = isError ? Color.Red : SystemColors.ControlText;
        }

        #endregion
    }
}