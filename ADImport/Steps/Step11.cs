using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;

using WinAppFoundation;

using WinFormsFramework;

namespace ADImport
{
    /// <summary>
    /// Step11 - Import.
    /// </summary>
    public partial class Step11 : AbstractStep
    {
        #region "Variables & constants"


        private static IMessageLog mCurrentLog = null;

        #endregion


        #region "Properties"

        /// <summary>
        /// Indicates whether step allows canceling its asynchronous action.
        /// </summary>
        public override bool AllowCancel
        {
            get
            {
                return true;
            }
        }


        /// <summary>
        /// Gets log instance.
        /// </summary>
        public static IMessageLog CurrentLog
        {
            get
            {
                return mCurrentLog ?? (mCurrentLog = new BaseMessageLog());
            }
            private set
            {
                mCurrentLog = value;
            }
        }

        #endregion


        #region "Constructors"

        /// <summary>
        /// Step11 constructor.
        /// </summary>
        public Step11()
            : this(null)
        {
        }

        /// <summary>
        /// Step11 constructor.
        /// </summary>
        public Step11(ADWizard wizard)
            : base(wizard)
        {
            InitializeComponent();
            CurrentLog.LogMessages.CollectionChanged += LogMessages_CollectionChanged;
        }

        #endregion


        #region "Control events"

        private void Step11_Load(object sender, EventArgs e)
        {
            Wizard.ButtonClose.Text = ResHelper.GetString("Wizard_Finish");

            // Handle events
            Wizard.StepLoadedEvent += Wizard_StepLoadedEvent;
            
            Wizard.SetWaitState(true, ResHelper.GetString("Step11_ImportInProgress"));

            // Perform CMS import
            CMSImport.ImportAsync(ADProvider, CurrentLog, FinishImport);            
        }


        private void Wizard_StepLoadedEvent(object sender, EventArgs e)
        {
            if (IsStepActive)
            {
                Wizard.ButtonNext.Enabled = false;
            }
        }


        private void LogMessages_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            using (InvokeHelper ih = new InvokeHelper(listLog))
            {
                switch (e.Action)
                {
                    case NotifyCollectionChangedAction.Reset:
                        ih.InvokeMethod(() => listLog.Items.Clear());
                        break;

                    case NotifyCollectionChangedAction.Add:
                        if (e.NewItems != null)
                        {
                            IEnumerable<LogItem> items = e.NewItems.Cast<LogItem>();
                            foreach (string message in items.Select(m => m.Message))
                            {
                                ih.InvokeMethod(() => listLog.Items.Insert(0, message));
                            }
                        }
                        break;

                    case NotifyCollectionChangedAction.Replace:
                        foreach (string message in CurrentLog.LogMessages.Select(m => m.Message))
                        {
                            ih.InvokeMethod(() => listLog.Items.Insert(0, message));
                        }
                        break;
                }
            }
        }

        #endregion


        #region "Methods"

        /// <summary>
        /// Ends local threads.
        /// </summary>
        public override void EndThreads()
        {
            CMSImport.CancelImport();
        }

        
        internal void FinishImport(object sender, RunWorkerCompletedEventArgs e)
        {            
            Wizard.SetControlEnabled(Wizard.ButtonClose, true);
            Wizard.SetWaitState(false, null);
        }

        #endregion
    }
}