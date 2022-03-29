using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.UI.Input;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Imaging;
using Microsoft.UI.Xaml.Shapes;
using Windows.Foundation;
using Windows.UI.Input;
using ChartAdornmentContainer = Syncfusion.UI.Xaml.Charts.ChartDataMarkerContainer;
using ChartAdornmentPresenter = Syncfusion.UI.Xaml.Charts.ChartDataMarkerPresenter;
using StackingBarSeries = Syncfusion.UI.Xaml.Charts.StackedBarSeries;
using StackingColumnSeries = Syncfusion.UI.Xaml.Charts.StackedColumnSeries;

namespace Syncfusion.UI.Xaml.Charts
{
    /// <summary>
    /// ChartSelectionBehavior enables the selection of segments in a chart.
    /// </summary>
    /// <remarks>
    /// The selected segment can be displayed with a different color specified using SelectionBrush property available in corresponding series.
    /// ChartSelectionBehavior is applicable only to certain series such as <see cref="ColumnSeries"/>,<see cref="BarSeries"/>,
    /// <see cref="RangeColumnSeries"/>,<see cref="StackingBarSeries"/>,<see cref="StackingColumnSeries"/>,<see cref="ScatterSeries"/>,
    /// <see cref="BubbleSeries"/>,<see cref="PieSeries"/>.
    /// </remarks>
    /// <example>
    /// This example, we are using <see cref="ChartSelectionBehavior"/>.
    /// <code language="XAML">
    ///     &lt;syncfusion:SfChart&gt;
    ///         &lt;syncfusion:SfChart.Behaviors&gt;
    ///             &lt;syncfusion:ChartSelectionBehavior/&gt;
    ///         &lt;/syncfusion:SfChart.Behaviors&gt;
    ///     &lt;/syncfusion:SfChart&gt;
    /// </code>
    /// <code language="C#">
    ///     ChartSelectionBehavior selectionBehavior = new ChartSelectionBehavior();
    ///     chartArea.Behaviors.Add(selectionBehavior);
    /// </code>
    /// </example>
    public class ChartSelectionBehavior : ChartBehavior
    {
        #region Dependency Property Registration

        /// <summary>
        /// The DependencyProperty for <see cref="Type"/> property.
        /// </summary>
        public static readonly DependencyProperty TypeProperty =
            DependencyProperty.Register(
                nameof(Type),
                typeof(SelectionType),
                typeof(ChartSelectionBehavior),
                new PropertyMetadata(SelectionType.Point, OnSelectionTypeChanged));


        /// <summary>
        /// The DependencyProperty for <see cref=" ActivationMode"/> property.
        /// </summary>
        internal static readonly DependencyProperty ActivationModeProperty =
            DependencyProperty.Register(
                nameof(ActivationMode),
                typeof(SelectionActivationMode),
                typeof(ChartSelectionBehavior),
                new PropertyMetadata(SelectionActivationMode.Click));


        /// <summary>
        /// The DependencyProperty for <see cref="Cursor"/> property.
        /// </summary>
        public static readonly DependencyProperty CursorProperty =
            DependencyProperty.Register(
                nameof(Cursor),
                typeof(InputSystemCursorShape),
                typeof(ChartSelectionBehavior),
                new PropertyMetadata(InputSystemCursorShape.Arrow));

        /// <summary>
        /// The DependencyProperty for <see cref="SeriesSelectionBrush"/> property.
        /// </summary>
        public static readonly DependencyProperty SeriesSelectionBrushProperty =
            DependencyProperty.Register(nameof(SeriesSelectionBrush), typeof(Brush), typeof(ChartSelectionBehavior),
                new PropertyMetadata(null, OnSeriesSelectionBrushChanged));

        #endregion

        #region Fields

        private ChartSegment mouseUnderSegment;
        private List<ChartSeries> seriesCollection;
        private ChartAdornmentPresenter selectedAdornmentPresenter;
        private int index;

        #endregion

        #region Properties

        #region Public Properties

        internal SelectionActivationMode ActivationMode
        {
            get { return (SelectionActivationMode)GetValue(ActivationModeProperty); }
            set { SetValue(ActivationModeProperty, value); }
        }

        /// <summary>
        /// Gets or sets the SelectionType value that indicated the selection type in SfChart.
        /// </summary>
        /// <value>
        /// <see cref="Syncfusion.UI.Xaml.Charts.SelectionType"/>
        /// </value>
        public SelectionType Type
        {
            get { return (SelectionType)GetValue(TypeProperty); }
            set { SetValue(TypeProperty, value); }
        }

        /// <summary>
        /// Gets or sets the pointer cursor for the series, which indicates that the series selection cursor type.
        /// </summary>
        /// <value>
        /// <see cref="InputSystemCursorShape"/>
        /// </value>
        /// <value>
        /// Default value is Arrow.
        /// </value>
        public InputSystemCursorShape Cursor
        {
            get { return (InputSystemCursorShape)GetValue(CursorProperty); }
            set { SetValue(CursorProperty, value); }
        }

