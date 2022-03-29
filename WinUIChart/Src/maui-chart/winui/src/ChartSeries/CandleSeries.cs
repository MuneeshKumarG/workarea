﻿using System;
using System.Collections.Generic;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;
using Windows.Foundation;
using AdornmentsPosition = Syncfusion.UI.Xaml.Charts.BarLabelAlignment;
using SelectionStyle = Syncfusion.UI.Xaml.Charts.SelectionType;

namespace Syncfusion.UI.Xaml.Charts
{
    /// <summary>
    /// CandleSeries is used primarily to analyze price movements of a stcock market over a period of time.
    /// </summary>
    /// <remarks>
    /// Each data point contains four values namely open, high, low, close. Typically, the high and low values are connected using a vertical straight line, 
    /// whereas the region between open and close values are connected using a vertical column segment.
    /// </remarks>
    /// <example>
    /// <code language = "XAML" >
    ///       &lt;syncfusion:CandleSeries ItemsSource="{Binding Data}" XBindingPath="Year" High="High" Open="Open" Close="Close" Low="Low"&gt;
    ///       &lt;/syncfusion:CandleSeries&gt;
    /// </code>
    /// <code language= "C#" >
    ///       CandleSeries series1 = new CandleSeries();
    ///       series1.ItemsSource = viewmodel.Data;
    ///       series1.XBindingPath = "Year";
    ///       series1.High="High";
    ///       series1.Low="Low";
    ///       series1.Open="Open";
    ///       series1.Close="Close";
    ///       chart.Series.Add(series1);
    ///    </code>
    /// </example>
    /// <seealso cref="CandleSegment"/>
    /// <seealso cref="HiLoSeries"/>
    /// <seealso cref="HiLoOpenCloseSeries"/>    
    internal class CandleSeries : FinancialSeriesBase, ISegmentSpacing, ISegmentSelectable
    {
        #region Dependency Property Registration

        /// <summary>
        /// The DependencyProperty for <see cref="SegmentSpacing"/> property.
        /// </summary>
        public static readonly DependencyProperty SegmentSpacingProperty =
            DependencyProperty.Register("SegmentSpacing", typeof(double), typeof(CandleSeries),
            new PropertyMetadata(0.0, OnSegmentSpacingChanged));

        /// <summary>
        /// The DependencyProperty for <see cref="SelectionBrush"/> property.
        /// </summary>
        public static readonly DependencyProperty SelectionBrushProperty =
            DependencyProperty.Register(nameof(SelectionBrush), typeof(Brush), typeof(CandleSeries),
            new PropertyMetadata(null, OnSegmentSelectionBrush));

        /// <summary>
        ///  The DependencyProperty for <see cref="SelectedIndex"/> property.
        /// </summary>
        public static readonly DependencyProperty SelectedIndexProperty =
            DependencyProperty.Register("SelectedIndex", typeof(int), typeof(CandleSeries),
            new PropertyMetadata(-1, OnSelectedIndexChanged));

        #endregion
        
        #region Constructor

        public CandleSeries()
        {
            DefaultStyleKey = typeof(CandleSeries);
        }

        #endregion

        #region Properties

        #region Public Properties
        
        /// <summary>
        /// Gets or sets the spacing between the segments across the series in cluster mode.
        /// </summary>
        /// <value>
        ///     The value ranges from 0 to 1.
        /// </value>
        public double SegmentSpacing
        {
            get { return (double)GetValue(SegmentSpacingProperty); }
            set { SetValue(SegmentSpacingProperty, value); }
        }

        /// <summary>
        /// Gets or sets the interior (brush) for the selected segment(s).
        /// </summary>
        public Brush SelectionBrush
        {
            get { return (Brush)GetValue(SelectionBrushProperty); }
            set { SetValue(SelectionBrushProperty, value); }
        }

        /// <summary>
        /// Gets or sets the index of the selected segment.
        /// </summary>
        /// <value>
        /// <c>Int</c> value represents the index of the data point(or segment) to be selected and its default value is -1. 
        /// </value>
        public int SelectedIndex
        {
            get { return (int)GetValue(SelectedIndexProperty); }
            set { SetValue(SelectedIndexProperty, value); }
        }

        #endregion

        #region Internal Override Properties

        internal override bool IsMultipleYPathRequired
        {
            get
            {
                return true;
            }
        }

        #endregion

        #region Protected Internal Override Properties

        /// <summary>
        /// Gets a value indicating whether this series is placed side by side.
        /// </summary>
        /// <returns>
        /// It returns <c>true</c>, if the series is placed side by side [cluster mode].
        /// </returns>
        internal override bool IsSideBySide
        {
            get
            {
                return true;
            }
        }

