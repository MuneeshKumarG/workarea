using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
#if SyncfusionLicense
using Syncfusion.Licensing;
#endif
using System;
using System.ComponentModel;

namespace Syncfusion.Core.WinUI
{
    /// <summary>
	/// LicenseHelper class used for validate the license of syncfusion controls
	/// </summary>
	/// <exclude/>
	[EditorBrowsable(EditorBrowsableState.Never)]
    [Browsable(false)]
    public class LicenseHelper 
    {
        static LicenseHelper() 
        {
             
        }

#if SyncfusionLicense
        /// <summary>
        /// Validates the license for WinUI XAML controls in .NET50 project
        /// </summary>
        public static async void ValidateLicense(XamlRoot xamlRoot)
        {
            var licenseMessage = FusionLicenseProvider.GetLicenseType(Platform.WinUI);
            if (!string.IsNullOrEmpty(licenseMessage) && xamlRoot != null)
            {
                ContentDialog dialog = new ContentDialog
                {
                    Title = "Syncfusion License",
                    Content = licenseMessage,
                    PrimaryButtonText = "Close",
                    XamlRoot = xamlRoot
                };

                await dialog.ShowAsync();
            }
        }
#endif
    }
}
