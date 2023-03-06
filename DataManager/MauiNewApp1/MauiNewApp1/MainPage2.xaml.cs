using System.Collections.ObjectModel;
using System.ComponentModel;

namespace MauiNewApp1;

public partial class MainPage2 : ContentPage
{
	int count = 0;

	public MainPage2()
	{
		InitializeComponent();
	}

    private void Button_Clicked(object sender, EventArgs e)
    {
		series2.XBindingPath = "XString";
    }
}

public class Model : INotifyPropertyChanged
{
	public int XValue { get; set; }

	public string XString { get; set; }

	private double yValue;

	public double YValue
	{
		get { return yValue; }
		set { yValue = value; }
	}

	private double yValue2;

	public double YValue2
	{
		get { return yValue2; }
		set { yValue2 = value; }
	}



	public event PropertyChangedEventHandler PropertyChanged;
}

public class ViewModel
{
	public ObservableCollection<Model> Data { get; set; }

    public ObservableCollection<Model> Data2 { get; set; }

    public ViewModel()
	{
		Random random = new Random();
		Data = new ObservableCollection<Model>();
		for (int i = 0; i < 100; i++)
		{
			Data.Add(new Model() {XString=i.ToString(), XValue = i, YValue = random.Next(0, 50), 
				YValue2 = random.Next(0, 50) });
		}

        Data2 = new ObservableCollection<Model>();
        for (int i = 0; i < 100; i++)
        {
            Data2.Add(new Model() { XValue = i, YValue = random.Next(0, 50) });
        }
    }
}

