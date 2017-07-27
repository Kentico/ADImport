using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows.Forms;

using WinAppFoundation;

using WinFormsFramework;

namespace ADImport
{
    /// <summary>
    /// Main form of ADImport.
    /// </summary>
    public partial class ADWizard : Form
    {
        #region "Private variables & constants"

        /// <summary>
        /// Current step
        /// </summary>
        private int currentStep = 0;


        /// <summary>
        /// Collection of steps
        /// </summary>
        private List<AbstractStep> mSteps = null;

        private static string mSupportedVersion = null;
        private static string mFileVersionInfo = null;
        
        #endregion


        #region "Delegates & events"

        /// <summary>
        /// Event raised when step is loaded
        /// </summary>
        public event EventHandler StepLoadedEvent;

        #endregion


        #region "Steps"

        /// <summary>
        /// Collection of steps.
        /// </summary>
        protected List<AbstractStep> Steps
        {
            get
            {
                return mSteps ?? (mSteps = new List<AbstractStep>
                {
                    null,
                    new Step1(this),
                    new Step2(this),
                    new Step3(this),
                    new Step4(this),
                    new Step5(this),
                    new Step6(this),
                    new Step7(this),
                    new Step8(this),
                    new Step9(this),
                    new Step10(this),
                    new Step11(this)
                });
            }
        }

        #endregion


        #region "Properties"

        private static string InformationalVersion
        {
            get
            {
                return mFileVersionInfo ?? (mFileVersionInfo = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).ProductVersion);
            }
        }


        /// <summary>
        /// Get current application version without suffix (Beta etc.).
        /// This version is used to validate supported project version.
        /// </summary>
        public static string SupportedVersion
        {
            get
            {
                if (string.IsNullOrEmpty(mSupportedVersion))
                {
                    Regex informationalRegex = new Regex(@"(?<version>\d+\.\d+)\.\d+\s*(?<suffix>\S*)");

                    string version = informationalRegex.Match(InformationalVersion).Groups["version"].Value;
                    string suffix = informationalRegex.Match(InformationalVersion).Groups["suffix"].Value;

                    mSupportedVersion = version + (string.IsNullOrEmpty(suffix) ? string.Empty : " " + suffix);
                }

                return mSupportedVersion;
            }
        }


        /// <summary>
        /// Directory service provider.
        /// </summary>
        public IPrincipalProvider PrincipalProvider
        {
            get;
            private set;
        }


        /// <summary>
        /// Gets "Next" button.
        /// </summary>
        public Button ButtonNext
        {
            get
            {
                return footerButtons.ButtonNext;
            }
        }


        /// <summary>
        /// Gets "Back" button.
        /// </summary>
        public Button ButtonBack
        {
            get
            {
                return footerButtons.ButtonBack;
            }
        }


        /// <summary>
        /// Gets "Cancel" button.
        /// </summary>
        public Button ButtonClose
        {
            get
            {
                return footerButtons.ButtonClose;
            }
        }


        /// <summary>
        /// Placeholder for steps.
        /// </summary>
        public Control WizardBody
        {
            get
            {
                return pnlBody;
            }
        }


        /// <summary>
        /// Gets current step.
        /// </summary>
        public AbstractStep CurrentStep
        {
            get
            {
                if (WizardBody.Controls.Count > 0)
                {
                    return (AbstractStep)WizardBody.Controls[0];
                }
                return null;
            }
        }

        #endregion


        #region "Constructors"

        /// <summary>
        /// Default constructor.
        /// </summary>
        protected ADWizard()
        {
            InitializeComponent();

            headerControl.SetSubtitle("Please choose the user name and role name format and specify mapping of user fields.");

            AcceptButton = footerButtons.ButtonNext;
            footerButtons.ButtonNext.Click += btnNext_Click;
            footerButtons.ButtonBack.Click += btnBack_Click;
            footerButtons.ButtonClose.Click += btnClose_Click;
            footerButtons.ButtonCancel.Click += btnCancel_Click;
        }


