using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Layouts;
using Syncfusion.Maui.Core.Internals;
using Syncfusion.Maui.Graphics.Internals;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;

namespace Syncfusion.Maui.Gauges
{
    /// <summary>
    /// The linear gauge is a data visualization control that can be used to display data on a linear scale in either horizontal or vertical orientation.
    /// </summary>
    public class SfLinearGauge : View, IContentView
    {
        #region Bindable properties

        /// <summary>
        /// Identifies the <see cref="Minimum"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="Minimum"/> bindable property.
        /// </value>
        public static readonly BindableProperty MinimumProperty =
            BindableProperty.Create(nameof(Minimum), typeof(double), typeof(SfLinearGauge), 0d, propertyChanged: OnMinMaxPropertyChanged);

        /// <summary>
        /// Identifies the <see cref="Maximum"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="Maximum"/> bindable property.
        /// </value>
        public static readonly BindableProperty MaximumProperty =
            BindableProperty.Create(nameof(Maximum), typeof(double), typeof(SfLinearGauge), 100d, propertyChanged: OnMinMaxPropertyChanged);

        /// <summary>
        /// Identifies the <see cref="Interval"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="Interval"/> bindable property.
        /// </value>
        public static readonly BindableProperty IntervalProperty =
            BindableProperty.Create(nameof(Interval), typeof(double), typeof(SfLinearGauge), double.NaN, propertyChanged: OnAxisPropertyChanged);

        /// <summary>
        /// Identifies the <see cref="MinorTicksPerInterval"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="MinorTicksPerInterval"/> bindable property.
        /// </value>
        public static readonly BindableProperty MinorTicksPerIntervalProperty =
            BindableProperty.Create(nameof(MinorTicksPerInterval), typeof(int), typeof(SfLinearGauge), 1, propertyChanged: OnAxisPropertyChanged);

        /// <summary>
        /// Identifies the <see cref="MaximumLabelsCount"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="MaximumLabelsCount"/> bindable property.
        /// </value>
        public static readonly BindableProperty MaximumLabelsCountProperty =
            BindableProperty.Create(nameof(MaximumLabelsCount), typeof(int), typeof(SfLinearGauge), 3, propertyChanged: OnAxisPropertyChanged);

        /// <summary>
        /// Identifies the <see cref="LabelFormat"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="LabelFormat"/> bindable property.
        /// </value>
        public static readonly BindableProperty LabelFormatProperty =
            BindableProperty.Create(nameof(LabelFormat), typeof(string), typeof(SfLinearGauge), null, propertyChanged: OnLabelFormatPropertyChanged);

        ///// <summary>
        ///// Identifies the <see cref="LabelTemplate"/> bindable property.
        ///// </summary>
        ///// <value>
        ///// The identifier for <see cref="LabelTemplate"/> bindable property.
        ///// </value>
        //public static readonly BindableProperty LabelTemplateProperty =
        //    BindableProperty.Create(nameof(LabelTemplate), typeof(string), typeof(SfLinearGauge), null);

        /// <summary>
        /// Identifies the <see cref="LabelPosition"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="LabelPosition"/> bindable property.
        /// </value>
        public static readonly BindableProperty LabelPositionProperty =
           BindableProperty.Create(nameof(LabelPosition), typeof(GaugeLabelsPosition), typeof(SfLinearGauge),
           GaugeLabelsPosition.Inside, propertyChanged: OnPropertyChanged);

        /// <summary>
        /// Identifies the <see cref="LabelOffset"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="LabelOffset"/> bindable property.
        /// </value>
        public static readonly BindableProperty LabelOffsetProperty =
           BindableProperty.Create(nameof(LabelOffset), typeof(double), typeof(SfLinearGauge), double.NaN, propertyChanged: OnLabelOffsetPropertyChanged);

        /// <summary>
        /// Identifies the <see cref="TickPosition"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="TickPosition"/> bindable property.
        /// </value>
        public static readonly BindableProperty TickPositionProperty =
            BindableProperty.Create(nameof(TickPosition), typeof(GaugeElementPosition), typeof(SfLinearGauge),
             GaugeElementPosition.Inside, propertyChanged: OnPropertyChanged);

        /// <summary>
        /// Identifies the <see cref="TickOffset"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="TickOffset"/> bindable property.
        /// </value>
        public static readonly BindableProperty TickOffsetProperty =
            BindableProperty.Create(nameof(TickOffset), typeof(double), typeof(SfLinearGauge), double.NaN, propertyChanged: OnTickOffsetPropertyChanged);

