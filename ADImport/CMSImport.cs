using System;
using System.Collections.Generic;
using System.ComponentModel;

using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml;

using CMS.DataEngine;
using CMS.FormEngine;
using CMS.Helpers;
using CMS.Base;
using CMS.SiteProvider;
using CMS.Membership;

using WinAppFoundation;

using File = CMS.IO.File;
using FileAccess = CMS.IO.FileAccess;
using FileMode = CMS.IO.FileMode;
using FileStream = CMS.IO.FileStream;

namespace ADImport
{
    /// <summary>
    /// Class that performs import to CMS.
    /// </summary>
    public static class CMSImport
    {
        #region "Delegates & events"

        /// <summary>
        /// Delegate for saving files.
        /// </summary>
        public delegate void SaveFileHandler();

        private static event SaveFileHandler SaveFileEvent;

        private static int warnings;

        #endregion


        #region "Private variables"

        private static FormInfo mUserFormInfo;
        private static BackgroundWorker mImportWorker;

        #endregion


        #region "Public properties"

        /// <summary>
        /// Connection string for CMS database.
        /// </summary>
        public static string ConnectionString
        {
            get
            {
                return GetCMSConnectionString(true);
            }
        }


        /// <summary>
        /// Connection string for SQL instance.
        /// </summary>
        public static string BaseConnectionString
        {
            get
            {
                return GetCMSConnectionString(false);
            }
        }

        #endregion


        #region "Private properties"

        /// <summary>
        /// Asynchronous worker for import.
        /// </summary>
        private static BackgroundWorker ImportWorker
        {
            get
            {
                if (mImportWorker == null)
                {
                    mImportWorker = new BackgroundWorker();
                    mImportWorker.WorkerSupportsCancellation = true;
                    mImportWorker.DoWork += ImportWorker_DoWork;
                    mImportWorker.RunWorkerCompleted += ImportWorker_RunWorkerCompleted;
                }
                return mImportWorker;
            }
        }


        /// <summary>
        /// Provides message logging.
        /// </summary>
        private static IMessageLog MessageLog
        {
            get;
            set;
        }


        /// <summary>
        /// Provides principals.
        /// </summary>
        private static IPrincipalProvider PrincipalProvider
        {
            get;
            set;
        }


        /// <summary> 
        /// Gets user's form information based on user's and user's settings data classes.
        /// </summary>
        private static FormInfo UserFormInfo
        {
            get
            {
                if (mUserFormInfo == null)
                {
                    FormInfo ufi = new FormInfo(DataClassInfoProvider.GetDataClassInfo("cms.user").ClassFormDefinition);
                    FormInfo usfi = new FormInfo(DataClassInfoProvider.GetDataClassInfo("cms.usersettings").ClassFormDefinition);
                    ufi.CombineWithForm(usfi, false);
                    mUserFormInfo = ufi;
                }
                return mUserFormInfo;
            }
        }

        #endregion


        #region "DB connection methods"

        /// <summary>
        /// Creates connection string for CMS database.
        /// </summary>
        /// <param name="appendCatalog">Indicates whether to include schema</param>
        /// <returns>Generated connection string</returns>
        private static string GetCMSConnectionString(bool appendCatalog)
        {
            return SqlServerHelper.GetCMSConnectionString(
                ImportProfile.SQLServerAddress,
                appendCatalog ? ImportProfile.SQLServerDatabase : null,
                ImportProfile.SQLUseTrustedConnection,
                ImportProfile.SQLServerUsername,
                ImportProfile.SQLServerPassword
                );
        }


        /// <summary>
        /// Sets up connection string to CMS database in application configuration (app.config).
        /// </summary>
        public static void ConfigureApplicationSettings()
        {
            // Create connection string
            ConnectionHelper.ConnectionString = ConnectionString;
        }

        #endregion


        #region "Saving files"

