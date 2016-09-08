using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

using CMS.Helpers;
using CMS.Base;

using WinFormsFramework;

namespace ADImport
{
    /// <summary>
    /// Active directory object (user or group).
    /// </summary>
    public class ADObject : IPrincipalObject
    {
        #region "Variables"

        private List<TreeNode> mTreeNodes = null;
        private List<Guid> mGroups = null;
        private bool mIsSelected = false;

        /// <summary>
        /// Flag determining whether check event is enabled.
        /// </summary> 
        private bool enableCheck = true;


        /// <summary>
        /// Lock used for loading object's memberships.
        /// </summary>
        private static readonly object locker = new object();

        /// <summary>
        /// Determines whether the object was disposed.
        /// </summary>
        private bool disposed = false;

        #endregion


        #region "Enumerations"

        /// <summary>
        /// Flags of userAccountControl attribute. (http://msdn.microsoft.com/en-us/library/aa772300.aspx)
        /// </summary>
        [Flags]
        public enum ADS_USER_FLAG_ENUM
        {
            /// <summary>
            /// 0x1 - The logon script is executed. This flag does not work for the ADSI LDAP provider on either read or write operations. For the ADSI WinNT provider, this flag is read-only data, and it cannot be set for user objects.
            /// </summary>
            ADS_UF_SCRIPT = 1,

            /// <summary>
            /// 0x2 - The user account is disabled.
            /// </summary>
            ADS_UF_ACCOUNTDISABLE = 2,

            /// <summary>
            /// 0x8 - The home directory is required.
            /// </summary>
            ADS_UF_HOMEDIR_REQUIRED = 8,

            /// <summary>
            /// 0x10 - The account is currently locked out.
            /// </summary>
            ADS_UF_LOCKOUT = 16,

            /// <summary>
            /// 0x20 - No password is required.
            /// </summary>
            ADS_UF_PASSWD_NOTREQD = 32,

            /// <summary>
            /// 0x40 - The user cannot change the password. This flag can be read, but not set directly.
            /// </summary>
            ADS_UF_PASSWD_CANT_CHANGE = 64,

            /// <summary>
            /// 0x80 - The user can send an encrypted password.
            /// </summary>
            ADS_UF_ENCRYPTED_TEXT_PASSWORD_ALLOWED = 128,

            /// <summary>
            /// 0x100 - This is an account for users whose primary account is in another domain. This account provides user access to this domain, but not to any domain that trusts this domain. Also known as a local user account.
            /// </summary>
            ADS_UF_TEMP_DUPLICATE_ACCOUNT = 256,

            /// <summary>
            /// 0x200 - This is a default account type that represents a typical user.
            /// </summary>
            ADS_UF_NORMAL_ACCOUNT = 512,

            /// <summary>
            /// 0x800 - This is a permit to trust account for a system domain that trusts other domains.
            /// </summary>
            ADS_UF_INTERDOMAIN_TRUST_ACCOUNT = 2048,

            /// <summary>
            /// 0x1000 - This is a computer account for a Windows 2000 Professional or Windows 2000 Server that is a member of this domain.
            /// </summary>
            ADS_UF_WORKSTATION_TRUST_ACCOUNT = 4096,

            /// <summary>
            /// 0x2000 - This is a computer account for a system backup domain controller that is a member of this domain.
            /// </summary>
            ADS_UF_SERVER_TRUST_ACCOUNT = 8192,

            /// <summary>
            /// 0x10000 - When set, the password will not expire on this account.
            /// </summary>
            ADS_UF_DONT_EXPIRE_PASSWD = 65536,

            /// <summary>
            /// 0x20000 - This is an Majority Node Set (MNS) logon account. With MNS, you can configure a multi-node Windows cluster without using a common shared disk.
            /// </summary>
            ADS_UF_MNS_LOGON_ACCOUNT = 131072,

            /// <summary>
            /// 0x40000 - When set, this flag will force the user to log on using a smart card.
            /// </summary>
            ADS_UF_SMARTCARD_REQUIRED = 262144,

            /// <summary>
            /// 0x80000 - When set, the service account (user or computer account), under which a service runs, is trusted for Kerberos delegation. Any such service can impersonate a client requesting the service. To enable a service for Kerberos delegation, set this flag on the userAccountControl property of the service account.
            /// </summary>
            ADS_UF_TRUSTED_FOR_DELEGATION = 524288,