        /// <summary>
        /// Gets or sets the brush to select the series.
        /// </summary>
        /// <value>
        /// The <see cref="Brush"/> value.
        /// </value>
        public Brush SeriesSelectionBrush
        {
            get { return (Brush)GetValue(SeriesSelectionBrushProperty); }
            set { SetValue(SeriesSelectionBrushProperty, value); }
        }

        #endregion

        #region Internal Properties

        internal bool EnableMultiSelection
        {
            get
            {
                return (Type == SelectionType.MultiSeries || Type == SelectionType.MultiPoint);
            }
        }

        #endregion

        #endregion

        #region Methods

        #region Public Methods

        /// <summary>
        /// Method used to get selection brush for series selection.
        /// </summary>
        /// <param name="series">ChartSeriesBase</param>
        /// <returns>Returns brush for selected segment.</returns>
        internal virtual Brush GetSeriesSelectionBrush(ChartSeriesBase series)
        {
            if (SeriesSelectionBrush != null)
                return SeriesSelectionBrush;
            else
                return null;
        }

        #endregion

        #region Protected Internal Override Methods

        /// <summary>
        /// Called when pointer released in the chart.
        /// </summary>
        /// <param name="e">PointerRoutedEventArgs</param>
        protected internal override void OnPointerReleased(PointerRoutedEventArgs e)
        {
            if(ChartArea == null)
            {
                return;
            }

            if (ActivationMode == SelectionActivationMode.Click)
            {
                FrameworkElement element = e.OriginalSource as FrameworkElement;
                ChartSegment segment = null;
                ChartArea.CurrentSelectedSeries = null;

                if (element != null)
                {
                    if (element.Tag != null) segment = element.Tag as ChartSegment;
                }

                if (segment is TrendlineSegment)
                    return;

                var image = element as Image;

                if (image != null && image.Source is WriteableBitmap)
                {
                    // Bitmap segment selection process handling.
                    OnBitmapSeriesMouseDownSelection(element, e);
                }
                else if (segment != null && segment == mouseUnderSegment && segment.Series is ISegmentSelectable
                         && !(segment.Item is Trendline))
                {
                    if (!segment.Series.IsSideBySide && segment.Series is CartesianSeries
                        && !(segment.Series is ScatterSeries) && !(segment.Series is BubbleSeries))
                    {
                        Point canvasPoint = e.GetCurrentPoint(segment.Series.ActualArea.GetAdorningCanvas()).Position;
                        ChartDataPointInfo data = (segment.Series as ChartSeries).GetDataPoint(canvasPoint);

                        OnMouseDownSelection(segment.Series, data);
                    }
                    else
                    {
                        int index = -1;
                        if ((segment.Series.ActualXAxis is CategoryAxis) && !(segment.Series.ActualXAxis as CategoryAxis).IsIndexed
                            && segment.Series.IsSideBySide && !(segment.Series is FinancialSeriesBase) && (!(segment.Series is RangeSeriesBase))
                            && !(segment.Series is WaterfallSeries))
                            index = segment.Series.GroupedActualData.IndexOf(segment.Item);
                        else
                            index = segment.Series is CircularSeries && !double.IsNaN(((CircularSeries)segment.Series).GroupTo)? segment.Series.Segments.IndexOf(segment): segment.Series.ActualData.IndexOf(segment.Item);
                        OnMouseDownSelection(segment.Series, index);
                    }
                }
                else
                {
                    // Get the selected adornment index and select the adornment marker
                    index = ChartExtensionUtils.GetAdornmentIndex(element);
                    FrameworkElement frameworkElement = e.OriginalSource as FrameworkElement;
                    var chartAdornmentPresenter = frameworkElement as ChartAdornmentPresenter;

                    while (frameworkElement != null && chartAdornmentPresenter == null)
                    {
                        frameworkElement = VisualTreeHelper.GetParent(frameworkElement) as FrameworkElement;
                        chartAdornmentPresenter = frameworkElement as ChartAdornmentPresenter;
                    }

                    if (chartAdornmentPresenter != null &&
                        chartAdornmentPresenter.Series is ISegmentSelectable)
                        OnMouseDownSelection(chartAdornmentPresenter.Series, index);
                }

                if (selectedAdornmentPresenter != null)
                {
                    selectedAdornmentPresenter = null;
                }
            }

            AdorningCanvas.ReleasePointerCapture(e.Pointer);
        }

        /// <summary>
        /// Called when pointer pressed in the chart.
        /// </summary>
        /// <param name="e">PointerRoutedEventArgs</param>
        protected internal override void OnPointerPressed(PointerRoutedEventArgs e)
        {
            if(ChartArea == null)
            {
                return;
            }

            FrameworkElement element = e.OriginalSource as FrameworkElement;
            ChartSegment segment = null;
            ChartArea.CurrentSelectedSeries = null;

            if (element != null)
            {
                if (element.Tag != null) segment = element.Tag as ChartSegment;
            }

            if (segment != null && segment.Series is ISegmentSelectable)
            {
                mouseUnderSegment = segment;
            }
        }

