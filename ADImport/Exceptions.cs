using System;

namespace ADImport
{
    /// <summary>
    /// Thrown when application runs under local (non-domain) account.
    /// </summary>
    public class LocalUserAccountException : Exception
    {
    }

    /// <summary>
    /// Thrown when error occurs during connecting to database.
    /// </summary>
    public class DCConnectionException : Exception
    {
    }
}