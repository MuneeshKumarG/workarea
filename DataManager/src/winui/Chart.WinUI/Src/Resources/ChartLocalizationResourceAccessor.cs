using Microsoft.UI.Xaml.Markup;
using Syncfusion.UI.Xaml.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syncfusion.UI.Xaml.Charts
{
    /// <summary>
    /// Represents the class that manages the localization of <see cref="ChartBase"/> control based on <see cref="System.Globalization.CultureInfo.CurrentUICulture"/>.
    /// </summary>
    internal sealed class ChartLocalizationResourceAccessor: LocalizationResourceAccessor
    {
        /// <summary>
        /// Gets the instance of <see cref="ChartLocalizationResourceAccessor"/> which manages the localization of chart control.
        /// </summary>
        public static ChartLocalizationResourceAccessor Instance = new ChartLocalizationResourceAccessor();

        /// <summary>
        /// Initializes new instance of <see cref="ChartLocalizationResourceAccessor"/>.
        /// </summary>
        private ChartLocalizationResourceAccessor()
        {
            this.DefaultResourceMap = Windows.ApplicationModel.Resources.Core.ResourceManager.Current.MainResourceMap.GetSubtree("Syncfusion.Chart.WinUI/Syncfusion.Chart.WinUI");
            this.CustomResourceMap = Windows.ApplicationModel.Resources.Core.ResourceManager.Current.MainResourceMap.GetSubtree("Syncfusion.Chart.WinUI");
        }
    }

    /// <summary>
    /// A markup extension that returns the localized string based on culture for the <see cref="ChartLocalizationResourceExtension.ResourceName"/>.
    /// </summary>

    [MarkupExtensionReturnType(ReturnType = typeof(string))]
    internal class ChartLocalizationResourceExtension : MarkupExtension
    {
        /// <summary>
        /// Gets or sets the resource name to get the localized string.
        /// </summary>
        public string ResourceName { get; set; }

        /// <inheritdoc/>
        protected override object ProvideValue()
        {
            return ChartLocalizationResourceAccessor.Instance.GetLocalizedStringResource(ResourceName);
        }
    }
}
