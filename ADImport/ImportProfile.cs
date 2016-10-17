using System;
using System.Collections.Generic;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Xml;
using System.Xml.Schema;

using CMS.Helpers;
using CMS.IO;
using CMS.Base;

using WinAppFoundation;

namespace ADImport
{
    /// <summary>
    /// Serializable data container class for storing import settings.
    /// </summary>
    public static class ImportProfile
    {
        #region "Constants"

        private const string initialVector = "]pouuug]!NqpP':S";
        private const string password = "In&eHNb:d`+07Gx2";

        /// <summary>
        /// Extension of import profile.
        /// </summary>
        public const string PROFILE_EXTENSION = ".xml";

        /// <summary>
        /// Path to embedded import profile XML schema.
        /// </summary>
        private const string PROFILE_XSD = "profile.xsd";

        #endregion


        #region "Delegates and events"

        /// <summary>
        /// Raised when connection to the directory controller has changed.
        /// </summary>
        public delegate void DirectoryControllerChanged();


        /// <summary>
        /// Raised when connection to the directory controller has changed.
        /// </summary>
        public static DirectoryControllerChanged OnDirectoryControllerChanged = null;


        /// <summary>
        /// Raised when connection to the SQL server has changed.
        /// </summary>
        public delegate void SQLServerChanged();


        /// <summary>
        /// Raised when connection to the SQL server has changed.
        /// </summary>
        public static SQLServerChanged OnSQLServerChanged = null;

        #endregion


        #region "Private variables"


        #region "Step 1"

        /// <summary>
        /// Full path to import profile file.
        /// </summary>
        private static string mImportProfileFilename = string.Empty;

        /// <summary>
        /// Determines whether application is in console mode.
        /// </summary>
        private static bool mUsesConsole = false;

        /// <summary>
        /// Determines whether profile is valid.
        /// </summary>
        private static string validationError = null;

        #endregion


        #region "Step 2"

        /// <summary>
        /// Domain name of IP address of SQL Server.
        /// </summary>
        private static string mSQLServerAddress = null;

        /// <summary>
        /// Database (schema) of CMS.
        /// </summary>
        private static string mSQLServerDatabase = null;

        /// <summary>
        /// Username for connecting to SQL server.
        /// </summary>
        private static string mSQLServerUsername = null;

        /// <summary>
        /// Password for connecting to SQL server.
        /// </summary>
        private static string mSQLServerPassword = null;

        /// <summary>
        /// Determines whether to use windows authentication or not.
        /// </summary>
        private static bool mSQLUseTrustedConnection = true;

        #endregion


        #region "Step 3"

        /// <summary>
        /// Determines whether to use current user account to connect to domain controller.
        /// </summary>
        private static bool mADUseCurrentUserAccount = true;

        /// <summary>
        /// Domain name or IP address of domain controller.
        /// </summary>
        private static string mADControllerAddress = null;

        /// <summary>
        /// Username for connecting to domain controller.
        /// </summary>
        private static string mADUsername = null;

        /// <summary>
        /// Password for connecting to domain controller.
        /// </summary>
        private static string mADPassword = null;

        #endregion


        #region "Step 4"

        /// <summary>
        /// All - Imports all users, Selected - Imports only selected users, UpdateSelectedImportNew - New users will be imported and selected users will be imported/updated
        /// </summary>
        private static ImportType mImportUsersType = ImportType.All;

        /// <summary>
        /// All - Imports all groups, Selected - Imports only selected groups, UpdateSelectedImportNew - New groups will be imported and selected groups will be imported/updated
        /// </summary>
        private static ImportType mImportRolesType = ImportType.All;

        /// <summary>
        /// Determines whether to update data of CMS roles and users.
        /// </summary>
        private static bool mUpdateObjectData = true;

        /// <summary>
        /// Determines whether during importing new users only users from selected roles will be imported.
        /// </summary>
        private static bool mImportNewUsersOnlyFromSelectedRoles = false;

        /// <summary>
        /// Determines whether all users from selected groups are imported and other users are ignored.
        /// </summary>
        private static bool mImportUsersOnlyFromSelectedRoles = false;

        /// <summary>
        /// Determines whether to update users' memberships in roles.
        /// </summary>
        private static bool mUpdateMemberships = true;

