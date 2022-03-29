using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;

namespace Syncfusion.Maui.Charts
{
    /// <summary>
    /// 
    /// </summary>
    public class ChartDataLabelStyle : ChartLabelStyle
    {
        #region Bindable Properties

        /// <summary>
        /// 
        /// </summary>        
        public static readonly BindableProperty LabelPaddingProperty = BindableProperty.Create(nameof(LabelPadding), typeof(double), typeof(ChartDataLabelStyle), 3d, BindingMode.Default, null, null);

        /// <summary>
        /// 
        /// </summary>        
        public static readonly BindableProperty OffsetXProperty = BindableProperty.Create(nameof(OffsetX), typeof(double), typeof(ChartDataLabelStyle), 0d, BindingMode.Default, null, null);

        /// <summary>
        /// 
        /// </summary>        
        public static readonly BindableProperty OffsetYProperty = BindableProperty.Create(nameof(OffsetY), typeof(double), typeof(ChartDataLabelStyle), 0d, BindingMode.Default, null, null);

        /// <summary>
        /// 
        /// </summary>        
        public static readonly BindableProperty AngleProperty = BindableProperty.Create(nameof(Angle), typeof(double), typeof(ChartDataLabelStyle), 0d, BindingMode.Default, null, null);

        #endregion

        #region Constructor 

        /// <summary>
        /// 
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
        /// 
        /// </summary>
        public double LabelPadding
        {
            get { return (double)GetValue(LabelPaddingProperty); }
            set { SetValue(LabelPaddingProperty, value); }
        }

        /// <summary>
        /// 
        /// </summary>
        public double OffsetX
        {
            get { return (double)GetValue(OffsetXProperty); }
            set { SetValue(OffsetXProperty, value); }
        }

        /// <summary>
        /// 
        /// </summary>
        public double OffsetY
        {
            get { return (double)GetValue(OffsetYProperty); }
            set { SetValue(OffsetYProperty, value); }
        }

        /// <summary>
        /// 
        /// </summary>
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

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        internal ChartDataLabelStyle Clone()
        {
            var style = new ChartDataLabelStyle();

            //Only returned values which help full to render chart label. 
            //TODO: Need to add all values when it use for other cases. 
            style.FontFamily = FontFamily;
            style.FontAttributes = FontAttributes;
            style.FontSize = FontSize;
            style.Rect = Rect;

            return style;
        }

        #endregion
    }
}