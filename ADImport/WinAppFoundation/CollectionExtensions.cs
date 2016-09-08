using System;
using System.Collections;
using System.Linq;
using System.Text;

namespace WinAppFoundation
{
    /// <summary>
    /// Provides extensions for collections such as IEnumerable.
    /// </summary>
    public static class CollectionExtensions
    {
        /// <summary>
        /// Disposes all objects in collection.
        /// </summary>
        /// <param name="set">Set whose members will be disposed.</param>
        public static void DisposeAll(this IEnumerable set)
        {
            foreach (Object obj in set)
            {
                IDisposable disposable = obj as IDisposable;
                if (disposable != null)
                {
                    disposable.Dispose();
                }
            }
        }


        /// <summary>
        /// Disposes all objects in collection.
        /// </summary>
        /// <param name="set">Set whose members will be disposed.</param>
        public static void DisposeAll(this IDictionary set)
        {
            foreach (DictionaryEntry dictionaryEntry in set)
            {
                IDisposable disposable = dictionaryEntry.Value as IDisposable;
                if (disposable != null)
                {
                    disposable.Dispose();
                }
            }
        }
    }
}