        /// <summary>
        /// Main constructor.
        /// </summary>
        /// <param name="principalProvider">Principal provider</param>
        public ADWizard(IPrincipalProvider principalProvider)
            : this()
        {
            if (principalProvider == null)
            {
                throw new ArgumentNullException("principalProvider");
            }
            PrincipalProvider = principalProvider;
        }

        #endregion


        #region "Button handling"

        private void btnNext_Click(object sender, EventArgs e)
        {
            // Make step
            MakeStep(true, true);
        }


        private void btnBack_Click(object sender, EventArgs e)
        {
            if (currentStep > 1)
            {
                // Make step
                MakeStep(false, false);
            }
        }


        private void btnClose_Click(object sender, EventArgs e)
        {
            // Close window
            Close();
        }


        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (CurrentStep != null)
            {
                // End all threads
                if (CurrentStep != null)
                {
                    DialogResult result = MessageBox.Show(ResHelper.GetString("Wizard_CancelImportConfirmation"), ResHelper.GetString("Wizard_CancelImportTitle"), MessageBoxButtons.YesNo);
                    if (result == DialogResult.Yes)
                    {
                        SetWaitState(false, null);
                        CurrentStep.EndThreads();
                    }
                }
            }
        }

        #endregion


        #region "Wizard events"

        private void ADWizard_Load(object sender, EventArgs e)
        {
            // Localize wizard
            ButtonBack.Text = ResHelper.GetString("Wizard_Back");
            ButtonClose.Text = ResHelper.GetString("Wizard_Close");
            footerButtons.ButtonCancel.Text = ResHelper.GetString("Wizard_Cancel");

            // Move to first step
            MakeStep(true, true);

            // Set form title (including version number)
            Text = ResHelper.GetString("Wizard_Title", SupportedVersion);

            ImportProfile.OnDirectoryControllerChanged += DirectoryControllerChanged;
            ImportProfile.OnSQLServerChanged += SQLServerChanged;
        }


        private void ADWizard_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Show confirmation when not last step
            if ((currentStep != 11) && (currentStep != 1))
            {
                DialogResult result = MessageBox.Show(ResHelper.GetString("Wizard_ConfirmExit"), ResHelper.GetString("Wizard_ConfirmExitCaption"), MessageBoxButtons.YesNo);
                if (result != DialogResult.Yes)
                {
                    // Cancel closing
                    e.Cancel = true;
                    return;
                }
            }

            // End all threads before application is closed
            if (CurrentStep != null)
            {
                CurrentStep.EndThreads();
            }
        }

        #endregion


        #region "Private methods"

        /// <summary>
        /// Performs step change of wizard.
        /// </summary>
        /// <param name="validate">Indicates whether to perform validation</param>
        /// <param name="forward">Whether to move forward or backward</param>
        private async void MakeStep(bool forward, bool validate)
        {
            int newStep = forward ? currentStep + 1 : currentStep - 1;
            
            // Adjust step
            switch (newStep)
            {
                case 8:
                    if (ImportProfile.Sites.Count == 0)
                    {
                        if (forward)
                        {
                            // Skip irrelevant steps
                            newStep = GetStepId<Step10>();
                        }
                    }
                    break;

                case 9:
                    if (ImportProfile.Sites.Count == 0)
                    {
                        if (!forward)
                        {
                            // Skip irrelevant steps
                            newStep = GetStepId<Step7>();
                        }
                    }
                    break;

            }

            AbstractStep fromStep = Steps[currentStep];
            AbstractStep toStep = Steps[newStep];

            bool valid = (fromStep == null) || !validate || await fromStep.IsValid();

            // If current step is valid
            if (valid)
            {
                try
                {
                    currentStep = newStep;

                    // Delete all controls from wizard
                    WizardBody.Controls.Clear();

                    // Step should fill parent panel
                    toStep.Dock = DockStyle.Fill;

                    // Add new step
                    WizardBody.Controls.Add(toStep);
                }
                catch (Exception ex)
                {
                    // Show message
                    ShowError(ResHelper.GetString("Error_MovingStep"), ResHelper.GetString("Error_MovingStepCaption"), ex);
                }
            }

            // Set step name and description
            headerControl.SetTitle(ResHelper.GetString("StepName_" + currentStep));
            headerControl.SetSubtitle(ResHelper.GetString("StepDescription_" + currentStep));

            // Ensure waiting state is turned off
            if (validate && (currentStep != 11))
            {
                SetWaitState(false, string.Empty);
            }

            // Set visibility of next and back buttons
            ButtonBack.Visible = ((currentStep != 1) && (currentStep != 11));
            ButtonNext.Visible = (currentStep != 11);

            // Set caption of next button
            ButtonNext.Text = ResHelper.GetString("Wizard_Next");
            ButtonNext.Enabled = true;

            // Raise step loaded event
            if (StepLoadedEvent != null)
            {
                StepLoadedEvent(this, null);
            }
        }


        /// <summary>
        /// Sets 'Enabled' property of control.
        /// </summary>
        /// <param name="control">Control to set property</param>
        /// <param name="enabled">Indicates whether control should be enabled</param>
        public void SetControlEnabled(Control control, bool enabled)
        {
            using (InvokeHelper ih = new InvokeHelper(control))
            {
                ih.InvokeMethod(() => control.Enabled = enabled);
            }
        }


        /// <summary>
        /// Displays waiting animation.
        /// </summary>
        /// <param name="wait">Indicates whether to wait</param>
        /// <param name="message">Message to display</param>
        public void SetWaitState(bool? wait, string message)
        {
            using (InvokeHelper ih = new InvokeHelper(this))
            {
                ih.InvokeMethod(() => SetWaitStateInternal(wait, message));
            }
        }


        /// <summary>
        /// Displays waiting animation.
        /// </summary>
        /// <param name="wait">Indicates whether to wait</param>
        /// <param name="message">Message to display</param>
        private void SetWaitStateInternal(bool? wait, string message)
        {
            if (wait != null)
            {
                progressBarWaiting.Visible = wait.Value;
                lblState.Visible = wait.Value;

                if (wait.Value)
                {
                    footerButtons.AsyncActionStart(CurrentStep.AllowCancel);
                }
                else
                {
                    footerButtons.AsyncActionFinish();
                }
            }

            lblState.Text = !string.IsNullOrEmpty(message) ? message : string.Empty;
            ButtonNext.Focus();
        }


        /// <summary>
        /// Displays error.
        /// </summary>
        /// <param name="text">Error text</param>
        /// <param name="caption">Caption of error window</param>
        /// <param name="ex">Exception to show</param>
        private static void ShowError(string text, string caption, Exception ex)
        {
            if (ex != null)
            {
                text += AbstractResHelper.LINE_BREAK + ex.Message;
            }
            MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }


        /// <summary>
        /// Determines whether given step is currently loaded in wizard.
        /// </summary>
        /// <param name="step">Step control to examine</param>
        /// <returns>TRUE if step is currently active</returns>
        public bool IsStepActive(AbstractStep step)
        {
            return step == CurrentStep;
        }


        private void SQLServerChanged()
        {
            ImportProfile.Sites.Clear();

            DisposeStep<Step4>();
            DisposeStep<Step5>();
            DisposeStep<Step9>();
        }


        private void DirectoryControllerChanged()
        {
            ImportProfile.Users.Clear();
            ImportProfile.Groups.Clear();

            DisposeStep<Step5>();
            DisposeStep<Step6>();
            DisposeStep<Step7>();
            DisposeStep<Step8>();
        }


        private int GetStepId<T>() where T : AbstractStep
        {
            return Steps.FindIndex(s => s is T);
        }


        private void DisposeStep<T>() where T : AbstractStep, new()
        {
            int i = GetStepId<T>();
            Steps[i].Dispose();
            Steps[i] = new T();
            Steps[i].Wizard = this;
        }

        #endregion
    }
}