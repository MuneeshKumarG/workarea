using System;
using System.Collections.Specialized;
using System.Collections.Generic;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Windows.Foundation;
using AdornmentSeries = Syncfusion.UI.Xaml.Charts.DataMarkerSeries;
using ChartAdornment = Syncfusion.UI.Xaml.Charts.ChartDataLabel;
using ChartAdornmentContainer = Syncfusion.UI.Xaml.Charts.ChartDataMarkerContainer;

namespace Syncfusion.UI.Xaml.Charts
{
    /// <summary>
    /// Represents a series which displays data in a funnel shape that equals to 100% when totaled, and its plots streamlined data to show various stages in a process.
    /// </summary>
    /// <remarks>
    /// It is a single series, representing data as portions of 100% and does not use any axes.
    /// </remarks>
    /// <example>
    /// # [MainWindow.xaml](#tab/tabid-1)
    /// <code><![CDATA[
    ///     <chart:SfChart>
    ///
    ///          <chart:SfChart.DataContext>
    ///            <local:ViewModel/>
    ///          </chart:SfChart.DataContext>
    ///          
    ///          <chart:FunnelSeries
    ///               ItemsSource="{Binding Data}"
    ///               XBindingPath="XValue"
    ///               YBindingPath="YValue"/>
    ///                    
    ///     </chart:SfChart>
    /// ]]></code>
    /// # [MainWindow.cs](#tab/tabid-2)
    /// <code><![CDATA[
    ///     SfChart chart = new SfChart();
    ///     
    ///     ViewModel viewmodel = new ViewModel();
    ///     
    ///     FunnelSeries series = new FunnelSeries();
    ///     series.ItemsSource = viewmodel.Data;
    ///     series.XBindingPath = "XValue";
    ///     series.YBindingPath = "YValue";
    ///     chart.Series.Add(series);
    /// ]]></code>
    /// # [ViewModel.cs](#tab/tabid-3)
    /// <code><![CDATA[
    /// public ObservableCollection<Model> Data { get; set; }
    /// 
    /// public ViewModel()
    /// {
    ///    Data = new ObservableCollection<Model>();
    ///    Data.Add(new Model() { XValue = 10, YValue = 100 });
    ///    Data.Add(new Model() { XValue = 20, YValue = 150 });
    ///    Data.Add(new Model() { XValue = 30, YValue = 110 });
    ///    Data.Add(new Model() { XValue = 40, YValue = 230 });
    /// }
    /// ]]></code>
    /// ***
    /// </example>

    /// <seealso cref="FunnelSegment"/>
    /// <seealso cref="PyramidSegment"/>
    /// <seealso cref="PyramidSeries"/>  
    [Obsolete("Use SfFunnelChart", false)]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1805: Do not initialize unnecessarily")]
    public class FunnelSeries : TriangularSeriesBase, ISegmentSelectable
    {
        #region Dependency Property Registration


        /// <summary>
        /// The DependencyProperty for <see cref="FunnelDataLabelSettings"/> property.
        /// </summary>
        public static readonly DependencyProperty DataLabelSettingsProperty =
          DependencyProperty.Register(nameof(DataLabelSettings), typeof(FunnelDataLabelSettings), typeof(TriangularSeriesBase),
          new PropertyMetadata(null, OnAdornmentsInfoChanged));


        /// <summary>
        /// Identifies the <c>FunnelMode</c> dependency property.
        /// </summary>        
        /// <value>
        /// The identifier for <c>FunnelMode</c> dependency property.
        /// </value> 
        public static readonly DependencyProperty FunnelModeProperty =
            DependencyProperty.Register(
                nameof(FunnelMode), 
                typeof(ChartFunnelMode),
                typeof(FunnelSeries),
                new PropertyMetadata(ChartFunnelMode.ValueIsHeight, OnFunnelModeChanged));

        /// <summary>
        /// Identifies the <c>MinWidth</c> dependency property.
        /// </summary>        
        /// <value>
        /// The identifier for <c>MinWidth</c> dependency property.
        /// </value> 
        public new static readonly DependencyProperty MinWidthProperty =
            DependencyProperty.Register(
                nameof(MinWidth),
                typeof(double), 
                typeof(FunnelSeries),
                new PropertyMetadata(40d, OnMinWidthChanged));

        #endregion

        #region Fields

        #region Private Fields

        double currY = 0d;

