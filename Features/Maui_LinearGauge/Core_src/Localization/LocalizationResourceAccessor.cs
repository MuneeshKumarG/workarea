using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Resources;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using System.Reflection;

namespace Syncfusion.Maui.Core.Localization
{
    /// <summary>
    /// 
    /// </summary>
    public class LocalizationResourceAccessor
    {

        #region Properties

        //  public static System.Resources.ResourceManager Manager { get; set; }

        /// <summary>
        /// Gets or sets the resource manager.
        /// </summary>
        /// <value>The resource manager.</value>
        public static ResourceManager? ResourceManager
        {
            get;
            set;
        }

        private static CultureInfo culture = Thread.CurrentThread.CurrentUICulture;

        /// <summary>
        /// Gets the culture.
        /// </summary>
        /// <value>The culture.</value>
        internal static CultureInfo Culture
        {
            get
            {
                return culture;
            }
            set
            {
                culture = value;
            }
        }



        /// <summary>
        /// Gets the localized string.
        /// </summary>
        /// <returns>The string.</returns>
        /// <param name="text">Text type</param>
        public static string? GetString(string text)
        {
            string? value = string.Empty;

            try
            {
                if (ResourceManager != null)
                {
                    value = ResourceManager.GetString(text, Culture);
                }
            }
            catch
            {

            }

            return value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="baseName"></param>
        public static void InitializeDefaultResource(string baseName)
        {
            if (LocalizationResourceAccessor.ResourceManager == null)
            {
                Assembly assembly = Assembly.GetCallingAssembly();
                LocalizationResourceAccessor.ResourceManager = new System.Resources.ResourceManager(baseName, assembly);
                LocalizationResourceAccessor.Culture = new CultureInfo("en-US");
            }
        }

        #endregion
    }

}
