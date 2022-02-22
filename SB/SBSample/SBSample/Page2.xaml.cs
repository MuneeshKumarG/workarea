using Syncfusion.Maui.Gauges;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SBSample;

public partial class Page2 : ContentPage, INotifyPropertyChanged
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

	public Page2()
	{
		InitializeComponent();
	}


	private void markerPointer_ValueChanging1(object sender, ValueChangingEventArgs e)
	{
		if (Math.Abs(e.NewValue - e.OldValue) > 20 || e.NewValue >= markerPointer2.Value ||
		Math.Abs(e.NewValue - markerPointer1.Value) > 10)
			e.Cancel = true;
		else
			UpdateAnnotationLabel(e.NewValue, true);
	}

	private void markerPointer_ValueChanging2(object sender, ValueChangingEventArgs e)
	{
		if (Math.Abs(e.NewValue - e.OldValue) > 20 || e.NewValue <= markerPointer1.Value ||
		Math.Abs(e.NewValue - markerPointer2.Value) > 10)
			e.Cancel = true;
		else
			UpdateAnnotationLabel(e.NewValue, false);
	}

	private void UpdateAnnotationLabel(double newValue, bool isFirstMarker)
	{
		double value = newValue;
		value = value > 50 ? Math.Ceiling(value) : Math.Floor(value);

		if (isFirstMarker)
			annotationLabel.Text = value.ToString() + " - " + (int)markerPointer2.Value + "%";
		else
			annotationLabel.Text = (int)markerPointer1.Value + " - " + value.ToString() + "%";
	}

	private void clockMarkerPointer1_ValueChanging(object sender, ValueChangingEventArgs e)
	{
		if (Math.Abs(e.NewValue - e.OldValue) > 2.4 || e.NewValue >= clockMarkerPointer2.Value ||
		Math.Abs(e.NewValue - clockMarkerPointer1.Value) > 1.2)
			e.Cancel = true;
		else
			UpdateClockAnnotationLabel(e.NewValue, true);
	}

	private void clockMarkerPointer2_ValueChanging(object sender, ValueChangingEventArgs e)
	{
		if (Math.Abs(e.NewValue - e.OldValue) > 2.4 || e.NewValue <= clockMarkerPointer1.Value ||
		Math.Abs(e.NewValue - clockMarkerPointer2.Value) > 1.2)
			e.Cancel = true;
		else
			UpdateClockAnnotationLabel(e.NewValue, false);
	}

	private void UpdateClockAnnotationLabel(double newValue, bool isFirstMarker)
	{
		double value = newValue;
		value = value > 6 ? Math.Ceiling(value) : Math.Floor(value);

		if (isFirstMarker)
		{
			double secondValue = (int)clockMarkerPointer2.Value == 0 ? 12 : (int)clockMarkerPointer2.Value;
			clockAnnotationLabel.Text = (value == 0 ? 12 : value) + " AM - " + secondValue + " AM";
		}
		else
		{
			double firstValue = (int)clockMarkerPointer1.Value == 0 ? 12 : (int)clockMarkerPointer1.Value;
			clockAnnotationLabel.Text = firstValue + " AM - " + (value == 0 ? 12 : value) + " AM";
		}
	}

	private void angleMarkerPointer1_ValueChanging(object sender, Syncfusion.Maui.Gauges.ValueChangingEventArgs e)
	{
		MarkerPointer markerPointer = sender as MarkerPointer;
		string text = AutomationProperties.GetHelpText(markerPointer);

		if (Math.Abs(e.NewValue - e.OldValue) > 20 ||
			(text == "angleAnnotationLabel1" && (e.NewValue >= angleMarker2.Value || Math.Abs(e.NewValue - angleMarker1.Value) > 10)) ||
			(text == "angleAnnotationLabel2" && (e.NewValue >= angleMarker4.Value || Math.Abs(e.NewValue - angleMarker3.Value) > 10)) ||
			(text == "angleAnnotationLabel3" && (e.NewValue >= angleMarker6.Value || Math.Abs(e.NewValue - angleMarker5.Value) > 10)) ||
			(text == "angleAnnotationLabel4" && (e.NewValue >= angleMarker8.Value || Math.Abs(e.NewValue - angleMarker7.Value) > 10)))
			e.Cancel = true;
		else
		{
			double value = e.NewValue > 50 ? Math.Ceiling(e.NewValue) : Math.Floor(e.NewValue);

			if (text == "angleAnnotationLabel1")
				angleAnnotationLabel1.Text = value.ToString() + "-" + (int)angleMarker2.Value;
			else if (text == "angleAnnotationLabel2")
				angleAnnotationLabel2.Text = value.ToString() + "-" + (int)angleMarker4.Value;
			else if (text == "angleAnnotationLabel3")
				angleAnnotationLabel3.Text = value.ToString() + "-" + (int)angleMarker6.Value;
			else if (text == "angleAnnotationLabel4")
				angleAnnotationLabel4.Text = value.ToString() + "-" + (int)angleMarker8.Value;
		}
	}

	private void angleMarkerPointer2_ValueChanging(object sender, Syncfusion.Maui.Gauges.ValueChangingEventArgs e)
	{
		MarkerPointer markerPointer = sender as MarkerPointer;
		string text = AutomationProperties.GetHelpText(markerPointer);

		if (Math.Abs(e.NewValue - e.OldValue) > 20 ||
			(text == "angleAnnotationLabel1" && (e.NewValue <= angleMarker1.Value || Math.Abs(e.NewValue - angleMarker2.Value) > 10)) ||
			(text == "angleAnnotationLabel2" && (e.NewValue <= angleMarker3.Value || Math.Abs(e.NewValue - angleMarker4.Value) > 10)) ||
			(text == "angleAnnotationLabel3" && (e.NewValue <= angleMarker5.Value || Math.Abs(e.NewValue - angleMarker6.Value) > 10)) ||
			(text == "angleAnnotationLabel4" && (e.NewValue <= angleMarker7.Value || Math.Abs(e.NewValue - angleMarker8.Value) > 10)))
			e.Cancel = true;
		else
		{
			double value = e.NewValue > 50 ? Math.Ceiling(e.NewValue) : Math.Floor(e.NewValue);

			if (text == "angleAnnotationLabel1")
				angleAnnotationLabel1.Text = (int)angleMarker1.Value + "-" + value.ToString();
			else if (text == "angleAnnotationLabel2")
				angleAnnotationLabel2.Text = (int)angleMarker3.Value + "-" + value.ToString();
			else if (text == "angleAnnotationLabel3")
				angleAnnotationLabel3.Text = (int)angleMarker5.Value + "-" + value.ToString();
			else if (text == "angleAnnotationLabel4")
				angleAnnotationLabel4.Text = (int)angleMarker7.Value + "-" + value.ToString();
		}
	}

	private void customTextMarker_ValueChanging1(object sender, ValueChangingEventArgs e)
	{
		
	}

	private void customTextMarker_ValueChanging2(object sender, ValueChangingEventArgs e)
	{
		
	}

	private void gradientMarker_ValueChanging(object sender, ValueChangingEventArgs e)
	{
		//double value = e.NewValue > 50 ? Math.Ceiling(e.NewValue) : Math.Floor(e.NewValue);
		//fahrenheitAnnotationLabel.Text = value.ToString() + "°F";
		//celsiusAnnotationLabel.Text = Math.Round((value - 32) / 1.8, 1).ToString() + "°C";
	}


	public event PropertyChangedEventHandler PropertyChanged;

	// This method is called by the Set accessor of each property.  
	// The CallerMemberName attribute that is applied to the optional propertyName  
	// parameter causes the property name of the caller to be substituted as an argument.  
	private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
	{
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}

}

