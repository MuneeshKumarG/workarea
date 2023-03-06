using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Animation;
using Microsoft.UI.Xaml.Shapes;
using Windows.Foundation;

namespace Syncfusion.UI.Xaml.Charts
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class CircularSeries : ChartSeries
    {
        #region Dependency Property Registration

        /// <summary>
        ///  The DependencyProperty for <see cref="StartAngle"/> property. 
        /// </summary>
        public static readonly DependencyProperty StartAngleProperty =
            DependencyProperty.Register(nameof(StartAngle), typeof(double), typeof(CircularSeries),
                                        new PropertyMetadata(0d, OnStartAngleChanged));

        /// <summary>
        /// The DependencyProperty for <see cref="EndAngle"/> property. 
        /// </summary>
        public static readonly DependencyProperty EndAngleProperty =
            DependencyProperty.Register(nameof(EndAngle), typeof(double), typeof(CircularSeries),
            new PropertyMetadata(360d, OnStartAngleChanged));

        /// <summary>
        /// The DependencyProperty for <see cref="GroupMode"/> property.
        /// </summary>
        public static readonly DependencyProperty GroupModeProperty =
            DependencyProperty.Register(nameof(GroupMode), typeof(PieGroupMode), typeof(CircularSeries),
            new PropertyMetadata(PieGroupMode.Value, OnGroupToPropertiesChanged));

        /// <summary>
        /// The DependencyProperty for <see cref="GroupTo"/> property.
        /// </summary>
        public static readonly DependencyProperty GroupToProperty =
            DependencyProperty.Register(nameof(GroupTo), typeof(double), typeof(CircularSeries),
            new PropertyMetadata(double.NaN, OnGroupToPropertiesChanged));

        /// <summary>
        /// Identifies the <see cref="DataLabelSettings"/> dependency property.
        /// </summary>
        /// <value>
        /// The identifier for <c>DataLabelSettings</c> dependency property.
        /// </value>
        public static readonly DependencyProperty DataLabelSettingsProperty =
          DependencyProperty.Register(nameof(DataLabelSettings), typeof(CircularDataLabelSettings), typeof(CircularSeries),
          new PropertyMetadata(null, OnAdornmentsInfoChanged));

        /// <summary>
        ///  The DependencyProperty for <see cref="Radius"/> property.
        /// </summary>
        public static readonly DependencyProperty RadiusProperty =
            DependencyProperty.Register(
                nameof(Radius),
                typeof(double),
                typeof(CircularSeries),
                new PropertyMetadata(0.8d, OnRadiusChanged));

        /// <summary>
        /// The DependencyProperty for <see cref="YBindingPath"/> property.
        /// </summary>
        public static readonly DependencyProperty YBindingPathProperty =
            DependencyProperty.Register(nameof(YBindingPath), typeof(string), typeof(CircularSeries),
            new PropertyMetadata(null, OnYPathChanged));

        /// <summary>
        /// The DependencyProperty for <see cref="Stroke"/> property.
        /// </summary>
        public static readonly DependencyProperty StrokeProperty =
            DependencyProperty.Register(nameof(Stroke), typeof(Brush), typeof(CircularSeries),
            new PropertyMetadata(null, OnStrokeChanged));

        /// <summary>
        /// The DependencyProperty for <see cref="StrokeWidth"/> property.
        /// </summary>
        public static readonly DependencyProperty StrokeWidthProperty =
            DependencyProperty.Register(nameof(StrokeWidth), typeof(double), typeof(CircularSeries),
            new PropertyMetadata(2d, OnStrokeChanged));

        #endregion

        #region Fields

        #region Constants

        internal const double TotalArcLength = Math.PI * 2;

        #endregion

        #region Private Fields

        private string groupingLabel = ChartLocalizationResourceAccessor.Instance.GetLocalizedStringResource("Others");

        private List<object> groupedData;

        #endregion

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="CircularSeries"/> class.
        /// </summary>
        public CircularSeries()
        {
            DataLabelSettings = new CircularDataLabelSettings();
            YValues = new List<double>();
        }

        #endregion

        #region Properties

        #region Public Properties

        /// <summary>
        /// Gets or sets a value to customize the appearance of the displaying data labels in the circular series.
        /// </summary>
        /// <remarks>
        /// This allows us to change the look of the displaying labels' content, shapes, and connector lines at the data point.
        /// </remarks>
        /// <value>This property takes the <see cref="CircularDataLabelSettings"/>.</value>
        /// <example>
        /// # [MainWindow.xaml](#tab/tabid-1)
        /// <code><![CDATA[
        ///     <chart:SfCircularChart>
        ///
        ///        <chart:PieSeries ItemsSource="{Binding Data}"
        ///                         XBindingPath="XValue"
        ///                         YBindingPath="YValue"
        ///                         ShowDataLabels="True">
        ///            <chart:PieSeries.DataLabelSettings>
        ///                <chart:CircularDataLabelSettings Position ="Outside" />
        ///            </chart:PieSeries.DataLabelSettings>
        ///        </chart:PieSeries>
        ///           
        ///     </chart:SfCircularChart>
        /// ]]>
        /// </code>
        /// # [MainWindow.cs](#tab/tabid-2)
        /// <code><![CDATA[
        ///     SfCircularChart chart = new SfCircularChart();
        ///     ViewModel viewModel = new ViewModel();
        ///
        ///     PieSeries series = new PieSeries();
        ///     series.ItemsSource = viewModel.Data;
        ///     series.XBindingPath = "XValue";
        ///     series.YBindingPath = "YValue";
        ///     series.ShowDataLabels = "True";
        ///     series.DataLabelSettings = new CircularDataLabelSettings()
        ///     {
        ///         Position = CircularSeriesLabelPosition.Outside,
        ///.    };
        ///     chart.Series.Add(series);
        ///	    
        /// ]]>
        /// </code>
        /// ***
        /// </example>
        public CircularDataLabelSettings DataLabelSettings
        {
            get
            {
                return (CircularDataLabelSettings)GetValue(DataLabelSettingsProperty);
            }

            set
            {
                SetValue(DataLabelSettingsProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets a value that can be used to modify the series start rendering position.
        /// </summary>
        /// <remarks>It is used to draw a series in different shapes.</remarks>
        /// <value>It accepts double values, and the default value is 0.</value>
        /// <example>
        /// # [Xaml](#tab/tabid-3)
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
        /// # [Xaml](#tab/tabid-5)
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
        /// # [C#](#tab/tabid-6)
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
        /// Gets or sets the group mode, which determines the type of grouping based on slice Angle, actual data point Value, or Percentage.
        /// </summary>
        /// <value>It accepts <see cref="PieGroupMode"/> values and the default value is <see cref="PieGroupMode.Value"/>.</value>
        /// <example>
        /// # [Xaml](#tab/tabid-9)
        /// <code><![CDATA[
        ///     <chart:SfCircularChart>
        ///
        ///          <chart:PieSeries ItemsSource="{Binding Data}"
        ///                            XBindingPath="XValue"
        ///                            YBindingPath="YValue"
        ///                            GroupMode = "Percentage"/>
        ///
        ///     </chart:SfCircularChart>
        /// ]]></code>
        /// # [C#](#tab/tabid-10)
        /// <code><![CDATA[
        ///     SfCircularChart chart = new SfCircularChart();
        ///     ViewModel viewModel = new ViewModel();
        ///
        ///     PieSeries series = new PieSeries()
        ///     {
        ///           ItemsSource = viewModel.Data,
        ///           XBindingPath = "XValue",
        ///           YBindingPath = "YValue",
        ///           GroupMode = PieGroupMode.Percentage,
        ///     };
        ///     
        ///     chart.Series.Add(series);
        ///
        /// ]]></code>
        /// ***
        /// </example>
        public PieGroupMode GroupMode
        {
            get { return (PieGroupMode)GetValue(GroupModeProperty); }
            set { SetValue(GroupModeProperty, value); }
        }

        /// <summary>
        /// Gets or sets the value that indicates the grouping of series segments.
        /// </summary>
        /// <remarks>It is used to limit the number of data points that can be grouped into a single slice.</remarks>
        /// <value>It accepts double values and the default value is double.NaN.</value>
        /// <example>
        /// # [Xaml](#tab/tabid-11)
        /// <code><![CDATA[
        ///     <chart:SfCircularChart>
        ///
        ///          <chart:PieSeries ItemsSource="{Binding Data}"
        ///                            XBindingPath="XValue"
        ///                            YBindingPath="YValue"
        ///                            GroupTo = "1000"
        ///                            GroupMode = "Value"/>
        ///
        ///     </chart:SfCircularChart>
        /// ]]></code>
        /// # [C#](#tab/tabid-12)
        /// <code><![CDATA[
        ///     SfCircularChart chart = new SfCircularChart();
        ///     ViewModel viewModel = new ViewModel();
        ///
        ///     PieSeries series = new PieSeries()
        ///     {
        ///           ItemsSource = viewModel.Data,
        ///           XBindingPath = "XValue",
        ///           YBindingPath = "YValue",
        ///           GroupTo = 1000,
        ///           GroupMode = PieGroupMode.Value,
        ///     };
        ///     
        ///     chart.Series.Add(series);
        ///
        /// ]]></code>
        /// ***
        /// </example>
        public double GroupTo
        {
            get { return (double)GetValue(GroupToProperty); }
            set { SetValue(GroupToProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value that can be used to render the series size.
        /// </summary>
        /// <value>It accepts double values, and the default value is 0.8. Here, the value is between 0 and 1.</value>
        /// <example>
        /// # [Xaml](#tab/tabid-13)
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
        /// # [C#](#tab/tabid-14)
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
        /// Gets or sets a path value on the source object to serve a y value to the series.
        /// </summary>
        /// <value>
        /// The string that represents the property name for the y plotting data, and its default value is null.
        /// </value>
        /// <example>
        /// # [Xaml](#tab/tabid-15)
        /// <code><![CDATA[
        ///     <chart:SfCircularChart>
        ///
        ///     <!-- ... Eliminated for simplicity-->
        ///
        ///          <chart:PieSeries ItemsSource="{Binding Data}"
        ///                            XBindingPath="XValue"
        ///                            YBindingPath="YValue" />
        ///
        ///     </chart:SfCircularChart>
        /// ]]>
        /// </code>
        /// # [C#](#tab/tabid-16)
        /// <code><![CDATA[
        ///     SfCircularChart chart = new SfCircularChart();
        ///     ViewModel viewModel = new ViewModel();
        ///
        ///     // Eliminated for simplicity
        ///
        ///     PieSeries pieSeries = new PieSeries()
        ///     {
        ///           ItemsSource = viewModel.Data,
        ///           XBindingPath = "XValue",
        ///           YBindingPath = "YValue",
        ///     };
        ///     
        ///     chart.Series.Add(pieSeries);
        ///
        /// ]]>
        /// </code>
        /// ***
        /// </example>
        public string YBindingPath
        {
            get { return (string)GetValue(YBindingPathProperty); }
            set { SetValue(YBindingPathProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value to specify the stroke thickness of a chart series.
        /// </summary>
        /// <value>It accepts double values and its default value is 2.</value>
        /// <example>
        /// # [Xaml](#tab/tabid-17)
        /// <code><![CDATA[
        ///     <chart:SfCircularChart>
        ///
        ///     <!-- ... Eliminated for simplicity-->
        ///
        ///          <chart:PieSeries ItemsSource="{Binding Data}"
        ///                            XBindingPath="XValue"
        ///                            YBindingPath="YValue"
        ///                            Stroke = "Red"
        ///                            StrokeThickness = "3"/>
        ///
        ///     </chart:SfCircularChart>
        /// ]]>
        /// </code>
        /// # [C#](#tab/tabid-18)
        /// <code><![CDATA[
        ///     SfCircularChart chart = new SfCircularChart();
        ///     ViewModel viewModel = new ViewModel();
        ///
        ///     // Eliminated for simplicity
        ///
        ///     PieSeries pieSeries = new PieSeries()
        ///     {
        ///           ItemsSource = viewModel.Data,
        ///           XBindingPath = "XValue",
        ///           YBindingPath = "YValue",
        ///           Stroke = new SolidColorBrush(Colors.Red),
        ///           StrokeThickness= 3,
        ///     };
        ///     
        ///     chart.Series.Add(pieSeries);
        ///
        /// ]]>
        /// </code>
        /// ***
        /// </example>
        public double StrokeWidth {
            get { return (double)GetValue(StrokeWidthProperty); }
            set { SetValue(StrokeWidthProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value to customize the stroke appearance of a chart series.
        /// </summary>
        /// <value>It accepts <see cref="Brush"/> values and its default value is null.</value>
        /// <example>
        /// # [Xaml](#tab/tabid-19)
        /// <code><![CDATA[
        ///     <chart:SfCircularChart>
        ///
        ///     <!-- ... Eliminated for simplicity-->
        ///
        ///          <chart:PieSeries ItemsSource="{Binding Data}"
        ///                            XBindingPath="XValue"
        ///                            YBindingPath="YValue"
        ///                            Stroke = "Red"
        ///                            StrokeThickness = "3"/>
        ///
        ///     </chart:SfCircularChart>
        /// ]]>
        /// </code>
        /// # [C#](#tab/tabid-20)
        /// <code><![CDATA[
        ///     SfCircularChart chart = new SfCircularChart();
        ///     ViewModel viewModel = new ViewModel();
        ///
        ///     // Eliminated for simplicity
        ///
        ///     PieSeries pieSeries = new PieSeries()
        ///     {
        ///           ItemsSource = viewModel.Data,
        ///           XBindingPath = "XValue",
        ///           YBindingPath = "YValue",
        ///           Stroke = new SolidColorBrush(Colors.Red),
        ///           StrokeThickness= 3,
        ///     };
        ///     
        ///     chart.Series.Add(pieSeries);
        ///
        /// ]]>
        /// </code>
        /// ***
        /// </example>
        public Brush Stroke {
            get { return (Brush)GetValue(StrokeProperty); }
            set { SetValue(StrokeProperty, value); }
        }

        internal List<object> GroupedData
        {
            get { return groupedData; }
            set
            {
                groupedData = value;
            }
        }


        #endregion

        #region Internal Properties

        /// <summary>
        /// Gets or sets the Y values collection binded with this series.
        /// </summary>
        internal IList<double> YValues { get; set; }

        internal double CircularRadius { get; set; }

        internal Point Center { get; set; }

        internal string GroupingLabel
        {
            get { return groupingLabel; }
            set { groupingLabel = value; }
        }

        #endregion

        #region Internal Override Properties

        internal override ChartDataLabelSettings AdornmentsInfo
        {
            get
            {
                return DataLabelSettings;
            }

        }

        internal override bool IsSingleAccumulationSeries
        {
            get
            {
                return true;
            }
        }

        #endregion

        #endregion

        #region Methods

        #region Internal Override Methods

        internal override void SetDataLabelsVisibility(bool isShowDataLabels)
        {
            if (DataLabelSettings != null)
            {
                DataLabelSettings.Visible = isShowDataLabels;
            }
        }

        internal override void ResetAdornmentAnimationState()
        {
            if (adornmentInfo != null)
            {
                foreach (var child in this.AdornmentPresenter.Children)
                {
                    (child as FrameworkElement).ClearValue(FrameworkElement.OpacityProperty);
                }
            }
        }

        internal override void SelectedSegmentsIndexes_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            ChartSegment selectedSegment = null, oldSegment = null;
            CircularSeries circularSeries = this;
            if (SelectionBehavior == null) return;

            bool isGroupTo = circularSeries != null && !double.IsNaN(circularSeries.GroupTo);
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    if (e.NewItems != null && SelectionBehavior != null && SelectionBehavior.EnableMultiSelection)
                    {
                        int oldIndex = PreviousSelectedIndex;
                        int newIndex = (int)e.NewItems[0];

                        if (oldIndex != -1 && oldIndex < ActualData.Count)
                        {
                            object oldItem = isGroupTo ? Segments[oldIndex].Item : ActualData[oldIndex];
                            oldSegment = Segments.Where(segment => segment.Item == oldItem).FirstOrDefault();
                        }

                        if (newIndex >= 0 && GetEnableSegmentSelection())
                        {
                            if (Segments.Count == 0)
                            {
                                triggerSelectionChangedEventOnLoad = true;
                                return;
                            }

                            if (newIndex < Segments.Count || newIndex < ActualData.Count)
                            {
                                // For adornment selection implementation
                                if (adornmentInfo is ChartDataLabelSettings && adornmentInfo.HighlightOnSelection)
                                    UpdateAdornmentSelection(newIndex);

                                object newItem = isGroupTo ? Segments[newIndex].Item : ActualData[newIndex];
                                selectedSegment = Segments.Where(segment => segment.Item == newItem).FirstOrDefault();
                                if (selectedSegment != null) {
                                    if (SelectionBehavior.SelectionBrush != null)
                                        selectedSegment.BindProperties();
                                    selectedSegment.IsSelectedSegment = true;
                                }
                                
                            }

                            if (newIndex < Segments.Count || newIndex < ActualData.Count)
                            {
                                OnSelectionChanged(newIndex, oldIndex);
                                PreviousSelectedIndex = newIndex;
                            }
                        }
                    }

                    break;

                case NotifyCollectionChangedAction.Remove:

                    if (e.OldItems != null && SelectionBehavior.EnableMultiSelection)
                    {
                        int newIndex = (int)e.OldItems[0];

                        if (PreviousSelectedIndex != -1 && PreviousSelectedIndex < ActualData.Count)
                        {
                            object oldItem = isGroupTo ? Segments[PreviousSelectedIndex].Item : ActualData[PreviousSelectedIndex];
                            oldSegment = Segments.Where(segment => segment.Item == oldItem).FirstOrDefault();
                        }

                        OnSelectionChanged(newIndex, PreviousSelectedIndex);
                        PreviousSelectedIndex = newIndex;
                        OnResetSegment(newIndex);
                    }

                    break;
            }
        }

        /// <summary>
        /// Method used to set SegmentSelectionBrush to selectedindex chartsegment.
        /// </summary>
        /// <param name="newIndex">new index</param>
        /// <param name="oldIndex">old index</param>
        internal override void SelectedIndexChanged(int newIndex, int oldIndex)
        {
            CircularSeries circularseries = this as CircularSeries;
            bool isGroupTo = circularseries != null && !double.IsNaN(circularseries.GroupTo);
            if (ActualArea != null && SelectionBehavior != null)
            {
                ChartSegment selectedSegment = null, oldSegment = null;

                // Reset the oldIndex segment Interior
                if (!SelectionBehavior.EnableMultiSelection)
                {
                    if (SelectedSegmentsIndexes.Contains(oldIndex))
                        SelectedSegmentsIndexes.Remove(oldIndex);

                    OnResetSegment(oldIndex);
                }

                if (IsItemsSourceChanged)
                {
                    return;
                }

                if (oldIndex != -1 && oldIndex < ActualData.Count)
                {
                    object oldItem = isGroupTo ? Segments[oldIndex].Item : ActualData[oldIndex];
                    oldSegment = Segments.Where(segment => segment.Item == oldItem).FirstOrDefault();
                }

                if (newIndex >= 0 && GetEnableSegmentSelection())
                {
                    if (!SelectedSegmentsIndexes.Contains(newIndex))
                        SelectedSegmentsIndexes.Add(newIndex);
                    if (Segments.Count == 0)
                    {
                        triggerSelectionChangedEventOnLoad = true;
                        return;
                    }

                    if (newIndex < Segments.Count || newIndex < ActualData.Count)
                    {
                        // For adornment selection implementation
                        if (adornmentInfo is ChartDataLabelSettings && adornmentInfo.HighlightOnSelection)
                            UpdateAdornmentSelection(newIndex);

                        // Set the SegmentSelectionBrush to newIndex segment Interior
                        object newItem = isGroupTo ? Segments[newIndex].Item : ActualData[newIndex];
                        selectedSegment = Segments.Where(segment => segment.Item == newItem).FirstOrDefault();
                        if (selectedSegment != null) 
                        {
                            if (SelectionBehavior.SelectionBrush != null)
                                selectedSegment.BindProperties();
                            selectedSegment.IsSelectedSegment = true;
                        }
                    }

                    if (newIndex < Segments.Count || newIndex < ActualData.Count)
                    {
                        OnSelectionChanged(newIndex, oldIndex);
                        PreviousSelectedIndex = newIndex;
                    }
                }
                else if (newIndex == -1)
                {
                    OnSelectionChanged(newIndex, oldIndex);
                    PreviousSelectedIndex = newIndex;
                }
            }
        }

        internal override void OnResetSegment(int index)
        {
            if (index >= 0)
            {
                object item = !double.IsNaN(this.GroupTo) ? Segments[index].Item : ActualData[index];
                var resetSegment = Segments.Where(segment => segment.Item == item).FirstOrDefault();
                if (resetSegment != null)
                {
                    resetSegment.BindProperties();
                    resetSegment.IsSelectedSegment = false;
                }

                if (adornmentInfo is ChartDataLabelSettings)
                    AdornmentPresenter.ResetAdornmentSelection(index, false);
            }
        }

        /// <summary>
        /// This method used to get the chart data at an index.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        internal override ChartDataPointInfo GetDataPoint(int index)
        {
            IList<double> xValues = (ActualXValues is IList<double>) ? ActualXValues as IList<double> : GetXValues();
            dataPoint = null;
            if (index < xValues.Count)
            {
                dataPoint = new ChartDataPointInfo();

                if (xValues.Count > index)
                    dataPoint.XData = IsIndexed ? index : xValues[index];

                if (YValues.Count > index)
                    dataPoint.YData = YValues[index];

                dataPoint.Index = index;
                dataPoint.Series = this;

                if (ActualData.Count > index)
                    dataPoint.Item = ActualData[index];
            }

            return dataPoint;
        }

        internal override int GetDataPointIndex(Point point)
        {
            Canvas canvas = Chart.GetAdorningCanvas();
            double left = Chart.ActualWidth - canvas.ActualWidth;
            double top = Chart.ActualHeight - canvas.ActualHeight;
            point.X = point.X - left + Chart.Margin.Left;
            point.Y = point.Y - top + Chart.Margin.Top;

            foreach (var segment in Segments)
            {
                Path path = segment.GetRenderedVisual() as Path;
                bool isHit = false;
                var pathData = path.Data as PathGeometry;
                if (pathData != null)
                    isHit = pathData.Bounds.Contains(point);
                if (isHit)
                    return Segments.IndexOf(segment);
            }

            return -1;
        }

        /// <summary>
        /// Method used to generate points for circular series.
        /// </summary>
        internal override void GenerateDataPoints()
        {
            GeneratePoints(new[] { YBindingPath }, YValues);
        }
        
        internal override void Dispose()
        {
            YValues = null;
            base.Dispose();
        }

        /// <inheritdoc/>
        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            AdornmentPresenter = new ChartDataMarkerPresenter();
            AdornmentPresenter.Series = this;

            if (Chart != null && AdornmentsInfo != null && ShowDataLabels)
            {
                Chart.DataLabelPresenter.Children.Add(AdornmentPresenter);
                AdornmentsInfo.PanelChanged(AdornmentPresenter);
            }
        }

        /// <summary>
        /// Called when ItemsSource property changed.
        /// </summary>
        /// <param name="oldValue">old value</param>
        /// <param name="newValue">new value</param>
        internal override void OnDataSourceChanged(object oldValue, object newValue)
        {
            base.OnDataSourceChanged(oldValue, newValue);

            if (AdornmentsInfo != null)
            {
                VisibleAdornments.Clear();
                Adornments.Clear();
                AdornmentsInfo.UpdateElements();
            }

            if (YValues != null)
                YValues.Clear();
            if (Segments != null)
                Segments.Clear();
            GeneratePoints(new[] { YBindingPath }, YValues);
            if (this.Chart != null && Chart.PlotArea != null)
                this.Chart.PlotArea.ShouldPopulateLegendItems = true;
            ScheduleUpdateChart();
        }

        internal override void UpdateOnSeriesBoundChanged(Size size)
        {
            if (AdornmentPresenter != null && AdornmentsInfo != null)
            {
                AdornmentsInfo.UpdateElements();
            }

            base.UpdateOnSeriesBoundChanged(size);

            if (AdornmentPresenter != null && AdornmentsInfo != null)
            {
                AdornmentPresenter.Update(size);
                AdornmentPresenter.Arrange(size);
            }
        }

        /// <summary>
        /// Called when binding path changed.
        /// </summary>
        /// <param name="args">args</param>
        internal override void OnBindingPathChanged()
        {

            YValues.Clear();
            Segments.Clear();
            ResetData();
            GeneratePoints(new[] { YBindingPath }, YValues);
            if (this.Chart != null && this.Chart.PlotArea != null)
                this.Chart.PlotArea.ShouldPopulateLegendItems = true;
            base.OnBindingPathChanged();
        }

        #endregion

        #region Internal Virtual Methods

        /// <summary>
        /// Method implementation for ExplodeIndex.
        /// </summary>
        /// <param name="i"></param>
        internal virtual void SetExplodeIndex(int i)
        {
        }

        /// <summary>
        /// Virtual Method for ExplodeRadius.
        /// </summary>
        internal virtual void SetExplodeRadius()
        {
        }

        /// <summary>
        /// Virtual method for ExplodeAll.
        /// </summary>
        internal virtual void SetExplodeAll()
        {
        }

        internal virtual void OnCircularRadiusChanged(DependencyPropertyChangedEventArgs e)
        {

        }

        #endregion

        #region Internal Methods

        internal void AnimateAdornments(Storyboard sb)
        {
            if (this.AdornmentsInfo != null)
            {
                double totalDuration = AnimationDuration.TotalSeconds;
                foreach (var child in this.AdornmentPresenter.Children)
                {
                    DoubleAnimationUsingKeyFrames keyFrames1 = new DoubleAnimationUsingKeyFrames();
                    SplineDoubleKeyFrame frame1 = new SplineDoubleKeyFrame();

                    frame1.KeyTime = frame1.KeyTime.GetKeyTime(TimeSpan.FromSeconds(0));
                    frame1.Value = 0;
                    keyFrames1.KeyFrames.Add(frame1);

                    frame1 = new SplineDoubleKeyFrame();
                    frame1.KeyTime = frame1.KeyTime.GetKeyTime(TimeSpan.FromSeconds(totalDuration));
                    frame1.Value = 0;
                    keyFrames1.KeyFrames.Add(frame1);

                    frame1 = new SplineDoubleKeyFrame();
                    frame1.KeyTime = frame1.KeyTime.GetKeyTime(TimeSpan.FromSeconds(totalDuration + 1));
                    frame1.Value = 1;
                    keyFrames1.KeyFrames.Add(frame1);

                    KeySpline keySpline = new KeySpline();
                    keySpline.ControlPoint1 = new Point(0.64, 0.84);
                    keySpline.ControlPoint2 = new Point(0, 1); // Animation have to provide same easing effect in all platforms.
                    keyFrames1.EnableDependentAnimation = true;
                    Storyboard.SetTargetProperty(keyFrames1, "(Opacity)");
                    frame1.KeySpline = keySpline;

                    Storyboard.SetTarget(keyFrames1, child as FrameworkElement);
                    sb.Children.Add(keyFrames1);
                }
            }
        }

        internal Point GetActualCenter(Point centerPoint, double radius)
        {
            if (Chart != null && Chart.GetSeriesCollection().IndexOf(this) > 0) return centerPoint;
            Point actualCenter = centerPoint;
            double startAngle = StartAngle;
            double endAngle = EndAngle;

            // WPF-29938 PieSeries is not getting aligned properly. The array is generated according to the start angle and end angle.
            var arraySize = ((Math.Max(Math.Abs((int)startAngle / 90), Math.Abs((int)endAngle / 90)) + 1) * 2) + 1;
            double[] regions = new double[(arraySize)];

            int arrayIndex = 0;
            for (int i = -(arraySize / 2); i < arraySize / 2 + 1; i++)
            {
                regions[arrayIndex] = i * 90;
                arrayIndex++;
            }

            List<int> region = new List<int>();
            if (startAngle < endAngle)
            {
                for (int i = 0; i < regions.Count(); i++)
                {
                    if (regions[i] > startAngle && regions[i] < endAngle)
                        region.Add((int)((regions[i] % 360) < 0 ? (regions[i] % 360) + 360 : (regions[i] % 360)));
                }
            }
            else
            {
                for (int i = 0; i < regions.Count(); i++)
                {
                    if (regions[i] < startAngle && regions[i] > endAngle)
                        region.Add((int)((regions[i] % 360) < 0 ? (regions[i] % 360) + 360 : (regions[i] % 360)));
                }
            }

            var startRadian = 2 * Math.PI * (startAngle) / 360;
            var endRadian = 2 * Math.PI * (endAngle) / 360;
            Point startPoint = new Point(centerPoint.X + radius * Math.Cos(startRadian), centerPoint.Y + radius * Math.Sin(startRadian));
            Point endPoint = new Point(centerPoint.X + radius * Math.Cos(endRadian), centerPoint.Y + radius * Math.Sin(endRadian));

            switch (region.Count)
            {
                case 0:
                    var longX = Math.Abs(centerPoint.X - startPoint.X) > Math.Abs(centerPoint.X - endPoint.X) ? startPoint.X : endPoint.X;
                    var longY = Math.Abs(centerPoint.Y - startPoint.Y) > Math.Abs(centerPoint.Y - endPoint.Y) ? startPoint.Y : endPoint.Y;
                    var midPoint = new Point(Math.Abs((centerPoint.X + longX)) / 2, Math.Abs((centerPoint.Y + longY)) / 2);
                    actualCenter.X = centerPoint.X + (centerPoint.X - midPoint.X);
                    actualCenter.Y = centerPoint.Y + (centerPoint.Y - midPoint.Y);
                    break;

                case 1:
                    Point point1 = new Point(), point2 = new Point();
                    var maxRadian = 2 * Math.PI * region[0] / 360;
                    var maxPoint = new Point(centerPoint.X + radius * Math.Cos(maxRadian), centerPoint.Y + radius * Math.Sin(maxRadian));
                    switch (region.ElementAt(0))
                    {
                        case 270:
                            point1 = new Point(startPoint.X, maxPoint.Y);
                            point2 = new Point(endPoint.X, centerPoint.Y);
                            break;
                        case 0:
                        case 360:
                            point1 = new Point(centerPoint.X, endPoint.Y);
                            point2 = new Point(maxPoint.X, startPoint.Y);
                            break;
                        case 90:
                            point1 = new Point(endPoint.X, centerPoint.Y);
                            point2 = new Point(startPoint.X, maxPoint.Y);
                            break;
                        case 180:
                            point1 = new Point(maxPoint.X, startPoint.Y);
                            point2 = new Point(centerPoint.X, endPoint.Y);
                            break;
                    }

                    midPoint = new Point((point1.X + point2.X) / 2, (point1.Y + point2.Y) / 2);
                    actualCenter.X = centerPoint.X + ((centerPoint.X - midPoint.X) >= radius ? 0 : (centerPoint.X - midPoint.X));
                    actualCenter.Y = centerPoint.Y + ((centerPoint.Y - midPoint.Y) >= radius ? 0 : (centerPoint.Y - midPoint.Y));
                    break;

                case 2:
                    var minRadian = 2 * Math.PI * region[0] / 360;
                    maxRadian = 2 * Math.PI * (region[1]) / 360;
                    maxPoint = new Point(centerPoint.X + radius * Math.Cos(maxRadian), centerPoint.Y + radius * Math.Sin(maxRadian));
                    Point minPoint = new Point(centerPoint.X + radius * Math.Cos(minRadian), centerPoint.Y + radius * Math.Sin(minRadian));
                    if (region[0] == 0 && region[1] == 90 || region[0] == 180 && region[1] == 270)
                        point1 = new Point(minPoint.X, maxPoint.Y);
                    else
                        point1 = new Point(maxPoint.X, minPoint.Y);
                    if (region[0] == 0 || region[0] == 180)
                        point2 = new Point(GetMinMaxValue(startPoint, endPoint, region[0]), GetMinMaxValue(startPoint, endPoint, region[1]));
                    else
                        point2 = new Point(GetMinMaxValue(startPoint, endPoint, region[1]), GetMinMaxValue(startPoint, endPoint, region[0]));
                    midPoint = new Point(Math.Abs(point1.X - point2.X) / 2 >= radius ? 0 : (point1.X + point2.X) / 2, y: Math.Abs(point1.Y - point2.Y) / 2 >= radius ? 0 : (point1.Y + point2.Y) / 2);
                    actualCenter.X = centerPoint.X + (midPoint.X == 0 ? 0 : (centerPoint.X - midPoint.X) >= radius ? 0 : (centerPoint.X - midPoint.X));
                    actualCenter.Y = centerPoint.Y + (midPoint.Y == 0 ? 0 : (centerPoint.Y - midPoint.Y) >= radius ? 0 : (centerPoint.Y - midPoint.Y));

                    break;
            }

            return actualCenter;
        }

        internal Tuple<List<double>, List<object>> GetGroupToYValues()
        {
            List<double> yValues = new List<double>();
            List<object> actualData = new List<object>();
            GroupedData = new List<object>();
            double lessThanGroupTo = 0;
            var sumOfYValues = (from val in YValues
                                select (val) > 0 ? val : Math.Abs(double.IsNaN(val) ? 0 : val)).Sum();

            for (int i = 0; i < PointsCount; i++)
            {
                double yValue = YValues[i];

                if (GetGroupModeValue(yValue, sumOfYValues) > GroupTo)
                {
                    yValues.Add(yValue);
                    actualData.Add(ActualData[i]);
                }
                else if (!double.IsNaN(yValue))
                {
                    lessThanGroupTo += yValue;
                    GroupedData.Add(ActualData[i]);
                }

                if (i == PointsCount - 1 && GroupedData.Count > 0)
                {
                    yValues.Add(lessThanGroupTo);
                    actualData.Add(GroupedData);
                }

            }

            return new Tuple<List<double>, List<object>>(yValues, actualData);
        }

        internal double GetGroupModeValue(double yValue, double sumOfYValues)
        {
            double value = 0;

            switch (GroupMode)
            {
                case PieGroupMode.Value:
                    value = yValue;
                    break;
                case PieGroupMode.Percentage:
                    float percentage = (float)(yValue / sumOfYValues * 100);
                    percentage = (float)Math.Floor(percentage * 100) / 100;
                    value = percentage;
                    break;
                case PieGroupMode.Angle:
                    double angleDifference = EndAngle - StartAngle;

                    if (Math.Abs(Math.Round(angleDifference * 100.0) / 100.0) > 360)
                    {
                        angleDifference = angleDifference % 360;
                    }

                    value = Math.Abs(double.IsNaN(yValue) ? 0 : yValue) * (angleDifference / sumOfYValues);
                    break;
            }

            return value;
        }

        internal List<double> GetToggleYValues(List<double> groupToYValues)
        {
            var yvalues = new List<double>();

            for (int i = 0; i < groupToYValues.Count; i++)
            {
                double yvalue = groupToYValues[i];

                if (!ToggledLegendIndex.Contains(i))
                {
                    yvalues.Add(yvalue);
                }
                else
                    yvalues.Add(double.NaN);
            }

            return yvalues;
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// Returns the radian value.
        /// </summary>
        /// <param name="degree">Degree</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "Reviewed")]
        internal double DegreeToRadianConverter(double degree)
        {
            return degree * Math.PI / 180;
        }

        #endregion

        #region Private Static Methods
      
        private static void OnStartAngleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CircularSeries series = d as CircularSeries;
            if (series != null)
            {
                if (!double.IsNaN(series.GroupTo))
                {
                    OnGroupToPropertiesChanged(d, e);
                }
                else
                    series.ScheduleUpdateChart();
            }
        }

       
        private static double GetMinMaxValue(Point point1, Point point2, int degree)
        {
            var minX = Math.Min(point1.X, point2.Y);
            var minY = Math.Min(point1.Y, point2.Y);
            var maxX = Math.Max(point1.X, point2.X);
            var maxY = Math.Max(point1.Y, point2.Y);
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

            return 0d;
        }

        private static void OnGroupToPropertiesChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CircularSeries series = d as CircularSeries;
            if (series != null && series.ActualArea != null)
            {
                if (series.ActualArea.PlotArea != null)
                    series.ActualArea.PlotArea.ShouldPopulateLegendItems = true;

                series.Segments.Clear();
                series.ScheduleUpdateChart();
            }
        }

        private static void OnRadiusChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CircularSeries series = d as CircularSeries;
            series.OnCircularRadiusChanged(e);
        }

        private static void OnYPathChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as CircularSeries).OnBindingPathChanged();
        }

        #endregion

        #endregion
    }
}
