using System.Threading;

namespace ADImport
{
    /// <summary>
    /// Helper for thread operations.
    /// </summary>
    public class ThreadHelper
    {
        /// <summary>
        /// Finalizes given thread.
        /// </summary>
        /// <param name="thread">Thread to end</param>
        public static void FinalizeThread(Thread thread)
        {
            if ((thread != null) && (thread.ThreadState != ThreadState.Stopped) && (thread.ThreadState != ThreadState.Aborted))
            {
                thread.Abort();

                while ((thread.ThreadState != ThreadState.Aborted) && (thread.ThreadState != ThreadState.Stopped))
                {
                    // Wait for thread to finish
                }
            }
        }
    }
}