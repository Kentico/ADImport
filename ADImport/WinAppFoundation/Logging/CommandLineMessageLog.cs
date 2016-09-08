using System;
using System.Collections.ObjectModel;
using System.Text;

namespace WinAppFoundation
{
    /// <summary>
    /// Class for logging messages to console or file.
    /// </summary>
    public class CommandLineMessageLog : IMessageLog
    {
        #region "Public properties"

        /// <summary>
        /// Indicates if logging to console is enabled.
        /// </summary>
        public bool LoggingEnabled
        {
            get;
            private set;
        }


        /// <summary>
        /// Gets collection of logging messages.
        /// </summary>
        public ObservableCollection<LogItem> LogMessages
        {
            get
            {
                return null;
            }
        }

        #endregion


        #region "Constructor"

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="loggingEnabled">Indicates whether the logging is enabled</param>
        public CommandLineMessageLog(bool loggingEnabled)
        {
            LoggingEnabled = loggingEnabled;
        }

        #endregion


        #region "Methods"

        /// <summary>
        /// Logs the event to the eventlog.
        /// </summary>
        /// <param name="message">Message to log</param>
        /// <param name="eventType">Type of the event</param>
        public void LogEvent(string message, EventTypeEnum eventType = EventTypeEnum.Data)
        {
            WriteToLog(DateTime.Now.ToString("[yyyy-MM-dd HH:mm:ss] ") + message);
        }


        /// <summary>
        /// Logs activity indicator or message (notifies user she needs to wait).
        /// </summary>
        /// <param name="indicator">Indicator (e.g ".")</param>
        public void IndicateActivity(string indicator = ".")
        {
            WriteToLog(indicator);
        }


        /// <summary>
        /// Logs message to log.
        /// </summary>
        /// <param name="message">Message to log</param>
        /// <param name="ex">Possible exception to get message from</param>
        /// <param name="eventType">Type of the event</param>
        public void LogError(string message, Exception ex = null, EventTypeEnum eventType = EventTypeEnum.Error)
        {
            WriteToLog(DateTime.Now.ToString("[yyyy-MM-dd HH:mm:ss] ") + message);
            if (ex != null)
            {
                WriteToLog(LogHelper.GetExceptionLogMessage(ex));
            }
        }


        /// <summary>
        /// Logs message to console.
        /// </summary>
        /// <param name="message">Message</param>
        private void WriteToLog(string message)
        {
            if (LoggingEnabled)
            {
                Console.WriteLine(message.Trim());
            }
        }

        #endregion
    }
}