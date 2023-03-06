using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using System;

namespace Syncfusion.Maui.Charts
{
    /// <summary>
    /// The <see cref="RadialBarSeries"/> displays data in different categories. Its most commonly used to make comparisons among a set of given data using the circular shapes of the bars.
    /// </summary>
    /// <remarks>
    /// <para>It is similar to the DoughnutSeries. To render a series, create an instance of the radial bar series class, and add it to the <see cref="SfCircularChart.Series"/> collection.</para>
    /// <para>It provides options for <see cref="TrackFill"/>, <see cref="TrackStroke"/>, <see cref="TrackStrokeWidth"/>, <see cref="MaximumValue"/>, <see cref="GapRatio"/>, <see cref="CapStyle"/>, <see cref="CenterView"/>, <see cref="ChartSeries.PaletteBrushes"/>, <see cref="ChartSeries.Fill"/>, <see cref="CircularSeries.Stroke"/>, <see cref="CircularSeries.StrokeWidth"/>, and <see cref="InnerRadius"/> to customize the appearance.</para>
    /// </remarks>
    /// <example>
    /// # [Xaml](#tab/tabid-1)
    /// <code><![CDATA[
    ///     <chart:SfCircularChart>
    ///
    ///           <chart:SfCircularChart.Series>
    ///               <chart:RadialBarSeries
    ///                   ItemsSource="{Binding Data}"
    ///                   XBindingPath="XValue"
    ///                   YBindingPath="YValue"
    ///                   TrackFill = "Red"
    ///                   TrackStroke = "Red"
    ///                   TrackStrokeWidth = 1 />
    ///           </chart:SfCircularChart.Series>
    ///           
    ///     </chart:SfCircularChart>
    /// ]]></code>
    /// # [C#](#tab/tabid-2)
    /// <code><![CDATA[
    ///     SfCircularChart chart = new SfCircularChart();
    ///     
    ///     ViewModel viewModel = new ViewModel();
    /// 
    ///     RadialBarSeries series = new RadialBarSeries();
    ///     series.ItemsSource = viewModel.Data;
    ///     series.XBindingPath = "XValue";
    ///     series.YBindingPath = "YValue";
    ///     chart.Series.Add(series);
    ///     
    /// ]]></code>
    /// # [ViewModel](#tab/tabid-3)
    /// <code><![CDATA[
    ///     public ObservableCollection<Model> Data { get; set; }
    /// 
    ///     public ViewModel()
    ///     {
    ///        Data = new ObservableCollection<Model>();
    ///        Data.Add(new Model() { XValue = 10, YValue = 100 });
    ///        Data.Add(new Model() { XValue = 20, YValue = 150 });
    ///        Data.Add(new Model() { XValue = 30, YValue = 110 });
    ///        Data.Add(new Model() { XValue = 40, YValue = 230 });
    ///     }
    /// ]]></code>
    /// ***
    /// </example>
    /// <seealso cref="RadialBarSegment"/>
    /// <seealso cref="DoughnutSeries"/>
    /// <seealso cref="DoughnutSegment"/>
    public class RadialBarSeries : CircularSeries
    {
        #region Bindable Properties

        /// <summary>
        /// Gets or sets the track fill color that fills the radial bar track behind the segments. This is a bindable property.
        /// </summary>
        public static readonly BindableProperty TrackFillProperty =
            BindableProperty.Create(
            nameof(TrackFill),
            typeof(Brush),
            typeof(RadialBarSeries),
            null,
            BindingMode.Default,
            null,
            OnTrackFillChanged);

        /// <summary>
        /// Gets or sets the track stroke to customize the outer and inner layers of the radial bar track. This is a bindable property.
        /// </summary>
        public static readonly BindableProperty TrackStrokeProperty =
            BindableProperty.Create(
            nameof(TrackStroke),
            typeof(Brush),
            typeof(RadialBarSeries),
            null,
            BindingMode.Default,
            null,
            OnTrackStrokeChanged);

        /// <summary>
        /// Gets or sets the track stroke width to customize the size of the outer and inner layers of the radial bar track. This is a bindable property.
        /// </summary>
        public static readonly BindableProperty TrackStrokeWidthProperty =
            BindableProperty.Create(
            nameof(TrackStrokeWidth),
            typeof(double), typeof(RadialBarSeries),
            0d,
            BindingMode.Default,
            null,
            OnTrackStrokeWidthChanged);