        #endregion

        #endregion

        #region Methods

        #region Interface Methods

        double ISegmentSpacing.CalculateSegmentSpacing(double spacing, double Right, double Left)
        {
            return CalculateSegmentSpacing(spacing, Right, Left);
        }

        #endregion

        #region Public Override Methods

        /// <summary>
        /// Creates the segments of CandleSeries.
        /// </summary>
        public override void CreateSegments()
        {
            DoubleRange sbsInfo = this.GetSideBySideInfo(this);
            double center = sbsInfo.Median;
            List<double> xValues = null;
            if (ActualXAxis is CategoryAxis && !(ActualXAxis as CategoryAxis).IsIndexed)
                xValues = GroupedXValuesIndexes;
            else
                xValues = GetXValues();
            IList<double> values = GetComparisionModeValues();
            if (xValues != null)
            {
                ClearUnUsedSegments(this.DataCount);

                if (AdornmentsInfo != null && ShowDataLabels)
                {
                    AdornmentsPosition markerPosition = this.adornmentInfo.GetAdornmentPosition();
                    if (markerPosition == AdornmentsPosition.Middle)
                        ClearUnUsedAdornments(this.DataCount * 4);
                    else
                        ClearUnUsedAdornments(this.DataCount * 2);
                }

                if (this.ActualXAxis is CategoryAxis && (!(this.ActualXAxis as CategoryAxis).IsIndexed))
                {
                    Segments.Clear();
                    Adornments.Clear();
                    for (int i = 0; i < xValues.Count; i++)
                    {
                        if (i < xValues.Count && GroupedSeriesYValues[0].Count > i)
                        {
                            double x1 = xValues[i] + sbsInfo.Start;
                            double x2 = xValues[i] + sbsInfo.End;
                            double y1 = GroupedSeriesYValues[2][i];
                            double y2 = GroupedSeriesYValues[3][i];
                            bool isbull = false;
                            if (i == 0 || this.ComparisonMode == FinancialPrice.None)
                                isbull = GroupedSeriesYValues[3][i] > GroupedSeriesYValues[2][i];
                            else
                                isbull = values[i] >= values[i - 1];
                            ChartPoint cdpBottomLeft = new ChartPoint(x1, y1);
                            ChartPoint cdpRightTop = new ChartPoint(x2, y2);

                            ChartPoint hipoint = new ChartPoint(xValues[i] + center, GroupedSeriesYValues[0][i]);
                            ChartPoint lopoint = new ChartPoint(xValues[i] + center, GroupedSeriesYValues[1][i]);
                            CandleSegment segment = CreateSegment() as CandleSegment;
                            if (segment != null)
                            {
                                segment.Series = this;
                                segment.BullFillColor = BullFillColor;
                                segment.BearFillColor = BearFillColor;
                                segment.Item = ActualData[i];
                                segment.SetData(cdpBottomLeft, cdpRightTop, hipoint, lopoint, isbull);
                                segment.High = GroupedSeriesYValues[0][i];
                                segment.Low = GroupedSeriesYValues[1][i];
                                segment.Open = GroupedSeriesYValues[2][i];
                                segment.Close = GroupedSeriesYValues[3][i];
                                Segments.Add(segment);
                            }

                            if (AdornmentsInfo != null && ShowDataLabels)
                                AddAdornments(xValues[i], hipoint, lopoint, cdpBottomLeft, cdpBottomLeft, cdpRightTop, cdpRightTop, i, 0);
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < DataCount; i++)
                    {
                        double x1 = xValues[i] + sbsInfo.Start;
                        double x2 = xValues[i] + sbsInfo.End;
                        double y1 = OpenValues[i];
                        double y2 = CloseValues[i];
                        bool isbull = false;
                        if (i == 0 || this.ComparisonMode == FinancialPrice.None)
                            isbull = CloseValues[i] > OpenValues[i];
                        else
                            isbull = values[i] >= values[i - 1];
                        ChartPoint cdpBottomLeft = new ChartPoint(x1, y1);
                        ChartPoint cdpRightTop = new ChartPoint(x2, y2);

                        ChartPoint hipoint = new ChartPoint(xValues[i] + center, HighValues[i]);
                        ChartPoint lopoint = new ChartPoint(xValues[i] + center, LowValues[i]);

                        if (i < Segments.Count)
                        {
                            (Segments[i]).SetData(cdpBottomLeft, cdpRightTop, hipoint, lopoint, isbull);
                            (Segments[i] as CandleSegment).High = HighValues[i];
                            (Segments[i] as CandleSegment).Low = LowValues[i];
                            (Segments[i] as CandleSegment).Open = OpenValues[i];
                            (Segments[i] as CandleSegment).Close = CloseValues[i];
                            Segments[i].Item = ActualData[i];
                        }
                        else
                        {
                            CandleSegment segment = CreateSegment() as CandleSegment;
                            if (segment != null)
                            {
                                segment.Series = this;
                                segment.BullFillColor = BullFillColor;
                                segment.BearFillColor = BearFillColor;
                                segment.Item = ActualData[i];
                                segment.SetData(cdpBottomLeft, cdpRightTop, hipoint, lopoint, isbull);
                                segment.High = HighValues[i];
                                segment.Low = LowValues[i];
                                segment.Open = OpenValues[i];
                                segment.Close = CloseValues[i];
                                Segments.Add(segment);
                            }
                        }

                        if (AdornmentsInfo != null && ShowDataLabels)
                            AddAdornments(xValues[i], hipoint, lopoint, cdpBottomLeft, cdpBottomLeft, cdpRightTop, cdpRightTop, i, 0);
                    }
                }

                if (ShowEmptyPoints)
                    UpdateEmptyPointSegments(xValues, true);
            }
        }

        #endregion

        #region Protected Override Methods

        /// <inheritdoc/>
        protected override ChartSegment CreateSegment()
        {
            return new CandleSegment();
        }

        #endregion

        #region Internal Override Methods

        internal override Point GetDataPointPosition(ChartTooltip tooltip)
        {
            CandleSegment candleSegment = ToolTipTag as CandleSegment;
            Point newPosition = new Point();
            Point point = ChartTransformer.TransformToVisible(candleSegment.XRange.Median, candleSegment.High);
            newPosition.X = point.X + ActualArea.SeriesClipRect.Left;
            newPosition.Y = point.Y + ActualArea.SeriesClipRect.Top;
            return newPosition;
        }

        #endregion


        #region Protected Methods

        ///<summary>
        ///Method used to calculate the segment spacing.
        ///</summary>
        /// <param name="spacing">Spacing</param>
        /// <param name="Right">Right</param>
        /// <param name="Left">Left</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "Reviewed")]
        protected double CalculateSegmentSpacing(double spacing, double Right, double Left)
        {
            double diff = Right - Left;
            double totalspacing = diff * spacing / 2;
            Left = Left + totalspacing;
            Right = Right - totalspacing;
            return Left;
        }

        #endregion

        #region Private Static Methods

        private static void OnSegmentSpacingChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var series = d as CandleSeries;
            if (series.Area != null)
                series.Area.ScheduleUpdate();
        }
        
