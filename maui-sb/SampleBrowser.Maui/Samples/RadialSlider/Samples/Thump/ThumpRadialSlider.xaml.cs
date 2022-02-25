using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using Syncfusion.Maui.Sliders;
using SampleBrowser.Maui.Core;
using Syncfusion.Maui.Gauges;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SampleBrowser.Maui.RadialSlider
{
    public partial class ThumpRadialSlider : SampleView, INotifyPropertyChanged
	{
        public ThumpRadialSlider()
        {
            InitializeComponent();
            BindingContext = this;
        }

        private double gaugeHeight;

        public double GaugeHeight
        {
            get { return gaugeHeight; }
            set { gaugeHeight = value; NotifyPropertyChanged(); }
        }

        private double gaugeWidth;

        public double GaugeWidth
        {
            get { return gaugeWidth; }
            set { gaugeWidth = value; NotifyPropertyChanged(); }
        }

        protected override Size MeasureOverride(double widthConstraint, double heightConstraint)
        {
            if (widthConstraint > heightConstraint)
            {
                rootLayout.Orientation = StackOrientation.Horizontal;
                GaugeWidth = widthConstraint / 3.5;
                GaugeHeight = heightConstraint;
            }
            else
            {
                rootLayout.Orientation = StackOrientation.Vertical;
                GaugeWidth = widthConstraint;
                GaugeHeight = heightConstraint / 3.5;
            }
            return base.MeasureOverride(widthConstraint, heightConstraint);
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

		public new event PropertyChangedEventHandler PropertyChanged;

		// This method is called by the Set accessor of each property.  
		// The CallerMemberName attribute that is applied to the optional propertyName  
		// parameter causes the property name of the caller to be substituted as an argument.  
		private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}