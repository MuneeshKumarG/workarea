using Syncfusion.Maui.Gauges;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SBSample;

public partial class Page1 : ContentPage, INotifyPropertyChanged
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

	public Page1()
	{
		InitializeComponent();
		BindingContext = this;

		fahrenheitAnnotationLabel.Text = "60°F";
		celsiusAnnotationLabel.Text = "15.6°C";
	}

	private void markerPointer_ValueChanging(object sender, ValueChangingEventArgs e)
	{
		if (Math.Abs(e.NewValue - e.OldValue) > 20)
			e.Cancel = true;
		else
		{
			MarkerPointer markerPointer = sender as MarkerPointer;
			string text = AutomationProperties.GetHelpText(markerPointer);

			double value = e.NewValue;
			value = value > 50 ? Math.Ceiling(value) : Math.Floor(value);

			if (text == "annotationLabel")
				annotationLabel.Text = value.ToString() + "%";
		}
	}

	private void labelsAndTicksPointer_ValueChanging(object sender, ValueChangingEventArgs e)
	{
		if (Math.Abs(e.NewValue - e.OldValue) > 2.4)
			e.Cancel = true;
		else
		{
			double value = e.NewValue;
			value = value > 6 ? Math.Ceiling(value) : Math.Floor(value);
			clockAnnotationLabel.Text = value.ToString() +  " hrs";
		}
	}

	private void angleMarkerPointer_ValueChanging(object sender, Syncfusion.Maui.Gauges.ValueChangingEventArgs e)
	{
		if (Math.Abs(e.NewValue - e.OldValue) > 20)
			e.Cancel = true;
		else
		{
			MarkerPointer markerPointer = sender as MarkerPointer;
			string text = AutomationProperties.GetHelpText(markerPointer);

			double value = e.NewValue > 50 ? Math.Ceiling(e.NewValue) : Math.Floor(e.NewValue);

			if (text == "angleAnnotationLabel1")
				angleAnnotationLabel1.Text = value.ToString() + "%";
			else if (text == "angleAnnotationLabel2")
				angleAnnotationLabel2.Text = value.ToString() + "%";
			else if (text == "angleAnnotationLabel3")
				angleAnnotationLabel3.Text = value.ToString() + "%";
			else if (text == "angleAnnotationLabel4")
				angleAnnotationLabel4.Text = value.ToString() + "%";
		}
	}

	private void styleMarker_ValueChanging(object sender, Syncfusion.Maui.Gauges.ValueChangingEventArgs e)
	{
		if (Math.Abs(e.NewValue - e.OldValue) > 20)
			e.Cancel = true;
		else
		{
			string text=string.Empty;
			if (sender is MarkerPointer markerPointer)
			text = AutomationProperties.GetHelpText(markerPointer);
			if (sender is NeedlePointer needlePointer)
				text = AutomationProperties.GetHelpText(needlePointer);

			double value = e.NewValue > 50 ? Math.Ceiling(e.NewValue) : Math.Floor(e.NewValue);

			if (text == "styleAnnotationLabel1")
				styleAnnotationLabel1.Text = value.ToString() + "%";
			else if (text == "styleAnnotationLabel2")
				styleAnnotationLabel2.Text = value.ToString() + "%";
			else if (text == "styleAnnotationLabel3")
				styleAnnotationLabel3.Text = value.ToString() + "%";
			else if (text == "styleAnnotationLabel4")
				styleAnnotationLabel4.Text = value.ToString() + "%";
			else if (text == "styleAnnotationLabel5")
				styleAnnotationLabel5.Text = value.ToString() + "%";
			else if (text == "styleAnnotationLabel6")
			{
				styleAnnotationLabel6.Text = value.ToString();
				styleRangePointer6.Value = value == 100 ? 100 : value == 0 ? 0.1 : e.NewValue;
			}
		}
	}

	private void thumpPointer_ValueChanging(object sender, ValueChangingEventArgs e)
	{
		if (Math.Abs(e.NewValue - e.OldValue) > 20)
			e.Cancel = true;
		else
		{
			MarkerPointer markerPointer = sender as MarkerPointer;
			string text = AutomationProperties.GetHelpText(markerPointer);

			double value = e.NewValue;
			value = value > 50 ? Math.Ceiling(value) : Math.Floor(value);

			if (text == "thumpAnnotationLabel1")
				thumpAnnotationLabel1.Text = value.ToString() + "%";
			else if (text == "thumpAnnotationLabel2")
				thumpAnnotationLabel2.Text = value.ToString() + "%";
			else if (text == "thumpAnnotationLabel3")
				thumpAnnotationLabel3.Text = value.ToString() + "%";
		}
	}

	private void customTextMarker_ValueChanged(object sender, Syncfusion.Maui.Gauges.ValueChangedEventArgs e)
	{
		if (e.Value > 99)
		{
			Brush brush = new SolidColorBrush(Color.FromRgb(0, 168, 181));
			(customTextAxis.Pointers[0] as RangePointer).Fill = brush;
			(customTextAxis.Pointers[1] as MarkerPointer).Stroke = Color.FromRgb(0, 168, 181);
			customTextAnnotation.Text = "Done";

		}
		else
		{
			Brush brush = new SolidColorBrush(Colors.Orange);
			(customTextAxis.Pointers[0] as RangePointer).Fill = brush;
			(customTextAxis.Pointers[1] as MarkerPointer).Stroke = Colors.Orange;
			customTextAnnotation.Text = "In-progress";
		}
	}

	private void gradientMarker_ValueChanging(object sender, ValueChangingEventArgs e)
	{
		double value = e.NewValue > 50 ? Math.Ceiling(e.NewValue) : Math.Floor(e.NewValue);
		fahrenheitAnnotationLabel.Text = value.ToString() + "°F";
		celsiusAnnotationLabel.Text = Math.Round((value - 32) / 1.8, 1).ToString() + "°C";
	}

	private void RadialAxis_LabelCreated(object sender, LabelCreatedEventArgs e)
	{
		double axisValue = double.Parse(e.Text);
		double celsiusValue = (axisValue - 32) / 1.8;
		e.Text = Math.Round(celsiusValue, 1).ToString();
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

