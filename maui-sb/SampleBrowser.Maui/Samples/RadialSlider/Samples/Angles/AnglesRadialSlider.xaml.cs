using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Layouts;
using SampleBrowser.Maui.Core;
using Syncfusion.Maui.Gauges;
using Syncfusion.Maui.Sliders;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SampleBrowser.Maui.RadialSlider
{
    public partial class AnglesRadialSlider : SampleView, INotifyPropertyChanged
    {
        public AnglesRadialSlider()
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
                angleSlidersLayout.Orientation = StackOrientation.Horizontal;
                GaugeWidth = widthConstraint / 4;
                GaugeHeight=heightConstraint;
            }
            else
            {
                angleSlidersLayout.Orientation = StackOrientation.Vertical;
                GaugeWidth = widthConstraint;
                GaugeHeight = heightConstraint / 4;
            }
            return base.MeasureOverride(widthConstraint, heightConstraint);
        }

        private void angleMarkerPointer_ValueChanging(object sender, ValueChangingEventArgs e)
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