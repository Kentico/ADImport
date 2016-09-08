using System;
using System.Collections.ObjectModel;

namespace WinAppFoundation
{
    /// <summary>
    /// Interface for the progress logging component.
    /// </summary>
    public interface IMessageLog
    {
        /// <summary>
        /// Gets collection of logging messages.
        /// </summary>
        ObservableCollection<LogItem> LogMessages
        {
            get;
        }
        

        /// <summary>
        /// Logs the event to the event log.
        /// </summary>
        /// <param name="message">Message to log</param>
        /// <param name="eventType">Type of the event</param>
        void LogEvent(string message, EventTypeEnum eventType = EventTypeEnum.Data);


        /// <summary>
        /// Logs message to log.
        /// </summary>
        /// <param name="message">Message to log</param>
        /// <param name="ex">Possible exception to get message from</param>
        /// <param name="eventType">Type of the event</param>
        void LogError(string message, Exception ex = null, EventTypeEnum eventType = EventTypeEnum.Error);


        /// <summary>
        /// Logs event indicating that progress still continues.
        /// </summary>
        /// <param name="indicator">Indicator (e.g ".")</param>
        void IndicateActivity(string indicator = ".");
    }
}