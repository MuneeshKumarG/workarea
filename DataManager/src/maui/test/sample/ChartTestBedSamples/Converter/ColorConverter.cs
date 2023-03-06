using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using System;

namespace ChartTestBedSamples
{
	public class ColorConverter : IValueConverter
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
					return Colors.Red;
				case 1:
					return Colors.Blue;
				case 2:
					return Colors.Gray;
				case 3:
					return Colors.Black;
				case 4:
					return Color.FromHex("#663300");
				case 5:
					return Color.FromRgb(0, 153, 51);
				case 6:
					return Color.FromRgba(204, 0, 204, 150);
				case 7:
					return Color.FromHex("#B2B2B2");
				case 8:
					return Colors.Lime;
				case 9:
					return Colors.Transparent;
				case 10:
					return Colors.Black;
			}
			return Colors.Red;
		}
		#endregion
	}
}