        /// <summary>
        /// Handles all errors that may occur during saving file.
        /// </summary>
        /// <param name="saveFileHandler">Method for saving file</param>
        private static void SaveFile(SaveFileHandler saveFileHandler)
        {
            try
            {
                SaveFileEvent = saveFileHandler;
                SaveFileEvent();
            }
            catch (ArgumentException ex)
            {
                MessageLog.LogError(ResHelper.GetString("Error_ArgumentException"), ex);
            }
            catch (PathTooLongException ex)
            {
                MessageLog.LogError(ResHelper.GetString("Error_PathTooLongException"), ex);
            }
            catch (DirectoryNotFoundException ex)
            {
                MessageLog.LogError(ResHelper.GetString("Error_DirectoryNotFoundException"), ex);
            }
            catch (UnauthorizedAccessException ex)
            {
                MessageLog.LogError(ResHelper.GetString("Error_UnauthorizedAccessException"), ex);
            }
            catch (FileNotFoundException ex)
            {
                MessageLog.LogError(ResHelper.GetString("Error_FileNotFoundException"), ex);
            }
            catch (NotSupportedException ex)
            {
                MessageLog.LogError(ResHelper.GetString("Error_PathTooLongException"), ex);
            }
        }


        /// <summary>
        /// Saves import profile to an XML file.
        /// </summary>
        private static void SaveImportProfile()
        {
            // Create or overwrite XML with import profile
            FileStream importProfile = File.Open(ImportProfile.ImportProfileFilename, FileMode.Create, FileAccess.Write);
            // Create new instance of XML text writer
            XmlTextWriter writer = new XmlTextWriter(importProfile, Encoding.Default);
            XmlDocument document = new XmlDocument();
            // Load XML document
            document.LoadXml(ImportProfile.ImportProfileXML);
            // Set formatting
            writer.Formatting = Formatting.Indented;
            // Write generated XML
            document.WriteContentTo(writer);
            // Flush & close writer
            writer.Flush();
            writer.Close();

            // Add message to log
            MessageLog.LogEvent(ResHelper.GetString("Log_ImportProfileSaved", ImportProfile.ImportProfileFilename));
        }

        #endregion


        #region "Import methods"

        /// <summary>
        /// Imports users and groups from active directory.
        /// </summary>
        /// <param name="provider">IPrincipalProvider</param>
        /// <param name="messageLog">Provides message logging</param>
        /// <param name="workerCompleted">Action to perform on worker completed.</param>
        public static void ImportAsync(IPrincipalProvider provider, IMessageLog messageLog, RunWorkerCompletedEventHandler workerCompleted = null)
        {
            if (!ImportWorker.IsBusy)
            {
                // Set AD provider
                PrincipalProvider = provider;

                // Set message logs
                MessageLog = messageLog;

                if (ImportProfile.LogImportProcess)
                {
                    MessageLog = new FileMessageLog(MessageLog, Path.GetDirectoryName(ImportProfile.LogPath), Path.GetFileName(ImportProfile.LogPath));
                }

                // Run import
                if (workerCompleted != null)
                {
                    ImportWorker.RunWorkerCompleted += workerCompleted;
                }

                ImportWorker.RunWorkerAsync();
            }
        }


        /// <summary>
        /// Imports users and groups from active directory.
        /// </summary>
        /// <param name="provider">IPrincipalProvider</param>
        /// <param name="messageLog">Provides message logging</param>
        public static void Import(IPrincipalProvider provider, IMessageLog messageLog)
        {
            AutoResetEvent importFinished = new AutoResetEvent(false);

            ImportAsync(provider, messageLog, (sender, e) => importFinished.Set());
            importFinished.WaitOne();
        }


        /// <summary>
        /// Cancel asynchronous import.
        /// </summary>
        public static void CancelImport()
        {
            if (ImportWorker.IsBusy)
            {
                ImportWorker.CancelAsync();
            }
        }


        private static void ImportWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                BackgroundWorker worker = (BackgroundWorker)sender;

                // Save import profile
                if (ImportProfile.SaveImportProfile && !ImportProfile.UsesConsole)
                {
                    SaveFile(SaveImportProfile);
                }

                // Decide whether to import
                if (!ImportProfile.ImportNow && !ImportProfile.UsesConsole)
                {
                    return;
                }

