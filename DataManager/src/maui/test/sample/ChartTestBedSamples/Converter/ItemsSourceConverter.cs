using Microsoft.Maui.Controls;
using System;

namespace ChartTestBedSamples
{
	public class ItemsSourceConverter : IValueConverter
	{
		#region IValueConverter implementation
		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			if (value == null)
				return 9;
			return value;
		}
		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			int index = (int)value;

			switch (index)
			{
				case 0:
					return new ViewModel().DateTimeData;
				case 1:
					return new ViewModel().StringData;
				case 2:
					return new ViewModel().NumericalData;
				case 3:
					return new ViewModel().FinancialData;
				case 4:
					return new ViewModel().DataPointValues;
				case 5:
					return new ViewModel().DateTimeSingleData;
				case 6:
					return new ViewModel().StringSingleData;
				case 7:
					return new ViewModel().NumericalSingleData;
				case 8:
					return new ViewModel().FinancialData;
				case 9:
					return new ViewModel().NullSource;
				case 10:
					return new ViewModel().DataPointNeg;
				case 11:
					return new ViewModel().DataPointPos;
				case 12:
					return new ViewModel().DataPointEmpPosNeg;
				case 13:
					return new ViewModel().NullSource;
				case 14:
					return new ViewModel().NullDPSource;
				case 15:
					return new ViewModel().FLEmpty;
				case 16:
					return new ViewModel().FNLPEmpty;
				case 17:
					return new ViewModel().SingleEmp;
				case 18:
					return new ViewModel().SinglePos;
				case 19:
					return new ViewModel().SingleNeg;
				case 20:
					return new ViewModel().ListData;
			}
			return value;
		}
		#endregion
	}

	public class CategoryItemsSourceConverter : IValueConverter
	{
		#region IValueConverter implementation
		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			if (value == null)
				return 9;
			return value;
		}
		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			int index = (int)value;

			switch (index)
			{
				case 0:
					return new AxisViewModel().CatEmptyPoint;
				case 1:
					return new AxisViewModel().CatNegative;
				case 2:
					return new AxisViewModel().CatPositive;
				case 3:
					return new AxisViewModel().CatEmptyNegPos;
				case 4:
					return new AxisViewModel().CatFirstLastEmpty;
				case 5:
					return new AxisViewModel().CatSingleEmpty;
				case 6:
					return new AxisViewModel().CatSinglePositive;
				case 7:
					return new AxisViewModel().CatSingleNegative;
				case 8:
					return new AxisViewModel().WithoutData;
				case 9:
					return null;
				case 10:
					return new AxisViewModel().DatePositive;
				case 11:
					return new AxisViewModel().MoreDataPoints;
				case 12:
					return new AxisViewModel().AllZero;
			}
			return new AxisViewModel().CatPositive;
		}
		#endregion
	}

	public class NumericItemsSourceConverter : IValueConverter
	{
		#region IValueConverter implementation
		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			if (value == null)
				return 9;
			return value;
		}
		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			int index = (int)value;

			switch (index)
			{
				case 0:
					return new AxisViewModel().NumEmptyPoint;
				case 1:
					return new AxisViewModel().NumNegative;
				case 2:
					return new AxisViewModel().NumPositive;
				case 3:
					return new AxisViewModel().NumEmptyNegPos;
				case 4:
					return new AxisViewModel().NumFirstLastEmpty;
				case 5:
					return new AxisViewModel().NumSingleEmpty;
				case 6:
					return new AxisViewModel().NumSinglePositive;
				case 7:
					return new AxisViewModel().NumSingleNegative;
				case 8:
					return new AxisViewModel().WithoutData;
				case 9:
					return null;
				case 10:
					return new AxisViewModel().NumFirstNextLastPrevEmpty;
				case 11:
					return new AxisViewModel().AllZero;
				case 12:
					return new AxisViewModel().LongXVal;
			}
			return new AxisViewModel().NumPositive;
		}
		#endregion
	}
	public class DateTimeItemsSourceConverter : IValueConverter
	{
		#region IValueConverter implementation
		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			if (value == null)
				return 9;
			return value;
		}
		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			int index = (int)value;

			switch (index)
			{
				case 0:
					return new AxisViewModel().DateEmptyPoint;
				case 1:
					return new AxisViewModel().DateNegative;
				case 2:
					return new AxisViewModel().DatePositive;
				case 3:
					return new AxisViewModel().DateEmptyNegPos;
				case 4:
					return new AxisViewModel().DateFirstLastEmpty;
				case 5:
					return new AxisViewModel().DateSingleEmpty;
				case 6:
					return new AxisViewModel().DateSinglePositive;
				case 7:
					return new AxisViewModel().DateSingleNegative;
				case 8:
					return new AxisViewModel().WithoutData;
				case 9:
					return null;
				case 10:
					return new AxisViewModel().DateMillisecond;
			}
			return new AxisViewModel().DatePositive;
		}
		#endregion
	}

	public class LabelConverter : IValueConverter
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
					return String.Empty;
				case 1:
					return "";
				case 2:
					return "Series";
				case 3:
					return "ChartTitle";
			}
			return "Series";
		}

		#endregion
	}
}