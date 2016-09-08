using System;
using System.IO;
using System.Linq;
using System.Text;

namespace WinAppFoundation
{
    /// <summary>
    /// Provides file logging functionality.
    /// </summary>
    public class FileMessageLog : AbstractMessageLog
    {
        #region "Variables"

        private string mLogFullPath = null;
        private readonly Encoding encoding = Encoding.UTF8;
        private bool mAddTimeStamp = true;
        private bool mAddType = false;

        #endregion


        #region "Private properties"

        private string LogFileFolder
        {
            get;
            set;
        }


        private string LogFileName
        {
            get;
            set;
        }


        private bool LogFileCreated
        {
            get;
            set;
        }


        private FileStream OutputFile
        {
            get;
            set;
        }


        /// <summary>
        /// Full log file path.
        /// </summary>
        private string LogFullPath
        {
            get
            {
                if (mLogFullPath == null)
                {
                    string tempFullPath = LogFileFolder ?? string.Empty;

                    // Use setup folder if relative path is used
                    tempFullPath = Path.IsPathRooted(LogFileName) ? LogFileName : Path.Combine(tempFullPath, LogFileName);
                    tempFullPath = Path.GetFullPath(tempFullPath);

                    mLogFullPath = tempFullPath;
                }
                return mLogFullPath;
            }
        }

        #endregion


        #region "Public properties"

        /// <summary>
        /// Whether to add timestamp to messages.
        /// </summary>
        public bool AddTimeStamp
        {
            get
            {
                return mAddTimeStamp;
            }
            set
            {
                mAddTimeStamp = value;
            }
        }


        /// <summary>
        /// Whether to add event type to messages.
        /// </summary>
        public bool AddType
        {
            get
            {
                return mAddType;
            }
            set
            {
                mAddType = value;
            }
        }

        #endregion


        #region "Constructors and destructors"

        /// <summary>
        /// Parametric constructor.
        /// </summary>
        /// <param name="messageLog">Logging object</param>
        /// <param name="logFileFolder">Folder for storing log</param>
        /// <param name="logFileName">Name of log file</param>
        public FileMessageLog(IMessageLog messageLog, string logFileFolder, string logFileName)
            : base(messageLog)
        {
            LogFileFolder = logFileFolder;
            LogFileName = logFileName;
        }


        /// <summary>
        /// Closes the output file.
        /// </summary>
        ~FileMessageLog()
        {
            if (OutputFile != null)
            {
                OutputFile.Close();
            }
        }

        #endregion


        #region "IMessageLog members"

        /// <summary>
        /// Logs information message.
        /// </summary>
        /// <param name="message">Message to log</param>
        /// <param name="eventType">Type of the event</param>
        public override void LogEvent(string message, EventTypeEnum eventType = EventTypeEnum.Data)
        {
            base.LogEvent(message, eventType);
            LogToFile(message, eventType);
        }


        /// <summary>
        /// Logs error message.
        /// </summary>
        /// <param name="message">Message to log (exception message and stack trace will be included)</param>
        /// <param name="ex">Exception</param>
        /// <param name="eventType">Type of the event</param>
        public override void LogError(string message, Exception ex = null, EventTypeEnum eventType = EventTypeEnum.Error)
        {
            base.LogError(message, ex, eventType);
            LogToFile(message, eventType, ex);
        }

        #endregion


        #region "Private methods"

        private void CreateOutputFile()
        {
            // Create directory to log file if does not exist
            string pathToDir = Path.GetDirectoryName(LogFullPath);
            if (!String.IsNullOrEmpty(pathToDir) && !Directory.Exists(pathToDir))
            {
                Directory.CreateDirectory(pathToDir);
            }

            if (String.IsNullOrEmpty(pathToDir) || Directory.Exists(pathToDir))
            {
                OutputFile = new FileStream(LogFullPath, FileMode.Create, FileAccess.Write, FileShare.ReadWrite);
            }
        }


        /// <summary>
        /// Logs message to log file
        /// </summary>
        /// <param name="message">Message to log</param>
        /// <param name="type">Type of message</param>
        /// <param name="ex">Exception to include to the logged message</param>
        private void LogToFile(string message, EventTypeEnum type, Exception ex = null)
        {
            try
            {
                if (!LogFileCreated)
                {
                    CreateOutputFile();
                    LogFileCreated = true;
                }

                if (OutputFile != null)
                {
                    StringBuilder sb = new StringBuilder();
                    if (AddTimeStamp)
                    {
                        sb.Append(DateTime.Now.ToString("[yyyy-MM-dd HH:mm:ss] "));
                    }
                    if (AddType)
                    {
                        sb.Append(type + " ");
                    }
                    sb.Append(message);
                    if (ex != null)
                    {
                        sb.Append(LogHelper.GetExceptionLogMessage(ex));
                    }
                    sb.AppendLine();

                    // Save complete message
                    message = sb.ToString();
                    OutputFile.Write(encoding.GetBytes(message), 0, encoding.GetByteCount(message));
                    OutputFile.Flush();
                }
            }
            catch
            {
                // Logging should not produce additional errors.
            }
        }

        #endregion
    }
}
