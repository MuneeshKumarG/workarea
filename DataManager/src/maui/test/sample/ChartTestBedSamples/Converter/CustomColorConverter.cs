using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using System;
using System.Collections;
using System.Collections.Generic;

namespace ChartTestBedSamples
{
	public class CustomColorConverter : IValueConverter
	{
		#region IValueConverter implementation
		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			if (value == null)
				return -1;

			return value;
		}
		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			int index = (int)value;

			IList<Color> colors = new List<Color>();
			switch (index)
			{
				case 0:
					colors.Add(Colors.Red);
					colors.Add(Colors.Green);
					colors.Add(Colors.Blue);
					colors.Add(Colors.Gray);
					colors.Add(Colors.Lime);
					return colors;
				case 1:
					colors.Add(Colors.Lime);
					colors.Add(Colors.Olive);
					colors.Add(Colors.Navy);
					return colors;
				case 2:
					colors.Add(Color.FromRgb(0, 151, 51));
					colors.Add(Color.FromRgb(0, 102, 153));
					colors.Add(Color.FromRgb(204, 152, 0));
					return colors;
				case 3:
					colors.Add(Color.FromRgba(204, 0, 204, 200));
					colors.Add(Color.FromRgba(102, 102, 153, 150));
					colors.Add(Color.FromRgba(153, 51, 0, 255));
					return colors;
				case 4:
					colors.Add(Color.FromHex("#D0D0D0"));
					colors.Add(Color.FromHex("#767676"));
					colors.Add(Color.FromHex("#303030"));
					return colors;
			}
			return colors;
		}
		#endregion

	}
}