using System.Collections.Generic;
using System.Collections.Specialized;
using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;
using System.Collections;

namespace Syncfusion.UI.Xaml.Charts
{
    /// <summary>
    /// Represents the AccumulationDistribution technical indicator.
    /// </summary>
    /// <seealso cref="FinancialTechnicalIndicator"/>
    /// <seealso cref="TechnicalIndicatorSegment"/>
    internal class AccumulationDistributionIndicator:FinancialTechnicalIndicator
    {
#region constructor 

        /// <summary>
        /// Called when instance created for <see cref="AccumulationDistributionIndicator"/>.
        /// </summary>
        public AccumulationDistributionIndicator()
        {
        }

#endregion

#region fields

        IList<double> _closeValues = new List<double>();

        IList<double> _highValues = new List<double>();

        IList<double> _lowValues = new List<double>();

        IList<double> _volumeValues = new List<double>();

        List<double> _xValues;

        readonly List<double> _xPoints = new List<double>();

        readonly List<double> _yPoints = new List<double>();

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
        /// Gets or sets the fill color for the Signal Line.
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
            DependencyProperty.Register("SignalLineColor", typeof(Brush), typeof(AccumulationDistributionIndicator),
                new PropertyMetadata(new SolidColorBrush(Colors.Green),OnColorChanged));

#endregion

#region Methods

        private static void OnColorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            AccumulationDistributionIndicator indicator = d as AccumulationDistributionIndicator;
            if (indicator!=null) indicator.UpdateArea();
        }

        /// <summary>
        ///  Called when ItemsSource property changed. 
        /// </summary>
        /// <param name="oldValue">Specifies the old value.</param>
        /// <param name="newValue">Specifies the new value.</param>
        protected override void OnDataSourceChanged(IEnumerable oldValue, IEnumerable newValue)
        {
            base.OnDataSourceChanged(oldValue, newValue);
            _fastLineSegment = null;
            _closeValues.Clear();
            _highValues.Clear();
            _lowValues.Clear();
            _volumeValues.Clear();
            GeneratePoints(new[] {High,Low, Close,Volume},_highValues,_lowValues,_closeValues,_volumeValues);
            UpdateArea();
        }

        /// <summary>
        /// Called when binding path changed.
        /// </summary>
        /// <param name="args">Event args.</param>
        protected override void OnBindingPathChanged(DependencyPropertyChangedEventArgs args)
        {
            _fastLineSegment = null;
            _closeValues.Clear();
            _highValues.Clear();
            _lowValues.Clear();
            _volumeValues.Clear();
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
                _volumeValues = Clone(series.ActualSeriesYValues[3]);
                Area.ScheduleUpdate();
            }
        }

        /// <summary>
        /// Method implementation for GeneratePoints for TechnicalIndicator.
        /// </summary>
        protected internal override void GeneratePoints()
        {
            GeneratePoints(new[] { High, Low, Close, Volume }, _highValues, _lowValues, _closeValues, _volumeValues);
        }

        /// <summary>
        /// Creates the segments of <see cref="AccumulationDistributionIndicator"/>.
        /// </summary>
        public override void CreateSegments()
        {
            _xValues = GetXValues();
            AddPoints(_xPoints, _yPoints);

            if (_fastLineSegment == null)
            {
                _fastLineSegment = new TechnicalIndicatorSegment(_xPoints, _yPoints, SignalLineColor, this);
                _fastLineSegment.SetValues(_xPoints, _yPoints, SignalLineColor, this);
                Segments.Add(_fastLineSegment);
            }
            else
            {
                _fastLineSegment.SetData(_xPoints, _yPoints,SignalLineColor);
                _fastLineSegment.SetRange();
            }
        }

        private void AddPoints(List<double> xPoints, List<double> yPoints)
        {
            xPoints.Clear();
            yPoints.Clear();
            double sum = 0d;

            for (int i = 0; i < DataCount; ++i)
            {
                double close = _closeValues[i];
                if (close != 0)
                {
                    sum += (_volumeValues[i]) * (((close - _lowValues[i]) - (_highValues[i] - close)) / (_highValues[i] - _lowValues[i]));
                }

                xPoints.Add(_xValues[i]);
                yPoints.Add(sum);
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