        /// <summary>
        /// Called when pointer moved in the chart.
        /// </summary>
        /// <param name="e">PointerRoutedEventArgs</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        protected internal override void OnPointerMoved(PointerRoutedEventArgs e)
        {
            if (ChartArea == null)
            {
                return;
            }

            if (e.OriginalSource != null && Type != SelectionType.None)
            {
                FrameworkElement element = e.OriginalSource as FrameworkElement;
                ChartSegment segment = null;

                if (element != null)
                {
                    if (element.Tag != null) segment = element.Tag as ChartSegment;
                }

                if (segment is TrendlineSegment)
                    return;

                if (element != null && element.DataContext is LegendItem)
                    return;

                var image = element as Image;

                if (segment != null && segment.Series is ISegmentSelectable && !(segment.Item is Trendline))
                {
                    // Scatter series supports selection and dragging at the same time.
                    if (!(segment.Series is ScatterSeries) && IsDraggableSeries(segment.Series))
                        return;
                    if (!segment.Series.IsLinear || Type == SelectionType.Point)
                        ChangeSelectionCursor(true);
                    else
                        ChangeSelectionCursor(false);

                    if (ActivationMode == SelectionActivationMode.Move)
                    {
                        if (!segment.Series.IsSideBySide && segment.Series is CartesianSeries
                            && !(segment.Series is ScatterSeries) && !(segment.Series is BubbleSeries))
                        {
                            Point canvasPoint = e.GetCurrentPoint(segment.Series.ActualArea.GetAdorningCanvas()).Position;
                            ChartDataPointInfo data = (segment.Series as ChartSeries).GetDataPoint(canvasPoint);
                            OnMouseMoveSelection(segment.Series, data);
                        }
                        else
                        {
                            int segIndex = segment.Series is  CircularSeries && !double.IsNaN(((CircularSeries)segment.Series).GroupTo) ?segment.Series.Segments.IndexOf(segment): segment.Series.ActualData.IndexOf(segment.Item);
                            OnMouseMoveSelection(segment.Series, segIndex);
                        }
                    }
                }
                else if (e.OriginalSource is Shape && (e.OriginalSource as Shape).DataContext is ChartAdornmentContainer
                    && ((e.OriginalSource as Shape).DataContext as ChartAdornmentContainer).Tag is int)
                {
                    // Check the selected element is adornment shape
                    selectedAdornmentPresenter = VisualTreeHelper.GetParent((e.OriginalSource as Shape).DataContext
                            as ChartAdornmentContainer) as ChartAdornmentPresenter;
                    if (selectedAdornmentPresenter != null && IsDraggableSeries(selectedAdornmentPresenter.Series))
                        return;
                    ChangeSelectionCursor(true);
                    if (ActivationMode == SelectionActivationMode.Move)
                    {
                        index = (int)((e.OriginalSource as Shape).DataContext as ChartAdornmentContainer).Tag;

                        if (selectedAdornmentPresenter != null && selectedAdornmentPresenter.Series is ISegmentSelectable)
                            OnMouseMoveSelection(selectedAdornmentPresenter.Series, index);
                    }
                }
                else if (element is Border || element is TextBlock || element is Shape) // check the selected element is adornment label 
                {
                    ChangeSelectionCursor(false);

                    FrameworkElement frameworkElement = e.OriginalSource as FrameworkElement;
                    int count = e.OriginalSource is TextBlock ? 3 : 2;
                    for (int i = 0; i < count; i++)
                    {
                        if (frameworkElement != null)
                            frameworkElement = VisualTreeHelper.GetParent(frameworkElement) as FrameworkElement;
                        else
                            break;
                    }

                    if (frameworkElement is ContentPresenter)
                    {
                        index = ChartExtensionUtils.GetAdornmentIndex(frameworkElement);
                        if (index != -1)
                            ChangeSelectionCursor(true);
                        if (ActivationMode == SelectionActivationMode.Move)
                        {
                            frameworkElement = VisualTreeHelper.GetParent(frameworkElement) as FrameworkElement;

                            if (frameworkElement is ChartAdornmentPresenter || frameworkElement is ChartAdornmentContainer)
                            {
                                while (!(frameworkElement is ChartAdornmentPresenter) && frameworkElement != null)
                                {
                                    frameworkElement = VisualTreeHelper.GetParent(frameworkElement) as FrameworkElement;
                                }

                                selectedAdornmentPresenter = frameworkElement as ChartAdornmentPresenter;
                                if(selectedAdornmentPresenter != null)
                                {
                                    if (IsDraggableSeries(selectedAdornmentPresenter.Series))
                                        return;
                                    if (selectedAdornmentPresenter.Series is ISegmentSelectable)
                                        OnMouseMoveSelection(selectedAdornmentPresenter.Series, index);
                                }
                            }
                        }
                    }

                    var contentControl = frameworkElement as ContentControl;
                    if (contentControl != null && contentControl.Tag is int)
                    {
                        ChangeSelectionCursor(true);
                        if (ActivationMode == SelectionActivationMode.Move)
                        {
                            index = (int)contentControl.Tag;
                            frameworkElement = VisualTreeHelper.GetParent(frameworkElement) as FrameworkElement;

                            var chartAdornmentPresenter = frameworkElement as ChartAdornmentPresenter;
                            if (chartAdornmentPresenter != null)
                            {
                                selectedAdornmentPresenter = chartAdornmentPresenter;
                                if (IsDraggableSeries(selectedAdornmentPresenter.Series))
                                    return;
                                if (selectedAdornmentPresenter != null && selectedAdornmentPresenter.Series is ISegmentSelectable)
                                    OnMouseMoveSelection(selectedAdornmentPresenter.Series, index);
                            }
                        }
                    }
                }
                else if (image != null && image.Source is WriteableBitmap)
                {
                    GetBitmapSeriesCollection(element, e);

                    // Bitmap segment selection process handling.
                    if (ActivationMode == SelectionActivationMode.Move)
                        OnBitmapSeriesMouseMoveSelection(element, e);
                }
                else if (ChartArea.PreviousSelectedSeries != null && ChartArea.CurrentSelectedSeries != null && ActivationMode == SelectionActivationMode.Move
                         && ChartArea.VisibleSeries.Contains(ChartArea.PreviousSelectedSeries))
                {
                    ChangeSelectionCursor(false);
                    bool isCancel;
                    if (Type == SelectionType.Series)
                        isCancel = ChartArea.CurrentSelectedSeries.RaiseSelectionChanging(-1, ChartArea.SelectedSeriesIndex);
                    else
                        isCancel = ChartArea.CurrentSelectedSeries.RaiseSelectionChanging(
                            -1,
                            (ChartArea.CurrentSelectedSeries as ISegmentSelectable).SelectedIndex);

                    if (!isCancel)
                    {
                        Deselect();
                    }
                }
                else
                    ChangeSelectionCursor(false);
            }
            else
                ChangeSelectionCursor(false);
        }
        
