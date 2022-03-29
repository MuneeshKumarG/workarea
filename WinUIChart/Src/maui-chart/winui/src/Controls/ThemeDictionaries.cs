﻿// <copyright file="ThemeDictionaries.cs" company="Syncfusion. Inc">
// Copyright Syncfusion Inc. 2001 - 2017. All rights reserved.
// Use of this code is subject to the terms of our license.
// A copy of the current license can be obtained at any time by e-mailing
// licensing@syncfusion.com. Any infringement will be prosecuted under
// applicable laws. 
// </copyright>
namespace Syncfusion.UI.Xaml.Charts
{

#if WinUI_Desktop
    using Microsoft.UI.Xaml;
#else
    using Windows.UI.Xaml;
#endif

    /// <summary>
    /// Represents the <see cref="ThemeDictionaries"/> class.
    /// </summary>
    public class ThemeDictionaries : ResourceDictionary
    {
        #region Fields

        private ResourceDictionary lightThemeDictionary;

        private ResourceDictionary darkThemeDictionary;

        #endregion

        #region Properties

        #region Public Properties

        /// <summary>
        /// Gets or sets the light theme for phone
        /// </summary>
        public ResourceDictionary LightThemeDictionary
        {
            get
            {
                return lightThemeDictionary;
            }

            set
            {
                lightThemeDictionary = value;

                if (!IsDarkTheme && value != null)
                {
                    MergedDictionaries.Add(value);
                }
            }
        }

        /// <summary>
        /// Gets or sets the dark theme for phone
        /// </summary>
        public ResourceDictionary DarkThemeDictionary
        {
            get
            {
                return darkThemeDictionary;
            }

            set
            {
                darkThemeDictionary = value;
                if (IsDarkTheme && value != null)
                {
                    MergedDictionaries.Add(value);
                }
            }
        }

        #endregion

        #region Private Properties

        /// <summary>
        /// Gets a value indicating whether is dark theme applied.
        /// </summary>
        private static bool IsDarkTheme
        {
            get
            {
                return (Visibility)Application.Current.Resources["PhoneDarkThemeVisibility"] == Visibility.Visible;
            }
        }

        #endregion

        #endregion
    }
}
