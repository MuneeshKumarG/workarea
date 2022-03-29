using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using System;
using System.Collections.ObjectModel;

namespace Syncfusion.Maui.Charts
{
    /// <summary>
    /// 
    /// </summary>
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

        #endregion

        #region Bindable Properties

        /// <summary>
        /// 
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

        #endregion

        #region Constructor

        /// <summary>
        /// 
        /// </summary>
        public DoughnutSeries() : base()
        {
            PaletteBrushes = ChartColorModel.DefaultBrushes;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// 
        /// </summary>
        public double InnerRadius
        {
            get { return (double)GetValue(InnerRadiusProperty); }
            set { SetValue(InnerRadiusProperty, value); }
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
            if (YValues != null)
            {
                doughnutStartAngle = (float)StartAngle;
                angleDifference = GetAngleDifference();
                total = CalculateTotalYValues(Segments);
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

        internal float GetInnerRadius()
        {
            var actualBounds = GetActualBound();
            return (float)InnerRadius * (Math.Min(actualBounds.Width, actualBounds.Height) / 2);
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

        static object CoerceDoughnutCoefficient(BindableObject bindable, object value)
        {
            double coefficient = Convert.ToDouble(value);
            return coefficient > 1 ? 1 : coefficient < 0 ? 0 : value;
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