        /// <summary>
        /// Identifies the <see cref="AxisLineStyle"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="AxisLineStyle"/> bindable property.
        /// </value>
        public static readonly BindableProperty AxisLineStyleProperty =
            BindableProperty.Create(nameof(AxisLineStyle), typeof(RadialLineStyle), typeof(SfLinearGauge), null, propertyChanged: OnStylePropertyChanged);

        /// <summary>
        /// Identifies the <see cref="AxisLabelStyle"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="AxisLabelStyle"/> bindable property.
        /// </value>
        public static readonly BindableProperty AxisLabelStyleProperty =
            BindableProperty.Create(nameof(AxisLabelStyle), typeof(GaugeLabelStyle), typeof(SfLinearGauge), null, propertyChanged: OnStylePropertyChanged);

        /// <summary>
        /// Identifies the <see cref="MajorTickStyle"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="MajorTickStyle"/> bindable property.
        /// </value>
        public static readonly BindableProperty MajorTickStyleProperty =
            BindableProperty.Create(nameof(MajorTickStyle), typeof(RadialTickStyle), typeof(SfLinearGauge), null, propertyChanged: OnMajorTickStylePropertyChanged);

        /// <summary>
        /// Identifies the <see cref="MinorTickStyle"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="MinorTickStyle"/> bindable property.
        /// </value>
        public static readonly BindableProperty MinorTickStyleProperty =
            BindableProperty.Create(nameof(MinorTickStyle), typeof(RadialTickStyle), typeof(SfLinearGauge), null, propertyChanged: OnMinorTickStylePropertyChanged);

        /// <summary>
        /// Identifies the <see cref="IsMirrored"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="IsMirrored"/> bindable property.
        /// </value>
        public static readonly BindableProperty IsMirroredProperty =
            BindableProperty.Create(nameof(IsMirrored), typeof(bool), typeof(SfLinearGauge), false, propertyChanged: OnPropertyChanged);

        /// <summary>
        /// Identifies the <see cref="IsInversed"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="IsInversed"/> bindable property.
        /// </value>
        public static readonly BindableProperty IsInversedProperty =
            BindableProperty.Create(nameof(IsInversed), typeof(bool), typeof(SfLinearGauge), false, propertyChanged: OnPropertyChanged);


        /// <summary>
        /// Identifies the <see cref="ShowTicks"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="ShowTicks"/> bindable property.
        /// </value>
        public static readonly BindableProperty ShowTicksProperty =
            BindableProperty.Create(nameof(ShowTicks), typeof(bool), typeof(SfLinearGauge), true, propertyChanged: OnPropertyChanged);

        /// <summary>
        /// Identifies the <see cref="ShowAxisLine"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="ShowAxisLine"/> bindable property.
        /// </value>
        public static readonly BindableProperty ShowAxisLineProperty =
            BindableProperty.Create(nameof(ShowAxisLine), typeof(bool), typeof(SfLinearGauge), true, propertyChanged: OnPropertyChanged);

        /// <summary>
        /// Identifies the <see cref="ShowLabels"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="ShowLabels"/> bindable property.
        /// </value>
        public static readonly BindableProperty ShowLabelsProperty =
            BindableProperty.Create(nameof(ShowLabels), typeof(bool), typeof(SfLinearGauge), true, propertyChanged: OnPropertyChanged);

        /// <summary>
        /// Identifies the <see cref="UseRangeColorForAxis"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="UseRangeColorForAxis"/> bindable property.
        /// </value>
        public static readonly BindableProperty UseRangeColorForAxisProperty =
            BindableProperty.Create(nameof(UseRangeColorForAxis), typeof(bool), typeof(SfLinearGauge), false, propertyChanged: OnInvalidatePropertyChanged);

        /// <summary>
        /// Identifies the <see cref="Orientation"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="Orientation"/> bindable property.
        /// </value>
        public static readonly BindableProperty OrientationProperty =
            BindableProperty.Create(nameof(Orientation), typeof(ItemsLayoutOrientation), typeof(SfLinearGauge), ItemsLayoutOrientation.Horizontal, propertyChanged: OnInvalidatePropertyChanged);


        #endregion

        #region Fields

        private Grid parentGrid;
        private LinearScaleView linearScaleView;
        private PathF? axisLinePath;

        /// <summary>
        /// Gets or sets the available size of <see cref="SfLinearGauge"/>.
        /// </summary>
        internal Size ScaleAvailableSize;

