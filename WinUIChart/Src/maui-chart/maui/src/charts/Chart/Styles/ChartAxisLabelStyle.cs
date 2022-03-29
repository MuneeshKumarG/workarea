using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using System.Collections.Generic;

namespace Syncfusion.Maui.Charts
{
    /// <summary>
    /// 
    /// </summary>
    public class ChartAxisLabelStyle : ChartLabelStyle
    {
        #region BindableProperties
        /// <summary>
        /// 
        /// </summary>        
        internal static readonly BindableProperty LabelsPositionProperty = BindableProperty.Create(nameof(LabelsPosition), typeof(AxisElementPosition), typeof(ChartAxisLabelStyle), AxisElementPosition.Outside, BindingMode.Default, null, OnLabelsPositionChanged);

        /// <summary>
        /// 
        /// </summary>        
        public static readonly BindableProperty LabelAlignmentProperty = BindableProperty.Create(nameof(LabelAlignment), typeof(ChartAxisLabelAlignment), typeof(ChartAxisLabelStyle), ChartAxisLabelAlignment.Center, BindingMode.Default, null, OnAxisAlignmentChanged);

        /// <summary>
        /// Identifies the <see cref="MaxWidth"/> bindable property.
        /// </summary>        
        /// <value>
        /// The identifier for <see cref="MaxWidth"/> bindable property.
        /// </value>
        internal static readonly BindableProperty MaxWidthProperty = BindableProperty.Create(nameof(MaxWidth), typeof(double), typeof(ChartAxisLabelStyle), double.NaN, BindingMode.Default, null, OnMaxWidthChanged);

        /// <summary>
        /// Identifies the <see cref="WrappedLabelAlignment"/> bindable property.
        /// </summary>        
        /// <value>
        /// The identifier for <see cref="WrappedLabelAlignment"/> bindable property.
        /// </value>
        internal static readonly BindableProperty WrappedLabelAlignmentProperty = BindableProperty.Create(nameof(WrappedLabelAlignment), typeof(ChartAxisLabelAlignment), typeof(ChartAxisLabelStyle), ChartAxisLabelAlignment.Start, BindingMode.Default, null, OnWrappedLabelAlignmentChanged);

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the position of the axis labels. 
        /// </summary>
        /// <value>This property takes the <see cref="AxisElementPosition"/> as its value.</value>
        internal AxisElementPosition LabelsPosition
        {
            get { return (AxisElementPosition)GetValue(LabelsPositionProperty); }
            set { SetValue(LabelsPositionProperty, value); }
        }

        /// <summary>
        /// 
        /// </summary>
        public ChartAxisLabelAlignment LabelAlignment
        {
            get { return (ChartAxisLabelAlignment)GetValue(LabelAlignmentProperty); }
            set { SetValue(LabelAlignmentProperty, value); }
        }

        /// <summary>
        /// Gets or sets the maximum value to wrap the axis label.
        /// </summary>
        internal double MaxWidth
        {
            get { return (double)GetValue(MaxWidthProperty); }
            set { SetValue(MaxWidthProperty, value); }
        }

        /// <summary>
        /// Gets or sets the value to place the label at different position.
        /// </summary>
        internal ChartAxisLabelAlignment WrappedLabelAlignment
        {
            get { return (ChartAxisLabelAlignment)GetValue(WrappedLabelAlignmentProperty); }
            set { SetValue(WrappedLabelAlignmentProperty, value); }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// 
        /// </summary>
        public ChartAxisLabelStyle()
        {
            Background = new SolidColorBrush(Colors.Transparent);
            FontSize = 12;
            TextColor = Color.FromArgb("CC000000");
            Margin = new Thickness(4f);
        }

        #endregion

        #region Internal Properties

        internal ChartTextWrapMode TextWrapMode { get; set; } = ChartTextWrapMode.WordWrap;

        internal Dictionary<string, double>? WrapWidthCollection { get; set; }

        internal AxisLabelsIntersectAction LabelsIntersectAction { get; set; } = AxisLabelsIntersectAction.Hide;
        
        #endregion

        #region Methods

        private static void OnLabelsPositionChanged(BindableObject bindable, object oldValue, object newValue)
        {
        }

        private static void OnAxisAlignmentChanged(BindableObject bindable, object oldValue, object newValue)
        {
        }

        private static void OnMaxWidthChanged(BindableObject bindable, object oldValue, object newValue)
        {
        }

        private static void OnWrappedLabelAlignmentChanged(BindableObject bindable, object oldValue, object newValue)
        {
        }
        #endregion
    }
}
