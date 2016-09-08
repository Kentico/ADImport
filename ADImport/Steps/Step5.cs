using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

using CMS.FormEngine;
using CMS.Base;
using CMS.Membership;
using CMS.DataEngine;

namespace ADImport
{
    /// <summary>
    /// Step5 - Property bindings.
    /// </summary>
    public partial class Step5 : AbstractStep
    {
        #region "Private variables"

        private List<string> mADUserProperties = null;
        private readonly List<string> CMSUserProperties = new List<string>();
        private List<string> mExcludeFields = null;
        private bool formatsBeingBinded = false;

        private const int CMS_COLUMN_INDEX = 0;
        private const int AD_COLUMN_INDEX = 1;

        #endregion


        #region "Private properties"

        private IEnumerable<string> ADSimpleUserProperties
        {
            get
            {
                return mADUserProperties ?? (mADUserProperties = new List<string>
                                                                     {
                                                                    #region "Properties list"
                                                                         ADProvider.NoneAttribute, 
                                                                         "l", 
                                                                         "comment", 
                                                                         "company", 
                                                                         "co", 
                                                                         "c", 
                                                                         "department", 
                                                                         "division", 
                                                                         "mail", 
                                                                         "otherMailbox", 
                                                                         "employeeID", 
                                                                         "facsimileTelephoneNumber",
                                                                         "otherFacsimileTelephoneNumber", 
                                                                         "givenName",
                                                                         "homePostalAddress",
                                                                         "homePhone", 
                                                                         "otherHomePhone",
                                                                         "homeDirectory", 
                                                                         "initials", 
                                                                         "ipPhone", 
                                                                         "otherIpPhone", 
                                                                         "title", 
                                                                         "lastLogon", 
                                                                         "sn", 
                                                                         "userPrincipalName", 
                                                                         "sAMAccountName", 
                                                                         "middleName", 
                                                                         "mobile", 
                                                                         "otherMobile",
                                                                         "cn", 
                                                                         "info", 
                                                                         "objectSid", 
                                                                         "physicalDeliveryOfficeName",
                                                                         "pager", 
                                                                         "otherPager",
                                                                         "otherTelephone",
                                                                         "postOfficeBox",
                                                                         "postalAddress", 
                                                                         "st",
                                                                         "street",
                                                                         "streetAddress",
                                                                         "telephoneNumber", 
                                                                         "primaryTelexNumber", 
                                                                         "telexNumber", 
                                                                         "personalTitle",
                                                                         "postalCode"
#endregion
                                                                     });
            }
        }


        private List<string> ExcludeFields
        {
            get
            {
                return mExcludeFields ?? (mExcludeFields = new List<string>
                                                               {
                                                                #region "Properties list"
                                                                   "PreferredCultureCode", 
                                                                   "PreferredUICultureCode", 
                                                                   "UserDialogsConfiguration", 
                                                                   "UserLastLogonInfo", 
                                                                   "UserPassword", 
                                                                   "UserPasswordFormat", 
                                                                   "UserPicture", 
                                                                   "UserPreferences", 
                                                                   "UserRegistrationInfo", 
                                                                   "UserURLReferrer", 
                                                                   "UserUsedWebParts", 
                                                                   "UserUsedWidgets", 
                                                                   "UserVisibility", 
                                                                   "WindowsLiveID", 
                                                                   "UserName", 
                                                                   "UserStartingAliasPath", 
                                                                   "UserPasswordRequestHash", 
                                                                   "UserInvalidLogOnAttemptsHash"
#endregion
                                                               });
            }
        }

        #endregion


        #region "Structures"

        /// <summary>
        /// This structure holds combobox item data.
        /// </summary>
        private struct ComboBoxItemContainer
        {
            #region "Private variables"

            private string mDisplayMember;

            private string mValueMember;

            #endregion


            #region "Public properties"

            /// <summary>
            /// Display member.
            /// </summary>
            public string DisplayMember
            {
                get
                {
                    return mDisplayMember;
                }
                set
                {
                    mDisplayMember = value;
                }
            }


            /// <summary>
            /// Value member.
            /// </summary>
            public string ValueMember
            {
                get
                {
                    return mValueMember;
                }
                set
                {
                    mValueMember = value;
                }
            }

            #endregion


