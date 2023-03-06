using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using System.Collections.Generic;

namespace Syncfusion.Maui.Charts
{
    /// <summary>
    /// Represents the chart axis's label style class.
    /// </summary>
    /// <remarks>
    /// To customize the axis labels appearance, create an instance of the <see cref="ChartAxisLabelStyle"/> class and set to the <see cref="ChartAxis.LabelStyle"/> property.
    /// 
    /// # [MainPage.xaml](#tab/tabid-1)
    /// <code><![CDATA[
    /// <chart:SfCartesianChart>
    ///  
    ///     <chart:SfCartesianChart.XAxes>
    ///         <chart:CategoryAxis>
    ///            <chart:CategoryAxis.LabelStyle>
    ///                <chart:ChartAxisLabelStyle TextColor = "Red" FontSize="14"/>
    ///            </chart:CategoryAxis.LabelStyle>
    ///        </chart:CategoryAxis>
    ///     </chart:SfCartesianChart.XAxes>
    /// 
    /// </chart:SfCartesianChart>
    /// ]]>
    /// </code>
    /// # [MainPage.xaml.cs](#tab/tabid-2)
    /// <code><![CDATA[
    /// SfCartesianChart chart = new SfCartesianChart();
    /// 
    /// CategoryAxis xaxis = new CategoryAxis();
    /// xaxis.LabelStyle = new ChartAxisLabelStyle()
    /// {
    ///     TextColor = Colors.Red,
    ///     FontSize = 14,
    /// };
    /// chart.XAxes.Add(xaxis);
    ///
    /// ]]>
    /// </code>
    /// *** 
    /// <para>It provides more options to customize the chart axis label.</para>
    /// 
    /// <para> <b>LabelAlignment - </b> To position the axis label, refer to this <see cref="LabelAlignment"/> property.</para>
    /// <para> <b>LabelFormat - </b> To customize the numeric or date-time format of the axis label, refer to this <see cref="ChartLabelStyle.LabelFormat"/> property. </para>
    /// <para> <b>TextColor - </b> To customize the text color, refer to this <see cref="ChartLabelStyle.TextColor"/> property. </para>
    /// <para> <b>Background - </b> To customize the background color, refer to this <see cref="ChartLabelStyle.Background"/> property. </para>
    /// <para> <b>Stroke - </b> To customize the stroke color, refer to this <see cref="ChartLabelStyle.Stroke"/> property. </para>
    /// <para> <b>StrokeWidth - </b> To modify the stroke width, refer to this <see cref="ChartLabelStyle.StrokeWidth"/> property. </para>
    /// </remarks>
    public class ChartAxisLabelStyle : ChartLabelStyle
    {
        #region BindableProperties
        /// <summary>
        /// Identifies the <see cref="LabelsPosition"/> bindable property.
        /// </summary>        
        internal static readonly BindableProperty LabelsPositionProperty = BindableProperty.Create(nameof(LabelsPosition), typeof(AxisElementPosition), typeof(ChartAxisLabelStyle), AxisElementPosition.Outside, BindingMode.Default, null, OnLabelsPositionChanged);

        /// <summary>
        /// Identifies the <see cref="LabelAlignment"/> bindable property.
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
        /// Gets or sets a value that indicates the position of the axis label.
        /// </summary>
        /// <value>It accepts <see cref="ChartAxisLabelAlignment"/> values and the default value is <see cref="ChartAxisLabelAlignment.Center"/>.</value>
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
        /// Initializes a new instance of the <see cref="ChartAxisLabelStyle"/>.
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