        /// <summary>
        /// TRUE = Deletes users and roles (having IsDomain flag) that no longer exists in Active Directory.
        /// </summary>
        private static bool mDeleteNotExistingObjects = true;

        /// <summary>
        /// Determines whether to log import process.
        /// </summary>
        private static bool mLogImportProcess = true;

        /// <summary>
        /// Path for storing log.
        /// </summary>
        private static string mLogPath = "adimport.log";

        #endregion


        #region "Step 5"

        /// <summary>
        /// Enumeration determining which format of name should be used for user name during import.
        /// </summary>
        private static CodenameFormat mUsernameFormat = CodenameFormat.DomainAndSAM;

        /// <summary>
        /// Values in format CMSProperty, ADProperty.
        /// </summary>
        private static Dictionary<string, string> mUserProperties = null;

        /// <summary>
        /// Enumeration determining which format of name should be used for display name during import.
        /// </summary>
        private static CodenameFormat mRoleDisplayNameFormat = CodenameFormat.DomainAndSAM;

        /// <summary>
        /// Enumeration determining which format of name should be used for code name during import.
        /// </summary>
        private static CodenameFormat mRoleCodeNameFormat = CodenameFormat.DomainAndSAM;

        /// <summary>
        /// Determines whether to import description of role.
        /// </summary>
        private static bool mImportRoleDescription = true;

        /// <summary>
        /// Configures users as CMS editors.
        /// </summary>
        private static bool mConfigureAsCMSEditor = true;

        /// <summary>
        /// Mode of user property binding editor.
        /// </summary>
        private static BindingEditorMode mBindingEditorMode = BindingEditorMode.Simple;

        #endregion


        #region "Step 6, 7 and 8"

        /// <summary>
        /// Collection containing user identifiers to import.
        /// </summary>
        private static List<object> mUsers = null;

        /// <summary>
        /// Collection containing group identifiers to import.
        /// </summary>
        private static List<object> mGroups = null;

        #endregion


        #region "Step 9"

        /// <summary>
        /// Key = Site codename, Value = collection of role identifiers.
        /// </summary>
        private static Dictionary<string, List<Guid>> mSites = null;

        #endregion


        #region "Step 10"

        /// <summary>
        /// Determines whether to import now.
        /// </summary>
        private static bool mImportNow = true;

        /// <summary>
        /// Determines whether to save import profile.
        /// </summary>
        private static bool mSaveImportProfile = false;

        #endregion


        #endregion


        #region "Properties"

        #region "Other properties"

        /// <summary>
        /// Filter pattern for file dialogs.
        /// </summary>
        public static string FileFilter
        {
            get
            {
                return ResHelper.GetString("General_XMLFiles") + " (*" + PROFILE_EXTENSION + ")|*" + PROFILE_EXTENSION;
            }
        }

        #endregion


        #region "Step 1"

        /// <summary>
        /// Full path to import profile file.
        /// </summary>
        public static string ImportProfileFilename
        {
            get
            {
                return mImportProfileFilename;
            }
            set
            {
                mImportProfileFilename = value;
            }
        }


        /// <summary>
        /// Determines whether application is in console mode.
        /// </summary>
        public static bool UsesConsole
        {
            get
            {
                return mUsesConsole;
            }
            set
            {
                mUsesConsole = value;
            }
        }


        /// <summary>
        /// Actual import profile XML.
        /// </summary>
        public static string ImportProfileXML
        {
            get
            {
                return GetImportProfile();
            }
        }

        #endregion


        #region "Step 2"

        /// <summary>
        /// Domain name of IP address of SQL Server.
        /// </summary>
        public static string SQLServerAddress
        {
            get
            {
                return mSQLServerAddress;
            }
            set
            {
                if (value != mSQLServerAddress)
                {
                    mSQLServerAddress = value;
                    // If value has changed
                    RaiseSQLServerChanged();
                }
            }
        }


        /// <summary>
        /// Database (schema) of CMS.
        /// </summary>
        public static string SQLServerDatabase
        {
            get
            {
                return mSQLServerDatabase;
            }
            set
            {
                if (value != mSQLServerDatabase)
                {
                    mSQLServerDatabase = value;
                    // If value has changed
                    RaiseSQLServerChanged();
                }
            }
        }