                using (new CMSActionContext() { LogEvents = false, ContinuousIntegrationAllowObjectSerialization = false })
                {
                    #region "Initialization"

                    // Import canceled
                    if (worker.CancellationPending)
                    {
                        e.Cancel = true;
                        return;
                    }

                    DateTime start = DateTime.Now;

                    // Initialize CMS context
                    CMSInit();

                    if (ImportProfile.UsesConsole)
                    {
                        // Ensure object in case they are not present in import profile
                        EnsureObjects();
                    }

                    if (ImportProfile.ImportUsersOnlyFromSelectedRoles)
                    {
                        // Narrow down imported users according to imported roles
                        ImportProfile.Users.Clear();
                    }

                    // Import canceled
                    if (worker.CancellationPending)
                    {
                        e.Cancel = true;
                        return;
                    }

                    // Initialize cumulative changed users and roles storages
                    var rolesChanged = new CumulatedChanges(WellKnownEventLogEventsEnum.RolesCreated, WellKnownEventLogEventsEnum.RolesUpdated, WellKnownEventLogEventsEnum.RolesDeleted);
                    var usersChanged = new CumulatedChanges(WellKnownEventLogEventsEnum.UsersCreated, WellKnownEventLogEventsEnum.UsersUpdated, WellKnownEventLogEventsEnum.UsersDeleted);

                    #endregion

                    // Delete non-existing objects (this also prevents conflicting code names)
                    if (ImportProfile.DeleteNotExistingObjects)
                    {
                        DeleteNonExistingObjects(usersChanged, rolesChanged);
                    }

                    #region "Role import"

                    foreach (var siteInfo in ImportProfile
                        .Sites
                        .Select(site => SiteInfo.Provider.Get(site.Key))
                        .Where(info => info != null))
                    {
                        foreach (Guid groupGuid in ImportProfile.Groups)
                        {
                            // Import canceled
                            if (worker.CancellationPending)
                            {
                                e.Cancel = true;
                                return;
                            }

                            // Try to get group
                            IPrincipalObject group = PrincipalProvider.GetPrincipalObject(groupGuid);

                            // If group is still null
                            if (group == null)
                            {
                                MessageLog.LogEvent(ResHelper.GetString("Log_SkippingNonExistingObject"));
                                warnings++;
                                // If deleting of not existing objects is enabled
                                if (ImportProfile.DeleteNotExistingObjects)
                                {
                                    DeleteRole(siteInfo, groupGuid);
                                }
                            }
                            else
                            {
                                // Get role description
                                string roleDescription = String.Empty;
                                if (ImportProfile.ImportRoleDescription && (group.Description != null))
                                {
                                    roleDescription = group.Description;
                                }

                                // Get correct role name format
                                string roleCodeName = group.GetCMSCodeName(true);

                                // Get role display name
                                string roleDisplayName = group.GetCMSDisplayName();

                                // Get safe role name
                                roleCodeName = ValidationHelper.GetSafeRoleName(roleCodeName, siteInfo.SiteName);

                                if (!String.IsNullOrEmpty(roleCodeName))
                                {
                                    // Add message to log
                                    MessageLog.LogEvent(ResHelper.GetString("Log_ImportingRole", roleDisplayName, CMS.Helpers.ResHelper.LocalizeString(siteInfo.DisplayName)));

                                    // Import role
                                    ImportRole(roleCodeName, roleDisplayName, siteInfo.SiteID, roleDescription, groupGuid, ImportProfile.UpdateObjectData, rolesChanged);

                                    if (ImportProfile.ImportUsersOnlyFromSelectedRoles)
                                    {
                                        ImportProfile.Users.AddRange(PrincipalProvider.GetUsersOf(group).Select(u => u.Identifier));
                                    }
                                }
                                else
                                {
                                    // Add message to log
                                    MessageLog.LogEvent(ResHelper.GetString("Log_SkippingEmptyRolename", group.Identifier));
                                    warnings++;
                                }
                            }
                        }
                    }

                    // Log created and updated and removed roles to EventLog
                    rolesChanged.WriteEventsToEventLog();

                    #endregion

                    #region "User import"

                    foreach (var user in ImportProfile
                        .Users
                        .Distinct()
                        .Select(userGuid => PrincipalProvider.GetPrincipalObject(userGuid)))
                    {
                        // Import canceled
                        if (worker.CancellationPending)
                        {
                            e.Cancel = true;
                            return;
                        }

                        if (user == null)
                        {
                            MessageLog.LogEvent(ResHelper.GetString("Log_SkippingNonExistingObject"));
                            continue;
                        }

                        string domainName = user.GetCMSCodeName(true);

                        if (!String.IsNullOrEmpty(domainName))
                        {
                            // Get user info object
                            UserInfo userInfo = (UserInfo.Provider.Get((Guid)user.Identifier) ?? UserInfo.Provider.Get(domainName));
                            bool newUser = (userInfo == null);

                            // When is desired to import new users only from selected roles
                            if (newUser && ImportProfile.ImportNewUsersOnlyFromSelectedRoles)
                            {
                                // Skip users that does not belong to one of selected role
                                bool skip = ImportProfile.Groups.Cast<Guid>().All(groupGuid => !user.IsPrincipalInGroup(groupGuid));
                                if (skip)
                                {
                                    MessageLog.LogEvent(ResHelper.GetString("Log_SkippingDoesNotBelongToSelectedRole", domainName));
                                    continue;
                                }
                            }

                            if (ImportProfile.UpdateObjectData || newUser)
                            {
                                if (userInfo == null)
                                {
                                    userInfo = new UserInfo();
                                    // Add message to log
                                    MessageLog.LogEvent(ResHelper.GetString("Log_ImportingUser", domainName));
                                }
                                else
                                {
                                    // Add message to log
                                    MessageLog.LogEvent(ResHelper.GetString("Log_UpdatingUser", domainName));
                                }

                                using (var transaction = new CMSTransactionScope())
                                {
                                    if (newUser)
                                    {
                                        userInfo.UserIsDomain = true;
                                        userInfo.UserGUID = (Guid)user.Identifier;

                                        // Set privilege level
                                        UserPrivilegeLevelEnum privilegeLevel = ImportProfile.ConfigureAsCMSEditor ? UserPrivilegeLevelEnum.Editor : UserPrivilegeLevelEnum.None;
                                        userInfo.SiteIndependentPrivilegeLevel = privilegeLevel;
                                    }

                                    if (userInfo.UserIsDomain)
                                    {
                                        // Set user's properties
                                        userInfo.UserIsExternal = true;
                                        userInfo.UserName = domainName;
                                        userInfo.Enabled = ValidationHelper.GetBoolean(user.Enabled, true);

                                        // Bind properties
                                        foreach (KeyValuePair<string, string> property in ImportProfile.UserProperties)
                                        {
                                            // Get attribute
                                            object attribute = user.GetProperty(property.Value);

                                            if (attribute != null)
                                            {
                                                try
                                                {
                                                    string attrValue;

                                                    // Get string representation of the attribute
                                                    if (attribute is float || attribute is double || attribute is decimal)
                                                    {
                                                        attrValue = String.Format(CultureInfo.InvariantCulture, "{0}", attribute);
                                                    }
                                                    else if (attribute.GetType() == typeof(byte[]))
                                                    {
                                                        attrValue = PrincipalProvider.GetSID(attribute);
                                                    }
                                                    else if (attribute.GetType().BaseType == typeof(MarshalByRefObject))
                                                    {
                                                        attrValue = PrincipalProvider.GetTimeFromInterval(attribute);
                                                    }
                                                    else
                                                    {
                                                        attrValue = attribute.ToString();
                                                    }

                                                    // Set property
                                                    userInfo.SetValue(property.Key, LimitLengthForField(attrValue, property.Key));
                                                }
                                                catch
                                                {
                                                    MessageLog.LogEvent(ResHelper.GetString("Log_ErrorParsingAttr", property.Value));
                                                    warnings++;
                                                }
                                            }
                                            else
                                            {
                                                FormFieldInfo field = UserFormInfo.GetFormField(property.Key);
                                                userInfo.SetValue(property.Key, field.GetPropertyValue(FormFieldPropertyEnum.DefaultValue));
                                            }
                                        }

                                        // Create full name if empty
                                        if (String.IsNullOrEmpty(userInfo.FullName))
                                        {
                                            userInfo.FullName = user.GetCMSDisplayName();
                                        }

                                        // Store user info object and its user-settings
                                        if (userInfo.ChangedColumns().Any())
                                        {
                                            // Store created/updated user for EventLog
                                            // User name is used, because AD accounts does not have to have first and/or given name specified (e.g. Guest, …)
                                            usersChanged.Add(userInfo.UserGUID, userInfo.UserName, newUser ? ChangeActionEnum.Created : ChangeActionEnum.Updated);

                                            UserInfo.Provider.Set(userInfo);
                                        }
                                    }
                                    else
                                    {
                                        MessageLog.LogEvent(ResHelper.GetString("Log_UserIsNotDomain", userInfo.UserName));
                                        warnings++;
                                    }

                                    transaction.Commit();
                                }
                            }
                            else
                            {
                                MessageLog.LogEvent(ResHelper.GetString("Log_SkippingExistingUser", domainName));
                            }

                            // Import canceled
                            if (worker.CancellationPending)
                            {
                                e.Cancel = true;
                                return;
                            }

                            // Assign user to sites and roles (for domain users only)
                            if (!userInfo.UserIsDomain)
                            {
                                continue;
                            }


                            #region "Membership (roles) synchronization"

                            if (!newUser && !ImportProfile.UpdateMemberships && !ImportProfile.UpdateMemberships)
                            {
                                // No membership synchronization will be performed
                                continue;
                            }

                            // Initialize collection to cumulate membership changes
                            var memberShipChanges = new CumulatedRolesMembership();

                            // Load all user roles from DB
                            var userRoles = new HashSet<RoleInfo>(newUser
                                ? Enumerable.Empty<RoleInfo>() // non-existing user cannot be present in a single role (in DB)
                                : RoleInfo.Provider
                                    .Get()
                                    .WhereIn("RoleID",
                                        UserRoleInfo.Provider
                                            .Get()
                                            .WhereEquals("UserID", userInfo.UserID)
                                            .Column("RoleID"))
                                    .Columns("RoleID", "RoleGUID", "RoleDisplayName", "RoleIsDomain"));

                            // Store user's roles before membership synchronization
                            memberShipChanges.SetRolesBefore(userRoles);
                            foreach (KeyValuePair<string, List<Guid>> site in ImportProfile.Sites)
                            {
                                // Get site info object
                                var siteInfo = SiteInfo.Provider.Get(site.Key);
                                if (siteInfo != null)
                                {
                                    try
                                    {
                                        // Add user to this site
                                        UserSiteInfo.Provider.Add(userInfo.UserID, siteInfo.SiteID);
                                    }
                                    catch (Exception ex)
                                    {
                                        MessageLog.LogEvent(ResHelper.GetString("Log_GeneralWarning", ex.Message));
                                        warnings++;
                                    }

                                    // Assign user to roles already existing in CMS
                                    if (newUser || ImportProfile.UpdateMemberships)
                                    {
                                        SetMemberships(user, userInfo, siteInfo, userRoles, site);
                                    }

                                    // Remove user from roles they is member no more
                                    if (!newUser && ImportProfile.UpdateMemberships)
                                    {
                                        RemoveExcessiveMemberships(user, userInfo, userRoles);
                                    }
                                }
                                else
                                {
                                    MessageLog.LogEvent(ResHelper.GetString("Log_SiteNotExist", site.Key));
                                    warnings++;
                                }
                            }

                            // Store user's roles after membership synchronization
                            memberShipChanges.SetRolesAfter(userRoles);

                            // Log created and removed memberships to EventLog
                            memberShipChanges.WriteEventsToEventLog(userInfo.UserName);

                            #endregion
                        }
                        else
                        {
                            // Add message to log
                            MessageLog.LogEvent(ResHelper.GetString("Log_SkippingEmptyUsername", user.Identifier));
                            warnings++;
                        }
                    }

                    // Log created and updated and deleted users to EventLog
                    usersChanged.WriteEventsToEventLog();

                    #endregion

                    // Import canceled
                    if (worker.CancellationPending)
                    {
                        e.Cancel = true;
                        return;
                    }

                    TimeSpan duration = DateTime.Now - start;

                    if (!worker.CancellationPending)
                    {
                        // Add message to log
                        MessageLog.LogEvent(warnings == 0
                            ? ResHelper.GetString("Log_ImportComplete", duration.Hours, duration.Minutes, duration.Seconds)
                            : ResHelper.GetString("Log_ImportCompleteWithWarnings", warnings, duration.Hours, duration.Minutes, duration.Seconds));
                    }
                }
            }
            catch (Exception ex)
            {
                MessageLog.LogError(ResHelper.GetString("Error_General"), ex);
            }
        }


        private static void ImportWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                MessageLog.LogEvent(ResHelper.GetString("Log_ImportCanceled"));
            }
        }


        /// <summary>
        /// Trims given value to maximum field length.
        /// </summary>
        /// <param name="attributeValue">Value to sanitize</param>
        /// <param name="fieldName">Name of field to use for validation</param>
        /// <returns>String valid for use in corresponding field</returns>
        private static string LimitLengthForField(string attributeValue, string fieldName)
        {
            string value = attributeValue;
            FormFieldInfo field = UserFormInfo.GetFormField(fieldName);
            if (field != null)
            {
                int fieldSize = field.Size;
                if ((value.Length > fieldSize) && (fieldSize > 0))
                {
                    value = TextHelper.LimitLength(value, fieldSize, String.Empty);
                    MessageLog.LogEvent(ResHelper.GetString("Log_ValueTrimmed", fieldName, fieldSize));
                    warnings++;
                }
            }
            return value;
        }


        /// <summary>
        /// Deletes role specified by site and group identifier.
        /// </summary>
        /// <param name="siteInfo">Site info object</param>
        /// <param name="groupGuid">AD group identifier</param>
        private static void DeleteRole(SiteInfo siteInfo, Guid groupGuid)
        {

            // Try to get role by GUID and site id
            RoleInfo role = RoleInfo.Provider.Get().OnSite(siteInfo.SiteID).WhereEquals("RoleGuid", groupGuid).FirstOrDefault();

            // If role is domain role
            if ((role != null) && role.RoleIsDomain)
            {
                MessageLog.LogEvent(ResHelper.GetString("Log_DeletingRole", role.RoleDisplayName));

                // Delete role
                RoleInfo.Provider.Delete(role);
            }
        }


        /// <summary>
        /// Determines whether to preselect new group (that does not exist in CMS).
        /// </summary>
        /// <param name="groupGuid">Group to preselect</param>
        /// <returns>TRUE if group should be preselected (group does not exist in at least one site)</returns>
        public static bool RoleExists(Guid groupGuid)
        {

            // Preselect roles
            foreach (string siteName in ImportProfile.Sites.Keys)
            {
                // If role is missing in any site, select it
                SiteInfo siteInfo = SiteInfo.Provider.Get(siteName);
                if (siteInfo != null)
                {
                    RoleInfo roleInfo = RoleInfo.Provider.Get().OnSite(siteInfo.SiteID).WhereEquals("RoleGuid", groupGuid).FirstOrDefault();
                    if (roleInfo != null)
                    {
                        return true;
                    }
                }
            }
            return true;
        }


        /// <summary>
        /// Determines whether user exists in CMS.
        /// </summary>
        /// <param name="userGuid">GUID of user</param>
        /// <returns>TRUE if user exists</returns>
        public static bool UserExists(Guid userGuid)
        {
            UserInfo userInfo = UserInfo.Provider.Get(userGuid);
            return (userInfo != null);
        }


        /// <summary>
        /// Ensures AD objects (for console mode purposes).
        /// </summary>
        private static void EnsureObjects()
        {
            MessageLog.LogEvent(ResHelper.GetString("Console_LoadingObjectFromAD"));

            if ((ImportProfile.ImportUsersType == ImportType.All) || (ImportProfile.ImportUsersType == ImportType.UpdateSelectedImportNew))
            {
                PrincipalProvider.LoadAllUsers();
            }

            if ((ImportProfile.ImportRolesType == ImportType.All) || (ImportProfile.ImportRolesType == ImportType.UpdateSelectedImportNew))
            {
                PrincipalProvider.LoadAllGroups();
            }
        }


        /// <summary>
        /// Assigns CMS and AD roles to user.
        /// </summary>
        /// <param name="user">AD user</param>
        /// <param name="userInfo">CMS user</param>
        /// <param name="userRoles">Collection of <see cref="RoleInfo"/> objects user is in – infos are supposed to contain RoleGUID.</param>
        /// <param name="site">CMS roles</param>
        /// <param name="siteInfo">Site info object</param>
        private static void SetMemberships(IPrincipalObject user, UserInfo userInfo, SiteInfo siteInfo, ICollection<RoleInfo> userRoles, KeyValuePair<string, List<Guid>> site)
        {

            var roleGuids = Enumerable.Empty<Guid>()
                .Union(site.Value)      // CMS role GUIDs user should be in
                .Union(user.Groups);    // AD role GUIDs user should be in (groups in which the user participates in AD and are imported to CMS)

            foreach (RoleInfo roleInfo in roleGuids
                .Except(userRoles.Select(userRole => userRole.RoleGUID))
                .Select(groupId => RoleInfo.Provider.Get().OnSite(siteInfo.SiteID).WhereEquals("RoleGuid", groupId).FirstOrDefault())
                .Where(roleInfo => (roleInfo != null)))
            {
                // Add user to the role
                UserRoleInfo.Provider.Add(userInfo.UserID, roleInfo.RoleID);

                // Update collection of user roles (to reflect real roles user is in)
                userRoles.Add(roleInfo);

                MessageLog.LogEvent(ResHelper.GetString("Log_AssigningUserToRole", userInfo.UserName, roleInfo.RoleDisplayName));
            }
        }


        /// <summary>
        /// Imports role to CMS.
        /// </summary>
        /// <param name="roleName">Name of role</param>
        /// <param name="displayName">Display name of role</param>
        /// <param name="siteId">ID of site</param>
        /// <param name="roleDescription">Role description</param>
        /// <param name="roleGuid">GUID of role</param>
        /// <param name="updateExistingObject">Determines whether update object if already exists</param>
        /// <param name="rolesChanged">Records added and updated roles for CMS event log</param>
        private static void ImportRole(string roleName, string displayName, int siteId, string roleDescription, Guid roleGuid, bool updateExistingObject, CumulatedChanges rolesChanged)
        {
            // Try to get role info by GUID, by GUID in code name, by name
            var roleInfo = RoleInfo.Provider.Get().OnSite(siteId).WhereEquals("RoleGuid", roleGuid).FirstOrDefault()
                                ?? RoleInfo.Provider.Get().OnSite(siteId).WhereEquals("RoleGuid", ValidationHelper.GetGuid(roleName, Guid.Empty)).FirstOrDefault()
                                ?? RoleInfo.Provider.Get(roleName, siteId);
            var newRole = roleInfo == null;

            if (newRole)
            {
                // Create new instance of role
                roleInfo = new RoleInfo();

                // Set new role properties
                roleInfo.SiteID = siteId;

                // Mark role as domain role
                roleInfo.RoleIsDomain = true;
            }
            else
            {
                // Don't update object
                if (!updateExistingObject)
                {
                    return;
                }
            }
            if (roleInfo.RoleIsDomain)
            {
                // Set role name
                roleInfo.RoleName = roleName;

                // Set display name
                roleInfo.RoleDisplayName = displayName;

                // Set description
                roleInfo.RoleDescription = roleDescription;

                // Set GUID
                roleInfo.RoleGUID = roleGuid;

                try
                {
                    if (!roleInfo.ChangedColumns().Any())
                    {
                        return;
                    }

                    // Store created/updated role ID for EventLog
                    rolesChanged.Add(roleInfo.RoleGUID, roleInfo.RoleDisplayName, newRole ? ChangeActionEnum.Created : ChangeActionEnum.Updated);

                    // Store role into database
                    RoleInfo.Provider.Set(roleInfo);
                }
                catch (CodeNameNotUniqueException)
                {
                    MessageLog.LogEvent(ResHelper.GetString("Log_RoleNameNotUnique", roleName));
                    warnings++;
                }
            }
            else
            {
                MessageLog.LogEvent(ResHelper.GetString("Log_RoleIsNotDomain", roleInfo.RoleDisplayName));
                warnings++;
            }
        }


        /// <summary>
        /// Synchronizes memberships of given user.
        /// </summary>
        /// <param name="user">AD user</param>
        /// <param name="userInfo">CMS user</param>
        /// <param name="userRoles">Collection of <see cref="RoleInfo"/> objects user is in – infos are supposed to contain RoleGUID and RoleIsDomain.</param>
        private static void RemoveExcessiveMemberships(IPrincipalObject user, UserInfo userInfo, ISet<RoleInfo> userRoles)
        {
            // Add message to log
            MessageLog.LogEvent(ResHelper.GetString("Log_UpdatingMemberships", userInfo.UserName));

            // Get all user's roles that originate in AD and user is no longer member of
            var removedRoles = new List<RoleInfo>();
            foreach (var roleInfo in userRoles
                .Where(role => role.RoleIsDomain)
                .Where(userRole => !user.IsPrincipalInGroup(userRole.RoleGUID)))
            {
                // Remove user from CMS role
                var userRole = UserRoleInfo.Provider.Get(userInfo.UserID, roleInfo.RoleID);
                UserRoleInfo.Provider.Delete(userRole);

                // Store removed roles
                removedRoles.Add(roleInfo);
            }

            // Update set of user roles (to reflect real roles user is in)
            userRoles.ExceptWith(removedRoles);
        }


        /// <summary>
        /// Deletes domain objects that exist in CMS and doesn't exist in AD.
        /// </summary>
        /// <param name="usersChanged">Records removed users for CMS event log.</param>
        /// <param name="rolesChanged">Records removed roles for CMS event log.</param>
        private static void DeleteNonExistingObjects(CumulatedChanges usersChanged, CumulatedChanges rolesChanged)
        {
            // Remove CMS (domain) roles that do not exist in AD anymore
            IQueryable<RoleInfo> excessiveRoles = RoleInfo.Provider
                .Get()
                .WhereTrue("RoleIsDomain")
                .WhereNotEquals("RoleGUID", Guid.Empty)
                .WhereGreaterThan("RoleID", 0)
                .Columns("RoleID", "RoleGUID", "RoleName")
                .Where(x => !PrincipalProvider.Exists(x.RoleGUID));

            foreach (var role in excessiveRoles)
            {
                // Delete role
                RoleInfo.Provider.Delete(role);

                // Store deleted role GUID and name for EventLog
                rolesChanged.Add(role.RoleGUID, role.RoleDisplayName, ChangeActionEnum.Deleted);

                // Add message to log
                MessageLog.LogEvent(ResHelper.GetString("Log_DeletingRole", role.RoleDisplayName));
            }

            // Remove CMS (domain) users that do not exist in AD anymore
            IQueryable<UserInfo> excessiveUsers = UserInfo.Provider
                .Get()
                .WhereTrue("UserIsDomain")
                .WhereNotEquals("UserGUID", Guid.Empty)
                .WhereGreaterThan("UserID", 0)
                .Columns("UserID", "UserGUID", "UserName")
                .Where(x => !PrincipalProvider.Exists(x.UserGUID));

            foreach (var user in excessiveUsers)
            {
                // Delete user
                UserInfo.Provider.Delete(user);

                // Store deleted user GUID and name for EventLog
                usersChanged.Add(user.UserGUID, user.UserName, ChangeActionEnum.Deleted);

                // Add message to log
                MessageLog.LogEvent(ResHelper.GetString("Log_DeletingUser", user.UserName));
            }
        }

        #endregion


        #region "Other methods"

        /// <summary>
        /// Initializes CMS context.
        /// </summary>
        public static void CMSInit()
        {
            // Initialize path of application
            SystemContext.WebApplicationPhysicalPath = Application.StartupPath;

            CMSApplication.Init();
        }

        #endregion
    }
}