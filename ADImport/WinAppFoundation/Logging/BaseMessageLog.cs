using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Threading;

namespace WinAppFoundation
{
    /// <summary>
    /// Provides basic logging.
    /// </summary>
    public class BaseMessageLog : IMessageLog
    {
        #region "Variables"

        private readonly ObservableCollection<LogItem> mLogMessages = new ObservableCollection<LogItem>();

        private readonly ObservableCollection<string> mSilentMessages = new ObservableCollection<string>();

        #endregion


        #region "Properties"

        /// <summary>
        /// Gets collection of logging messages.
        /// </summary>
        public ObservableCollection<LogItem> LogMessages
        {
            get
            {
                return mLogMessages;
            }
        }


        /// <summary>
        /// Gets collection of silent messages.
        /// </summary>
        public ObservableCollection<string> SilentMessages
        {
            get
            {
                return mSilentMessages;
            }
        }


        private Dispatcher Dispatcher
        {
            get;
            set;
        }

        #endregion


        #region "Constructors"

        /// <summary>
        /// Default constructor.
        /// </summary>
        public BaseMessageLog(Dispatcher dispatcher = null)
        {
            Dispatcher = dispatcher ?? Dispatcher.CurrentDispatcher;
        }

        #endregion


        #region "Methods"

        /// <summary>
        /// Logs the event to the event log.
        /// </summary>
        /// <param name="message">Message to log</param>
        /// <param name="eventType">Type of the event</param>
        public void LogEvent(string message, EventTypeEnum eventType = EventTypeEnum.Data)
        {
            Dispatcher.Invoke(new Action(() =>
            {
                string[] lines = (message ?? String.Empty).Split(new[] { Environment.NewLine }, StringSplitOptions.None);

                foreach (string line in lines.Reverse())
                {
                    if (eventType == EventTypeEnum.Silent)
                    {
                        SilentMessages.Insert(0, line);
                    }
                    else
                    {
                        LogMessages.Insert(0, new LogItem { Message = line, EventType = eventType });
                    }
                }
            }));
        }


        /// <summary>
        /// Logs message to log.
        /// </summary>
        /// <param name="message">Message to log</param>
        /// <param name="ex">Possible exception to get message from</param>
        /// <param name="eventType">Type of the event</param>
        public void LogError(string message, Exception ex = null, EventTypeEnum eventType = EventTypeEnum.Error)
        {
            if (ex != null)
            {
                message += Environment.NewLine + LogHelper.GetExceptionLogMessage(ex);
            }
            LogEvent(message, eventType);
        }


        /// <summary>
        /// Logs event indicating that progress still continues.
        /// </summary>
        /// <param name="indicator">Indicator (e.g ".")</param>
        public void IndicateActivity(string indicator = ".")
        {
            Dispatcher.Invoke(new Action(() =>
            {
                LogItem message = new LogItem(LogMessages[0]);
                message.Message += indicator;
                LogMessages[0] = message;
            }));
        }

        #endregion
    }
}
