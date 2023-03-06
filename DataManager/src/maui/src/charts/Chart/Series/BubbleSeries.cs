using System;
using Microsoft.Maui.Controls;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui.Graphics;
using System.Collections;
using System.Data;

namespace Syncfusion.Maui.Charts
{
    /// <summary>
    /// The <see cref="BubbleSeries"/> displays a collection of data points represented by a bubble of diffrent size.
    /// </summary>
    /// <remarks>
    /// <para>To render a series, create an instance of <see cref="BubbleSeries"/> class, and add it to the <see cref="SfCartesianChart.Series"/> collection.</para>
    /// 
    /// <para>It provides options for <see cref="ChartSeries.Fill"/>, <see cref="ChartSeries.PaletteBrushes"/>, <see cref="XYDataSeries.StrokeWidth"/>, <see cref="Stroke"/>, and <see cref="ChartSeries.Opacity"/> to customize the appearance.</para>
    /// 
    /// <para> <b>MaximumRadius - </b> Specifies the maximum radius to the bubble series.<see cref="MaximumRadius"/>property.</para>
    /// <para> <b>MinimumRadius - </b> Specifies the minimum radius to the bubble series.<see cref="MinimumRadius"/>property.</para>
    /// <para> <b>SizeValuePath - </b> Specify the bubble size using the <see cref="SizeValuePath"/>property.</para>
    /// <para> <b>ShowZeroSizeBubbles - </b> Specifies the option to show zero size bubble, when its true the zero size bubble render with minimum radius. <see cref="ShowZeroSizeBubbles"/>property.</para>
    /// <para> <b>EnableTooltip - </b> A tooltip displays information while tapping or mouse hovering above a segment. To display the tooltip on a chart, you need to set the <see cref="ChartSeries.EnableTooltip"/> property as <b>true</b> in <see cref="BubbleSeries"/> class, and also refer <seealso cref="ChartBase.TooltipBehavior"/> property.</para>
    /// <para> <b>Data Label - </b> Data labels are used to display values related to a chart segment. To render the data labels, you need to set the <see cref="ChartSeries.ShowDataLabels"/> property as <b>true</b> in <see cref="BubbleSeries"/> class. To customize the chart data labels alignment, placement, and label styles, you need to create an instance of <see cref="CartesianDataLabelSettings"/> and set to the <see cref="CartesianSeries.DataLabelSettings"/> property.</para>
    /// <para> <b>Animation - </b> To animate the series, set <b>True</b> to the <see cref="ChartSeries.EnableAnimation"/> property.</para>
    /// <para> <b>LegendIcon - </b> To customize the legend icon using the <see cref="ChartSeries.LegendIcon"/> property.</para>
    /// </remarks>
    /// <example>
    /// # [Xaml](#tab/tabid-1)
    /// <code><![CDATA[
    ///     <chart:SfCartesianChart>
    ///
    ///           <chart:SfCartesianChart.XAxes>
    ///               <chart:NumericalAxis/>
    ///           </chart:SfCartesianChart.XAxes>
    ///
    ///           <chart:SfCartesianChart.YAxes>
    ///               <chart:NumericalAxis/>
    ///           </chart:SfCartesianChart.YAxes>
    ///
    ///           <chart:SfCartesianChart.Series>
    ///               <chart:BubbleSeries
    ///                   ItemsSource="{Binding Data}"
    ///                   XBindingPath="XValue"
    ///                   YBindingPath="YValue"
    ///                   SizeValuePath="Size"/>
    ///           </chart:SfCartesianChart.Series>  
    ///           
    ///     </chart:SfCartesianChart>
    /// ]]></code>
    /// # [C#](#tab/tabid-2)
    /// <code><![CDATA[
    ///     SfCartesianChart chart = new SfCartesianChart();
    ///     
    ///     NumericalAxis xAxis = new NumericalAxis();
    ///     NumericalAxis yAxis = new NumericalAxis();
    ///     
    ///     chart.XAxes.Add(xAxis);
    ///     chart.YAxes.Add(yAxis);
    ///     
    ///     ViewModel viewModel = new ViewModel();
    /// 
    ///     BubbleSeries series = new BubbleSeries();
    ///     series.ItemsSource = viewModel.Data;
    ///     series.XBindingPath = "XValue";
    ///     series.YBindingPath = "YValue";
    ///     series.SizeValuePath = "Size";
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
    ///        Data.Add(new Model() { XValue = 10, YValue = 100, Size = 10 });
    ///        Data.Add(new Model() { XValue = 20, YValue = 150, Size = 50 });
    ///        Data.Add(new Model() { XValue = 30, YValue = 110, Size = 20 });
    ///        Data.Add(new Model() { XValue = 40, YValue = 230, Size = 60 });
    ///     }
    /// ]]></code>
    /// ***
    /// </example>
    /// <seealso cref="BubbleSegment"/>
    public partial class BubbleSeries : XYDataSeries
    {