        /// <summary>
        /// Gets or sets the maximum value used to define the span of the segment-filled area in the radial bar track. This is a bindable property.
        /// </summary>
        public static readonly BindableProperty MaximumValueProperty =
            BindableProperty.Create(
            nameof(MaximumValue),
            typeof(double),
            typeof(RadialBarSeries),
            double.NaN,
            BindingMode.Default,
            null,
            OnRadialBarSeriesPropertyChanged);

        /// <summary>
        /// Gets or sets the gap ratio used to determine the spacing between each segment. This is a bindable property.
        /// </summary>
        public static readonly BindableProperty GapRatioProperty =
            BindableProperty.Create(
            nameof(GapRatio),
            typeof(double),
            typeof(RadialBarSeries),
            0.2d,
            BindingMode.Default,
            null,
            OnSpacingPropertyChanged);

        /// <summary>
        /// Gets or sets the center view used to add place view inside the radial bar. This is a bindable property.
        /// </summary>
        public static readonly BindableProperty CenterViewProperty =
            BindableProperty.Create(
            nameof(CenterView),
            typeof(View),
            typeof(RadialBarSeries),
            null,
            BindingMode.Default,
            null,
            OnCenterViewPropertyChanged);

        /// <summary>
        /// Gets or sets the cap style used to customize the start and end points of the segment shapes. This is a bindable property.
        /// </summary>
        public static readonly BindableProperty CapStyleProperty =
            BindableProperty.Create(
            nameof(CapStyle),
            typeof(CapStyle),
            typeof(RadialBarSeries),
            CapStyle.BothFlat,
            BindingMode.Default,
            null,
            OnRadialBarSeriesPropertyChanged);

        /// <summary>
        /// Gets or sets the inner radius used to define the inner circle size of the segments. This is a bindable property.
        /// </summary>        
        public static readonly BindableProperty InnerRadiusProperty =
            BindableProperty.Create(
                nameof(InnerRadius),
                typeof(double),
                typeof(RadialBarSeries),
                0.4d,
                BindingMode.Default,
                null,
                OnInnerRadiusPropertyChanged,
                null,
                coerceValue: CoerceRadialBarCoefficient);
        #endregion

        #region Constructor 

        /// <summary>
        /// Initializes a new instance of the <see cref="RadialBarSeries"/> class.
        /// </summary>
        public RadialBarSeries() : base()
        {
            PaletteBrushes = ChartColorModel.DefaultBrushes;
            TrackFill = new SolidColorBrush(Color.FromRgba(0, 0, 0, 0.08));
            TrackStroke = new SolidColorBrush(Color.FromRgba(0, 0, 0, 0.24));
            StartAngle = -90;
            EndAngle = 270;
        }
        #endregion

        #region Puplic properties

        /// <summary>
        /// Gets or sets the brush value that represents the radial bar's each segment track fill color.
        /// </summary>
        /// <value>It accepts <see cref="Brush"/> values.</value>
        /// <example>
        /// # [Xaml](#tab/tabid-4)
        /// <code><![CDATA[
        ///     <chart:SfCircularChart>
        ///     
        ///         <chart:SfCircularChart.BindingContext>
        ///             <local:ViewModel/>
        ///         </chart:SfCircularChart.BindingContext>
        ///         
        ///         <chart:RadialBarSeries ItemsSource="{Binding Data}"
        ///                            XBindingPath="XValue"
        ///                            YBindingPath="YValue"
        ///                            TrackFill = "Red" />                       
        ///     </chart:SfCircularChart>
        /// ]]></code>
        /// # [C#](#tab/tabid-5)
        /// <code><![CDATA[
        ///     SfCircularChart chart = new SfCircularChart();
        ///     
        ///     ViewModel viewModel = new ViewModel();
        ///     chart.BindingContext = viewModel;
        ///     
        ///     RadialBarSeries series = new RadialBarSeries()
        ///     {
        ///           ItemsSource = viewModel.Data,
        ///           XBindingPath = "XValue",
        ///           YBindingPath = "YValue",
        ///           TrackFill = Brush.Red
        ///     };
        ///     
        ///     chart.Series.Add(series);
        ///
        /// ]]></code>
        /// ***
        /// </example>
        public Brush TrackFill
        {
            get { return (Brush)GetValue(TrackFillProperty); }
            set { SetValue(TrackFillProperty, value); }
        }

