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

		fahrenheitAnnotationLabel.Text = "0°F to 60°F";
		celsiusAnnotationLabel.Text = "-17.8°C to 15.6°C";
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

	private void styleMarker_ValueChanging1(object sender, Syncfusion.Maui.Gauges.ValueChangingEventArgs e)
	{
		string text = string.Empty;
		if (sender is MarkerPointer markerPointer)
			text = AutomationProperties.GetHelpText(markerPointer);
		if (sender is NeedlePointer needlePointer)
			text = AutomationProperties.GetHelpText(needlePointer);

		if (Math.Abs(e.NewValue - e.OldValue) > 20 ||
			(text == "styleAnnotationLabel1" && (e.NewValue >= styleMarker2.Value || Math.Abs(e.NewValue - styleMarker1.Value) > 10)) ||
			(text == "styleAnnotationLabel2" && (e.NewValue >= styleMarker4.Value || Math.Abs(e.NewValue - styleMarker3.Value) > 10)) ||
			(text == "styleAnnotationLabel3" && (e.NewValue >= styleMarker6.Value || Math.Abs(e.NewValue - styleMarker5.Value) > 10)) ||
			(text == "styleAnnotationLabel4" && (e.NewValue >= styleMarker8.Value || Math.Abs(e.NewValue - styleMarker7.Value) > 10)) ||
			(text == "styleAnnotationLabel5" && (e.NewValue >= styleMarker10.Value || Math.Abs(e.NewValue - styleMarker9.Value) > 10)) ||
			(text == "styleAnnotationLabel6" && (e.NewValue >= styleMarker12.Value || Math.Abs(e.NewValue - styleMarker11.Value) > 10)))
			e.Cancel = true;
		else
		{
			double value = e.NewValue > 50 ? Math.Ceiling(e.NewValue) : Math.Floor(e.NewValue);

			if (text == "styleAnnotationLabel1")
				styleAnnotationLabel1.Text = value.ToString() + " - " + (int)styleMarker2.Value + "%";
			else if (text == "styleAnnotationLabel2")
				styleAnnotationLabel2.Text = value.ToString() + " - " + (int)styleMarker4.Value + "%";
			else if (text == "styleAnnotationLabel3")
				styleAnnotationLabel3.Text = value.ToString() + " - " + (int)styleMarker6.Value + "%";
			else if (text == "styleAnnotationLabel4")
				styleAnnotationLabel4.Text = value.ToString() + " - " + (int)styleMarker8.Value + "%";
			else if (text == "styleAnnotationLabel5")
				styleAnnotationLabel5.Text = value.ToString() + " - " + (int)styleMarker10.Value + "%";
			else if (text == "styleAnnotationLabel6")
				styleAnnotationLabel6.Text = value.ToString() + " - " + (int)styleMarker12.Value;
		}
	}

	private void styleMarker_ValueChanging2(object sender, Syncfusion.Maui.Gauges.ValueChangingEventArgs e)
	{
		string text = string.Empty;
		if (sender is MarkerPointer markerPointer)
			text = AutomationProperties.GetHelpText(markerPointer);
		if (sender is NeedlePointer needlePointer)
			text = AutomationProperties.GetHelpText(needlePointer);

		if (Math.Abs(e.NewValue - e.OldValue) > 20 ||
			(text == "styleAnnotationLabel1" && (e.NewValue <= styleMarker1.Value || Math.Abs(e.NewValue - styleMarker2.Value) > 10)) ||
			(text == "styleAnnotationLabel2" && (e.NewValue <= styleMarker3.Value || Math.Abs(e.NewValue - styleMarker4.Value) > 10)) ||
			(text == "styleAnnotationLabel3" && (e.NewValue <= styleMarker5.Value || Math.Abs(e.NewValue - styleMarker6.Value) > 10)) ||
			(text == "styleAnnotationLabel4" && (e.NewValue <= styleMarker7.Value || Math.Abs(e.NewValue - styleMarker8.Value) > 10)) ||
			(text == "styleAnnotationLabel5" && (e.NewValue <= styleMarker9.Value || Math.Abs(e.NewValue - styleMarker10.Value) > 10)) ||
			(text == "styleAnnotationLabel6" && (e.NewValue <= styleMarker11.Value || Math.Abs(e.NewValue - styleMarker12.Value) > 10)))
			e.Cancel = true;
		else
		{
			double value = e.NewValue > 50 ? Math.Ceiling(e.NewValue) : Math.Floor(e.NewValue);

			if (text == "styleAnnotationLabel1")
				styleAnnotationLabel1.Text = (int)styleMarker1.Value + " - " + value.ToString() + "%";
			else if (text == "styleAnnotationLabel2")
				styleAnnotationLabel2.Text = (int)styleMarker3.Value + " - " + value.ToString() + "%";
			else if (text == "styleAnnotationLabel3")
				styleAnnotationLabel3.Text = (int)styleMarker5.Value + " - " + value.ToString() + "%";
			else if (text == "styleAnnotationLabel4")
				styleAnnotationLabel4.Text = (int)styleMarker7.Value + " - " + value.ToString() + "%";
			else if (text == "styleAnnotationLabel5")
				styleAnnotationLabel5.Text = (int)styleMarker9.Value + " - " + value.ToString() + "%";
			else if (text == "styleAnnotationLabel6")
				styleAnnotationLabel6.Text = (int)styleMarker11.Value + " - " + value.ToString();
		}
	}

	private void thumpPointer1_ValueChanging(object sender, ValueChangingEventArgs e)
	{
		MarkerPointer markerPointer = sender as MarkerPointer;
		string text = AutomationProperties.GetHelpText(markerPointer);

		if (Math.Abs(e.NewValue - e.OldValue) > 20 ||
			(text == "thumpAnnotationLabel1" && (e.NewValue >= thumpMarkerPointer2.Value || Math.Abs(e.NewValue - thumpMarkerPointer1.Value) > 10)) ||
			(text == "thumpAnnotationLabel2" && (e.NewValue >= thumpMarkerPointer4.Value || Math.Abs(e.NewValue - thumpMarkerPointer3.Value) > 10)) ||
			(text == "thumpAnnotationLabel3" && (e.NewValue >= thumpMarkerPointer6.Value || Math.Abs(e.NewValue - thumpMarkerPointer5.Value) > 10)))
			e.Cancel = true;
		else
		{
			double value = e.NewValue;
			value = value > 50 ? Math.Ceiling(value) : Math.Floor(value);

			if (text == "thumpAnnotationLabel1")
				thumpAnnotationLabel1.Text = value.ToString() + " - " + (int)thumpMarkerPointer2.Value;
			else if (text == "thumpAnnotationLabel2")
				thumpAnnotationLabel2.Text = value.ToString() + " - " + (int)thumpMarkerPointer4.Value;
			else if (text == "thumpAnnotationLabel3")
				thumpAnnotationLabel3.Text = value.ToString() + " - " + (int)thumpMarkerPointer6.Value;
		}
	}

	private void thumpPointer2_ValueChanging(object sender, ValueChangingEventArgs e)
	{
		MarkerPointer markerPointer = sender as MarkerPointer;
		string text = AutomationProperties.GetHelpText(markerPointer);

		if (Math.Abs(e.NewValue - e.OldValue) > 20 ||
			(text == "thumpAnnotationLabel1" && (e.NewValue <= thumpMarkerPointer1.Value || Math.Abs(e.NewValue - thumpMarkerPointer2.Value) > 10)) ||
			(text == "thumpAnnotationLabel2" && (e.NewValue <= thumpMarkerPointer3.Value || Math.Abs(e.NewValue - thumpMarkerPointer4.Value) > 10)) ||
			(text == "thumpAnnotationLabel3" && (e.NewValue <= thumpMarkerPointer5.Value || Math.Abs(e.NewValue - thumpMarkerPointer6.Value) > 10)))
			e.Cancel = true;
		else
		{
			double value = e.NewValue;
			value = value > 50 ? Math.Ceiling(value) : Math.Floor(value);

			if (text == "thumpAnnotationLabel1")
				thumpAnnotationLabel1.Text = (int)thumpMarkerPointer1.Value + " - " + value.ToString();
			else if (text == "thumpAnnotationLabel2")
				thumpAnnotationLabel2.Text = (int)thumpMarkerPointer3.Value + " - " + value.ToString();
			else if (text == "thumpAnnotationLabel3")
				thumpAnnotationLabel3.Text = (int)thumpMarkerPointer5.Value + " - " + value.ToString();
		}
	}

	private void customTextMarker_ValueChanging1(object sender, ValueChangingEventArgs e)
	{
		if (e.NewValue >= customTextMarker2.Value || Math.Abs(e.NewValue - customTextMarker1.Value) > 10)
			e.Cancel = true;
		else
			UpdateCustomTextAnnotationLabel(Math.Abs(customTextMarker2.Value - customTextMarker1.Value));
	}

	private void customTextMarker_ValueChanging2(object sender, ValueChangingEventArgs e)
	{
		if (e.NewValue <= customTextMarker1.Value || Math.Abs(e.NewValue - customTextMarker2.Value) > 10)
			e.Cancel = true;
		else
			UpdateCustomTextAnnotationLabel(Math.Abs(customTextMarker2.Value - customTextMarker1.Value));
	}

	private void UpdateCustomTextAnnotationLabel(double value)
    {
		if (value > 99)
		{
			Brush brush = new SolidColorBrush(Color.FromRgb(0, 168, 181));
			customTextAxis.Ranges[0].Fill = brush;
			(customTextAxis.Pointers[0] as MarkerPointer).Stroke = Color.FromRgb(0, 168, 181);
			(customTextAxis.Pointers[1] as MarkerPointer).Stroke = Color.FromRgb(0, 168, 181);
			customTextAnnotation.Text = "Done";

		}
		else
		{
			Brush brush = new SolidColorBrush(Colors.Orange);
			customTextAxis.Ranges[0].Fill = brush;
			(customTextAxis.Pointers[0] as MarkerPointer).Stroke = Colors.Orange;
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

	private void gradientMarker1_ValueChanging(object sender, ValueChangingEventArgs e)
	{
		if (e.NewValue >= gradientMarker2.Value || Math.Abs(e.NewValue - gradientMarker1.Value) > 10)
			e.Cancel = true;
		else
		{
			double firstMarkerValue = e.NewValue > 50 ? Math.Ceiling(e.NewValue) : Math.Floor(e.NewValue);
			double secondMarkerValue = (int)gradientMarker2.Value;

			UpdateGradientAnnotationLabel(firstMarkerValue, secondMarkerValue);
		}
	}

	private void gradientMarker2_ValueChanging(object sender, ValueChangingEventArgs e)
	{
		if (e.NewValue <= gradientMarker1.Value || Math.Abs(e.NewValue - gradientMarker2.Value) > 10)
			e.Cancel = true;
		else
		{
			double firstMarkerValue = (int)gradientMarker1.Value;
			double secondMarkerValue = e.NewValue > 50 ? Math.Ceiling(e.NewValue) : Math.Floor(e.NewValue);

			UpdateGradientAnnotationLabel(firstMarkerValue, secondMarkerValue);
		}
    }

	private void UpdateGradientAnnotationLabel(double firstMarkerValue, double secondMarkerValue)
	{
		fahrenheitAnnotationLabel.Text = firstMarkerValue.ToString() + "°F to " + secondMarkerValue + "°F";
		celsiusAnnotationLabel.Text = Math.Round((firstMarkerValue - 32) / 1.8, 1) + "°C to " +
			Math.Round((secondMarkerValue - 32) / 1.8, 1) + "°C";
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

