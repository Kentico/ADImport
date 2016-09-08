using System;
using System.Windows.Forms;

namespace WinFormsFramework
{
    /// <summary>
    /// Provides thread-safe option to invoke code influencing UI.
    /// </summary>
    public class InvokeHelper : IDisposable
    {
        #region "Public properties"

        /// <summary>
        /// Gets control upon which the methods are being invoked.
        /// </summary>
        public Control Control
        {
            get;
            private set;
        }

        #endregion


        #region "Constructor"

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="control">Control upon which to invoke method(s)</param>
        public InvokeHelper(Control control)
        {
            Control = control;
        }

        #endregion


        #region "Methods"

        /// <summary>
        /// Thread-safe method invoker.
        /// </summary>
        /// <param name="methodInvoker">Method to invoke</param>
        public void InvokeMethod(MethodInvoker methodInvoker)
        {
            if (Control.InvokeRequired)
            {
                Control.Invoke(methodInvoker);
            }
            else
            {
                methodInvoker.Invoke();
            }
        }

        #endregion


        #region "IDisposable members"

        /// <summary>
        /// Disposes object.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }


        /// <summary>
        /// Disposes object.
        /// </summary>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // Free managed resources
            }
        }

        #endregion
    }
}