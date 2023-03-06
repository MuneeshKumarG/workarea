using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Layouts;
using System;
using System.Collections.ObjectModel;

namespace Syncfusion.Maui.Charts
{
    /// <summary>
    /// The <see cref="DoughnutSeries"/> displays data as a proportion of the whole. Its most commonly used to make comparisons among a set of given data.
    /// </summary>
    /// <remarks>
    /// <para>It is similar to the PieSeries. To render a series, create an instance of the doughnut series class, and add it to the <see cref="SfCircularChart.Series"/> collection.</para>
    /// 
    /// <para>It Provides options for <see cref="ChartSeries.PaletteBrushes"/>, <see cref="ChartSeries.Fill"/>, <see cref="CircularSeries.Stroke"/>, <see cref="CircularSeries.StrokeWidth"/>, and <see cref="InnerRadius"/> to customize the appearance.</para>
    /// 
    /// <para> <b>EnableTooltip - </b> The tooltip displays information while tapping or mouse hovering on the segment. To display the tooltip on the chart, you need to set the <see cref="ChartSeries.EnableTooltip"/> property as <b>true</b> in <see cref="DoughnutSeries"/> and refer to the <seealso cref="ChartBase.TooltipBehavior"/> property.</para>
    /// <para> <b>Data Label - </b> Data labels are used to display values related to a chart segment. To render the data labels, you need to set the <see cref="ChartSeries.ShowDataLabels"/> property as <b>true</b> in the <see cref="DoughnutSeries"/> class. To customize the chart data labels’ alignment, placement, and label styles, you need to create an instance of <see cref="CircularDataLabelSettings"/> and set it to the <see cref="CircularSeries.DataLabelSettings"/> property. </para>
    /// <para> <b>Animation - </b> To animate the series, set <b>True</b> to the <see cref="ChartSeries.EnableAnimation"/> property.</para>
    /// <para> <b>Selection - </b> To enable the data point selection in the series, create an instance of the <see cref="DataPointSelectionBehavior"/> and set it to the <see cref="ChartSeries.SelectionBehavior"/> property of the doughnut series. To highlight the selected segment, set the value for the <see cref="ChartSelectionBehavior.SelectionBrush"/> property in the <see cref="DataPointSelectionBehavior"/> class.</para>
    /// <para> <b>LegendIcon - </b> To customize the legend icon using the <see cref="ChartSeries.LegendIcon"/> property.</para>
    /// 
    /// </remarks>
    /// <example>
    /// # [Xaml](#tab/tabid-1)
    /// <code><![CDATA[
    ///     <chart:SfCircularChart>
    ///
    ///           <chart:SfCircularChart.Series>
    ///               <chart:DoughnutSeries
    ///                   ItemsSource="{Binding Data}"
    ///                   XBindingPath="XValue"
    ///                   YBindingPath="YValue"/>
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
    ///     DoughnutSeries series = new DoughnutSeries();
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
    /// <seealso cref="DoughnutSegment"/>
    /// <seealso cref="PieSeries"/>
    /// <seealso cref="PieSegment"/>
    public class DoughnutSeries : PieSeries
    {
        #region Fields

        float doughnutStartAngle;
        float doughnutEndAngle;
        double total = 0;
        double angleDifference;
        float yValue;
        double centerHoleSize = 1;

        #endregion

        #region Bindable Properties

        /// <summary>
        /// Identifies the <see cref="InnerRadius"/> bindable property.
        /// </summary>        
        public static readonly BindableProperty InnerRadiusProperty =
            BindableProperty.Create(
                nameof(InnerRadius),
                typeof(double),
                typeof(DoughnutSeries),
                0.4d,
                BindingMode.Default,
                null,
                OnInnerRadiusPropertyChanged,
                null,
                coerceValue: CoerceDoughnutCoefficient);

