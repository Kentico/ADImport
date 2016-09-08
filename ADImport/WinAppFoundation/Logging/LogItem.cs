using System;
using System.Linq;
using System.Text;

namespace WinAppFoundation
{
    /// <summary>
    /// Represents one record in log.
    /// </summary>
    public class LogItem
    {
        #region "Properties"

        /// <summary>
        /// Type of event.
        /// </summary>
        public EventTypeEnum EventType
        {
            get;
            set;
        }


        /// <summary>
        /// Event message.
        /// </summary>
        public string Message
        {
            get;
            set;
        }


        /// <summary>
        /// Time of event
        /// </summary>
        public DateTime Time
        {
            get;
            private set;
        }

        #endregion


        #region "Constructors"

        /// <summary>
        /// Default constructor.
        /// </summary>
        public LogItem()
        {
            Time = DateTime.Now;
        }


        /// <summary>
        /// Clones item.
        /// </summary>
        public LogItem(LogItem item)
            : this()
        {
            Message = item.Message;
            EventType = item.EventType;
        }

        #endregion
    }
}