        #endregion

        #region Protected Internal Virtual Methods

        /// <summary>
        /// Invoked whenever the SelectionChanging event have raised.
        /// </summary>
        /// <param name="eventArgs">type of <see cref="ChartSelectionChangingEventArgs"/></param>
        protected internal virtual void OnSelectionChanging(ChartSelectionChangingEventArgs eventArgs)
        {
        }

        /// <summary>
        /// Invoked whenever the SelectionChanged event have raised.
        /// </summary>
        /// <param name="eventArgs">type of <see cref="ChartSelectionChangedEventArgs"/></param>
        protected internal virtual void OnSelectionChanged(ChartSelectionChangedEventArgs eventArgs)
        {
        }

        #endregion

        #region Private Static Methods
        private static void OnSeriesSelectionBrushChanged(DependencyObject d, DependencyPropertyChangedEventArgs args)
        {
        }
            private static void OnEnableSeriesSelectionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ChartBase chartBase = (d as ChartSelectionBehavior).ChartArea;

            if (chartBase != null && !(bool)e.NewValue)
            {
                var seriesCollection = chartBase.GetSeriesCollection();
                foreach (ChartSeries series in seriesCollection)
                {
                    if (chartBase.SelectedSeriesCollection.Contains(series))
                    {
                        chartBase.SelectedSeriesCollection.Remove(series);
                        chartBase.OnResetSeries(series);
                    }
                }

                chartBase.SelectedSeriesIndex = -1;
                chartBase.SelectedSeriesCollection.Clear();
            }
            else if (chartBase != null && (bool)e.NewValue && chartBase.SelectedSeriesIndex != -1)
                chartBase.SeriesSelectedIndexChanged(chartBase.SelectedSeriesIndex, -1);
        }

        private static void OnEnableSegmentSelectionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ChartBase chartBase = (d as ChartSelectionBehavior).ChartArea;