        /// <summary>
        /// Gets or sets the brush value that represents the radial bar's each segment track stroke color. 
        /// </summary>
        /// <value>It accepts <see cref="Brush"/> values.</value>
        /// <example>
        /// # [Xaml](#tab/tabid-6)
        /// <code><![CDATA[
        ///     <chart:SfCircularChart>
        ///     
        ///         <chart:SfCircularChart.BindingContext>
        ///             <local:ViewModel/>
        ///         </chart:SfCircularChart.BindingContext>
        ///         
        ///         <chart:RadialBarSeries ItemsSource="{Binding Data}"
        ///                            XBindingPath="XValue"
        ///                            YBindingPath="YValue"
        ///                            TrackStroke = "Red"
        ///                            TrackStrokeWidth = "1"   />                       
        ///     </chart:SfCircularChart>
        /// ]]></code>
        /// # [C#](#tab/tabid-7)
        /// <code><![CDATA[
        ///     SfCircularChart chart = new SfCircularChart();
        ///     
        ///     ViewModel viewModel = new ViewModel();
        ///     chart.BindingContext = viewModel;
        ///     
        ///     RadialBarSeries series = new RadialBarSeries()
        ///     {
        ///           ItemsSource = viewModel.Data,
        ///           XBindingPath = "XValue",
        ///           YBindingPath = "YValue",
        ///           TrackStroke = Brush.Red,
        ///           TrackStrokeWidth = 1
        ///     };
        ///     
        ///     chart.Series.Add(series);
        ///
        /// ]]></code>
        /// ***
        /// </example>
        public Brush TrackStroke
        {
            get { return (Brush)GetValue(TrackStrokeProperty); }
            set { SetValue(TrackStrokeProperty, value); }
        }

        /// <summary>
        /// Gets or sets the double value that represents the radial bar track stroke thickness. 
        /// </summary>
        /// <value>It accepts double values and its default value is 1.</value>
        /// <example>
        /// # [Xaml](#tab/tabid-8)
        /// <code><![CDATA[
        ///     <chart:SfCircularChart>
        ///     
        ///         <chart:SfCircularChart.BindingContext>
        ///             <local:ViewModel/>
        ///         </chart:SfCircularChart.BindingContext>
        ///         
        ///         <chart:RadialBarSeries ItemsSource="{Binding Data}"
        ///                            XBindingPath="XValue"
        ///                            YBindingPath="YValue"
        ///                            TrackStroke = "Red"
        ///                            TrackStrokeWidth = "1"   />                       
        ///     </chart:SfCircularChart>
        /// ]]></code>
        /// # [C#](#tab/tabid-9)
        /// <code><![CDATA[
        ///     SfCircularChart chart = new SfCircularChart();
        ///     
        ///     ViewModel viewModel = new ViewModel();
        ///     chart.BindingContext = viewModel;
        ///     
        ///     RadialBarSeries series = new RadialBarSeries()
        ///     {
        ///           ItemsSource = viewModel.Data,
        ///           XBindingPath = "XValue",
        ///           YBindingPath = "YValue",
        ///           TrackStroke = Brush.Red,
        ///           TrackStrokeWidth = 1
        ///     };
        ///     
        ///     chart.Series.Add(series);
        ///
        /// ]]></code>
        /// ***
        /// </example>
        public double TrackStrokeWidth
        {
            get { return (double)GetValue(TrackStrokeWidthProperty); }
            set { SetValue(TrackStrokeWidthProperty, value); }
        }

