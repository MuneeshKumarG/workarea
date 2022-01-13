using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Layouts;
using Syncfusion.Maui.Graphics.Internals;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;
using GradientStop = Microsoft.Maui.Controls.GradientStop;

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
            BindableProperty.Create(nameof(AxisLineStyle), typeof(LinearLineStyle), typeof(SfLinearGauge), null, propertyChanged: OnStylePropertyChanged);

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
            BindableProperty.Create(nameof(MajorTickStyle), typeof(LinearTickStyle), typeof(SfLinearGauge), null, propertyChanged: OnMajorTickStylePropertyChanged);

        /// <summary>
        /// Identifies the <see cref="MinorTickStyle"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="MinorTickStyle"/> bindable property.
        /// </value>
        public static readonly BindableProperty MinorTickStyleProperty =
            BindableProperty.Create(nameof(MinorTickStyle), typeof(LinearTickStyle), typeof(SfLinearGauge), null, propertyChanged: OnMinorTickStylePropertyChanged);

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
            BindableProperty.Create(nameof(Orientation), typeof(GaugeOrientation), typeof(SfLinearGauge), GaugeOrientation.Horizontal, propertyChanged: OnOrientationPropertyChanged);

        /// <summary>
        /// Identifies the <see cref="Orientation"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="Ranges"/> bindable property.
        /// </value>
        public static readonly BindableProperty RangesProperty =
            BindableProperty.Create(nameof(Ranges), typeof(ObservableCollection<LinearRange>), typeof(SfLinearGauge), null, propertyChanged: OnRangesPropertyChanged);

        #endregion

        #region Fields

        private Grid parentGrid;
        private LinearScaleView linearScaleView;
        private PathF? axisLinePath;
        private double axisLineLength;
        private Point majorTicksPanelPosition, minorTicksPanelPosition, labelsPanelPosition;
        private Size firstLabelSize, lastLabelSize;

        internal Point AxisLinePosition;
        internal Size ScaleAvailableSize, LabelMaximumSize;
        internal List<GaugeLabelInfo>? VisibleLabels;
        internal List<AxisTickInfo> MajorTickPositions, MinorTickPositions;
        internal double ActualMinimum, ActualMaximum, ActualInterval;

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
            this.AxisLineStyle = new LinearLineStyle();
            this.MajorTickStyle = new LinearTickStyle();
            this.MinorTickStyle = new LinearTickStyle();
            this.AxisLabelStyle = new GaugeLabelStyle();
            this.ActualMaximum = this.Minimum;
            this.ActualMaximum = this.Maximum;

            this.MajorTickPositions = new List<AxisTickInfo>();
            this.MinorTickPositions = new List<AxisTickInfo>();
            this.Ranges = new ObservableCollection<LinearRange>();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the minimum value of the axis. The axis starts from this value.
        /// </summary>
        /// <value>
        /// It defines the minimum values of the <see cref="SfLinearGauge"/>. The default value is <c>0</c>.
        /// </value>
        public double Minimum
        {
            get { return (double)this.GetValue(MinimumProperty); }
            set { this.SetValue(MinimumProperty, value); }
        }

        /// <summary>
        /// Gets or sets the maximum value of the axis. The axis ends at this value.
        /// </summary>
        /// <value>
        /// It defines the maximum value of the <see cref="SfLinearGauge"/>. The default value is <c>100</c>.
        /// </value>
        public double Maximum
        {
            get { return (double)this.GetValue(MaximumProperty); }
            set { this.SetValue(MaximumProperty, value); }
        }

        /// <summary>
        /// Gets or sets the interval value of the axis. Using this, the axis labels can be displayed after a certain interval value.
        /// </summary>
        /// <value>
        /// It defines the interval of the <see cref="SfLinearGauge"/>. The default value is <c>double.NaN</c>.
        /// </value>
        public double Interval
        {
            get { return (double)this.GetValue(IntervalProperty); }
            set { this.SetValue(IntervalProperty, value); }
        }

        /// <summary>
        /// Gets or sets the interval of the minor ticks.
        /// </summary>
        /// <value>
        /// It defines number of minor ticks will be rendered between the major ticks. The default value is <c>1</c>.
        /// </value>
        public int MinorTicksPerInterval
        {
            get { return (int)this.GetValue(MinorTicksPerIntervalProperty); }
            set { this.SetValue(MinorTicksPerIntervalProperty, value); }
        }

        /// <summary>
        /// Gets or sets the maximum number of labels to be displayed in an axis in 100 logical pixels.
        /// </summary>
        /// <value>
        /// Maximum number of labels to be displayed in a axis in 100 logical pixels. Its default value is <c>3</c>. 
        /// </value>
        public int MaximumLabelsCount
        {
            get { return (int)this.GetValue(MaximumLabelsCountProperty); }
            set { this.SetValue(MaximumLabelsCountProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value to formats the axis labels with globalized string formats.
        /// </summary>
        /// <value>
        /// The string that specifies the globalized string formats for the axis labels. Its default value is <c>string.Empty</c>. 
        /// </value>
        public string LabelFormat
        {
            get { return (string)this.GetValue(LabelFormatProperty); }
            set { this.SetValue(LabelFormatProperty, value); }
        }

        /// <summary>
        /// Gets or sets the value that indicates the position of the axis labels inside or outside the axis line.
        /// </summary>
        /// <value>
        /// One of the enumeration values that specifies the position of labels in the linear gauge.
        /// The registered default is <see cref="GaugeLabelsPosition.Inside"/>.
        /// </value>
        public GaugeLabelsPosition LabelPosition
        {
            get { return (GaugeLabelsPosition)this.GetValue(LabelPositionProperty); }
            set { this.SetValue(LabelPositionProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value to adjusts the axis label position from tick end. Specify value either in logical pixel value.
        /// </summary>
        /// <value>
        /// It defines the offset of the axis labels. The default value is <c>double.NaN</c>.
        /// </value>
        public double LabelOffset
        {
            get { return (double)this.GetValue(LabelOffsetProperty); }
            set { this.SetValue(LabelOffsetProperty, value); }
        }

        /// <summary>
        /// Gets or sets the value that indicates the position of the ticks inside, center, or outside the axis line.
        /// </summary>
        /// <value>
        /// One of the enumeration values that specifies the position of ticks in the linear gauge.
        /// The default is <see cref="GaugeElementPosition.Inside"/>.
        /// </value>
        public GaugeElementPosition TickPosition
        {
            get { return (GaugeElementPosition)this.GetValue(TickPositionProperty); }
            set { this.SetValue(TickPositionProperty, value); }
        }

        /// <summary>
        /// Gets or sets the value to adjusts the axis ticks position from the axis lines. Specify value either in logical pixel value.
        /// </summary>
        /// <value>
        /// It defines the offset of the ticks. The default value is <c>double.NaN</c>.
        /// </value>
        public double TickOffset
        {
            get { return (double)this.GetValue(TickOffsetProperty); }
            set { this.SetValue(TickOffsetProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value to customize the axis line.
        /// </summary>
        public LinearLineStyle AxisLineStyle
        {
            get { return (LinearLineStyle)this.GetValue(AxisLineStyleProperty); }
            set { this.SetValue(AxisLineStyleProperty, value); }
        }

        /// <summary>
        /// Gets or sets <see cref="GaugeLabelStyle"/>, that used to customize gauge axis labels.
        /// </summary>
        public GaugeLabelStyle AxisLabelStyle
        {
            get { return (GaugeLabelStyle)this.GetValue(AxisLabelStyleProperty); }
            set { this.SetValue(AxisLabelStyleProperty, value); }
        }

        /// <summary>
        /// Gets or sets <see cref="LinearTickStyle"/>, that used to customize major ticks.
        /// </summary>
        public LinearTickStyle MajorTickStyle
        {
            get { return (LinearTickStyle)this.GetValue(MajorTickStyleProperty); }
            set { this.SetValue(MajorTickStyleProperty, value); }
        }

        /// <summary>
        /// Gets or sets <see cref="LinearTickStyle"/>, that used to customize minor ticks.
        /// </summary>
        public LinearTickStyle MinorTickStyle
        {
            get { return (LinearTickStyle)this.GetValue(MinorTickStyleProperty); }
            set { this.SetValue(MinorTickStyleProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether inverts the axis rendered in opposed view.
        /// </summary>
        /// <value>
        /// <b>true</b> if axis is mirrored; otherwise, <b>false</b>.The default value is <b>false</b>.
        /// </value>
        /// <remarks>
        /// <c>IsInversed</c> decides whether the axis will be inversed or not.
        /// If <see cref="IsMirrored"/> is <c>true</c>, the axis will be mirrored, otherwise not mirrored.
        /// </remarks>
        public bool IsMirrored
        {
            get { return (bool)this.GetValue(IsMirroredProperty); }
            set { this.SetValue(IsMirroredProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether inverts the axis direction.
        /// </summary>
        /// <value>
        /// <b>true</b> if axis is inversed; otherwise, <b>false</b>.The default value is <b>false</b>.
        /// </value>
        /// <remarks>
        /// <c>IsInversed</c> decides whether the axis will be inversed or not.
        /// If <see cref="IsInversed"/> is <c>true</c>, the axis will be inversed, otherwise not inversed.
        /// </remarks>
        public bool IsInversed
        {
            get { return (bool)this.GetValue(IsInversedProperty); }
            set { this.SetValue(IsInversedProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to shows or hides the axis tick lines.
        /// </summary>
        /// <value>
        /// <b>true</b> if axis line ticks are displayed; otherwise, <b>false</b>.The default value is <b>true</b>.
        /// </value>
        /// <remarks>
        /// It decides whether the axis ticks will be rendered or not.
        /// If <see cref="ShowTicks"/> is <c>true</c>, the axis ticks will be rendered, otherwise not rendered.
        /// </remarks>
        public bool ShowTicks
        {
            get { return (bool)this.GetValue(ShowTicksProperty); }
            set { this.SetValue(ShowTicksProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to shows or hides the axis line.
        /// </summary>
        /// <value>
        /// <b>true</b> if axis line is displayed; otherwise, <b>false</b>.The default value is <b>true</b>.
        /// </value>
        /// <remarks>
        /// It decides whether the axis line will be rendered or not.
        /// If <see cref="ShowAxisLine"/> is <c>true</c>, the axis line will be rendered, otherwise not rendered.
        /// </remarks>
        public bool ShowAxisLine
        {
            get { return (bool)this.GetValue(ShowAxisLineProperty); }
            set { this.SetValue(ShowAxisLineProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to shows or hides the axis labels.
        /// </summary>
        /// <value>
        /// <b>true</b> if axis labels are displayed; otherwise, <b>false</b>.The default value is <b>true</b>.
        /// </value>
        /// <remarks>
        /// It decides whether the axis labels will be rendered or not.
        /// If <see cref="ShowLabels"/> is <c>true</c>, the axis labels will be rendered, otherwise not rendered.
        /// </remarks>
        public bool ShowLabels
        {
            get { return (bool)this.GetValue(ShowLabelsProperty); }
            set { this.SetValue(ShowLabelsProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to use the range color for axis elements such as labels and ticks.
        /// </summary>
        /// <value>
        /// <b>true</b> if use range color is enabled; otherwise, <b>false</b>.The default value is <b>false</b>.
        /// </value>
        /// <remarks>
        /// It decides whether the corresponding range color will be applied to the axis elements like labels and ticks or not.
        /// If <see cref="UseRangeColorForAxis"/> is <c>true</c>, the corresponding range colors will be applied, otherwise not.
        /// </remarks>
        public bool UseRangeColorForAxis
        {
            get { return (bool)this.GetValue(UseRangeColorForAxisProperty); }
            set { this.SetValue(UseRangeColorForAxisProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value that indicates the dimension whether to show horizontal or vertical linear gauge. 
        /// </summary>
        /// <value>
        /// Orientation of linear gauge. The default is <c>GaugeOrientation.Horizontal"</c>.
        /// </value>
        /// <remarks>
        /// <c>Orientation</c> decides whether the gauge will be rendered in horizontal or vertical direction.
        /// </remarks>
        public GaugeOrientation Orientation
        {
            get { return (GaugeOrientation)this.GetValue(OrientationProperty); }
            set { this.SetValue(OrientationProperty, value); }
        }

        /// <summary>
        /// Gets or sets the <see cref="LinearRange"/> collection to the linear gauge.
        /// </summary>
        /// <value>
        /// The collection of linear range to display the current value of the axis. The default value is empty collection.
        /// </value>
        public ObservableCollection<LinearRange> Ranges
        {
            get { return (ObservableCollection<LinearRange>)this.GetValue(RangesProperty); }
            set { this.SetValue(RangesProperty, value); }
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

        #region Public virtual methods

        /// <summary>
        /// Calculates the visible labels based on axis interval and range.
        /// </summary>
        /// <returns>The visible label collection.</returns>
        public virtual List<GaugeLabelInfo> GenerateVisibleLabels()
        {
            this.MinorTickPositions.Clear();
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

        /// <summary>
        /// Converts axis value to circular factor value.
        /// </summary>
        /// <param name="value">The axis value to convert as factor.</param>
        /// <returns>Circular factor of the provided value.</returns>
        public virtual double ValueToFactor(double value)
        {
            double factor = (value - this.ActualMinimum) / (this.ActualMaximum - this.ActualMinimum);

            if (this.Orientation == GaugeOrientation.Horizontal)
            {
                return this.IsInversed ? 1d - factor : factor;
            }

            return !this.IsInversed ? 1d - factor : factor;
        }

        #endregion

        #region Internal methods

        internal void Draw(ICanvas canvas)
        {
            DrawAxisLine(canvas);

            if (this.ShowTicks)
            {
                this.DrawMajorTicks(canvas);
                this.DrawMinorTicks(canvas);
            }

            if (this.ShowLabels)
            {
                this.DrawAxisLabels(canvas);
            }
        }

        /// <summary>
        /// To get the screen position from value.
        /// </summary>
        /// <param name="value">The value to be convert as screen position.</param>
        /// <returns>Screen position for the given value.</returns>
        internal double GetPositionFromValue(double value)
        {
            double factor = this.ValueToFactor(value);
            return factor * axisLineLength;
        }

        internal void InvalidateDrawable()
        {
            this.linearScaleView.InvalidateDrawable();
        }

        /// <summary>
        /// To get the actual label offset based on ShowAxisLine property.
        /// </summary>
        /// <returns>The actual label offset based on ShowAxisLine property.</returns>
        internal double GetActualAxisLineThickness()
        {
            if (this.ShowAxisLine && this.AxisLineStyle != null)
            {
                return Math.Abs(this.AxisLineStyle.Thickness);
            }

            return 0d;
        }

        /// <summary>
        /// To get the actual element position based on IsMirrored property.
        /// </summary>
        /// <param name="gaugeElementPosition">The current linear gauge element position.</param>
        /// <returns>The actual element position value based on IsMirrored property.</returns>
        internal GaugeElementPosition GetActualElementPosition(GaugeElementPosition gaugeElementPosition)
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

        internal void ScaleInvalidateMeasureOverride()
        {
            this.InvalidateMeasureOverride();
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
                linearGauge.ValidateMinimumMaximum();
                linearGauge.UpdateAxis();
                linearGauge.InvalidateAxis();
            }
        }

        /// <summary>
        /// Called when <see cref="MinorTickStyle"/> property got changed.
        /// </summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="e">The PropertyChangedEventArgs.</param>
        private void AxisMinorStyle_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName != null)
            {
                AxisTicStyle_PropertyChanged(e.PropertyName, false);
            }
        }

        /// <summary>
        /// Called when <see cref="MajorTickStyle"/> property got changed.
        /// </summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="e">The PropertyChangedEventArgs.</param>
        private void AxisMajorStyle_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName != null)
            {
                AxisTicStyle_PropertyChanged(e.PropertyName, true);
            }
        }

        /// <summary>
        /// Called when <see cref="MajorTickStyle"/> or <see cref="MinorTickStyle"/> property got changed.
        /// </summary>
        /// <param name="propertyName">Property name.</param>
        /// <param name="isMajorTick">Boolean to identify major tick or not.</param>
        private void AxisTicStyle_PropertyChanged(string propertyName, bool isMajorTick)
        {
            if (propertyName == GaugeTickStyle.LengthProperty.PropertyName)
            {
                if (this.TickPosition != GaugeElementPosition.Inside)
                {
                    this.UpdateAxis();
                    this.InvalidateAxis();
                }
                else
                {
                    this.UpdateAxisElements();
                    this.InvalidateDrawable();
                }
            }
            else if (propertyName == GaugeTickStyle.StrokeThicknessProperty.PropertyName)
            {
                this.UpdateAxisElements();
                this.InvalidateDrawable();
            }
            else
            {
                this.InvalidateDrawable();
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
                sfLinearGauge.UpdateAxisElements();
                sfLinearGauge.InvalidateDrawable();
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
            if (bindable is SfLinearGauge sfLinearGauge)
            {
                sfLinearGauge.UpdateAxis();
                sfLinearGauge.InvalidateAxis();
            }
        }

        /// <summary>
        /// Called when axis orientation properties changed.
        /// </summary>
        /// <param name="bindable">The BindableObject.</param>
        /// <param name="oldValue">Old value.</param>
        /// <param name="newValue">New value.</param>
        private static void OnOrientationPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is SfLinearGauge sfLinearGauge)
            {
                sfLinearGauge.ScaleInvalidateMeasureOverride();
            }
        }

        /// <summary>
        /// Called when <see cref="LabelFormat"/> property changed.
        /// </summary>
        /// <param name="bindable">The BindableObject.</param>
        /// <param name="oldValue">Old value.</param>
        /// <param name="newValue">New value.</param>
        private static void OnLabelFormatPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is SfLinearGauge sfLinearGauge)
            {
                if (sfLinearGauge.LabelPosition == GaugeLabelsPosition.Outside)
                {
                    sfLinearGauge.UpdateAxis();
                    sfLinearGauge.InvalidateAxis();
                }
                else
                {
                    sfLinearGauge.UpdateAxisElements();
                    sfLinearGauge.InvalidateDrawable();
                }
            }
        }

        /// <summary>
        /// Called when <see cref="LabelOffset"/> property changed.
        /// </summary>
        /// <param name="bindable">The BindableObject.</param>
        /// <param name="oldValue">Old value.</param>
        /// <param name="newValue">New value.</param>
        private static void OnLabelOffsetPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is SfLinearGauge sfLinearGauge)
            {
                if (sfLinearGauge.LabelPosition == GaugeLabelsPosition.Outside)
                {
                    sfLinearGauge.UpdateAxis();
                    sfLinearGauge.InvalidateAxis();
                }
                else
                {
                    sfLinearGauge.UpdateAxisElements();
                    sfLinearGauge.InvalidateDrawable();
                }
            }
        }

        /// <summary>
        /// Called when <see cref="TickOffset"/> property changed.
        /// </summary>
        /// <param name="bindable">The BindableObject.</param>
        /// <param name="oldValue">Old value.</param>
        /// <param name="newValue">New value.</param>
        private static void OnTickOffsetPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is SfLinearGauge sfLinearGauge)
            {
                if (sfLinearGauge.TickPosition == GaugeElementPosition.Outside)
                {
                    sfLinearGauge.UpdateAxis();
                    sfLinearGauge.InvalidateAxis();
                }
                else
                {
                    sfLinearGauge.UpdateAxisElements();
                    sfLinearGauge.InvalidateDrawable();
                }
            }
        }

        /// <summary>
        /// Called when <see cref="AxisLabelStyle"/> or property changed.
        /// </summary>
        /// <param name="bindable">The BindableObject.</param>
        /// <param name="oldValue">Old value.</param>
        /// <param name="newValue">New value.</param>
        private static void OnStylePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is SfLinearGauge sfLinearGauge)
            {
                if (oldValue is LinearLineStyle oldAxisLineStyle)
                {
#nullable disable
                    if (oldAxisLineStyle.GradientStops is ObservableCollection<GaugeGradientStop> gradientStops)
                    {
                        gradientStops.CollectionChanged -= sfLinearGauge.GradientStops_CollectionChanged;
                    }
#nullable enable
                    oldAxisLineStyle.PropertyChanged -= sfLinearGauge.AxisLineStyle_PropertyChanged;
                }
                else if (oldValue is GaugeLabelStyle oldGaugeLabelStyle)
                {
                    oldGaugeLabelStyle.PropertyChanged -= sfLinearGauge.GaugeLabelStyle_PropertyChanged;
                }

                if (newValue is LinearLineStyle axisLineStyle)
                {
#nullable disable
                    if (axisLineStyle.GradientStops is ObservableCollection<GaugeGradientStop> gradientStops)
                    {
                        gradientStops.CollectionChanged += sfLinearGauge.GradientStops_CollectionChanged;
                    }
#nullable enable
                    axisLineStyle.PropertyChanged += sfLinearGauge.AxisLineStyle_PropertyChanged;
                }
                else if (newValue is GaugeLabelStyle gaugeLabelStyle)
                {
                    gaugeLabelStyle.PropertyChanged += sfLinearGauge.GaugeLabelStyle_PropertyChanged;
                }

                sfLinearGauge.InvalidateDrawable();
            }
        }

        /// <summary>
        /// Called when <see cref="MinorTickStyle"/> property changed.
        /// </summary>
        /// <param name="bindable">The BindableObject.</param>
        /// <param name="oldValue">Old value.</param>
        /// <param name="newValue">New value.</param>
        private static void OnMinorTickStylePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is SfLinearGauge sfLinearGauge)
            {
                if (oldValue is LinearTickStyle oldMinorTickStyle)
                {
                    oldMinorTickStyle.PropertyChanged -= sfLinearGauge.AxisMinorStyle_PropertyChanged;
                }

                if (newValue is LinearTickStyle newMinorTickStyle)
                {
                    newMinorTickStyle.PropertyChanged += sfLinearGauge.AxisMinorStyle_PropertyChanged;
                }

                sfLinearGauge.InvalidateDrawable();
            }
        }

        /// <summary>
        /// Called when <see cref="MajorTickStyle"/> property changed.
        /// </summary>
        /// <param name="bindable">The BindableObject.</param>
        /// <param name="oldValue">Old value.</param>
        /// <param name="newValue">New value.</param>
        private static void OnMajorTickStylePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is SfLinearGauge sfLinearGauge)
            {
                if (oldValue is LinearTickStyle oldMajorTickStyle)
                {
                    oldMajorTickStyle.PropertyChanged -= sfLinearGauge.AxisMajorStyle_PropertyChanged;
                }

                if (newValue is LinearTickStyle newMajorTickStyle)
                {
                    newMajorTickStyle.IsMajorTicks = true;
                    newMajorTickStyle.PropertyChanged += sfLinearGauge.AxisMajorStyle_PropertyChanged;
                }

                sfLinearGauge.InvalidateDrawable();
            }
        }

        /// <summary>
        /// Called when axis drawing related properties changed.
        /// </summary>
        /// <param name="bindable">The BindableObject.</param>
        /// <param name="oldValue">Old value.</param>
        /// <param name="newValue">New value.</param>
        private static void OnInvalidatePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is SfLinearGauge sfLinearGauge)
            {
                sfLinearGauge.InvalidateDrawable();
            }
        }

        /// <summary>
        /// Called when <see cref="AxisLineStyle"/> property got changed.
        /// </summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="e">The PropertyChangedEventArgs.</param>
        private void AxisLineStyle_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == LinearLineStyle.ThicknessProperty.PropertyName)
            {
                this.UpdateAxisElements();
            }
            else if (e.PropertyName == LinearLineStyle.CornerStyleProperty.PropertyName ||
                e.PropertyName == LinearLineStyle.DashArrayProperty.PropertyName ||
                e.PropertyName == LinearLineStyle.CornerRadiusProperty.PropertyName)
            {
                this.CalculateAxisLine();
            }
            else if (e.PropertyName == LinearLineStyle.GradientStopsProperty.PropertyName)
            {
#nullable disable
                if (this.AxisLineStyle.GradientStops is ObservableCollection<GaugeGradientStop> gradientStops)
                {
                    gradientStops.CollectionChanged += this.GradientStops_CollectionChanged;
                }
#nullable enable
                this.CalculateAxisLine();
            }

            this.InvalidateDrawable();
        }

        /// <summary>
        /// Called when <see cref="AxisLabelStyle"/> property got changed.
        /// </summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="e">The PropertyChangedEventArgs.</param>
        private void GaugeLabelStyle_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == GaugeLabelStyle.TextColorProperty.PropertyName)
            {
                this.InvalidateDrawable();
            }
            else if (e.PropertyName == GaugeLabelStyle.FontAttributesProperty.PropertyName || e.PropertyName == GaugeLabelStyle.FontFamilyProperty.PropertyName || e.PropertyName == GaugeLabelStyle.FontSizeProperty.PropertyName)
            {
                if (this.LabelPosition == GaugeLabelsPosition.Inside)
                {
                    this.UpdateAxisElements();
                    this.InvalidateDrawable();
                }
                else
                {
                    this.UpdateAxis();
                    this.InvalidateAxis();
                }
            }
        }

        #nullable disable
        
        /// <summary>
        /// Called when <see cref="Ranges"/> property changed.
        /// </summary>
        /// <param name="bindable">The BindableObject.</param>
        /// <param name="oldValue">Old value.</param>
        /// <param name="newValue">New value.</param>
        private static void OnRangesPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is SfLinearGauge linearGauge)
            {
                if (oldValue != null)
                {
                    if (oldValue is ObservableCollection<LinearRange> ranges)
                    {
                        ranges.CollectionChanged -= linearGauge.Ranges_CollectionChanged;
                    }
                }

                if (newValue != null)
                {
                    if (newValue is ObservableCollection<LinearRange> ranges)
                    {
                        ranges.CollectionChanged += linearGauge.Ranges_CollectionChanged;
                    }
                }
            }
        }

        #nullable enable
        
        #endregion

        #region Private methods

        #region Drawing methods

        private void DrawAxisLine(ICanvas canvas)
        {
            if (this.AxisLineStyle != null && this.axisLinePath != null)
            {
                if (this.AxisLineStyle.LinearGradientBrush != null)
                    canvas.SetFillPaint(this.AxisLineStyle.LinearGradientBrush, axisLinePath.Bounds);
                else
                    canvas.SetFillPaint(this.AxisLineStyle.Fill, axisLinePath.Bounds);
            }

            canvas.FillPath(axisLinePath);
        }

        private void DrawMajorTicks(ICanvas canvas)
        {
            Paint? majorStrokePaint = null;

            //Setting major tick style.
            if (this.MajorTickStyle != null)
            {
                if (this.MajorTickStyle.StrokeThickness >= 0)
                {
                    canvas.StrokeSize = (float)this.MajorTickStyle.StrokeThickness;
                }

                //Setting axis major tick line dash array.
                if (this.MajorTickStyle.StrokeDashArray?.Count > 0)
                {
                    canvas.StrokeDashPattern = this.MajorTickStyle.StrokeDashArray.ToFloatArray();
                }
                else
                {
                    canvas.StrokeDashPattern = null;
                }

                if (this.MajorTickStyle.Stroke != null)
                {
                    majorStrokePaint = this.MajorTickStyle.Stroke;
                }
            }

            //Drawing major ticks.
            double length = this.MajorTickPositions.Count;

            for (int i = 0; i < length; i++)
            {
                AxisTickInfo tick = this.MajorTickPositions[i];

                Color? rangeColor = GetRangeColor(tick.Value);

                if (rangeColor != null)
                {
                    canvas.StrokeColor = rangeColor;
                }
                else if (majorStrokePaint != null)
                {
                    // TODO: Add Paint support for Stroke in Microsoft.Maui.Graphics.
                    // For now, only support a solid color.
                    canvas.StrokeColor = majorStrokePaint.ToColor();
                }

                canvas.DrawLine(tick.StartPoint, tick.EndPoint);
            }
        }

        private void DrawMinorTicks(ICanvas canvas)
        {
            Paint? minorStrokePaint = null;

            //Setting minor tick style.
            if (this.MinorTickStyle != null)
            {
                if (this.MinorTickStyle.StrokeThickness >= 0)
                {
                    canvas.StrokeSize = (float)this.MinorTickStyle.StrokeThickness;
                }

                //Setting axis major tick line dash array.
                if (this.MinorTickStyle.StrokeDashArray?.Count > 0)
                {
                    canvas.StrokeDashPattern = this.MinorTickStyle.StrokeDashArray.ToFloatArray();
                }
                else
                {
                    canvas.StrokeDashPattern = null;
                }
                if (this.MinorTickStyle.Stroke != null)
                {
                    minorStrokePaint = this.MinorTickStyle.Stroke;
                }
            }

            //Drawing minor ticks.
            double length = this.MinorTickPositions.Count;

            for (int i = 0; i < length; i++)
            {
                AxisTickInfo tick = this.MinorTickPositions[i];

                Color? rangeColor = GetRangeColor(tick.Value);

                if (rangeColor != null)
                {
                    canvas.StrokeColor = rangeColor;
                }
                else if (minorStrokePaint != null)
                {
                    // TODO: Add Paint support for Stroke in Microsoft.Maui.Graphics.
                    // For now, only support a solid color.
                    canvas.StrokeColor = minorStrokePaint.ToColor();
                }

                canvas.DrawLine(tick.StartPoint, tick.EndPoint);
            }
        }

        private void DrawAxisLabels(ICanvas canvas)
        {
            if (this.VisibleLabels != null && this.VisibleLabels.Count > 0)
            {
                double length = this.VisibleLabels.Count;

                for (int i = 0; i < length; i++)
                {
                    GaugeLabelInfo label = this.VisibleLabels[i];

                    if (label.LabelStyle == null) continue;

                    PointF position = label.Position;

                    //Setting text color axis labels.
                    Color? rangeColor = GetRangeColor(label.Value);
                    GaugeLabelStyle? labelStyle = null;
                    if (rangeColor != null)
                    {
                        labelStyle = new GaugeLabelStyle()
                        {
                            TextColor = rangeColor,
                            FontAttributes = label.LabelStyle.FontAttributes,
                            FontFamily = label.LabelStyle.FontFamily,
                            FontSize = label.LabelStyle.FontSize
                        };

                    }

                    //Drawing axis labels.
                    canvas.DrawText(label.Text, position.X, position.Y, labelStyle ?? label.LabelStyle);
                }
            }
        }

        /// <summary>
        /// Method used to get value match range color.
        /// </summary>
        /// <param name="value">Input axis value</param>
        /// <returns>Returns range color.</returns>
        private Color? GetRangeColor(double value)
        {
            LinearRange? range = null;
            if (UseRangeColorForAxis && this.Ranges != null && this.Ranges.Count > 0)
            {
                range = this.Ranges.FirstOrDefault(item => value <= item.ActualEndValue &&
                    value >= item.ActualStartValue);
            }

            if (range != null)
            {
                Paint rangeFillPaint = range.Fill;
                return rangeFillPaint.ToColor();
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Method used to invalidate axis. 
        /// </summary>
        private void InvalidateAxis()
        {
            this.InvalidateDrawable();

            foreach (var range in Ranges)
            {
                range.InvalidateDrawable();
            }

            //foreach (var pointer in Pointers)
            //{
            //    pointer.InvalidateDrawable();
            //}
        }

        #endregion

        #region Calculation methods

        /// <summary>
        /// To update the axis elements
        /// </summary>
        private void UpdateAxis()
        {
            if (!this.ScaleAvailableSize.IsZero)
            {
                this.UpdateAxisElements();
                this.CreateRanges();
                //this.CreateBarPointers();
                //this.CreateMarkerPointers();
            }
        }

        /// <summary>
        /// To create the ranges.
        /// </summary>
        private void CreateRanges()
        {
            foreach (LinearRange linearRange in this.Ranges)
            {
                linearRange.CreateRangePath();
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
                this.CalculateAxisElementsPosition();
                this.CalculateAxisLine();

                if (this.ShowTicks)
                {
                    this.CalculateMajorTickPosition();

                    if (this.MinorTicksPerInterval > 0)
                    {
                        this.CalculateMinorTickPosition();
                    }
                }
                if (this.ShowLabels)
                {
                    this.CalculateAxisLabelsPosition();
                }
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
            double area = this.Orientation == GaugeOrientation.Horizontal ? this.ScaleAvailableSize.Width :
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
            label.LabelStyle = this.AxisLabelStyle;

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
                    this.MinorTickPositions.Add(new AxisTickInfo() { Value = tickPosition });
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
                int i = 0;

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

                    if (i == 0)
                    {
                        if (this.IsInversed)
                        {
                            this.lastLabelSize = labelInfo.DesiredSize;
                        }
                        else
                        {
                            this.firstLabelSize = labelInfo.DesiredSize;
                        }
                    }
                    else if (i == this.VisibleLabels.Count - 1)
                    {
                        if (this.IsInversed)
                        {
                            this.firstLabelSize = labelInfo.DesiredSize;
                        }
                        else
                        {
                            this.lastLabelSize = labelInfo.DesiredSize;
                        }
                    }

                    maximumHeight = Math.Max(maximumHeight, labelInfo.DesiredSize.Height);
                    maximumWidth = Math.Max(maximumWidth, labelInfo.DesiredSize.Width);
                    this.LabelMaximumSize = new Size(maximumWidth, maximumHeight);
                    i++;
                }
            }
        }

        /// <summary>
        /// To calculate axis line.
        /// </summary>
        private void CalculateAxisLine()
        {
            if (VisibleLabels == null) return;

            if (this.Orientation == GaugeOrientation.Horizontal)
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

            double actualAxisLineThickness = this.GetActualAxisLineThickness();
            double axisLineStartPosition = this.AxisLinePosition.X;
            double axisLineEndPosition = this.AxisLinePosition.X + axisLineLength;
            float x, y, width, height;
            if (this.Orientation == GaugeOrientation.Horizontal)
            {
                x = (float)axisLineStartPosition;
                y = (float)(this.AxisLinePosition.Y);
                width = (float)Math.Abs(axisLineEndPosition - axisLineStartPosition);
                height = (float)actualAxisLineThickness;
            }
            else
            {
                x = (float)(this.AxisLinePosition.Y);
                y = (float)axisLineStartPosition;
                width = (float)actualAxisLineThickness;
                height = (float)Math.Abs(axisLineEndPosition - axisLineStartPosition);
            }

            axisLinePath = new PathF();

            if ((this.AxisLineStyle.CornerStyle == CornerStyle.StartCurve && !IsInversed) ||
                (this.AxisLineStyle.CornerStyle == CornerStyle.EndCurve && IsInversed) ||
                this.AxisLineStyle.CornerStyle == CornerStyle.BothCurve)
            {
                if (Orientation == GaugeOrientation.Horizontal)
                {
                    axisLinePath.MoveTo(x + (height / 2), y);
                    axisLinePath.AddArc(x, y, x + height, y + height, 90, 270, false);
                    x += height / 2;
                    width -= height / 2;
                }
                else
                {
                    axisLinePath.MoveTo(x, y + height - (width / 2));
                    axisLinePath.AddArc(x, y + height - width, x + width,
                        y + height, 180, 360, false);
                    height -= width / 2;
                }
                axisLinePath.Close();
            }

            if ((this.AxisLineStyle.CornerStyle == CornerStyle.EndCurve && !IsInversed) ||
                (this.AxisLineStyle.CornerStyle == CornerStyle.StartCurve && IsInversed) ||
                this.AxisLineStyle.CornerStyle == CornerStyle.BothCurve)
            {
                if (Orientation == GaugeOrientation.Horizontal)
                {
                    axisLinePath.MoveTo(x + width - (height / 2), y);
                    axisLinePath.AddArc(x + width - height, y, x + width, y + height, 90, 270, true);
                    axisLinePath.Close();
                    width -= height / 2;
                }
                else
                {
                    axisLinePath.MoveTo(x, y + (width / 2));
                    axisLinePath.AddArc(x, y, x + width, y + width, 0, 180, false);
                    y += width / 2;
                    height -= width / 2;
                }
            }

            double dashLineLength = 0, dashLineGap = 0;

            if (this.AxisLineStyle.DashArray != null && this.AxisLineStyle.DashArray.Count > 1)
            {
                dashLineLength = this.AxisLineStyle.DashArray[0];
                dashLineGap = this.AxisLineStyle.DashArray[1];
            }

            if (dashLineLength > 0 && dashLineGap > 0)
            {
                float dashArrayStartValue, dashArrayEndValue;
                float axisLineEndValue = Orientation == GaugeOrientation.Horizontal ? x + width : y + height;
                dashArrayStartValue = Orientation == GaugeOrientation.Horizontal ? x : y;
                dashArrayEndValue = dashArrayStartValue + (float)dashLineLength;

                while (dashArrayEndValue <= axisLineEndValue)
                {
                    if (Orientation == GaugeOrientation.Horizontal)
                    {
                        axisLinePath.AppendRectangle(dashArrayStartValue, y, (float)dashLineLength, height);
                        dashArrayStartValue = dashArrayEndValue + (float)dashLineGap;
                        dashArrayEndValue = dashArrayStartValue + (float)dashLineLength;
                    }
                    else
                    {
                        axisLinePath.AppendRectangle(x, dashArrayStartValue, width, (float)dashLineLength);
                        dashArrayStartValue = dashArrayEndValue + (float)dashLineGap;
                        dashArrayEndValue = dashArrayStartValue + (float)dashLineLength;
                    }
                }

                if (dashArrayEndValue != axisLineEndValue)
                {
                    dashArrayStartValue = (float)axisLineEndValue - 1;

                    if (Orientation == GaugeOrientation.Horizontal)
                        axisLinePath.AppendRectangle(dashArrayStartValue, y, 1, height);
                    else
                        axisLinePath.AppendRectangle(x, dashArrayStartValue, width, 1);
                }
            }
            else
            {
                Thickness radius = AxisLineStyle.CornerRadius;

                if (AxisLineStyle.CornerStyle != CornerStyle.BothFlat)
                    radius = Thickness.Zero;

                axisLinePath.AppendRoundedRectangle(x, y, width, height, (float)radius.Left,
                    (float)radius.Top, (float)radius.Bottom, (float)radius.Right);
            }

            if (AxisLineStyle.GradientStops != null && AxisLineStyle.GradientStops.Count > 0)
                AxisLineStyle.LinearGradientBrush = GetLinearGradient(AxisLineStyle.GradientStops, Minimum, Maximum);
            else
                AxisLineStyle.LinearGradientBrush = null;
        }

        private void CalculateMajorTickPosition()
        {
            if (this.VisibleLabels != null)
            {
                this.MajorTickPositions.Clear();
                double adjustment, valuePosition;
                for (int i = 0; i < VisibleLabels.Count; i++)
                {
                    if (i == 0)
                    {
                        adjustment = MajorTickStyle.StrokeThickness / 2;
                    }
                    else if (i == this.VisibleLabels.Count - 1)
                    {
                        adjustment = -MajorTickStyle.StrokeThickness / 2;
                    }
                    else
                    {
                        adjustment = 0;
                    }

                    if ((Orientation == GaugeOrientation.Horizontal && IsInversed)
                        || (Orientation == GaugeOrientation.Vertical && !IsInversed))
                    {
                        adjustment *= -1;
                    }

                    AxisTickInfo axisTickInfo = new AxisTickInfo();
                    valuePosition = GetPositionFromValue(VisibleLabels[i].Value) + adjustment;
                    if (Orientation == GaugeOrientation.Horizontal)
                    {
                        axisTickInfo.StartPoint = new PointF((float)(this.majorTicksPanelPosition.X +
                            valuePosition), (float)this.majorTicksPanelPosition.Y);

                        axisTickInfo.EndPoint = new PointF(axisTickInfo.StartPoint.X,
                            (float)(this.majorTicksPanelPosition.Y + this.MajorTickStyle.Length));

                        axisTickInfo.Value = VisibleLabels[i].Value;
                        MajorTickPositions.Add(axisTickInfo);
                    }
                    else
                    {
                        axisTickInfo.StartPoint = new PointF((float)(this.majorTicksPanelPosition.Y),
                            (float)(this.majorTicksPanelPosition.X + valuePosition));

                        axisTickInfo.EndPoint = new PointF((float)(this.majorTicksPanelPosition.Y +
                            this.MajorTickStyle.Length), (float)(this.majorTicksPanelPosition.X + valuePosition));

                        axisTickInfo.Value = VisibleLabels[i].Value;
                        MajorTickPositions.Add(axisTickInfo);
                    }
                }
            }
        }

        private void CalculateMinorTickPosition()
        {
            double valuePosition;
            for (int i = 0; i < MinorTickPositions.Count; i++)
            {
                AxisTickInfo axisTickInfo = MinorTickPositions[i];
                valuePosition = this.GetPositionFromValue(axisTickInfo.Value);

                if (this.Orientation == GaugeOrientation.Horizontal)
                {
                    axisTickInfo.StartPoint = new PointF((float)(this.minorTicksPanelPosition.X + valuePosition),
                        (float)this.minorTicksPanelPosition.Y);

                    axisTickInfo.EndPoint = new PointF((float)(this.minorTicksPanelPosition.X + valuePosition),
                        (float)(this.minorTicksPanelPosition.Y + this.MinorTickStyle.Length));
                }
                else
                {
                    axisTickInfo.StartPoint = new PointF((float)(this.minorTicksPanelPosition.Y),
                        (float)(this.minorTicksPanelPosition.X + valuePosition));

                    axisTickInfo.EndPoint = new PointF((float)(this.minorTicksPanelPosition.Y + this.MinorTickStyle.Length),
                        (float)(this.minorTicksPanelPosition.X + valuePosition));
                }
            }
        }

        private void CalculateAxisLabelsPosition()
        {
            if (VisibleLabels != null)
            {
                foreach (var label in VisibleLabels)
                {
                    double position = this.GetPositionFromValue(label.Value);

                    if (this.Orientation == GaugeOrientation.Horizontal)
                    {
                        float labelPosY = (float)this.labelsPanelPosition.Y;
#if ANDROID
                        labelPosY += (float)label.DesiredSize.Height;
#endif
                        label.Position = new PointF((float)(this.labelsPanelPosition.X +
                            position - (label.DesiredSize.Width / 2)), labelPosY);
                    }
                    else
                    {
                        var y = this.labelsPanelPosition.X + position - (label.DesiredSize.Height / 2);
#if ANDROID
                        y += (float)label.DesiredSize.Height;
#endif
                        if ((this.LabelPosition == GaugeLabelsPosition.Outside && !this.IsMirrored)
                            || (this.LabelPosition == GaugeLabelsPosition.Inside && this.IsMirrored))
                        {
                            label.Position = new PointF((float)(this.labelsPanelPosition.Y +
                                this.LabelMaximumSize.Width - label.DesiredSize.Width), (float)y);
                        }
                        else
                        {
                            label.Position = new PointF((float)this.labelsPanelPosition.Y, (float)y);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// To calculate the axis line position.
        /// </summary>
        private void CalculateAxisElementsPosition()
        {
            double maximumTickLength = this.GetActualMaxTickLength();
            double actualTickOffset = this.GetActualTickOffset();
            double labelMaximumSize = this.GetLabelMaxLength();
            double firstLabelSize = this.GetFirstLabelLength();
            double actualLabelOffset = this.GetActualLabelOffset();
            double actualAxisLineThickness = this.GetActualAxisLineThickness();
            GaugeLabelsPosition actualLabelsPosition = this.GetActualLabelPosition();
            double outsideRangeHeight = 0d, insideRangeHeight = 0d;
            this.GetRangeHeights(ref outsideRangeHeight, ref insideRangeHeight);
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

                            var tickYPos = actualAxisLineThickness + actualTickOffset + outsideAxisHeight;
                            this.majorTicksPanelPosition = new Point(AxisLinePosition.X, tickYPos);
                            this.minorTicksPanelPosition = new Point(AxisLinePosition.X, tickYPos);

                            var labelYPos = this.majorTicksPanelPosition.Y + maximumTickLength + actualLabelOffset;
                            this.labelsPanelPosition = new Point(AxisLinePosition.X, labelYPos);
                            break;
                        case GaugeLabelsPosition.Outside:
                            y = labelMaximumSize + actualLabelOffset;
                            this.AxisLinePosition = new Point(x, y > outsideAxisHeight ? y : outsideAxisHeight);

                            tickYPos = this.AxisLinePosition.Y + actualAxisLineThickness + actualTickOffset;
                            this.majorTicksPanelPosition = new Point(AxisLinePosition.X, tickYPos);
                            this.minorTicksPanelPosition = new Point(AxisLinePosition.X, tickYPos);

                            labelYPos = outsideAxisHeight - labelMaximumSize - actualLabelOffset;
                            labelYPos = labelYPos < 0d ? 0d : labelYPos;
                            this.labelsPanelPosition = new Point(AxisLinePosition.X, labelYPos);
                            break;
                    }
                    break;
                case GaugeElementPosition.Outside:
                    switch (actualLabelsPosition)
                    {
                        case GaugeLabelsPosition.Inside:
                            y = maximumTickLength + actualTickOffset;
                            this.AxisLinePosition = new Point(x, y > outsideAxisHeight ? y : outsideAxisHeight);

                            var labelYPos = this.AxisLinePosition.Y + actualAxisLineThickness + actualLabelOffset;
                            this.labelsPanelPosition = new Point(AxisLinePosition.X, labelYPos);
                            break;
                        case GaugeLabelsPosition.Outside:
                            y = maximumTickLength + labelMaximumSize + actualTickOffset + actualLabelOffset;
                            this.AxisLinePosition = new Point(x, y > outsideAxisHeight ? y : outsideAxisHeight);

                            labelYPos = outsideAxisHeight - labelMaximumSize - actualLabelOffset - maximumTickLength - actualTickOffset;
                            labelYPos = labelYPos < 0d ? 0d : labelYPos;
                            this.labelsPanelPosition = new Point(AxisLinePosition.X, labelYPos);
                            break;
                    }

                    var majorTickY = this.AxisLinePosition.Y - (actualTickOffset + this.MajorTickStyle.Length);
                    this.majorTicksPanelPosition = new Point(AxisLinePosition.X, majorTickY);

                    var minorTicky = this.AxisLinePosition.Y - (actualTickOffset + this.MinorTickStyle.Length);
                    this.minorTicksPanelPosition = new Point(AxisLinePosition.X, minorTicky);
                    break;
                case GaugeElementPosition.Cross:
                    switch (actualLabelsPosition)
                    {
                        case GaugeLabelsPosition.Inside:
                            y = actualAxisLineThickness < maximumTickLength ? (maximumTickLength - actualAxisLineThickness) / 2 : 0d;
                            this.AxisLinePosition = new Point(x, y > outsideAxisHeight ? y : outsideAxisHeight);

                            majorTickY = this.AxisLinePosition.Y + (actualAxisLineThickness / 2) -
                                (this.MajorTickStyle.Length / 2);
                            this.majorTicksPanelPosition = new Point(AxisLinePosition.X, majorTickY);

                            minorTicky = this.AxisLinePosition.Y + (actualAxisLineThickness / 2) -
                                (this.MinorTickStyle.Length / 2);
                            this.minorTicksPanelPosition = new Point(AxisLinePosition.X, minorTicky);

                            var labelYPos = this.AxisLinePosition.Y + actualLabelOffset;
                            labelYPos += maximumTickLength > actualAxisLineThickness ?
                                (actualAxisLineThickness / 2) + (maximumTickLength / 2) :
                                actualAxisLineThickness;
                            this.labelsPanelPosition = new Point(AxisLinePosition.X, labelYPos);
                            break;
                        case GaugeLabelsPosition.Outside:
                            y = (actualAxisLineThickness < maximumTickLength ? (maximumTickLength - actualAxisLineThickness) / 2 : 0d) + actualLabelOffset + labelMaximumSize;
                            this.AxisLinePosition = new Point(x, y > outsideAxisHeight ? y : outsideAxisHeight);

                            majorTickY = this.AxisLinePosition.Y + (actualAxisLineThickness / 2) -
                                (this.MajorTickStyle.Length / 2);
                            this.majorTicksPanelPosition = new Point(AxisLinePosition.X, majorTickY);

                            minorTicky = this.AxisLinePosition.Y + (actualAxisLineThickness / 2) -
                               (this.MinorTickStyle.Length / 2);
                            this.minorTicksPanelPosition = new Point(AxisLinePosition.X, minorTicky);

                            labelYPos = this.AxisLinePosition.Y - actualLabelOffset - labelMaximumSize;
                            labelYPos -= maximumTickLength > actualAxisLineThickness ?
                                (actualAxisLineThickness / 2) + (maximumTickLength / 2) :
                                actualAxisLineThickness;
                            labelYPos = labelYPos < 0d ? 0d : labelYPos;
                            this.labelsPanelPosition = new Point(AxisLinePosition.X, labelYPos);
                            break;
                    }
                    break;
            }
        }

        /// <summary>
        /// To get the range heights based on positions.
        /// </summary>
        /// <param name="outsideRangeHeight">Outside positioned axis height.</param>
        /// <param name="insideRangeHeight">Inside positioned axis height.</param>
        internal void GetRangeHeights(ref double outsideRangeHeight, ref double insideRangeHeight)
        {
            double fillRangeHeight = 0d;
            double maxRangeWidth;
            
            foreach (LinearRange range in this.Ranges)
            {
                if (double.IsNaN(range.MidWidth))
                {
                    maxRangeWidth = Math.Max(range.StartWidth, range.EndWidth);
                }
                else
                {
                    maxRangeWidth = Math.Max(Math.Max(range.StartWidth, range.MidWidth), range.EndWidth);
                }

                switch (range.RangePosition)
                {
                    case GaugeElementPosition.Inside:
                        insideRangeHeight = Math.Max(insideRangeHeight, maxRangeWidth);
                        break;
                    case GaugeElementPosition.Outside:
                        outsideRangeHeight = Math.Max(outsideRangeHeight, maxRangeWidth);
                        break;
                    case GaugeElementPosition.Cross:
                        fillRangeHeight = Math.Max(fillRangeHeight, maxRangeWidth);
                        break;
                }
            }

            double actualAxisLineThickness = this.GetActualAxisLineThickness();
            if (fillRangeHeight > actualAxisLineThickness)
            {
                double diffWidth = (fillRangeHeight - actualAxisLineThickness) / 2;
                insideRangeHeight = Math.Max(diffWidth, insideRangeHeight);
                outsideRangeHeight = Math.Max(diffWidth, outsideRangeHeight);
            }

            if (this.IsMirrored)
            {
                Utility.Swap(ref insideRangeHeight, ref outsideRangeHeight);
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

            var majorTickLength = this.MajorTickStyle != null ? this.MajorTickStyle.Length : 0;
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
                return this.Orientation == GaugeOrientation.Horizontal ? this.LabelMaximumSize.Height : this.LabelMaximumSize.Width;
            }

            return 0d;
        }

        /// <summary>
        /// To get the first label length based on Orientation and ShowLabels  property.
        /// </summary>
        /// <returns>The first label length based on Orientation and ShowLabels  property.</returns>
        private double GetFirstLabelLength()
        {
            if (this.ShowLabels)
            {
                return this.Orientation == GaugeOrientation.Horizontal ? this.firstLabelSize.Width :
                    this.firstLabelSize.Height;
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
#if ANDROID || IOS
                return this.LabelOffset > 0d ? this.LabelOffset : 5d;
#else
                return this.LabelOffset > 0d ? this.LabelOffset : 2d;
#endif
            }

            return 0d;
        }

        /// <summary>
        /// To validate Minimum and Maximum.
        /// </summary>
        private void ValidateMinimumMaximum()
        {
            double minimum = Minimum;
            double maximum = Maximum;
            Utility.ValidateMinimumMaximumValue(ref minimum, ref maximum);
            ActualMinimum = minimum;
            ActualMaximum = maximum;
        }

        /// <summary>
        /// Called when <see cref="LinearLineStyle.GradientStops"/> collection got changed.
        /// </summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="e">The NotifyCollectionChangedEventArgs.</param>
        private void GradientStops_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (!this.ScaleAvailableSize.IsZero)
            {
                this.CalculateAxisLine();
                this.InvalidateDrawable();
            }
        }

        /// <summary>
        /// To get linear gradient brush based on gradient stops, start and end value.
        /// </summary>
        /// <param name="gradientStops">The gradient stops.</param>
        /// <param name="startValue">The start value.</param>
        /// <param name="endValue">The end value.</param>
        /// <returns>The linear gradient brush based on gradient stops, start and end value.</returns>
        internal LinearGradientBrush? GetLinearGradient(ObservableCollection<GaugeGradientStop> gradientStops,
            double startValue, double endValue)
        {
            if (gradientStops != null && gradientStops.Count > 0)
            {
                LinearGradientBrush gradient = new LinearGradientBrush();
                if (this.Orientation == GaugeOrientation.Vertical)
                {
                    gradient.StartPoint = new Point(0.5, 0);
                    gradient.EndPoint = new Point(0.5, 1);
                }
                else
                {
                    gradient.StartPoint = new Point(0, 0.5);
                    gradient.EndPoint = new Point(1, 0.5);
                }

                if (gradientStops.Count == 1)
                {
                    gradient.GradientStops.Add(new GradientStop()
                    {
                        Color = gradientStops[0].Color,
                        Offset = (float)this.ValueToFactor(this.ActualMaximum)
                    });
                }
                else
                {
                    List<GaugeGradientStop> gradientStopsList = gradientStops.OrderBy(x => x.ActualValue).ToList();
                    if (gradientStopsList[0].Value != startValue)
                    {
                        gradientStopsList.Insert(0, new GaugeGradientStop
                        {
                            Color = gradientStopsList[0].Color,
                            Value = this.ActualMinimum
                        });
                    }

                    if (gradientStopsList[gradientStopsList.Count - 1].Value != endValue)
                    {
                        gradientStopsList.Add(new GaugeGradientStop
                        {
                            Color = gradientStopsList[gradientStopsList.Count - 1].Color,
                            Value = this.ActualMaximum
                        });
                    }

                    for (int i = 0; i < gradientStopsList.Count; i++)
                    {
                        if (gradientStopsList[i].Value >= startValue && gradientStopsList[i].ActualValue <= endValue)
                        {
                            gradient.GradientStops.Add(new GradientStop()
                            {
                                Color = gradientStopsList[i].Color,
                                Offset = (float)this.ValueToFactor(gradientStopsList[i].ActualValue)
                            });
                        }
                    }

                    if (gradient.GradientStops.Count == 0)
                    {
                        for (int i = 0; i < gradientStopsList.Count; i++)
                        {
                            if (gradientStopsList[i].ActualValue >= startValue)
                            {
                                gradient.GradientStops.Add(new GradientStop()
                                {
                                    Color = gradientStopsList[i].Color,
                                    Offset = (float)this.ValueToFactor(this.ActualMaximum)
                                });
                                break;
                            }
                        }
                    }
                }

                return gradient;
            }

            return null;
        }

        /// <summary>
        /// Called when <see cref="Ranges"/> collection got changed.
        /// </summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="e">The NotifyCollectionChangedEventArgs.</param>
        private void Ranges_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                case NotifyCollectionChangedAction.Remove:
                case NotifyCollectionChangedAction.Replace:
                    if (e.NewItems != null && e.NewItems.Count > 0)
                    {
                        foreach (LinearRange linearRange in e.NewItems)
                        {
                            linearRange.LinearGauge = this;
                            if (!this.parentGrid.Contains(linearRange.RangeView))
                                this.parentGrid.Add(linearRange.RangeView);

                            if (!this.ScaleAvailableSize.IsZero)
                            {
                                SetInheritedBindingContext(linearRange, this.BindingContext);
                                linearRange.CreateRangePath();
                            }
                        }
                    }

                    if (e.OldItems != null && e.OldItems.Count > 0)
                    {
                        foreach (LinearRange linearRange in e.OldItems)
                        {
                            linearRange.LinearGauge = null;

                            if (this.parentGrid.Children.Contains(linearRange.RangeView))
                            {
                                this.parentGrid.Children.Remove(linearRange.RangeView);
                            }
                        }
                    }
                    break;

                case NotifyCollectionChangedAction.Reset:
                    foreach (var range in Ranges)
                    {
                        if (this.parentGrid.Children.Contains(range.RangeView))
                        {
                            this.parentGrid.Children.Remove(range.RangeView);
                        }
                    }
                    break;
                default:
                    break;
            }
        }

        #endregion

        #region Measure method

        /// <summary>
        /// Measure the content.
        /// </summary>
        /// <param name="widthConstraint"></param>
        /// <param name="heightConstraint"></param>
        /// <returns></returns>
        Size IContentView.CrossPlatformMeasure(double widthConstraint, double heightConstraint)
        {
            if (this.Orientation == GaugeOrientation.Horizontal && double.IsInfinity(widthConstraint))
            {
                widthConstraint = 350d;
            }
            else if (this.Orientation == GaugeOrientation.Vertical && double.IsInfinity(heightConstraint))
            {
                heightConstraint = 350d;
            }

            this.ScaleAvailableSize = new Size(widthConstraint, heightConstraint);
            this.UpdateAxis();

            if ((this.Orientation == GaugeOrientation.Horizontal && this.HeightRequest == -1)
                || (this.Orientation == GaugeOrientation.Vertical && this.WidthRequest == -1))
            {
                this.ScaleAvailableSize = this.GetAxisSize();
            }

            this.MeasureContent(ScaleAvailableSize.Width, ScaleAvailableSize.Height);

            return ScaleAvailableSize;
        }

        /// <summary>
        /// Arrange the content.
        /// </summary>
        /// <param name="bounds"></param>
        /// <returns></returns>
        Size IContentView.CrossPlatformArrange(Rectangle bounds)
        {
            this.ArrangeContent(bounds);
            return bounds.Size;
        }

        /// <summary>
        /// To get the axis size based on its children's size and positions.
        /// </summary>
        /// <returns>The size to be rendered for axis along with its elements.</returns>
        private Size GetAxisSize()
        {
            double maximumTickLength = this.GetActualMaxTickLength();
            double actualTickOffset = this.GetActualTickOffset();
            double labelMaximumSize = this.GetLabelMaxLength();
            double actualLabelOffset = this.GetActualLabelOffset();
            double actualAxisLineThickness = this.GetActualAxisLineThickness();
            GaugeLabelsPosition actualLabelPosition = this.GetActualLabelPosition();
            GaugeElementPosition actualTickPosition = this.GetActualElementPosition(this.TickPosition);
            double axisHeight, outsideRangeHeight = 0d, insideRangeHeight = 0d;
            this.GetRangeHeights(ref outsideRangeHeight, ref insideRangeHeight);
            double outsideBarPointerHeight = 0d, insideBarPointerHeight = 0d;
            //this.GetBarPointersHeight(ref outsideBarPointerHeight, ref insideBarPointerHeight);
            double outsideMarkerPointerHeight = 0d, insideMarkerPointerHeight = 0d;
            //this.GetMarkerPointersHeight(ref outsideMarkerPointerHeight, ref insideMarkerPointerHeight);
            double outsideAxisHeight = Math.Max(Math.Max(outsideRangeHeight, outsideBarPointerHeight), outsideMarkerPointerHeight);
            double insideAxisHeight = Math.Max(Math.Max(insideRangeHeight, insideBarPointerHeight), insideMarkerPointerHeight);
            double labelPanelHeight = labelMaximumSize + actualLabelOffset;
            double tickPanelHeight = maximumTickLength + actualTickOffset;
            double tickAndLabelHeight = labelPanelHeight + tickPanelHeight;

            if (actualTickPosition == GaugeElementPosition.Cross)
            {
                if (actualAxisLineThickness < maximumTickLength)
                {
                    double axisTop = (maximumTickLength - actualAxisLineThickness) / 2;
                    double diffHeight = axisTop;
                    if (actualLabelPosition == GaugeLabelsPosition.Outside)
                    {
                        axisTop += labelPanelHeight;
                        axisTop = outsideAxisHeight > axisTop ? outsideAxisHeight : axisTop;
                        axisHeight = actualAxisLineThickness + axisTop;
                        axisHeight += diffHeight > insideAxisHeight ? diffHeight : insideAxisHeight;
                    }
                    else
                    {
                        axisTop = outsideAxisHeight > axisTop ? outsideAxisHeight : axisTop;
                        axisHeight = actualAxisLineThickness + axisTop;
                        axisHeight += insideAxisHeight < (diffHeight + labelPanelHeight) ? (diffHeight + labelPanelHeight) : insideAxisHeight;
                    }
                }
                else
                {
                    if (actualLabelPosition == GaugeLabelsPosition.Outside)
                    {
                        axisHeight = labelPanelHeight > outsideAxisHeight ? labelPanelHeight : outsideAxisHeight;
                        axisHeight += actualAxisLineThickness + insideAxisHeight;
                    }
                    else
                    {
                        axisHeight = labelPanelHeight > insideAxisHeight ? labelPanelHeight : insideAxisHeight;
                        axisHeight += actualAxisLineThickness + outsideAxisHeight;
                    }
                }
            }
            else
            {
                if (actualLabelPosition == GaugeLabelsPosition.Inside && actualTickPosition == GaugeElementPosition.Inside)
                {
                    axisHeight = actualAxisLineThickness + outsideAxisHeight;
                    axisHeight += insideAxisHeight > tickAndLabelHeight ? insideAxisHeight : tickAndLabelHeight;
                }
                else if (actualLabelPosition == GaugeLabelsPosition.Outside && actualTickPosition == GaugeElementPosition.Outside)
                {
                    axisHeight = actualAxisLineThickness + insideAxisHeight;
                    axisHeight += outsideAxisHeight > tickAndLabelHeight ? outsideAxisHeight : tickAndLabelHeight;
                }
                else
                {
                    axisHeight = actualAxisLineThickness;
                    if (actualLabelPosition == GaugeLabelsPosition.Outside)
                    {
                        axisHeight += outsideAxisHeight < labelPanelHeight ? labelPanelHeight : outsideAxisHeight;
                    }

                    if (actualLabelPosition == GaugeLabelsPosition.Inside)
                    {
                        axisHeight += insideAxisHeight < labelPanelHeight ? labelPanelHeight : insideAxisHeight;
                    }

                    if (actualTickPosition == GaugeElementPosition.Outside)
                    {
                        axisHeight += outsideAxisHeight < tickPanelHeight ? tickPanelHeight : outsideAxisHeight;
                    }

                    if (actualTickPosition == GaugeElementPosition.Inside)
                    {
                        axisHeight += insideAxisHeight < tickPanelHeight ? tickPanelHeight : insideAxisHeight;
                    }
                }
            }

            return this.Orientation == GaugeOrientation.Horizontal
                ? new Size(this.ScaleAvailableSize.Width, axisHeight)
                : new Size(axisHeight, this.ScaleAvailableSize.Height);
        }

        #endregion

        #endregion
    }
}
