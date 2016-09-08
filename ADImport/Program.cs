using System;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.InteropServices;
using System.Windows.Forms;

using CMS.DataEngine;
using CMS.Base;
using CMS.Helpers;
using CMS.IO;

using WinAppFoundation;

namespace ADImport
{
    /// <summary>
    /// Entry point for the application.
    /// </summary>
    public static class Program
    {
        #region "Variables"

        private static IMessageLog mMessageLog = null;
        private static IPrincipalProvider mPrincipalProvider = null;

        #endregion


        #region "Properties"

        /// <summary>
        /// Gets object for logging events.
        /// </summary>
        public static IMessageLog MessageLog
        {
            get
            {
                if (mMessageLog == null)
                {
                    return mMessageLog ?? (mMessageLog = new CommandLineMessageLog(true));
                }
                return mMessageLog;
            }
        }


        /// <summary>
        /// Directory service provider.
        /// </summary>
        private static IPrincipalProvider PrincipalProvider
        {
            get
            {
                return mPrincipalProvider ?? (mPrincipalProvider = new ADProvider());
            }
        }

        #endregion


        #region "Main application

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main(string[] args)
        {
            string profileName = string.Empty;

            // Create new instance of AD provider
            ImportProfile.OnDirectoryControllerChanged += () => PrincipalProvider.ClearContext();

            // If there are some arguments specified
            if (args.Length != 0)
            {
                // Try to attach console
                if (!AttachConsole(-1))
                {
                    // Create new console
                    AllocConsole();
                }

                // For each argument
                for (int i = 0; i < args.Length; i++)
                {
                    string arg = args[i];

                    // If argument specifies profile
                    if ((arg == "/profile") || (arg == "-profile"))
                    {
                        // Get profile name
                        if ((i + 1) < args.Length)
                        {
                            if (profileName == string.Empty)
                            {
                                profileName = args[i + 1].Trim();
                            }
                        }
                    }
                    if ((arg == "/h") || (arg == "-h") || (arg == "--help") || (arg == "-?") || (arg == "/?"))
                    {
                        // Write help
                        Console.Write(ResHelper.GetString("Console_Help").Replace("\\n", "\n").Replace("\\r", "\r"));
                        return;
                    }
                }

                // If there was profile specified
                if (profileName != string.Empty)
                {
                    // If there is no such file
                    if (!File.Exists(profileName))
                    {
                        Console.WriteLine(ResHelper.GetString("Error_ProfileDoesNotExist", profileName));
                    }
                    else
                    {
                        // Try to get file info
                        FileInfo fi = FileInfo.New(profileName);

                        Console.WriteLine(ResHelper.GetString("Console_SelectedImportProfile", fi.FullName));

                        // Initialize import profile
                        string validationError = ImportProfile.InitializeImportProfile(profileName);
                        if (!String.IsNullOrEmpty(validationError))
                        {
                            Console.WriteLine(ResHelper.GetString("Error_ProfileIsNotValid"));
                            Console.WriteLine(validationError);
                        }
                        else
                        {
                            // Application is in console mode
                            ImportProfile.UsesConsole = true;

                            // Check permissions
                            string permissionsCheckResult = PrincipalProvider.CheckPermissions();
                            if (!string.IsNullOrEmpty(permissionsCheckResult))
                            {
                                Console.WriteLine(permissionsCheckResult);
                            }
                            else
                            {
                                // Initialize principal provider
                                bool providerInitialized = ValidatePrincipalProvider(PrincipalProvider);
                                bool databaseValid = ValidateDatabase();

                                if (providerInitialized && databaseValid)
                                {
                                    // Initialize CMS connection
                                    CMSImport.ConfigureApplicationSettings();

                                    // Perform CMS import
                                    CMSImport.Import(PrincipalProvider, MessageLog);
                                }
                            }
                        }
                    }
                }
                else
                {
                    // Write message
                    Console.WriteLine(ResHelper.GetString("Console_SpecifyProfile"));
                }
            }
            // Launch windows form application
            else
            {
                // Initialize application
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new ADWizard(PrincipalProvider));
            }
        }

        #endregion


        #region "Validation"

        private static bool ValidateDatabase()
        {
            bool validationResult = false;
            try
            {
                // Change message
                Console.WriteLine(ResHelper.GetString("Step2_CheckingLicense"));

                if (!string.IsNullOrEmpty(ImportProfile.SQLServerDatabase))
                {
                    // Try to open connection
                    SqlConnection sc = new SqlConnection(CMSImport.ConnectionString);

                    // Find out whether CMS version is correct
                    SqlCommand getVersion = new SqlCommand("SELECT [KeyValue] FROM [CMS_SettingsKey] WHERE [KeyName] = 'CMSDBVersion'", sc);

                    DataSet ds = null;
                    using (SqlDataAdapter dataAdapter = new SqlDataAdapter(getVersion))
                    {
                        ds = new DataSet();
                        dataAdapter.Fill(ds);
                    }

                    // Get current project version
                    string projectVersion = null;
                    if (!DataHelper.DataSourceIsEmpty(ds))
                    {
                        projectVersion = ValidationHelper.GetString(ds.Tables[0].Rows[0]["KeyValue"], string.Empty);
                    }

                    if (string.IsNullOrEmpty(projectVersion))
                    {
                        Console.WriteLine(ResHelper.GetString("Step2_OtherDB"));
                    }
                    else
                    {
                        Console.WriteLine(ResHelper.GetString("Step2_CheckingVersion"));

                        // Version has to correspond with DLL version
                        if (projectVersion.EqualsCSafe(ADWizard.SupportedVersion))
                        {
                            // Set new connection string
                            ConnectionHelper.ConnectionString = CMSImport.ConnectionString;

                            // Initialize application to enable usage of providers
                            CMSImport.CMSInit();

                            validationResult = true;
                            Console.WriteLine(ResHelper.GetString("Step2_ConnectionToDBSuccessfull"));
                        }
                        else
                        {
                            Console.WriteLine(ResHelper.GetString("Step2_WrongVersion", ADWizard.SupportedVersion, projectVersion));
                        }
                    }
                }
                else
                {
                    Console.WriteLine(ResHelper.GetString("Step2_SelectDatabase"));
                }
            }
            catch (SqlException)
            {
                validationResult = false;
                Console.WriteLine(ResHelper.GetString("Step2_OtherDB"));
            }
            return validationResult;
        }


        private static bool ValidatePrincipalProvider(IPrincipalProvider principalProvider)
        {
            bool providerInitialized = false;
            try
            {
                principalProvider.InitializeDomainContext();
                providerInitialized = true;
            }
            catch (LocalUserAccountException)
            {
                Console.WriteLine(ResHelper.GetString("Error_ConnectingAD") + AbstractResHelper.LINE_BREAK + ResHelper.GetString("Error_LocalUserAccountException"));
            }
            catch (DCConnectionException)
            {
                Console.WriteLine(ResHelper.GetString("Error_ConnectingAD") + AbstractResHelper.LINE_BREAK + ResHelper.GetString("Error_LocalUserAccountException"));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ResHelper.GetString("Error_ConnectingAD") + AbstractResHelper.LINE_BREAK + ex.Message);
            }
            return providerInitialized;
        }

        #endregion


        #region "DLL imports"

        /// <summary>
        /// Attaches the calling process to the console of the specified process.
        /// </summary>
        [DllImport("kernel32.dll")]
        private static extern bool AttachConsole(int dwProcessId);


        /// <summary>
        /// Allocates a new console for the calling process.
        /// </summary>
        /// <returns></returns>
        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool AllocConsole();

        #endregion
    }
}