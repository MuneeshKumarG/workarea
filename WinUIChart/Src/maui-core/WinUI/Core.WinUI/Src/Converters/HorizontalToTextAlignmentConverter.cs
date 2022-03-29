// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Data;

namespace Syncfusion.UI.Xaml.Core
{
    /// <summary>
    /// A converter that converts HorizontalAlignment to TextAlignment.
    /// </summary>
    public class HorizontalToTextAlignmentConverter : IValueConverter
    {
        /// <summary>
        /// Converts a value from HorizontalAlignment to TextAlignment.
        /// </summary>
        /// <param name="value">Object to transform to string.</param>
        /// <param name="targetType">The type of the target property, as a type reference</param>
        /// <param name="parameter">An optional parameter to be used in the string.Format method.</param>
        /// <param name="language">The language of the conversion (not used).</param>
        /// <returns>Formatted string.</returns>
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            TextAlignment textAlignment;
            switch (value)
            {
                case HorizontalAlignment.Right:
                    textAlignment = TextAlignment.Right;
                    break;
                case HorizontalAlignment.Center:
                    textAlignment = TextAlignment.Center;
                    break;

                case HorizontalAlignment.Stretch:
                    textAlignment = TextAlignment.Justify;
                    break;
                default:
                    textAlignment = TextAlignment.Left;
                    break;
            }
            return textAlignment;
        }

        /// <summary>
        /// Converts back to the value from TextAlignment to HorizontalAlignment.
        /// </summary>
        /// <param name="value">The target data being passed to the source.</param>
        /// <param name="targetType">The type of the target property, as a type reference (System.Type for Microsoft .NET, a TypeName helper struct for Visual C++ component extensions (C++/CX)).</param>
        /// <param name="parameter">An optional parameter to be used in the converter logic.</param>
        /// <param name="language">The language of the conversion.</param>
        /// <returns>The value to be passed to the source object.</returns>
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            HorizontalAlignment horizontalAlignment;
            switch (value)
            {
                case TextAlignment.Right:
                    horizontalAlignment = HorizontalAlignment.Right;
                    break;
                case TextAlignment.Center:
                    horizontalAlignment = HorizontalAlignment.Center;
                    break;
                case TextAlignment.Justify:
                    horizontalAlignment = HorizontalAlignment.Stretch;
                    break;
                default:
                    horizontalAlignment = HorizontalAlignment.Left;
                    break;
            }

            return horizontalAlignment;
        }
    }
}