        #region Bindable Properties

        /// <summary>
        /// Identifies the <see cref="MaximumRadius"/> bindable property.
        /// </summary>        
        public static readonly BindableProperty MaximumRadiusProperty = 
            BindableProperty.Create(nameof(MaximumRadius), typeof(double), typeof(BubbleSeries), 10d, 
                BindingMode.Default, null, OnMaximumRadiusChanged);

        /// <summary>
        /// Identifies the <see cref="MinimumRadius"/> bindable property.
        /// </summary>        
        public static readonly BindableProperty MinimumRadiusProperty =
            BindableProperty.Create(nameof(MinimumRadius), typeof(double), typeof(BubbleSeries), 3d, 
                BindingMode.Default, null, OnMinimumRadiusChanged);

        /// <summary>
        /// Identifies the <see cref="Stroke"/> bindable property.
        /// </summary>        
        public static readonly BindableProperty StrokeProperty = 
            BindableProperty.Create(nameof(Stroke), typeof(Brush), typeof(BubbleSeries), SolidColorBrush.Transparent, 
                BindingMode.Default, null, OnStrokeChanged);

        /// <summary>
        /// Identifies the <see cref="SizeValuePath"/> bindable property.
        /// </summary>
        public static readonly BindableProperty SizeValuePathProperty =
            BindableProperty.Create(nameof(SizeValuePath), typeof(string), typeof(BubbleSeries), string.Empty,
                BindingMode.Default, null, OnSizeValuePathChanged);


        ///<summary>
        /// Identifies the <see cref="ShowZeroSizeBubbles"/> bindable property.
        ///</summary>
        public static readonly BindableProperty ShowZeroSizeBubblesProperty = 
            BindableProperty.Create(nameof(ShowZeroSizeBubbles), typeof(bool), typeof(BubbleSeries), true,
                BindingMode.Default, null, OnShowZeroSizeBubblesChanged);


        #endregion

        #region  Properties

        #region Public Properties

        /// <summary>
        /// Gets or sets a value to customize the border appearance of the bubble.
        /// </summary>
        /// <value>It accepts <see cref="Brush"/> and its default is null.</value>
        /// <example>
        /// # [Xaml](#tab/tabid-28)
        /// <code><![CDATA[
        ///     <chart:SfCartesianChart>
        ///
        ///     <!-- ... Eliminated for simplicity-->
        ///
        ///          <chart:BubbleSeries ItemsSource="{Binding Data}"
        ///                               XBindingPath="XValue"
        ///                               YBindingPath="YValue"
        ///                               SizeValuePath="Size"
        ///                               Stroke ="Red" />
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
        ///     BubbleSeries series = new BubbleSeries()
        ///     {
        ///           ItemsSource = viewModel.Data,
        ///           XBindingPath = "XValue",
        ///           YBindingPath = "YValue",
        ///           SizeValuePath = "Size",
        ///           Stroke = new SolidColorBrush(Colors.Red)
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
        /// Gets or sets a path value on the source object to serve a size to the bubble series.
        /// </summary>
        /// <value>It accepts <see cref="String"/> and its default is <c>String.Empty</c>.</value>
        /// <example>
        /// # [Xaml](#tab/tabid-28)
        /// <code><![CDATA[
        ///     <chart:SfCartesianChart>
        ///
        ///     <!-- ... Eliminated for simplicity-->
        ///
        ///          <chart:BubbleSeries ItemsSource="{Binding Data}"
        ///                               XBindingPath="XValue"
        ///                               YBindingPath="YValue"
        ///                               SizeValuePath="Size"/>
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
        ///     BubbleSeries series = new BubbleSeries()
        ///     {
        ///           ItemsSource = viewModel.Data,
        ///           XBindingPath = "XValue",
        ///           YBindingPath = "YValue",
        ///           SizeValuePath = "Size",
        ///     };
        ///     
        ///     chart.Series.Add(series);
        ///
        /// ]]>
        /// </code>
        /// ***
        /// </example>
        public string SizeValuePath
        {
            get { return (string)GetValue(SizeValuePathProperty); }
            set { SetValue(SizeValuePathProperty, value); }
        }