        /// <summary>
        /// Username for connecting to SQL server.
        /// </summary>
        public static string SQLServerUsername
        {
            get
            {
                return mSQLServerUsername;
            }
            set
            {
                if (value != mSQLServerUsername)
                {
                    mSQLServerUsername = value;
                    // If value has changed
                    RaiseSQLServerChanged();
                }
            }
        }


        /// <summary>
        /// Password for connecting to SQL server.
        /// </summary>
        public static string SQLServerPassword
        {
            get
            {
                return mSQLServerPassword;
            }
            set
            {
                if (value != mSQLServerPassword)
                {
                    mSQLServerPassword = value;
                    // If value has changed
                    RaiseSQLServerChanged();
                }
            }
        }


        /// <summary>
        /// Determines whether to use windows authentication or not.
        /// </summary>
        public static bool SQLUseTrustedConnection
        {
            get
            {
                return mSQLUseTrustedConnection;
            }
            set
            {
                if (value != mSQLUseTrustedConnection)
                {
                    mSQLUseTrustedConnection = value;
                    // If value has changed
                    RaiseSQLServerChanged();
                }
            }
        }

        #endregion


        #region "Step 3"

        /// <summary>
        /// Determines whether to use current user account to connect to domain controller.
        /// </summary>
        public static bool ADUseCurrentUserAccount
        {
            get
            {
                return mADUseCurrentUserAccount;
            }
            set
            {
                if (value != mADUseCurrentUserAccount)
                {
                    mADUseCurrentUserAccount = value;
                    // If value has changed
                    RaiseDirectoryControllerChanged();
                }
            }
        }


        /// <summary>
        /// Domain name or IP address of domain controller.
        /// </summary>
        public static string ADControllerAddress
        {
            get
            {
                return mADControllerAddress;
            }
            set
            {
                if (value != mADControllerAddress)
                {
                    mADControllerAddress = value;
                    // If value has changed
                    RaiseDirectoryControllerChanged();
                }
            }
        }


        /// <summary>
        /// Username for connecting to domain controller.
        /// </summary>
        public static string ADUsername
        {
            get
            {
                return mADUsername;
            }
            set
            {
                if (value != mADUsername)
                {
                    mADUsername = value;
                    // If value has changed
                    RaiseDirectoryControllerChanged();
                }
            }
        }


        /// <summary>
        /// Password for connecting to domain controller.
        /// </summary>
        public static string ADPassword
        {
            get
            {
                return mADPassword;
            }
            set
            {
                if (value != mADPassword)
                {
                    mADPassword = value;
                    // If value has changed
                    RaiseDirectoryControllerChanged();
                }
            }
        }

        #endregion


        #region "Step 4 and 9"

        /// <summary>
        /// All - Imports all users, Selected - Imports only selected users, UpdateSelectedImportNew - New users will be imported and selected users will be imported/updated
        /// </summary>
        public static ImportType ImportUsersType
        {
            get
            {
                return mImportUsersType;
            }
            set
            {
                mImportUsersType = value;
            }
        }


        /// <summary>
        /// All - Imports all groups, Selected - Imports only selected groups, UpdateSelectedImportNew - New groups will be imported and selected groups will be imported/updated
        /// </summary>
        public static ImportType ImportRolesType
        {
            get
            {
                return mImportRolesType;
            }
            set
            {
                mImportRolesType = value;
            }
        }


        /// <summary>
        /// Determines whether to update data of CMS roles and users.
        /// </summary>
        public static bool UpdateObjectData
        {
            get
            {
                return mUpdateObjectData;
            }
            set
            {
                mUpdateObjectData = value;
            }
        }


        /// <summary>
        /// Determines whether during importing new users only users from selected roles will be imported.
        /// </summary>
        public static bool ImportNewUsersOnlyFromSelectedRoles
        {
            get
            {
                return mImportNewUsersOnlyFromSelectedRoles;
            }
            set
            {
                mImportNewUsersOnlyFromSelectedRoles = value;
            }
        }


        /// <summary>
        /// Determines whether all users from selected groups are imported and other users are ignored.
        /// </summary>
        public static bool ImportUsersOnlyFromSelectedRoles
        {
            get
            {
                return mImportUsersOnlyFromSelectedRoles;
            }
            set
            {
                mImportUsersOnlyFromSelectedRoles = value;
            }
        }


        /// <summary>
        /// Determines whether to update users' memberships in roles.
        /// </summary>
        public static bool UpdateMemberships
        {
            get
            {
                return mUpdateMemberships;
            }
            set
            {
                mUpdateMemberships = value;
            }
        }


