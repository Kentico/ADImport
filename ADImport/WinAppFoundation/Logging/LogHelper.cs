using System;
using System.Linq;
using System.Text;

namespace WinAppFoundation
{
    /// <summary>
    /// Provides helper methods for logging events.
    /// </summary>
    public class LogHelper
    {
        /// <summary>
        /// Gets complete log for exception with message and deep stack trace.
        /// Gets logs for all inner exceptions when an AggregateException is passed.
        /// </summary>
        /// <param name="ex">Exception to log</param>
        public static string GetExceptionLogMessage(Exception ex)
        {
            StringBuilder message = new StringBuilder();

            if (ex != null)
            {
                if (ex is AggregateException)
                {
                    // Process all aggregated exceptions
                    foreach (var inner in ((AggregateException)ex).InnerExceptions)
                    {
                        message.Append(GetExceptionLogMessage(inner));
                    }
                }
                else
                {
                    message.AppendLine("Message: " + ex.Message);

                    // Add stack trace
                    message.AppendLine("Stack Trace: ");
                    if (ex.StackTrace != null)
                    {
                        message.AppendLine(ex.StackTrace);
                    }

                    // Add inner exception stack trace
                    while (ex.InnerException != null)
                    {
                        message.AppendLine(ex.InnerException.Message);
                        message.AppendLine(ex.InnerException.StackTrace);
                        ex = ex.InnerException;
                    }
                }
            }
            return message.ToString();
        }
    }
}
