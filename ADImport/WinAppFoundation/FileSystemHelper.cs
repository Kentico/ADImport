using System;
using System.IO;
using System.Linq;

namespace WinAppFoundation
{
    /// <summary>
    /// Provides basic operations upon file system.
    /// </summary>
    public class FileSystemHelper
    {
        /// <summary>
        /// Validates given path and returns error message.
        /// </summary>
        /// <param name="path">Path to validate</param>
        public static string ValidatePath(string path)
        {
            // Check emptiness
            if (String.IsNullOrEmpty(path) || String.IsNullOrEmpty(path.Trim()))
            {
                return ResHelper.GetString("path.mustnotbeempty");
            }

            // Check length
            if (path.Length > 260)
            {
                return ResHelper.GetString("path.istoolong");
            }

            // Check invalid characters
            if (Path.GetInvalidPathChars().Any(path.Contains))
            {
                return ResHelper.GetString("path.containsinvalidchars");
            }

            try
            {
                path = Path.GetFullPath(path);
            }
            catch (Exception ex)
            {
                return ResHelper.GetString("path.isnotvalid", path, ex.Message);
            }

            return null;
        }
    }
}