            if (chartBase != null && !(bool)e.NewValue)
            {
                foreach (var series in chartBase.VisibleSeries)
                {
                    for (int i = 0; i < series.ActualData.Count; i++)
                    {
                        if (series.SelectedSegmentsIndexes.Contains(i))
                        {
                            series.SelectedSegmentsIndexes.Remove(i);
                            series.OnResetSegment(i);
                        }
                    }

                    var selectableSegment = series as ISegmentSelectable;
                    if (selectableSegment != null)
                        selectableSegment.SelectedIndex = -1;
                    series.SelectedSegmentsIndexes.Clear();
                }
            }
            else if (chartBase != null && (bool)e.NewValue)
            {
                for (int index = 0; index < chartBase.VisibleSeries.Count; index++)
                {
                    ChartSeriesBase series = chartBase.VisibleSeries[index];
                    var selectableSegment = series as ISegmentSelectable;

                    if (selectableSegment != null && selectableSegment.SelectedIndex != -1)
                        series.SelectedIndexChanged(selectableSegment.SelectedIndex, -1);
                }
            }
        }

        private static void OnSelectionTypeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ChartSelectionBehavior behavior = d as ChartSelectionBehavior;

            ChartBase chartBase = behavior.ChartArea;
            // WPF-26121, When selection style is changed the last selected segment remains selected
            if (chartBase == null)
                return;
            var seriesCollection = chartBase.GetSeriesCollection();
            if (seriesCollection == null)
                return;
            chartBase.SelectedSeriesIndex = -1;

            foreach (ChartSeries series in seriesCollection)
            {
                var segmentSelectableSeries = series as ISegmentSelectable;
                if (segmentSelectableSeries != null)
                    segmentSelectableSeries.SelectedIndex = -1;

                if (chartBase.SelectedSeriesCollection.Contains(series) &&
                    chartBase.SelectedSeriesIndex != seriesCollection.IndexOf(series))
                {
                    chartBase.SelectedSeriesCollection.Remove(series);
                    chartBase.OnResetSeries(series);
                }

                // Need to revisit the code.
                for (int i = 0; i < series.ActualData.Count; i++)
                {
                    if (series.SelectedSegmentsIndexes.Contains(i))
                    {
                        series.SelectedSegmentsIndexes.Remove(i);
                        series.OnResetSegment(i);
                    }
                }
            }
        }

        #endregion

        #region Private Methods
        
        /// <summary>
        /// Called for deselecting the selected segment or series in MouseMove selection.
        /// </summary>
        private void Deselect()
        {
            (ChartArea.PreviousSelectedSeries as ISegmentSelectable).SelectedIndex = -1;
            ChartArea.SelectedSeriesIndex = -1;
            ChartArea.PreviousSelectedSeries = null;
            ChartArea.CurrentSelectedSeries = null;
            seriesCollection = null;
        }

        /// <summary>
        /// Method used to change the cursor for series and segments and adornments.
        /// </summary>
        private void ChangeSelectionCursor(bool isCursorChanged)
        {
            if (isCursorChanged)
            {
                if (Windows.System.Profile.AnalyticsInfo.VersionInfo.DeviceFamily != "Windows.Mobile")
                {
#if WinUI_Desktop
                    ChartArea.ChangeCursorType(Cursor, isCursorChanged);
#else
                    if (Window.Current.CoreWindow.PointerCursor.Type == CoreCursorType.Arrow)
                        Window.Current.CoreWindow.PointerCursor = new CoreCursor(Cursor, 1);
#endif
                }
            }
            else
            {

                if (Windows.System.Profile.AnalyticsInfo.VersionInfo.DeviceFamily != "Windows.Mobile")
                {
#if WinUI_Desktop
                    ChartArea.ChangeCursorType(Cursor, isCursorChanged);
#else
                    if (Window.Current.CoreWindow.PointerCursor.Type != CoreCursorType.Arrow)
                        Window.Current.CoreWindow.PointerCursor = new CoreCursor(CoreCursorType.Arrow, 2);
#endif
                }

            }
        }

        /// <summary>
        /// Method used to get the bool value for series or segment has dragging base.
        /// </summary>
        private bool IsDraggableSeries(ChartSeriesBase chartSeries)
        {
            if (ChartExtensionUtils.IsDraggable(chartSeries))
            {
                ChangeSelectionCursor(false);
                return true;
            }
            else
                return false;
        }
        
        /// <summary>
        /// Method used to set SelectedIndex while mouse move in segment/adornment.
        /// </summary>
        /// <param name="series"></param>
        /// <param name="value"></param>
        private void OnMouseMoveSelection(ChartSeriesBase series, object value)
        {
            if (EnableMultiSelection)
                return;

            ChartArea.CurrentSelectedSeries = series;

            bool seriesSelection = (Type == SelectionType.Series)
                && (series.IsSideBySide || series is ScatterSeries
                || series is BubbleSeries
                || series is AccumulationSeriesBase
                || series is FastScatterBitmapSeries
                || !series.IsSideBySide);

            var chartDataPointInfo = value as ChartDataPointInfo;
            if (seriesSelection)
            {
                if (ChartArea.PreviousSelectedSeries != null && ChartArea.PreviousSelectedSeries != ChartArea.CurrentSelectedSeries)
                    (ChartArea.PreviousSelectedSeries as ISegmentSelectable).SelectedIndex = -1;
                
                int seriesIndex = ChartArea.VisibleSeries.IndexOf(series);

                // Call OnSelectionChanging method to raise SelectionChanging event
                ChartArea.CurrentSelectedSeries.selectionChangingEventArgs.IsDataPointSelection = false;
                bool isCancel = ChartArea.CurrentSelectedSeries.RaiseSelectionChanging(seriesIndex, ChartArea.SelectedSeriesIndex);

                if (!isCancel)
                {
                    ChartArea.SelectedSeriesIndex = seriesIndex;
                    ChartArea.PreviousSelectedSeries = ChartArea.CurrentSelectedSeries;
                }
            }
            else if (ChartArea.CurrentSelectedSeries is ISegmentSelectable && Type == SelectionType.Point
                && (value != null && value.GetType() == typeof(int) || chartDataPointInfo != null))
            {
                ChartArea.SelectedSeriesIndex = -1;

                if (ChartArea.PreviousSelectedSeries != null && ChartArea.PreviousSelectedSeries != ChartArea.CurrentSelectedSeries)
                    (ChartArea.PreviousSelectedSeries as ISegmentSelectable).SelectedIndex = -1;

                int pointIndex = value.GetType() == typeof(int) ? (int)value : chartDataPointInfo.Index;

                // Call OnSelectionChanging method to raise SelectionChanging event
                ChartArea.CurrentSelectedSeries.selectionChangingEventArgs.IsDataPointSelection = true;
                bool isCancel = ChartArea.CurrentSelectedSeries.RaiseSelectionChanging(
                    pointIndex,
                    (ChartArea.CurrentSelectedSeries as ISegmentSelectable).SelectedIndex);

                if (!isCancel)
                {
                    (ChartArea.CurrentSelectedSeries as ISegmentSelectable).SelectedIndex = pointIndex;
                    ChartArea.PreviousSelectedSeries = ChartArea.CurrentSelectedSeries;
                }
            }
            else if (ChartArea.PreviousSelectedSeries != null
                     && ChartArea.VisibleSeries.Contains(ChartArea.PreviousSelectedSeries))
            {
                bool isCancel;
                if (seriesSelection)
                    isCancel = ChartArea.CurrentSelectedSeries.RaiseSelectionChanging(
                        -1, 
                        ChartArea.SelectedSeriesIndex);
                else
                    isCancel = ChartArea.CurrentSelectedSeries.RaiseSelectionChanging(
                        -1,
                        (ChartArea.CurrentSelectedSeries as ISegmentSelectable).SelectedIndex);

                if (!isCancel)
                {
                    Deselect();
                }
            }
        }

        /// <summary>
        /// Method used to set SelectedIndex while mouse down in segment/adornment.
        /// </summary>
        /// <param name="series"></param>
        /// <param name="value"></param>
        private void OnMouseDownSelection(ChartSeriesBase series, object value)
        {
            var isScatterSeries = series is ScatterSeries;

            // Scatter series supports selection and dragging at the same time.
            if (!isScatterSeries && IsDraggableSeries(series))
                return;

            ChartArea.CurrentSelectedSeries = series;

            bool seriesSelection = (Type == SelectionType.Series || Type == SelectionType.MultiSeries)
                && (series.IsSideBySide
                || isScatterSeries
                || series is BubbleSeries
                || series is AccumulationSeriesBase
                || series is FastScatterBitmapSeries
                || !series.IsSideBySide);

            var chartDataPointInfo = value as ChartDataPointInfo;
            if (seriesSelection)
            {
                int index = ChartArea.VisibleSeries.IndexOf(series);
                ChartArea.CurrentSelectedSeries.selectionChangingEventArgs.IsDataPointSelection = false;

                // Call OnSelectionChanging method to raise SelectionChanging event 
                bool isCancel = ChartArea.CurrentSelectedSeries.RaiseSelectionChanging(index, ChartArea.SelectedSeriesIndex);
                if (!isCancel)
                {
                    if (EnableMultiSelection && ChartArea.SelectedSeriesCollection.Contains(ChartArea.CurrentSelectedSeries))
                    {
                        ChartArea.SelectedSeriesCollection.Remove(ChartArea.CurrentSelectedSeries);
                        ChartArea.SelectedSeriesIndex = -1;
                        ChartArea.OnResetSeries(ChartArea.CurrentSelectedSeries as ChartSeries);
                    }
                    else if (ChartArea.SelectedSeriesIndex == index)
                        ChartArea.SelectedSeriesIndex = -1;
                    else
                    {
                        ChartArea.SelectedSeriesIndex = index;
                        ChartArea.PreviousSelectedSeries = ChartArea.CurrentSelectedSeries;
                    }
                }
            }
            else if (ChartArea.CurrentSelectedSeries is ISegmentSelectable && (Type == SelectionType.Point || Type == SelectionType.MultiPoint)
                && ((value != null && value.GetType() == typeof(int)) || chartDataPointInfo != null))
            {
                int pointIndex = value.GetType() == typeof(int) ? (int)value : chartDataPointInfo.Index;
                ChartArea.CurrentSelectedSeries.selectionChangingEventArgs.IsDataPointSelection = true;

                // Call OnSelectionChanging method to raise SelectionChanging event  
                bool isCancel = ChartArea.CurrentSelectedSeries.RaiseSelectionChanging(
                    pointIndex,
                    (ChartArea.CurrentSelectedSeries as ISegmentSelectable).SelectedIndex);
                if (!isCancel)
                {
                    if (EnableMultiSelection)
                    {
                        if (ChartArea.CurrentSelectedSeries.SelectedSegmentsIndexes.Contains(pointIndex))
                            ChartArea.CurrentSelectedSeries.SelectedSegmentsIndexes.Remove(pointIndex);
                        else
                        {
                            ChartArea.CurrentSelectedSeries.SelectedSegmentsIndexes.Add(pointIndex);
                            ChartArea.PreviousSelectedSeries = ChartArea.CurrentSelectedSeries;
                        }
                    }
                    else
                    {
                        ISegmentSelectable currentSelectedSeries = (ChartArea.CurrentSelectedSeries as ISegmentSelectable);
                        if (currentSelectedSeries.SelectedIndex == pointIndex)
                            currentSelectedSeries.SelectedIndex = -1;
                        else
                        {
                            currentSelectedSeries.SelectedIndex = pointIndex;
                            ChartArea.PreviousSelectedSeries = ChartArea.CurrentSelectedSeries;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Method used to select bitmap series in mouse move.
        /// </summary>
        /// <param name="element"></param>
        /// <param name="e"></param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1801: Review unused parameters")]
        private void OnBitmapSeriesMouseMoveSelection(FrameworkElement element, PointerRoutedEventArgs e)
        {
            Point canvasPoint = e.GetCurrentPoint(ChartArea.GetAdorningCanvas()).Position;

            if (seriesCollection.Count > 0)
            {
                ChartArea.CurrentSelectedSeries = seriesCollection[seriesCollection.Count - 1];
                if (IsDraggableSeries(ChartArea.CurrentSelectedSeries))
                    return;

                if (ChartArea.CurrentSelectedSeries is ISegmentSelectable)
                {
                    ChartDataPointInfo data = ChartArea.CurrentSelectedSeries.GetDataPoint(canvasPoint);
                    OnMouseMoveSelection(ChartArea.CurrentSelectedSeries, data);
                }
            }
            else if (ChartArea.PreviousSelectedSeries != null && ChartArea.VisibleSeries.Contains(ChartArea.PreviousSelectedSeries))
            {
                bool isCancel;
                if (Type == SelectionType.Series)
                    isCancel = ChartArea.CurrentSelectedSeries.RaiseSelectionChanging(-1, ChartArea.SelectedSeriesIndex);
                else
                    isCancel = ChartArea.CurrentSelectedSeries.RaiseSelectionChanging(
                        -1,
                        (ChartArea.CurrentSelectedSeries as ISegmentSelectable).SelectedIndex);

                if (!isCancel)
                {
                    Deselect();
                }
            }
        }

        /// <summary>
        /// Method used to get the fast series in the mouse point.
        /// </summary>
        /// <param name="element"></param>
        /// <param name="e"></param>
#if NETFX_CORE
        private void GetBitmapSeriesCollection(FrameworkElement element, PointerRoutedEventArgs e)
#else
        private void GetBitmapSeriesCollection(FrameworkElement element, MouseEventArgs e)
#endif
        {
            Image bitmapImage = element as Image;
            var mousePoint = e.GetCurrentPoint(bitmapImage);
            int position = ((bitmapImage.Source as WriteableBitmap).PixelWidth *
                        (int)mousePoint.Position.Y + (int)mousePoint.Position.X) * 4;

            if (!ChartArea.isBitmapPixelsConverted)
                ChartArea.ConvertBitmapPixels();
            
            seriesCollection = (from series in ChartArea.GetChartSeriesCollection()
                                where (series.Pixels.Count > 0 && series.Pixels.Contains(position))
                                select series).ToList();

            if (seriesCollection.Count > 0)
            {
                foreach (ChartSeries series in seriesCollection)
                {
                    if (!series.IsLinear || Type == SelectionType.Series)
                        ChangeSelectionCursor(true);
                }
            }
            else
            {
                ChangeSelectionCursor(false);
            }
        }

        /// <summary>
        /// Method used to select bitmap series in mouse down.
        /// </summary>
        /// <param name="element"></param>
        /// <param name="e"></param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1801: Review unused parameters")]
        private void OnBitmapSeriesMouseDownSelection(FrameworkElement element, PointerRoutedEventArgs e)
        {
            Point canvasPoint = e.GetCurrentPoint(ChartArea.GetAdorningCanvas()).Position;

            if (seriesCollection.Count > 0)
            {
                ChartArea.CurrentSelectedSeries = seriesCollection[seriesCollection.Count - 1];

                if (ChartArea.CurrentSelectedSeries is ISegmentSelectable)
                {
                    ChartDataPointInfo data = ChartArea.CurrentSelectedSeries.GetDataPoint(canvasPoint);
                    OnMouseDownSelection(ChartArea.CurrentSelectedSeries, data);
                }
            }
        }

        #endregion

        #endregion
    }

    /// <summary>
    /// Represents chart segment selection changed event arguments.
    /// </summary>
    /// <remarks>
    /// It contains information like selected segment and series.
    /// </remarks>
    public class ChartSelectionChangedEventArgs : EventArgs
    {
        #region Fields

        private bool isDataPointSelection = true;

        #endregion

        #region Properties

        #region Public Properties

        /// <summary>
        /// Gets the series which has been selected through mouse interaction or selected index.
        /// </summary>
        public ChartSeriesBase SelectedSeries
        {
            get;
            internal set;
        }

        /// <summary>
        /// Gets the series which had been selected through mouse interaction or selected index.
        /// </summary>
        public ChartSeriesBase PreviousSelectedSeries
        {
            get;
            internal set;
        }

        /// <summary>
        /// Gets the series collection which has been selected through rectangle selection or mouse interaction.
        /// </summary>
        public List<ChartSeriesBase> SelectedSeriesCollection
        {
            get;
            internal set;
        }

        /// <summary>
        /// Gets the segment which has been selected through mouse interaction or selected index.
        /// </summary>
        public ChartSegment SelectedSegment
        {
            get;
            internal set;
        }

        /// <summary>
        /// Gets the segments  collection which has been selected through rectangle selection or mouse interaction.
        /// </summary>
        public List<ChartSegment> SelectedSegments
        {
            get;
            internal set;
        }

        /// <summary>
        /// Gets the segments  collection which has been selected through rectangle selection or mouse interaction previously.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "Reviewed")]
        public List<ChartSegment> PreviousSelectedSegments
        {
            get;
            internal set;
        }

        /// <summary>
        /// Gets the segment which had been selected through mouse interaction or selected index.
        /// </summary>
        public ChartSegment PreviousSelectedSegment
        {
            get;
            internal set;
        }

        /// <summary>
        /// Gets the current index of the segment which has been selected through mouse interaction or selected index.
        /// </summary>
        public int SelectedIndex { get; internal set; }

        /// <summary>
        /// Gets the previous index of the segment which had been selected through mouse interaction or SelectedIndex.
        /// </summary>
        public int PreviousSelectedIndex { get; internal set; }

        /// <summary>
        /// Gets a value indicating whether the segment or series is selected.
        /// </summary>
        public bool IsSelected
        {
            get;
            internal set;
        }

        /// <summary>
        /// Gets a value indicating whether the selection is segment selection or series selection.
        /// </summary>
        public bool IsDataPointSelection
        {
            get
            {
                return isDataPointSelection;
            }

            internal set
            {
                isDataPointSelection = value;
            }
        }

        /// <summary>
        /// Gets the selected segment item value.
        /// </summary>
        public object NewPointInfo
        {
            get;
            internal set;
        }

        /// <summary>
        /// Gets the previous selected segment item value.
        /// </summary>
        public object OldPointInfo
        {
            get;
            internal set;
        }

        #endregion
        
        #endregion
    }

    /// <summary>
    /// Represents chart segment selection changing event arguments.
    /// </summary>
    /// <remarks>
    /// It contains information like selected segment and series.
    /// </remarks>
    public class ChartSelectionChangingEventArgs : EventArgs
    {
        #region Fields

        private bool isDataPointSelection = true;

        #endregion

        #region Properties

        #region Public Properties

        /// <summary>
        /// Gets the series which has been selected through mouse interaction or selected index.
        /// </summary>
        public ChartSeriesBase SelectedSeries
        {
            get;
            internal set;
        }

        /// <summary>
        /// Gets the segment which has been selected through mouse interaction or selected index.
        /// </summary>
        public ChartSegment SelectedSegment
        {
            get;
            internal set;
        }

        /// <summary>
        /// Gets or sets the segments collection which has been selected through rectangle selection or mouse interaction.
        /// </summary>
        public List<ChartSegment> SelectedSegments
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the current index of the segment which has been selected through mouse interaction or selected index.
        /// </summary>
        public int SelectedIndex { get; internal set; }

        /// <summary>
        /// Gets the previous index of the segment which had been selected through mouse interaction or selected index.
        /// </summary>
        public int PreviousSelectedIndex { get; internal set; }

        /// <summary>
        /// Gets or sets a value indicating whether to avoid selection.
        /// </summary>
        public bool Cancel { get; set; }

        /// <summary>
        /// Gets a value indicating whether the selection is segment selection or series selection.
        /// </summary>
        public bool IsDataPointSelection
        {
            get
            {
                return isDataPointSelection;
            }

            internal set
            {
                isDataPointSelection = value;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the segment or series is selected.
        /// </summary>
        public bool IsSelected
        {
            get;
            internal set;
        }

        #endregion
        
        #endregion
    }
}
