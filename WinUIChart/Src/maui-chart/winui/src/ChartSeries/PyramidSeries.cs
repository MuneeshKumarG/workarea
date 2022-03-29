using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using Microsoft.UI.Xaml;
using Windows.Foundation;
using AdornmentSeries = Syncfusion.UI.Xaml.Charts.DataMarkerSeries;
using ChartAdornment = Syncfusion.UI.Xaml.Charts.ChartDataLabel;

namespace Syncfusion.UI.Xaml.Charts
{
    /// <summary>
    /// Represents a series which displays data in the form of a triangle with lines dividing it into sections and each section has a different width. Depending on the Y coordinates, this width indicates a level of hierarchy among other categories.
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
    ///          <chart:PyramidSeries
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
    ///     PyramidSeries series = new PyramidSeries();
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
    /// <seealso cref="PyramidSegment"/>
    /// <seealso cref="FunnelSegment"/>
    /// <seealso cref="FunnelSeries"/> 
    [Obsolete("Use SfPyramidChart", false)]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1805: Do not initialize unnecessarily")]
    public class PyramidSeries : TriangularSeriesBase, ISegmentSelectable
    {
        #region Dependency Property Registration


        /// <summary>
        /// The DependencyProperty for <see cref="PyramidDataLabelSettings"/> property.
        /// </summary>
        public static readonly DependencyProperty DataLabelSettingsProperty =
          DependencyProperty.Register(nameof(DataLabelSettings), typeof(PyramidDataLabelSettings), typeof(TriangularSeriesBase),
          new PropertyMetadata(null, OnAdornmentsInfoChanged));


        /// <summary>
        /// Identifies the <c>PyramidMode</c> dependency property.
        /// </summary>
        /// <value>
        /// The identifier for <c>PyramidMode</c> dependency property.
        /// </value> 
        public static readonly DependencyProperty PyramidModeProperty =
            DependencyProperty.Register(
                "PyramidMode", 
                typeof(ChartPyramidMode), 
                typeof(PyramidSeries),
                new PropertyMetadata(ChartPyramidMode.Linear, OnPyramidModeChanged));

        #endregion

        #region Fields