        /// <summary>
        /// TRUE = Deletes users and roles (having IsDomain flag) that no longer exists in Active Directory.
        /// </summary>
        public static bool DeleteNotExistingObjects
        {
            get
            {
                return mDeleteNotExistingObjects;
            }
            set
            {
                mDeleteNotExistingObjects = value;
            }
        }


        /// <summary>
        /// Determines whether to log import process.
        /// </summary>
        public static bool LogImportProcess
        {
            get
            {
                return mLogImportProcess;
            }
            set
            {
                mLogImportProcess = value;
            }
        }


        /// <summary>
        /// Path for storing log.
        /// </summary>
        public static string LogPath
        {
            get
            {
                return mLogPath;
            }
            set
            {
                mLogPath = value;
            }
        }


        /// <summary>
        /// Key = Site codename, Value = collection of role identifiers.
        /// </summary>
        public static Dictionary<string, List<Guid>> Sites
        {
            get
            {
                return mSites ?? (mSites = new Dictionary<string, List<Guid>>());
            }
            set
            {
                mSites = value;
            }
        }

        #endregion


        #region "Step 5"

        /// <summary>
        /// Enumeration determining which format of name should be used for user name during import.
        /// </summary>
        public static CodenameFormat UsernameFormat
        {
            get
            {
                return mUsernameFormat;
            }
            set
            {
                mUsernameFormat = value;
            }
        }


        /// <summary>
        /// Values in format CMSProperty, ADProperty.
        /// </summary>
        public static Dictionary<string, string> UserProperties
        {
            get
            {
                return mUserProperties ?? (mUserProperties = new Dictionary<string, string>());
            }
            set
            {
                mUserProperties = value;
            }
        }


        /// <summary>
        /// Enumeration determining which format of name should be used for display name during import.
        /// </summary>
        public static CodenameFormat RoleDisplayNameFormat
        {
            get
            {
                return mRoleDisplayNameFormat;
            }
            set
            {
                mRoleDisplayNameFormat = value;
            }
        }


        /// <summary>
        /// Enumeration determining which format of name should be used for code name during import.
        /// </summary>
        public static CodenameFormat RoleCodeNameFormat
        {
            get
            {
                return mRoleCodeNameFormat;
            }
            set
            {
                mRoleCodeNameFormat = value;
            }
        }


        /// <summary>
        /// Determines whether to import description of role.
        /// </summary>
        public static bool ImportRoleDescription
        {
            get
            {
                return mImportRoleDescription;
            }
            set
            {
                mImportRoleDescription = value;
            }
        }


        /// <summary>
        /// Configures users as CMS editors.
        /// </summary>
        public static bool ConfigureAsCMSEditor
        {
            get
            {
                return mConfigureAsCMSEditor;
            }
            set
            {
                mConfigureAsCMSEditor = value;
            }
        }


        /// <summary>
        /// Mode of user property binding editor.
        /// </summary>
        public static BindingEditorMode BindingEditorMode
        {
            get
            {
                return mBindingEditorMode;
            }
            set
            {
                mBindingEditorMode = value;
            }
        }

        #endregion


        #region "Step 6, 7 and 8"

        /// <summary>
        /// Collection containing user identifiers to import.
        /// </summary>
        public static List<object> Users
        {
            get
            {
                return mUsers ?? (mUsers = new List<object>());
            }
            set
            {
                mUsers = value;
            }
        }


        /// <summary>
        /// Collection containing group identifiers to import.
        /// </summary>
        public static List<object> Groups
        {
            get
            {
                return mGroups ?? (mGroups = new List<object>());
            }
            set
            {
                mGroups = value;
            }
        }

        #endregion


        #region "Step 10"

        /// <summary>
        /// Determines whether to import now.
        /// </summary>
        public static bool ImportNow
        {
            get
            {
                return mImportNow;
            }
            set
            {
                mImportNow = value;
            }
        }


        /// <summary>
        /// Determines whether to save import profile.
        /// </summary>
        public static bool SaveImportProfile
        {
            get
            {
                return mSaveImportProfile;
            }
            set
            {
                mSaveImportProfile = value;
            }
        }

        #endregion


        #endregion


        #region "Methods"

