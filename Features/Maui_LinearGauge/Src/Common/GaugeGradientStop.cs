using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;

namespace Syncfusion.Maui.Gauges
{
    /// <summary>
    ///  Describes the location and color of a transition point in a gradient.
    /// </summary>
    public class GaugeGradientStop : BindableObject
    {
        #region Dependency registrations

        /// <summary>
        /// Identifies the <see cref="Color"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="Color"/> bindable property.
        /// </value>
        public static readonly BindableProperty ColorProperty =
        BindableProperty.Create(nameof(Color), typeof(Color), typeof(GaugeGradientStop), null);

        /// <summary>
        /// Identifies the <see cref="Value"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="Value"/> bindable property.
        /// </value>
        public static readonly BindableProperty ValueProperty =
            BindableProperty.Create(nameof(Value), typeof(double), typeof(GaugeGradientStop), 0d, propertyChanged : OnValuePropertyChanged);

        #endregion

        #region Fields

        /// <summary>
        /// Backing field to store the actual value of gradient stop.
        /// </summary>
        private double actualValue;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="GaugeGradientStop"/> class.
        /// </summary>
        public GaugeGradientStop()
        {
            this.Color = Colors.Transparent;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the color that describes the gradient color value.
        /// </summary>
        public Color Color
        {
            get { return (Color)this.GetValue(ColorProperty); }
            set { this.SetValue(ColorProperty, value); }
        }

        /// <summary>
        /// Gets or sets the value that describes the gradient value.
        /// </summary>
        public double Value
        {
            get { return (double)this.GetValue(ValueProperty); }
            set { this.SetValue(ValueProperty, value); }
        }

        /// <summary>
        /// Gets or sets the value for actual value of gradient stop
        /// </summary>
        internal double ActualValue
        {
            get
            {
                return this.actualValue;
            }

            set
            {
                if (this.actualValue != value)
                {
                    this.actualValue = value;
                }
            }
        }

        #endregion

        #region Property changed

        /// <summary>
        /// Called when the value of gauge gradient stop is changed
        /// </summary>
        /// <param name="bindable">The bindable object</param>
        /// <param name="oldValue">Represents old value</param>
        /// <param name="newValue">Represents new value</param>
        private static void OnValuePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is GaugeGradientStop gradientStop)
            {
                gradientStop.ActualValue = gradientStop.Value;
            }
        }

        #endregion
    }
}
