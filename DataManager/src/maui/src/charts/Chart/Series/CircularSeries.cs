using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using Syncfusion.Maui.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Syncfusion.Maui.Charts
{
    /// <summary>
    /// CircularSeries is the base class for pie and doughnut charts.
    /// </summary>
    public abstract class CircularSeries : ChartSeries
    {
        #region Fields  

        //Space between connector line edge and data label.
        const float labelGap = 4;

        #endregion

        #region Bindable properties

        /// <summary>
        /// Identifies the <see cref="YBindingPath"/> bindable property.
        /// </summary>        
        public static readonly BindableProperty YBindingPathProperty =
                BindableProperty.Create(
                nameof(YBindingPath),
                typeof(string),
                typeof(CircularSeries),
                null,
                BindingMode.Default,
                null,
                OnYBindingPathChanged);

        /// <summary>
        /// Identifies the <see cref="Stroke"/> bindable property.
        /// </summary>        
        public static readonly BindableProperty StrokeProperty =
                BindableProperty.Create(
                nameof(Stroke),
                typeof(Brush),
                typeof(CircularSeries),
                SolidColorBrush.Transparent,
                BindingMode.Default,
                null,
                OnStrokeChanged);

        /// <summary>
        /// Identifies the <see cref="StrokeWidth"/> bindable property.
        /// </summary>        
        public static readonly BindableProperty StrokeWidthProperty =
            BindableProperty.Create(
                nameof(StrokeWidth),
                typeof(double),
                typeof(CircularSeries),
                2d,
                BindingMode.Default,
                null,
                OnStrokeWidthPropertyChanged);

        /// <summary>
        /// Identifies the <see cref="Radius"/> bindable property.
        /// </summary>        
        public static readonly BindableProperty RadiusProperty =
            BindableProperty.Create(
                nameof(Radius),
                typeof(double),
                typeof(CircularSeries),
                0.8d,
                BindingMode.Default,
                null,
                OnRadiusPropertyChanged);

        /// <summary>
        /// Identifies the <see cref="StartAngle"/> bindable property.
        /// </summary>        
        public static readonly BindableProperty StartAngleProperty =
            BindableProperty.Create(
                nameof(StartAngle),
                typeof(double),
                typeof(CircularSeries),
                0d,
                BindingMode.Default,
                null,
                OnAngleChanged);

        /// <summary>
        /// Identifies the <see cref="EndAngle"/> bindable property.
        /// </summary>        
        public static readonly BindableProperty EndAngleProperty =
            BindableProperty.Create(
                nameof(EndAngle),
                typeof(double),
                typeof(CircularSeries),
                360d,
                BindingMode.Default,
                null,
                OnAngleChanged);

        /// <summary>
        /// Identifies the <see cref="DataLabelSettings"/> bindable property.
        /// </summary>        
        public static readonly BindableProperty DataLabelSettingsProperty =
            BindableProperty.Create(
                nameof(DataLabelSettings),
                typeof(CircularDataLabelSettings),
                typeof(CircularSeries),
                null,
                BindingMode.Default,
                null,
                OnDataLabelSettingsChanged);

        /// <summary>
        /// 
        /// </summary>        
        internal static readonly BindableProperty SmartLabelAlignmentProperty =
                BindableProperty.Create(
                    nameof(SmartLabelAlignment),
                    typeof(SmartLabelAlignment),
                    typeof(CircularSeries),
                    SmartLabelAlignment.Shift,
                    BindingMode.Default,
                    null,
                    OnSmartLabelAlignmentChanged);

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="CircularSeries"/> class.
        /// </summary>
        public CircularSeries()
        {
            YValues = new List<double>();
            DataLabelSettings = new CircularDataLabelSettings();
            InnerBounds = new List<RectF>();
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets a path value on the source object to serve a y value to the series.
        /// </summary>
        /// <value>
        /// The string that represents the property name for the y plotting data, and its default value is null.
        /// </value>
        /// <example>
        /// # [Xaml](#tab/tabid-1)
        /// <code><![CDATA[
        ///     <chart:SfCircularChart>
        ///
        ///          <chart:PieSeries ItemsSource="{Binding Data}"
        ///                            XBindingPath="XValue"
        ///                            YBindingPath="YValue" />
        ///
        ///     </chart:SfCircularChart>
        /// ]]></code>
        /// # [C#](#tab/tabid-2)
        /// <code><![CDATA[
        ///     SfCircularChart chart = new SfCircularChart();
        ///     ViewModel viewModel = new ViewModel();
        ///
        ///     PieSeries series = new PieSeries()
        ///     {
        ///           ItemsSource = viewModel.Data,
        ///           XBindingPath = "XValue",
        ///           YBindingPath = "YValue",
        ///     };
        ///     
        ///     chart.Series.Add(series);
        ///
        /// ]]></code>
        /// ***
        /// </example>
        public string YBindingPath
        {
            get { return (string)GetValue(YBindingPathProperty); }
            set { SetValue(YBindingPathProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value to customize the stroke appearance of the series.
        /// </summary>
        /// <value>It accepts <see cref="Brush"/> values and its default value is null.</value>
        /// <example>
        /// # [Xaml](#tab/tabid-1)
        /// <code><![CDATA[
        ///     <chart:SfCircularChart>
        ///
        ///          <chart:PieSeries ItemsSource="{Binding Data}"
        ///                            XBindingPath="XValue"
        ///                            YBindingPath="YValue"
        ///                            Stroke = "Red"
        ///                            StrokeWidth = "3"/>
        ///
        ///     </chart:SfCircularChart>
        /// ]]></code>
        /// # [C#](#tab/tabid-2)
        /// <code><![CDATA[
        ///     SfCircularChart chart = new SfCircularChart();
        ///     ViewModel viewModel = new ViewModel();
        ///
        ///     PieSeries series = new PieSeries()
        ///     {
        ///           ItemsSource = viewModel.Data,
        ///           XBindingPath = "XValue",
        ///           YBindingPath = "YValue",
        ///           Stroke = new SolidColorBrush(Colors.Red),
        ///           StrokeWidth = 3,
        ///     };
        ///     
        ///     chart.Series.Add(series);
        ///
        /// ]]></code>
        /// ***
        /// </example>
        public Brush Stroke
        {
            get { return (Brush)GetValue(StrokeProperty); }
            set { SetValue(StrokeProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value to specify the stroke width of a chart series.
        /// </summary>
        /// <value>It accepts double values and its default value is 2.</value>
        /// <example>
        /// # [Xaml](#tab/tabid-3)
        /// <code><![CDATA[
        ///     <chart:SfCircularChart>
        ///
        ///          <chart:PieSeries ItemsSource="{Binding Data}"
        ///                            XBindingPath="XValue"
        ///                            YBindingPath="YValue"
        ///                            Stroke = "Red"
        ///                            StrokeWidth = "3"/>
        ///
        ///     </chart:SfCircularChart>
        /// ]]></code>
        /// # [C#](#tab/tabid-4)
        /// <code><![CDATA[
        ///     SfCircularChart chart = new SfCircularChart();
        ///     ViewModel viewModel = new ViewModel();
        ///
        ///     PieSeries series = new PieSeries()
        ///     {
        ///           ItemsSource = viewModel.Data,
        ///           XBindingPath = "XValue",
        ///           YBindingPath = "YValue",
        ///           Stroke = new SolidColorBrush(Colors.Red),
        ///           StrokeWidth = 3,
        ///     };
        ///     
        ///     chart.Series.Add(series);
        ///
        /// ]]></code>
        /// ***
        /// </example>
        public double StrokeWidth
        {
            get { return (double)GetValue(StrokeWidthProperty); }
            set { SetValue(StrokeWidthProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value that can be used to render the series size.
        /// </summary>
        /// <value>It accepts double values, and the default value is 0.8. Here, the value is between 0 and 1.</value>
        /// <example>
        /// # [Xaml](#tab/tabid-1)
        /// <code><![CDATA[
        ///     <chart:SfCircularChart>
        ///
        ///          <chart:PieSeries ItemsSource="{Binding Data}"
        ///                            XBindingPath="XValue"
        ///                            YBindingPath="YValue"
        ///                            Radius = "0.7"/>
        ///
        ///     </chart:SfCircularChart>
        /// ]]></code>
        /// # [C#](#tab/tabid-2)
        /// <code><![CDATA[
        ///     SfCircularChart chart = new SfCircularChart();
        ///     ViewModel viewModel = new ViewModel();
        ///
        ///     PieSeries series = new PieSeries()
        ///     {
        ///           ItemsSource = viewModel.Data,
        ///           XBindingPath = "XValue",
        ///           YBindingPath = "YValue",
        ///           Radius = 0.7,
        ///     };
        ///     
        ///     chart.Series.Add(series);
        ///
        /// ]]></code>
        /// ***
        /// </example>
        public double Radius
        {
            get { return (double)GetValue(RadiusProperty); }
            set { SetValue(RadiusProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value that can be used to modify the series start rendering position.
        /// </summary>
        /// <remarks>It is used to draw a series in different shapes.</remarks>
        /// <value>It accepts double values, and the default value is 0.</value>
        /// <example>
        /// # [Xaml](#tab/tabid-1)
        /// <code><![CDATA[
        ///     <chart:SfCircularChart>
        ///
        ///          <chart:PieSeries ItemsSource="{Binding Data}"
        ///                            XBindingPath="XValue"
        ///                            YBindingPath="YValue"
        ///                            StartAngle = "180"/>
        ///
        ///     </chart:SfCircularChart>
        /// ]]></code>
        /// # [C#](#tab/tabid-2)
        /// <code><![CDATA[
        ///     SfCircularChart chart = new SfCircularChart();
        ///     ViewModel viewModel = new ViewModel();
        ///
        ///     PieSeries series = new PieSeries()
        ///     {
        ///           ItemsSource = viewModel.Data,
        ///           XBindingPath = "XValue",
        ///           YBindingPath = "YValue",
        ///           StartAngle = 180,
        ///     };
        ///     
        ///     chart.Series.Add(series);
        ///
        /// ]]></code>
        /// ***
        /// </example>
        public double StartAngle
        {
            get { return (double)GetValue(StartAngleProperty); }
            set { SetValue(StartAngleProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value that can be used to modify the series end rendering position.
        /// </summary>
        /// <remarks>It is used to draw a series in different shapes.</remarks>
        /// <value>It accepts double values, and the default value is 360.</value>
        /// <example>
        /// # [Xaml](#tab/tabid-1)
        /// <code><![CDATA[
        ///     <chart:SfCircularChart>
        ///
        ///          <chart:PieSeries ItemsSource="{Binding Data}"
        ///                            XBindingPath="XValue"
        ///                            YBindingPath="YValue"
        ///                            EndAngle = "270"/>
        ///
        ///     </chart:SfCircularChart>
        /// ]]></code>
        /// # [C#](#tab/tabid-2)
        /// <code><![CDATA[
        ///     SfCircularChart chart = new SfCircularChart();
        ///     ViewModel viewModel = new ViewModel();
        ///
        ///     PieSeries series = new PieSeries()
        ///     {
        ///           ItemsSource = viewModel.Data,
        ///           XBindingPath = "XValue",
        ///           YBindingPath = "YValue",
        ///           EndAngle = 270,
        ///     };
        ///     
        ///     chart.Series.Add(series);
        ///
        /// ]]></code>
        /// ***
        /// </example>
        public double EndAngle
        {
            get { return (double)GetValue(EndAngleProperty); }
            set { SetValue(EndAngleProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value to customize the appearance of the displaying data labels in the circular series.
        /// </summary>
        /// <remarks>
        /// This allows us to change the look of the displaying labels' content, shapes, and connector lines at the data point.
        /// </remarks>
        /// <value>This property takes the <see cref="CircularDataLabelSettings"/>.</value>
        /// <example>
        /// # [MainWindow.xaml](#tab/tabid-5)
        /// <code><![CDATA[
        ///     <chart:SfCircularChart>
        ///
        ///           <chart:SfCircularChart.Series>
        ///               <chart:PieSeries ItemsSource="{Binding Data}"
        ///                                XBindingPath="XValue"
        ///                                YBindingPath="YValue"
        ///                                ShowDataLabels="True">
        ///                    <syncfusion:PieSeries.DataLabelSettings>
        ///                          <chart:CircularDataLabelSettings LabelPlacement="Outside" />
        ///                    <syncfusion:PieSeries.DataLabelSettings>
        ///               </chart:PieSeries> 
        ///           </chart:SfCircularChart.Series>
        ///           
        ///     </chart:SfCircularChart>
        /// ]]></code>
        /// # [MainWindow.cs](#tab/tabid-6)
        /// <code><![CDATA[
        ///     SfCircularChart chart = new SfCircularChart();
        ///     
        ///     PieSeries series = new PieSeries();
        ///     series.ItemsSource = viewmodel.Data;
        ///     series.XBindingPath = "XValue";
        ///     series.YBindingPath = "YValue";
        ///     series.ShowDataLabels = "True";
        ///     chart.Series.Add(series);
        ///     
        ///     series.DataLabelSettings = new CircularDataLabelSettings(){ LabelPlacement = DataLabelPlacement.OutSide };
        /// ]]></code>
        /// ***
        /// </example>
        public CircularDataLabelSettings DataLabelSettings
        {
            get { return (CircularDataLabelSettings)GetValue(DataLabelSettingsProperty); }
            set { SetValue(DataLabelSettingsProperty, value); }
        }

        /// <summary>
        /// 
        /// </summary>
        internal SmartLabelAlignment SmartLabelAlignment
        {
            get { return (SmartLabelAlignment)GetValue(SmartLabelAlignmentProperty); }
            set { SetValue(SmartLabelAlignmentProperty, value); }
        }

        #endregion

        #region Internal Properties

        internal CircularChartArea? ChartArea { get; set; }

        /// <summary>
		/// Gets or sets the y values.
		/// </summary>
		internal IList<double> YValues { get; set; }

        internal PointF Center { get; set; }

        internal float ActualRadius { get; set; }

        internal override ChartDataLabelSettings ChartDataLabelSettings => DataLabelSettings;

        internal List<RectF> InnerBounds { get; set; }

        internal List<PieSegment>? LeftTopPoints { get; set; }

        internal List<PieSegment>? LeftBottomPoints { get; set; }

        internal List<PieSegment>? RightTopPoints { get; set; }

        internal List<PieSegment>? RightBottomPoints { get; set; }

        //TODO: Need to remove by calculate smartlabels without using series. 
        internal Rect AdjacentLabelRect { get; set; } = Rect.Zero;
        #endregion

        #region Methods

        #region Protected Methods

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();

            if (DataLabelSettings != null)
            {
                SetInheritedBindingContext(DataLabelSettings, BindingContext);
            }
        }

        /// <inheritdoc/>
        protected internal override void DrawDataLabel(ICanvas canvas, Brush? fillcolor, string label, PointF point, int index)
        {
            var pieSegment = Segments[index] as PieSegment;

            if (pieSegment == null) return;

            if (SmartLabelAlignment == SmartLabelAlignment.None ||  (pieSegment.IsVisible && !IsOverlapWithPrevious(pieSegment)))
            {
                //If customer changes the label in DrawDataLabel method.
                if (pieSegment.DataLabel != label)
                {
                    pieSegment.DataLabel = label;
                    var labelSize = DataLabelSettings.LabelStyle.MeasureLabel(label);
                    List<float> xpoints = pieSegment.XPoints;

                    if (xpoints != null && xpoints.Count > 2)
                    {
                        float x = xpoints[2];
                        x = pieSegment.IsLeft ? x - (labelSize.Width / 2) - labelGap : x + (labelSize.Width / 2) + labelGap;
                        pieSegment.LabelPositionPoint = new PointF(x, point.Y);
                        point = pieSegment.LabelPositionPoint;
                    }

                    EdgeDetectionforLabel(pieSegment);
                }

                pieSegment.IsVisible = !string.IsNullOrEmpty(pieSegment.TrimmedText);
                if (pieSegment.IsVisible)
                {
                    base.DrawDataLabel(canvas, fillcolor, pieSegment.TrimmedText, point, index);
                    DrawConnectorLine(canvas, pieSegment);
                }
            }
        }

        #endregion

        #region Internal Methods

        internal void UpdateCenterViewBounds(View centerView)
        {
            var center = GetCenter();

            if (center != PointF.Zero && centerView != null)
            {
                var xPosition = center.X / AreaBounds.Width;
                var yPosition = center.Y / AreaBounds.Height;
                AbsoluteLayout.SetLayoutBounds(centerView, new Rect(xPosition, yPosition, -1, -1));
                AbsoluteLayout.SetLayoutFlags(centerView, Microsoft.Maui.Layouts.AbsoluteLayoutFlags.PositionProportional);
            }
        }

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

        internal float SumOfYValues()
        {
            float sum = 0f;

            if (YValues != null)
            {
                foreach (double number in YValues)
                {
                    if (!double.IsNaN(number))
                    {
                        sum += (float)number;
                    }
                }
            }

            return sum;
        }

        internal override void UpdateLegendIconColor()
        {
            var legend = Chart?.Legend;
            var legendItems = ChartArea?.PlotArea.LegendItems;

            if (legend != null && legend.IsVisible && legendItems != null)
            {
                foreach (LegendItem legendItem in legendItems)
                {
                    if (legendItem != null)
                    {
                        legendItem.IconBrush = GetFillColor(legendItem.Index) ?? new SolidColorBrush(Colors.Transparent);
                    }
                }
            }
        }

        internal override void UpdateLegendIconColor(int index)
        {
            var legend = Chart?.Legend;
            var legendItems = ChartArea?.PlotArea.LegendItems;

            if (legend != null && legend.IsVisible && legendItems != null && index < legendItems.Count)
            {
                if (legendItems[index] is LegendItem legendItem)
                {
                    legendItem.IconBrush = GetFillColor(index) ?? new SolidColorBrush(Colors.Transparent);
                }
            }
        }

        internal override void UpdateSelectedItem(int index)
        {
            base.UpdateSelectedItem(index);
            UpdateLegendIconColor(index);
        }

        internal float GetRadius()
        {
            if (AreaBounds != Rect.Zero)
            {
                return (float)Radius * (float)(Math.Min(AreaBounds.Width, AreaBounds.Height) / 2);
            }

            return 0.0f;
        }

        internal PointF GetCenter()
        {
            if (AreaBounds != Rect.Zero)
            {
                return GetActualCenter((float)(AreaBounds.Width * 0.5), (float)(AreaBounds.Height * 0.5), ActualRadius);
            }

            return default(PointF);
        }

        internal double GetAngleDifference()
        {
            var angleDifference = EndAngle - StartAngle;

            //Circular series not rendering with single segment, so reduced with 0.01 when the angle difference is 360.
            angleDifference = Math.Abs(angleDifference).Equals(360) ? Math.Abs(angleDifference) - 0.0001 : angleDifference;

            if (Math.Abs(Math.Round(angleDifference * 100.0) / 100.0) > 360)
            {
                angleDifference = angleDifference % 360;
            }

            return angleDifference;
        }

        internal double CalculateTotalYValues()
        {
            double total = 0;

            for (int i = 0; i < YValues.Count; i++)
            {
                if (!double.IsNaN(YValues[i]) && (Segments.Count == 0 || ((Segments.Count <= i) || (Segments.Count > i && Segments[i].IsVisible))))
                {
                    total += Math.Abs(YValues[i]);
                }
            }

            return total;
        }

        #region DataLabels Methods

        internal virtual float GetDataLabelRadius()
        {
            return 1;
        }

        internal override void DrawDataLabels(ICanvas canvas)
        {
            base.DrawDataLabels(canvas);

            if (!NeedToAnimateDataLabel)
            {
                if (InnerBounds != null)
                {
                    InnerBounds.Clear();
                }

                AdjacentLabelRect = Rect.Zero;
            }
        }

        #endregion

        #endregion

        #region Private Methods

        private void DrawConnectorLine(ICanvas canvas, ChartSegment datalabel)
        {
            var pieSegment = datalabel as PieSegment;

            if (pieSegment == null) return;

            canvas.StrokeSize = 2;
            canvas.StrokeColor = datalabel.Fill?.ToColor();
            canvas.StrokeLineCap =  LineCap.Round; 

            if (DataLabelSettings != null && pieSegment.IsVisible && pieSegment.XPoints?.Count > 2)
            {
                List<float> xPoints = pieSegment.XPoints;
                List<float> yPoints = pieSegment.YPoints;

                if (DataLabelSettings.ConnectorType == ConnectorType.Line)
                {
                    canvas.DrawLine(xPoints[0], yPoints[0], xPoints[1], yPoints[1]);
                    canvas.DrawLine(xPoints[1], yPoints[1], xPoints[2], yPoints[2]);
                }
                else
                {
                    PathF path = new PathF();
                    path.MoveTo(xPoints[0], yPoints[0]);
                    path.QuadTo(xPoints[1], yPoints[1], xPoints[2], yPoints[2]);
                    canvas.DrawPath(path);
                }
            }
        }

        private PointF GetActualCenter(float x, float y, float radius)
        {
            PointF actualCenter = new PointF(x, y);

            double startAngle1 = GetWrapAngle(StartAngle, -630, 630);
            double endAngle1 = GetWrapAngle(EndAngle, -630, 630);
            float[] regions = new float[] { -630, -540, -450, -360, -270, -180, -90, 0, 90, 180, 270, 360, 450, 540, 630 };
            List<int> region = new List<int>();
            if (startAngle1 < endAngle1)
            {
                for (int i = 0; i < regions.Length; i++)
                {
                    if (regions[i] > startAngle1 && regions[i] < endAngle1)
                    {
                        region.Add((int)((regions[i] % 360) < 0 ? (regions[i] % 360) + 360 : (regions[i] % 360)));
                    }
                }
            }
            else
            {
                for (int i = 0; i < regions.Length; i++)
                {
                    if (regions[i] < startAngle1 && regions[i] > endAngle1)
                    {
                        region.Add((int)((regions[i] % 360) < 0 ? (regions[i] % 360) + 360 : (regions[i] % 360)));
                    }
                }
            }

            double startRadian = 2 * Math.PI * (startAngle1 / 360);
            double endRadian = 2 * Math.PI * (endAngle1 / 360);
            PointF startPoint = new PointF((float)(x + (radius * Math.Cos(startRadian))), (float)(y + (radius * Math.Sin(startRadian))));
            PointF endPoint = new PointF((float)(x + (radius * Math.Cos(endRadian))), (float)(y + (radius * Math.Sin(endRadian))));

            switch (region.Count)
            {
                case 0:
                    float longX = Math.Abs(x - startPoint.X) > Math.Abs(x - endPoint.X) ? startPoint.X : endPoint.X;
                    float longY = Math.Abs(y - startPoint.Y) > Math.Abs(y - endPoint.Y) ? startPoint.Y : endPoint.Y;
                    PointF midPoint = new PointF(Math.Abs(x + longX) / 2, Math.Abs(y + longY) / 2);
                    actualCenter.X = x + (x - midPoint.X);
                    actualCenter.Y = y + (y - midPoint.Y);
                    break;
                case 1:
                    PointF point1 = new PointF(), point2 = new PointF();
                    float maxRadian = (float)(2 * Math.PI * region[0] / 360);
                    PointF maxPoint = new PointF((float)(x + (radius * Math.Cos(maxRadian))), (float)(y + (radius * Math.Sin(maxRadian))));

                    switch (region[0])
                    {
                        case 270:
                            point1 = new PointF(startPoint.X, maxPoint.Y);
                            point2 = new PointF(endPoint.X, y);
                            break;
                        case 0:
                        case 360:
                            point1 = new PointF(x, endPoint.Y);
                            point2 = new PointF(maxPoint.X, startPoint.Y);
                            break;
                        case 90:
                            point1 = new PointF(endPoint.X, y);
                            point2 = new PointF(startPoint.X, maxPoint.Y);
                            break;
                        case 180:
                            point1 = new PointF(maxPoint.X, startPoint.Y);
                            point2 = new PointF(x, endPoint.Y);
                            break;
                    }

                    midPoint = new PointF((point1.X + point2.X) / 2, (point1.Y + point2.Y) / 2);
                    actualCenter.X = x + ((x - midPoint.X) >= radius ? 0 : (x - midPoint.X));
                    actualCenter.Y = y + ((y - midPoint.Y) >= radius ? 0 : (y - midPoint.Y));
                    break;
                case 2:
                    float minRadian = (float)(2 * Math.PI * region[0] / 360);
                    maxRadian = (float)(2 * Math.PI * region[1] / 360);
                    maxPoint = new PointF((float)(x + (radius * Math.Cos(maxRadian))), (float)(y + (radius * Math.Sin(maxRadian))));
                    PointF minPoint = new PointF((float)(x + (radius * Math.Cos(minRadian))), (float)(y + (radius * Math.Sin(minRadian))));

                    if ((region[0] == 0 && region[1] == 90) || (region[0] == 180
                        && region[1] == 270))
                    {
                        point1 = new PointF(minPoint.X, maxPoint.Y);
                    }
                    else
                    {
                        point1 = new PointF(maxPoint.X, minPoint.Y);
                    }

                    if (region[0] == 0 || region[0] == 180)
                    {
                        point2 = new PointF(GetMinMaxValue(startPoint, endPoint, region[0]), GetMinMaxValue(startPoint, endPoint, region[1]));
                    }
                    else
                    {
                        point2 = new PointF(GetMinMaxValue(startPoint, endPoint, region[1]), GetMinMaxValue(startPoint, endPoint, region[0]));
                    }

                    midPoint = new PointF(
                        Math.Abs(point1.X - point2.X) / 2 >= radius ? 0 : (point1.X + point2.X) / 2,
                        Math.Abs(point1.Y - point2.Y) / 2 >= radius ? 0 : (point1.Y + point2.Y) / 2);
                    actualCenter.X = x + (midPoint.X == 0 ? 0 : (x - midPoint.X) >= radius ? 0 : (x - midPoint.X));
                    actualCenter.Y = y + (midPoint.Y == 0 ? 0 : (y - midPoint.Y) >= radius ? 0 : (y - midPoint.Y));
                    break;
            }

            return actualCenter;
        }

        private void AngleValueChanged()
        {
            if (AreaBounds != Rect.Zero)
            {
                SegmentsCreated = false;
                ScheduleUpdateChart();
            }
        }

        internal void ChangeIntersectedLabelPosition()
        {
            if (SmartLabelAlignment == SmartLabelAlignment.Shift)
            {
                LeftBottomPoints = new List<PieSegment>();
                LeftTopPoints = new List<PieSegment>();
                RightBottomPoints = new List<PieSegment>();
                RightTopPoints = new List<PieSegment>();

                foreach (PieSegment segment in Segments)
                {
                    segment.IsVisible = true;

                    if (segment.RenderingPosition == RenderingPosition.Inner || segment.YValue == 0)
                        continue;

                    if (segment.LabelPositionPoint.X < Center.X)
                    {
                        if (segment.LabelPositionPoint.Y > Center.Y)
                            LeftBottomPoints.Add(segment);
                        else LeftTopPoints.Add(segment);
                    }
                    else
                    {
                        if (segment.LabelPositionPoint.Y > Center.Y)
                            RightBottomPoints.Add(segment);
                        else RightTopPoints.Add(segment);
                    }
                }

                int count = LeftTopPoints.Count - 1;

                for (int i = count; i > 0; i--)
                {
                    PieSegment currentSegment = LeftTopPoints[i];
                    PieSegment previousSegment = LeftTopPoints[i - 1];

                    if (IsOverlap(currentSegment.LabelRect, previousSegment.LabelRect) || (currentSegment.LabelRect.Y + currentSegment.LabelRect.Height) > previousSegment.LabelRect.Y)
                    {
                        float y = (float)(currentSegment.LabelRect.Bottom + (previousSegment.LabelRect.Height / 4));
                        float x = (float)(previousSegment.LabelRect.X - (previousSegment.LabelRect.Width * 3 / 4));
                        previousSegment.LabelPositionPoint = new PointF(x, y);
                        previousSegment.LabelRect = new Rect(previousSegment.LabelPositionPoint, previousSegment.LabelRect.Size);
                        previousSegment.XPoints[2] = (float)(x + (previousSegment.LabelRect.Width / 2) + labelGap);
                        previousSegment.YPoints[2] = (float)(previousSegment.LabelRect.Center.Y - (previousSegment.LabelRect.Height / 2));
                    }
                }

                count = RightTopPoints.Count - 1;

                for (int i = 0; i < count; i++)
                {
                    PieSegment currentSegment = RightTopPoints[i];
                    PieSegment nextSegment = RightTopPoints[i + 1];

                    if (IsOverlap(currentSegment.LabelRect, nextSegment.LabelRect) || (currentSegment.LabelRect.Y + currentSegment.LabelRect.Height) > nextSegment.LabelRect.Y)
                    {
                        float y = (float)(currentSegment.LabelRect.Bottom + (nextSegment.LabelRect.Height / 4));
                        float x = (float)(nextSegment.LabelRect.X + (nextSegment.LabelRect.Width * 3 / 4));
                        nextSegment.LabelPositionPoint = new PointF(x, y);
                        nextSegment.LabelRect = new Rect(nextSegment.LabelPositionPoint, nextSegment.LabelRect.Size);
                        nextSegment.XPoints[2] = (float)(x - (nextSegment.LabelRect.Width / 2) - labelGap);
                        nextSegment.YPoints[2] = (float)(nextSegment.LabelRect.Center.Y - (nextSegment.LabelRect.Height / 2));
                    }
                }

                count = LeftBottomPoints.Count - 1;

                for (int i = 0; i < count; i++)
                {
                    PieSegment currentSegment = LeftBottomPoints[i];
                    PieSegment nextSegment = LeftBottomPoints[i + 1];

                    if (IsOverlap(currentSegment.LabelRect, nextSegment.LabelRect) || (currentSegment.LabelRect.Top < nextSegment.LabelRect.Bottom) || (currentSegment.LabelRect.Top < nextSegment.LabelRect.Y))
                    {
                        float y = (float)(currentSegment.LabelRect.Top - (nextSegment.LabelRect.Height / 2));
                        y = (float)(nextSegment.LabelRect.Y < y ? y - (nextSegment.LabelRect.Height / 2) : y);
                        float x = (float)(nextSegment.LabelRect.X - (nextSegment.LabelRect.Width * 3 / 4));
                        nextSegment.LabelPositionPoint = new PointF(x, y);
                        nextSegment.LabelRect = new Rect(nextSegment.LabelPositionPoint, nextSegment.LabelRect.Size);
                        nextSegment.XPoints[2] = (float)(x + (nextSegment.LabelRect.Width / 2) + labelGap);
                        nextSegment.YPoints[2] = (float)(nextSegment.LabelRect.Center.Y - (nextSegment.LabelRect.Height / 2));
                    }
                }

                count = RightBottomPoints.Count - 1;

                for (int i = count; i > 0; i--)
                {
                    PieSegment currentSegment = RightBottomPoints[i];
                    PieSegment previousSegment = RightBottomPoints[i - 1];

                    if (IsOverlap(currentSegment.LabelRect, previousSegment.LabelRect) || (currentSegment.LabelRect.Top < previousSegment.LabelRect.Bottom) || (currentSegment.LabelRect.Top < previousSegment.LabelRect.Y))
                    {
                        float y = (float)(currentSegment.LabelRect.Top - (previousSegment.LabelRect.Height / 2));
                        y = (float)(previousSegment.LabelRect.Y < (y + (previousSegment.LabelRect.Height / 2)) ? y - (previousSegment.LabelRect.Height / 2) : y);
                        float x = (float)(previousSegment.LabelRect.X + (previousSegment.LabelRect.Width * 3 / 4));
                        previousSegment.LabelPositionPoint = new PointF(x, y);
                        previousSegment.LabelRect = new Rect(previousSegment.LabelPositionPoint, previousSegment.LabelRect.Size);
                        previousSegment.XPoints[2] = (float)(x - (previousSegment.LabelRect.Width / 2) - labelGap);
                        previousSegment.YPoints[2] = (float)(previousSegment.LabelRect.Center.Y - (previousSegment.LabelRect.Height / 2));
                    }
                }
            }
            else if (SmartLabelAlignment == SmartLabelAlignment.Hide)
            {
                for (int i = 0; i < Segments.Count - 1; i++)
                {
                    PieSegment? currentSegment = Segments[i] as PieSegment;
                    PieSegment? nextSegment = Segments[i + 1] as PieSegment;

                    if (currentSegment == null || nextSegment == null) return;

                    if (currentSegment.IsVisible && currentSegment.LabelRect.IntersectsWith(nextSegment.LabelRect) ||
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                       (!currentSegment.IsVisible && currentSegment.Index > 0 && (Segments[currentSegment.Index - 1] as PieSegment).LabelRect.IntersectsWith(nextSegment.LabelRect)))
#pragma warning restore CS8602 // Dereference of a possibly null reference.
                    {
                        Segments[nextSegment.Index].IsVisible = false;
                    }
                }
            }

            foreach (var segment in Segments)
            {
                var pieSegment = segment as PieSegment;
                EdgeDetectionforLabel(pieSegment);
            }
        }

        internal bool IsOverlap(Rect currentRect, Rect rect)
        {
            return currentRect.Left < rect.Left + rect.Width &&
                currentRect.Left + currentRect.Width > rect.Left &&
                currentRect.Top < (rect.Top + rect.Height) &&
                (currentRect.Height + currentRect.Top) > rect.Top;
        }

        /// To find the current point overlapped with previous points
        private bool IsOverlapWithPrevious(PieSegment currentPoint)
        {
            if (currentPoint.SeriesView == null) return false;

            var segments = Segments;    
            int currentPointIndex = currentPoint.Index;

            for (int i = 0; i < currentPointIndex; i++)
            {
                if (i != segments.IndexOf(currentPoint) && segments[i].IsVisible &&
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                    IsOverlap(currentPoint.LabelRect, (segments[i] as PieSegment).LabelRect))
#pragma warning restore CS8602 // Dereference of a possibly null reference.
                {
                    segments[currentPointIndex].IsVisible = false;
                    return true;
                }
            }

            if (LeftTopPoints != null && LeftTopPoints.Contains(currentPoint) || RightTopPoints != null && RightTopPoints.Contains(currentPoint))
            {
                if (currentPoint.LabelRect.Y > Center.Y)
                    return true;
            }

            return false;
        }

        private void EdgeDetectionforLabel(PieSegment? pieSegment)
        {
            if (pieSegment == null || pieSegment.DataLabel == null) return;

            PointF point = pieSegment.LabelPositionPoint;
            string label = pieSegment.DataLabel;
            Rect clipRect = AreaBounds;
            var x = point.X;
            double left = x - pieSegment.LabelRect.Width / 2;
            double right = x + pieSegment.LabelRect.Width / 2;

            if (left < clipRect.Left)
            {
                label = GetTrimmedText(label, ref x, right - clipRect.Left, DataLabelSettings.LabelStyle, pieSegment.IsLeft);
            }

            if (right > clipRect.Right)
            {
                label = GetTrimmedText(label, ref x, clipRect.Right - left, DataLabelSettings.LabelStyle, pieSegment.IsLeft);
            }

            point.X = x;
            pieSegment.LabelPositionPoint = point;
            pieSegment.TrimmedText = label;
        }

        /// To trim the text by given width.
        private string GetTrimmedText(string text, ref float x, double labelsExtent, ChartLabelStyle labelStyle, bool isLeft)
        {
            string label = text;
            double oldSize = labelStyle.MeasureLabel(label).Width;

            if (oldSize > labelsExtent)
            {
                int textLength = text.Length;

                for (int i = textLength - 1; i >= 0; --i)
                {
                    label = text.Substring(0, i) + "...";
                    double newSize = labelStyle.MeasureLabel(label).Width;

                    if (newSize <= labelsExtent)
                    {
                        x = (float)(isLeft ? (x + (oldSize/2) - (newSize/2)) : (x - (oldSize / 2) + (newSize / 2)));
                        return label == "..." ? "" : label;
                    }
                }
            }

            return label == "..." ? "" : label;
        }

        #endregion

        #region Static Methods

        private static void OnYBindingPathChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var series = bindable as CircularSeries;
            if (series != null)
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

        private static void OnStrokeChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var circularSeries = bindable as CircularSeries;

            if (circularSeries != null && circularSeries.AreaBounds != Rect.Zero)
            {
                circularSeries.UpdateStrokeColor();
                circularSeries.InvalidateSeries();
            }
        }

        private static void OnStrokeWidthPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var circularSeries = bindable as CircularSeries;

            if (circularSeries != null && circularSeries.AreaBounds != Rect.Zero)
            {
                circularSeries.UpdateStrokeWidth();
                circularSeries.InvalidateSeries();
            }
        }

        private static void OnRadiusPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var circularSeries = bindable as CircularSeries;
            if (circularSeries != null)
            {
                circularSeries.ScheduleUpdateChart();
            }
        }

        private static void OnAngleChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var circularSeries = bindable as CircularSeries;
            if (circularSeries != null)
            {
                circularSeries.AngleValueChanged();
            }
        }

        private static void OnDataLabelSettingsChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var circularSeries = bindable as CircularSeries; 

            if (circularSeries != null)
            {
                circularSeries.OnDataLabelSettingsPropertyChanged(oldValue as ChartDataLabelSettings, newValue as ChartDataLabelSettings);
            }
        }

        private static void OnSmartLabelAlignmentChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var circularSeries = bindable as CircularSeries;

            if (circularSeries != null && circularSeries.Chart != null)
            {
                circularSeries.InvalidateMeasureDataLabel();
                circularSeries.ChangeIntersectedLabelPosition();
                circularSeries.InvalidateDataLabel();
            }
        }

        private static double GetWrapAngle(double angle, int min, int max)
        {
            if (max - min == 0)
            {
                return min;
            }

            angle = ((angle - min) % (max - min)) + min;
            while (angle < min)
            {
                angle += max - min;
            }

            return angle;
        }

        private static float GetMinMaxValue(PointF point1, PointF point2, int degree)
        {
            float minX = Math.Min(point1.X, point2.Y);
            float minY = Math.Min(point1.Y, point2.Y);
            float maxX = Math.Max(point1.X, point2.X);
            float maxY = Math.Max(point1.Y, point2.Y);
            switch (degree)
            {
                case 270:
                    return maxY;
                case 0:
                case 360:
                    return minX;
                case 90:
                    return minY;
                case 180:
                    return maxX;
            }

            return 0f;
        }

        #endregion

        #endregion
    }

}
