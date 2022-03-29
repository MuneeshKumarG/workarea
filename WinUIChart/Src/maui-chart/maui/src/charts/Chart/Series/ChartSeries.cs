using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace Syncfusion.Maui.Charts
{
    /// <summary>
    /// 
    /// </summary>
    public abstract partial class ChartSeries : BindableObject
    {

        #region Bindable Properties
        /// <summary>
        /// 
        /// </summary>        
        public static readonly BindableProperty ItemsSourceProperty =
            BindableProperty.Create(nameof(ItemsSource), typeof(object), typeof(ChartSeries), null, BindingMode.Default, null, OnItemsSourceChanged);

        /// <summary>
        /// 
        /// </summary>        
        public static readonly BindableProperty XBindingPathProperty =
            BindableProperty.Create(nameof(XBindingPath), typeof(string), typeof(ChartSeries), null, BindingMode.Default, null, OnXBindingPathChanged);

        /// <summary>
        /// 
        /// </summary>        
        public static readonly BindableProperty FillProperty =
            BindableProperty.Create(nameof(Fill), typeof(Brush), typeof(ChartSeries), null, BindingMode.Default, null, OnFillPropertyChanged);

        /// <summary>
        /// 
        /// </summary>        
        public static readonly BindableProperty PaletteBrushesProperty =
            BindableProperty.Create(nameof(PaletteBrushes), typeof(IList<Brush>), typeof(ChartSeries), null, BindingMode.Default, null, OnPaletteBrushesChanged);

        /// <summary>
        /// 
        /// </summary>        
        public static readonly BindableProperty IsVisibleProperty =
            BindableProperty.Create(nameof(IsVisible), typeof(bool), typeof(ChartSeries), true, BindingMode.Default, null, OnVisiblePropertyChanged);

        /// <summary>
        /// 
        /// </summary>        
        public static readonly BindableProperty OpacityProperty =
            BindableProperty.Create(nameof(Opacity), typeof(double), typeof(ChartSeries), 1d, BindingMode.Default, null, OnOpacityPropertyChanged);

        /// <summary>
        /// 
        /// </summary>        
        public static readonly BindableProperty EnableAnimationProperty =
            BindableProperty.Create(nameof(EnableAnimation), typeof(bool), typeof(ChartSeries), false, BindingMode.Default, null, OnEnableAnimationPropertyChanged);

        /// <summary>
        /// 
        /// </summary>     
        public static readonly BindableProperty EnableTooltipProperty =
           BindableProperty.Create(nameof(EnableTooltip), typeof(bool), typeof(ChartSeries), false, BindingMode.Default, null, OnEnableTooltipPropertyChanged);

        /// <summary>
        /// 
        /// </summary>  
        public static readonly BindableProperty TooltipTemplateProperty =
           BindableProperty.Create(nameof(TooltipTemplate), typeof(DataTemplate), typeof(ChartSeries), null, BindingMode.Default, null, null);

        /// <summary>
        /// 
        /// </summary>
        public static readonly BindableProperty SelectionBrushProperty =
            BindableProperty.Create(nameof(SelectionBrush), typeof(Brush), typeof(ChartSeries), null, BindingMode.Default, null, OnSelectionColorPropertyChanged);

        /// <summary>
        /// 
        /// </summary>
        public static readonly BindableProperty SelectedIndexProperty =
            BindableProperty.Create(nameof(SelectedIndex), typeof(int), typeof(ChartSeries), -1, BindingMode.Default, null, OnSelectedIndexPropertyChanged);

        /// <summary>
        /// 
        /// </summary>
        public static readonly BindableProperty ShowDataLabelsProperty =
            BindableProperty.Create(nameof(ShowDataLabels), typeof(bool), typeof(ChartSeries), false, BindingMode.Default, null, OnShowDataLabelsChanged);

        /// <summary>
        /// 
        /// </summary>        
        public static readonly BindableProperty LegendIconProperty =
            BindableProperty.Create(nameof(LegendIcon), typeof(ChartLegendIconType), typeof(ChartSeries), ChartLegendIconType.Circle, BindingMode.Default, null, OnLegendIconChanged);

        /// <summary>
        /// 
        /// </summary>        
        public static readonly BindableProperty IsVisibleOnLegendProperty =
            BindableProperty.Create(
                nameof(IsVisibleOnLegend),
                typeof(bool),
                typeof(ChartSeries),
                true,
                BindingMode.Default,
                null,
                null);

        /// <summary>
        /// 
        /// </summary>        
        internal static readonly BindableProperty AnimationDurationProperty =
            BindableProperty.Create(nameof(AnimationDuration), typeof(double), typeof(ChartSeries), 1d, BindingMode.Default, null, null);
        #endregion

        #region Constructor

        /// <summary>
        /// 
        /// </summary>
        public ChartSeries()
        {
            Segments = new ObservableCollection<ChartSegment>();
            Segments.CollectionChanged += Segments_CollectionChanged;
        }

        #endregion

        #region Public Properties
        /// <summary>
        /// 
        /// </summary>
        public object ItemsSource
        {
            get { return (object)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        /// <summary>
        /// 
        /// </summary>
        public string XBindingPath
        {
            get { return (string)GetValue(XBindingPathProperty); }
            set { SetValue(XBindingPathProperty, value); }
        }

        /// <summary>
        /// 
        /// </summary>
        public Brush Fill
        {
            get { return (Brush)GetValue(FillProperty); }
            set { SetValue(FillProperty, value); }
        }

        /// <summary>
        /// 
        /// </summary>
        public IList<Brush> PaletteBrushes
        {
            get { return (IList<Brush>)GetValue(PaletteBrushesProperty); }
            set { SetValue(PaletteBrushesProperty, value); }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsVisible
        {
            get { return (bool)GetValue(IsVisibleProperty); }
            set { SetValue(IsVisibleProperty, value); }
        }

        /// <summary>
        /// 
        /// </summary>
        public double Opacity
        {
            get { return (double)GetValue(OpacityProperty); }
            set { SetValue(OpacityProperty, value); }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool EnableAnimation
        {
            get { return (bool)GetValue(EnableAnimationProperty); }
            set { SetValue(EnableAnimationProperty, value); }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool EnableTooltip
        {
            get { return (bool)GetValue(EnableTooltipProperty); }
            set { SetValue(EnableTooltipProperty, value); }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <example>
        /// # [MainWindow.xaml](#tab/tabid-1)
        /// <code><![CDATA[
        ///     <chart:SfCartesianChart>
        ///
        ///           <chart:SfCartesianChart.Resources>
        ///               <DataTemplate x:Key="tooltipTemplate1">
        ///                  <StackLayout Orientation = "Horizontal" >
        ///                     <Label Text="{Binding Item.XValue}" TextColor="Black" FontAttributes="Bold" FontSize="12" HorizontalOptions="Center" VerticalOptions="Center"/>
        ///                     <Label Text = " : " TextColor="Black" FontAttributes="Bold" FontSize="12" HorizontalOptions="Center" VerticalOptions="Center"/>
        ///                     <Label Text = "{Binding Item.YValue}" TextColor="Black" FontAttributes="Bold" FontSize="12" HorizontalOptions="Center" VerticalOptions="Center"/>
        ///                  </StackLayout>
        ///               </DataTemplate>
        ///           </chart:SfCartesianChart.Resources>
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
        ///               <chart:ColumnSeries ItemsSource="{Binding Data}" XBindingPath="XValue" YBindingPath="YValue"
        ///               EnableTooltip="True" TooltipTemplate="{StaticResource tooltipTemplate1}">
        ///               </chart:ColumnSeries> 
        ///           </chart:SfCartesianChart.Series>
        ///           
        ///     </chart:SfCartesianChart>
        /// ]]></code>
        /// ***
        /// </example>
        public DataTemplate TooltipTemplate
        {
            get { return (DataTemplate)GetValue(TooltipTemplateProperty); }
            set { SetValue(TooltipTemplateProperty, value); }
        }

        /// <summary>
        /// 
        /// </summary>
        public Brush SelectionBrush
        {
            get { return (Brush)GetValue(SelectionBrushProperty); }
            set { SetValue(SelectionBrushProperty, value); }
        }

        /// <summary>
        /// 
        /// </summary>
        public int SelectedIndex
        {
            get { return (int)GetValue(SelectedIndexProperty); }
            set { SetValue(SelectedIndexProperty, value); }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <example>
        /// # [MainWindow.xaml](#tab/tabid-1)
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
        ///               <chart:ColumnSeries ItemsSource="{Binding Data}" XBindingPath="XValue" YBindingPath="YValue" ShowDataLabels="True">
        ///               </chart:ColumnSeries> 
        ///           </chart:SfCartesianChart.Series>
        ///           
        ///     </chart:SfCartesianChart>
        /// ]]></code>
        /// # [MainWindow.cs](#tab/tabid-2)
        /// <code><![CDATA[
        ///     SfCartesianChart chart = new SfCartesianChart();
        ///     
        ///     NumericalAxis xAxis = new NumericalAxis();
        ///     NumericalAxis yAxis = new NumericalAxis();
        ///     
        ///     chart.XAxes.Add(xAxis);
        ///     chart.YAxes.Add(yAxis);
        ///     
        ///     ColumnSeries series = new ColumnSeries();
        ///     series.ItemsSource = viewmodel.Data;
        ///     series.XBindingPath = "XValue";
        ///     series.YBindingPath = "YValue";
        ///     series.ShowDataLabels = "True";
        ///     chart.Series.Add(series);
        ///     
        /// ]]></code>
        /// ***
        /// </example>
        public bool ShowDataLabels
        {
            get { return (bool)GetValue(ShowDataLabelsProperty); }
            set { SetValue(ShowDataLabelsProperty, value); }
        }

        /// <summary>
        /// 
        /// </summary>
        public ChartLegendIconType LegendIcon
        {
            get { return (ChartLegendIconType)GetValue(LegendIconProperty); }
            set { SetValue(LegendIconProperty, value); }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsVisibleOnLegend
        {
            get { return (bool)GetValue(IsVisibleOnLegendProperty); }
            set { SetValue(IsVisibleOnLegendProperty, value); }
        }

        /// <summary>
        /// 
        /// </summary>
        public DoubleRange XRange { get; internal set; } = DoubleRange.Empty;

        /// <summary>
        /// 
        /// </summary>
        public DoubleRange YRange { get; internal set; } = DoubleRange.Empty;

        /// <summary>
        /// 
        /// </summary>
        internal double AnimationDuration
        {
            get { return (double)GetValue(AnimationDurationProperty); }
            set { SetValue(AnimationDurationProperty, value); }
        }
        #endregion

        #region Methods

        #region Protected Methods

        /// <summary>
        /// 
        /// </summary>
        protected abstract ChartSegment? CreateSegment();

        /// <summary>
        ///
        /// </summary>
        protected virtual Animation? CreateAnimation(Action<double> callback)
        {
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        internal void Animate()
        {
            if (EnableAnimation && Segments != null && Segments.Count > 0)
            {
                SeriesView? seriesView = Segments[0].SeriesView;

                if (seriesView != null)
                    seriesView.Animate();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        protected internal virtual void DrawDataLabel(ICanvas canvas, Brush? fillcolor, string label, PointF point, int index)
        {
            ChartDataLabelStyle? dataLabelStyle = ChartDataLabelSettings?.LabelStyle;
            var datalabel = Segments[index];

            if (!string.IsNullOrEmpty(label) && dataLabelStyle != null && ChartDataLabelSettings != null && datalabel != null)
            {
                dataLabelStyle.DrawBackground(canvas, label, fillcolor, point);

                ChartLabelStyle style = dataLabelStyle;
                Color fontColor = dataLabelStyle.TextColor;
                if (fontColor == null || fontColor == Colors.Transparent)
                {
                    fontColor = ChartDataLabelSettings.GetContrastTextColor(this, fillcolor, datalabel.Fill);
                    var textColor = fontColor.WithAlpha(NeedToAnimateDataLabel ? AnimationValue : 1);

                    //Created new font family, as need to pass contrast text color for native font family rendering.
                    style = dataLabelStyle.Clone();
                    style.TextColor = textColor;
                }

                style.DrawLabel(canvas, label, point);
            }

            if (dataLabelStyle?.Angle != 0)
            {
                canvas.RestoreState();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        protected internal virtual void DrawSeries(ICanvas canvas, ReadOnlyObservableCollection<ChartSegment> segments, RectF clipRect)
        {
            //TODO: Faced issue while having animation in WinUI.
#if WINDOWS
            foreach (var segment in segments)
            {
                canvas.SaveState();
                segment.Draw(canvas);
                canvas.RestoreState();
            }
#else
            canvas.SaveState();
            foreach (var segment in segments)
            {
                segment.Draw(canvas);
            }

            canvas.RestoreState();
#endif
        }

        #endregion

        #region Private Static Methods

        private static void OnItemsSourceChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var chartSeries = bindable as ChartSeries;
            if (chartSeries != null)
            {
                chartSeries.OnItemsSourceChanged(oldValue, newValue);
            }
        }

        private static void OnXBindingPathChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var chartSeries = bindable as ChartSeries;
            if (chartSeries != null)
            {
                if (newValue != null && newValue is string)
                {
                    chartSeries.XComplexPaths = ((string)newValue).Split(new char[] { '.' });
                }

                chartSeries.OnBindingPathChanged();
            }
        }

        private static void OnFillPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var chartSeries = bindable as ChartSeries;

            if (chartSeries !=  null)
            {
                chartSeries.UpdateColor();
                chartSeries.InvalidateSeries();
                if (chartSeries.ShowDataLabels)
                {
                    chartSeries.InvalidateDataLabel();
                }
                chartSeries.UpdateLegendIconColor();
            }
        }

        private static void OnOpacityPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var chartSeries = bindable as ChartSeries;

            if (chartSeries != null)
            {
                chartSeries.UpdateAlpha();
                chartSeries.InvalidateSeries();
            }
        }

        private static void OnPaletteBrushesChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (Equals(oldValue, newValue))
            {
                return;
            }

            var chartSeries = bindable as ChartSeries;

            if (chartSeries != null)
            {
                chartSeries.PaletteColors = (IList<Brush>)newValue;

                chartSeries.OnCustomBrushesChanged(oldValue as ObservableCollection<Brush>, newValue as ObservableCollection<Brush>);

                if (chartSeries.AreaBounds != Rect.Zero)//Not to call at load time
                {
                    chartSeries.PaletteColorsChanged();
                }
            }
        }

        private static void OnVisiblePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var chartSeries = bindable as ChartSeries;

            //TODO:Need to move this code to CartesianSeries class.
            if (chartSeries != null && chartSeries.IsSideBySide && chartSeries.chart?.Area is CartesianChartArea chartArea)
            {
                chartArea.ResetSBSSegments();
            }

            chartSeries?.ScheduleUpdateChart();
        }

        private static void OnEnableTooltipPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            
        }

        private static void OnEnableAnimationPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var chartSeries = bindable as ChartSeries;

            if (chartSeries != null)
            {
                chartSeries.OnAnimationPropertyChanged();
            }
        }

        private static void OnSelectionColorPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var chartSeries = bindable as ChartSeries;
            if (chartSeries != null)
            {
                chartSeries.DataPointSelection();
            }
        }

        private static void OnSelectedIndexPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var chartSeries = bindable as ChartSeries;
            if (chartSeries != null)
            {
                if (oldValue != null)
                {
                    chartSeries.PreviousSelectedIndex = (int)oldValue;
                }

                if (newValue != null)
                {
                    int index = (int)newValue;
                    if (chartSeries.ActualSelectedIndex == index)
                    {
                        return;
                    }

                    chartSeries.UpdateOnSelection();
                }
            }
        }

        private static void OnShowDataLabelsChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var chartSeries = bindable as ChartSeries;

            if (chartSeries != null && chartSeries.Chart != null)
            {
                if ((bool)newValue)
                {
                    chartSeries.InvalidateMeasureDataLabel();
                }

                chartSeries.InvalidateDataLabel();
            }
        }

        private static void OnLegendIconChanged(BindableObject bindable, object oldValue, object newValue)
        {
            
        }

#endregion

#region Private Methods

        private void OnItemsSourceChanged(object oldValue, object newValue)
        {
            if (Equals(oldValue, newValue))
            {
                return;
            }

            if (EnableAnimation && AnimationDuration > 0 && Segments != null && Segments.Count > 0)
            {
                OldSegments = new ObservableCollection<ChartSegment>(Segments);
                PreviousXRange = XRange;
                Segments[0].SeriesView?.AbortAnimation();
            }

            UpdateLegendItems();
            NeedToAnimateSeries = EnableAnimation;
            ResetData();
            OnDataSourceChanged(oldValue, newValue);
            HookAndUnhookCollectionChangedEvent(oldValue, newValue);
            SegmentsCreated = false;
            ScheduleUpdateChart();
        }

        private void Segments_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            e.ApplyCollectionChanges((index, obj) => AddSegment(obj), (index, obj) => RemoveSegment(obj), ResetSegment);
        }

        private void AddSegment(object chartSegment)
        {
            var segment = chartSegment as ChartSegment;

            if (segment != null)
            {
                SetFillColor(segment);
                SetStrokeColor(segment);
                SetStrokeWidth(segment);
                SetDashArray(segment);
                segment.Opacity = (float)Opacity;
            }
        }

        private void RemoveSegment(object chartSegment)
        {
            //Todo: Need to consider this case later.
        }

        private void ResetSegment()
        {
            //Todo: Need to consider this case later.
        }

        private void UpdateOnSelection()
        {
            if (PreviousSelectedIndex != SelectedIndex)
            {
                var chartSelectionBehavior = Chart?.ActualSelectionBehavior;
                if (chartSelectionBehavior != null)
                {
                    if (SelectedIndex == -1)
                    {
                        chartSelectionBehavior.UpdateSelection(this, PreviousSelectedIndex);
                    }
                    else
                    {
                        chartSelectionBehavior.UpdateSelection(this, SelectedIndex);
                    }

                }
            }
            //Todo: Why need to call invalidate even PreviousSelectedIndex != SelectedIndex
            InvalidateSeries();
        }

        private void DataPointSelection()
        {
            if (SelectedIndex >= 0 && Segments.Count > SelectedIndex)
            {
                var chartSelection = Chart?.ActualSelectionBehavior;

                if (chartSelection != null && chartSelection.Type == SelectionType.Point && IsIndividualSegment())
                {
                    chartSelection.UpdateSegmentColor(this, SelectedIndex);
                    InvalidateSeries();

                    if (ShowDataLabels)
                        InvalidateDataLabel();
                    //Todo:Need to check circular chart with legend items color.
                    //if ((chart as SfCircularChart) != null && chart.Legend != null && chart.Legend.IsVisible && chart.LegendItems != null && chart.LegendItems.Count > SelectedIndex)
                    //{
                    //    chart.LegendItems[SelectedIndex].IconBrush = SelectionBrush;
                    //}
                }

            }
        }

#endregion

#endregion
    }
}