            /// <summary>
            /// 0x100000 - When set, the security context of the user will not be delegated to a service even if the service account is set as trusted for Kerberos delegation.
            /// </summary>
            ADS_UF_NOT_DELEGATED = 1048576,

            /// <summary>
            /// 0x200000 - Restrict this principal to use only Data Encryption Standard (DES) encryption types for keys.
            /// </summary>
            ADS_UF_USE_DES_KEY_ONLY = 2097152,

            /// <summary>
            /// 0x400000 - This account does not require Kerberos preauthentication for logon.
            /// </summary>
            ADS_UF_DONT_REQUIRE_PREAUTH = 4194304,

            /// <summary>
            /// 0x800000 - The user password has expired. This flag is created by the system using data from the password last set attribute and the domain policy. It is read-only and cannot be set.
            /// </summary>
            ADS_UF_PASSWORD_EXPIRED = 8388608,

            /// <summary>
            /// 0x1000000 - The account is enabled for delegation. This is a security-sensitive setting; accounts with this option enabled should be strictly controlled. This setting enables a service running under the account to assume a client identity and authenticate as that user to other remote servers on the network.
            /// </summary>
            ADS_UF_TRUSTED_TO_AUTHENTICATE_FOR_DELEGATION = 16777216
        }

        #endregion


        #region "Properties"

        /// <summary>
        /// List of TreeNodes.
        /// </summary>
        private List<TreeNode> TreeNodes
        {
            get
            {
                return mTreeNodes ?? (mTreeNodes = new List<TreeNode>());
            }
        }


        /// <summary>
        /// Inner principal object.
        /// </summary>
        private Principal InnerPrincipal
        {
            get;
            set;
        }


        /// <summary>
        /// Collection containing identifiers of groups where object participates.
        /// </summary>
        public List<Guid> Groups
        {
            get
            {
                if (mGroups == null)
                {
                    lock (locker)
                    {
                        if (mGroups == null)
                        {
                            try
                            {
                                mGroups = PrincipalProvider.GetMemberships(InnerPrincipal).ToList();
                            }
                            catch (Exception exception)
                            {
                                // NoMatchingPrincipalException could occur in case of not well-maintained AD
                                PrincipalProvider.ExceptionLog.AddException(exception);
                            }
                        }
                    }
                }

                return mGroups ?? (mGroups = new List<Guid>());
            }
        }


        /// <summary>
        /// Referring principal provider.
        /// </summary>
        private IPrincipalProvider PrincipalProvider
        {
            get;
            set;
        }


        /// <summary>
        /// Gets name of AD object.
        /// </summary>
        /// <returns>Name</returns>
        public string Name
        {
            get
            {
                return InnerPrincipal.Name;
            }
        }


        /// <summary>
        /// Gets display name of AD object.
        /// </summary>
        /// <returns>Display name</returns>
        public string DisplayName
        {
            get
            {
                return InnerPrincipal.DisplayName;
            }
        }


        /// <summary>
        /// Gets SAM account name of AD object.
        /// </summary>
        /// <returns>SAM account name</returns>
        protected string SamAccountName
        {
            get
            {
                return InnerPrincipal.SamAccountName;
            }
        }


        /// <summary>
        /// Gets distinguished name of AD object.
        /// </summary>
        /// <returns>Distinguished name</returns>
        protected string DistinguishedName
        {
            get
            {
                return InnerPrincipal.DistinguishedName;
            }
        }


        /// <summary>
        /// Gets user principal name of AD object.
        /// </summary>
        /// <returns>User principal name</returns>
        protected string UserPrincipalName
        {
            get
            {
                return InnerPrincipal.UserPrincipalName;
            }
        }


        /// <summary>
        /// Gets unique identifier of AD object.
        /// </summary>
        /// <returns>Identifier</returns>
        public object Identifier
        {
            get
            {
                if (InnerPrincipal != null)
                {
                    return InnerPrincipal.Guid;
                }
                return null;
            }
        }


        /// <summary>
        /// Gets description of the AD object.
        /// </summary>
        public string Description
        {
            get
            {
                return InnerPrincipal.Description;
            }
        }


        /// <summary>
        /// User is enabled. Only applies to AD users.
        /// </summary>
        public bool Enabled
        {
            get
            {
                return (InnerPrincipal.GetType() == typeof(UserPrincipal)) && (((UserPrincipal)InnerPrincipal).Enabled ?? false);
            }
        }


