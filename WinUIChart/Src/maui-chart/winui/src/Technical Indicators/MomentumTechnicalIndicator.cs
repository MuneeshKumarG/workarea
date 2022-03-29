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
    /// Represents the Momentum technical indicator.
    /// </summary>
    /// <seealso cref="FinancialTechnicalIndicator"/>
    /// <seealso cref="TechnicalIndicatorSegment"/>

    internal class MomentumTechnicalIndicator : FinancialTechnicalIndicator
    {
        #region ctor

        /// <summary>
        /// Called when instance created for <see cref="MomentumTechnicalIndicator"/>.
        /// </summary>
        public MomentumTechnicalIndicator()
        {
        }

        #endregion

        #region fields

        IList<double> closeValues = new List<double>();

        List<double> xValues;

        List<double> momentumXPoints = new List<double>();

        List<double> momentumYPoints = new List<double>();

        List<double> centerXPoints = new List<double>();

        List<double> centerYPoints = new List<double>();

        TechnicalIndicatorSegment momentumLineSegment;

        TechnicalIndicatorSegment centerlLineSegment;

        #endregion

        #region properties

        /// <summary>
        /// Gets or sets the momentum time span.
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
            DependencyProperty.Register("Period", typeof(int), typeof(MomentumTechnicalIndicator), 
            new PropertyMetadata(14, OnMovingAverageChanged));

        /// <summary>
        /// Gets or sets the momentum line color.
        /// </summary>
        /// <value>
        /// The <see cref="Brush"/> value.
        /// </value>
        public Brush MomentumLineColor
        {
            get { return (Brush)GetValue(MomentumLineColorProperty); }
            set { SetValue(MomentumLineColorProperty, value); }
        }

        /// <summary>
        /// The DependencyProperty for <see cref="MomentumLineColor"/> property.
        /// </summary>
        public static readonly DependencyProperty MomentumLineColorProperty =
            DependencyProperty.Register("MomentumLineColor", typeof(Brush), typeof(MomentumTechnicalIndicator),
                new PropertyMetadata(new SolidColorBrush(Colors.Red), OnColorChanged));

        /// <summary>
        /// Gets or sets the center line color.
        /// </summary>
        /// <value>
        /// The <see cref="Brush"/> value.
        /// </value>
        public Brush CenterLineColor
        {
            get { return (Brush)GetValue(CenterLineColorProperty); }
            set { SetValue(CenterLineColorProperty, value); }
        }

        /// <summary>
        /// The DependencyProperty for <see cref="CenterLineColor"/> property.
        /// </summary>
        public static readonly DependencyProperty CenterLineColorProperty =
            DependencyProperty.Register("CenterLineColor", typeof(Brush), typeof(MomentumTechnicalIndicator),
            new PropertyMetadata(new SolidColorBrush(Colors.Green), OnColorChanged));

        #endregion

        #region Methods


        internal override void SetIndicatorInfo(ChartPointInfo info, List<double> yValue, bool seriesPalette)
        {
            if (yValue.Count > 0)
                info.SignalLine = double.IsNaN(yValue[0])? "null" : Math.Round(yValue[0], 2).ToString();
        }       

        private static void OnColorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            MomentumTechnicalIndicator indicator = d as MomentumTechnicalIndicator;
            if (indicator != null) indicator.UpdateArea();
        }

        private static void OnMovingAverageChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            MomentumTechnicalIndicator indicator = d as MomentumTechnicalIndicator;
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
            momentumLineSegment = null;
            centerlLineSegment = null;
            closeValues.Clear();
            GeneratePoints(new string[] { Close }, closeValues);
            this.UpdateArea();
        }

        /// <summary>
        /// Called when binding path changed.
        /// </summary>
        /// <param name="args">Event args.</param>
        protected override void OnBindingPathChanged(DependencyPropertyChangedEventArgs args)
        {
            closeValues.Clear();
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
                this.closeValues = Clone(series.ActualSeriesYValues[0]);
                this.Area.ScheduleUpdate();
            }
        }

        /// <summary>
        /// Method implementation for GeneratePoints for TechnicalIndicator.
        /// </summary>
        protected internal override void GeneratePoints()
        {
            GeneratePoints(new string[] { Close }, closeValues);
        }

        /// <summary>
        /// Creates the segments of <see cref="MomentumTechnicalIndicator"/>.
        /// </summary>
        public override void CreateSegments()
        {
            xValues = GetXValues();
			if (Period < 1) return;
            if ((int)Period < DataCount)
            {
                ComputeMomentum(Period);
                centerXPoints.Clear();
                centerYPoints.Clear();
                for (int i = 0; i < DataCount; i++)
                {
                        centerXPoints.Add(xValues[i]);
                        centerYPoints.Add(100);
                }
                if (momentumLineSegment == null || centerlLineSegment == null)
                {
                    Segments.Clear();
                    momentumLineSegment = new TechnicalIndicatorSegment(momentumXPoints, momentumYPoints, MomentumLineColor, this);
                    momentumLineSegment.SetValues(momentumXPoints, momentumYPoints, MomentumLineColor, this);
                    Segments.Add(momentumLineSegment);
                    centerlLineSegment = new TechnicalIndicatorSegment(centerXPoints, centerYPoints, CenterLineColor, this);
                    centerlLineSegment.SetValues(centerXPoints, centerYPoints, CenterLineColor, this);
                    Segments.Add(centerlLineSegment);
                }
                else
                {
                    momentumLineSegment.SetData(momentumXPoints, momentumYPoints, MomentumLineColor);
                    momentumLineSegment.SetRange();
                    centerlLineSegment.SetData(centerXPoints, centerYPoints, CenterLineColor);
                    centerlLineSegment.SetRange();
                }
            }
            if (momentumLineSegment != null && centerlLineSegment != null && DataCount > 0)
            {
                momentumLineSegment.SetRange();
                centerlLineSegment.SetRange();
            }
        }

        /// <summary>
        /// Updates the segment at the specified index.
        /// </summary>
        /// <param name="index">The index of the segment.</param>
        /// <param name="action">The action that caused the segments collection changed event.</param>
        public override void UpdateSegments(int index, NotifyCollectionChangedAction action)
        {
            base.UpdateSegments(index, action);
            this.Area.ScheduleUpdate();
        }

        private void ComputeMomentum(int len)
        {
            momentumXPoints.Clear();
            momentumYPoints.Clear();
            for (int i = 0; i < DataCount; ++i)
            {
                if (!(i < len))
                {
                    momentumXPoints.Add(xValues[i]);
                    momentumYPoints.Add(closeValues[i] / closeValues[i - len] * 100);
                }
            }
        }

        /// <inheritdoc/>
        protected override ChartSegment CreateSegment()
        {
            return null;
        }
#endregion

    }
}