        /// <summary>
        /// Gets or sets the span of the each segment that represents the proportion of an individual data value relative to the maximum value. 
        /// </summary>
        /// <remarks>It is used to ensure that the data segments that exceed maximum value will be considered as the maximum value for the chart.</remarks>
        /// <value>It accepts double values and its default value is <see cref="double.NaN"/></value>
        /// <example>
        /// # [Xaml](#tab/tabid-10)
        /// <code><![CDATA[
        ///     <chart:SfCircularChart>
        ///     
        ///         <chart:SfCircularChart.BindingContext>
        ///             <local:ViewModel/>
        ///         </chart:SfCircularChart.BindingContext>
        ///         
        ///         <chart:RadialBarSeries ItemsSource="{Binding Data}"
        ///                            XBindingPath="XValue"
        ///                            YBindingPath="YValue"
        ///                            MaximumValue = "100"   />                       
        ///     </chart:SfCircularChart>
        /// ]]></code>
        /// # [C#](#tab/tabid-11)
        /// <code><![CDATA[
        ///     SfCircularChart chart = new SfCircularChart();
        ///     
        ///     ViewModel viewModel = new ViewModel();
        ///     chart.BindingContext = viewModel;
        ///     
        ///     RadialBarSeries series = new RadialBarSeries()
        ///     {
        ///           ItemsSource = viewModel.Data,
        ///           XBindingPath = "XValue",
        ///           YBindingPath = "YValue",
        ///           MaximumValue = 100;
        ///     };
        ///     
        ///     chart.Series.Add(series);
        ///
        /// ]]></code>
        /// ***
        /// </example>
        public double MaximumValue
        {
            get { return (double)GetValue(MaximumValueProperty); }
            set { SetValue(MaximumValueProperty, value); }
        }

        /// <summary>
        /// Gets or sets the gap ratio that is used to define the distance between the each circular bar. 
        /// </summary>
        /// <value>It accepts double values, and the default value is 0.2. Here, the value is between 0 and 1.</value>
        /// <example>
        /// # [Xaml](#tab/tabid-12)
        /// <code><![CDATA[
        ///     <chart:SfCircularChart>
        ///     
        ///         <chart:SfCircularChart.BindingContext>
        ///             <local:ViewModel/>
        ///         </chart:SfCircularChart.BindingContext>
        ///         
        ///         <chart:RadialBarSeries ItemsSource="{Binding Data}"
        ///                            XBindingPath="XValue"
        ///                            YBindingPath="YValue"
        ///                            GapRatio = "0.3" />                       
        ///     </chart:SfCircularChart>
        /// ]]></code>
        /// # [C#](#tab/tabid-13)
        /// <code><![CDATA[
        ///     SfCircularChart chart = new SfCircularChart();
        ///     
        ///     ViewModel viewModel = new ViewModel();
        ///     chart.BindingContext = viewModel;
        ///     
        ///     RadialBarSeries series = new RadialBarSeries()
        ///     {
        ///           ItemsSource = viewModel.Data,
        ///           XBindingPath = "XValue",
        ///           YBindingPath = "YValue",
        ///           GapRatio = 0.3
        ///     };
        ///     
        ///     chart.Series.Add(series);
        ///
        /// ]]></code>
        /// ***
        /// </example>
        public double GapRatio
        {
            get { return (double)GetValue(GapRatioProperty); }
            set { SetValue(GapRatioProperty, value); }
        }

        /// <summary>
        /// Gets or sets the view that added to the center of the radial bar.
        /// </summary>
        /// <value>It accepts any view, and the default value is null.</value> 
        /// <para>Code example for the radial bar center view.</para>
        /// <example>
        /// # [MainPage.xaml](#tab/tabid-14)
        /// <code><![CDATA[
        /// <chart:SfCircularChart>
        /// 
        ///         <chart:SfCircularChart.BindingContext>
        ///             <local:ViewModel/>
        ///         </chart:SfCircularChart.BindingContext>  
        /// 
        ///         <chart:SfCircularChart.Series>
        ///             <chart:RadialBarSeries ItemsSource="{Binding Data}" 
        ///                                    XBindingPath="XValue" 
        ///                                    YBindingPath="YValue"/>
        ///                  <chart:DoughnutSeries.CenterView>
        ///                         <Label Text = "CenterView"/>
        ///                 </chart:RadialBarSeries.CenterView>
        ///         </chart:SfCircularChart.Series>
        /// 
        /// </chart:SfCircularChart>
        /// ]]>
        /// </code>
        /// 
        /// # [MainPage.xaml.cs](#tab/tabid-15)
        /// <code><![CDATA[
        ///  SfCircularChart chart = new SfCircularChart();
        ///  
        ///  ViewModel viewModel = new ViewModel();
        ///	 chart.BindingContext = viewModel;
        ///  
        ///  RadialBarSeries series = new RadialBarSeries()
        ///  {
        ///     ItemsSource = viewmodel.Data,
        ///     XBindingPath = "XValue",
        ///     YBindingPath = "YValue",
        ///  };
        ///  
        ///  Label label = new Label();
        ///  label.Text = "CenterView"
        ///  series.CenterView = label;
        ///  chart.Series.Add(series);
        /// ]]>
        /// </code>
        /// ***
        /// </example>
        public View CenterView
        {
            get { return (View)GetValue(CenterViewProperty); }
            set { SetValue(CenterViewProperty, value); }
        }