        /// <summary>
        /// Returns domain name in format 'NetBIOSDomainName\SAMAccountName'.
        /// </summary>
        protected string DomainName
        {
            get
            {
                string principalNamePart = SamAccountName;
                string domainPart = PrincipalProvider.DomainNetBiosName;

                if (string.IsNullOrEmpty(domainPart))
                {
                    if ((InnerPrincipal.StructuralObjectClass == PrincipalProvider.GroupIdentifier) && (DistinguishedName != null))
                    {
                        // Get top domain component
                        domainPart = DistinguishedName
                            .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                            .Where(part => part.StartsWithCSafe("DC="))
                            .Select(part => part.Split('=')[1])
                            .FirstOrDefault();
                    }
                    else if ((InnerPrincipal.StructuralObjectClass == PrincipalProvider.UserIdentifier) && (UserPrincipalName != null))
                    {
                        // Try to parse domain from UPN
                        var upnParts = UserPrincipalName.Split(new[] { '@' }, StringSplitOptions.None);

                        if (upnParts.Length > 1)
                        {
                            domainPart = upnParts[2].Split(new[] { '.' }, StringSplitOptions.None).FirstOrDefault();
                        }
                    }
                }

                return domainPart + "\\" + principalNamePart;
            }
        }


        /// <summary>
        /// Gets or sets whether AD object is selected.
        /// </summary>
        public bool IsSelected
        {
            get
            {
                return mIsSelected;
            }
            set
            {
                mIsSelected = value;
                RefreshTreeNodes();
            }
        }

        #endregion


        #region "Constructors"

        /// <summary>
        /// Constructor for Active Directory object.
        /// </summary>
        /// <param name="provider">IPrincipalProvider</param>
        public ADObject(IPrincipalProvider provider)
        {
            PrincipalProvider = provider;
        }

        #endregion


        #region "Methods"

        /// <summary>
        /// Gets properties of inner object.
        /// </summary>
        public object GetProperty(string name)
        {
            PropertyValueCollection attribute = ((DirectoryEntry)InnerPrincipal.GetUnderlyingObject()).Properties[name];
            return attribute.Value;
        }


        /// <summary>
        /// Refresh checked status of all nodes that belong to this principal.
        /// Refresh import profile.
        /// </summary>
        private void RefreshTreeNodes()
        {
            if (enableCheck)
            {
                foreach (TreeNode node in TreeNodes)
                {
                    enableCheck = false;
                    if (node.TreeView != null)
                    {
                        // Check node thread safely
                        using (InvokeHelper ih = new InvokeHelper(node.TreeView))
                        {
                            ih.InvokeMethod(() => node.Checked = IsSelected);
                        }
                    }
                    else
                    {
                        node.Checked = IsSelected;
                    }
                    enableCheck = true;
                }

                // Update import profile
                Guid? identifier = ValidationHelper.GetGuid(Identifier, Guid.Empty);
                if (identifier != Guid.Empty)
                {
                    if (InnerPrincipal.StructuralObjectClass == PrincipalProvider.GroupIdentifier)
                    {
                        if (ImportProfile.Groups.Contains(identifier) && !IsSelected)
                        {
                            ImportProfile.Groups.Remove(identifier);
                        }
                        else if (!ImportProfile.Groups.Contains(identifier) && IsSelected)
                        {
                            ImportProfile.Groups.Add(identifier);
                        }
                    }
                    else if (InnerPrincipal.StructuralObjectClass == PrincipalProvider.UserIdentifier)
                    {
                        if (ImportProfile.Users.Contains(identifier) && !IsSelected)
                        {
                            ImportProfile.Users.Remove(identifier);
                        }
                        else if (!ImportProfile.Users.Contains(identifier) && IsSelected)
                        {
                            ImportProfile.Users.Add(identifier);
                        }
                    }
                }

                if (IsSelected)
                {
                    // Load collection with memberships
                    if (InnerPrincipal.StructuralObjectClass == ADProvider.USER)
                    {
                        WaitCallback touchCallback = TouchGroups;
                        ThreadPool.QueueUserWorkItem(touchCallback);
                    }
                }
            }
        }


        /// <summary>
        /// Adds TreeNode to collection of related nodes.
        /// </summary>
        /// <param name="node">Node to add</param>
        public void AddTreeNode(TreeNode node)
        {
            // Add TreeNode to collection
            TreeNodes.Add(node);
            RefreshTreeNodes();
        }


