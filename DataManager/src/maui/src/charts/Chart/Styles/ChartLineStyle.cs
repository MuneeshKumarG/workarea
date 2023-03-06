using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;

namespace Syncfusion.Maui.Charts
{
    /// <summary>
    /// Represents the chart line style class that can be used to customize the axis line and grid lines.
    /// </summary>
    /// <remarks>
    /// <para>It provides more options to customize the chart lines.</para>
    /// 
    /// <para> <b>Stroke - </b> To customize the stroke color, refer to this <see cref="Stroke"/> property. </para>
    /// <para> <b>StrokeWidth - </b> To modify the stroke width, refer to this <see cref="StrokeWidth"/> property. </para>
    /// <para> <b>StrokeDashArray - </b> To customize the line with dashes and gaps, refer to this <see cref="StrokeDashArray"/> property. </para>
    /// 
    /// </remarks>
    public class ChartLineStyle : BindableObject
    {
        #region Bindable Properties

        /// <summary>
        /// Identifies the <see cref="Stroke"/> bindable property.
        /// </summary>        
        public static readonly BindableProperty StrokeProperty = BindableProperty.Create(nameof(Stroke), typeof(Brush), typeof(ChartLineStyle), new SolidColorBrush(Color.FromArgb("#D1D8DB")), BindingMode.Default, null, OnStrokeColorChanged);

        /// <summary>
        /// Identifies the <see cref="StrokeWidth"/> bindable property.
        /// </summary>        
        public static readonly BindableProperty StrokeWidthProperty = BindableProperty.Create(nameof(StrokeWidth), typeof(double), typeof(ChartLineStyle), 1d , BindingMode.Default, null, OnStrokeWidthChanged);

        /// <summary>
        /// Identifies the <see cref="StrokeDashArray"/> bindable property.
        /// </summary>        
        public static readonly BindableProperty StrokeDashArrayProperty = BindableProperty.Create(nameof(StrokeDashArray), typeof(DoubleCollection), typeof(ChartLineStyle), null, BindingMode.Default, null, OnStrokeDashArrayChanged);

        #endregion

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="ChartLineStyle"/>.
        /// </summary>
        public ChartLineStyle()
        {
        }
        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets a value to customize the stroke color of the chart lines.
        /// </summary>
        /// <value>It accepts <see cref="Brush"/> values.</value>
        public Brush Stroke
        {
            get { return (Brush)GetValue(StrokeProperty); }
            set { SetValue(StrokeProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value that indicates the width of the chart lines.
        /// </summary>
        /// <value>It accepts <see cref="double"/> values and the defaule value is 1.</value>
        public double StrokeWidth
        {
            get { return (double)GetValue(StrokeWidthProperty); }
            set { SetValue(StrokeWidthProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value to customize the appearance of the chart lines.
        /// </summary>
        /// <value>It accepts <see cref="DoubleCollection"/> values.</value>
        public DoubleCollection StrokeDashArray
        {
            get { return (DoubleCollection)GetValue(StrokeDashArrayProperty); }
            set { SetValue(StrokeDashArrayProperty, value); }
        }

        #endregion

        #region Methods

        internal bool CanDraw()
        {
            return StrokeWidth > 0 && !ChartColor.IsEmpty(Stroke.ToColor());
        }

        private static void OnStrokeWidthChanged(BindableObject bindable, object oldValue, object newValue)
        {
        }

        private static void OnStrokeColorChanged(BindableObject bindable, object oldValue, object newValue)
        {
        }

        private static void OnStrokeDashArrayChanged(BindableObject bindable, object oldValue, object newValue)
        {
        }

        #endregion
    }
}