        /// <summary>
        /// Gets or sets the enum <see cref="CapStyle"/> value, that represents the radial segment corner shape.
        /// </summary>
        /// <value>The default value of the cap style property is <see cref="CapStyle.BothFlat"/>.</value>
        /// <example>
        /// # [Xaml](#tab/tabid-16)
        /// <code><![CDATA[
        ///     <chart:SfCircularChart>
        ///     
        ///         <chart:SfCircularChart.BindingContext>
        ///             <local:ViewModel/>
        ///         </chart:SfCircularChart.BindingContext>
        ///         
        ///         <chart:RadialBarSeries ItemsSource="{Binding Data}"
        ///                            XBindingPath="XValue"
        ///                            YBindingPath="YValue"
        ///                            CapStyle = "BothCurve"  />                       
        ///     </chart:SfCircularChart>
        /// ]]></code>
        /// # [C#](#tab/tabid-17)
        /// <code><![CDATA[
        ///     SfCircularChart chart = new SfCircularChart();
        ///     
        ///     ViewModel viewModel = new ViewModel();
        ///     chart.BindingContext = viewModel;
        ///     
        ///     RadialBarSeries series = new RadialBarSeries()
        ///     {
        ///           ItemsSource = viewModel.Data,
        ///           XBindingPath = "XValue",
        ///           YBindingPath = "YValue",
        ///           CapStyle = CapStyle.BothCurve
        ///     };
        ///     
        ///     chart.Series.Add(series);
        ///
        /// ]]></code>
        /// ***
        /// </example>
        public CapStyle CapStyle
        {
            get { return (CapStyle)GetValue(CapStyleProperty); }
            set { SetValue(CapStyleProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value that can be used to define the size of the inner circle.
        /// </summary>
        /// <value>It accepts double values, and the default value is 0.4. Here, the value is between 0 and 1.</value>
        /// <example>
        /// # [Xaml](#tab/tabid-18)
        /// <code><![CDATA[
        ///     <chart:SfCircularChart>
        ///     
        ///         <chart:SfCircularChart.BindingContext>
        ///             <local:ViewModel/>
        ///         </chart:SfCircularChart.BindingContext>
        ///         
        ///         <chart:RadialBarSeries ItemsSource="{Binding Data}"
        ///                            XBindingPath="XValue"
        ///                            YBindingPath="YValue"
        ///                            InnerRadius = "0.2" />                       
        ///     </chart:SfCircularChart>
        /// ]]></code>
        /// # [C#](#tab/tabid-19)
        /// <code><![CDATA[
        ///     SfCircularChart chart = new SfCircularChart();
        ///     
        ///     ViewModel viewModel = new ViewModel();
        ///     chart.BindingContext = viewModel;
        ///     
        ///     RadialBarSeries series = new RadialBarSeries()
        ///     {
        ///           ItemsSource = viewModel.Data,
        ///           XBindingPath = "XValue",
        ///           YBindingPath = "YValue",
        ///           InnerRadius = 0.2
        ///     };
        ///     
        ///     chart.Series.Add(series);
        ///
        /// ]]></code>
        /// ***
        /// </example>
        public double InnerRadius
        {
            get { return (double)GetValue(InnerRadiusProperty); }
            set { SetValue(InnerRadiusProperty, value); }
        }

        /// <summary>
        /// Gets the size of the radial bar center hole.
        /// </summary>
        /// <value>Default value is 1.</value>
        /// 
        /// <para>It used to customize the view size which is placed in the radial bar <see cref="CenterView"/>.</para>
        public double CenterHoleSize
        {
            get { return centerHoleSize; }
            internal set 
            {
                if (value >= 1)
                {
                    centerHoleSize = value;
                }

                OnPropertyChanged(nameof(CenterHoleSize));
            }
        }

        #endregion

        #region Fields
        internal float yValue;
        double total = 0;
        float startAngle;
        float endAngle;
        double angleDifference;
        double centerHoleSize = 1;
        #endregion

        #region Methods

        #region Internal Methods

        /// <inheritdoc />
        protected override ChartSegment CreateSegment()
        {
            return new RadialBarSegment();
        }

        /// <inheritdoc />
        internal override void GenerateSegments(SeriesView seriesView)
        {
            GetActualXValues();

            if (YValues != null && ActualData != null && YValues.Count > 0)
            {
                startAngle = (float)StartAngle;
                int visibleSegmentCount = 0;
                angleDifference = GetAngleDifference();
                total = !double.IsNaN(MaximumValue) ? MaximumValue : CalculateTotalYValues();
                for (int i = 0; i < YValues.Count; i++)
                {
                    yValue = (float)YValues[i];
                    endAngle = (float)(Math.Abs(float.IsNaN(yValue) ? 0 : yValue >= total ? total : yValue) * (angleDifference / total));
                   
                    if (i < Segments.Count && Segments[i] is RadialBarSegment)
                    {
                        ((RadialBarSegment)Segments[i]).SetData(startAngle, endAngle);
                    }
                    else
                    {
                        RadialBarSegment radialBarSegment = (RadialBarSegment)CreateSegment();
                        radialBarSegment.Series = this;
                        radialBarSegment.Index = visibleSegmentCount;
                        radialBarSegment.Item = ActualData[i];
                        radialBarSegment.SetData(startAngle, endAngle);
                        radialBarSegment.SeriesView = seriesView;
                        Segments.Add(radialBarSegment);
                    }

                    if (!double.IsNaN(YValues[i]))
                    {
                        visibleSegmentCount++;
                    }
                }
            }
        }

        /// <inheritdoc />
        internal override void OnDetachedToChart()
        {
            RemoveCenterView(CenterView);
        }

        /// <inheritdoc />
        internal override void OnAttachedToChart()
        {
            AddCenterView();
        }
       
        internal void UpdateTrackProperties(Object property)
        {
            for (int i = 0; i < Segments.Count; i++)
            {
                var segment = (RadialBarSegment)Segments[i];
                switch(property)
                {
                    case double:
                        {
                            segment.TrackStrokeWidth = (float)TrackStrokeWidth;
                            break;
                        }
                    case Brush:
                        {
                             if(TrackFill != segment.TrackFill)
                            {
                                segment.TrackFill = TrackFill;
                            }
                            else if(TrackStroke != segment.TrackStroke)
                            {
                                segment.TrackStroke = TrackStroke;
                            }
                            break;
                        }
                }
            }
        }

        /// <inheritdoc />
        internal override void OnSeriesLayout()
        {
            CenterHoleSize = GetCenterHoleSize();
            UpdateCenterViewBounds(CenterView);
            base.OnSeriesLayout();
        }

        /// <inheritdoc />
        internal override TooltipInfo? GetTooltipInfo(ChartTooltipBehavior tooltipBehavior, float x, float y)
        {
            if (AreaBounds == Rect.Zero) return null;

            int index = GetDataPointIndex(x, y);

            if (index < 0 || ActualData == null || YValues == null)
            {
                return null;
            }

            object dataPoint = ActualData[index];
            double yValue = YValues[index];
            var radialBarSegment = Segments[index] as RadialBarSegment;

            if (radialBarSegment == null) return null;

            float segmentRadius = GetTooltipRadius(radialBarSegment);
            PointF center = Center;
            double midAngle = (radialBarSegment.StartAngle + (radialBarSegment.EndAngle / 2)) * 0.0174532925f;
            float xPosition = (float)(center.X + (Math.Cos(midAngle) * segmentRadius));
            float yPosition = (float)(center.Y + (Math.Sin(midAngle) * segmentRadius));

            TooltipInfo tooltipInfo = new TooltipInfo(this);
            tooltipInfo.X = xPosition;
            tooltipInfo.Y = yPosition;
            tooltipInfo.Index = index;
            tooltipInfo.Margin = tooltipBehavior.Margin;
            tooltipInfo.TextColor = tooltipBehavior.TextColor;
            tooltipInfo.FontFamily = tooltipBehavior.FontFamily;
            tooltipInfo.FontSize = tooltipBehavior.FontSize;
            tooltipInfo.FontAttributes = tooltipBehavior.FontAttributes;
            tooltipInfo.Background = tooltipBehavior.Background;
            tooltipInfo.Text = yValue.ToString("#.##");
            tooltipInfo.Item = dataPoint;
            return tooltipInfo;
        }

        internal float GetTooltipRadius(RadialBarSegment radialBarSegment)
        {
            return (float)(radialBarSegment.InnerRingRadius + radialBarSegment.OuterRingRadius) / 2;
        }

        #endregion

        #region Private CallBack Methods

        private static void OnTrackFillChanged(BindableObject bindable, Object oldValue, Object newValue)
        {
            if (bindable is RadialBarSeries radialBarSeries)
            {
                radialBarSeries.UpdateTrackProperties(newValue);
                radialBarSeries.ScheduleUpdateChart();
            }
        }

        private static void OnTrackStrokeChanged(BindableObject bindable, Object oldValue, Object newValue)
        {
            if (bindable is RadialBarSeries series)
            {
                series.UpdateTrackProperties(newValue);
                series.InvalidateSeries();
            }
        }

        private static void OnTrackStrokeWidthChanged(BindableObject bindable, Object oldValue, Object newValue)
        {
            if (bindable is RadialBarSeries series)
            {
                series.UpdateTrackProperties(newValue);
                series.InvalidateSeries();
            }
        }

        private static void OnRadialBarSeriesPropertyChanged(BindableObject bindable, Object oldValue, Object newValue)
        {
            if (bindable is RadialBarSeries series)
            {
                series.SegmentsCreated = false;
                series.ScheduleUpdateChart();
            }
        }

        private static void OnSpacingPropertyChanged(BindableObject bindable, Object oldValue, Object newValue)
        {
            if (bindable is RadialBarSeries series)
            {
                series.ScheduleUpdateChart();
            }
        }

        private static void OnCenterViewPropertyChanged(BindableObject bindable, Object oldValue, Object newValue)
        {
            if (bindable is RadialBarSeries series)
            {
                if (oldValue is View oldView)
                {
                    series.RemoveCenterView(oldView);
                }

                if (newValue is View newView)
                {
                    series.AddCenterView();
                    if(series.Chart != null)
                    {
                        series.UpdateCenterViewBounds(newView);
                    }
                }
            }
        }

        private static void OnInnerRadiusPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (oldValue == newValue) return;
          
            if (bindable is RadialBarSeries series)
            {
                series.ScheduleUpdateChart();
            }
        }

        static object CoerceRadialBarCoefficient(BindableObject bindable, object value)
        {
            double coefficient = Convert.ToDouble(value);
            return coefficient > 1 ? 1 : coefficient < 0 ? 0 : value;
        }
        #endregion

        #region Private methods

        private void AddCenterView()
        {
            var plotArea = ChartArea?.PlotArea as ChartPlotArea;
           
            if (plotArea != null && CenterView != null )
            {
                CenterView.BindingContext = this;
                plotArea.Add(CenterView);
            }
        }

        private void RemoveCenterView(View oldView)
        {
            var plotArea = ChartArea?.PlotArea as ChartPlotArea;
            if (plotArea != null && plotArea.Contains(oldView))
            {
                oldView.RemoveBinding(AbsoluteLayout.LayoutBoundsProperty);
                oldView.RemoveBinding(AbsoluteLayout.LayoutFlagsProperty);
                SetInheritedBindingContext(oldView, null);
                plotArea.Remove(oldView);
            }
        }

        private double GetCenterHoleSize()
        {
            var actualBounds = GetActualBound();
            return (float)InnerRadius * Math.Min(actualBounds.Width, actualBounds.Height);
        }

        private RectF GetActualBound()
        {
            if (AreaBounds != Rect.Zero)
            {
                float minScale = (float)(Math.Min(AreaBounds.Width, AreaBounds.Height) * Radius);
                return new RectF(((Center.X * 2) - minScale) / 2, ((Center.Y * 2) - minScale) / 2, minScale, minScale);
            }

            return default(RectF);
        }

        #endregion

        #endregion
    }
}
