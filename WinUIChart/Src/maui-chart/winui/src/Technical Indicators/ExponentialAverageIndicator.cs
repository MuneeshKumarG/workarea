using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
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
    /// Represents the ExponentialAverage technical indicator.
    /// </summary>
    /// <seealso cref="FinancialTechnicalIndicator"/>
    /// <seealso cref="TechnicalIndicatorSegment"/>
    internal class ExponentialAverageIndicator : FinancialTechnicalIndicator
    {
        #region constructor

        /// <summary>
        /// Called when instance created for <see cref="ExponentialAverageIndicator"/>.
        /// </summary>
        public ExponentialAverageIndicator()
        {
        }

        #endregion

        #region fields

        IList<double> CloseValues = new List<double>();

        List<double> xValues;

        List<double> xPoints = new List<double>();

        List<double> yPoints = new List<double>();

        TechnicalIndicatorSegment fastLineSegment;

        #endregion

        #region properties

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
            DependencyProperty.Register("Period", typeof(int), typeof(ExponentialAverageIndicator), 
            new PropertyMetadata(2, OnPeriodChanged));

        /// <summary>
        /// Gets or sets the color for the signal line.
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
            DependencyProperty.Register("SignalLineColor", typeof(Brush), typeof(ExponentialAverageIndicator),
                new PropertyMetadata(new SolidColorBrush(Colors.Green),OnColorChanged));

        #endregion

        #region Methods

        private static void OnColorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ExponentialAverageIndicator indicator = d as ExponentialAverageIndicator;
            indicator.UpdateArea();
        }

        private static void OnPeriodChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ExponentialAverageIndicator indicator = d as ExponentialAverageIndicator;
            indicator.UpdateArea();
        }
        /// <summary>
        /// Called when ItemsSource changed.
        /// </summary>
        /// <param name="oldValue">Specifies the old value.</param>
        /// <param name="newValue">Specifies the new value.</param>
        protected override void OnDataSourceChanged(IEnumerable oldValue, IEnumerable newValue)
        {
            base.OnDataSourceChanged(oldValue, newValue);
            fastLineSegment = null;
            CloseValues.Clear();
            GeneratePoints(new string[] { Close }, CloseValues);
            this.UpdateArea();
        }

        /// <summary>
        /// Called when binding path changed.
        /// </summary>
        /// <param name="args">Event args.</param>
        protected override void OnBindingPathChanged(DependencyPropertyChangedEventArgs args)
        {
            fastLineSegment = null;
            CloseValues.Clear();
            base.OnBindingPathChanged(args);
        }

        /// <summary>
        /// Method implementation for set ItemsSource to TechnicalIndicator.
        /// </summary>
        /// <param name="series">TechnicalIndicator instance.</param>
        protected internal override void SetSeriesItemSource(ChartSeriesBase series)
        {
            if (series.ActualSeriesYValues.Length > 0)
            {
                this.ActualXValues = Clone(series.ActualXValues);
                this.CloseValues = Clone(series.ActualSeriesYValues[0]);
                this.Area.ScheduleUpdate();
            }
        }

        /// <summary>
        /// Method implementation for GeneratePoints for TechnicalIndicator.
        /// </summary>
        protected internal override void GeneratePoints()
        {
            GeneratePoints(new string[] { Close }, CloseValues);
        }

        /// <summary>
        /// Creates the Segments of <see cref="ExponentialAverageIndicator"/>.
        /// </summary>
        public override void CreateSegments()
        {
            xValues = GetXValues();
            if (Period < 1) return;
            Period = Period < xValues.Count ? Period : xValues.Count - 1;
            CalculateExponential(Period, xValues, xPoints, yPoints);

            if (fastLineSegment == null)
            {
                fastLineSegment = new TechnicalIndicatorSegment(xValues, yPoints, SignalLineColor, this, Period);
                fastLineSegment.SetValues(xValues, yPoints, SignalLineColor, this, Period);
                Segments.Add(fastLineSegment);
            }
            else if (ActualXValues != null)
            {
                fastLineSegment.SetData(xPoints, yPoints,SignalLineColor,Period);
                fastLineSegment.SetRange();
            }
           
        }

        private void CalculateExponential(int len, List<double> xValues, List<double> xPoints, List<double> yPoints)
        {
            double lastValue = double.NaN;
            double alpha = 2 / (1d + len);
            double oneMinusAlpha = 1d - alpha;
            xPoints.Clear(); 
            yPoints.Clear();
            for (int j = 0; j < this.DataCount; j++)
            {
                if (double.IsNaN(lastValue))
                {
                    lastValue = CloseValues[j];
                }
                else
                {
                    lastValue = alpha * CloseValues[j] + oneMinusAlpha * lastValue;
                }
                yPoints.Add(lastValue);
                xPoints.Add(xValues[j]);
            }
        }

        /// <summary>
        /// Updates the segment at the specified index.
        /// </summary>
        /// <param name="index">The index of the segment.</param>
        /// <param name="action">The action that caused the segments collection changed event.</param>
        public override void UpdateSegments(int index, NotifyCollectionChangedAction action)
        {
            this.Area.ScheduleUpdate();
        }

        /// <inheritdoc/>
        protected override ChartSegment CreateSegment()
        {
            return null;
        }
#endregion

    }
}
