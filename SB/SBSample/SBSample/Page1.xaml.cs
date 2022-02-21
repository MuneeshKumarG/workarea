using Syncfusion.Maui.Gauges;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SBSample;

public partial class Page1 : ContentPage, INotifyPropertyChanged
{
	public Page1()
	{
		InitializeComponent();
		BindingContext = this;
	}

    #region State

    private double pointerValue = 30;

    public double PointerValue
    {
        get { return pointerValue; }
        set { pointerValue = value; }
    }

	private void markerPointer_ValueChanging(object sender, Syncfusion.Maui.Gauges.ValueChangingEventArgs e)
	{
		if (Math.Abs(e.NewValue - PointerValue) > 20)
			e.Cancel = true;
		else
        {
			double value = e.NewValue;
			PointerValue = value > 50 ? Math.Ceiling(value) : Math.Floor(value);
			annotationLabel.Text = PointerValue.ToString() + "%";
		}
	}

    #endregion

    #region LabelsAndTicsk

    private double clockPointerValue = 9;

    public double ClockPointerValue
    {
        get { return clockPointerValue; }
        set { clockPointerValue = value; }
    }

	private void markerPointer_ValueChanging1(object sender, Syncfusion.Maui.Gauges.ValueChangingEventArgs e)
	{
		if (Math.Abs(e.NewValue - ClockPointerValue) > 2.4)
			e.Cancel = true;
		else
		{
			double value = e.NewValue;
			ClockPointerValue = value > 6 ? Math.Ceiling(value) : Math.Floor(value);
			clockAnnotationLabel.Text = ClockPointerValue.ToString() +  " hrs";
		}
	}

    #endregion

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

    private double anglePointerValue = 30;

	public double AnglePointerValue
	{
		get { return anglePointerValue; }
		set { anglePointerValue = value; }
	}

	private void markerPointer_ValueChanging3(object sender, Syncfusion.Maui.Gauges.ValueChangingEventArgs e)
	{
		if (Math.Abs(e.NewValue - AnglePointerValue) > 20)
			e.Cancel = true;
		else
		{
			double value = e.NewValue;
			AnglePointerValue = value > 50 ? Math.Ceiling(value) : Math.Floor(value);
			angleAnnotationLabel1.Text = AnglePointerValue.ToString() + "%";
		}
	}

	private void markerPointer_ValueChanging4(object sender, Syncfusion.Maui.Gauges.ValueChangingEventArgs e)
	{
		double value = e.NewValue > 50 ? Math.Ceiling(e.NewValue) : Math.Floor(e.NewValue);
		angleAnnotationLabel2.Text = value.ToString() + "%";
	}

	private void markerPointer_ValueChanging5(object sender, Syncfusion.Maui.Gauges.ValueChangingEventArgs e)
	{
		double value = e.NewValue > 50 ? Math.Ceiling(e.NewValue) : Math.Floor(e.NewValue);
		angleAnnotationLabel3.Text = value.ToString() + "%";
	}

	private void markerPointer_ValueChanging6(object sender, Syncfusion.Maui.Gauges.ValueChangingEventArgs e)
	{
		double value = e.NewValue > 50 ? Math.Ceiling(e.NewValue) : Math.Floor(e.NewValue);
		angleAnnotationLabel4.Text = value.ToString() + "%";
	}


	public event PropertyChangedEventHandler PropertyChanged;

	// This method is called by the Set accessor of each property.  
	// The CallerMemberName attribute that is applied to the optional propertyName  
	// parameter causes the property name of the caller to be substituted as an argument.  
	private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
	{
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}

	private void customTextMarker_ValueChanged(object sender, Syncfusion.Maui.Gauges.ValueChangedEventArgs e)
	{
		if (e.Value > 99)
		{
			Brush brush = new SolidColorBrush(Color.FromRgb(0, 168, 181));
			(customTextAxis.Pointers[0] as RangePointer).Fill = brush;
			(customTextAxis.Pointers[1] as MarkerPointer).Stroke = Color.FromRgb(0, 168, 181);
			customTextAnnotation.Text = "Completed";

		}
		else
		{
			Brush brush = new SolidColorBrush(Colors.Orange);
			(customTextAxis.Pointers[0] as RangePointer).Fill = brush;
			(customTextAxis.Pointers[1] as MarkerPointer).Stroke = Colors.Orange;
			customTextAnnotation.Text = "In-progress";
		}
	}

    private void RadialAxis_LabelCreated(object sender, LabelCreatedEventArgs e)
    {
		double axisValue = double.Parse(e.Text);
		double celsiusValue = (axisValue - 32) / 1.8;
		e.Text = Math.Round(celsiusValue, 1).ToString();
	}
}

