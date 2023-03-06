using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;

namespace Syncfusion.Maui.Charts
{
    /// <summary>
    /// Represents a LabelStyle class that can be used to customize the data labels.
    /// </summary>
    /// <remarks>
    /// It provides more options to customize the data label.
    /// <para> <b>TextColor - </b> To customize the text color, refer to this <see cref="ChartLabelStyle.TextColor"/> property. </para>
    /// <para> <b>Background - </b> To customize the background color, refer to this <see cref="ChartLabelStyle.Background"/> property. </para>
    /// <para> <b>Stroke - </b> To customize the stroke color, refer to this <see cref="ChartLabelStyle.Stroke"/> property. </para>
    /// <para> <b>StrokeWidth - </b> To modify the stroke width, refer to this <see cref="ChartLabelStyle.StrokeWidth"/> property. </para>
    /// <para> <b>Margin - </b> To adjust the outer margin for labels, refer to this <see cref="ChartLabelStyle.Margin"/> property. </para>
    /// <para> <b>LabelPadding - </b> To adjust the padding for labels, refer to this <see cref="LabelPadding"/> property. </para>
    /// <para> <b>Angle - </b> To adjust the angle rotaion for labels, refer to this <see cref="Angle"/> property. </para>
    /// </remarks>
    public class ChartDataLabelStyle : ChartLabelStyle
    {
        #region Bindable Properties

        /// <summary>
        /// Identifies the <see cref="LabelPadding"/> bindable property.
        /// </summary>        
        public static readonly BindableProperty LabelPaddingProperty = BindableProperty.Create(nameof(LabelPadding), typeof(double), typeof(ChartDataLabelStyle), 3d, BindingMode.Default, null, null);

        /// <summary>
        /// Identifies the <see cref="OffsetX"/> bindable property.
        /// </summary>        
        public static readonly BindableProperty OffsetXProperty = BindableProperty.Create(nameof(OffsetX), typeof(double), typeof(ChartDataLabelStyle), 0d, BindingMode.Default, null, null);

        /// <summary>
        /// Identifies the <see cref="OffsetY"/> bindable property.
        /// </summary>        
        public static readonly BindableProperty OffsetYProperty = BindableProperty.Create(nameof(OffsetY), typeof(double), typeof(ChartDataLabelStyle), 0d, BindingMode.Default, null, null);

        /// <summary>
        /// Identifies the <see cref="Angle"/> bindable property.
        /// </summary>        
        public static readonly BindableProperty AngleProperty = BindableProperty.Create(nameof(Angle), typeof(double), typeof(ChartDataLabelStyle), 0d, BindingMode.Default, null, null);

        #endregion

        #region Constructor 

        /// <summary>
        /// Initializes a new instance of the <see cref="ChartDataLabelStyle"/>.
        /// </summary>
        public ChartDataLabelStyle()
        {
            TextColor = Colors.Transparent;
            CornerRadius = new CornerRadius(5);
            FontSize = 12;
            Margin = new Thickness(5);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets a value that indicates the label's padding.
        /// </summary>
        /// <value>It accepts <see cref="double"/> values and the defaule value is 3.</value>
        public double LabelPadding
        {
            get { return (double)GetValue(LabelPaddingProperty); }
            set { SetValue(LabelPaddingProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value to adjust the data label position horizontally.
        /// </summary>
        /// <value>It accepts <see cref="double"/> values and the defaule value is 0.</value>
        public double OffsetX
        {
            get { return (double)GetValue(OffsetXProperty); }
            set { SetValue(OffsetXProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value to adjust the data label position vertically.
        /// </summary>
        /// <value>It accepts <see cref="double"/> values and the defaule value is 0.</value>
        public double OffsetY
        {
            get { return (double)GetValue(OffsetYProperty); }
            set { SetValue(OffsetYProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value that indicates the angle rotation of the data label.
        /// </summary>
        /// <value>It accepts <see cref="double"/> values and the defaule value is 0.</value>
        public double Angle
        {
            get { return (double)GetValue(AngleProperty); }
            set { SetValue(AngleProperty, value); }
        }

        #endregion

        #region Methods

        internal bool NeedDataLabelMeasure(string propertyName)
        {
            return propertyName.Equals(nameof(Angle)) || propertyName.Equals(nameof(OffsetX)) || propertyName.Equals(nameof(OffsetY)) || propertyName.Equals(nameof(LabelPadding)) || propertyName.Equals(nameof(Margin)) || propertyName.Equals(nameof(FontSize)) || propertyName.Equals(nameof(FontFamily)) || propertyName.Equals(nameof(FontAttributes)) || propertyName.Equals(nameof(LabelFormat)) || propertyName.Equals(nameof(StrokeWidth));
        }

        #endregion
    }
}