        /// <summary>
        /// Gets or sets maximum radius to the bubble series.  
        /// </summary>
        /// <value>It accepts <see cref="double"/> and its default is 10.</value>
        /// <example>
        /// # [Xaml](#tab/tabid-28)
        /// <code><![CDATA[
        ///     <chart:SfCartesianChart>
        ///
        ///     <!-- ... Eliminated for simplicity-->
        ///
        ///          <chart:BubbleSeries ItemsSource="{Binding Data}"
        ///                               XBindingPath="XValue"
        ///                               YBindingPath="YValue"
        ///                               SizeValuePath="Size"
        ///                               MaximumRadius="15" />
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
        ///     BubbleSeries series = new BubbleSeries()
        ///     {
        ///           ItemsSource = viewModel.Data,
        ///           XBindingPath = "XValue",
        ///           YBindingPath = "YValue",
        ///           SizeValuePath = "Size",
        ///           MaximumRadius = 15,
        ///     };
        ///     
        ///     chart.Series.Add(series);
        ///
        /// ]]>
        /// </code>
        /// ***
        /// </example>
        public double MaximumRadius
        {
            get { return (double)GetValue(MaximumRadiusProperty); }
            set { SetValue(MaximumRadiusProperty, value); }
        }

        /// <summary>
        /// Gets or sets minimum radius to the bubble series.  
        /// </summary>
        /// <value>It accepts <see cref="double"/> and its default is 3.</value>
        /// <example>
        /// # [Xaml](#tab/tabid-28)
        /// <code><![CDATA[
        ///     <chart:SfCartesianChart>
        ///
        ///     <!-- ... Eliminated for simplicity-->
        ///
        ///          <chart:BubbleSeries ItemsSource="{Binding Data}"
        ///                               XBindingPath="XValue"
        ///                               YBindingPath="YValue"
        ///                               SizeValuePath="Size"
        ///                               MinimumRadius="5" />
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
        ///     BubbleSeries series = new BubbleSeries()
        ///     {
        ///           ItemsSource = viewModel.Data,
        ///           XBindingPath = "XValue",
        ///           YBindingPath = "YValue",
        ///           SizeValuePath = "Size",
        ///           MaximumRadius = 5,
        ///     };
        ///     
        ///     chart.Series.Add(series);
        ///
        /// ]]>
        /// </code>
        /// ***
        /// </example>
        public double MinimumRadius
        {
            get { return (double)GetValue(MinimumRadiusProperty); }
            set { SetValue(MinimumRadiusProperty, value); }
        }

        /// <summary>
        /// Gets or sets the option to show zero size bubble, when its true the zero size bubble render with minimum radius.  
        /// </summary>
        /// <value>It accepts <see cref="bool"/> and its default is true.</value>
        /// <example>
        /// # [Xaml](#tab/tabid-28)
        /// <code><![CDATA[
        ///     <chart:SfCartesianChart>
        ///
        ///     <!-- ... Eliminated for simplicity-->
        ///
        ///          <chart:BubbleSeries ItemsSource="{Binding Data}"
        ///                               XBindingPath="XValue"
        ///                               YBindingPath="YValue"
        ///                               SizeValuePath="Size"
        ///                               ShowZeroSizeBubbles="True"/>
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
        ///     BubbleSeries series = new BubbleSeries()
        ///     {
        ///           ItemsSource = viewModel.Data,
        ///           XBindingPath = "XValue",
        ///           YBindingPath = "YValue",
        ///           SizeValuePath = "Size",
        ///           ShowZeroSizeBubbles = "True",
        ///     };
        ///     
        ///     chart.Series.Add(series);
        ///
        /// ]]>
        /// </code>
        /// ***
        /// </example>
        public bool ShowZeroSizeBubbles
        {
            get { return (bool)GetValue(ShowZeroSizeBubblesProperty); }
            set { SetValue(ShowZeroSizeBubblesProperty, value); }
        }

        #endregion

        #region Internal Overrided Properties
        internal override bool IsMultipleYPathRequired => true;
        internal List<double> SizeValues { get; set; }

        #endregion

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="BubbleSeries"/> class.
        /// </summary>
        public BubbleSeries() : base()
        {
            SizeValues = new List<double>();
        }

        #endregion

        #region Methods

        #region Protected Override Methods

        /// <inheritdoc/>
        protected override ChartSegment? CreateSegment()
        {
            return new BubbleSegment();
        }

        #endregion

        #region Internal Methods