        /// <summary>
        /// Sets visibility of context menu.
        /// </summary>
        /// <param name="enabled">Determines whether to allow using context menu</param>
        public void SetContextMenuEnabledState(bool enabled)
        {
            foreach (TreeNode node in TreeNodes)
            {
                if (node.ContextMenuStrip != null)
                {
                    node.ContextMenuStrip.Enabled = enabled;
                }
            }
        }


        /// <summary>
        /// Sets principal object.
        /// </summary>
        /// <param name="principal">Object containing principal</param>
        public void SetPrincipal(object principal)
        {
            InnerPrincipal = (Principal)principal;
        }


        /// <summary>
        /// Touches Groups property to perform initialization.
        /// </summary>
        protected void TouchGroups(object data)
        {
            // Touch collection with groups to initialize
            object temporary = Groups;
        }


        /// <summary>
        /// Gets principal object.
        /// </summary>
        /// <returns>Principal object</returns>
        public object GetPrincipal()
        {
            return InnerPrincipal;
        }


        /// <summary>
        /// Finds out whether object participates in given role.
        /// </summary>
        /// <returns>TRUE if object belongs to role</returns>
        public bool IsPrincipalInGroup(Guid groupIdentifier)
        {
            return Groups.Contains(groupIdentifier);
        }


        /// <summary>
        /// Gets account name of principal object for import to CMS.
        /// </summary>
        /// <param name="safe">Determines whether to get safe form of the name</param>
        /// <returns>Code name for CMS</returns>
        public string GetCMSCodeName(bool safe)
        {
            string toReturn = string.Empty;
            if (InnerPrincipal.StructuralObjectClass == PrincipalProvider.GroupIdentifier)
            {
                switch (ImportProfile.RoleCodeNameFormat)
                {
                    case CodenameFormat.DomainAndSAM:
                        toReturn = DomainName;
                        break;

                    case CodenameFormat.SAM:
                        toReturn = SamAccountName;
                        break;

                    case CodenameFormat.Guid:
                        toReturn = InnerPrincipal.Guid.ToString();
                        break;
                }

                if (safe)
                {
                    toReturn = ValidationHelper.GetSafeRoleName(toReturn, null);
                }
            }
            else if (InnerPrincipal.StructuralObjectClass == PrincipalProvider.UserIdentifier)
            {
                switch (ImportProfile.UsernameFormat)
                {
                    case CodenameFormat.DomainAndSAM:
                        toReturn = DomainName;
                        break;

                    case CodenameFormat.SAM:
                        toReturn = SamAccountName;
                        break;

                    case CodenameFormat.UPN:
                        toReturn = InnerPrincipal.UserPrincipalName;
                        break;
                }

                if (safe)
                {
                    toReturn = ValidationHelper.GetSafeUserName(toReturn, null);
                }
            }
            return toReturn;
        }


        /// <summary>
        /// Gets display name of role in correct format.
        /// </summary>
        /// <returns>Display name in format defined by ImportProfile</returns>
        public string GetCMSDisplayName()
        {
            string roleDisplayName = String.Empty;
            if (InnerPrincipal.StructuralObjectClass == PrincipalProvider.GroupIdentifier)
            {
                switch (ImportProfile.RoleDisplayNameFormat)
                {
                    case CodenameFormat.DomainAndSAM:
                        roleDisplayName = DomainName;
                        break;

                    case CodenameFormat.SAM:
                        roleDisplayName = SamAccountName;
                        break;
                }
            }
            else if (InnerPrincipal.StructuralObjectClass == PrincipalProvider.UserIdentifier)
            {
                UserPrincipal principal = ((UserPrincipal)InnerPrincipal);
                // Concatenate first name, middle name and surname
                string fullName = principal.GivenName;
                if (!String.IsNullOrEmpty(principal.MiddleName))
                {
                    fullName += " " + principal.MiddleName;
                }
                fullName += " " + principal.Surname;
                return fullName;
            }

            return roleDisplayName;
        }

        #endregion


        #region "IDisposable implementation"

        /// <summary>
        /// Disposes the current instance of the ADObject object.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
        }


        /// <summary>
        /// Disposes the current instance of the ADObject object.
        /// </summary>
        /// <param name="disposing">Whether to dispose unmanaged resources</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    if (InnerPrincipal != null)
                    {
                        InnerPrincipal.Dispose();
                    }
                    foreach (TreeNode treeNode in TreeNodes)
                    {
                        treeNode.ContextMenuStrip.Dispose();
                    }
                }

                // Instance has been disposed
                InnerPrincipal = null;
                disposed = true;
            }
        }

        #endregion
    }
}