        #endregion

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="FunnelSeries"/> class.
        /// </summary>
        public FunnelSeries()
        {
            DefaultStyleKey = typeof(FunnelSeries);
        }

        #endregion

        #region Properties

        #region Public Properties

        /// <summary>
        /// <para>Gets or sets data labels for the series.</para> <para>This allows us to customize the appearance of a data point by displaying labels, shapes and connector lines.</para>
        /// </summary>
        /// <value>
        /// The <see cref="ChartDataLabelSettings" /> value.
        /// </value>
        /// <example>
        /// # [MainWindow.xaml](#tab/tabid-1)
        /// <code><![CDATA[
        ///     <chart:SfFunnelChart ItemsSource="{Binding Data}" XBindingPath="XValue" YBindingPath="YValue" ShowDataLabels="True">
        ///
        ///                    <syncfusion:SfFunnelChart.DataLabelSettings>
        ///                          <chart:FunnelDataLabelSettings/>
        ///                    <syncfusion:SfFunnelChart.DataLabelSettings>
        ///           
        ///     </chart:SfFunnelChart>
        /// ]]></code>
        /// # [MainWindow.cs](#tab/tabid-2)
        /// <code><![CDATA[
        ///     SfFunnelChart chart = new SfFunnelChart();
        ///     
        ///     chart.ItemsSource = viewmodel.Data;
        ///     chart.XBindingPath = "XValue";
        ///     chart.YBindingPath = "YValue";
        ///     chart.ShowDataLabels = "True";
        ///     
        ///     chart.DataLabelSettings = new FunnelDataLabelSettings(){};
        /// ]]></code>
        /// ***
        /// </example>
        public FunnelDataLabelSettings DataLabelSettings
        {
            get
            {
                return (FunnelDataLabelSettings)GetValue(DataLabelSettingsProperty);  
            }
            set
            {
                SetValue(DataLabelSettingsProperty, value);
            }
        }

