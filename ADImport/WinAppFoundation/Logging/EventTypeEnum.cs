using System;
using System.Linq;
using System.Text;

namespace WinAppFoundation
{
    /// <summary>
    /// Event type enumeration.
    /// </summary>
    public enum EventTypeEnum
    {
        /// <summary>
        /// Common event data.
        /// </summary>
        Data,

        /// <summary>
        /// Event notifying an action has started.
        /// </summary>
        ActionStart,

        /// <summary>
        /// Error event.
        /// </summary>
        Error,

        /// <summary>
        /// Silent message (does not appear in UI).
        /// </summary>
        Silent
    }
}
