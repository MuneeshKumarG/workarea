using System;
using System.Collections.Generic;
using System.ComponentModel;
using Microsoft.UI.Input;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Imaging;
using Windows.Foundation;

namespace Syncfusion.UI.Xaml.Charts
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class ChartSelectionBehavior : ChartBehavior
    {
        #region Dependency Property Registration

        /// <summary>
        /// The DependencyProperty for <see cref="Type"/> property.
        /// </summary>
        public static readonly DependencyProperty TypeProperty =
            DependencyProperty.Register(
                nameof(Type),
                typeof(ChartSelectionType),
                typeof(ChartSelectionBehavior),
                new PropertyMetadata(ChartSelectionType.Single, OnSelectionTypeChanged));


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
        internal static readonly DependencyProperty CursorProperty =
            DependencyProperty.Register(
                nameof(Cursor),
                typeof(InputSystemCursorShape),
                typeof(ChartSelectionBehavior),
                new PropertyMetadata(InputSystemCursorShape.Arrow));

        /// <summary>
        /// The DependencyProperty for <see cref="SelectionBrush"/> property.
        /// </summary>
        public static readonly DependencyProperty SelectionBrushProperty =
            DependencyProperty.Register(nameof(SelectionBrush), typeof(Brush), typeof(ChartSelectionBehavior),
                new PropertyMetadata(null, OnSeriesSelectionBrushChanged));

        /// <summary>
        /// The DependencyProperty for <see cref="SelectedIndex"/> property.
        /// </summary>
        public static readonly DependencyProperty SelectedIndexProperty =
            DependencyProperty.Register(nameof(SelectedIndex), typeof(int), typeof(ChartSelectionBehavior),
            new PropertyMetadata(-1, OnSelectedIndexChanged));

        /// <summary>
        /// The DependencyProperty for <see cref="SelectedIndexes"/> property.
        /// </summary>
        public static readonly DependencyProperty SelectedIndexesProperty =
            DependencyProperty.Register(nameof(SelectedIndexes), typeof(List<int>), typeof(ChartSelectionBehavior),
            new PropertyMetadata(null, null));

        #endregion

        #region Fields

        private ChartSegment? mouseUnderSegment;
        private ChartDataMarkerPresenter? selectedAdornmentPresenter;
        private int index;
        private ChartSelectionChangingEventArgs? selectionChangingEventArguments;
        private ChartSelectionChangedEventArgs? selectionChangedEventArguments;

        #endregion

        #region Events

        /// <summary>
        /// Occurs when the user clicks on series segment or sets the value for the <see cref="SelectedIndex"/> property. Here you can get the corresponding series, current selected index, and previous selected index. 
        /// </summary>
        public event EventHandler<ChartSelectionChangedEventArgs> SelectionChanged;

        /// <summary>
        /// Occurs when the user clicks on the series segment or sets the value for <see cref="SelectedIndex"/> property. This event is triggered before a segment or series is selected. 
        /// </summary>
        /// <remarks>Restrict a data point from being selected, by canceling this event, by setting cancel property to true in the event argument.</remarks>
        public event EventHandler<ChartSelectionChangingEventArgs> SelectionChanging;

        #endregion

        #region Constructor 

        /// <summary>
        /// Initializes a new instance of the <see cref="ChartSelectionBehavior"/> class.
        /// </summary>
        public ChartSelectionBehavior()
        {
            SelectedIndexes = new List<int>();

            selectionChangingEventArguments = new ChartSelectionChangingEventArgs();
            selectionChangedEventArguments = new ChartSelectionChangedEventArgs();
        }

        #endregion

        #region Properties

        #region Public Properties

        internal SelectionActivationMode ActivationMode
        {
            get { return (SelectionActivationMode)GetValue(ActivationModeProperty); }
            set { SetValue(ActivationModeProperty, value); }
        }

        /// <summary>
        /// Gets or sets the selection mode for selection behavior.
        /// </summary>
        /// <remarks>It's used to select single or multiple segments or series.</remarks>
        /// <value>One of the <see cref="ChartSelectionType"/>. The default is <see cref="ChartSelectionType.Single"/> selection.</value>
        /// <example>
        /// # [MainPage.xaml](#tab/tabid-1)
        /// <code><![CDATA[
        /// <chart:SfCircularChart>
        /// 
        ///         <chart:PieSeries ItemsSource="{Binding Data}"
        ///                          XBindingPath="XValue"
        ///                          YBindingPath="YValue">
        ///             <chart:PieSeries.SelectionBehavior>
        ///                 <chart:DataPointSelectionBehavior Type="Multiple" SelectionBrush = "Red" />
        ///         </chart:PieSeries.SelectionBehavior>
        ///         </chart:PieSeries>
        /// 
        /// </chart:SfCircularChart>
        /// ]]>
        /// </code>
        /// 
        /// # [MainPage.xaml.cs](#tab/tabid-2)
        /// <code><![CDATA[
        ///  SfCircularChart chart = new SfCircularChart();
        ///  ViewModel viewModel = new ViewModel();
        ///  
        ///  PieSeries series = new PieSeries()
        ///  {
        ///     ItemsSource = viewModel.Data,
        ///     XBindingPath = "XValue",
        ///     YBindingPath = "YValue",
        ///  };
        ///  
        ///  series.SelectionBehavior = new DataPointSelectionBehavior()
        ///  {
        ///      Type = ChartSelectionType.Multiple,
        ///      SelectionBrush = new SolidColorBrush(Colors.Red),
        ///  };
        ///  
        ///  chart.Series.Add(series);
        ///
        /// ]]>
        /// </code>
        /// ***
        /// </example>
        public ChartSelectionType Type
        {
            get { return (ChartSelectionType)GetValue(TypeProperty); }
            set { SetValue(TypeProperty, value); }
        }

        internal InputSystemCursorShape Cursor
        {
            get { return (InputSystemCursorShape)GetValue(CursorProperty); }
            set { SetValue(CursorProperty, value); }
        }

         /// <summary>
        /// Gets or sets the selection brush color for selection behavior.
        /// </summary>
        /// <value>This property takes <see cref="Brush"/> value and its default value is null.</value>
        /// <example>
        /// # [MainPage.xaml](#tab/tabid-3)
        /// <code><![CDATA[
        /// <chart:SfCircularChart>
        /// 
        ///         <chart:PieSeries ItemsSource="{Binding Data}"
        ///                          XBindingPath="XValue"
        ///                          YBindingPath="YValue">
        ///             <chart:PieSeries.SelectionBehavior>
        ///                 <chart:DataPointSelectionBehavior SelectionBrush = "Red" />
        ///         </chart:PieSeries.SelectionBehavior>
        ///         </chart:PieSeries>
        /// 
        /// </chart:SfCircularChart>
        /// ]]>
        /// </code>
        /// 
        /// # [MainPage.xaml.cs](#tab/tabid-4)
        /// <code><![CDATA[
        ///  SfCircularChart chart = new SfCircularChart();
        ///  ViewModel viewModel = new ViewModel();
        ///  
        ///  PieSeries series = new PieSeries()
        ///  {
        ///     ItemsSource = viewModel.Data,
        ///     XBindingPath = "XValue",
        ///     YBindingPath = "YValue",
        ///  };
        ///  
        ///  series.SelectionBehavior = new DataPointSelectionBehavior()
        ///  {
        ///      SelectionBrush = new SolidColorBrush(Colors.Red),
        ///  };
        ///  
        ///  chart.Series.Add(series);
        ///
        /// ]]>
        /// </code>
        /// ***
        /// </example>
        public Brush SelectionBrush
        {
            get { return (Brush)GetValue(SelectionBrushProperty); }
            set { SetValue(SelectionBrushProperty, value); }
        }

        /// <summary>
        /// Gets or sets the index of segment or series to be selected in selection behavior.
        /// </summary>
        /// <value>This property takes <see cref="int"/> value and its default value is -1.</value>
        /// <remarks>
        /// <para>This property value is used only when <see cref="ChartSelectionBehavior.Type"/> is set to <see cref="ChartSelectionType.Single"/> or <see cref="ChartSelectionType.SingleDeselect"/>.</para>
        /// </remarks>
        /// <example>
        /// # [MainPage.xaml](#tab/tabid-5)
        /// <code><![CDATA[
        /// <chart:SfCircularChart>
        ///
        ///         <chart:PieSeries ItemsSource="{Binding Data}"
        ///                          XBindingPath="XValue"
        ///                          YBindingPath="YValue">
        ///             <chart:PieSeries.SelectionBehavior>
        ///                 <chart:DataPointSelectionBehavior SelectedIndex="3" SelectionBrush = "Red" />
        ///         </chart:PieSeries.SelectionBehavior>
        ///         </chart:PieSeries>
        ///
        /// </chart:SfCircularChart>
        /// ]]>
        /// </code>
        /// 
        /// # [MainPage.xaml.cs](#tab/tabid-6)
        /// <code><![CDATA[
        ///  SfCircularChart chart = new SfCircularChart();
        ///  ViewModel viewModel = new ViewModel();
        ///
        ///  PieSeries series = new PieSeries()
        ///  {
        ///     ItemsSource = viewModel.Data,
        ///     XBindingPath = "XValue",
        ///     YBindingPath = "YValue",
        ///  };
        ///
        ///  series.SelectionBehavior = new DataPointSelectionBehavior()
        ///  {
        ///      SelectedIndex = 3,
        ///      SelectionBrush = new SolidColorBrush(Colors.Red),
        ///  };
        ///
        ///  chart.Series.Add(series);
        ///
        /// ]]>
        /// </code>
        /// ***
        /// </example>
        public int SelectedIndex
        {
            get { return (int)GetValue(SelectedIndexProperty); }
            set { SetValue(SelectedIndexProperty, value); }
        }

        /// <summary>
        /// Gets or sets the list of segments or series to be selected in selection behavior.
        /// </summary>
        /// <value>This property takes the list of <see cref="int"/> values and its default value is null.</value>
        /// <remarks>
        /// <para>This property value is used only when <see cref="ChartSelectionBehavior.Type"/> is set to <see cref="ChartSelectionType.Multiple"/>.</para>
        /// </remarks>
        /// <example>
        /// # [MainPage.xaml](#tab/tabid-7)
        /// <code><![CDATA[
        /// <chart:SfCircularChart>
        ///
        ///         <chart:PieSeries ItemsSource="{Binding Data}"
        ///                          XBindingPath="XValue"
        ///                          YBindingPath="YValue">
        ///             <chart:PieSeries.SelectionBehavior>
        ///                 <chart:DataPointSelectionBehavior SelectedIndexes="{Binding indexes}" SelectionBrush = "Red" />
        ///         </chart:PieSeries.SelectionBehavior>
        ///         </chart:PieSeries>
        ///
        /// </chart:SfCircularChart>
        /// ]]>
        /// </code>
        ///
        /// # [MainPage.xaml.cs](#tab/tabid-8)
        /// <code><![CDATA[
        ///  SfCircularChart chart = new SfCircularChart();
        ///  ViewModel viewModel = new ViewModel();
        ///
        ///  PieSeries series = new PieSeries()
        ///  {
        ///     ItemsSource = viewModel.Data,
        ///     XBindingPath = "XValue",
        ///     YBindingPath = "YValue",
        ///  };
        ///
        ///  List<int> indexes = new List<int>() { 1, 3, 5 };
        ///  series.SelectionBehavior = new DataPointSelectionBehavior()
        ///  {
        ///      SelectedIndexes= indexes,
        ///      SelectionBrush = new SolidColorBrush(Colors.Red),
        ///  };
        ///
        ///  chart.Series.Add(series);
        ///
        /// ]]>
        /// </code>
        /// ***
        /// </example> 
        public List<int> SelectedIndexes
        {
            get { return (List<int>)GetValue(SelectedIndexesProperty); }
            set { SetValue(SelectedIndexesProperty, value); }
        }

        #endregion

        #region Internal Properties

        internal bool EnableMultiSelection
        {
            get
            {
                return Type == ChartSelectionType.Multiple;
            }
        }

        #endregion

        #endregion

        #region Methods

        #region Public Methods

        internal void ClearSelection()
        {
            SelectedIndex = -1;

            SelectedIndexes?.Clear();
            Chart.SelectedSeriesCollection?.Clear();

            var seriesCollection = Chart.GetSeriesCollection();
            if (seriesCollection == null)
                return;

            foreach (ChartSeries series in seriesCollection)
            {
                 Chart.OnResetSeries(series);

                    series.SelectedSegmentsIndexes.Clear();

                    for (int i = 0; i < series.Segments.Count; i++)
                    {
                        series.OnResetSegment(i);
                    }
            }

        }

        #endregion

        #region Protected Internal Override Methods

        internal override void DetachElements()
        {
            base.DetachElements();

            if (SelectionChanged != null)
            {
                foreach (var handler in SelectionChanged.GetInvocationList())
                {
                    SelectionChanged -= handler as EventHandler<ChartSelectionChangedEventArgs>;
                }

                SelectionChanged = null;
            }

            if (SelectionChanging != null)
            {
                foreach (var handler in SelectionChanging.GetInvocationList())
                {
                    SelectionChanging -= handler as EventHandler<ChartSelectionChangingEventArgs>;
                }

                SelectionChanging = null;
            }

            this.selectionChangingEventArguments = null;
            this.selectionChangedEventArguments = null;
        }

        /// <inheritdoc />
        protected internal override void OnPointerReleased(PointerRoutedEventArgs e)
        {
            if(Chart == null)
            {
                return;
            }

            if (ActivationMode == SelectionActivationMode.Click && e.OriginalSource is FrameworkElement element)
            {
                ChartSegment segment = null;
                Chart.CurrentSelectedSeries = null;

                if (element.Tag != null && element.Tag is ChartSegment segmentTag) segment = segmentTag;

                var image = element as Image;

                if (image != null && image.Source is WriteableBitmap)
                {
                    // Bitmap segment selection process handling.
                    OnBitmapSeriesMouseDownSelection(element, e);
                }
                else if (segment != null && segment == mouseUnderSegment)
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
                        if ((segment.Series.ActualXAxis is CategoryAxis) && segment.Series.ActualXAxis is CategoryAxis actualXAxis 
                            && !actualXAxis.IsIndexed && segment.Series.IsSideBySide)
                            index = segment.Series.GroupedActualData.IndexOf(segment.Item);
                        else
                            index = segment.Series is CircularSeries && !double.IsNaN(((CircularSeries)segment.Series).GroupTo) ? segment.Series.Segments.IndexOf(segment) : segment.Series.ActualData.IndexOf(segment.Item);
                        OnMouseDownSelection(segment.Series, index);
                    }
                }
                else
                {
                    // Get the selected adornment index and select the adornment marker
                    index = ChartExtensionUtils.GetAdornmentIndex(element);
                    FrameworkElement frameworkElement = e.OriginalSource as FrameworkElement;
                    var chartAdornmentPresenter = frameworkElement as ChartDataMarkerPresenter;

                    while (frameworkElement != null && chartAdornmentPresenter == null)
                    {
                        frameworkElement = VisualTreeHelper.GetParent(frameworkElement) as FrameworkElement;
                        chartAdornmentPresenter = frameworkElement as ChartDataMarkerPresenter;
                    }

                    if (chartAdornmentPresenter != null)
                        OnMouseDownSelection(chartAdornmentPresenter.Series, index);
                }

                if (selectedAdornmentPresenter != null)
                {
                    selectedAdornmentPresenter = null;
                }
            }

            AdorningCanvas.ReleasePointerCapture(e.Pointer);
        }

        /// <inheritdoc />
        protected internal override void OnPointerPressed(PointerRoutedEventArgs e)
        {
            if(Chart == null)
            {
                return;
            }

            ChartSegment segment = null;
            Chart.CurrentSelectedSeries = null;

            if (e.OriginalSource is FrameworkElement element)
            {
                if (element.Tag != null && element.Tag is ChartSegment segmentTag) segment = segmentTag;
            }

            if (segment != null)
            {
                mouseUnderSegment = segment;
            }
        }

        #endregion

        #region Internal Methods

        
        internal void OnSelectionChanged(List<int> newIndexes, List<int> oldIndexes, object sender)
        {
            if (SelectionChanged != null && selectionChangedEventArguments != null)
            {
                selectionChangedEventArguments.NewIndexes = newIndexes ;
                selectionChangedEventArguments.OldIndexes = oldIndexes;
                SelectionChanged(sender, selectionChangedEventArguments);
            }
        }

        
        internal bool OnSelectionChanging(List<int> newIndexes, List<int> oldIndexes, object sender)
        {
            if (SelectionChanging != null && selectionChangingEventArguments != null)
            {
                selectionChangingEventArguments.NewIndexes = newIndexes;
                selectionChangingEventArguments.OldIndexes = oldIndexes;
                selectionChangingEventArguments.Cancel = false;
                
                SelectionChanging(sender, selectionChangingEventArguments);
                return selectionChangingEventArguments.Cancel;
            }

            return false;
        }

        internal virtual void OnSelectionIndexChanged(DependencyPropertyChangedEventArgs args)
        {

        }

        #endregion

        #region Private Static Methods
        private static void OnSeriesSelectionBrushChanged(DependencyObject d, DependencyPropertyChangedEventArgs args)
        {
            if (d is ChartSelectionBehavior behavior)
                behavior.UpdateArea();
        }

        private static void OnSelectedIndexChanged(DependencyObject d, DependencyPropertyChangedEventArgs args)
        {
            if (d is ChartSelectionBehavior behavior)
                behavior.OnSelectionIndexChanged(args);
        }

        private static void OnSelectionTypeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ChartSelectionBehavior behavior)
            {
                ChartBase chartBase = behavior.Chart;
                // WPF-26121, When selection style is changed the last selected segment remains selected
                if (chartBase == null)
                    return;
                var seriesCollection = chartBase.GetSeriesCollection();
                if (seriesCollection == null)
                    return;
                behavior.SelectedIndex = -1;

                foreach (ChartSeries series in seriesCollection)
                {
                    if (series != null)
                    {
                        if (series.SelectionBehavior != null)
                            series.SelectionBehavior.SelectedIndex = -1;

                        if (chartBase.SelectedSeriesCollection.Contains(series) &&
                            behavior.SelectedIndex != seriesCollection.IndexOf(series))
                        {
                            chartBase.SelectedSeriesCollection.Remove(series);
                            chartBase.OnResetSeries(series);
                        }

                        if (series.ActualData != null)
                        {
                            //TODO : Need to revisit the code.
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
                }
            }
        }

        #endregion

        #region Private Methods
        

       
        internal virtual void OnMouseDownSelection(ChartSeries series, object value)
        {
            var isScatterSeries = series is ScatterSeries;

            Chart.CurrentSelectedSeries = series;

            bool seriesSelection = (Chart.GetSeriesSelectionBehavior() != null)
                && (series.IsSideBySide
                || isScatterSeries
                || series is BubbleSeries
                || series is TriangularSeriesBase
                || series is CircularSeries
                || series is FastScatterBitmapSeries
                || !series.IsSideBySide);

            if (seriesSelection)
            {
                int index = Chart.VisibleSeries.IndexOf(series);

                // Call OnSelectionChanging method to raise SelectionChanging event 
                bool isCancel = Chart.OnSeriesSelectionChanging(SelectedIndex, index);
                if (!isCancel)
                {
                    if (EnableMultiSelection && Chart.SelectedSeriesCollection.Contains(Chart.CurrentSelectedSeries))
                    {
                        Chart.SelectedSeriesCollection.Remove(Chart.CurrentSelectedSeries);
                        SelectedIndex = -1;
                        Chart.OnResetSeries(Chart.CurrentSelectedSeries as ChartSeries);
                    }
                    else if (SelectedIndex == index)
                        SelectedIndex = -1;
                    else
                    {
                        SelectedIndex = index;
                        Chart.PreviousSelectedSeries = Chart.CurrentSelectedSeries;
                    }
                }
            }
        }

        
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1801: Review unused parameters")]
        private void OnBitmapSeriesMouseDownSelection(FrameworkElement element, PointerRoutedEventArgs e)
        {
            Point canvasPoint = e.GetCurrentPoint(Chart.GetAdorningCanvas()).Position;
            var seriesCollection = Chart.GetSeriesCollection();
            if (seriesCollection.Count > 0 && seriesCollection[seriesCollection.Count - 1] is ChartSeries series)
            {
                Chart.CurrentSelectedSeries = series;

                ChartDataPointInfo data = Chart.CurrentSelectedSeries.GetDataPoint(canvasPoint);
                OnMouseDownSelection(Chart.CurrentSelectedSeries, data);
            }
        }

        #endregion

        #endregion
    }

    /// <summary>
    /// Represents the chart selection changed event arguments.
    /// </summary>
    /// <remarks>
    /// It contains data for the CurrentIndex and PreviousIndex.
    /// </remarks>
    public class ChartSelectionChangedEventArgs : EventArgs
    {
        #region Properties

        /// <summary>
        /// Gets the collection of selected data point indexes.
        /// </summary>
        /// <remarks>NewIndexes[0] is the current selected index.</remarks>
        public List<int> NewIndexes { get; internal set; }

        /// <summary>
        /// Gets the collection of previous selected data point indexes.
        /// </summary>
        /// <remarks>OldIndexes[0] is the current unselected index.</remarks>
        public List<int> OldIndexes { get; internal set; }
        
        #endregion
    }

    /// <summary>
    /// Represents the chart selection changing event arguments.
    /// </summary>
    /// <remarks>
    /// It contains data for the CurrentIndex and PreviousIndex.
    /// </remarks>
    public class ChartSelectionChangingEventArgs : CancelEventArgs
    {
        #region Properties

        /// <summary>
        /// Gets the collection of selected data point indexes.
        /// </summary>
        /// <remarks>NewIndexes[0] is the current selected index.</remarks>
        public List<int> NewIndexes { get; internal set; }

        /// <summary>
        /// Gets the collection of previous selected data point indexes.
        /// </summary>
        /// <remarks>OldIndexes[0] is the current unselected index.</remarks>
        public List<int> OldIndexes { get; internal set; }

        #endregion
    }
}
