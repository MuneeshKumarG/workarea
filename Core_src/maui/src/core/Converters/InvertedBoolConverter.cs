using Microsoft.Maui.Controls;
using System;
using System.Globalization;

namespace Syncfusion.Maui.Core.Converters
{
    /// <summary>
    /// Converts true to false and false to true.
    /// </summary>
    public class InvertedBoolConverter : IValueConverter
    {
        /// <summary>
        /// Convert a boolean value to its inverse value.
        /// </summary>
        /// <param name="value">Boolean value to inverse.</param>
        /// <param name="targetType">The type of the target property.</param>
        /// <param name="parameter">Optional. Additonal parameter for converter to handle.</param>
        /// <param name="culture">Instance of CultureInfo. Not used</param>
        /// <returns>Inverted boolean value of input value.</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return InvertBool(value);
        }

        /// <summary>
        /// Convert back a boolean value to its inverse value.
        /// </summary>
        /// <param name="value">Boolean value to inverse.</param>
        /// <param name="targetType">The type of the target property.</param>
        /// <param name="parameter">Additonal parameter for converter to handle. Not used</param>
        /// <param name="culture">Instance of CultureInfo. Not used</param>
        /// <returns>Inverted boolean value of input value.</returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
           return InvertBool(value);
        }

        /// <summary>
        /// Invert the given boolean value.
        /// </summary>
        /// <param name="value">Input value.</param>
        /// <returns>Returns inverse of given boolean value.</returns>
        static object InvertBool(object value)
        {
            if (value is bool)
            {
                return !(bool)value;
            }

            return value;
        }
    }
}
