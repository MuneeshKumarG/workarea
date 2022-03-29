﻿using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;
#if WinUI_Desktop
using Microsoft.UI;
#else
using Windows.UI;
#endif
using System.Collections;

namespace Syncfusion.UI.Xaml.Charts
{
    /// <summary>
    /// Represents the AverageTrueRange technical indicator.
    /// </summary>
    /// <seealso cref="FinancialTechnicalIndicator"/>
    /// <seealso cref="TechnicalIndicatorSegment"/>
    internal class AverageTrueRangeIndicator : FinancialTechnicalIndicator
    {
#region constructor

        /// <summary>
        /// Called when instance created for <see cref="AverageTrueRangeIndicator"/>.
        /// </summary>
        public AverageTrueRangeIndicator()
        {
        }

#endregion

#region fields

        IList<double> _closeValues = new List<double>();

        IList<double> _highValues = new List<double>();

        IList<double> _lowValues = new List<double>();

        IList<double> range = new List<double>();

        IList<double> trueRange = new List<double>();

        IList<double> HCp = new List<double>();

        IList<double> LCp = new List<double>();


        List<double> _xValues;

        List<double> xPoints = new List<double>();

        List<double> yPoints = new List<double>();

        TechnicalIndicatorSegment _fastLineSegment;

#endregion

#region Properties

        internal override bool IsMultipleYPathRequired
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// Gets or sets the moving average value for the indicator.
        /// </summary>
        public int Period
        {
            get { return (int)GetValue(PeriodProperty); }
            set { SetValue(PeriodProperty, value); }
        }

        /// <summary>
        /// The DependencyProperty for <see cref="Period"/> property.
        /// </summary>
        public static readonly DependencyProperty PeriodProperty =
            DependencyProperty.Register("Period", typeof(int), typeof(AverageTrueRangeIndicator), 
            new PropertyMetadata(14, OnMovingAverageChanged));

        /// <summary>
        /// Gets or sets the color for the signal Line.
        /// </summary>
        /// <value>
        /// The <see cref="Brush"/> value.
        /// </value>
        public Brush SignalLineColor
        {
            get { return (Brush)GetValue(SignalLineColorProperty); }
            set { SetValue(SignalLineColorProperty, value); }
        }

        /// <summary>
        /// The DependencyProperty for <see cref="SignalLineColor"/> property.
        /// </summary>
        public static readonly DependencyProperty SignalLineColorProperty =
            DependencyProperty.Register("SignalLineColor", typeof(Brush), typeof(AverageTrueRangeIndicator),
                new PropertyMetadata(new SolidColorBrush(Colors.Green),OnColorChanged));

#endregion

#region Methods

        private static void OnColorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            AverageTrueRangeIndicator indicator = d as AverageTrueRangeIndicator;
            if (indicator != null) indicator.UpdateArea();
        }

        private static void OnMovingAverageChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            AverageTrueRangeIndicator indicator = d as AverageTrueRangeIndicator;
            if (indicator != null) indicator.UpdateArea();
        }

        /// <summary>
        /// Called when ItemsSource property changed.
        /// </summary>
        /// <param name="oldValue">Specifies the old value.</param>
        /// <param name="newValue">Specifies the new value.</param>
        protected override void OnDataSourceChanged(IEnumerable oldValue, IEnumerable newValue)
        {
            base.OnDataSourceChanged(oldValue, newValue);
            _fastLineSegment = null;
            _highValues.Clear();
            _lowValues.Clear();
            _closeValues.Clear();
            GeneratePoints(new[] {High,Low, Close },_highValues,_lowValues,_closeValues);
            UpdateArea();
        }

        /// <summary>
        /// Called when binding path changed.
        /// </summary>
        /// <param name="args">Event args.</param>
        protected override void OnBindingPathChanged(DependencyPropertyChangedEventArgs args)
        {
            _fastLineSegment = null;
            _highValues.Clear();
            _lowValues.Clear();
            _closeValues.Clear();
            base.OnBindingPathChanged(args);
        }

        /// <summary>
        /// Method implementation for set ItemsSource to TechnicalIndicator.
        /// </summary>
        /// <param name="series">TechnicalIndicator instance.</param>
        protected internal override void SetSeriesItemSource(ChartSeriesBase series)
        {
            if (series.ActualSeriesYValues.Length > 3)
            {
                ActualXValues = Clone(series.ActualXValues);
                _highValues = Clone(series.ActualSeriesYValues[0]);
                _lowValues = Clone(series.ActualSeriesYValues[1]);
                _closeValues = Clone(series.ActualSeriesYValues[2]);
                Area.ScheduleUpdate();
            }
        }

        /// <summary>
        /// Method implementation for GeneratePoints for TechnicalIndicator.
        /// </summary>
        protected internal override void GeneratePoints()
        {
            GeneratePoints(new[] { High, Low, Close }, _highValues, _lowValues, _closeValues);
        }

        /// <summary>
        /// Creates the segments of <see cref="AverageTrueRangeIndicator"/>.
        /// </summary>
        public override void CreateSegments()
        {
            _xValues = GetXValues();
            if ((Period < 1) || (Period >= DataCount))
            {
                Segments.Clear();
                return;
            }

            ComputeAverageTrueRange(Period);

            if ((_fastLineSegment == null) || (Segments.Count == 0))
            {
                _fastLineSegment = new TechnicalIndicatorSegment(xPoints, yPoints, SignalLineColor, this,Period);
                _fastLineSegment.SetValues(xPoints, yPoints, SignalLineColor, this, Period);
                Segments.Add(_fastLineSegment);
            }
            else
            {
                _fastLineSegment.SetData(xPoints, yPoints,SignalLineColor,Period);
                _fastLineSegment.SetRange();
            }
        }

        private void ComputeAverageTrueRange(int len)
        {
            xPoints.Clear();
            yPoints.Clear();
            double atr = 0;
            range.Add(_highValues[0] - _lowValues[0]);
            HCp.Add(double.NaN);
            LCp.Add(double.NaN);
            trueRange.Add(range[0]);
            for (int i = 1; i < DataCount; i++)
            {
                range.Add(_highValues[i] - _lowValues[i]);
                HCp.Add(Math.Abs(_highValues[i] - _closeValues[i - 1]));
                LCp.Add(Math.Abs(_lowValues[i] - _closeValues[i - 1]));
                trueRange.Add(Math.Max(range[i], Math.Max(HCp[i], LCp[i])));
            }
            for (int i = 0; i < len; i++)
            {
                xPoints.Add(_xValues[i]);
                yPoints.Add(0);
                atr += trueRange[i];
            }
            yPoints[len - 1] = atr / len;
            for (int i = len; i < DataCount; i++)
            {
                xPoints.Add(_xValues[i]);
                yPoints.Add(((yPoints[i - 1] * (len - 1)) + trueRange[i]) / len);
            }
        }

        /// <summary>
        /// Updates the segment at the specified index.
        /// </summary>
        /// <param name="index">The index of the segment.</param>
        /// <param name="action">The action that caused the segments collection changed event.</param>
        public override void UpdateSegments(int index, NotifyCollectionChangedAction action)
        {
            Area.ScheduleUpdate();
        }

        /// <inheritdoc/>
        protected override ChartSegment CreateSegment()
        {
            return null;
        }
#endregion

    }
}