        internal override ChartDataLabelSettings AdornmentsInfo
        {
            get
            {
                return (FunnelDataLabelSettings)GetValue(DataLabelSettingsProperty);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the y value should interpret the height or width of the funnel block.
        /// </summary>
        /// <value>
        /// One of the <see cref="ChartFunnelMode"/> enumeration values. The default value is <see cref="ChartFunnelMode.ValueIsHeight"/>.
        /// </value>
        public ChartFunnelMode FunnelMode
        {
            get { return (ChartFunnelMode)GetValue(FunnelModeProperty); }
            set { SetValue(FunnelModeProperty, value); }
        }

        /// <summary>
        /// Gets or sets the minimum width for the funnel block.
        /// </summary>
        /// <value>Default value is 40.</value>
        public new double MinWidth
        {
            get { return (double)GetValue(MinWidthProperty); }
            set { SetValue(MinWidthProperty, value); }
        }

        #endregion

        #endregion

        #region Methods

        #region Public Override Methods

        /// <summary>
        /// Creates the segments of FunnelSeries.
        /// </summary>
        public override void CreateSegments()
        {
            Segments.Clear();
            Adornments.Clear();
            List<double> xValues = GetXValues();
            double sumValues = 0d, gapRatio = this.GapRatio;
            int count = DataCount;
            int explodedIndex = this.ExplodeIndex;
            ChartFunnelMode funnelmode = this.FunnelMode;

            IList<double> toggledYValues = null;

            if (ToggledLegendIndex.Count > 0)
                toggledYValues = GetYValues();
            else
                toggledYValues = YValues;

            for (int i = 0; i < count; i++)
            {
                sumValues += Math.Max(0, Math.Abs(double.IsNaN(toggledYValues[i]) ? 0 : toggledYValues[i]));
            }

            if (funnelmode == ChartFunnelMode.ValueIsHeight)
                this.CalculateValueIsHeightSegments(toggledYValues, xValues, sumValues, gapRatio, explodedIndex);
            else
                this.CalculateValueIsWidthSegments(toggledYValues, xValues, sumValues, gapRatio, count, explodedIndex);

            if (ShowEmptyPoints)
                UpdateEmptyPointSegments(xValues, false);
            if (ActualArea != null)
                ActualArea.IsUpdateLegend = true;
        }

        #endregion

        #region Internal Override Methods

        internal override void SetDataLabelsVisibility(bool isShowDataLabels)
        {
            if (DataLabelSettings != null)
            {
                DataLabelSettings.Visible = isShowDataLabels;
            }
        }

        internal override object GetTooltipTag(FrameworkElement element)
        {
            object tooltipTag = null;

            if (element.Tag is ChartSegment)
                tooltipTag = element.Tag;
            else if (element.DataContext is ChartSegment && !(element.DataContext is ChartAdornment))
                tooltipTag = element.DataContext;
            else if (element.DataContext is ChartAdornmentContainer)
            {
                if (Segments.Count > 0)
                    tooltipTag = Segments[ChartExtensionUtils.GetAdornmentIndex(element)];
            }
            else
            {
                var contentPresenter = VisualTreeHelper.GetParent(element) as ContentPresenter;

                if (contentPresenter != null && contentPresenter.Content is ChartAdornment)
                {
                    tooltipTag = GetSegment((contentPresenter.Content as ChartAdornment).Item);
                }
                else
                {
                    int index = ChartExtensionUtils.GetAdornmentIndex(element);

                    if (index != -1 && index < Adornments.Count && index < Segments.Count)
                    {
                        if (index < ActualData.Count)
                            tooltipTag = GetSegment(ActualData[index]);
                    }
                }
            }

            return tooltipTag;
        }

        #endregion

        #region Protected Internal Override Methods

        /// <inheritdoc/>
        protected internal override IChartTransformer CreateTransformer(Size size, bool create)
        {
            if (create || ChartTransformer == null)
            {
                ChartTransformer = ChartTransform.CreateSimple(size);
            }

            return ChartTransformer;
        }

        #endregion

        #region Protected Override Methods

        /// <inheritdoc/>
        protected override ChartSegment CreateSegment()
        {
            return new FunnelSegment();
        }

        /// <summary>
        /// Creates the datamarker label of funnel series.
        /// </summary>
        /// <param name="series">Used to specify the series instance.</param>
        /// <param name="xVal">Used to specify the xvalue.</param>
        /// <param name="yVal">Used to specify the yvalue.</param>
        /// <param name="height">Used to specify the height.</param>
        /// <param name="currY">Used to specify the yposition.</param>
        /// <returns>returns <see cref="ChartAdornment"/></returns>
        protected override ChartAdornment CreateDataMarker(AdornmentSeries series, double xVal, double yVal, double height, double currY)
        {
            return new TriangularAdornment(xVal, yVal, currY, height, series);
        }

        internal override ChartAdornment CreateAdornment(AdornmentSeries series, double xVal, double yVal, double height, double currY)
        {
            return CreateDataMarker(series, xVal, yVal, height, currY);
        }

        /// <summary>
        /// Method implementation for ExplodeIndex.
        /// </summary>
        /// <param name="i">Exploded segment index.</param>
        internal override void SetExplodeIndex(int i)
        {
            if (Segments.Count > 0)
            {
                foreach (FunnelSegment segment in Segments)
                {
                    int index = ActualData.IndexOf(segment.Item);
                    if (i == index)
                    {
                        segment.IsExploded = !segment.IsExploded;
                        base.UpdateSegments(i, NotifyCollectionChangedAction.Remove);

                        if (index != ExplodeIndex)
                        {
                            ExplodeIndex = i;
                        }
                    }
                    else if (i == -1)
                    {
                        segment.IsExploded = false;
                        base.UpdateSegments(i, NotifyCollectionChangedAction.Remove);

                        if (index != ExplodeIndex)
                        {
                            ExplodeIndex = i;
                        }
                    }
                }
            }
        }

        #endregion

        #region Private Static Methods


        private static void OnFunnelModeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            FunnelSeries funnelSeries = d as FunnelSeries;
            if (funnelSeries != null && funnelSeries.Area != null)
            {
                funnelSeries.Area.IsUpdateLegend = true;
                funnelSeries.Area.ScheduleUpdate();
            }
        }

        private static void OnMinWidthChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            FunnelSeries funnelSeries = d as FunnelSeries;
            if (funnelSeries != null && funnelSeries.Area != null)
                funnelSeries.Area.ScheduleUpdate();
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// To calculate the segments if the pyramid mode is ValueIsHeight.
        /// </summary>
        private void CalculateValueIsHeightSegments(IList<double> yValues, List<double> xValues, double sumValues, double gapRatio, int explodedIndex)
        {
            currY = 0d;
            double coefHeight = 1 / sumValues;
            double spacing = gapRatio / (DataCount - 1);

            // spacing value is set to NaN, when DataCount is 1. Since data marker is not visible for single data.
            if (DataCount == 1)
                spacing = 0;

            for (int i = DataCount - 1; i >= 0; i--)
            {
                double height = 0;
                if (!double.IsNaN(YValues[i]))
                {
                    height = Math.Abs(double.IsNaN(yValues[i]) ? 0 : yValues[i]) * coefHeight;

                    FunnelSegment funnelSegment = CreateSegment() as FunnelSegment;
                    if (funnelSegment != null)
                    {
                        funnelSegment.Series = this;
                        funnelSegment.SetData(currY, currY + height, currY / 2, (currY + height) / 2, MinWidth, ExplodeOffset);
                        funnelSegment.IsExploded = explodedIndex == i || this.ExplodeAll ? true : false;

                        funnelSegment.Item = ActualData[i]; // WPF-14426 Funnel series legend and segment colour is changing while setting emptypoint
                        funnelSegment.XData = xValues[i];
                        funnelSegment.YData = YValues[i];

                        if (ToggledLegendIndex.Contains(i))
                            funnelSegment.IsSegmentVisible = false;
                        else
                            funnelSegment.IsSegmentVisible = true;
                        Segments.Add(funnelSegment);
                    }

                    if (AdornmentsInfo != null && ShowDataLabels)
                    {
                        ChartAdornment adornment = (this.CreateAdornment(this, xValues[i], yValues[i], 0, double.IsNaN(currY) ? 0 : currY + (height + spacing) / 2));
                        adornment.Item = ActualData[i];
                        Adornments.Add(adornment);
                    }

                    currY += height + spacing;
                }
            }
        }

        /// <summary>
        /// To calculate the segments if the pyramid mode is valueisWidth.
        /// </summary>
        private void CalculateValueIsWidthSegments(IList<double> yValues, List<double> xValues, double sumValues, double gapRatio, int count, int explodedIndex)
        {
            currY = 0d;
            if (ToggledLegendIndex.Count > 0)
                count = YValues.Count - ToggledLegendIndex.Count;
            double offset = 1d / (count - 1);
            double height = (1 - gapRatio) / (count - 1);
            for (int i = DataCount - 1; i > 0; i--)
            {
                if (!double.IsNaN(YValues[i]))
                {
                    double w1 = Math.Abs(YValues[i]);
                    double w2 = 0;
                    if (ToggledLegendIndex.Contains(i - 1))
                    {
                        for (int k = i - 2; k >= 0; k--)
                        {
                            if (!(ToggledLegendIndex.Contains(k)))
                            {
                                w2 = Math.Abs(YValues[k]);
                                break;
                            }
                        }
                    }
                    else
                        w2 = Math.Abs(YValues[i - 1]);

                    if (ToggledLegendIndex.Contains(i))
                    {
                        height = 0;
                        w2 = w1;
                    }
                    else
                        height = (1 - gapRatio) / (count - 1);

                    double widthTop = w1 / sumValues;
                    double widthBottom = w2 / sumValues;

                    var funnelSegment = CreateSegment() as FunnelSegment;
                    if (funnelSegment != null)
                    {
                        funnelSegment.Series = this;
                        funnelSegment.SetData(currY, currY + height, (1 - widthTop) / 2, (1 - widthBottom) / 2, MinWidth, ExplodeOffset);
                        funnelSegment.IsExploded = explodedIndex == i || this.ExplodeAll ? true : false;
                        funnelSegment.Item = ActualData[i];
                        funnelSegment.XData = xValues[i];
                        funnelSegment.YData = YValues[i];

                        if (ToggledLegendIndex.Contains(i))
                            funnelSegment.IsSegmentVisible = false;
                        else
                            funnelSegment.IsSegmentVisible = true;

                        Segments.Add(funnelSegment);
                    }
                    if (AdornmentsInfo != null && ShowDataLabels)
                    {
                        Adornments.Add(this.CreateAdornment(this, xValues[i], yValues[i], height, currY));
                        Adornments[Adornments.Count - 1].Item = ActualData[i];
                    }

                    if (!(ToggledLegendIndex.Contains(i)))
                        currY += offset;
                }
            }

            if (AdornmentsInfo != null && ShowDataLabels && DataCount > 0)
            {
                Adornments.Add(this.CreateAdornment(this, xValues[0], yValues[0], height, currY));
                Adornments[Adornments.Count - 1].Item = ActualData[0];
            }
        }

        #endregion

        #endregion
    }
}