        /// <summary>
        /// XML validation.
        /// </summary>
        private static void xmlReaderSettings_ValidationEventHandler(object sender, ValidationEventArgs e)
        {
            validationError += e.Message;
        }


        /// <summary>
        /// Returns full manifest resource name (including assembly name).
        /// </summary>
        /// <param name="type">The type whose namespace is used to scope the manifest resource name.</param>
        /// <param name="resourceName">The case-sensitive name of the manifest resource being requested</param>
        private static string GetManifestResourceName(Type type, string resourceName)
        {
            return type.Assembly.GetName().Name + "." + resourceName;
        }


        /// <summary>
        /// Sets up properties by given XML file.
        /// </summary>
        /// <param name="fileName">Path to import profile XML</param>
        public static string InitializeImportProfile(string fileName)
        {
            // Reset error
            validationError = null;
            try
            {
                // Set profile path
                ImportProfileFilename = fileName;
                if (File.Exists(ImportProfileFilename))
                {
                    using (var schemaStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(
                        GetManifestResourceName(typeof(ImportProfile), PROFILE_XSD)))
                    {
                        // Set profile path
                        ImportProfileFilename = fileName;

                        // Load XML with import profile
                        XmlDocument profile = new XmlDocument();

                        // Load XML schema
                        XmlSchema xmlSchema = XmlSchema.Read(schemaStream, xmlReaderSettings_ValidationEventHandler);

                        // Initialize reader settings
                        XmlReaderSettings xmlReaderSettings = new XmlReaderSettings();
                        xmlReaderSettings.ValidationType = ValidationType.Schema;
                        xmlReaderSettings.ValidationEventHandler += xmlReaderSettings_ValidationEventHandler;
                        xmlReaderSettings.Schemas.Add(xmlSchema);

                        // Load and validate profile
                        using (XmlReader reader = XmlReader.Create(fileName, xmlReaderSettings))
                        {
                            profile.Load(reader);
                        }

                        if (String.IsNullOrEmpty(validationError))
                        {
                            // Get settings node
                            XmlNodeList settings = profile.GetElementsByTagName("setting");
                            foreach (XmlNode setting in settings)
                            {
                                if (setting.Attributes != null)
                                {
                                    object value = setting.Attributes["value"].Value;
                                    switch (setting.Attributes["key"].Value)
                                    {
                                        case "SQLServerAddress":
                                            SQLServerAddress = ValidationHelper.GetString(value.ToString(), string.Empty);
                                            break;

                                        case "SQLServerDatabase":
                                            SQLServerDatabase = ValidationHelper.GetString(value.ToString(), string.Empty);
                                            break;

                                        case "SQLServerUsername":
                                            SQLServerUsername = ValidationHelper.GetString(value.ToString(), string.Empty);
                                            break;

                                        case "SQLServerPassword":
                                            string sqlPassword = ValidationHelper.GetString(value.ToString(), string.Empty);
                                            try
                                            {
                                                // Try to decrypt password
                                                SQLServerPassword = DecryptPassword(sqlPassword);
                                            }
                                            catch
                                            {
                                                // Password is probably in plaintext form
                                                SQLServerPassword = sqlPassword;
                                            }

                                            break;

                                        case "SQLUseTrustedConnection":
                                            SQLUseTrustedConnection = ValidationHelper.GetBoolean(value.ToString(), true);
                                            break;

                                        case "ADControllerAddress":
                                            ADControllerAddress = ValidationHelper.GetString(value.ToString(), string.Empty);
                                            break;

                                        case "ADUseCurrentUserAccount":
                                            ADUseCurrentUserAccount = ValidationHelper.GetBoolean(value.ToString(), true);
                                            break;

                                        case "ADUsername":
                                            ADUsername = ValidationHelper.GetString(value.ToString(), string.Empty);
                                            break;

                                        case "ADPassword":
                                            string adPassword = ValidationHelper.GetString(value.ToString(), string.Empty);
                                            try
                                            {
                                                // Try to decrypt password
                                                ADPassword = DecryptPassword(adPassword);
                                            }
                                            catch
                                            {
                                                // Password is probably in plaintext form
                                                ADPassword = adPassword;
                                            }
                                            break;

                                        case "ImportUsersType":
                                            ImportUsersType = (ImportType)Enum.Parse(typeof(ImportType), value.ToString());

                                            break;

                                        case "ImportRolesType":
                                            ImportRolesType = (ImportType)Enum.Parse(typeof(ImportType), value.ToString());

                                            break;

                                        case "UpdateObjectData":
                                            UpdateObjectData = ValidationHelper.GetBoolean(value.ToString(), true);
                                            break;

                                        case "ImportOnlyUsersFromSelectedRoles":
                                            ImportNewUsersOnlyFromSelectedRoles = ValidationHelper.GetBoolean(value.ToString(), false);
                                            break;

                                        case "ImportAllUsersFromSelectedRolesAndIgnoreOthers":
                                            ImportUsersOnlyFromSelectedRoles = ValidationHelper.GetBoolean(value.ToString(), false);
                                            break;

                                        case "UpdateMemberships":
                                            UpdateMemberships = ValidationHelper.GetBoolean(value.ToString(), true);
                                            break;

                                        case "DeleteNotExistingObjects":
                                            DeleteNotExistingObjects = ValidationHelper.GetBoolean(value.ToString(), true);
                                            break;

                                        case "LogImportProcess":
                                            LogImportProcess = ValidationHelper.GetBoolean(value.ToString(), true);
                                            break;

                                        case "LogPath":
                                            LogPath = ValidationHelper.GetString(value.ToString(), string.Empty);
                                            break;

                                        case "UsernameFormat":
                                            UsernameFormat = (CodenameFormat)Enum.Parse(typeof(CodenameFormat), value.ToString());
                                            break;

                                        case "ConfigureAsCMSEditor":
                                            ConfigureAsCMSEditor = ValidationHelper.GetBoolean(value.ToString(), true);
                                            break;

                                        case "BindingEditorMode":
                                            BindingEditorMode = (BindingEditorMode)Enum.Parse(typeof(BindingEditorMode), value.ToString());
                                            break;

                                        case "RoleDisplayNameFormat":
                                            RoleDisplayNameFormat = (CodenameFormat)Enum.Parse(typeof(CodenameFormat), value.ToString());
                                            break;

                                        case "RoleCodeNameFormat":
                                            RoleCodeNameFormat = (CodenameFormat)Enum.Parse(typeof(CodenameFormat), value.ToString());
                                            break;

                                        case "ImportRoleDescription":
                                            ImportRoleDescription = ValidationHelper.GetBoolean(value.ToString(), true);
                                            break;

                                        case "ImportNow":
                                            ImportNow = ValidationHelper.GetBoolean(value.ToString(), true);
                                            break;
                                    }
                                }
                            }

                            // Get user properties bindings
                            XmlNodeList properties = profile.GetElementsByTagName("property");
                            UserProperties.Clear();
                            foreach (XmlNode property in properties)
                            {
                                if ((property.Attributes != null) && ((property.Attributes["cmsname"] != null) && (property.Attributes["adname"] != null)))
                                {
                                    string cmsPropertyName = property.Attributes["cmsname"].Value;
                                    string adPropertyName = property.Attributes["adname"].Value;
                                    UserProperties.Add(cmsPropertyName, adPropertyName);
                                }
                            }

                            // Get users to import
                            XmlNodeList users = profile.GetElementsByTagName("user");
                            Users.Clear();
                            foreach (XmlNode user in users)
                            {
                                if ((user.Attributes) != null && (user.Attributes["guid"] != null))
                                {
                                    Guid userGuid = ValidationHelper.GetGuid(user.Attributes["guid"].Value, Guid.Empty);
                                    Users.Add(userGuid);
                                }
                            }

                            // Get groups to import
                            XmlNodeList groups = profile.GetElementsByTagName("group");
                            Groups.Clear();
                            foreach (XmlNode group in groups)
                            {
                                if ((group.Attributes) != null && (group.Attributes["guid"] != null))
                                {
                                    Guid groupGuid = ValidationHelper.GetGuid(group.Attributes["guid"].Value, Guid.Empty);
                                    Groups.Add(groupGuid);
                                }
                            }

                            // Get sites & roles
                            XmlNodeList sites = profile.GetElementsByTagName("site");
                            Sites.Clear();
                            foreach (XmlNode site in sites)
                            {
                                if ((site.Attributes != null) && (site.Attributes["codename"] != null))
                                {
                                    string siteCodeName = site.Attributes["codename"].Value;
                                    List<Guid> roles = new List<Guid>();
                                    foreach (XmlNode role in site.ChildNodes)
                                    {
                                        if (role.Attributes != null)
                                        {
                                            if (role.Attributes["guid"] != null)
                                            {
                                                XmlAttribute roleGuidAttr = role.Attributes["guid"];

                                                // Get guid of CMS role
                                                Guid roleGuid = Guid.Empty;
                                                if (roleGuidAttr != null)
                                                {
                                                    roleGuid = ValidationHelper.GetGuid(roleGuidAttr.Value, Guid.Empty);
                                                }
                                                roles.Add(roleGuid);
                                            }
                                        }
                                    }
                                    Sites.Add(siteCodeName.ToLowerCSafe(), roles);
                                }
                            }
                        }
                    }
                }
                else
                {
                    return ResHelper.GetString("Error_ProfileDoesNotExist", ImportProfileFilename);
                }
            }
            catch (System.IO.IOException ex)
            {
                return ex.Message;
            }
            catch (UnauthorizedAccessException ex)
            {
                return ex.Message;
            }
            catch (Exception ex)
            {
                // Log detailed information when unexpected exception occurs
                return LogHelper.GetExceptionLogMessage(ex);
            }

            return validationError;
        }


