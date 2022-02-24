using Syncfusion.Maui.Gauges;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SBSample;

public partial class Page4 : ContentPage, INotifyPropertyChanged
{
	private double size;

	public double Size
	{
		get { return size; }
		set
		{
			size = value;
			NotifyPropertyChanged();
		}
	}

	protected override void OnSizeAllocated(double width, double height)
	{
		base.OnSizeAllocated(width, height);

		Size = width / 4;
	}

	public event PropertyChangedEventHandler PropertyChanged;

	// This method is called by the Set accessor of each property.  
	// The CallerMemberName attribute that is applied to the optional propertyName  
	// parameter causes the property name of the caller to be substituted as an argument.  
	private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
	{
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}



	public Page4()
	{
		InitializeComponent();
		BindingContext = this;
	}

    private void SfLinearGauge_LabelCreated(object sender, LabelCreatedEventArgs e)
    {
		e.Text = e.Text + " cm";
    }

    private double heightPointerValue = 130;

    public double HeightPointerValue
    {
        get { return heightPointerValue; }
        set { heightPointerValue = value; NotifyPropertyChanged(); }
    }

    private void Pointer1_ValueChanged(object sender, Syncfusion.Maui.Gauges.ValueChangedEventArgs e)
    {
		HeightPointerValue = e.Value;
		heightAnnotationLabel.Text = (int)HeightPointerValue + " cm";
	}



    private double waterLevelPointerValue = 150;

    public double WaterLavelPointerValue
    {
        get { return waterLevelPointerValue; }
        set { waterLevelPointerValue = value; NotifyPropertyChanged(); }
    }

	private void Pointer1_ValueChanged1(object sender, Syncfusion.Maui.Gauges.ValueChangedEventArgs e)
	{
		WaterLavelPointerValue = e.Value;
		waterLevelAnnotationLabel.Text = ((int)WaterLavelPointerValue).ToString()+" ml";
	}

    
}

public class TaskTrackerGauge :SfLinearGauge
{
	public override List<GaugeLabelInfo> GenerateVisibleLabels()
	{
		List<GaugeLabelInfo> customLabels = new List<GaugeLabelInfo>();
		customLabels.Add(new GaugeLabelInfo { Value = 0, Text = "Mar 19" });
		customLabels.Add(new GaugeLabelInfo { Value = 20, Text = "Jul 19" });
		customLabels.Add(new GaugeLabelInfo { Value = 40, Text = "Oct 19" });
		customLabels.Add(new GaugeLabelInfo { Value = 60, Text = "Jan 20" });
		customLabels.Add(new GaugeLabelInfo { Value = 80, Text = "Apr 20" });
		customLabels.Add(new GaugeLabelInfo { Value = 100, Text = "Jul 20" });

		return customLabels;

	}
}

public class SleepWatchScoreGauge : SfLinearGauge
{
	public override List<GaugeLabelInfo> GenerateVisibleLabels()
	{
		List<GaugeLabelInfo> customLabels = new List<GaugeLabelInfo>();
		customLabels.Add(new GaugeLabelInfo { Value = 150, Text = "Poor" });
		customLabels.Add(new GaugeLabelInfo { Value = 250, Text = "Fair" });
		customLabels.Add(new GaugeLabelInfo { Value = 350, Text = "Good" });
		customLabels.Add(new GaugeLabelInfo { Value = 450, Text = "Excellent" });

		return customLabels;

	}
}

