using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using System;

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
    ///               <chart:PieSeries
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
    ///     PieSeries series = new PieSeries();
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
    /// <seealso cref="DoughnutSeries"/>
    /// <seealso cref="PieSegment"/>
    public class PieSeries : CircularSeries
    {
        #region Fields
        double total = 0;
        float yValue;
        float pieEndAngle;
        float pieStartAngle;
        double angleDifference;
        #endregion

        #region Bindable Properties

        /// <summary>
        /// 
        /// </summary>        
        public static readonly BindableProperty ExplodeIndexProperty =
                BindableProperty.Create(
                nameof(ExplodeIndex),
                typeof(int),
                typeof(PieSeries),
                -1,
                BindingMode.TwoWay,
                null,
                OnExplodeIndexChanged);

        /// <summary>
        /// 
        /// </summary>
        public static readonly BindableProperty ExplodeRadiusProperty =
            BindableProperty.Create(
                nameof(ExplodeRadius),
                typeof(double),
                typeof(PieSeries),
                10d,
                BindingMode.Default,
                null,
                OnExplodeRadiusChanged);

        #endregion

        #region Constructor

        /// <summary>
        /// 
        /// </summary>
        public PieSeries() : base()
        {
            PaletteBrushes = ChartColorModel.DefaultBrushes;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// 
        /// </summary>
        public int ExplodeIndex
        {
            get { return (int)GetValue(ExplodeIndexProperty); }
            set { SetValue(ExplodeIndexProperty, value); }
        }

        /// <summary>
        /// 
        /// </summary>
        public double ExplodeRadius
        {
            get { return (double)GetValue(ExplodeRadiusProperty); }
            set { SetValue(ExplodeRadiusProperty, value); }
        }

        #endregion

        #region Methods

        #region Protected Methods

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        protected override ChartSegment CreateSegment()
        {
            return new PieSegment();
        }

        #endregion

        #region Internal Methods

        internal override void GenerateSegments(SeriesView seriesView)
        {
            if (YValues != null)
            {
                pieStartAngle = (float)StartAngle;
                angleDifference = GetAngleDifference();
                total = CalculateTotalYValues(Segments);
                var oldSegments = OldSegments != null && OldSegments.Count > 0 && PointsCount == OldSegments.Count ? OldSegments : null;

                for (int i = 0; i < YValues.Count; i++)
                {
                    yValue = (float)YValues[i];
                    pieEndAngle = (float)(Math.Abs(float.IsNaN(yValue) ? 0 : yValue) * (angleDifference / total));

                    if (i < Segments.Count && Segments[i] is PieSegment)
                    {
                        ((PieSegment)Segments[i]).SetData(pieStartAngle, pieEndAngle, yValue);
                    }
                    else
                    {
                        PieSegment pieSegment = (PieSegment)CreateSegment();
                        pieSegment.Series = this;
                        pieSegment.SeriesView = seriesView;
                        pieSegment.Index = i;
                        pieSegment.Exploded = ExplodeIndex == i;
                        pieSegment.SetData(pieStartAngle, pieEndAngle, yValue);
                        Segments.Add(pieSegment);

                        if (oldSegments != null)
                        {
                            PieSegment? oldSegment = oldSegments[i] as PieSegment;

                            if (oldSegment != null)
                                pieSegment.SetPreviousData(new[] { oldSegment.StartAngle, oldSegment.EndAngle });
                        }
                    }

                    if (Segments[i].IsVisible)
                    {
                        pieStartAngle += pieEndAngle;
                    }
                }
            }
        }

        internal override float GetDataLabelRadius()
        {
            float radius = DataLabelSettings.LabelPlacement == DataLabelPlacement.Inner || DataLabelSettings.LabelPlacement == DataLabelPlacement.Auto || DataLabelSettings.LabelPlacement == DataLabelPlacement.Center ? GetRadius() / 2 : GetRadius();
            return radius;
        }

        internal virtual float GetTooltipRadius()
        {
            return GetRadius() / 2;
        }

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
            var pieSegment = Segments[index] as PieSegment;

            if (pieSegment == null) return null;

            float segmentRadius = GetTooltipRadius();
            segmentRadius = pieSegment.Index == ExplodeIndex ? segmentRadius + (float)ExplodeRadius : segmentRadius;
            PointF center = Center;
            double midAngle = (pieSegment.StartAngle + (pieSegment.EndAngle / 2)) * 0.0174532925f;
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

        internal override void SetTooltipTargetRect(TooltipInfo tooltipInfo, Rect seriesBounds)
        {
            float xPosition = tooltipInfo.X;
            float yPosition = tooltipInfo.Y;
            float sizeValue = 1;
            float noseOffset = 2;
            float halfSizeValue = 0.5f;

            Rect targetRect = new Rect(xPosition - halfSizeValue, yPosition + noseOffset, sizeValue, sizeValue);
            tooltipInfo.TargetRect = targetRect;
        }

        internal void UpdateExplode()
        {
            for (int i = 0; i < Segments.Count; i++)
            {
                var segment = (PieSegment)Segments[i];
                segment.Exploded = ExplodeIndex == i;
            }
        }

        #endregion

        #region Private Methods

        private static void OnExplodeIndexChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var series = bindable as PieSeries;

            if (series != null && series.AreaBounds != Rect.Zero)
            {
                series.OnExplodePropertiesChanged();
            }
        }

        private static void OnExplodeRadiusChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var series = bindable as PieSeries;

            if (series != null && series.AreaBounds != Rect.Zero)
            {
                series.OnExplodePropertiesChanged();
            }
        }

        private void OnExplodePropertiesChanged()
        {
            UpdateExplode();
            Invalidate();
            InvalidateSeries();

            if (ShowDataLabels)
                InvalidateDataLabel();
        }

        #endregion

        #endregion
    }
}
