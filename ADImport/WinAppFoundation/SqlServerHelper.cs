using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;

using Microsoft.Win32;

namespace WinAppFoundation
{
    /// <summary>
    /// Provides information about SQL servers, LocalDBs and native clients.
    /// </summary>
    public static class SqlServerHelper
    {
        #region "Constants"

        /// <summary>
        /// Timeout in seconds.
        /// </summary>
        private const int SQL_TIMEOUT = 5;

        #endregion


        #region "Public methods"

        /// <summary>
        /// Returns names of all available SQL instances.
        /// </summary>
        public static List<string> GetNeighbouringSQLInstanceNames()
        {
            List<string> names = new List<string>();

            DataTable sqlServers = SqlDataSourceEnumerator.Instance.GetDataSources();
            if (sqlServers.Rows.Count > 0)
            {
                foreach (DataRow sqlServer in sqlServers.Rows)
                {
                    string serverName = sqlServer["ServerName"].ToString();
                    string instanceName = sqlServer["InstanceName"].ToString();

                    if (!String.IsNullOrEmpty(instanceName))
                    {
                        // Append the instance name
                        serverName = String.Format("{0}\\{1}", serverName, instanceName);
                    }
                    if (!String.IsNullOrEmpty(serverName))
                    {
                        names.Add(serverName);
                    }
                }
                names.Sort();
            }

            return names;
        }


        /// <summary>
        /// Gets a sorted list of database names.
        /// </summary>
        /// <param name="baseConnectionString">Connection string without database name</param>
        public static List<string> GetDatabaseNames(string baseConnectionString)
        {
            List<string> databaseNames = new List<string>();

            if (!String.IsNullOrEmpty(baseConnectionString))
            {
                SqlCommand command = new SqlCommand
                {
                    Connection = new SqlConnection(baseConnectionString),
                    CommandType = CommandType.Text,
                    CommandText = "SELECT name FROM sys.databases"
                };

                using (SqlDataAdapter sda = new SqlDataAdapter(command))
                {
                    DataSet ds = new DataSet();
                    if (sda.Fill(ds) > 0)
                    {
                        databaseNames.AddRange(ds.Tables[0].Rows.Cast<DataRow>().Select(dataRow => dataRow["name"].ToString()));
                        databaseNames.Sort();
                    }
                }
            }

            return databaseNames;
        }


        /// <summary>
        /// Creates connection string for CMS database.
        /// </summary>
        /// <param name="serverAddress">SQL server name or address</param>
        /// <param name="databaseName">SQL database name - pass null or empty to receive base connection string.</param>
        /// <param name="integratedSecurity">Indicates whether connection should use integrated security.</param>
        /// <param name="userName">SQL server user name</param>
        /// <param name="password">SQL server password</param>
        public static string GetCMSConnectionString(string serverAddress, string databaseName, bool integratedSecurity, string userName, string password)
        {
            if (!String.IsNullOrEmpty(serverAddress) && (integratedSecurity || !String.IsNullOrEmpty(userName)))
            {
                // Create connection string
                SqlConnectionStringBuilder connectionString = new SqlConnectionStringBuilder
                {
                    DataSource = serverAddress,
                    ConnectTimeout = SQL_TIMEOUT,
                    IntegratedSecurity = integratedSecurity
                };

                if (!String.IsNullOrEmpty(databaseName))
                {
                    connectionString.InitialCatalog = databaseName;
                }

                // Set other properties if windows authentication is not used
                if (!integratedSecurity)
                {
                    connectionString.PersistSecurityInfo = false;
                    connectionString.UserID = userName;
                    connectionString.Password = password;
                    connectionString.CurrentLanguage = "English";
                }
                return connectionString.ToString();
            }

            return null;
        }

        #endregion
    }
}