        /// <inheritdoc/>
        internal override void GenerateSegments(SeriesView seriesView)
        {
            var xValues = GetXValues();
            
            if (YDataPaths.Contains(this.SizeValuePath))
            {
                int index = YDataPaths.IndexOf(this.SizeValuePath);
                this.SizeValues = YDoubleList[index];
            }

            if (xValues == null || xValues.Count == 0 || SizeValues.Count == 0 || YValues.Count == 0)
            {
                return;
            }

            double maximumSizeValue = SizeValues[0];

            double segmentRadius;

            for (int i = 0; i < SizeValues.Count; i++)
            {
                if (SizeValues[i] > maximumSizeValue)
                {
                    maximumSizeValue = SizeValues[i];
                }
            }

            var radius = MaximumRadius - MinimumRadius;

            for (int i = 0; i < PointsCount; i++)
            {
                var bubbleSize = SizeValues[i];
                double relativeSize = radius * (Math.Abs(bubbleSize) / maximumSizeValue);
                segmentRadius = !ShowZeroSizeBubbles && bubbleSize == 0d ? 0d : MinimumRadius +
                    (double.IsNaN(relativeSize) ? 0 : relativeSize);

                if (i < Segments.Count)
                {
                    Segments[i].SetData(new[] { xValues[i], YValues[i], bubbleSize, segmentRadius });
                }
                else
                {
                    CreateSegment(i, xValues, bubbleSize, segmentRadius);
                }
            }
        }

        private void CreateSegment(int i, List<double> xValues, double bubbleSize, double segmentRadius)
        {
            var segment = CreateSegment() as BubbleSegment;
            if (segment == null) return;
            segment.Series = this;
            segment.Index = i;
            segment.SetData(new[] { xValues[i], YValues[i], bubbleSize, segmentRadius });
            Segments.Add(segment);
            
            if (OldSegments != null && OldSegments.Count > 0 && OldSegments.Count > i && OldSegments[i] is BubbleSegment oldSegment)
            {
                segment.SetPreviousData(new[] { oldSegment.CenterX, oldSegment.CenterY, oldSegment.Radius });
            }
        }

        internal override void SetTooltipTargetRect(TooltipInfo tooltipInfo, Rect seriesBounds)
        {
            if (Chart == null) return;

            if (Segments[tooltipInfo.Index] is BubbleSegment bubbleSegment)
            {
                RectF targetRect = bubbleSegment.SegmentBounds;

                float xPosition = targetRect.X;
                float yPosition = targetRect.Y;
                float height = targetRect.Height;
                float width = targetRect.Width;

                if ((xPosition + width / 2 + seriesBounds.Left) == seriesBounds.Left)
                {
                    targetRect = new Rect(xPosition + width / 2, yPosition, width / 2, height);
                    tooltipInfo.Position = Core.TooltipPosition.Right;
                }
                else if ((xPosition + width / 2) == seriesBounds.Width)
                {
                    targetRect = new Rect(xPosition, yPosition, width, height);
                    tooltipInfo.Position = Core.TooltipPosition.Left;
                }

                tooltipInfo.TargetRect = targetRect;
            }
        }

        internal override void SetStrokeColor(ChartSegment segment)
        {
            segment.Stroke = Stroke;
        }

        internal override PointF GetDataLabelPosition(ChartSegment dataLabel, SizeF labelSize, PointF labelPosition, float padding)
        {
            if (dataLabel is BubbleSegment label)
            {
                return DataLabelSettings.GetLabelPositionForSeries(this, labelSize, labelPosition, padding, new Size(label.Radius, label.Radius * 2));
            }

            return PointF.Zero;
        }

        #endregion

        #endregion

        #region Callback Methods

        private static void OnMaximumRadiusChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is BubbleSeries series)
            {
                series.SegmentsCreated = false;
                series.ScheduleUpdateChart();
            }
        }

        private static void OnMinimumRadiusChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is BubbleSeries series)
            {
                series.SegmentsCreated = false;
                series.ScheduleUpdateChart();
            }
        }

        private static void OnStrokeChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is BubbleSeries series)
            {
                series.UpdateStrokeColor();
                series.InvalidateSeries();
            }
        }
        private static void OnSizeValuePathChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is BubbleSeries series)
            {
                if (bindable is XYDataSeries xyDataSeries)
                {
                    OnYBindingPathChanged(bindable, oldValue, newValue);
                }
            }
        }

        private static void OnShowZeroSizeBubblesChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is BubbleSeries series)
            {
                series.SegmentsCreated = false;
                series.ScheduleUpdateChart();
            }
        }

        #endregion
    }
}