        private static void OnSegmentSelectionBrush(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as CandleSeries).UpdateArea();
        }
        
        private static void OnSelectedIndexChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ChartSeries series = d as ChartSeries;
            series.OnPropertyChanged("SelectedIndex");
            if (series.ActualArea == null || series.ActualArea.SelectionBehaviour == null) return;
            if (!series.ActualArea.SelectionBehaviour.EnableMultiSelection)
                series.SelectedIndexChanged((int)e.NewValue, (int)e.OldValue);
            else if ((int)e.NewValue != -1)
                series.SelectedSegmentsIndexes.Add((int)e.NewValue);
        }

        #endregion

        #region Private Methods
        
        /// <summary>
        /// Method is used to check the point in on line
        /// </summary>
        /// <param name="startPoint"></param>
        /// <param name="endPoint"></param>
        /// <param name="checkPoint"></param>
        /// <returns></returns>
        private bool PointIsOnLine(Point startPoint, Point endPoint, Point checkPoint)
        {
            if (!IsTransposed)
            {
                if (Math.Round(startPoint.X) == Math.Ceiling(checkPoint.X)
                    || Math.Round(startPoint.X) == Math.Floor(checkPoint.X))
                {
                    return endPoint.Y - startPoint.Y ==
                        (endPoint.Y - checkPoint.Y) + (checkPoint.Y - startPoint.Y) ? true : false;
                }

                return false;
            }
            else
            {
                if (Math.Round(startPoint.Y) == Math.Ceiling(checkPoint.Y)
                    || Math.Round(startPoint.Y) == Math.Floor(checkPoint.Y))
                {
                    return endPoint.X - startPoint.X ==
                        (endPoint.X - checkPoint.X) + (checkPoint.X - startPoint.X) ? true : false;
                }

                return false;
            }
        }

        #endregion

        #endregion
    }
}
