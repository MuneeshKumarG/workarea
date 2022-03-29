using Microsoft.UI.Xaml.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Resources;
using Windows.ApplicationModel.Resources.Core;

namespace Syncfusion.UI.Xaml.Core
{
    /// <summary>
    /// Represents the class that manages the localization for syncfusion controls.
    /// </summary>
    public abstract class LocalizationResourceAccessor
    {
        /// <summary>
        /// Initializes new instance of <see cref="LocalizationResourceAccessor"/>.
        /// </summary>
        public LocalizationResourceAccessor()
        {

        }

        /// <summary>
        /// Gets or sets the custom resource map. 
        /// </summary>
        public ResourceMap CustomResourceMap { get; set; }

        /// <summary>
        /// Gets the default resource map of the controls in the assembly.
        /// </summary>
        protected ResourceMap DefaultResourceMap { get; set; }

        /// <summary>
        /// Returns to localized string based on <see cref="System.Globalization.CultureInfo.CurrentUICulture"/>.
        /// </summary>
        /// <param name="resourcename">Resource name to get localized string.</param>
        /// <returns>Localized string resource.</returns>
        public virtual string GetLocalizedStringResource(string resourcename)
        {
            ResourceContext resourceContext;
            if (Windows.UI.Core.CoreWindow.GetForCurrentThread() == null)
            {
                resourceContext = new ResourceContext();
            }
            else
            {
                resourceContext = ResourceContext.GetForCurrentView();
            }
            if (this.CustomResourceMap != null)
            {
                var resourcecandidate = this.CustomResourceMap.GetValue(resourcename, resourceContext);
                if (resourcecandidate != null)
                {
                    return resourcecandidate.ValueAsString;
                }
            }
            return this.DefaultResourceMap.GetValue(resourcename, resourceContext).ValueAsString;
        }
    }

    /// <summary>
    /// Value converter that look up for the source string in based on culture with the help of <see cref="LocalizationResourceAccessor"/> and returns its value, if found.
    /// </summary>
    public class ResourceNameToResourceStringConverter : IValueConverter
    {
        /// <summary>
        /// Take the source string as a resource name that will be looked up in the App Resources.
        /// If the resource exists, the value is returned, otherwise.
        /// </summary>
        /// <param name="value">The resource name to get localized resource string.</param>
        /// <param name="targetType">The type of the target property, as a type reference.</param>
        /// <param name="parameter">Optional parameter. resource name to get localized resource sting.</param>
        /// <param name="language">The language of the conversion.</param>
        /// <returns>The string corresponding to the resource name.</returns>
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            string resourcename = string.Empty;
            if (value != null)
            {
                resourcename = value.ToString();
            }
            else if(parameter != null)
            {
                resourcename = parameter.ToString();
            }
            if (string.IsNullOrEmpty(resourcename))
            {
                return string.Empty;
            }

            var resourceaccessor = this.GetResourceAccessor();
            if (resourceaccessor == null)
            {
                return string.Empty;
            }

            return GetResourceAccessor().GetLocalizedStringResource(resourcename);
        }

        /// <summary>
        /// Returns the <see cref="LocalizationResourceAccessor"/> associated with control assembly.
        /// </summary>
        /// <returns></returns>
        protected virtual LocalizationResourceAccessor GetResourceAccessor()
        {
            return null;
        }

        /// <summary>
        /// Not implemented.
        /// </summary>
        /// <param name="value">The source string being passed to the target.</param>
        /// <param name="targetType">The type of the target property, as a type reference.</param>
        /// <param name="parameter">Optional parameter. Not used.</param>
        /// <param name="language">The language of the conversion.</param>
        /// <returns>The value to be passed to the target dependency property.</returns>
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
