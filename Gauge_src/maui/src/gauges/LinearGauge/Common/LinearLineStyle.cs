using Microsoft.Maui;
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
           BindableProperty.Create(nameof(Fill), typeof(Brush), typeof(LinearLineStyle), null, defaultValueCreator: bindable => new SolidColorBrush(Color.FromRgba(33, 33, 33, 20)));
        
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
            BindableProperty.Create(nameof(Thickness), typeof(double), typeof(LinearLineStyle), 10d);

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

        #region Constructor

        
        public LinearLineStyle()
        {
            GradientStops = new ObservableCollection<GaugeGradientStop>();
        }

        #endregion

        #region Properties

        
        public Brush Fill
        {
            get { return (Brush)this.GetValue(FillProperty); }
            set { this.SetValue(FillProperty, value); }
        }

        public ObservableCollection<GaugeGradientStop> GradientStops
        {
            get { return (ObservableCollection<GaugeGradientStop>)this.GetValue(GradientStopsProperty); }
            set { this.SetValue(GradientStopsProperty, value); }
        }

        public Thickness CornerRadius
        {
            get { return (Thickness)this.GetValue(CornerRadiusProperty); }
            set { this.SetValue(CornerRadiusProperty, value); }
        }

        public double Thickness
        {
            get { return (double)this.GetValue(ThicknessProperty); }
            set { this.SetValue(ThicknessProperty, value); }
        }

        public CornerStyle CornerStyle
        {
            get { return (CornerStyle)this.GetValue(CornerStyleProperty); }
            set { this.SetValue(CornerStyleProperty, value); }
        }

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
