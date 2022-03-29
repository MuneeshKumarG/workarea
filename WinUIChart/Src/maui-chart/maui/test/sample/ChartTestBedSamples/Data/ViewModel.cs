using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;

namespace ChartTestBedSamples
{
	public class ViewModel
	{
		public ObservableCollection<Model> DateTimeData
		{
			get;
			set;
		}

		public ObservableCollection<Model> StringData
		{
			get;
			set;
		}

		public ObservableCollection<Model> NumericalData
		{
			get;
			set;
		}

		public ObservableCollection<Model> FinancialData
		{
			get;
			set;
		}

		public ObservableCollection<Model> DataPointValues
		{
			get;
			set;
		}

		public ObservableCollection<Model> DataPointPos
		{
			get;
			set;
		}

		public ObservableCollection<Model> DataPointNeg
		{
			get;
			set;
		}

		public ObservableCollection<Model> FLEmpty
		{
			get;
			set;
		}

		public ObservableCollection<Model> FNLPEmpty
		{
			get;
			set;
		}

		public ObservableCollection<Model> SingleEmp
		{
			get;
			set;
		}

		public ObservableCollection<Model> SingleNeg
		{
			get;
			set;
		}

		public ObservableCollection<Model> SinglePos
		{
			get;
			set;
		}

		public ObservableCollection<Model> DataPointEmpPosNeg
		{
			get;
			set;
		}


		public ObservableCollection<Model> FinancialDataPointValues
		{
			get;
			set;
		}

		public ObservableCollection<Model> DateTimeSingleData
		{
			get;
			set;
		}

		public ObservableCollection<Model> StringSingleData
		{
			get;
			set;
		}

		public ObservableCollection<Model> NumericalSingleData
		{
			get;
			set;
		}

		public ObservableCollection<Model> NullDPSource
		{
			get;
			set;
		}

		public ObservableCollection<Model> NullSource
		{
			get;
			set;
		}

		public List<Model> ListData
		{
			get;
			set;
		}

		public ViewModel()
		{
			DateTimeData = DateTimeDatas();
			StringData = StringDatas();
			NumericalData = DataValues();

			ListData = new List<Model>()
			{
				new Model(12, 45,12),
				new Model(14, 86,23),
				new Model(21, 45,11),
				new Model(22, 43,21),
				new Model(23, 54,10)
			};

			NullSource = new ObservableCollection<Model>();

			NullDPSource = new ObservableCollection<Model>();

			DataPointValues = new ObservableCollection<Model>()
			{
				new Model("2010", 45,23),
				new Model("2011", 86,12),
				new Model("2012", double.NaN,11),
				new Model("2013", 43,21),
				new Model("2014", 54,10)
			};

			FLEmpty = new ObservableCollection<Model>()
			{
				new Model(12, double.NaN,10),
				new Model(14, 86,12),
				new Model(21, 45,11),
				new Model(22, 43,21),
				new Model(23, double.NaN,10)
			};

			FNLPEmpty = new ObservableCollection<Model>()
			{
				new Model(11, 86,23),
				new Model(12, double.NaN,12),
				new Model(21, 45,11),
				new Model(23, double.NaN,21),
				new Model(24, 43,10)
			};

			SingleEmp = new ObservableCollection<Model>()
			{
				new Model(12, double.NaN,10)
			};

			SinglePos = new ObservableCollection<Model>()
			{
				new Model(12, 50,23)
			};

			SingleNeg = new ObservableCollection<Model>()
			{
				new Model(12, -45,12)
			};

			DataPointPos = new ObservableCollection<Model>()
			{
				new Model(12, 45,12),
				new Model(14, 86,23),
				new Model(21, 45,11),
				new Model(22, 43,21),
				new Model(23, 54,10)
			};

			DataPointNeg = new ObservableCollection<Model>()
			{
				new Model(12, -45,12),
				new Model(14, -86,23),
				new Model(21, -45,11),
				new Model(22, -43,21),
				new Model(23, -54,10)
			};

			DataPointEmpPosNeg = new ObservableCollection<Model>()
			{
				new Model(12, -45,12),
				new Model(14, 86,23),
				new Model(21, double.NaN,11),
				new Model(22, 43,21),
				new Model(23, -54,10)
			};

			Model model = new Model();

			model.XDate = new DateTime(2007, 1, 1);
			model.XString = "2007";
			model.XValue1 = 10;

			StringSingleData = new ObservableCollection<Model>() {
				new Model ("2007", 50.0, 50, double.NaN)
			};

			DateTimeSingleData = new ObservableCollection<Model>() {
				new Model (new DateTime(2010,1,1), 50.0, 50, double.NaN)
			};

			NumericalSingleData = new ObservableCollection<Model>() {
				new Model (10, 50.0, 50, double.NaN)
			};
		}


