using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using System;
using System.Globalization;

namespace Syncfusion.Maui.Core
{
    /// <summary>
    /// Used to convert a legend item color with DisableBrush or IconBrush when toggle.
    /// </summary>
    internal class ToggleColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null && parameter != null)
            {
                bool toggled = (bool)value;
                SfShapeView? shape = parameter as SfShapeView;
                if (shape != null)
                {
                    LegendItem? item = shape.BindingContext as LegendItem;
                    if (item != null)
                    {
                        return toggled ? item.DisableBrush : item.IconBrush;
                    }
                }

                Label? label = parameter as Label;
                if (label != null)
                {
                    LegendItem? item = label.BindingContext as LegendItem;
                    if (item != null)
                    {
                        return toggled ? item.DisableBrush : item.TextColor;
                    }
                }
            }

            return Colors.Transparent;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }

    //Used to get the legend item color with disable brush or selection brush.
    internal class MultiBindingIconBrushConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values != null && targetType.IsAssignableFrom(typeof(Brush)))
            {
                SfShapeView? shape = parameter as SfShapeView;

                if (shape != null)
                {
                    LegendItem? item = shape.BindingContext as LegendItem;

                    if (item != null)
                    {
                        return item.IsToggled ? item.DisableBrush : item.IconBrush;
                    }
                }
            }

            return new SolidColorBrush(Colors.Transparent);
        }

        public object[]? ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