        /// <summary>
        /// Gets or sets the visible labels in Axis.
        /// </summary>
        internal List<GaugeLabelInfo> VisibleLabels;

        /// <summary>
        /// Gets or sets minor tick points. 
        /// </summary>
        internal List<AxisTickInfo> MinorTickPoints;

        /// <summary>
        /// Gets or sets the actual minimum value of the axis.
        /// </summary>
        internal double ActualMinimum;

        /// <summary>
        /// Gets or sets the actual maximum value of the axis.
        /// </summary>
        internal double ActualMaximum;

        /// <summary>
        /// Gets or sets the actual interval of the Axis.
        /// </summary>
        internal double ActualInterval;

        /// <summary>
        /// Gets or sets the maximum label size.
        /// </summary>
        internal Size LabelMaximumSize;

        /// <summary>
        /// Gets or sets the axis line start position.
        /// </summary>
        internal Point AxisLinePosition;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="SfLinearGauge"/> class.
        /// </summary>
        public SfLinearGauge()
        {
            this.linearScaleView = new LinearScaleView(this);
            this.parentGrid = new Grid();
            this.parentGrid.Children.Add(this.linearScaleView);
            this.AxisLineStyle=new RadialLineStyle();
            this.MajorTickStyle=new RadialTickStyle();
            this.MinorTickStyle=new RadialTickStyle();
            this.ActualInterval = this.Interval;
            this.ActualMaximum = this.Minimum;
            this.ActualMaximum = this.Maximum;
            this.MinorTickPoints = new List<AxisTickInfo>();
        }

        #endregion

        #region Properties

        public double Minimum
        {
            get { return (double)this.GetValue(MinimumProperty); }
            set { this.SetValue(MinimumProperty, value); }
        }

        public double Maximum
        {
            get { return (double)this.GetValue(MaximumProperty); }
            set { this.SetValue(MaximumProperty, value); }
        }

        public double Interval
        {
            get { return (double)this.GetValue(IntervalProperty); }
            set { this.SetValue(IntervalProperty, value); }
        }

        public int MinorTicksPerInterval
        {
            get { return (int)this.GetValue(MinorTicksPerIntervalProperty); }
            set { this.SetValue(MinorTicksPerIntervalProperty, value); }
        }

        public int MaximumLabelsCount
        {
            get { return (int)this.GetValue(MaximumLabelsCountProperty); }
            set { this.SetValue(MaximumLabelsCountProperty, value); }
        }

        public string LabelFormat
        {
            get { return (string)this.GetValue(LabelFormatProperty); }
            set { this.SetValue(LabelFormatProperty, value); }
        }

        //public DataTemplate LabelTemplate
        //{
        //    get { return (DataTemplate)this.GetValue(LabelTemplateProperty); }
        //    set { this.SetValue(LabelTemplateProperty, value); }
        //}

        public GaugeLabelsPosition LabelPosition
        {
            get { return (GaugeLabelsPosition)this.GetValue(LabelPositionProperty); }
            set { this.SetValue(LabelPositionProperty, value); }
        }

        public double LabelOffset
        {
            get { return (double)this.GetValue(LabelOffsetProperty); }
            set { this.SetValue(LabelOffsetProperty, value); }
        }

        public GaugeElementPosition TickPosition
        {
            get { return (GaugeElementPosition)this.GetValue(TickPositionProperty); }
            set { this.SetValue(TickPositionProperty, value); }
        }

        public double TickOffset
        {
            get { return (double)this.GetValue(TickOffsetProperty); }
            set { this.SetValue(TickOffsetProperty, value); }
        }

        public RadialLineStyle AxisLineStyle
        {
            get { return (RadialLineStyle)this.GetValue(AxisLineStyleProperty); }
            set { this.SetValue(AxisLineStyleProperty, value); }
        }

        public GaugeLabelStyle AxisLabelStyle
        {
            get { return (GaugeLabelStyle)this.GetValue(AxisLabelStyleProperty); }
            set { this.SetValue(AxisLabelStyleProperty, value); }
        }

        public RadialTickStyle MajorTickStyle
        {
            get { return (RadialTickStyle)this.GetValue(MajorTickStyleProperty); }
            set { this.SetValue(MajorTickStyleProperty, value); }
        }

        public RadialTickStyle MinorTickStyle
        {
            get { return (RadialTickStyle)this.GetValue(MinorTickStyleProperty); }
            set { this.SetValue(MinorTickStyleProperty, value); }
        }

        public bool IsMirrored
        {
            get { return (bool)this.GetValue(IsMirroredProperty); }
            set { this.SetValue(IsMirroredProperty, value); }
        }

