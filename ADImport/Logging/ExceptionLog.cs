using System;
using System.Collections;
using System.Linq;
using System.Text;

namespace ADImport
{
    /// <summary>
    /// Collects exceptions during whole import process.
    /// </summary>
    public class ExceptionLog
    {
        #region "Variables"

        /// <summary>
        /// Logs with potential exceptions.
        /// </summary>
        private string mLog = null;

        #endregion


        #region "Properties"

        /// <summary>
        /// Gets log with potential exceptions.
        /// </summary>
        public string Log
        {
            get
            {
                return mLog;
            }
            private set
            {
                mLog = value;
            }
        }

        #endregion


        #region "Methods"

        /// <summary>
        /// Adds exception to ExceptionLog property.
        /// </summary>
        /// <param name="exception">Exception to add</param>
        public void AddException(Exception exception)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine(exception.Message);
            stringBuilder.AppendLine(string.Empty);
            stringBuilder.AppendLine(exception.StackTrace);
            // Append extra data
            foreach (DictionaryEntry dictionaryEntry in exception.Data)
            {
                stringBuilder.AppendLine("[" + dictionaryEntry.Key + "] : " + dictionaryEntry.Value);
            }
            stringBuilder.AppendLine(string.Empty);
            stringBuilder.AppendLine(string.Empty);
            Log = stringBuilder + Log;
        }

        #endregion
    }
}