        double currY = 0;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="PyramidSeries"/> class.
        /// </summary>
        public PyramidSeries()
        {
            DefaultStyleKey = typeof(PyramidSeries);
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
        ///     <chart:SfPyramidChart ItemsSource="{Binding Data}" XBindingPath="XValue" YBindingPath="YValue" ShowDataLabels="True">
        ///
        ///                    <syncfusion:SfPyramidChart.DataLabelSettings>
        ///                          <chart:PyramidDataLabelSettings/>
        ///                    <syncfusion:SfPyramidChart.DataLabelSettings>
        ///           
        ///     </chart:SfPyramidChart>
        /// ]]></code>
        /// # [MainWindow.cs](#tab/tabid-2)
        /// <code><![CDATA[
        ///     SfPyramidChart chart = new SfPyramidChart();
        ///     
        ///     chart.ItemsSource = viewmodel.Data;
        ///     chart.XBindingPath = "XValue";
        ///     chart.YBindingPath = "YValue";
        ///     chart.ShowDataLabels = "True";
        ///     
        ///     chart.DataLabelSettings = new PyramidDataLabelSettings(){};
        /// ]]></code>
        /// ***
        /// </example>
        public PyramidDataLabelSettings DataLabelSettings
        {
            get
            {
                return (PyramidDataLabelSettings)GetValue(DataLabelSettingsProperty);
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
                return (PyramidDataLabelSettings)GetValue(DataLabelSettingsProperty);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the y value should interpret the length or surface of the pyramid block.
        /// </summary>
        /// <value>
        /// One of the <see cref="ChartPyramidMode"/> enumeration values. The default value is <see cref="Syncfusion.UI.Xaml.Charts.ChartPyramidMode.Linear"/>
        /// </value>
        public ChartPyramidMode PyramidMode
        {
            get { return (ChartPyramidMode)GetValue(PyramidModeProperty); }
            set { SetValue(PyramidModeProperty, value); }
        }

        #endregion

        #endregion

        #region Methods

        #region Public Static Methods

        /// <summary>
        /// To get the SurfaceHeight for PyramidSeries.
        /// </summary>
        private static double GetSurfaceHeight(double y, double surface)
        {
            double r1, r2;
            if (ChartMath.SolveQuadraticEquation(1, 2 * y, -surface, out r1, out r2))
            {
                return Math.Max(r1, r2);
            }

            return double.NaN;
        }

        #endregion

        #region Public Override Methods

        /// <summary>
        /// Creates the segment of PyramidSeries.
        /// </summary>
        public override void CreateSegments()
        {
            Adornments.Clear();
            this.Segments.Clear();
            int count = DataCount;
            List<double> xValues = GetXValues();
            IList<double> toggledYValues = null;
            if (ToggledLegendIndex.Count > 0)
                toggledYValues = GetYValues();
            else
                toggledYValues = YValues;
            double sumValues = 0;
            double gapRatio = this.GapRatio;
            ChartPyramidMode pyramidMode = this.PyramidMode;

            for (int i = 0; i < count; i++)
            {
                sumValues += Math.Max(0, Math.Abs(double.IsNaN(toggledYValues[i]) ? 0 : toggledYValues[i]));
            }

            double gapHeight = gapRatio / (count - 1);
            if (pyramidMode == ChartPyramidMode.Linear)
                this.CalculateLinearSegments(sumValues, gapRatio, count, xValues);
            else
                this.CalculateSurfaceSegments(sumValues, count, gapHeight, xValues);

            if (ShowEmptyPoints)
                UpdateEmptyPointSegments(xValues, false);

            if (ActualArea != null)
                ActualArea.IsUpdateLegend = true;
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
            return new PyramidSegment();
        }

        /// <summary>
        /// Create the datamarker label for pyramid series.
        /// </summary>
        /// <param name="series">Series instance.</param>
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
                foreach (PyramidSegment segment in Segments)
                {
                    int index = ActualData.IndexOf(segment.Item);
                    if (i == index)
                    {
                        segment.isExploded = !segment.isExploded;
                        base.UpdateSegments(i, NotifyCollectionChangedAction.Remove);

                        if (index != ExplodeIndex)
                        {
                            ExplodeIndex = i;
                        }
                    }
                    else if (i == -1)
                    {
                        segment.isExploded = false;
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

        #region Internal Override Methods

        internal override void SetDataLabelsVisibility(bool isShowDataLabels)
        {
            if (DataLabelSettings != null)
            {
                DataLabelSettings.Visible = isShowDataLabels;
            }
        }

        #endregion

        #region Private Static Methods

        private static void OnPyramidModeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            PyramidSeries pyramidSeries = d as PyramidSeries;
            if (pyramidSeries != null && pyramidSeries.Area != null)
                pyramidSeries.Area.ScheduleUpdate();
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// To calculate the segments if the pyramid mode is linear
        /// </summary>
        private void CalculateLinearSegments(double sumValues, double gapRatio, int count, List<double> xValues)
        {
            var toggledYValues = YValues.ToList();
            if (ToggledLegendIndex.Count > 0)
                toggledYValues = GetYValues();
            currY = 0;
            double coef = 1d / (sumValues * (1 + gapRatio / (1 - gapRatio)));

            for (int i = 0; i < count; i++)
            {
                double height = 0;
                if (!double.IsNaN(YValues[i]))
                {
                    height = coef * Math.Abs(double.IsNaN(toggledYValues[i]) ? 0 : toggledYValues[i]);

                    PyramidSegment pyramidSegment = CreateSegment() as PyramidSegment;
                    if (pyramidSegment != null)
                    {
                        pyramidSegment.Series = this;
                        pyramidSegment.SetData(currY, height, ExplodeOffset);
                        pyramidSegment.isExploded = i == ExplodeIndex || this.ExplodeAll ? true : false;
                        pyramidSegment.Item = ActualData[i];
                        pyramidSegment.XData = xValues[i];
                        pyramidSegment.YData = Math.Abs(YValues[i]);

                        if (ToggledLegendIndex.Contains(i))
                            pyramidSegment.IsSegmentVisible = false;
                        else
                            pyramidSegment.IsSegmentVisible = true;
                        this.Segments.Add(pyramidSegment);
                    }
                    currY += (gapRatio / (count - 1)) + height;
                    if (AdornmentsInfo != null && ShowDataLabels)
                    {
                        Adornments.Add(this.CreateAdornment(this, xValues[i], toggledYValues[i], 0, double.IsNaN(currY) ? 1 - height / 2 : currY - height / 2));
                        Adornments[Segments.Count - 1].Item = ActualData[i];
                    }
                }
            }
        }

        /// <summary>
        /// To calculate the segments if the pyramid mode is surface
        /// </summary>
        private void CalculateSurfaceSegments(double sumValues, int count, double gapHeight, List<double> xValues)
        {
            var toggledYValues = YValues.ToList();
            if (ToggledLegendIndex.Count > 0)
                toggledYValues = GetYValues();
            currY = 0;
            double[] y = new double[count];
            double[] height = new double[count];
            double preSum = GetSurfaceHeight(0, sumValues);

            for (int i = 0; i < count; i++)
            {
                y[i] = currY;
                height[i] = GetSurfaceHeight(currY, Math.Abs(double.IsNaN(toggledYValues[i]) ? 0 : toggledYValues[i]));
                currY += height[i] + gapHeight * preSum;
            }

            double coef = 1 / (currY - gapHeight * preSum);
            for (int i = 0; i < count; i++)
            {
                double currHeight = 0;
                if (!double.IsNaN(YValues[i]))
                {
                    currHeight = coef * y[i];

                    var pyramidSegment = CreateSegment() as PyramidSegment;
                    if (pyramidSegment != null)
                    {
                        pyramidSegment.Series = this;
                        pyramidSegment.SetData(currHeight, coef * height[i], ExplodeOffset);
                        pyramidSegment.isExploded = i == this.ExplodeIndex || this.ExplodeAll ? true : false;
                        pyramidSegment.Item = ActualData[i];
                        pyramidSegment.XData = xValues[i];
                        pyramidSegment.YData = Math.Abs(YValues[i]);

                        if (ToggledLegendIndex.Contains(i))
                            pyramidSegment.IsSegmentVisible = false;
                        else
                            pyramidSegment.IsSegmentVisible = true;
                        this.Segments.Add(pyramidSegment);
                    }

                    if (AdornmentsInfo != null && ShowDataLabels)
                    {
                        Adornments.Add(this.CreateAdornment(this, xValues[i], toggledYValues[i], 0, double.IsNaN(currHeight) ? 1 - height[i] / 2 : currHeight + coef * height[i] / 2));
                    }
                }
            }
        }

        #endregion

        #endregion
    }
}