        /// <summary>
        /// Identifies the <see cref="CenterView"/> bindable property.
        /// </summary>
        public static readonly BindableProperty CenterViewProperty =
            BindableProperty.Create(
                nameof(CenterView),
                typeof(View),
                typeof(DoughnutSeries),
                null,
                BindingMode.Default,
                propertyChanged: OnCenterViewChanged);

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="DoughnutSeries"/> class.
        /// </summary>
        public DoughnutSeries() : base()
        {
            PaletteBrushes = ChartColorModel.DefaultBrushes;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets a value that can be used to define the inner circle.
        /// </summary>
        /// <value>It accepts double values, and the default value is 0.4. Here, the value is between 0 and 1.</value>
        /// <example>
        /// # [Xaml](#tab/tabid-1)
        /// <code><![CDATA[
        ///     <chart:SfCircularChart>
        ///
        ///          <chart:DoughnutSeries ItemsSource="{Binding Data}"
        ///                                XBindingPath="XValue"
        ///                                YBindingPath="YValue"
        ///                                InnerRadius = "0.5"/>
        ///
        ///     </chart:SfCircularChart>
        /// ]]></code>
        /// # [C#](#tab/tabid-2)
        /// <code><![CDATA[
        ///     SfCircularChart chart = new SfCircularChart();
        ///     ViewModel viewModel = new ViewModel();
        ///
        ///     DoughnutSeries series = new DoughnutSeries()
        ///     {
        ///           ItemsSource = viewModel.Data,
        ///           XBindingPath = "XValue",
        ///           YBindingPath = "YValue",
        ///           InnerRadius = 0.5,
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
        /// Gets or sets the view to be added to the center of the doughnut.
        /// </summary>
        /// <value>Defaults to null.</value> 
        /// 
        /// <img src="https://cdn.syncfusion.com/content/images/maui/MAUIDoughnutCenterView.png"/>
        /// 
        /// <para>Code example for the doughnut center view.</para>
        /// 
        /// # [MainPage.xaml](#tab/tabid-4)
        /// <code><![CDATA[
        /// <chart:SfCircularChart>
        /// 
        ///         <chart:SfCircularChart.BindingContext>
        ///             <local:DoughnutSeriesViewModel/>
        ///         </chart:SfCircularChart.BindingContext>    
        /// 
        ///         <chart:SfCircularChart.Series>
        ///             <chart:DoughnutSeries ItemsSource="{Binding DoughnutSeriesData}" XBindingPath="Name" YBindingPath="Value"/>
        ///                  <chart:DoughnutSeries.CenterView>
        ///                     <StackLayout x:Name="layout" HeightRequest="{Binding CenterHoleSize}" WidthRequest="{Binding CenterHoleSize}">
        ///                         <Label Text = "{Binding Name,Source={x:Reference doughnutViewModel}}"/>
        ///                         <Label Text = "{Binding Value,Source={x:Reference doughnutViewModel},StringFormat='{0} %'}"/>
        ///                     </StackLayout>
        ///                 </chart:DoughnutSeries.CenterView>
        ///         </chart:SfCircularChart.Series>
        /// 
        /// </chart:SfCircularChart>
        /// ]]>
        /// </code>
        /// 
        /// # [MainPage.xaml.cs](#tab/tabid-5)
        /// <code><![CDATA[
        ///  SfCircularChart chart = new SfCircularChart();
        ///  
        ///  DoughnutSeriesViewModel viewModel = new DoughnutSeriesViewModel();
        ///	 chart.BindingContext = viewModel;
        ///  
        ///  DoughnutSeries series = new DoughnutSeries()
        ///  {
        ///     ItemsSource = viewmodel.DoughnutSeriesData,
        ///     XBindingPath = "Name",
        ///     YBindingPath = "Value",
        ///  };
        ///  
        ///  Label name = new Label();
        ///  Label value = new Label(); 
        ///  StackLayout layout = new StackLayout();
        ///  layout.Children.Add(name);
        ///  layout.Children.Add(value);
        ///  
        ///  name.SetBinding(Label.TextProperty, nameof(doughnutViewModel.Name));
        ///  value.SetBinding(Label.TextProperty, new Binding(nameof(doughnutViewModel.Value),default,null,null,"0%"));
        ///  layout.SetBinding(StackLayout.HeightRequestProperty, nameof(DoughnutSeries.CenterHoleSize));
        ///  layout.SetBinding(StackLayout.WidthRequestProperty, nameof(DoughnutSeries.CenterHoleSize));
        ///  
        ///  series.CenterView = layout;
        ///  chart.Series.Add(series);
        /// ]]>
        /// </code>
        /// ***
        /// 
        public View CenterView
        {
            get { return (View)GetValue(CenterViewProperty); }
            set { SetValue(CenterViewProperty, value); }
        }

        /// <summary>
        /// Gets the size of the doughnut center hole.
        /// </summary>
        /// <value>Default value is 1.</value>
        /// 
        /// <para>It used to customize the view size which is placed in the doughnut <see cref="CenterView"/>.</para>
        /// 
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

        #region Methods

        #region Protected Methods

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        protected override ChartSegment CreateSegment()
        {
            return new DoughnutSegment();
        }

        #endregion

        #region Internal Methods

        internal override void GenerateSegments(SeriesView seriesView)
        {
            GetActualXValues();

            if (YValues != null && YValues.Count > 0)
            {
                doughnutStartAngle = (float)StartAngle;
                angleDifference = GetAngleDifference();
                total = CalculateTotalYValues();
                var oldSegments = OldSegments != null && OldSegments.Count > 0 && PointsCount == OldSegments.Count ? OldSegments : null;

                for (int i = 0; i < PointsCount; i++)
                {
                    yValue = (float)YValues[i];
                    doughnutEndAngle = (float)(Math.Abs(float.IsNaN(yValue) ? 0 : yValue) * (angleDifference / total));

                    if (i < Segments.Count && Segments[i] is DoughnutSegment)
                    {
                        ((DoughnutSegment)Segments[i]).SetData(doughnutStartAngle, doughnutEndAngle, yValue);
                    }
                    else
                    {
                        DoughnutSegment doughnutSegment = (DoughnutSegment)CreateSegment();
                        doughnutSegment.Series = this;
                        doughnutSegment.SeriesView = seriesView;
                        doughnutSegment.Index = i;
                        doughnutSegment.Exploded = ExplodeIndex == i;
                        doughnutSegment.SetData(doughnutStartAngle, doughnutEndAngle, yValue);
                        Segments.Add(doughnutSegment);

                        if (oldSegments != null)
                        {
                            var oldSegment = oldSegments[i] as DoughnutSegment;

                            if (oldSegment != null)
                                doughnutSegment.SetPreviousData(new[] { oldSegment.StartAngle, oldSegment.EndAngle });
                        }
                    }

                    if (Segments[i].IsVisible)
                        doughnutStartAngle += doughnutEndAngle;
                }
            }
        }

        internal override void OnSeriesLayout()
        {
            CenterHoleSize = GetCenterHoleSize();
            UpdateCenterViewBounds(CenterView);
            base.OnSeriesLayout();
        }

        internal override float GetDataLabelRadius()
        {
            float innerRadius = GetInnerRadius();
            float radius = DataLabelSettings.LabelPlacement == DataLabelPlacement.Inner || DataLabelSettings.LabelPlacement == DataLabelPlacement.Auto || DataLabelSettings.LabelPlacement == DataLabelPlacement.Center ? ((GetRadius() - innerRadius) / 2) + innerRadius : GetRadius();
            return radius;
        }

        internal override float GetTooltipRadius()
        {
            float innerRadius = GetInnerRadius();
            return ((GetRadius() - innerRadius) / 2) + innerRadius;
        }

        internal override void OnDetachedToChart()
        {
            RemoveCenterView(CenterView);
        }

        internal override void OnAttachedToChart()
        {
            AddCenterView();
        }
        #endregion

        #region Private Methods

        private static void OnInnerRadiusPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (oldValue == newValue) return;
            var series = bindable as DoughnutSeries;
            if (series != null)
            {
                series.ScheduleUpdateChart();
            }
        }

        private static void OnCenterViewChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is DoughnutSeries series)
            {
                if (oldValue is View oldView)
                {
                    series.RemoveCenterView(oldView);
                }

                if (newValue is View newView)
                {
                    series.AddCenterView();
                    if (series.Chart != null)
                    {
                        series.UpdateCenterViewBounds(newView);
                    }
                }
            }
        }

        static object CoerceDoughnutCoefficient(BindableObject bindable, object value)
        {
            double coefficient = Convert.ToDouble(value);
            return coefficient > 1 ? 1 : coefficient < 0 ? 0 : value;
        }

        private void AddCenterView()
        {
            var plotArea = ChartArea?.PlotArea as ChartPlotArea;
            if (plotArea != null && CenterView != null)
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

        private float GetInnerRadius()
        {
            var actualBounds = GetActualBound();
            return (float)InnerRadius * (Math.Min(actualBounds.Width, actualBounds.Height) / 2);
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