            #region "Constructors"

            /// <summary>
            /// Constructor.
            /// </summary>
            /// <param name="displayMember">Display member</param>
            /// <param name="valueMember">Value member</param>
            public ComboBoxItemContainer(string displayMember, string valueMember)
            {
                mDisplayMember = displayMember;
                mValueMember = valueMember;
            }

            #endregion
        }

        #endregion


        #region "Constructors"

        /// <summary>
        /// Step5 constructor.
        /// </summary>
        public Step5()
            : this(null)
        {
        }

        /// <summary>
        /// Step5 constructor.
        /// </summary>
        public Step5(ADWizard wizard)
            : base(wizard)
        {
            InitializeComponent();
        }

        #endregion


        #region "Control events"

        private void Step5_Load(object sender, EventArgs e)
        {
            // Localize step
            grdUserProperties.Columns[CMS_COLUMN_INDEX].HeaderText = ResHelper.GetString("Step5_Target");
            grdUserProperties.Columns[AD_COLUMN_INDEX].HeaderText = ResHelper.GetString("Step5_Source");
        }


        private void Step5_ParentChanged(object sender, EventArgs e)
        {
            if (Parent != null)
            {
                // Initialize radio buttons
                switch (ImportProfile.BindingEditorMode)
                {
                    case BindingEditorMode.Simple:
                        chkAllAttributes.Checked = false;
                        break;

                    case BindingEditorMode.Advanced:
                        chkAllAttributes.Checked = true;
                        break;
                }

                // Initialize checkbox tooltips
                InitToolTips();

                // Preselect value from import profile
                chkCMSEditor.Checked = ImportProfile.ConfigureAsCMSEditor;
                chkImportRoleDescription.Checked = ImportProfile.ImportRoleDescription;

                // Bind comboboxes
                BindNameFormats();

                // Bind user attributes -> properties
                BindProperties();
            }
        }


        private void chkCMSEditor_CheckedChanged(object sender, EventArgs e)
        {
            ImportProfile.ConfigureAsCMSEditor = chkCMSEditor.Checked;
        }


        private void chkImportRoleDescription_CheckedChanged(object sender, EventArgs e)
        {
            ImportProfile.ImportRoleDescription = chkImportRoleDescription.Checked;
        }


