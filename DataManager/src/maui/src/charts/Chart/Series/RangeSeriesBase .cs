using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syncfusion.Maui.Charts
{
    /// <summary>
    /// Serves as a base class for all types of range series.
    /// </summary>
    public abstract class RangeSeriesBase : CartesianSeries
    {
        #region Internal Properties

        internal IList<double> HighValues { get; set; }

        internal IList<double> LowValues { get; set; }

        internal override bool IsMultipleYPathRequired
        {
            get
            {
                return !string.IsNullOrEmpty(High) && !string.IsNullOrEmpty(Low);
            }
        }

        #endregion

        #region Bindable Properties

        /// <summary>
        /// Identifies the <see cref="High"/> bindable property.
        /// </summary>
        public static readonly BindableProperty HighProperty =
            BindableProperty.Create(nameof(High), typeof(string), typeof(RangeSeriesBase), string.Empty, BindingMode.Default, null, OnHighAndLowBindingPathChanged);

        /// <summary>
        /// Identifies the <see cref="Low"/> bindable property.
        /// </summary>
        public static readonly BindableProperty LowProperty =
              BindableProperty.Create(nameof(Low), typeof(string), typeof(RangeSeriesBase), string.Empty, BindingMode.Default, null, OnHighAndLowBindingPathChanged);

        /// <summary>
        /// Identifies the <see cref="Stroke"/> bindable property.
        /// </summary>
        public static readonly BindableProperty StrokeProperty =
           BindableProperty.Create(nameof(Stroke), typeof(Brush), typeof(RangeSeriesBase), null, BindingMode.Default, null, OnStrokeColorChanged);

        /// <summary>
        /// Identifies the <see cref="StrokeWidth"/> bindable property.
        /// </summary>
        public static readonly BindableProperty StrokeWidthProperty =
           BindableProperty.Create(nameof(StrokeWidth), typeof(double), typeof(RangeSeriesBase), 0d, BindingMode.Default, null, OnStrokeWidthPropertyChanged);

        /// <summary>
        /// Identifies the <see cref="StrokeDashArray"/> bindable property.
        /// </summary>
        internal static readonly BindableProperty StrokeDashArrayProperty =
           BindableProperty.Create(nameof(StrokeDashArray), typeof(DoubleCollection), typeof(RangeSeriesBase), null, BindingMode.Default, null, OnStrokeDashArrayPropertyChanged);

        #endregion

        #region Constructor

        /// <summary>
        ///  Initializes a new instance of the <see cref="RangeSeriesBase"/>.
        /// </summary>
        public RangeSeriesBase()
        {
            HighValues = new List<double>();
            LowValues = new List<double>();
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets a path value on the source object to serve a high value to the series.
        /// </summary>
        /// <value>The string that represents the property name for the higher plotting data, and its default value is empty.</value>
        /// <example>
        /// # [Xaml](#tab/tabid-28)
        /// <code><![CDATA[
        ///     <chart:SfCartesianChart>
        ///
        ///     <!-- ... Eliminated for simplicity-->
        ///
        ///          <chart:RangeColumnSeries ItemsSource="{Binding Data}"
        ///                                   XBindingPath="XValue"
        ///                                   High ="HighValue"
        ///                                   Low ="LowValue"/>
        ///
        ///     </chart:SfCartesianChart>
        /// ]]>
        /// </code>
        /// # [C#](#tab/tabid-29)
        /// <code><![CDATA[
        ///     SfCartesianChart chart = new SfCartesianChart();
        ///     ViewModel viewModel = new ViewModel();
        ///
        ///     // Eliminated for simplicity
        ///
        ///     RangeColumnSeries series = new RangeColumnSeries()
        ///     {
        ///           ItemsSource = viewModel.Data,
        ///           XBindingPath = "XValue",
        ///           High ="HighValue",
        ///           Low ="LowValue",
        ///     };
        ///     
        ///     chart.Series.Add(series);
        ///
        /// ]]>
        /// </code>
        /// ***
        /// </example>
        public string High
        {
            get { return (string)GetValue(HighProperty); }
            set { SetValue(HighProperty, value); }
        }

        /// <summary>
        /// Gets or sets a path value on the source object to serve a low value to the series.
        /// </summary>
        /// <value>The string that represents the property name for the lower plotting data, and its default value is empty.</value>
        /// <example>
        /// # [Xaml](#tab/tabid-28)
        /// <code><![CDATA[
        ///     <chart:SfCartesianChart>
        ///
        ///     <!-- ... Eliminated for simplicity-->
        ///
        ///          <chart:RangeColumnSeries ItemsSource="{Binding Data}"
        ///                                   XBindingPath="XValue"
        ///                                   High ="HighValue"
        ///                                   Low ="LowValue"/>
        ///
        ///     </chart:SfCartesianChart>
        /// ]]>
        /// </code>
        /// # [C#](#tab/tabid-29)
        /// <code><![CDATA[
        ///     SfCartesianChart chart = new SfCartesianChart();
        ///     ViewModel viewModel = new ViewModel();
        ///
        ///     // Eliminated for simplicity
        ///
        ///     RangeColumnSeries series = new RangeColumnSeries()
        ///     {
        ///           ItemsSource = viewModel.Data,
        ///           XBindingPath = "XValue",
        ///           High ="HighValue",
        ///           Low ="LowValue",
        ///     };
        ///     
        ///     chart.Series.Add(series);
        ///
        /// ]]>
        /// </code>
        /// ***
        /// </example>
        public string Low
        {
            get { return (string)GetValue(LowProperty); }
            set { SetValue(LowProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value to customize the border appearance of the range column series.
        /// </summary>
        /// <value>It accepts <see cref="Brush"/> values and its default value is Transparent.</value>
        /// <example>
        /// # [Xaml](#tab/tabid-28)
        /// <code><![CDATA[
        ///     <chart:SfCartesianChart>
        ///
        ///     <!-- ... Eliminated for simplicity-->
        ///
        ///          <chart:RangeColumnSeries ItemsSource="{Binding Data}"
        ///                                   XBindingPath="XValue"
        ///                                   High ="HighValue"
        ///                                   Low ="LowValue"
        ///                                   Stroke = "Red" />
        ///
        ///     </chart:SfCartesianChart>
        /// ]]>
        /// </code>
        /// # [C#](#tab/tabid-29)
        /// <code><![CDATA[
        ///     SfCartesianChart chart = new SfCartesianChart();
        ///     ViewModel viewModel = new ViewModel();
        ///
        ///     // Eliminated for simplicity
        ///
        ///     RangeColumnSeries series = new RangeColumnSeries()
        ///     {
        ///           ItemsSource = viewModel.Data,
        ///           XBindingPath = "XValue",
        ///           High ="HighValue",
        ///           Low ="LowValue",
        ///           Stroke = new SolidColorBrush(Colors.Red),
        ///     };
        ///     
        ///     chart.Series.Add(series);
        ///
        /// ]]>
        /// </code>
        /// ***
        /// </example>
        public Brush Stroke
        {
            get { return (Brush)GetValue(StrokeProperty); }
            set { SetValue(StrokeProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value to specify the border width of the range column series.
        /// </summary>
        /// <value>It accepts double values and its default value is 0.</value>
        /// <example>
        /// # [Xaml](#tab/tabid-28)
        /// <code><![CDATA[
        ///     <chart:SfCartesianChart>
        ///
        ///     <!-- ... Eliminated for simplicity-->
        ///
        ///          <chart:RangeColumnSeries ItemsSource="{Binding Data}"
        ///                                   XBindingPath="XValue"
        ///                                   High ="HighValue"
        ///                                   Low ="LowValue"
        ///                                   StrokeWidth = "3" />
        ///
        ///     </chart:SfCartesianChart>
        /// ]]>
        /// </code>
        /// # [C#](#tab/tabid-29)
        /// <code><![CDATA[
        ///     SfCartesianChart chart = new SfCartesianChart();
        ///     ViewModel viewModel = new ViewModel();
        ///
        ///     // Eliminated for simplicity
        ///
        ///     RangeColumnSeries series = new RangeColumnSeries()
        ///     {
        ///           ItemsSource = viewModel.Data,
        ///           XBindingPath = "XValue",
        ///           High ="HighValue",
        ///           Low ="LowValue",
        ///           StrokeWidth = 3,
        ///     };
        ///     
        ///     chart.Series.Add(series);
        ///
        /// ]]>
        /// </code>
        /// ***
        /// </example>
        public double StrokeWidth
        {
            get { return (double)GetValue(StrokeWidthProperty); }
            set { SetValue(StrokeWidthProperty, value); }
        }

        /// <summary>
        /// Gets or sets the stroke dash array to customize the appearance of series border.
        /// </summary>
        /// <value>It accepts the <see cref="DoubleCollection"/> value and the default value is null.</value>
        /// <example>
        /// # [Xaml](#tab/tabid-28)
        /// <code><![CDATA[
        ///     <chart:SfCartesianChart>
        ///
        ///     <!-- ... Eliminated for simplicity-->
        ///
        ///          <chart:RangeColumnSeries ItemsSource="{Binding Data}"
        ///                                   XBindingPath="XValue"
        ///                                   High ="HighValue"
        ///                                   Low ="LowValue"
        ///                                   StrokeDashArray="5,3"
        ///                                   Stroke = "Red" />
        ///
        ///     </chart:SfCartesianChart>
        /// ]]>
        /// </code>
        /// # [C#](#tab/tabid-29)
        /// <code><![CDATA[
        ///     SfCartesianChart chart = new SfCartesianChart();
        ///     ViewModel viewModel = new ViewModel();
        ///
        ///     // Eliminated for simplicity
        ///
        ///     DoubleCollection doubleCollection = new DoubleCollection();
        ///     doubleCollection.Add(5);
        ///     doubleCollection.Add(3);
        ///     RangeColumnSeries series = new RangeColumnSeries()
        ///     {
        ///           ItemsSource = viewModel.Data,
        ///           XBindingPath = "XValue",
        ///           High ="HighValue",
        ///           Low ="LowValue",
        ///           StrokeDashArray = doubleCollection,
        ///           Stroke = new SolidColorBrush(Colors.Red),
        ///     };
        ///     
        ///     chart.Series.Add(series);
        ///
        /// ]]>
        /// </code>
        /// ***
        /// </example>
        internal DoubleCollection StrokeDashArray
        {
            get { return (DoubleCollection)GetValue(StrokeDashArrayProperty); }
            set { SetValue(StrokeDashArrayProperty, value); }
        }

        #endregion

        #region Methods

        #region Internal Methods

        internal override void OnBindingPathChanged()
        {
            base.OnBindingPathChanged();
        }

        internal override void SetStrokeColor(ChartSegment segment)
        {
            segment.Stroke = Stroke;
        }

        internal override void SetStrokeWidth(ChartSegment segment)
        {
            segment.StrokeWidth = StrokeWidth;
        }

        internal override void SetDashArray(ChartSegment segment)
        {
            segment.StrokeDashArray = StrokeDashArray;
        }

        internal override void DrawDataLabels(ICanvas canvas)
        {
            var dataLabelSettings = ChartDataLabelSettings;
            if (dataLabelSettings == null) return;

            ChartDataLabelStyle labelStyle = dataLabelSettings.LabelStyle;
            foreach (RangeColumnSegment datalabel in Segments)
            {
                if (!datalabel.InVisibleRange || datalabel.IsEmpty) continue;
                RangeColumnSeriesDataLabelAppearance(canvas, datalabel, dataLabelSettings, labelStyle);
            }
        }

        internal double GetDataLabelPositionAtIndex(int index, double value)
        {
            if (DataLabelSettings == null) return 0;

            var yValue = HighValues?[index] ?? 0f;
            var yValue1 = LowValues?[index] ?? 0f;
            var returnValue = value == yValue ? yValue : yValue1;

            return returnValue;
        }

        internal override void CalculateDataPointPosition(int index, ref double x, ref double y)
        {
            if (ChartArea == null) return;

            var x1 = SbsInfo.Start + x;
            var x2 = SbsInfo.End + x;
            var xCal = x1 + ((x2 - x1) / 2);
            var yCal = y;

            if (ActualYAxis != null && ActualXAxis != null && !double.IsNaN(yCal))
            {
                y = ChartArea.IsTransposed ? ActualXAxis.ValueToPoint(xCal) : ActualYAxis.ValueToPoint(yCal);
            }

            if (ActualXAxis != null && ActualYAxis != null && !double.IsNaN(x))
            {
                x = ChartArea.IsTransposed ? ActualYAxis.ValueToPoint(yCal) : ActualXAxis.ValueToPoint(xCal);
            }
        }

        #endregion

        #region Private Methods

        private static void OnHighAndLowBindingPathChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is RangeSeriesBase series)
            {
                if (newValue != null && newValue is string value)
                {
                    series.YDataPaths.Add(value);
                }

                if (series is ChartSeries chartSeries && chartSeries.Chart != null)
                {
                    chartSeries.Chart.DataManager.OnBindingPathChanged(series, (string)oldValue, newValue, chartSeries.Chart.IsDataPopulated, false);
                }
                
                series.OnBindingPathChanged();
            }
        }

        private static void OnStrokeColorChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is RangeSeriesBase series)
            {
                series.UpdateStrokeColor();
                series.InvalidateSeries();
            }
        }

        private static void OnStrokeWidthPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is RangeSeriesBase series)
            {
                series.UpdateStrokeWidth();
                series.InvalidateSeries();
            }
        }

        private static void OnStrokeDashArrayPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is RangeSeriesBase series)
            {
                series.UpdateDashArray();
                series.InvalidateSeries();
            }
        }

        private void RangeColumnSeriesDataLabelAppearance(ICanvas canvas, RangeColumnSegment datalabel, ChartDataLabelSettings dataLabelSettings, ChartDataLabelStyle labelStyle)
        {
            for(int i = 0; i < 2; i++)
            {
                string labelText;
                PointF position;
                
                if (i == 0)
                {
                    labelText = datalabel.DataLabels[0] ?? string.Empty;
                    position = datalabel.LabelPositionPoints[0];
                }
                else
                {
                    labelText = datalabel.DataLabels[1] ?? string.Empty;
                    position = datalabel.LabelPositionPoints[1];
                }

                if (labelStyle.Angle != 0)
                {
                    float angle = (float)(labelStyle.Angle > 360 ? labelStyle.Angle % 360 : labelStyle.Angle);
                    canvas.SaveState();
                    canvas.Rotate(angle, position.X, position.Y);
                }

                canvas.StrokeSize = (float)labelStyle.StrokeWidth;
                canvas.StrokeColor = labelStyle.Stroke.ToColor();

                var fillcolor = labelStyle.IsBackgroundColorUpdated ? labelStyle.Background : dataLabelSettings.UseSeriesPalette ? datalabel.Fill : labelStyle.Background;
                DrawDataLabel(canvas, fillcolor, labelText, position, datalabel.Index);
            }
        }

        #endregion

        #endregion
    }
}