        public bool IsInversed
        {
            get { return (bool)this.GetValue(IsInversedProperty); }
            set { this.SetValue(IsInversedProperty, value); }
        }

        public bool ShowTicks
        {
            get { return (bool)this.GetValue(ShowTicksProperty); }
            set { this.SetValue(ShowTicksProperty, value); }
        }

        public bool ShowAxisLine
        {
            get { return (bool)this.GetValue(ShowAxisLineProperty); }
            set { this.SetValue(ShowAxisLineProperty, value); }
        }

        public bool ShowLabels
        {
            get { return (bool)this.GetValue(ShowLabelsProperty); }
            set { this.SetValue(ShowLabelsProperty, value); }
        }

        public bool UseRangeColorForAxis
        {
            get { return (bool)this.GetValue(UseRangeColorForAxisProperty); }
            set { this.SetValue(UseRangeColorForAxisProperty, value); }
        }

        public ItemsLayoutOrientation Orientation
        {
            get { return (ItemsLayoutOrientation)this.GetValue(OrientationProperty); }
            set { this.SetValue(OrientationProperty, value); }
        }

        /// <summary>
        /// Gets the root content.
        /// </summary>
        object IContentView.Content
        {
            get
            {
                return parentGrid;
            }
        }

        /// <summary>
        /// Gets the root content.
        /// </summary>
        IView? IContentView.PresentedContent
        {
            get
            {
                return parentGrid;
            }
        }

        /// <summary>
        /// Gets the content padding.
        /// </summary>
        Thickness IPadding.Padding
        {
            get
            {
                return new Thickness(0);
            }
        }

        #endregion

        #region Override methods

        protected override Size MeasureOverride(double widthConstraint, double heightConstraint)
        {
            base.MeasureOverride(widthConstraint, heightConstraint);

            if (this.Orientation == ItemsLayoutOrientation.Horizontal && double.IsInfinity(widthConstraint))
            {
                widthConstraint = 350d;
            }
            else if (this.Orientation == ItemsLayoutOrientation.Vertical && double.IsInfinity(heightConstraint))
            {
                heightConstraint = 350d;
            }

            this.ScaleAvailableSize = new Size(widthConstraint, heightConstraint);

            if ((this.Orientation == ItemsLayoutOrientation.Horizontal && double.IsNaN(this.HeightRequest))
                || (this.Orientation == ItemsLayoutOrientation.Vertical && double.IsNaN(this.WidthRequest)))
            {
                //this.ScaleAvailableSize = this.GetAxisSize();
            }

            this.UpdateAxis();
            
            return ScaleAvailableSize;
        }

        #endregion

        #region Public virtual methods


        public virtual List<GaugeLabelInfo> GenerateVisibleLabels()
        {
            this.MinorTickPoints.Clear();
            List<GaugeLabelInfo> visibleLabels = new List<GaugeLabelInfo>();
            this.ActualInterval = this.GetNiceInterval();
            if (this.ActualInterval != 0 && this.ActualMinimum != this.ActualMaximum)
            {
                for (double i = this.ActualMinimum; i <= this.ActualMaximum; i += this.ActualInterval)
                {
                    GaugeLabelInfo currentLabel = this.GetAxisLabel(i);
                    visibleLabels.Add(currentLabel);
                    if (this.MinorTicksPerInterval > 0)
                    {
                        this.AddMinorTicksPoint(i);
                    }
                }

                GaugeLabelInfo label = visibleLabels[visibleLabels.Count - 1];
                if (label.Value != this.ActualMaximum && label.Value < this.ActualMaximum)
                {
                    GaugeLabelInfo currentLabel = this.GetAxisLabel(this.ActualMaximum);
                    visibleLabels.Add(currentLabel);
                }
            }

            return visibleLabels;
        }

        #endregion

        #region Internal methods

        internal void Draw(ICanvas canvas)
        {
            //this.AxisPanel.ArrangeAxisLine();
            //    this.AxisPanel.ArrangeTicks();
            //    this.AxisPanel.ArrangeLabels();

            canvas.FillColor = Colors.LightGray;
            canvas.FillPath(axisLinePath);
        }


        #endregion

        #region Property changed

