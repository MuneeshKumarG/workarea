﻿using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using System.Collections.ObjectModel;

namespace Syncfusion.Maui.Gauges
{
    /// <summary>
    /// Represents the style of axis line, that used to customize axis line color, corner style, thickness and dash array.
    /// </summary>
    public class LinearLineStyle : BindableObject
    {
        #region Bindable properties

        /// <summary>
        /// Identifies the <see cref="Fill"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="Fill"/> bindable property.
        /// </value>
        public static readonly BindableProperty FillProperty =
           BindableProperty.Create(nameof(Fill), typeof(Brush), typeof(LinearLineStyle), null, defaultValueCreator: bindable => new SolidColorBrush(Color.FromRgba(0, 0, 0, 31)));
        
        /// <summary>
        /// Identifies the <see cref="GradientStops"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="GradientStops"/> bindable property.
        /// </value>
        public static readonly BindableProperty GradientStopsProperty =
          BindableProperty.Create(nameof(GradientStops), typeof(ObservableCollection<GaugeGradientStop>), typeof(LinearLineStyle), null);

        /// <summary>
        /// Identifies the <see cref="Thickness"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="Thickness"/> bindable property.
        /// </value>
        public static readonly BindableProperty ThicknessProperty =
            BindableProperty.Create(nameof(Thickness), typeof(double), typeof(LinearLineStyle), 5d);

        /// <summary>
        /// Identifies the <see cref="CornerRadius"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="CornerRadius"/> bindable property.
        /// </value>
        public static readonly BindableProperty CornerRadiusProperty =
            BindableProperty.Create(nameof(CornerRadius), typeof(Thickness), typeof(LinearLineStyle), null);

        /// <summary>
        /// Identifies the <see cref="CornerStyle"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="CornerStyle"/> bindable property.
        /// </value>
        public static readonly BindableProperty CornerStyleProperty =
            BindableProperty.Create(nameof(CornerStyle), typeof(CornerStyle), typeof(LinearLineStyle), CornerStyle.BothFlat);

        /// <summary>
        /// Identifies the <see cref="DashArray"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="DashArray"/> bindable property.
        /// </value>
        public static readonly BindableProperty DashArrayProperty =
            BindableProperty.Create(nameof(DashArray), typeof(DoubleCollection), typeof(LinearLineStyle), null);

        #endregion

        #region Fields

        internal LinearGradientBrush? LinearGradientBrush;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="LinearLineStyle"/> class.
        /// </summary>
        public LinearLineStyle()
        {
            GradientStops = new ObservableCollection<GaugeGradientStop>();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets color of axis line. 
        /// </summary>
        public Brush Fill
        {
            get { return (Brush)this.GetValue(FillProperty); }
            set { this.SetValue(FillProperty, value); }
        }

        /// <summary>
        /// Gets or sets <see cref="GaugeGradientStop"/> collection, that used to fill axis line in with gradient effect. 
        /// </summary>
        public ObservableCollection<GaugeGradientStop> GradientStops
        {
            get { return (ObservableCollection<GaugeGradientStop>)this.GetValue(GradientStopsProperty); }
            set { this.SetValue(GradientStopsProperty, value); }
        }

        /// <summary>
        /// Gets or sets corner radius value of axis line. 
        /// </summary>
        public Thickness CornerRadius
        {
            get { return (Thickness)this.GetValue(CornerRadiusProperty); }
            set { this.SetValue(CornerRadiusProperty, value); }
        }

        /// <summary>
        /// Gets or sets thickness of axis line. 
        /// </summary>
        public double Thickness
        {
            get { return (double)this.GetValue(ThicknessProperty); }
            set { this.SetValue(ThicknessProperty, value); }
        }

        /// <summary>
        /// Gets or sets corner style of axis line.
        /// </summary>
        public CornerStyle CornerStyle
        {
            get { return (CornerStyle)this.GetValue(CornerStyleProperty); }
            set { this.SetValue(CornerStyleProperty, value); }
        }

        /// <summary>
        /// Gets or sets double collection, that represents dash array of axis line. 
        /// </summary>
        public DoubleCollection DashArray
        {
            get { return (DoubleCollection)this.GetValue(DashArrayProperty); }
            set { this.SetValue(DashArrayProperty, value); }
        }

        #endregion

        #region Override methods

        /// <summary>
        /// Invoked whenever the binding context of the View changes.
        /// </summary>
        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();

            if (this.GradientStops != null)
            {
                foreach (var gradientStop in this.GradientStops)
                    SetInheritedBindingContext(gradientStop, BindingContext);
            }
        }

        #endregion
    }
}
