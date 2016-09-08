using System;
using System.Linq;
using System.Text;

namespace WinAppFoundation
{
    /// <summary>
    /// Provides helper methods related to DataSet.
    /// </summary>
    public static class DataSetHelper
    {
        /// <summary>
        /// Makes search phrase valid.
        /// </summary>
        /// <param name="toSearch">Value to search</param>
        /// <returns>Valid search pattern</returns>
        public static string EscapeLikeValue(string toSearch)
        {
            StringBuilder pattern = new StringBuilder();
            foreach (char c in toSearch)
            {
                switch (c)
                {
                    case ']':
                    case '[':
                    case '%':
                    case '*':
                        pattern.Append("[").Append(c).Append("]");
                        break;

                    case '\'':
                        pattern.Append("''");
                        break;

                    default:
                        pattern.Append(c);
                        break;
                }
            }
            return pattern.ToString();
        }
    }
}