        /// <summary>
        /// Called when <see cref="Minimum"/> or <see cref="Maximum"/> property changed.
        /// </summary>
        /// <param name="bindable">The BindableObject.</param>
        /// <param name="oldValue">Old value.</param>
        /// <param name="newValue">New value.</param>
        private static void OnMinMaxPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is SfLinearGauge linearGauge)
            {
               
            }
        }

        /// <summary>
        /// Called when <see cref="MinorTicksPerInterval"/>, <see cref="Interval"/> and <see cref="MaximumLabelsCount"/> property changed.
        /// </summary>
        /// <param name="bindable">The BindableObject.</param>
        /// <param name="oldValue">Old value.</param>
        /// <param name="newValue">New value.</param>
        private static void OnAxisPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is SfLinearGauge sfLinearGauge)
            {
               
            }
        }

        /// <summary>
        /// Called when axis properties changed.
        /// </summary>
        /// <param name="bindable">The BindableObject.</param>
        /// <param name="oldValue">Old value.</param>
        /// <param name="newValue">New value.</param>
        private static void OnPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            
        }

        /// <summary>
        /// Called when <see cref="LabelFormat"/> property changed.
        /// </summary>
        /// <param name="bindable">The BindableObject.</param>
        /// <param name="oldValue">Old value.</param>
        /// <param name="newValue">New value.</param>
        private static void OnLabelFormatPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            
        }

        /// <summary>
        /// Called when <see cref="LabelOffset"/> property changed.
        /// </summary>
        /// <param name="bindable">The BindableObject.</param>
        /// <param name="oldValue">Old value.</param>
        /// <param name="newValue">New value.</param>
        private static void OnLabelOffsetPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {

        }

        /// <summary>
        /// Called when <see cref="TickOffset"/> property changed.
        /// </summary>
        /// <param name="bindable">The BindableObject.</param>
        /// <param name="oldValue">Old value.</param>
        /// <param name="newValue">New value.</param>
        private static void OnTickOffsetPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {

        }

        /// <summary>
        /// Called when <see cref="AxisLabelStyle"/> or property changed.
        /// </summary>
        /// <param name="bindable">The BindableObject.</param>
        /// <param name="oldValue">Old value.</param>
        /// <param name="newValue">New value.</param>
        private static void OnStylePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {

        }

        /// <summary>
        /// Called when <see cref="MinorTickStyle"/> property changed.
        /// </summary>
        /// <param name="bindable">The BindableObject.</param>
        /// <param name="oldValue">Old value.</param>
        /// <param name="newValue">New value.</param>
        private static void OnMinorTickStylePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {

        }

        /// <summary>
        /// Called when <see cref="MajorTickStyle"/> property changed.
        /// </summary>
        /// <param name="bindable">The BindableObject.</param>
        /// <param name="oldValue">Old value.</param>
        /// <param name="newValue">New value.</param>
        private static void OnMajorTickStylePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {

        }

        /// <summary>
        /// Called when axis drawing related properties changed.
        /// </summary>
        /// <param name="bindable">The BindableObject.</param>
        /// <param name="oldValue">Old value.</param>
        /// <param name="newValue">New value.</param>
        private static void OnInvalidatePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
           
        }
        #endregion

        #region Private methods

        /// <summary>
        /// To update the axis elements
        /// </summary>
        private void UpdateAxis()
        {
            if (!this.ScaleAvailableSize.IsZero)
            {
                this.UpdateAxisElements();
                //this.CreateRanges();
                //this.CreateBarPointers();
                //this.CreateMarkerPointers();
            }
        }

        /// <summary>
        /// To update axis line, ticks and labels axis elements.
        /// </summary>
        private void UpdateAxisElements()
        {
            if (!this.ScaleAvailableSize.IsZero)
            {
                this.VisibleLabels = this.GenerateVisibleLabels();
                this.MeasureLabels();
                this.CalculateAxisLineLength();
            }
        }

        /// <summary>
        /// Calculate the interval based on <see cref="Minimum"/> and <see cref="Maximum"/>.
        /// </summary>
        /// <returns>Auto interval based on <see cref="Minimum"/> and <see cref="Maximum"/>.</returns>
        private double GetNiceInterval()
        {
            if (double.IsNaN(this.Interval) || this.Interval == 0)
            {
                return this.CalculateAxisInterval();
            }

            return this.Interval;
        }

        /// <summary>
        /// To calculate the axis interval based on the maximum axis label count.
        /// </summary>
        /// <returns>Returns the interval based on the maximum number of labels for 100 labels</returns>
        private double CalculateAxisInterval()
        {
            double delta = Math.Abs(this.ActualMaximum - this.ActualMinimum);
            double area = this.Orientation == ItemsLayoutOrientation.Horizontal ? this.ScaleAvailableSize.Width : 
                this.ScaleAvailableSize.Height;
            double actualDesiredIntervalsCount = Math.Max((area * this.MaximumLabelsCount) / 100, 1.0);
            double niceInterval = delta / actualDesiredIntervalsCount;
            double minInterval = Math.Pow(10, Math.Floor(Math.Log10(niceInterval)));
            List<double> intervalDivisions = new List<double>() { 10, 5, 2, 1 };
            
            foreach (int intervalDivision in intervalDivisions)
            {
                double currentInterval = minInterval * intervalDivision;
                if (actualDesiredIntervalsCount < (delta / currentInterval))
                {
                    break;
                }

                niceInterval = currentInterval;
            }

            return niceInterval;
        }

        /// <summary>
        /// Gets the current axis labels
        /// </summary>
        /// <param name="value">The value of the label.</param>
        /// <returns>The corresponding axis label of given value.</returns>
        private GaugeLabelInfo GetAxisLabel(double value)
        {
            GaugeLabelInfo label = new GaugeLabelInfo
            {
                Value = value
            };
            string labelText = value.ToString(this.LabelFormat, CultureInfo.CurrentCulture);
            label.Text = labelText;
            return label;
        }

        /// <summary>
        /// To add minor tick points
        /// </summary>
        /// <param name="position">The position</param>
        private void AddMinorTicksPoint(double position)
        {
            var tickInterval = this.ActualInterval / (this.MinorTicksPerInterval + 1);
            var tickPosition = position + tickInterval;
            position += this.ActualInterval;
            while (tickPosition < position && tickPosition <= this.ActualMaximum)
            {
                if (this.ActualMinimum <= tickPosition && tickPosition <= this.ActualMaximum)
                {
                    this.MinorTickPoints.Add(new AxisTickInfo() { Value = tickPosition });
                }

                tickPosition += tickInterval;

                // While adding two decimal digits, some minute difference get added.
                // For example while adding 0.2 + 0.4 results 0.60000000000000009
                // Due to this, additional minor ticks get added and overlapped with the major ticks. In order to avoid this rounded the result.
                tickPosition = Math.Round(tickPosition, 8);
            }
        }

        /// <summary>
        /// To measure the axis labels
        /// </summary>
        private void MeasureLabels()
        {
            if (this.VisibleLabels != null)
            {
                double maximumWidth = 0, maximumHeight = 0;

                foreach (var labelInfo in this.VisibleLabels)
                {
                    if (labelInfo.LabelStyle == null && this.AxisLabelStyle != null)
                    {
                        labelInfo.LabelStyle = this.AxisLabelStyle;
                    }

                    if (labelInfo.LabelStyle != null)
                    {
                        labelInfo.DesiredSize = labelInfo.Text.Measure(labelInfo.LabelStyle);
                    }

                    maximumHeight = Math.Max(maximumHeight, labelInfo.DesiredSize.Height);
                    maximumWidth = Math.Max(maximumWidth, labelInfo.DesiredSize.Width);
                    this.LabelMaximumSize = new Size(maximumWidth, maximumHeight);
                }
            }
        }

        /// <summary>
        /// To calculate axis line length.
        /// </summary>
        private void CalculateAxisLineLength()
        {
            var firstLabelSize = VisibleLabels[0].DesiredSize;
            var lastLabelSize = VisibleLabels[VisibleLabels.Count-1].DesiredSize;
            double axisLineLength;
            if (this.Orientation == ItemsLayoutOrientation.Horizontal)
            {
                axisLineLength = this.ShowLabels
                    ? this.ScaleAvailableSize.Width - (firstLabelSize.Width / 2) - (lastLabelSize.Width / 2)
                    : this.ScaleAvailableSize.Width;
            }
            else
            {
                axisLineLength = this.ShowLabels
                    ? this.ScaleAvailableSize.Height - (firstLabelSize.Height / 2) - (lastLabelSize.Height / 2)
                    : this.ScaleAvailableSize.Height;
            }

            this.CalculateAxisLinePosition();
            double actualAxisLineThickness = this.GetActualAxisLineThickness();
            double halfAxisThickness = actualAxisLineThickness / 2;
            double axisLineStartPosition = this.AxisLinePosition.X;
            double axisLineEndPosition = this.AxisLinePosition.X + axisLineLength;
            float x,y,width, height;
            if (this.Orientation == ItemsLayoutOrientation.Horizontal)
            {
                x = (float)axisLineStartPosition;
                y = (float)(this.AxisLinePosition.Y + (actualAxisLineThickness / 2));
                width = (float)Math.Abs( axisLineEndPosition - axisLineStartPosition);
                height= (float)Math.Abs(this.AxisLinePosition.Y + (actualAxisLineThickness / 2) -
                    this.AxisLinePosition.Y + (actualAxisLineThickness / 2));
            }
            else
            {
                x = (float)(this.AxisLinePosition.Y + (actualAxisLineThickness / 2));
                y = (float)axisLineStartPosition;
                width = (float)Math.Abs(this.AxisLinePosition.Y + (actualAxisLineThickness / 2) -
                   this.AxisLinePosition.Y + (actualAxisLineThickness / 2));
                height = (float)Math.Abs(axisLineEndPosition - axisLineStartPosition);
            }

            axisLinePath = new PathF();
            axisLinePath.AppendRectangle(x,y,width,height);
        }

        /// <summary>
        /// To calculate the axis line position.
        /// </summary>
        private void CalculateAxisLinePosition()
        {
            double maximumTickLength = this.GetActualMaxTickLength();
            double actualTickOffset = this.GetActualTickOffset();
            double labelMaximumSize = this.GetLabelMaxLength();
            double firstLabelSize = this.GetFirstLabelLength();
            double actualLabelOffset = this.GetActualLabelOffset();
            double actualAxisLineThickness = this.GetActualAxisLineThickness();
            GaugeLabelsPosition actualLabelsPosition = this.GetActualLabelPosition();
            double outsideRangeHeight = 0d, insideRangeHeight = 0d;
            //this.GetRangeHeights(ref outsideRangeHeight, ref insideRangeHeight);
            double outsideBarPointerHeight = 0d, insideBarPointerHeight = 0d;
            //this.GetBarPointersHeight(ref outsideBarPointerHeight, ref insideBarPointerHeight);
            double outsideMarkerPointerHeight = 0d, insideMarkerPointerHeight = 0d;
            //this.GetMarkerPointersHeight(ref outsideMarkerPointerHeight, ref insideMarkerPointerHeight);
            double outsideAxisHeight = Math.Max(Math.Max(outsideRangeHeight, outsideBarPointerHeight), outsideMarkerPointerHeight);
            double x = firstLabelSize / 2, y = 0d;
            switch (this.GetActualElementPosition(this.TickPosition))
            {
                case GaugeElementPosition.Inside:
                    switch (actualLabelsPosition)
                    {
                        case GaugeLabelsPosition.Inside:
                            this.AxisLinePosition = new Point(x, y > outsideAxisHeight ? y : outsideAxisHeight);
                            break;
                        case GaugeLabelsPosition.Outside:
                            y = labelMaximumSize + actualLabelOffset;
                            this.AxisLinePosition = new Point(x, y > outsideAxisHeight ? y : outsideAxisHeight);
                            break;
                    }

                    break;
                case GaugeElementPosition.Outside:
                    switch (actualLabelsPosition)
                    {
                        case GaugeLabelsPosition.Inside:
                            y = maximumTickLength + actualTickOffset;
                            this.AxisLinePosition = new Point(x, y > outsideAxisHeight ? y : outsideAxisHeight);
                            break;
                        case GaugeLabelsPosition.Outside:
                            y = maximumTickLength + labelMaximumSize + actualTickOffset + actualLabelOffset;
                            this.AxisLinePosition = new Point(x, y > outsideAxisHeight ? y : outsideAxisHeight);
                            break;
                    }

                    break;
                case GaugeElementPosition.Cross:
                    switch (actualLabelsPosition)
                    {
                        case GaugeLabelsPosition.Inside:
                            y = actualAxisLineThickness < maximumTickLength ? (maximumTickLength - actualAxisLineThickness) / 2 : 0d;
                            this.AxisLinePosition = new Point(x, y > outsideAxisHeight ? y : outsideAxisHeight);
                            break;
                        case GaugeLabelsPosition.Outside:
                            y = (actualAxisLineThickness < maximumTickLength ? (maximumTickLength - actualAxisLineThickness) / 2 : 0d) + actualLabelOffset + labelMaximumSize;
                            this.AxisLinePosition = new Point(x, y > outsideAxisHeight ? y : outsideAxisHeight);
                            break;
                    }

                    break;
            }
        }

        /// <summary>
        /// To get actual maximum tick length.
        /// </summary>
        /// <returns>Actual maximum tick length.</returns>
        private double GetActualMaxTickLength()
        {
            if (!this.ShowTicks)
            {
                return 0d;
            }

            var majorTickLength= this.MajorTickStyle!=null? this.MajorTickStyle.Length : 0;
            var minorTickLength = this.MinorTickStyle != null ? this.MinorTickStyle.Length : 0;

            if (this.MinorTicksPerInterval <= 0)
            {
                return majorTickLength;
            }

            return majorTickLength > minorTickLength ? majorTickLength : minorTickLength;
        }

        /// <summary>
        /// To get actual tick offset.
        /// </summary>
        /// <returns>The actual tick offset based on ShowTicks property.</returns>
        private double GetActualTickOffset()
        {
            if (this.ShowTicks)
            {
                return this.TickOffset > 0d ? this.TickOffset : 0d;
            }

            return 0d;
        }

        /// <summary>
        /// To get the actual linear label position based on IsMirrored property.
        /// </summary>
        /// <returns>The actual linear label position value based on IsMirrored property.</returns>
        private GaugeLabelsPosition GetActualLabelPosition()
        {
            if (this.IsMirrored)
            {
                return this.LabelPosition == GaugeLabelsPosition.Outside ? GaugeLabelsPosition.Inside : GaugeLabelsPosition.Outside;
            }

            return this.LabelPosition;
        }

        /// <summary>
        /// To get the maximum label length based on Orientation and ShowLabels  property.
        /// </summary>
        /// <returns>The maximum label length based on Orientation and ShowLabels  property.</returns>
        private double GetLabelMaxLength()
        {
            if (this.ShowLabels)
            {
                return this.Orientation == ItemsLayoutOrientation.Horizontal ? this.LabelMaximumSize.Height : this.LabelMaximumSize.Width;
            }

            return 0d;
        }

        /// <summary>
        /// To get the first label length based on Orientation and ShowLabels  property.
        /// </summary>
        /// <returns>The first label length based on Orientation and ShowLabels  property.</returns>
        private double GetFirstLabelLength()
        {
            if (this.ShowLabels && this.VisibleLabels.Count > 0)
            {
                var size = this.VisibleLabels[0].DesiredSize;
                return this.Orientation == ItemsLayoutOrientation.Horizontal ? size.Width : size.Height;
            }

            return 0d;
        }

        /// <summary>
        /// To get the actual label offset based on ShowAxisLine property.
        /// </summary>
        /// <returns>The actual label offset based on ShowAxisLine property.</returns>
        private double GetActualAxisLineThickness()
        {
            if (this.ShowAxisLine && this.AxisLineStyle != null)
            {
                return Math.Abs(this.AxisLineStyle.Thickness);
            }

            return 0d;
        }

        /// <summary>
        /// To get the actual label offset based on Orientation and ShowLabels property.
        /// </summary>
        /// <returns>The actual label offset based on Orientation and ShowLabels property.</returns>
        private double GetActualLabelOffset()
        {
            if (this.ShowLabels)
            {
                return this.LabelOffset > 0d ? this.LabelOffset : 0d;
            }

            return 0d;
        }

        /// <summary>
        /// To get the actual element position based on IsMirrored property.
        /// </summary>
        /// <param name="gaugeElementPosition">The current linear gauge element position.</param>
        /// <returns>The actual element position value based on IsMirrored property.</returns>
        private GaugeElementPosition GetActualElementPosition(GaugeElementPosition gaugeElementPosition)
        {
            if (this.IsMirrored)
            {
                switch (gaugeElementPosition)
                {
                    case GaugeElementPosition.Outside:
                        return GaugeElementPosition.Inside;
                    case GaugeElementPosition.Inside:
                        return GaugeElementPosition.Outside;
                }
            }

            return gaugeElementPosition;
        }


        private void InvalidateDrawable()
        {
            this.linearScaleView.InvalidateDrawable();
        }

        /// <summary>
        /// Measure the content.
        /// </summary>
        /// <param name="widthConstraint"></param>
        /// <param name="heightConstraint"></param>
        /// <returns></returns>
        Size IContentView.CrossPlatformMeasure(double widthConstraint, double heightConstraint)
        {
            return this.MeasureContent(widthConstraint, heightConstraint);
        }

        /// <summary>
        /// Arrange the content.
        /// </summary>
        /// <param name="bounds"></param>
        /// <returns></returns>
        Size IContentView.CrossPlatformArrange(Rectangle bounds)
        {
            this.ArrangeContent(bounds);
            this.InvalidateDrawable();
            return bounds.Size;
        }

        #endregion
    }
}