		public static ObservableCollection<Model> StringDatas()
		{
			var datas = new ObservableCollection<Model>();
			datas.Add(new Model("2007", 50.0, 50, double.NaN));
			datas.Add(new Model("2008", 50.0, 50, 50));
			datas.Add(new Model("2009", 45.0, -30, 50));
			datas.Add(new Model("2010", 70.0, 40, double.NaN));
			datas.Add(new Model("2011", 20.0, 80, double.NaN));
			datas.Add(new Model("2012", 60.0, -60, 50));
			datas.Add(new Model("2013", 30.0, 50, 50));
			datas.Add(new Model("2014", 60.0, -60, 50));
			datas.Add(new Model("2015", 30.0, 50, double.NaN));
			return datas;
		}

		public static ObservableCollection<Model> DateTimeDatas()
		{
			var datas = new ObservableCollection<Model>();
			datas.Add(new Model(new DateTime(2007, 1, 1), 50.0, 50, double.NaN));
			datas.Add(new Model(new DateTime(2008, 1, 1), 50.0, 50, 50));
			datas.Add(new Model(new DateTime(2009, 1, 1), 45.0, -30, 50));
			datas.Add(new Model(new DateTime(2010, 1, 1), 70.0, 40, double.NaN));
			datas.Add(new Model(new DateTime(2011, 1, 1), 20.0, 80, double.NaN));
			datas.Add(new Model(new DateTime(2012, 1, 1), 60.0, -60, 50));
			datas.Add(new Model(new DateTime(2013, 1, 1), 30.0, 50, 50));
			datas.Add(new Model(new DateTime(2014, 1, 1), 60.0, -60, 50));
			datas.Add(new Model(new DateTime(2015, 1, 1), 30.0, 50, double.NaN));
			return datas;
		}

		public static ObservableCollection<Model> DataValues()
		{
			var datas = new ObservableCollection<Model>();
			datas.Add(new Model(10, 50.0, 50, double.NaN));
			datas.Add(new Model(20, 50.0, 50, 50));
			datas.Add(new Model(30, 45.0, -30, 50));
			datas.Add(new Model(40, 70.0, 40, double.NaN));
			datas.Add(new Model(50, 20.0, 80, double.NaN));
			datas.Add(new Model(60, 60.0, -60, 50));
			datas.Add(new Model(70, 30.0, 50, 50));
			datas.Add(new Model(80, 60.0, -60, 50));
			datas.Add(new Model(90, 30.0, 50, double.NaN));
			return datas;
		}

	}

	public class Model
	{
		public Model()
		{

		}

		public Model(string x, double yvalue)
		{
			XString = x;
			YValue1 = yvalue;
		}

		public Model(string x, double yvalue, double yNeg, double yEmpty)
		{
			XString = x;
			YValue1 = yvalue;
			Size1 = yvalue / 4;
			Size2 = yvalue / 2;
			YNegativeValue = yNeg;
			YEmptyValue = yEmpty;
		}

		public Model(DateTime x, double yvalue)
		{
			XDate = x;
			YValue1 = yvalue;
		}

		public Model(DateTime x, double yvalue, double yNeg, double yEmpty)
		{
			XDate = x;
			YValue1 = yvalue;
			Size1 = yvalue / 4;
			Size2 = yvalue / 2;
			YNegativeValue = yNeg;
			YEmptyValue = yEmpty;
		}

		public Model(double xvalue, double yvalue)
		{
			XValue1 = xvalue;
			YValue1 = yvalue;
		}

		public Model(double x, double yvalue, double yNeg, double yEmpty)
		{
			XValue1 = x;
			YValue1 = yvalue;
			Size1 = yvalue / 4;
			Size2 = yvalue / 2;
			YNegativeValue = yNeg;
			YEmptyValue = yEmpty;
		}

		public Model(double x, double yvalue, double yvalue1)
		{
			XValue1 = x;
			YValue1 = yvalue;
			Size1 = yvalue1;
		}

		public Model(string x, double yvalue, double yvalue1)
		{
			XString = x;
			YValue1 = yvalue;
			Size1 = yvalue1;
		}

		public DateTime XDate
		{
			get;
			set;
		}

		public double XValue1
		{
			get;
			set;
		}

		public string XString
		{
			get;
			set;
		}

		public double YValue1
		{
			get;
			set;
		}

		public double YNegativeValue
		{
			get;
			set;
		}

		public double YEmptyValue
		{
			get;
			set;
		}

		public double Size1
		{
			get;
			set;
		}

		public double Size2
		{
			get;
			set;
		}
	}
}