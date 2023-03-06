using Syncfusion.Maui.Charts;
using System.Collections.ObjectModel;

namespace MauiNewApp1;

public partial class MainPage1 : ContentPage
{
	int count = 0;

	public MainPage1()
	{
		InitializeComponent();
	}

    private void Button_Clicked(object sender, EventArgs e)
    {
        realTimeChartViewModel.StartTimer();
    }

    private void Button_Clicked_1(object sender, EventArgs e)
    {
        realTimeChartViewModel.StopTimer();
    }
}

public class RealTimeChartViewModel
{
    private bool canStopTimer;
    private static int count = 0;
    private static int value = 0;
    readonly float[] datas1 = new float[]
    {
            762,772,762,772,772,770,766,763,765,772,763,768,764,772,762,
            766,768,766,762,772,774,766,770,767,777,772,762,772,765,766,
            762,766,766,770,768,765,772,766,766,766,772,774,771,774,769,
            780,780,777,788,794,778,775,777,783,786,775,765,780,770,770,
            770,772,771,770,772,780,771,770,766,787,788,775,780,779,780,
            784,774,790,774,779,788,788,774,791,786,788,758,779,786,777,
            764,799,788,823,784,642,783,804,703,784,790,823,806,834,816,
            760,608,804,809,786,810,794,836,801,844,798,823,812,828,835,
            818,819,811,806,820,828,811,810,812,813,806,784,825,805,830,
            819,826,802,835,1023,1001,1023,1019,1023,990,879,939,812,852,818,802,854,818,820,
            806,852,809,800,811,794,800,808,812,812,816,827,850,831,812,819,820,780,810,
    };

    public ObservableCollection<ChartDataModel> LiveData { get; set; }

    public RealTimeChartViewModel()
    {
        LiveData = new ObservableCollection<ChartDataModel>();
      //  UpdateLiveData();
        //if (BaseConfig.RunTimeDeviceLayout == SBLayout.Mobile)
        //{
        //    UpdateLiveData();
        //}
    }

    private void UpdateLiveData()
    {
        for (int i = 0; i < 500; i++)
        {
            if (count >= datas1.Length)
                count = 0;
            LiveData.Add(new ChartDataModel(value, datas1[count]));
            count++;
            value++;
        }
    }

    private bool UpdateData()
    {
        if (canStopTimer) return false;

        if (count >= datas1.Length)
            count = 0;

        value = LiveData.Count >= 1 ? (int)(LiveData[LiveData.Count - 1].Value) + 1 : 1;

        if (value > 250)
        {
            LiveData.RemoveAt(0);
        }

        LiveData.Add(new ChartDataModel(value, 700));
        count++;
        return true;
    }

    public void StopTimer()
    {
        canStopTimer = true;

        //if (!(BaseConfig.RunTimeDeviceLayout == SBLayout.Mobile))
        //{
        //    value = 0;
        //    count = 0;
        //    LiveData.Clear();
        //    UpdateLiveData();
        //}
    }
    public async void StartTimer()
    {
        await Task.Delay(500);

        if (Application.Current != null)
            Application.Current.Dispatcher.StartTimer(new TimeSpan(0, 0, 0, 0, 25), UpdateData);

        canStopTimer = false;
    }
}

public class ChartDataModel
{
    public string? Name { get; set; }

    public double Data { get; set; }

    public string? Label { get; set; }
    public DateTime Date { get; set; }

    public double Value { get; set; }

    public double Value1 { get; set; }

    public double Size { get; set; }

    public double High { get; set; }

    public double Low { get; set; }

    public bool IsSummary { get; set; }

    public string? Levels { get; set; }
    public string? Department { get; set; }

    public List<double>? Energy { get; set; }

    public ChartDataModel() { }

    public ChartDataModel(string department, List<double> employeeAges)
    {
        Levels = department;
        Energy = employeeAges;
    }

    public ChartDataModel(string name, double value)
    {
        Name = name;
        Value = value;
    }

    public ChartDataModel(string name, double value, double horizontalErrorValue, double verticalErrorValue)
    {
        Name = name;
        Value = value;
        High = horizontalErrorValue;
        Low = verticalErrorValue;
    }

    public ChartDataModel(string name, double value, double size)
    {
        Name = name;
        Value = value;
        Size = size;
    }

    public ChartDataModel(DateTime date, double value, double size)
    {
        Date = date;
        Value = value;
        Size = size;
    }

    public ChartDataModel(double value, double value1, double size)
    {
        Value1 = value;
        Value = value1;
        Size = size;
    }

    public ChartDataModel(double value1, double value, double size, string label)
    {
        Value1 = value1;
        Value = value;
        Size = size;
        Label = label;
    }

    public ChartDataModel(string name, double high, double low, double open, double close)
    {
        Name = name;
        High = high;
        Low = low;
        Value = open;
        Size = close;
    }

    public ChartDataModel(double name, double high, double low, double open, double close)
    {
        Data = name;
        High = high;
        Low = low;
        Value = open;
        Size = close;
    }

    public ChartDataModel(DateTime date, double high, double low, double open, double close)
    {
        Date = date;
        High = high;
        Low = low;
        Value = open;
        Size = close;
    }
    public ChartDataModel(double value, double size)
    {
        Value = value;
        Size = size;
    }
    public ChartDataModel(DateTime dateTime, double value)
    {
        Date = dateTime;
        Value = value;
    }

    public ChartDataModel(string name, double value, bool isSummary)
    {
        Name = name;
        Value = value;
        IsSummary = isSummary;
    }

    public ChartDataModel(DateTime date, double value, double value1, double value2)
    {
        Date = date;
        Value = value;
        High = value1;
        Low = value2;
    }
}

