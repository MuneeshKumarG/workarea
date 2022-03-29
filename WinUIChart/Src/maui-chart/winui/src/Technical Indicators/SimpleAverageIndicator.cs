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
    /// Represents the SimpleAverage technical indicator.
    /// </summary>
    /// <seealso cref="FinancialTechnicalIndicator"/>
    /// <seealso cref="TechnicalIndicatorSegment"/>
    internal class SimpleAverageIndicator : FinancialTechnicalIndicator
    {

        #region constructor

        /// <summary>
        /// Called when instance created for <see cref="SimpleAverageIndicator"/>.
        /// </summary>
        public SimpleAverageIndicator()
        {
        }

        #endregion

        #region fields

        IList<double> YValues = new List<double>();

        List<double> xValues;

        List<double> xPoints=new List<double>();

        List<double> yPoints=new List<double>();

        TechnicalIndicatorSegment fastLineSegment;

        #endregion

        #region Properties
        
        /// <summary>
        /// Gets or sets the moving average period for indicator.
        /// </summary>
        /// <remarks>
        /// The default value is 14 days.
        /// </remarks>
        public int Period
        {
            get { return (int)GetValue(PeriodProperty); }
            set { SetValue(PeriodProperty, value); }
        }

        /// <summary>
        /// The DependencyProperty for <see cref="Period"/> property.
        /// </summary>
        public static readonly DependencyProperty PeriodProperty =
            DependencyProperty.Register("Period", typeof(int), typeof(SimpleAverageIndicator), 
            new PropertyMetadata(2,OnMovingAverageChanged));

        /// <summary>
        /// Gets or sets the signal line color.
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
            DependencyProperty.Register("SignalLineColor", typeof(Brush), typeof(SimpleAverageIndicator),
            new PropertyMetadata(new SolidColorBrush(Colors.Blue),OnColorChanged));


        #endregion

        #region Methods

        private static void OnColorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            SimpleAverageIndicator indicator = d as SimpleAverageIndicator;
            if (indicator != null) indicator.UpdateArea();
        }

        private static void OnMovingAverageChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            SimpleAverageIndicator indicator = d as SimpleAverageIndicator;
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
            YValues.Clear();
            GeneratePoints(new string[] { Close }, YValues);
            this.UpdateArea();
        }

        /// <summary>
        /// Called when binding path changed.
        /// </summary>
        /// <param name="args">Event args.</param>
        protected override void OnBindingPathChanged(DependencyPropertyChangedEventArgs args)
        {
            fastLineSegment = null;
            YValues.Clear();
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
                this.YValues = Clone(series.ActualSeriesYValues[0]);
                this.Area.ScheduleUpdate();
            }
        }

        /// <summary>
        /// Method implementation for GeneratePoints for TechnicalIndicator.
        /// </summary>
        protected internal override void GeneratePoints()
        {
            GeneratePoints(new string[] { Close }, YValues);
        }

        /// <summary>
        /// Creates the segments of <see cref="SimpleAverageIndicator"/>.
        /// </summary>
        public override void CreateSegments()
        {
            xValues = GetXValues();
            if (Period < 1) return;
            Period = Period < xValues.Count ? Period : xValues.Count - 1;
            ComputeMovingAverage(Period, xValues, YValues, xPoints, yPoints);

            if (fastLineSegment == null)
            {
                TechnicalIndicatorSegment segment = new TechnicalIndicatorSegment(xValues, yPoints, SignalLineColor, this,Period);
                segment.SetValues(xValues, yPoints, SignalLineColor, this, Period);
                fastLineSegment = segment;
                Segments.Add(segment);
                
            }
            else if (ActualXValues != null)
            {
                fastLineSegment.SetData(xValues, yPoints, SignalLineColor, Period);
                fastLineSegment.SetRange();
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
