using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Media;
using Windows.Foundation;
using Windows.UI.Text;

namespace Syncfusion.UI.Xaml.Charts
{
    /// <summary>
    /// 
    /// </summary>
    public class ChartDataLabel : ChartSegment 
    {
        #region Dependency Property Registration

        /// <summary>
        /// Identifies the <see cref="Background"/> dependency property.
        /// </summary>        
        /// <value>
        /// The identifier for <c>Background</c> dependency property.
        /// </value>
        public static readonly DependencyProperty BackgroundProperty =
            DependencyProperty.Register("Background", typeof(Brush), typeof(ChartDataLabel),
            new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="BorderBrush"/> dependency property.
        /// </summary>        
        /// <value>
        /// The identifier for <c>BorderBrush</c> dependency property.
        /// </value>
        public static readonly DependencyProperty BorderBrushProperty =
            DependencyProperty.Register("BorderBrush", typeof(Brush), typeof(ChartDataLabel),
            new PropertyMetadata(new SolidColorBrush(Colors.Transparent)));

        /// <summary>
        /// Identifies the <see cref="BorderThickness"/> dependency property.
        /// </summary>        
        /// <value>
        /// The identifier for <c>BorderThickness</c> dependency property.
        /// </value>
        public static readonly DependencyProperty BorderThicknessProperty =
            DependencyProperty.Register("BorderThickness", typeof(Thickness), typeof(ChartDataLabel),
            new PropertyMetadata(new Thickness().GetThickness(0, 0, 0, 0)));

        /// <summary>
        /// Identifies the <see cref="Margin"/> dependency property.
        /// </summary>        
        /// <value>
        /// The identifier for <c>Margin</c> dependency property.
        /// </value>
        public static readonly DependencyProperty MarginProperty =
            DependencyProperty.Register("Margin", typeof(Thickness), typeof(ChartDataLabel),
            new PropertyMetadata(new Thickness().GetThickness(5, 5, 5, 5)));

        /// <summary>
        /// Identifies the <see cref="FontFamily"/> dependency property.
        /// </summary>        
        /// <value>
        /// The identifier for <c>FontFamily</c> dependency property.
        /// </value>
        public static readonly DependencyProperty FontFamilyProperty =
                DependencyProperty.Register("FontFamily", typeof(FontFamily), typeof(ChartDataLabel),
                new PropertyMetadata(new FontFamily("Times New Roman")));

        /// <summary>
        /// Identifies the <see cref="FontStyle"/> dependency property.
        /// </summary>        
        /// <value>
        /// The identifier for <c>FontStyle</c> dependency property.
        /// </value>
        public static readonly DependencyProperty FontStyleProperty =
            DependencyProperty.Register("FontStyle", typeof(FontStyle), typeof(ChartDataLabel), new PropertyMetadata(FontStyle.Normal));

        /// <summary>
        /// Identifies the <see cref="FontSize"/> dependency property.
        /// </summary>        
        /// <value>
        /// The identifier for <c>FontSize</c> dependency property.
        /// </value>
        public static readonly DependencyProperty FontSizeProperty =
            DependencyProperty.Register("FontSize", typeof(double), typeof(ChartDataLabel),
            new PropertyMetadata(0d));

        /// <summary>
        /// Identifies the <see cref="Foreground"/> dependency property.
        /// </summary>        
        /// <value>
        /// The identifier for <c>Foreground</c> dependency property.
        /// </value>
        public static readonly DependencyProperty ForegroundProperty =
            DependencyProperty.Register("Foreground", typeof(Brush), typeof(ChartDataLabel),
            new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="ConnectorRotation"/> dependency property.
        /// </summary>        
        /// <value>
        /// The identifier for <c>ConnectorRotation</c> dependency property.
        /// </value>
        public static readonly DependencyProperty ConnectorRotationProperty =
            DependencyProperty.Register(nameof(ConnectorRotation), typeof(double), typeof(ChartDataLabel),
            new PropertyMetadata(0d));

        /// <summary>
        /// Identifies the <see cref="ConnectorHeight"/> dependency property.
        /// </summary>        
        /// <value>
        /// The identifier for <c>ConnectorHeight</c> dependency property.
        /// </value>
        public static readonly DependencyProperty ConnectorHeightProperty =
            DependencyProperty.Register("ConnectorHeight", typeof(double), typeof(ChartDataLabel),
            new PropertyMetadata(0d));

        #endregion

        #region Fields

        #region Internal Fields

        internal ChartDataMarkerContainer adormentContainer;

        #endregion

        #region Private Fields

        double x;

        double y;

        double xpos, ypos;

        double xData;

        double yData;

        private ActualLabelPosition actualLabelPosition;

        #endregion

        #endregion

        #region Constructor

        /// <summary>
        /// 
        /// </summary>
        public ChartDataLabel()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xVal"></param>
        /// <param name="yVal"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="series"></param>
        public ChartDataLabel(double xVal, double yVal, double x, double y, ChartSeries series)
        {

        }

        #endregion

        #region Properties

        #region Public Properties

        /// <summary>
        /// Gets or sets series property.
        /// </summary>
        /// <value>The <see cref="ChartSeries"/> value.</value>
        internal new ChartSeries Series
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a brush value to customize the appearance of the chart data label.
        /// </summary>
        /// <value>It accepts a <see cref="Brush"/> value and its default value is null.</value>
        public Brush Background
        {
            get { return (Brush)GetValue(BackgroundProperty); }
            set { SetValue(BackgroundProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value to modify the border thickness of the chart data label.
        /// </summary>
        /// <value>It accepts a <see cref="Thickness"/> value and its default value is 0.</value>
        public Thickness BorderThickness
        {
            get { return (Thickness)GetValue(BorderThicknessProperty); }
            set { SetValue(BorderThicknessProperty, value); }
        }

        /// <summary>
        /// Gets or sets a brush value to customize the border appearance of the chart data label.
        /// </summary>
        /// <value>It accepts a <see cref="Brush"/> value and its default value is <b>Transparent</b>.</value>
        public Brush BorderBrush
        {
            get { return (Brush)GetValue(BorderBrushProperty); }
            set { SetValue(BorderBrushProperty, value); }
        }

        /// <summary>
        /// Gets or sets a thickness value to adjust the data label's margin.
        /// </summary>
        /// <value>It accepts the <see cref="Thickness"/> values and the default value is 5.</value>
        public Thickness Margin
        {
            get { return (Thickness)GetValue(MarginProperty); }
            set { SetValue(MarginProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value to customize the font family for the data label.
        /// </summary>
        /// <value>It accepts the <see cref="FontFamily"/> values and the default value is <b>Times New Roman</b>.</value>
        public FontFamily FontFamily
        {
            get { return (FontFamily)GetValue(FontFamilyProperty); }
            set { SetValue(FontFamilyProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value to customize the font style for the data label.
        /// </summary>
        /// <value>It accepts the <see cref="FontStyle"/> values and the default value is <see cref="FontStyle.Normal"/>.</value>
        public FontStyle FontStyle
        {
            get { return (FontStyle)GetValue(FontStyleProperty); }
            set { SetValue(FontStyleProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value to change the data label's text size.
        /// </summary>
        /// <value>It accepts the double value.</value>
        public double FontSize
        {
            get { return (double)GetValue(FontSizeProperty); }
            set { SetValue(FontSizeProperty, value); }
        }

        /// <summary>
        /// Gets or sets a brush value to customize the text color of the data label.
        /// </summary>
        /// <value>It takes the <see cref="Brush"/> value.</value>
        public Brush Foreground
        {
            get { return (Brush)GetValue(ForegroundProperty); }
            set { SetValue(ForegroundProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value to rotate the connector line.
        /// </summary>
        /// <value>It accepts the double value and its default value is 0.</value>
        public double ConnectorRotation
        {
            get { return (double)GetValue(ConnectorRotationProperty); }
            set { SetValue(ConnectorRotationProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value to change the data label's connector line height.
        /// </summary>
        /// <value>It accepts the double value and its default value is 0.</value>
        public double ConnectorHeight
        {
            get { return (double)GetValue(ConnectorHeightProperty); }
            set { SetValue(ConnectorHeightProperty, value); }
        }

        /// <summary>
        /// Gets the content of the data label.
        /// </summary>
        public object Content
        {
            get
            {
                if (Series.adornmentInfo == null) return null;
                string labelFormat = Series.adornmentInfo.Format;
                var content = Series.adornmentInfo.Context;
                //WPF-14375 : segmentLabelContent not working properly, while setting Adornmentinfo labelTemplate and UseSeriesPalette as true
                if (Series.adornmentInfo.ContentTemplate == null)
                {
                    content = LabelContext.DataLabelItem;
                }
                return CalculateLabelContent(content, labelFormat);

            }
        }

        /// <summary>
        /// Gets the or sets a data point value that is bound with x for the data label.
        /// </summary>
        public double XData
        {
            get
            {
                return xData;
            }
            set
            {
                xData = value;
                OnPropertyChanged("XData");
                OnPropertyChanged(nameof(Content));
            }
        }

        /// <summary>
        /// Gets the or sets a data point value that is bound with y for the data label.
        /// </summary>
        public double YData
        {
            get
            {
                return yData;
            }
            set
            {
                yData = value;
                OnPropertyChanged("YData");
                OnPropertyChanged(nameof(Content));
            }
        }

        /// <summary>
        /// Gets the x coordinate relative to the series.
        /// </summary>
        public double X
        {
            get
            {
                return x;
            }
            internal set
            {
                x = value;
            }
        }

        /// <summary>
        /// Gets the y coordinate relative to the series.
        /// </summary>
        public double Y
        {
            get
            {
                return y;
            }
            internal set
            {
                y = value;
            }
        }

        #endregion

        #region Internal Properties

        /// <summary>
        /// Gets or sets contrast foreground or theme base foreground.
        /// XAMARIN-38561 foreground not updated while using datamarker created event. provide majority for <see cref="Foreground"/> and <see cref="ChartDataLabelSettings.Foreground"/>.
        /// </summary>
        internal Brush ContrastForeground { get; set; }

        internal bool CanHideLabel
        {
            get
            {
                return ((YData == 0 && (this.Series is TriangularSeriesBase || this.Series is CircularSeries)) || double.IsNaN(YData));
            }
        }

        internal double XPos
        {
            get
            {
                return xpos;
            }
            set
            {
                xpos = value;
                OnPropertyChanged("XPos");
            }
        }

        internal double YPos
        {
            get
            {
                return ypos;
            }
            set
            {
                ypos = value;
                OnPropertyChanged("YPos");
            }
        }

        internal ActualLabelPosition ActualLabelPosition
        {
            get
            {
                return actualLabelPosition;
            }
            set
            {
                actualLabelPosition = value;
            }
        }

        internal string Label { get; set; }

        internal double GrandTotal { get; set; }

        #endregion

        #endregion

        #region Methods

        #region Public Override Methods

        /// <summary>
        /// Sets the values for this segment. This method is not
        /// intended to be called explicitly outside the Chart but it can be overriden by
        /// any derived class.
        /// </summary>
        /// <param name="Values"></param>
        internal override void SetData(params double[] Values)
        {
            XData = Values[0];
            YData = Values[1];
            XPos = Values[2];
            YPos = Values[3];
        }

        /// <summary>
        /// Used for creating UIElement for rendering this segment. This method is not
        /// intended to be called explicitly outside the Chart but it can be overriden by
        /// any derived class.
        /// </summary>
        /// <param name="size">Size of the panel</param>
        /// <returns>
        /// retuns UIElement
        /// </returns>
        internal override UIElement CreateVisual(Size size)
        {
            if (adormentContainer == null)
                adormentContainer = new ChartDataMarkerContainer(this);

            return adormentContainer;
        }

        /// <summary>
        /// Gets the UIElement used for rendering this segment.
        /// </summary>
        /// <returns>reurns UIElement</returns>

        internal override UIElement GetRenderedVisual()
        {
            return adormentContainer;
        }

        /// <summary>
        /// Updates the segments based on its data point value. This method is not
        /// intended to be called explicitly outside the Chart but it can be overriden by
        /// any derived class.
        /// </summary>
        /// <param name="transformer">Reresents the view port of chart control.(refer <see cref="IChartTransformer"/>)</param>

        internal override void Update(IChartTransformer transformer)
        {
            Point point = transformer.TransformToVisible(XPos, YPos);
            this.X = point.X;
            this.Y = point.Y;
        }

        /// <summary>
        /// Called whenever the segment's size changed. This method is not
        /// intended to be called explicitly outside the Chart but it can be overriden by
        /// any derived class.
        /// </summary>
        /// <param name="size"></param>

        internal override void OnSizeChanged(Size size)
        {

        }

        #endregion

        #region Internal Virtual Methods

        internal virtual ChartDataMarkerContainer GetAdornmentContainer()
        {
            return adormentContainer;
        }

        #endregion

        #region Internal Override Methods

        internal override UIElement CreateSegmentVisual(Size size)
        {
            BindColorProperties();
            return CreateVisual(size);
        }

        #endregion

        #region Internal Methods

        /// <summary>
        /// Updated the text for the adornment label TextBlock.
        /// </summary>      
        internal object GetTextContent()
        {
            if (this.CanHideLabel)
                return "";

            string format = this.Series.adornmentInfo.Format;
            var content = this.Series.adornmentInfo.Context;

            if (content == LabelContext.DataLabelItem)
            {
                content = LabelContext.YValue;
            }

            return this.CalculateLabelContent(content, format);
        }

        internal double CalculateSumOfValues(IList<double> yValues)
        {
            if (yValues != null)
            {
                GrandTotal = (from val in yValues where !double.IsNaN(val) select val).Sum();
            }
            return GrandTotal;
        }

        internal object CalculateLabelContent(LabelContext content, string labelFormat)
        {
            switch (content)
            {
                case LabelContext.XValue:
                    if (this.Series.ActualXValues is List<string> xValues)
                    {
                        return xValues[(int)this.XData].Tostring();
                    }
                    return this.XData.ToString(labelFormat, CultureInfo.CurrentCulture);
                case LabelContext.YValue:
                    if (!double.IsNaN(YData))
                    {
                        labelFormat = string.IsNullOrEmpty(labelFormat) ? "0.##" : labelFormat;//Round off default decimal point to two digits while segment dragging-WRT-4212
                        return this.YData.ToString(labelFormat, CultureInfo.CurrentCulture);
                    }
                    else
                        return "";
                case LabelContext.Percentage:
                    double grandPercentage;

                    grandPercentage = this.Series.GetGrandTotal(this.Series.ActualSeriesYValues[0]);

                    labelFormat = string.IsNullOrEmpty(labelFormat) ? "0.##" : labelFormat; //WP-559 - decimal digits round-off to 2 positions by default
                    return (this.YData / grandPercentage * 100).ToString(labelFormat, CultureInfo.CurrentCulture) + "%";
#if !WinUI
                case LabelContent.YofTot:
                    double grandTotal;

                    grandTotal = (Series is HistogramSeries) ?
                        this.Series.GetGrandTotal((Series as HistogramSeries).ActualYValues) :
                        this.Series.GetGrandTotal(this.Series.ActualSeriesYValues[0]);

                    labelFormat = string.IsNullOrEmpty(labelFormat) ? "0.##" : labelFormat;
                    return this.YData.ToString(labelFormat, CultureInfo.CurrentCulture) + " of " + grandTotal.ToString(labelFormat, CultureInfo.CurrentCulture);
#endif
                case LabelContext.DateTime:
                    object? datetimeContent = null;
                    if (this.Series.IsIndexed)
                    {
                        if (this.Series.ActualXValues is List<double> dateValues)
                            datetimeContent = dateValues[(int)this.XData].FromOADate().ToString(this.Series.adornmentInfo.Format, CultureInfo.CurrentCulture);
                        else
                        {
                            if (this.Series.ActualXValues is List<string> stringValues)
                            {
                                DateTime date = DateTime.MinValue;
                                DateTime.TryParse(stringValues[(int)this.XData], out date);
                                datetimeContent = date.ToString(this.Series.adornmentInfo.Format, CultureInfo.CurrentCulture);
                            }
                        }

                    }
                    else
                    {
                        datetimeContent = this.XData.FromOADate().ToString(this.Series.adornmentInfo.Format,
                            CultureInfo.CurrentCulture);
                    }
                    return datetimeContent;
                case LabelContext.DataLabelItem:

                    return this;
                default:
                    if (!double.IsNaN(YData))
                    {
                        labelFormat = string.IsNullOrEmpty(labelFormat) ? "0.##" : labelFormat;
                        return this.YData.ToString(labelFormat, CultureInfo.CurrentCulture);
                    }
                    else
                        return "";
            }
        }

        internal void BindColorProperties()
        {
            var isGrouping = (Series.ActualXAxis is CategoryAxis categoryAxis) ? categoryAxis.IsIndexed : true;

            Binding binding = new Binding();
            binding.Source = Series;
            binding.Path = new PropertyPath("Fill");
            binding.Converter = new InteriorConverter(Series);
            if (Series is CircularSeries && !double.IsNaN(((CircularSeries)Series).GroupTo))
                binding.ConverterParameter = Series.Adornments.IndexOf(this);
            else if (!isGrouping && Series.IsSideBySide)
                binding.ConverterParameter = Series.GroupedActualData.IndexOf(Item);
            else
                binding.ConverterParameter = Series.ActualData?.IndexOf(Item);
            BindingOperations.SetBinding(this, FillProperty, binding);


            Binding strokeBinding = new Binding();
            strokeBinding.Source = Series;
            strokeBinding.Path = new PropertyPath("Stroke");
            BindingOperations.SetBinding(this, StrokeProperty, strokeBinding);

            strokeBinding = new Binding();
            strokeBinding.Source = Series;
            strokeBinding.Path = new PropertyPath("StrokeWidth");
            BindingOperations.SetBinding(this, StrokeWidthProperty, strokeBinding);
        }

        #endregion

        #endregion
    }

    /// <summary>
    /// Represents chart adornment.
    /// </summary>
    /// <remarks>Class instance is created automatically by WINRT Chart building system.</remarks>
    internal class ChartPieDataLabel : ChartDataLabel
    {
#region Fields

#region Private Fields

        double angle;

        double radius;

        int pieIndex;

        #endregion

        #endregion

        #region Constructor

        /// <summary>
        /// Called when instance created for ChartPieAdornment
        /// </summary>
        /// <param name="xVal"></param>
        /// <param name="yVal"></param>
        /// <param name="angle"></param>
        /// <param name="radius"></param>
        /// <param name="series"></param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1801: Review unused parameters")]
        public ChartPieDataLabel(double xVal, double yVal, double angle, double radius, ChartSeries series)
        {

        }

#endregion

#region Properties

        /// <summary>
        /// Gets or sets Angle property 
        /// </summary>
        public double Angle
        {
            get
            {
                return angle;
            }
            internal set
            {
                angle = value;
                OnPropertyChanged("Angle");
            }
        }

        /// <summary>
        /// Gets or sets Radius property
        /// </summary>
        public double Radius
        {
            get
            {
                return radius;
            }
            internal set
            {
                radius = value;
                OnPropertyChanged("Radius");
            }
        }

        internal double InnerDoughnutRadius
        {
            get; set;
        }

        #endregion

        #region Methods

        #region Public Override Methods

        /// <summary>
        /// Sets the values for this segment. This method is not
        /// intended to be called explicitly outside the Chart but it can be overriden by
        /// any derived class.
        /// </summary>
        /// <param name="Values"></param>
        internal override void SetData(params double[] Values)
        {
            XPos = XData = Values[0];
            YPos = YData = Values[1];
            Angle = Values[2];
            Radius = Values[3];
        }

        /// <summary>
        /// Updates the segments based on its data point value. This method is not
        /// intended to be called explicitly outside the Chart but it can be overriden by
        /// any derived class.
        /// </summary>
        /// <param name="transformer">Reresents the view port of chart control.(refer <see cref="IChartTransformer"/>)</param>
        internal override void Update(IChartTransformer transformer)
        {
            double radius = Radius;
            double actualRadius = Math.Min(transformer.Viewport.Width, transformer.Viewport.Height) / 2;
            
            if (Series is DoughnutSeries doughnutHostSeries)
            {
                actualRadius *= doughnutHostSeries.InternalDoughnutCoefficient;

                Point center;
                double remainingWidth = 0d, equalParts = 0d, innerRadius = 0d;
                var adornmentAngle = Angle;

                if (doughnutHostSeries.IsStackedDoughnut)
                {
                    var adornmentIndex = Series.Adornments.IndexOf(this);
                    var doughnutSegment = doughnutHostSeries.Segments[adornmentIndex] as DoughnutSegment;
                    int doughnutSegmentsCount = doughnutHostSeries.Segments.Count;
                    center = doughnutHostSeries.Center;
                    remainingWidth = actualRadius - (actualRadius * Series.ActualArea.InternalDoughnutHoleSize);
                    equalParts = (remainingWidth / doughnutSegmentsCount) * doughnutHostSeries.InternalDoughnutCoefficient;
                    radius = actualRadius - (equalParts * (doughnutSegmentsCount - (doughnutSegment.DoughnutSegmentIndex + 1)));
                    InnerDoughnutRadius = innerRadius = radius - equalParts;
                    radius = radius - equalParts * doughnutHostSeries.SegmentSpacing;
                    Radius = radius;
                    innerRadius = ChartMath.MaxZero(innerRadius);
                    
                    double difference = (radius - innerRadius) / 2;
                    radius = radius - difference;

                    adornmentAngle = (doughnutHostSeries.DataLabelSettings as CircularDataLabelSettings).Position == CircularSeriesLabelPosition.Outside ? doughnutSegment.StartAngle : (doughnutHostSeries.DataLabelSettings as CircularDataLabelSettings).Position == CircularSeriesLabelPosition.OutsideExtended ? doughnutSegment.EndAngle : Angle;
                }
                else
                {
                    int doughnutSeriesCount = doughnutHostSeries.GetDoughnutSeriesCount();
                    center = doughnutSeriesCount == 1 ? doughnutHostSeries.Center : ChartLayoutUtils.GetCenter(transformer.Viewport);
                    remainingWidth = actualRadius - (actualRadius * Series.ActualArea.InternalDoughnutHoleSize);
                    equalParts = remainingWidth / doughnutSeriesCount;
                    InnerDoughnutRadius = innerRadius = radius - (equalParts * doughnutHostSeries.InternalDoughnutCoefficient);
                    innerRadius = ChartMath.MaxZero(innerRadius);

                    if (doughnutHostSeries != null && (doughnutHostSeries.DataLabelSettings as CircularDataLabelSettings).Position == CircularSeriesLabelPosition.Inside)
                    {
                        double difference = (radius - innerRadius) / 2;
                        radius = radius - difference;
                    }
                }
                
                this.X = center.X + radius * Math.Cos(adornmentAngle);
                this.Y = center.Y + radius * Math.Sin(adornmentAngle);
            }
            else if (Series is PieSeries hostSeries)
            {
                double pieSeriesCount = hostSeries.GetPieSeriesCount();
                double equalParts = actualRadius / pieSeriesCount;
                double innerRadius = equalParts * pieIndex;

                Point center;
                if (pieSeriesCount == 1)
                    center = hostSeries.Center;
                else
                    center = ChartLayoutUtils.GetCenter(transformer.Viewport);

                if (hostSeries != null && (hostSeries.DataLabelSettings as CircularDataLabelSettings).Position == CircularSeriesLabelPosition.Inside)
                {
                    if (pieIndex > 0)
                    {
                        double difference = (radius - innerRadius) / 2;
                        radius = radius - difference;
                    }
                    else
                    {
                        radius = radius / 2;
                    }
                }

                this.X = center.X + radius * Math.Cos(Angle);
                this.Y = center.Y + radius * Math.Sin(Angle);
            }
        }

#endregion

#region Internal Methods

        internal void SetValues(double xVal, double yVal, double angle, double radius, ChartSeries series)
        {
            XPos = XData = xVal;
            YPos = YData = yVal;
            Radius = radius;
            Angle = angle;
            Series = base.Series = series;
            pieIndex = (from pieSeries in Series.ActualArea.VisibleSeries where pieSeries is CircularSeries select pieSeries).ToList().IndexOf(series);
        }

#endregion

#endregion
    }

    /// <summary>
    /// Class implementation for triangularAdornments
    /// </summary>
    internal class TriangularAdornment : ChartDataLabel
    {
#region Fields

#region Private Fields

        double CurrY, Height;

#endregion

#endregion

#region Constructor

        /// <summary>
        /// Called when instance created for TriangularAdornment
        /// </summary>
        /// <param name="xVal"></param>
        /// <param name="yVal"></param>
        /// <param name="currY"></param>
        /// <param name="height"></param>
        /// <param name="series"></param>

        public TriangularAdornment(double xVal, double yVal, double currY, double height, ChartSeries series)
        {
            XPos = XData = xVal;
            YPos = YData = yVal;
            CurrY = currY;
            Height = height;
            Series = base.Series = series;
        }

        #endregion

        #region Methods

        #region Public Override Methods

        /// <summary>
        /// Sets the values for this segment. This method is not
        /// intended to be called explicitly outside the Chart but it can be overriden by
        /// any derived class.
        /// </summary>
        /// <param name="Values"></param>
        internal override void SetData(params double[] Values)
        {
            XPos = XData = Values[0];
            YPos = YData = Values[1];
        }

        /// <summary>
        /// Updates the segments based on its data point value. This method is not
        /// intended to be called explicitly outside the Chart but it can be overriden by
        /// any derived class.
        /// </summary>
        /// <param name="transformer">Reresents the view port of chart control.(refer <see cref="IChartTransformer"/>)</param>

        internal override void Update(IChartTransformer transformer)
        {
            double bottom;
            Point center = ChartLayoutUtils.GetCenter(transformer.Viewport);
            bottom = center.Y;
            center.Y = 0;
            center.Y += ((CurrY * bottom) * 2) - (Height / 2) * 4;
            this.X = center.X;
            this.Y = center.Y;
        }
#endregion

#endregion
    }
}
