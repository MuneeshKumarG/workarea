using Microsoft.Maui.Controls;
using Syncfusion.Maui.Charts;
using System;

namespace ChartTestBedSamples
{
	public class PaletteConverter : IValueConverter
	{
		#region IValueConverter implementation

		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			return value;
		}

		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			int index = (int)value;

			switch (index)
			{
				case 0:
					return ChartColorPalette.None;
				case 1:
					return ChartColorPalette.Metro;
				case 2:
					return ChartColorPalette.Custom;
			}
			return ChartColorPalette.Metro;
		}

		#endregion
	}
}