        private void cmbUsernameFromat_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!formatsBeingBinded)
            {
                ImportProfile.UsernameFormat = GetCodeNameFormatFromComboBox(cmbUsernameFromat);
            }
        }


        private void cmbRoleDisplayNameFormat_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!formatsBeingBinded)
            {
                ImportProfile.RoleDisplayNameFormat = GetCodeNameFormatFromComboBox(cmbRoleDisplayNameFormat);
            }
        }


        private void cmbRoleCodeNameFormat_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!formatsBeingBinded)
            {
                ImportProfile.RoleCodeNameFormat = GetCodeNameFormatFromComboBox(cmbRoleCodeNameFormat);
            }
        }


        private void chkAllAttributes_CheckedChanged(object sender, EventArgs e)
        {
            SetBindingEditorMode();
        }


        /// <summary>
        /// Saves property bindings.
        /// </summary>
        protected void grdUserProperties_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if ((grdUserProperties.CurrentCell != null) && (e.ColumnIndex == AD_COLUMN_INDEX) && (e.RowIndex >= 0))
            {
                DataGridViewCell keyCell = grdUserProperties[CMS_COLUMN_INDEX, grdUserProperties.CurrentCell.RowIndex];

                string key = keyCell.Value.ToString();
                string value = grdUserProperties.CurrentCell.Value.ToString();

                if (value != ADProvider.NoneAttribute)
                {
                    ImportProfile.UserProperties[key] = value;
                }
                else
                {
                    if (ImportProfile.UserProperties.ContainsKey(key))
                    {
                        ImportProfile.UserProperties.Remove(key);
                    }
                }
            }
        }


        private void grdUserProperties_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            ComboBox ctl = e.Control as ComboBox;
            if (ctl != null)
            {
                ctl.Enter -= ctl_Enter;
                ctl.Enter += ctl_Enter;
            }
        }


        private void ctl_Enter(object sender, EventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            if (comboBox != null)
            {
                comboBox.DroppedDown = true;
            }
        }

        #endregion


        #region "Methods"

        /// <summary>
        /// Retrieves code name format from the value of combo box.
        /// </summary>
        /// <param name="cmbControl">Combo box to take the value from</param>
        /// <returns>Selected code name format</returns>
        private CodenameFormat GetCodeNameFormatFromComboBox(ComboBox cmbControl)
        {
            return (CodenameFormat) Enum.Parse(typeof (CodenameFormat), cmbControl.SelectedValue.ToString());
        }


        /// <summary>
        /// Binds comboboxes.
        /// </summary>
        private void BindNameFormats()
        {
            // Set flag
            formatsBeingBinded = true;

            // Bind username format
            List<ComboBoxItemContainer> userNameFormats = new List<ComboBoxItemContainer>();
            foreach (string format in Enum.GetNames(typeof(CodenameFormat)))
            {
                if (format != CodenameFormat.Guid.ToString())
                {
                    userNameFormats.Add(new ComboBoxItemContainer(ResHelper.GetString("Username_Format_" + format), format));
                }
            }
            cmbUsernameFromat.DataSource = userNameFormats;

            // Preselect value from import profile
            if (cmbUsernameFromat.Items.Count > 0)
            {
                cmbUsernameFromat.SelectedValue = ImportProfile.UsernameFormat.ToString();
            }

            // Bind role display name format
            List<ComboBoxItemContainer> roleDisplayNameFormats = new List<ComboBoxItemContainer>();
            foreach (string format in Enum.GetNames(typeof(CodenameFormat)))
            {
                if ((format != CodenameFormat.UPN.ToString()) && (format != CodenameFormat.Guid.ToString()))
                {
                    roleDisplayNameFormats.Add(new ComboBoxItemContainer(ResHelper.GetString("Rolename_Format_" + format), format));
                }
            }
            cmbRoleDisplayNameFormat.DataSource = roleDisplayNameFormats;

            // Preselect value from import profile
            if (cmbRoleDisplayNameFormat.Items.Count > 0)
            {
                cmbRoleDisplayNameFormat.SelectedValue = ImportProfile.RoleDisplayNameFormat.ToString();
            }

            // Bind role code name format
            List<ComboBoxItemContainer> roleCodeNameFormats = new List<ComboBoxItemContainer>();
            foreach (string format in Enum.GetNames(typeof(CodenameFormat)))
            {
                if (format != CodenameFormat.UPN.ToString())
                {
                    roleCodeNameFormats.Add(new ComboBoxItemContainer(ResHelper.GetString("Rolename_Format_" + format), format));
                }
            }
            cmbRoleCodeNameFormat.DataSource = roleCodeNameFormats;

            // Preselect value from import profile
            if (cmbRoleCodeNameFormat.Items.Count > 0)
            {
                cmbRoleCodeNameFormat.SelectedValue = ImportProfile.RoleCodeNameFormat.ToString();
            }

            // Set flag
            formatsBeingBinded = false;
        }


        /// <summary>
        /// Reflects radiobutton settings to ImportProfile.
        /// </summary>
        private void SetBindingEditorMode()
        {
            ImportProfile.BindingEditorMode = chkAllAttributes.Checked ? BindingEditorMode.Advanced : BindingEditorMode.Simple;

            InitToolTips();

            BindProperties();
        }


        /// <summary>
        /// Initializes tooltips for checkbox.
        /// </summary>
        private void InitToolTips()
        {
            switch (ImportProfile.BindingEditorMode)
            {
                case BindingEditorMode.Simple:
                    toolTipSimple.RemoveAll();
                    toolTipAdvanced.SetToolTip(chkAllAttributes, ResHelper.GetString("Step5_ToolTipAdvanced"));
                    break;

                case BindingEditorMode.Advanced:
                    toolTipAdvanced.RemoveAll();
                    toolTipSimple.SetToolTip(chkAllAttributes, ResHelper.GetString("Step5_ToolTipSimple"));
                    break;
            }
        }


        /// <summary>
        /// Binds grid with properties.
        /// </summary>
        private void BindProperties()
        {
            // Clear rows
            grdUserProperties.Rows.Clear();

            // Create list of AD properties
            List<ComboBoxItemContainer> attributeList = new List<ComboBoxItemContainer>();
            switch (ImportProfile.BindingEditorMode)
            {
                case BindingEditorMode.Simple:
                    foreach (string attr in ADSimpleUserProperties)
                    {
                        string attrName = ResHelper.GetString("ADAttribute_" + attr);
                        if (attr != ADProvider.NoneAttribute)
                        {
                            attrName += " (" + attr + ")";
                        }
                        ComboBoxItemContainer container = new ComboBoxItemContainer(attrName, attr);
                        attributeList.Add(container);
                    }
                    break;

                case BindingEditorMode.Advanced:
                    // Add 'none' item
                    attributeList.Add(new ComboBoxItemContainer(ResHelper.GetString("ADAttribute_" + ADProvider.NoneAttribute), ADProvider.NoneAttribute));

                    // Add non-culture-specific attributes
                    attributeList.AddRange(ADProvider.GetUserAttributes().Select(attribute => new ComboBoxItemContainer(attribute, attribute)));
                    break;
            }


            // Sort attributes alphabetically
            attributeList.Sort((c1, c2) => CMSString.Compare(c1.DisplayMember, c2.DisplayMember, StringComparison.OrdinalIgnoreCase));

            if (CMSUserProperties.Count == 0)
            {
                // Load user data class
                DataClassInfo dci = DataClassInfoProvider.GetDataClassInfo(UserInfo.OBJECT_TYPE);

                // Load columns from the user data class
                LoadColumns(dci);

                // Load user settings data class
                dci = DataClassInfoProvider.GetDataClassInfo(UserSettingsInfo.OBJECT_TYPE);

                // Load columns from the user settings data class
                LoadColumns(dci);

                // Sort properties by name
                CMSUserProperties.Sort();
            }

            // Create default preselection
            if (ImportProfile.UserProperties.Count == 0)
            {
                ImportProfile.UserProperties.Add("FirstName", "givenName");
                ImportProfile.UserProperties.Add("MiddleName", "middleName");
                ImportProfile.UserProperties.Add("LastName", "sn");
                ImportProfile.UserProperties.Add("Email", "mail");
            }

            foreach (string property in CMSUserProperties)
            {
                // Create new row
                DataGridViewRow dr = new DataGridViewRow();

                // Create cell with CMS property
                DataGridViewTextBoxCell cmsProp = new DataGridViewTextBoxCell();
                cmsProp.Value = property;

                // Create cell with AD attributes
                DataGridViewComboBoxCell adAttr = new DataGridViewComboBoxCell();

                // Bind combobox cell
                adAttr.DisplayMember = "DisplayMember";
                adAttr.ValueMember = "ValueMember";
                adAttr.DataSource = attributeList;

                // Preselect values based on import profile
                if (ImportProfile.UserProperties.ContainsKey(property))
                {
                    string val = ImportProfile.UserProperties[property];
                    if (!chkAllAttributes.Checked && !ADSimpleUserProperties.Contains(val))
                    {
                        // Add values selected in advanced mode
                        attributeList.Add(new ComboBoxItemContainer(val, val));
                    }

                    adAttr.Value = val;
                }
                else
                {
                    // Set empty mapping
                    adAttr.Value = ADProvider.NoneAttribute;
                }

                // Add both cells to datarow
                dr.Cells.Add(cmsProp);
                dr.Cells.Add(adAttr);

                // Set CMS property read-only
                cmsProp.ReadOnly = true;

                // Add row to DataGridView
                grdUserProperties.Rows.Add(dr);
            }
        }


        /// <summary>
        /// Loads columns from given data class info.
        /// </summary>
        /// <param name="dci">DataClassInfo containing columns definitions</param>
        private void LoadColumns(DataClassInfo dci)
        {
            // Get info object based on form definition
            FormInfo fi = FormHelper.GetFormInfo(dci.ClassName, false);

            // Get column names from form info
            List<string> columnNames = fi.GetColumnNames();

            if (columnNames != null)
            {
                foreach (string name in columnNames)
                {
                    var ffi = fi.GetFormField(name);

                    // Add text fields to collection
                    if (DataTypeManager.IsString(TypeEnum.Field, ffi.DataType) && !ExcludeFields.Contains(name))
                    {
                        CMSUserProperties.Add(name);
                    }
                }
            }
        }

        #endregion
    }
}