        /// <summary>
        /// Generates profile XML from properties.
        /// </summary>
        /// <returns>Import profile XML</returns>
        public static string GetImportProfile()
        {
            StringBuilder sb = new StringBuilder();

            // Formatting strings
            const string settingStr = "<setting key=\"{0}\" value=\"{1}\" />";
            const string propertyStr = "<property cmsname=\"{0}\" adname=\"{1}\" />";
            const string siteStr = "<site codename=\"{0}\">";
            const string roleStr = "<role guid=\"{0}\" />";
            const string userStr = "<user guid=\"{0}\" />";
            const string groupStr = "<group guid=\"{0}\" />";

            // Add properties
            sb.Append("<?xml version=\"1.0\" encoding=\"utf-8\" ?><profile><settings>");
            sb.Append(string.Format(settingStr, "SQLServerAddress", HTMLHelper.HTMLEncode(SQLServerAddress)));
            sb.Append(string.Format(settingStr, "SQLServerDatabase", HTMLHelper.HTMLEncode(SQLServerDatabase)));
            sb.Append(string.Format(settingStr, "SQLServerUsername", HTMLHelper.HTMLEncode(SQLServerUsername)));
            sb.Append(string.Format(settingStr, "SQLServerPassword", EncryptPassword(SQLServerPassword)));
            sb.Append(string.Format(settingStr, "SQLUseTrustedConnection", SQLUseTrustedConnection));
            sb.Append(string.Format(settingStr, "ADControllerAddress", ADControllerAddress));
            sb.Append(string.Format(settingStr, "ADUseCurrentUserAccount", ADUseCurrentUserAccount));
            sb.Append(string.Format(settingStr, "ADUsername", HTMLHelper.HTMLEncode(ADUsername)));
            sb.Append(string.Format(settingStr, "ADPassword", EncryptPassword(ADPassword)));
            sb.Append(string.Format(settingStr, "ImportUsersType", ImportUsersType));
            sb.Append(string.Format(settingStr, "ImportRolesType", ImportRolesType));
            sb.Append(string.Format(settingStr, "UpdateObjectData", UpdateObjectData));
            sb.Append(string.Format(settingStr, "ImportOnlyUsersFromSelectedRoles", ImportNewUsersOnlyFromSelectedRoles));
            sb.Append(string.Format(settingStr, "ImportAllUsersFromSelectedRolesAndIgnoreOthers", ImportUsersOnlyFromSelectedRoles));
            sb.Append(string.Format(settingStr, "UpdateMemberships", UpdateMemberships));
            sb.Append(string.Format(settingStr, "DeleteNotExistingObjects", DeleteNotExistingObjects));
            sb.Append(string.Format(settingStr, "LogImportProcess", LogImportProcess));
            sb.Append(string.Format(settingStr, "LogPath", LogPath));
            sb.Append(string.Format(settingStr, "UsernameFormat", UsernameFormat));
            sb.Append(string.Format(settingStr, "ConfigureAsCMSEditor", ConfigureAsCMSEditor));
            sb.Append(string.Format(settingStr, "BindingEditorMode", BindingEditorMode));
            sb.Append(string.Format(settingStr, "RoleDisplayNameFormat", RoleDisplayNameFormat));
            sb.Append(string.Format(settingStr, "RoleCodeNameFormat", RoleCodeNameFormat));
            sb.Append(string.Format(settingStr, "ImportRoleDescription", ImportRoleDescription));
            sb.Append(string.Format(settingStr, "ImportNow", ImportNow));
            // End settings section
            sb.Append("</settings><properties>");

            // Add users' property bindings
            foreach (KeyValuePair<string, string> property in UserProperties)
            {
                sb.Append(string.Format(propertyStr, property.Key, property.Value));
            }

            // End properties section
            sb.Append("</properties><sites>");

            // Add sites & roles
            foreach (KeyValuePair<string, List<Guid>> site in Sites)
            {
                sb.Append(string.Format(siteStr, site.Key));
                foreach (Guid roleGuid in site.Value)
                {
                    sb.Append(string.Format(roleStr, roleGuid));
                }
                sb.Append("</site>");
            }

            // End sites section
            sb.Append("</sites><objects><users>");

            // Add users
            foreach (Guid userGuid in Users)
            {
                sb.Append(string.Format(userStr, userGuid));
            }

            // End users section
            sb.Append("</users><groups>");

            // Add groups
            foreach (Guid groupGuid in Groups)
            {
                sb.Append(string.Format(groupStr, groupGuid));
            }

            // End groups section
            sb.Append("</groups></objects></profile>");

            // Return generated XML
            return sb.ToString();
        }

