using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Reflection;
using System.Resources;

namespace WinAppFoundation
{
    /// <summary>
    /// Class providing resource translations.
    /// </summary>
    public abstract class AbstractResHelper : IResHelper
    {
        #region "Constants"

        /// <summary>
        /// Line end.
        /// </summary>
        public static string LINE_BREAK = Environment.NewLine;

        #endregion


        #region "Private variables"

        private string mAssemblyName = null;
        private string mCultureCode = null;
        private Assembly mAssembly = null;
        private CultureInfo mCulture = null;
        private ResourceManager mResourceManager = null;
        private readonly List<ILocalizableItem> localizations = new List<ILocalizableItem>();

        #endregion


        #region "Public properties"

        /// <summary>
        /// Name of assembly containing resource file.
        /// </summary>
        public string AssemblyName
        {
            get
            {
                return mAssemblyName;
            }
            set
            {
                mAssemblyName = value;

                // Reset dependent variables
                mAssembly = null;
                mResourceManager = null;

                UpdateLocalizations();
            }
        }


        /// <summary>
        /// Gets the assembly containing translation resources.
        /// </summary>
        public Assembly Assembly
        {
            get
            {
                if (mAssembly == null)
                {
                    // Get assembly using reflection
                    mAssembly = String.IsNullOrEmpty(AssemblyName) ? Assembly.GetExecutingAssembly() : Assembly.Load(AssemblyName);
                }
                return mAssembly;
            }
            set
            {
                mAssembly = value;
            }
        }


        /// <summary>
        /// Determines whether resource keys are case sensitive.
        /// </summary>
        public bool IgnoreCase
        {
            get
            {
                return ResourceManager.IgnoreCase;
            }
            set
            {
                ResourceManager.IgnoreCase = value;
            }
        }

        #endregion


        #region "Private properties"

        /// <summary>
        /// Gets or sets name of class containing translation resources.
        /// </summary>
        private string ResourceFullClassName
        {
            get;
            set;
        }


        /// <summary>
        /// Gets selected UI culture from application settings (english by default).
        /// </summary>
        private string CultureCode
        {
            get
            {
                if (string.IsNullOrEmpty(mCultureCode))
                {
                    string uiCulture = ConfigurationManager.AppSettings["UICulture"];
                    mCultureCode = !string.IsNullOrEmpty(uiCulture) ? uiCulture : "en-us";
                }
                return mCultureCode;
            }
        }


        /// <summary>
        /// Gets object with culture.
        /// </summary>
        private CultureInfo Culture
        {
            get
            {
                return mCulture ?? (mCulture = new CultureInfo(CultureCode));
            }
        }


        /// <summary>
        /// Gets object for managing resources.
        /// </summary>
        private ResourceManager ResourceManager
        {
            get
            {
                // Load resource file
                return mResourceManager ?? (mResourceManager = new ResourceManager(ResourceFullClassName, Assembly));
            }
        }

        #endregion


        #region "Constructors"

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="assembly">Assembly to use</param>
        /// <param name="resourceFullClassName">Full name (Namespace.ClassName) of class with resources (should be findable in supplied assembly)</param>
        /// <param name="ignoreCase">Whether resource keys are case sensitive.</param>
        protected AbstractResHelper(Assembly assembly, string resourceFullClassName, bool ignoreCase = false)
        {
            if (assembly == null)
            {
                throw new ArgumentNullException("resourceFullClassName", "[AbstractResHelper] : assembly cannot be null.");
            }
            if (string.IsNullOrEmpty(resourceFullClassName))
            {
                throw new ArgumentNullException("resourceFullClassName", "[AbstractResHelper] : resourceClassName cannot be empty.");
            }
            Assembly = assembly;
            ResourceFullClassName = resourceFullClassName;
            IgnoreCase = ignoreCase;
        }


        /// <summary>
        /// Constructor utilizing type of localization file (resource file).
        /// </summary>
        /// <param name="localizationType">Type of resource file</param>
        /// <param name="ignoreCase">Whether resource keys are case sensitive.</param>
        protected AbstractResHelper(Type localizationType, bool ignoreCase = false)
            : this(localizationType.Assembly, localizationType.FullName, ignoreCase)
        {

        }

        #endregion


        #region "Public methods"

        /// <summary>
        /// Gets localized string from resource file.
        /// </summary>
        /// <param name="stringName">Codename of string</param>
        /// <param name="args">An Object array containing zero or more objects to format</param>
        /// <returns>Localized text</returns>
        public string GetString(string stringName, params object[] args)
        {
            return GetStringInternal(stringName, args);
        }


        /// <summary>
        /// Changes language of supplied translations.
        /// </summary>
        /// <param name="cultureCode">Desired culture</param>
        public void ChangeLanguage(string cultureCode)
        {
            mCultureCode = cultureCode;
            mCulture = null;
            mResourceManager = null;
            UpdateLocalizations();
        }


        /// <summary>
        /// Not implemented.
        /// </summary>
        public virtual void AddLocalization(ILocalizableItem localization)
        {
            // Collect garbage among localizations
            localizations.RemoveAll(e => !e.IsAlive);

            localizations.Add(localization);
        }

        #endregion


        #region "Internal methods"

        /// <summary>
        /// Gets localized string from resource file.
        /// </summary>
        /// <param name="stringName">Codename of string</param>
        /// <param name="args">An Object array containing zero or more objects to format</param>
        /// <returns>Localized text if localized resource exists, stringName otherwise</returns>
        protected virtual string GetStringInternal(string stringName, params object[] args)
        {
            if (stringName == null)
            {
                return string.Empty;
            }

            // uses localized string from resource manager if stringName does exist, codename itself otherwise 
            string toReturn = ResourceManager.GetString(stringName, Culture) ?? stringName;
            
            if ((args != null) && (args.Length > 0))
            {
                toReturn = string.Format(toReturn, args);
            }
            return toReturn;
        }


        /// <summary>
        /// Updates localizations.
        /// </summary>
        protected virtual void UpdateLocalizations()
        {
            foreach (ILocalizableItem item in localizations)
            {
                item.UpdateTargetValue();
            }
        }

        #endregion
    }
}
