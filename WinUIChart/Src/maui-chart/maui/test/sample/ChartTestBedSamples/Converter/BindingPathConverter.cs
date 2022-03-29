using Microsoft.Maui.Controls;
using System;

namespace ChartTestBedSamples
{
	public class XBindingPathConverter : IValueConverter
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
					return "XDate";
				case 1:
					return "XValue1";
				case 2:
					return "XString";
				case 3:
					return null;
				case 4:
					return String.Empty;
			}
			return "XString";
		}
		#endregion

	}

	public class YBindingPathConverter : IValueConverter
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
					return "YValue1";
				case 1:
					return "YNegativeValue";
				case 2:
					return "YEmptyValue";
				case 3:
					return String.Empty;
				case 4:
					return null;
			}
			return "YValue1";
		}
		#endregion

	}

	public class SizeConverter : IValueConverter
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
					return "Size1";
				case 1:
					return "Size2";
				case 2:
					return string.Empty;
			}
			return "";
		}
		#endregion

	}
}