        #endregion


        #region "Handler raising"

        private static void RaiseDirectoryControllerChanged()
        {
            if (OnDirectoryControllerChanged != null)
            {
                OnDirectoryControllerChanged();
            }
        }


        private static void RaiseSQLServerChanged()
        {
            if (OnSQLServerChanged != null)
            {
                OnSQLServerChanged();
            }
        }

        #endregion


        #region "Encryption"

        /// <summary>
        /// Encrypts given string using AES encryption.
        /// </summary>
        /// <param name="plainText">Text to encrypt</param>
        /// <returns>Encrypted base64 string</returns>
        private static string EncryptPassword(string plainText)
        {
            byte[] initialVectorBytes = Encoding.ASCII.GetBytes(initialVector);
            byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            byte[] keyBytes = Encoding.UTF8.GetBytes(password);
            using (RijndaelManaged symmetricKey = new RijndaelManaged())
            {
                symmetricKey.Mode = CipherMode.CBC;
                symmetricKey.Padding = PaddingMode.PKCS7;
                using (ICryptoTransform encryptor = symmetricKey.CreateEncryptor(keyBytes, initialVectorBytes))
                using (System.IO.MemoryStream memStream = new System.IO.MemoryStream())
                using (CryptoStream cryptoStream = new CryptoStream(memStream, encryptor, CryptoStreamMode.Write))
                {
                    cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
                    cryptoStream.FlushFinalBlock();
                    byte[] cipherTextBytes = memStream.ToArray();

                    return Convert.ToBase64String(cipherTextBytes);
                }
            }
        }


        /// <summary>
        /// Decrypts given base64 string using AES decryption.
        /// </summary>
        /// <param name="cipherText">Encrypted base64 string</param>
        /// <returns>Plaintext string</returns>
        private static string DecryptPassword(string cipherText)
        {
            byte[] initialVectorBytes = Encoding.ASCII.GetBytes(initialVector);
            byte[] cipherTextBytes = Convert.FromBase64String(cipherText);
            byte[] keyBytes = Encoding.UTF8.GetBytes(password);
            using (RijndaelManaged symmetricKey = new RijndaelManaged())
            {
                symmetricKey.Mode = CipherMode.CBC;
                symmetricKey.Padding = PaddingMode.PKCS7;
                using (ICryptoTransform decryptor = symmetricKey.CreateDecryptor(keyBytes, initialVectorBytes))
                using (System.IO.MemoryStream memStream = new System.IO.MemoryStream(cipherTextBytes))
                using (CryptoStream cryptoStream = new CryptoStream(memStream, decryptor, CryptoStreamMode.Read))
                {
                    byte[] plainTextBytes = new byte[cipherTextBytes.Length];
                    int byteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);

                    return Encoding.UTF8.GetString(plainTextBytes, 0, byteCount);
                }
            }
        }

        #endregion
    }
}