using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace WinAppFoundation
{
    /// <summary>
    /// Base class for logging providers.
    /// </summary>
    public abstract class AbstractMessageLog : IMessageLog
    {
        #region "Properties"

        /// <summary>
        /// Logging provider.
        /// </summary>
        protected IMessageLog MessageLog
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
                return MessageLog.LogMessages;
            }
        }

        #endregion


        #region "Constructors"

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="messageLog">Logging provider</param>
        protected AbstractMessageLog(IMessageLog messageLog)
        {
            MessageLog = messageLog;
        }

        #endregion


        #region "Methods"

        /// <summary>
        /// Logs the event to the event log.
        /// </summary>
        /// <param name="message">Message to log</param>
        /// <param name="eventType">Type of the event</param>
        public virtual void LogEvent(string message, EventTypeEnum eventType = EventTypeEnum.Data)
        {
            MessageLog.LogEvent(message, eventType);
        }


        /// <summary>
        /// Logs event indicating that progress still continues.
        /// </summary>
        /// <param name="indicator">Indicator (e.g ".")</param>
        public virtual void IndicateActivity(string indicator = ".")
        {
            MessageLog.IndicateActivity(indicator);
        }


        /// <summary>
        /// Logs message to log.
        /// </summary>
        /// <param name="message">Message to log</param>
        /// <param name="ex">Possible exception to get message from</param>
        /// <param name="eventType">Type of the event</param>
        public virtual void LogError(string message, Exception ex = null, EventTypeEnum eventType = EventTypeEnum.Error)
        {
            MessageLog.LogError(message, ex, eventType);
        }

        #endregion
    }
}
