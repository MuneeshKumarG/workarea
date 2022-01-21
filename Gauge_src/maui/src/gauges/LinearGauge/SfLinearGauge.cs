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
    public class SfLinearGauge : View, IContentView, IVisualTreeElement
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
            BindableProperty.Create(nameof(Interval), typeof(double), typeof(SfLinearGauge), double.NaN, propertyChanged: OnScalePropertyChanged);

        /// <summary>
        /// Identifies the <see cref="MinorTicksPerInterval"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="MinorTicksPerInterval"/> bindable property.
        /// </value>
        public static readonly BindableProperty MinorTicksPerIntervalProperty =
            BindableProperty.Create(nameof(MinorTicksPerInterval), typeof(int), typeof(SfLinearGauge), 1, propertyChanged: OnScalePropertyChanged);

        /// <summary>
        /// Identifies the <see cref="MaximumLabelsCount"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="MaximumLabelsCount"/> bindable property.
        /// </value>
        public static readonly BindableProperty MaximumLabelsCountProperty =
            BindableProperty.Create(nameof(MaximumLabelsCount), typeof(int), typeof(SfLinearGauge), 3, propertyChanged: OnScalePropertyChanged);

        /// <summary>
        /// Identifies the <see cref="LabelFormat"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="LabelFormat"/> bindable property.
        /// </value>
        public static readonly BindableProperty LabelFormatProperty =
            BindableProperty.Create(nameof(LabelFormat), typeof(string), typeof(SfLinearGauge), null, propertyChanged: OnLabelFormatPropertyChanged);

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
        /// Identifies the <see cref="LineStyle"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="LineStyle"/> bindable property.
        /// </value>
        public static readonly BindableProperty LineStyleProperty =
            BindableProperty.Create(nameof(LineStyle), typeof(LinearLineStyle), typeof(SfLinearGauge), null, propertyChanged: OnStylePropertyChanged);

        /// <summary>
        /// Identifies the <see cref="LabelStyle"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="LabelStyle"/> bindable property.
        /// </value>
        public static readonly BindableProperty LabelStyleProperty =
            BindableProperty.Create(nameof(LabelStyle), typeof(GaugeLabelStyle), typeof(SfLinearGauge), null, propertyChanged: OnStylePropertyChanged);

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
        /// Identifies the <see cref="ShowLine"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="ShowLine"/> bindable property.
        /// </value>
        public static readonly BindableProperty ShowLineProperty =
            BindableProperty.Create(nameof(ShowLine), typeof(bool), typeof(SfLinearGauge), true, propertyChanged: OnPropertyChanged);

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
        /// Identifies the <see cref="Ranges"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="Ranges"/> bindable property.
        /// </value>
        public static readonly BindableProperty RangesProperty =
            BindableProperty.Create(nameof(Ranges), typeof(ObservableCollection<LinearRange>), typeof(SfLinearGauge), null, propertyChanged: OnRangesPropertyChanged);

        /// <summary>
        /// Identifies the <see cref="BarPointers"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="BarPointers"/> bindable property.
        /// </value>
        public static readonly BindableProperty BarPointersProperty =
            BindableProperty.Create(nameof(BarPointers), typeof(ObservableCollection<BarPointer>), typeof(SfLinearGauge), null, propertyChanged: OnBarPointersPropertyChanged);

        /// <summary>
        /// Identifies the <see cref="MarkerPointers"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="MarkerPointers"/> bindable property.
        /// </value>
        public static readonly BindableProperty MarkerPointersProperty =
            BindableProperty.Create(nameof(MarkerPointers), typeof(ObservableCollection<LinearMarkerPointer>), typeof(SfLinearGauge), null, propertyChanged: OnMarkerPointersPropertyChanged);

        #endregion

        #region Fields

        private Grid parentLayout;
        private AbsoluteLayout rangeLayout, barPointersLayout, shapePointersLayout;
        private LinearScaleView linearScaleView;
        private PathF? scaleLinePath;
        private double scaleLineLength;
        private PointF majorTicksLayoutPosition, minorTicksLayoutPosition, labelsLayoutPosition;
        private Size firstLabelSize, lastLabelSize;

        internal Point ScalePosition;
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
            this.parentLayout = new Grid();
            this.rangeLayout = new AbsoluteLayout();
            this.barPointersLayout = new AbsoluteLayout();
            this.shapePointersLayout = new AbsoluteLayout();
            this.parentLayout.Children.Add(this.linearScaleView);

            this.LineStyle = new LinearLineStyle();
            this.MajorTickStyle = new LinearTickStyle();
            this.MinorTickStyle = new LinearTickStyle();
            this.LabelStyle = new GaugeLabelStyle();
            this.ActualMaximum = this.Minimum;
            this.ActualMaximum = this.Maximum;

            this.MajorTickPositions = new List<AxisTickInfo>();
            this.MinorTickPositions = new List<AxisTickInfo>();
            this.Ranges = new ObservableCollection<LinearRange>();
            this.BarPointers = new ObservableCollection<BarPointer>();
            this.MarkerPointers = new ObservableCollection<LinearMarkerPointer> ();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the minimum value of the scale. The scale starts from this value.
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
        /// Gets or sets the maximum value of the scale. The scale ends at this value.
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
        /// Gets or sets the interval value of the scale. Using this, the scale labels can be displayed after a certain interval value.
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
        /// Gets or sets the maximum number of labels to be displayed in an scale in 100 logical pixels.
        /// </summary>
        /// <value>
        /// Maximum number of labels to be displayed in a scale in 100 logical pixels. Its default value is <c>3</c>. 
        /// </value>
        public int MaximumLabelsCount
        {
            get { return (int)this.GetValue(MaximumLabelsCountProperty); }
            set { this.SetValue(MaximumLabelsCountProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value to formats the scale labels with globalized string formats.
        /// </summary>
        /// <value>
        /// The string that specifies the globalized string formats for the scale labels. Its default value is <c>string.Empty</c>. 
        /// </value>
        public string LabelFormat
        {
            get { return (string)this.GetValue(LabelFormatProperty); }
            set { this.SetValue(LabelFormatProperty, value); }
        }

        /// <summary>
        /// Gets or sets the value that indicates the position of the scale labels inside or outside the scale line.
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
        /// Gets or sets a value to adjusts the scale label position from tick end. Specify value either in logical pixel value.
        /// </summary>
        /// <value>
        /// It defines the offset of the scale labels. The default value is <c>double.NaN</c>.
        /// </value>
        public double LabelOffset
        {
            get { return (double)this.GetValue(LabelOffsetProperty); }
            set { this.SetValue(LabelOffsetProperty, value); }
        }

        /// <summary>
        /// Gets or sets the value that indicates the position of the ticks inside, center, or outside the scale line.
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
        /// Gets or sets the value to adjusts the scale ticks position from the scale line. Specify value either in logical pixel value.
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
        /// Gets or sets a value to customize the scale line.
        /// </summary>
        public LinearLineStyle LineStyle
        {
            get { return (LinearLineStyle)this.GetValue(LineStyleProperty); }
            set { this.SetValue(LineStyleProperty, value); }
        }

        /// <summary>
        /// Gets or sets <see cref="GaugeLabelStyle"/>, that used to customize gauge scale labels.
        /// </summary>
        public GaugeLabelStyle LabelStyle
        {
            get { return (GaugeLabelStyle)this.GetValue(LabelStyleProperty); }
            set { this.SetValue(LabelStyleProperty, value); }
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
        /// Gets or sets a value indicating whether inverts the scale rendered in opposed view.
        /// </summary>
        /// <value>
        /// <b>true</b> if scale is mirrored; otherwise, <b>false</b>.The default value is <b>false</b>.
        /// </value>
        /// <remarks>
        /// <c>IsInversed</c> decides whether the scale will be inversed or not.
        /// If <see cref="IsMirrored"/> is <c>true</c>, the scale will be mirrored, otherwise not mirrored.
        /// </remarks>
        public bool IsMirrored
        {
            get { return (bool)this.GetValue(IsMirroredProperty); }
            set { this.SetValue(IsMirroredProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether inverts the scale direction.
        /// </summary>
        /// <value>
        /// <b>true</b> if scale is inversed; otherwise, <b>false</b>.The default value is <b>false</b>.
        /// </value>
        /// <remarks>
        /// <c>IsInversed</c> decides whether the scale will be inversed or not.
        /// If <see cref="IsInversed"/> is <c>true</c>, the scale will be inversed, otherwise not inversed.
        /// </remarks>
        public bool IsInversed
        {
            get { return (bool)this.GetValue(IsInversedProperty); }
            set { this.SetValue(IsInversedProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to shows or hides the scale tick lines.
        /// </summary>
        /// <value>
        /// <b>true</b> if scale line ticks are displayed; otherwise, <b>false</b>.The default value is <b>true</b>.
        /// </value>
        /// <remarks>
        /// It decides whether the scale ticks will be rendered or not.
        /// If <see cref="ShowTicks"/> is <c>true</c>, the scale ticks will be rendered, otherwise not rendered.
        /// </remarks>
        public bool ShowTicks
        {
            get { return (bool)this.GetValue(ShowTicksProperty); }
            set { this.SetValue(ShowTicksProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to shows or hides the scale line.
        /// </summary>
        /// <value>
        /// <b>true</b> if scale line is displayed; otherwise, <b>false</b>.The default value is <b>true</b>.
        /// </value>
        /// <remarks>
        /// It decides whether the scale line will be rendered or not.
        /// If <see cref="ShowLine"/> is <c>true</c>, the scale line will be rendered, otherwise not rendered.
        /// </remarks>
        public bool ShowLine
        {
            get { return (bool)this.GetValue(ShowLineProperty); }
            set { this.SetValue(ShowLineProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to shows or hides the scale labels.
        /// </summary>
        /// <value>
        /// <b>true</b> if scale labels are displayed; otherwise, <b>false</b>.The default value is <b>true</b>.
        /// </value>
        /// <remarks>
        /// It decides whether the scale labels will be rendered or not.
        /// If <see cref="ShowLabels"/> is <c>true</c>, the scale labels will be rendered, otherwise not rendered.
        /// </remarks>
        public bool ShowLabels
        {
            get { return (bool)this.GetValue(ShowLabelsProperty); }
            set { this.SetValue(ShowLabelsProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to use the range color for scale elements such as labels and ticks.
        /// </summary>
        /// <value>
        /// <b>true</b> if use range color is enabled; otherwise, <b>false</b>.The default value is <b>false</b>.
        /// </value>
        /// <remarks>
        /// It decides whether the corresponding range color will be applied to the scale elements like labels and ticks or not.
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
        /// The collection of linear range to display the current value of the scale. The default value is empty collection.
        /// </value>
        public ObservableCollection<LinearRange> Ranges
        {
            get { return (ObservableCollection<LinearRange>)this.GetValue(RangesProperty); }
            set { this.SetValue(RangesProperty, value); }
        }

        /// <summary>
        /// Gets or sets the <see cref="BarPointer"/> collection to the linear gauge.
        /// </summary>
        public ObservableCollection<BarPointer> BarPointers
        {
            get { return (ObservableCollection<BarPointer>)this.GetValue(BarPointersProperty); }
            set { this.SetValue(BarPointersProperty, value); }
        }

        /// <summary>
        /// Gets or sets the <see cref="MarkerPointers"/> collection to the linear gauge.
        /// </summary>
        /// <value>
        /// The collection of <see cref="MarkerPointers"/> and <see cref="ContentPointer"/> to display the current value of the axis.
        /// The default value is empty collection.
        /// </value>
        public ObservableCollection<LinearMarkerPointer> MarkerPointers
        {
            get { return (ObservableCollection<LinearMarkerPointer>)this.GetValue(MarkerPointersProperty); }
            set { this.SetValue(MarkerPointersProperty, value); }
        }

        /// <summary>
        /// Gets the root content.
        /// </summary>
        object IContentView.Content
        {
            get
            {
                return parentLayout;
            }
        }

        /// <summary>
        /// Gets the root content.
        /// </summary>
        IView? IContentView.PresentedContent
        {
            get
            {
                return parentLayout;
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


        /// <summary>
        /// Invoked whenever the binding context of the View changes.
        /// </summary>
        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();

            if (this.LabelStyle != null)
            {
                SetInheritedBindingContext(this.LabelStyle, this.BindingContext);
            }

            if (this.MajorTickStyle != null)
            {
                SetInheritedBindingContext(this.MajorTickStyle, this.BindingContext);
            }

            if (this.MinorTickStyle != null)
            {
                SetInheritedBindingContext(this.MinorTickStyle, this.BindingContext);
            }

            if (this.LineStyle != null)
            {
                SetInheritedBindingContext(this.LineStyle, this.BindingContext);
            }

            foreach (var range in this.Ranges)
            {
                SetInheritedBindingContext(range, this.BindingContext);
            }

            foreach (var pointer in this.BarPointers)
            {
                SetInheritedBindingContext(pointer, this.BindingContext);
            }

            foreach (var pointer in this.MarkerPointers)
            {
                SetInheritedBindingContext(pointer, this.BindingContext);
            }
        }
        #endregion

        #region Public virtual methods

        /// <summary>
        /// Calculates the visible labels based on scale interval and range.
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
                    GaugeLabelInfo currentLabel = this.GetScaleLabel(i);
                    visibleLabels.Add(currentLabel);
                    if (this.MinorTicksPerInterval > 0)
                    {
                        this.AddMinorTicksPoint(i);
                    }
                }

                GaugeLabelInfo label = visibleLabels[visibleLabels.Count - 1];
                if (label.Value != this.ActualMaximum && label.Value < this.ActualMaximum)
                {
                    GaugeLabelInfo currentLabel = this.GetScaleLabel(this.ActualMaximum);
                    visibleLabels.Add(currentLabel);
                }
            }

            return visibleLabels;
        }

        /// <summary>
        /// Converts scale value to linear factor value.
        /// </summary>
        /// <param name="value">The scale value to convert as factor.</param>
        /// <returns>Linear factor of the provided value.</returns>
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
            DrawScaleLine(canvas);

            if (this.ShowTicks)
            {
                this.DrawMajorTicks(canvas);
                this.DrawMinorTicks(canvas);
            }

            if (this.ShowLabels)
            {
                this.DrawScaleLabels(canvas);
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
            return factor * scaleLineLength;
        }

        internal void InvalidateDrawable()
        {
            this.linearScaleView.InvalidateDrawable();
        }

        /// <summary>
        /// To get the actual label offset based on ShowLine property.
        /// </summary>
        /// <returns>The actual label offset based on ShowLine property.</returns>
        internal double GetActualScaleLineThickness()
        {
            if (this.ShowLine && this.LineStyle != null)
            {
                return Math.Abs(this.LineStyle.Thickness);
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
            this.InvalidateScale();
        }

        internal void MoveToPath(PathF path, double x, double y)
        {
            if (path == null)
                return;

            if (this.Orientation == GaugeOrientation.Horizontal)
                path.MoveTo((float)x, (float)y);
            else
                path.MoveTo((float)y, (float)x);
        }

        internal void LineToPath(PathF path, double x, double y)
        {
            if (path == null)
                return;

            if (this.Orientation == GaugeOrientation.Horizontal)
                path.LineTo((float)x, (float)y);
            else
                path.LineTo((float)y, (float)x);
        }

        internal void BarPointerChildUpdate(object? oldView, object? newView)
        {
            if (oldView is View oldChild && this.barPointersLayout.Children.Contains(oldChild))
            {
                this.barPointersLayout.Children.Remove(oldChild);
            }

            if (newView is View newChild && !this.barPointersLayout.Children.Contains(newChild))
            {
                this.barPointersLayout.Children.Add(newChild);
            }
        }

        internal void ShapePointerChildUpdate(object? oldView, object? newView)
        {
            if (oldView is View oldChild && this.shapePointersLayout.Children.Contains(oldChild))
            {
                this.shapePointersLayout.Children.Remove(oldChild);
            }

            if (newView is View newChild && !this.shapePointersLayout.Children.Contains(newChild))
            {
                this.shapePointersLayout.Children.Add(newChild);
            }
        }

        internal void RangeChildUpdate(object? oldView, object? newView)
        {
            if (oldView is View oldChild && this.rangeLayout.Children.Contains(oldChild))
            {
                this.rangeLayout.Children.Remove(oldChild);
            }

            if (newView is View newChild && !this.rangeLayout.Children.Contains(newChild))
            {
                this.rangeLayout.Children.Add(newChild);
            }
        }

        internal void UpdateChild(View child, RectangleF bounds)
        {
            child.Measure(bounds.Width, bounds.Height);
            AbsoluteLayout.SetLayoutBounds(child, bounds);
        }

        /// <summary>
        /// Method used to invalidate scale. 
        /// </summary>
        internal void InvalidateScale()
        {
            this.InvalidateDrawable();

            foreach (var range in Ranges)
            {
                range.InvalidateDrawable();
            }

            foreach (var pointer in BarPointers)
            {
                pointer.InvalidateDrawable();
            }

            foreach (var pointer in MarkerPointers)
            {
                pointer.InvalidateDrawable();
            }
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
                linearGauge.UpdateScale();
                linearGauge.InvalidateScale();
            }
        }

        /// <summary>
        /// Called when <see cref="MinorTickStyle"/> property got changed.
        /// </summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="e">The PropertyChangedEventArgs.</param>
        private void ScaleMinorStyle_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName != null)
            {
                ScaleTicStyle_PropertyChanged(e.PropertyName, false);
            }
        }

        /// <summary>
        /// Called when <see cref="MajorTickStyle"/> property got changed.
        /// </summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="e">The PropertyChangedEventArgs.</param>
        private void ScaleMajorStyle_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName != null)
            {
                ScaleTicStyle_PropertyChanged(e.PropertyName, true);
            }
        }

        /// <summary>
        /// Called when <see cref="MajorTickStyle"/> or <see cref="MinorTickStyle"/> property got changed.
        /// </summary>
        /// <param name="propertyName">Property name.</param>
        /// <param name="isMajorTick">Boolean to identify major tick or not.</param>
        private void ScaleTicStyle_PropertyChanged(string propertyName, bool isMajorTick)
        {
            if (propertyName == GaugeTickStyle.LengthProperty.PropertyName)
            {
                if (this.TickPosition != GaugeElementPosition.Inside)
                {
                    this.UpdateScale();
                    this.InvalidateScale();
                }
                else
                {
                    this.UpdateScaleElements();
                    this.InvalidateDrawable();
                }
            }
            else if (propertyName == GaugeTickStyle.StrokeThicknessProperty.PropertyName)
            {
                this.UpdateScaleElements();
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
        private static void OnScalePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is SfLinearGauge sfLinearGauge)
            {
                sfLinearGauge.UpdateScaleElements();
                sfLinearGauge.InvalidateDrawable();
            }
        }

        /// <summary>
        /// Called when scale properties changed.
        /// </summary>
        /// <param name="bindable">The BindableObject.</param>
        /// <param name="oldValue">Old value.</param>
        /// <param name="newValue">New value.</param>
        private static void OnPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is SfLinearGauge sfLinearGauge)
            {
                sfLinearGauge.UpdateScale();
                sfLinearGauge.InvalidateScale();
            }
        }

        /// <summary>
        /// Called when scale orientation properties changed.
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
                    sfLinearGauge.UpdateScale();
                    sfLinearGauge.InvalidateScale();
                }
                else
                {
                    sfLinearGauge.UpdateScaleElements();
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
                    sfLinearGauge.UpdateScale();
                    sfLinearGauge.InvalidateScale();
                }
                else
                {
                    sfLinearGauge.UpdateScaleElements();
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
                    sfLinearGauge.UpdateScale();
                    sfLinearGauge.InvalidateScale();
                }
                else
                {
                    sfLinearGauge.UpdateScaleElements();
                    sfLinearGauge.InvalidateDrawable();
                }
            }
        }

        /// <summary>
        /// Called when <see cref="LabelStyle"/> or property changed.
        /// </summary>
        /// <param name="bindable">The BindableObject.</param>
        /// <param name="oldValue">Old value.</param>
        /// <param name="newValue">New value.</param>
        private static void OnStylePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is SfLinearGauge sfLinearGauge)
            {
                if (oldValue is LinearLineStyle oldScaleLineStyle)
                {
#nullable disable
                    if (oldScaleLineStyle.GradientStops is ObservableCollection<GaugeGradientStop> gradientStops)
                    {
                        gradientStops.CollectionChanged -= sfLinearGauge.GradientStops_CollectionChanged;
                    }
#nullable enable
                    oldScaleLineStyle.PropertyChanged -= sfLinearGauge.ScaleLineStyle_PropertyChanged;
                }
                else if (oldValue is GaugeLabelStyle oldGaugeLabelStyle)
                {
                    oldGaugeLabelStyle.PropertyChanged -= sfLinearGauge.GaugeLabelStyle_PropertyChanged;
                }

                if (newValue is LinearLineStyle scaleLineStyle)
                {
#nullable disable
                    if (scaleLineStyle.GradientStops is ObservableCollection<GaugeGradientStop> gradientStops)
                    {
                        gradientStops.CollectionChanged += sfLinearGauge.GradientStops_CollectionChanged;
                    }
#nullable enable
                    scaleLineStyle.PropertyChanged += sfLinearGauge.ScaleLineStyle_PropertyChanged;
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
                    oldMinorTickStyle.PropertyChanged -= sfLinearGauge.ScaleMinorStyle_PropertyChanged;
                }

                if (newValue is LinearTickStyle newMinorTickStyle)
                {
                    newMinorTickStyle.PropertyChanged += sfLinearGauge.ScaleMinorStyle_PropertyChanged;
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
                    oldMajorTickStyle.PropertyChanged -= sfLinearGauge.ScaleMajorStyle_PropertyChanged;
                }

                if (newValue is LinearTickStyle newMajorTickStyle)
                {
                    newMajorTickStyle.IsMajorTicks = true;
                    newMajorTickStyle.PropertyChanged += sfLinearGauge.ScaleMajorStyle_PropertyChanged;
                }

                sfLinearGauge.InvalidateDrawable();
            }
        }

        /// <summary>
        /// Called when scale drawing related properties changed.
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
        /// Called when <see cref="LineStyle"/> property got changed.
        /// </summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="e">The PropertyChangedEventArgs.</param>
        private void ScaleLineStyle_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == LinearLineStyle.ThicknessProperty.PropertyName)
            {
                this.ScaleInvalidateMeasureOverride();
            }
            else if (e.PropertyName == LinearLineStyle.CornerStyleProperty.PropertyName ||
                e.PropertyName == LinearLineStyle.DashArrayProperty.PropertyName ||
                e.PropertyName == LinearLineStyle.CornerRadiusProperty.PropertyName)
            {
                this.CalculateScaleLine();
            }
            else if (e.PropertyName == LinearLineStyle.GradientStopsProperty.PropertyName)
            {
#nullable disable
                if (this.LineStyle.GradientStops is ObservableCollection<GaugeGradientStop> gradientStops)
                {
                    gradientStops.CollectionChanged += this.GradientStops_CollectionChanged;
                }
#nullable enable
                this.CalculateScaleLine();
            }

            this.InvalidateDrawable();
        }

        /// <summary>
        /// Called when <see cref="LabelStyle"/> property got changed.
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
                    this.UpdateScaleElements();
                    this.InvalidateDrawable();
                }
                else
                {
                    this.UpdateScale();
                    this.InvalidateScale();
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

        /// <summary>
        /// Called when <see cref="BarPointers"/> property changed.
        /// </summary>
        /// <param name="bindable">The BindableObject.</param>
        /// <param name="oldValue">Old value.</param>
        /// <param name="newValue">New value.</param>
        private static void OnBarPointersPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is SfLinearGauge linearGauge)
            {
                if (oldValue != null)
                {
                    if (oldValue is ObservableCollection<BarPointer> barPointers)
                    {
                        barPointers.CollectionChanged -= linearGauge.BarPointers_CollectionChanged;
                    }
                }

                if (newValue != null)
                {
                    if (newValue is ObservableCollection<BarPointer> barPointers)
                    {
                        barPointers.CollectionChanged += linearGauge.BarPointers_CollectionChanged;
                    }
                }
            }
        }

        /// <summary>
        /// Called when <see cref="MarkerPointers"/> property changed.
        /// </summary>
        /// <param name="bindable">The BindableObject.</param>
        /// <param name="oldValue">Old value.</param>
        /// <param name="newValue">New value.</param>
        private static void OnMarkerPointersPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is SfLinearGauge linearGauge)
            {
                if (oldValue != null)
                {
                    if (oldValue is ObservableCollection<LinearMarkerPointer> markerPointers)
                    {
                        markerPointers.CollectionChanged -= linearGauge.MarkerPointers_CollectionChanged;
                    }
                }

                if (newValue != null)
                {
                    if (newValue is ObservableCollection<LinearMarkerPointer> markerPointers)
                    {
                        markerPointers.CollectionChanged += linearGauge.MarkerPointers_CollectionChanged;
                    }
                }
            }
        }

#nullable enable

        #endregion

        #region Private methods

        #region Drawing methods

        private void DrawScaleLine(ICanvas canvas)
        {
            if (this.LineStyle != null && this.scaleLinePath != null)
            {
                if (this.LineStyle.LinearGradientBrush != null)
                    canvas.SetFillPaint(this.LineStyle.LinearGradientBrush, scaleLinePath.Bounds);
                else
                    canvas.SetFillPaint(this.LineStyle.Fill, scaleLinePath.Bounds);
            }

            canvas.FillPath(scaleLinePath);
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

                //Setting scale major tick line dash array.
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

                //Setting scale major tick line dash array.
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

        private void DrawScaleLabels(ICanvas canvas)
        {
            if (this.VisibleLabels != null && this.VisibleLabels.Count > 0)
            {
                double length = this.VisibleLabels.Count;

                for (int i = 0; i < length; i++)
                {
                    GaugeLabelInfo label = this.VisibleLabels[i];

                    if (label.LabelStyle == null) continue;

                    PointF position = label.Position;

                    //Setting text color scale labels.
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

                    //Drawing scale labels.
                    canvas.DrawText(label.Text, position.X, position.Y, labelStyle ?? label.LabelStyle);
                }
            }
        }

        /// <summary>
        /// Method used to get value match range color.
        /// </summary>
        /// <param name="value">Input scale value</param>
        /// <returns>Returns range color.</returns>
        private Color? GetRangeColor(double value)
        {
            if (UseRangeColorForAxis && this.Ranges != null && this.Ranges.Count > 0)
            {
                LinearRange? range = this.Ranges.FirstOrDefault(item => value <= item.ActualEndValue &&
                    value >= item.ActualStartValue);
                if (range != null)
                {
                    Paint rangeFillPaint = range.Fill;
                    return rangeFillPaint.ToColor();
                }
            }
            return null;
        }

        #endregion

        #region Calculation methods

        /// <summary>
        /// To update the scale elements
        /// </summary>
        private void UpdateScale()
        {
            if (!this.ScaleAvailableSize.IsZero)
            {
                this.UpdateScaleElements();
                this.CreateRanges();
                this.CreateBarPointers();
                this.CreateMarkerPointers();

                foreach (var range in Ranges)
                {
                    AbsoluteLayout.SetLayoutBounds(range.RangeView, new Rectangle(0, 0, ScaleAvailableSize.Width, ScaleAvailableSize.Height));
                }
                foreach (var pointer in BarPointers)
                {
                    AbsoluteLayout.SetLayoutBounds(pointer.PointerView, new Rectangle(0, 0, ScaleAvailableSize.Width, ScaleAvailableSize.Height));
                }
                foreach (var pointer in MarkerPointers)
                {
                    AbsoluteLayout.SetLayoutBounds(pointer.PointerView, new Rectangle(0, 0, ScaleAvailableSize.Width, ScaleAvailableSize.Height));
                }
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
        /// To create the bar pointers.
        /// </summary>
        private void CreateBarPointers()
        {
            foreach (BarPointer barPointer in this.BarPointers)
            {
                barPointer.CreatePointer();
            }
        }

        /// <summary>
        /// To create the markers pointers.
        /// </summary>
        private void CreateMarkerPointers()
        {
            foreach (LinearMarkerPointer pointer in this.MarkerPointers)
            {
                pointer.CreatePointer();
            }
        }

        /// <summary>
        /// To update scale line, ticks and labels scale elements.
        /// </summary>
        private void UpdateScaleElements()
        {
            if (!this.ScaleAvailableSize.IsZero)
            {
                this.VisibleLabels = this.GenerateVisibleLabels();
                this.MeasureLabels();
                this.CalculateScaleElementsPosition();
                this.CalculateScaleLine();

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
                    this.CalculateScaleLabelsPosition();
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
                return this.CalculateScaleInterval();
            }

            return this.Interval;
        }

        /// <summary>
        /// To calculate the scale interval based on the maximum scale label count.
        /// </summary>
        /// <returns>Returns the interval based on the maximum number of labels for 100 labels</returns>
        private double CalculateScaleInterval()
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
        /// Gets the current scale labels
        /// </summary>
        /// <param name="value">The value of the label.</param>
        /// <returns>The corresponding scale label of given value.</returns>
        private GaugeLabelInfo GetScaleLabel(double value)
        {
            GaugeLabelInfo label = new GaugeLabelInfo
            {
                Value = value
            };
            string labelText = value.ToString(this.LabelFormat, CultureInfo.CurrentCulture);
            label.Text = labelText;
            label.LabelStyle = this.LabelStyle;

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
        /// To measure the scale labels
        /// </summary>
        private void MeasureLabels()
        {
            if (this.VisibleLabels != null)
            {
                double maximumWidth = 0, maximumHeight = 0;
                int i = 0;

                foreach (var labelInfo in this.VisibleLabels)
                {
                    if (labelInfo.LabelStyle == null && this.LabelStyle != null)
                    {
                        labelInfo.LabelStyle = this.LabelStyle;
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
        /// To calculate scale line.
        /// </summary>
        private void CalculateScaleLine()
        {
            if (this.Orientation == GaugeOrientation.Horizontal)
            {
                scaleLineLength = this.ShowLabels
                    ? this.ScaleAvailableSize.Width - (firstLabelSize.Width / 2) - (lastLabelSize.Width / 2)
                    : this.ScaleAvailableSize.Width;
            }
            else
            {
                scaleLineLength = this.ShowLabels
                    ? this.ScaleAvailableSize.Height - (firstLabelSize.Height / 2) - (lastLabelSize.Height / 2)
                    : this.ScaleAvailableSize.Height;
            }

            float actualScaleLineThickness = (float)this.GetActualScaleLineThickness();
            float scaleLineStartPosition = (float)this.ScalePosition.X;
            float scaleLineEndPosition = (float)(this.ScalePosition.X + scaleLineLength);
            float x, y, width, height;
            if (this.Orientation == GaugeOrientation.Horizontal)
            {
                x = scaleLineStartPosition;
                y = (float)this.ScalePosition.Y;
                width = Math.Abs(scaleLineEndPosition - scaleLineStartPosition);
                height = actualScaleLineThickness;
            }
            else
            {
                x = (float)this.ScalePosition.Y;
                y = (float)scaleLineStartPosition;
                width = actualScaleLineThickness;
                height = Math.Abs(scaleLineEndPosition - scaleLineStartPosition);
            }

            scaleLinePath = new PathF();

            if ((this.LineStyle.CornerStyle == CornerStyle.StartCurve && !IsInversed) ||
                (this.LineStyle.CornerStyle == CornerStyle.EndCurve && IsInversed) ||
                this.LineStyle.CornerStyle == CornerStyle.BothCurve)
            {
                if (Orientation == GaugeOrientation.Horizontal)
                {
                    scaleLinePath.MoveTo(x + (height / 2), y);
                    scaleLinePath.AddArc(x, y, x + height, y + height, 90, 270, false);
                    x += height / 2;
                    width -= height / 2;
                }
                else
                {
                    scaleLinePath.MoveTo(x, y + height - (width / 2));
                    scaleLinePath.AddArc(x, y + height - width, x + width,
                        y + height, 180, 360, false);
                    height -= width / 2;
                }
                scaleLinePath.Close();
            }

            if ((this.LineStyle.CornerStyle == CornerStyle.EndCurve && !IsInversed) ||
                (this.LineStyle.CornerStyle == CornerStyle.StartCurve && IsInversed) ||
                this.LineStyle.CornerStyle == CornerStyle.BothCurve)
            {
                if (Orientation == GaugeOrientation.Horizontal)
                {
                    scaleLinePath.MoveTo(x + width - (height / 2), y);
                    scaleLinePath.AddArc(x + width - height, y, x + width, y + height, 90, 270, true);
                    scaleLinePath.Close();
                    width -= height / 2;
                }
                else
                {
                    scaleLinePath.MoveTo(x, y + (width / 2));
                    scaleLinePath.AddArc(x, y, x + width, y + width, 0, 180, false);
                    y += width / 2;
                    height -= width / 2;
                }
            }

            if (this.LineStyle.DashArray != null && this.LineStyle.DashArray.Count > 1)
            {
                //Calculate dashed scale line.
                this.CalculateDashedScaleLine(x, y, width, height);
            }
            else
            {
                Thickness radius = LineStyle.CornerRadius;

                if (LineStyle.CornerStyle != CornerStyle.BothFlat)
                    radius = Thickness.Zero;

                scaleLinePath.AppendRoundedRectangle(x, y, width, height, (float)radius.Left,
                    (float)radius.Top, (float)radius.Bottom, (float)radius.Right);
            }

            if (LineStyle.GradientStops != null && LineStyle.GradientStops.Count > 0)
                LineStyle.LinearGradientBrush = GetLinearGradient(LineStyle.GradientStops, Minimum, Maximum);
            else
                LineStyle.LinearGradientBrush = null;
        }

        private void CalculateDashedScaleLine(float x, float y, float width, float height)
        {
            double dashLineLength = this.LineStyle.DashArray[0];
            double dashLineGap = this.LineStyle.DashArray[1];
            if (dashLineLength > 0 && dashLineGap > 0 && scaleLinePath != null)
            {
                float dashArrayStartValue, dashArrayEndValue;
                float scaleLineEndValue = Orientation == GaugeOrientation.Horizontal ? x + width : y + height;
                dashArrayStartValue = Orientation == GaugeOrientation.Horizontal ? x : y;
                dashArrayEndValue = dashArrayStartValue + (float)dashLineLength;

                while (dashArrayEndValue <= scaleLineEndValue)
                {
                    if (Orientation == GaugeOrientation.Horizontal)
                    {
                        scaleLinePath.AppendRectangle(dashArrayStartValue, y, (float)dashLineLength, height);
                        dashArrayStartValue = dashArrayEndValue + (float)dashLineGap;
                        dashArrayEndValue = dashArrayStartValue + (float)dashLineLength;
                    }
                    else
                    {
                        scaleLinePath.AppendRectangle(x, dashArrayStartValue, width, (float)dashLineLength);
                        dashArrayStartValue = dashArrayEndValue + (float)dashLineGap;
                        dashArrayEndValue = dashArrayStartValue + (float)dashLineLength;
                    }
                }

                if (dashArrayEndValue != scaleLineEndValue)
                {
                    dashArrayStartValue = (float)scaleLineEndValue - 1;

                    if (Orientation == GaugeOrientation.Horizontal)
                        scaleLinePath.AppendRectangle(dashArrayStartValue, y, 1, height);
                    else
                        scaleLinePath.AppendRectangle(x, dashArrayStartValue, width, 1);
                }
            }
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

                    AxisTickInfo tickInfo = new AxisTickInfo();
                    valuePosition = GetPositionFromValue(VisibleLabels[i].Value) + adjustment;
                    if (Orientation == GaugeOrientation.Horizontal)
                    {
                        tickInfo.StartPoint = new PointF(this.majorTicksLayoutPosition.X +
                            (float)valuePosition, this.majorTicksLayoutPosition.Y);

                        tickInfo.EndPoint = new PointF(tickInfo.StartPoint.X,
                            this.majorTicksLayoutPosition.Y + (float)this.MajorTickStyle.Length);

                        tickInfo.Value = VisibleLabels[i].Value;
                        MajorTickPositions.Add(tickInfo);
                    }
                    else
                    {
                        tickInfo.StartPoint = new PointF(this.majorTicksLayoutPosition.Y,
                            this.majorTicksLayoutPosition.X + (float)valuePosition);

                        tickInfo.EndPoint = new PointF(this.majorTicksLayoutPosition.Y +
                            (float)this.MajorTickStyle.Length, this.majorTicksLayoutPosition.X + (float)valuePosition);

                        tickInfo.Value = VisibleLabels[i].Value;
                        MajorTickPositions.Add(tickInfo);
                    }
                }
            }
        }

        private void CalculateMinorTickPosition()
        {
            double valuePosition;
            for (int i = 0; i < MinorTickPositions.Count; i++)
            {
                AxisTickInfo tickInfo = MinorTickPositions[i];
                valuePosition = this.GetPositionFromValue(tickInfo.Value);

                if (this.Orientation == GaugeOrientation.Horizontal)
                {
                    tickInfo.StartPoint = new PointF(this.minorTicksLayoutPosition.X + (float)valuePosition,
                        (float)this.minorTicksLayoutPosition.Y);

                    tickInfo.EndPoint = new PointF(this.minorTicksLayoutPosition.X + (float)valuePosition,
                        this.minorTicksLayoutPosition.Y + (float)this.MinorTickStyle.Length);
                }
                else
                {
                    tickInfo.StartPoint = new PointF(this.minorTicksLayoutPosition.Y,
                        this.minorTicksLayoutPosition.X + (float)valuePosition);

                    tickInfo.EndPoint = new PointF(this.minorTicksLayoutPosition.Y + (float)this.MinorTickStyle.Length,
                        this.minorTicksLayoutPosition.X + (float)valuePosition);
                }
            }
        }

        private void CalculateScaleLabelsPosition()
        {
            if (VisibleLabels != null)
            {
                foreach (var label in VisibleLabels)
                {
                    double position = this.GetPositionFromValue(label.Value);

                    if (this.Orientation == GaugeOrientation.Horizontal)
                    {
                        float labelPosY = this.labelsLayoutPosition.Y;
#if ANDROID
                        labelPosY += (float)label.DesiredSize.Height;
#endif
                        label.Position = new PointF((float)(this.labelsLayoutPosition.X +
                            position - (label.DesiredSize.Width / 2)), labelPosY);
                    }
                    else
                    {
                        var y = this.labelsLayoutPosition.X + position - (label.DesiredSize.Height / 2);
#if ANDROID
                        y += (float)label.DesiredSize.Height;
#endif
                        if ((this.LabelPosition == GaugeLabelsPosition.Outside && !this.IsMirrored)
                            || (this.LabelPosition == GaugeLabelsPosition.Inside && this.IsMirrored))
                        {
                            label.Position = new PointF((float)(this.labelsLayoutPosition.Y +
                                this.LabelMaximumSize.Width - label.DesiredSize.Width), (float)y);
                        }
                        else
                        {
                            label.Position = new PointF(this.labelsLayoutPosition.Y, (float)y);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// To calculate the scale line position.
        /// </summary>
        private void CalculateScaleElementsPosition()
        {
            double maximumTickLength = this.GetActualMaxTickLength();
            double actualTickOffset = this.GetActualTickOffset();
            double labelMaximumSize = this.GetLabelMaxLength();
            double firstLabelSize = this.GetFirstLabelLength();
            double actualLabelOffset = this.GetActualLabelOffset();
            double actualScaleLineThickness = this.GetActualScaleLineThickness();
            GaugeLabelsPosition actualLabelsPosition = this.GetActualLabelPosition();
            double outsideRangeHeight = 0d, insideRangeHeight = 0d;
            this.GetRangeHeights(ref outsideRangeHeight, ref insideRangeHeight);
            double outsideBarPointerHeight = 0d, insideBarPointerHeight = 0d;
            this.GetBarPointersHeight(ref outsideBarPointerHeight, ref insideBarPointerHeight);
            double outsideMarkerPointerHeight = 0d, insideMarkerPointerHeight = 0d;
            this.GetMarkerPointersHeight(ref outsideMarkerPointerHeight, ref insideMarkerPointerHeight);
            double outsideScaleHeight = Math.Max(Math.Max(outsideRangeHeight, outsideBarPointerHeight), outsideMarkerPointerHeight);
            double x = firstLabelSize / 2, y = 0d;

            switch (this.GetActualElementPosition(this.TickPosition))
            {
                case GaugeElementPosition.Inside:
                    switch (actualLabelsPosition)
                    {
                        case GaugeLabelsPosition.Inside:
                            this.ScalePosition = new Point(x, y > outsideScaleHeight ? y : outsideScaleHeight);

                            var tickYPos = actualScaleLineThickness + actualTickOffset + outsideScaleHeight;
                            this.majorTicksLayoutPosition = new Point(ScalePosition.X, tickYPos);
                            this.minorTicksLayoutPosition = new Point(ScalePosition.X, tickYPos);

                            var labelYPos = this.majorTicksLayoutPosition.Y + maximumTickLength + actualLabelOffset;
                            this.labelsLayoutPosition = new Point(ScalePosition.X, labelYPos);
                            break;
                        case GaugeLabelsPosition.Outside:
                            y = labelMaximumSize + actualLabelOffset;
                            this.ScalePosition = new Point(x, y > outsideScaleHeight ? y : outsideScaleHeight);

                            tickYPos = this.ScalePosition.Y + actualScaleLineThickness + actualTickOffset;
                            this.majorTicksLayoutPosition = new Point(ScalePosition.X, tickYPos);
                            this.minorTicksLayoutPosition = new Point(ScalePosition.X, tickYPos);

                            labelYPos = outsideScaleHeight - labelMaximumSize - actualLabelOffset;
                            labelYPos = labelYPos < 0d ? 0d : labelYPos;
                            this.labelsLayoutPosition = new Point(ScalePosition.X, labelYPos);
                            break;
                    }
                    break;
                case GaugeElementPosition.Outside:
                    switch (actualLabelsPosition)
                    {
                        case GaugeLabelsPosition.Inside:
                            y = maximumTickLength + actualTickOffset;
                            this.ScalePosition = new Point(x, y > outsideScaleHeight ? y : outsideScaleHeight);

                            var labelYPos = this.ScalePosition.Y + actualScaleLineThickness + actualLabelOffset;
                            this.labelsLayoutPosition = new Point(ScalePosition.X, labelYPos);
                            break;
                        case GaugeLabelsPosition.Outside:
                            y = maximumTickLength + labelMaximumSize + actualTickOffset + actualLabelOffset;
                            this.ScalePosition = new Point(x, y > outsideScaleHeight ? y : outsideScaleHeight);

                            labelYPos = outsideScaleHeight - labelMaximumSize - actualLabelOffset - maximumTickLength - actualTickOffset;
                            labelYPos = labelYPos < 0d ? 0d : labelYPos;
                            this.labelsLayoutPosition = new Point(ScalePosition.X, labelYPos);
                            break;
                    }

                    var majorTickY = this.ScalePosition.Y - (actualTickOffset + this.MajorTickStyle.Length);
                    this.majorTicksLayoutPosition = new Point(ScalePosition.X, majorTickY);

                    var minorTicky = this.ScalePosition.Y - (actualTickOffset + this.MinorTickStyle.Length);
                    this.minorTicksLayoutPosition = new Point(ScalePosition.X, minorTicky);
                    break;
                case GaugeElementPosition.Cross:
                    switch (actualLabelsPosition)
                    {
                        case GaugeLabelsPosition.Inside:
                            y = actualScaleLineThickness < maximumTickLength ? (maximumTickLength - actualScaleLineThickness) / 2 : 0d;
                            this.ScalePosition = new Point(x, y > outsideScaleHeight ? y : outsideScaleHeight);

                            majorTickY = this.ScalePosition.Y + (actualScaleLineThickness / 2) -
                                (this.MajorTickStyle.Length / 2);
                            this.majorTicksLayoutPosition = new Point(ScalePosition.X, majorTickY);

                            minorTicky = this.ScalePosition.Y + (actualScaleLineThickness / 2) -
                                (this.MinorTickStyle.Length / 2);
                            this.minorTicksLayoutPosition = new Point(ScalePosition.X, minorTicky);

                            var labelYPos = this.ScalePosition.Y + actualLabelOffset;
                            labelYPos += maximumTickLength > actualScaleLineThickness ?
                                (actualScaleLineThickness / 2) + (maximumTickLength / 2) :
                                actualScaleLineThickness;
                            this.labelsLayoutPosition = new Point(ScalePosition.X, labelYPos);
                            break;
                        case GaugeLabelsPosition.Outside:
                            y = (actualScaleLineThickness < maximumTickLength ? (maximumTickLength - actualScaleLineThickness) / 2 : 0d) + actualLabelOffset + labelMaximumSize;
                            this.ScalePosition = new Point(x, y > outsideScaleHeight ? y : outsideScaleHeight);

                            majorTickY = this.ScalePosition.Y + (actualScaleLineThickness / 2) -
                                (this.MajorTickStyle.Length / 2);
                            this.majorTicksLayoutPosition = new Point(ScalePosition.X, majorTickY);

                            minorTicky = this.ScalePosition.Y + (actualScaleLineThickness / 2) -
                               (this.MinorTickStyle.Length / 2);
                            this.minorTicksLayoutPosition = new Point(ScalePosition.X, minorTicky);

                            labelYPos = this.ScalePosition.Y - actualLabelOffset - labelMaximumSize;
                            labelYPos -= maximumTickLength > actualScaleLineThickness ?
                                (actualScaleLineThickness / 2) + (maximumTickLength / 2) :
                                actualScaleLineThickness;
                            labelYPos = labelYPos < 0d ? 0d : labelYPos;
                            this.labelsLayoutPosition = new Point(ScalePosition.X, labelYPos);
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
                this.CalculateScaleLine();
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
                        Offset = (float)this.ValueToFactor(this.ActualMaximum, startValue, endValue)
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
                                Offset = (float)this.ValueToFactor(gradientStopsList[i].ActualValue, startValue, endValue)
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
                                    Offset = (float)this.ValueToFactor(this.ActualMaximum, startValue, endValue)
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
        /// Converts value to factor value.
        /// </summary>
        /// <param name="value">The value to convert as factor.</param>
        /// <param name="minimum">The minimum value.</param>
        /// <param name="maximum">The maximum value.</param>
        /// <returns>Factor of the provided value.</returns>
        private double ValueToFactor(double value, double minimum, double maximum)
        {
            double factor = (value - minimum) / (maximum - minimum);

            if (this.Orientation == GaugeOrientation.Horizontal)
            {
                return this.IsInversed ? 1d - factor : factor;
            }

            return !this.IsInversed ? 1d - factor : factor;
        }

        #region Ranges collection changed

        /// <summary>
        /// Called when <see cref="Ranges"/> collection got changed.
        /// </summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="e">The NotifyCollectionChangedEventArgs.</param>
        private void Ranges_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            e.ApplyCollectionChanges((obj, index, _) => AddRange(obj, index), (obj, index) => RemoveRange(obj, index), ResetRanges);
        }

        /// <summary>
        /// Add/insert the axis to the ranges collection. 
        /// </summary>
        /// <param name="range"></param>
        /// <param name="index"></param>
        private void AddRange(object range, int index)
        {
            if (range is LinearRange linearRange)
            {
                linearRange.Scale = this;

                if (!this.rangeLayout.Contains(linearRange.RangeView))
                    this.rangeLayout.Insert(index, linearRange.RangeView);

                if (linearRange.Child != null)
                {
                    linearRange.Child.BindingContext = linearRange;
                    this.RangeChildUpdate(null, linearRange.Child);
                }

                SetInheritedBindingContext(linearRange, this.BindingContext);

                if (this.rangeLayout.Children.Count > 0 && !this.parentLayout.Children.Contains(rangeLayout))
                {
                    this.parentLayout.Children.Add(rangeLayout);
                }

                this.ScaleInvalidateMeasureOverride();
            }
        }

        /// <summary>
        /// Remove the range from the ranges collection. 
        /// </summary>
        /// <param name="range"></param>
        /// <param name="index"></param>
        private void RemoveRange(object range, int index)
        {
            if (range is LinearRange linearRange)
            {
                linearRange.Scale = null;

                if (linearRange.Child != null)
                    this.RangeChildUpdate(linearRange.Child, null);

                if (this.rangeLayout.Children.Contains(linearRange.RangeView))
                {
                    this.rangeLayout.Children.RemoveAt(index);
                }

                this.ScaleInvalidateMeasureOverride();
            }
        }

        /// <summary>
        /// Clear the ranges collection. 
        /// </summary>
        private void ResetRanges()
        {
            this.rangeLayout.Children.Clear();
            this.ScaleInvalidateMeasureOverride();
        }

        #endregion

        #region BarPointers collection changed

        /// <summary>
        /// Called when <see cref="BarPointer"/> collection got changed.
        /// </summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="e">The NotifyCollectionChangedEventArgs.</param>
        private void BarPointers_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            e.ApplyCollectionChanges((obj, index, _) => AddBarPointer(obj, index), (obj, index) => RemoveBarPointer(obj, index), ResetBarPointers);
        }

        /// <summary>
        /// Add/insert the axis to the bar pointers collection. 
        /// </summary>
        /// <param name="pointer"></param>
        /// <param name="index"></param>
        private void AddBarPointer(object pointer, int index)
        {
            if (pointer is BarPointer barPointer)
            {
                barPointer.Scale = this;
                barPointer.CanAnimate = true;

                if (!this.barPointersLayout.Contains(barPointer.PointerView))
                    this.barPointersLayout.Insert(index, barPointer.PointerView);

                if (barPointer.Child != null)
                {
                    barPointer.Child.BindingContext = barPointer;
                    this.BarPointerChildUpdate(null, barPointer.Child);
                }

                SetInheritedBindingContext(barPointer, this.BindingContext);

                if (this.barPointersLayout.Children.Count > 0 && !this.parentLayout.Children.Contains(barPointersLayout))
                {
                    this.parentLayout.Children.Add(barPointersLayout);
                }

                this.ScaleInvalidateMeasureOverride();
            }
        }

        /// <summary>
        /// Remove the range from the bar pointers collection. 
        /// </summary>
        /// <param name="pointer"></param>
        /// <param name="index"></param>
        private void RemoveBarPointer(object pointer, int index)
        {
            if (pointer is BarPointer barPointer)
            {
                barPointer.Scale = null;

                if (this.barPointersLayout.Children.Contains(barPointer.PointerView))
                {
                    this.barPointersLayout.Children.RemoveAt(index);
                }

                if (barPointer.Child != null)
                    this.BarPointerChildUpdate(barPointer.Child, null);

                this.ScaleInvalidateMeasureOverride();
            }
        }

        /// <summary>
        /// Clear the bar pointers collection. 
        /// </summary>
        private void ResetBarPointers()
        {
            this.barPointersLayout.Children.Clear();
            this.ScaleInvalidateMeasureOverride();
        }

        #endregion

        #region MarkerPointers collection changed

        /// <summary>
        /// Called when <see cref="MarkerPointers"/> collection got changed.
        /// </summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="e">The NotifyCollectionChangedEventArgs.</param>
        private void MarkerPointers_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            e.ApplyCollectionChanges((obj, index, _) => AddMarkerPointer(obj, index), (obj, index) => RemoveMarkerPointer(obj, index), ResetMarkerPointers);
        }

        /// <summary>
        /// Add/insert the axis to the marker pointers collection. 
        /// </summary>
        /// <param name="pointer"></param>
        /// <param name="index"></param>
        private void AddMarkerPointer(object pointer, int index)
        {
            if (pointer is LinearMarkerPointer markerPointer)
            {
                markerPointer.Scale = this;
                markerPointer.CanAnimate = true;

                if (markerPointer is ContentPointer contentPointer && contentPointer.Content != null)
                {
                    contentPointer.Content.BindingContext = contentPointer;
                    this.ShapePointerChildUpdate(null, contentPointer.Content);
                }

                if (!this.shapePointersLayout.Contains(markerPointer.PointerView))
                    this.shapePointersLayout.Insert(index, markerPointer.PointerView);

                if (!this.ScaleAvailableSize.IsZero)
                {
                    SetInheritedBindingContext(markerPointer, this.BindingContext);
                    markerPointer.CreatePointer();
                }

                if (this.shapePointersLayout.Children.Count > 0 && !this.parentLayout.Children.Contains(shapePointersLayout))
                {
                    this.parentLayout.Children.Add(shapePointersLayout);
                }

                this.ScaleInvalidateMeasureOverride();
            }
        }

        /// <summary>
        /// Remove the range from the marker pointers collection. 
        /// </summary>
        /// <param name="pointer"></param>
        /// <param name="index"></param>
        private void RemoveMarkerPointer(object pointer, int index)
        {
            if (pointer is LinearMarkerPointer markerPointer)
            {
                markerPointer.Scale = null;

                if (this.shapePointersLayout.Children.Contains(markerPointer.PointerView))
                {
                    this.shapePointersLayout.Children.RemoveAt(index);
                }

                if (markerPointer is ContentPointer contentPointer && contentPointer.Content != null)
                    this.ShapePointerChildUpdate(contentPointer.Content, null);

                this.ScaleInvalidateMeasureOverride();
            }
        }

        /// <summary>
        /// Clear the bar pointers collection. 
        /// </summary>
        private void ResetMarkerPointers()
        {
            this.shapePointersLayout.Children.Clear();
            this.ScaleInvalidateMeasureOverride();
        }

        #endregion

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
            this.UpdateScale();

            if ((this.Orientation == GaugeOrientation.Horizontal && this.HeightRequest == -1)
                || (this.Orientation == GaugeOrientation.Vertical && this.WidthRequest == -1))
            {
                this.ScaleAvailableSize = this.GetScaleSize();
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
        /// Annotation content collection added in the visual tree elements for hot reload case. 
        /// </summary>
        /// <returns></returns>
        IReadOnlyList<IVisualTreeElement> IVisualTreeElement.GetVisualChildren()
        {
            if (this.parentLayout != null)
            {
                return new List<IVisualTreeElement>() { parentLayout };
            }
            return new List<IVisualTreeElement>();
        }

        /// <summary>
        /// To get the scale size based on its children's size and positions.
        /// </summary>
        /// <returns>The size to be rendered for scale along with its elements.</returns>
        private Size GetScaleSize()
        {
            double maximumTickLength = this.GetActualMaxTickLength();
            double actualTickOffset = this.GetActualTickOffset();
            double labelMaximumSize = this.GetLabelMaxLength();
            double actualLabelOffset = this.GetActualLabelOffset();
            double actualScaleLineThickness = this.GetActualScaleLineThickness();
            GaugeLabelsPosition actualLabelPosition = this.GetActualLabelPosition();
            GaugeElementPosition actualTickPosition = this.GetActualElementPosition(this.TickPosition);
            double scaleHeight, outsideRangeHeight = 0d, insideRangeHeight = 0d;
            this.GetRangeHeights(ref outsideRangeHeight, ref insideRangeHeight);
            double outsideBarPointerHeight = 0d, insideBarPointerHeight = 0d;
            this.GetBarPointersHeight(ref outsideBarPointerHeight, ref insideBarPointerHeight);
            double outsideMarkerPointerHeight = 0d, insideMarkerPointerHeight = 0d;
            this.GetMarkerPointersHeight(ref outsideMarkerPointerHeight, ref insideMarkerPointerHeight);
            double outsideScaleHeight = Math.Max(Math.Max(outsideRangeHeight, outsideBarPointerHeight), outsideMarkerPointerHeight);
            double insideScaleHeight = Math.Max(Math.Max(insideRangeHeight, insideBarPointerHeight), insideMarkerPointerHeight);
            double labelsLayoutHeight = labelMaximumSize + actualLabelOffset;
            double ticksLayoutHeight = maximumTickLength + actualTickOffset;
            double tickAndLabelHeight = labelsLayoutHeight + ticksLayoutHeight;

            if (actualTickPosition == GaugeElementPosition.Cross)
            {
                if (actualScaleLineThickness < maximumTickLength)
                {
                    double ScaleTop = (maximumTickLength - actualScaleLineThickness) / 2;
                    double diffHeight = ScaleTop;
                    if (actualLabelPosition == GaugeLabelsPosition.Outside)
                    {
                        ScaleTop += labelsLayoutHeight;
                        ScaleTop = outsideScaleHeight > ScaleTop ? outsideScaleHeight : ScaleTop;
                        scaleHeight = actualScaleLineThickness + ScaleTop;
                        scaleHeight += diffHeight > insideScaleHeight ? diffHeight : insideScaleHeight;
                    }
                    else
                    {
                        ScaleTop = outsideScaleHeight > ScaleTop ? outsideScaleHeight : ScaleTop;
                        scaleHeight = actualScaleLineThickness + ScaleTop;
                        scaleHeight += insideScaleHeight < (diffHeight + labelsLayoutHeight) ? (diffHeight + labelsLayoutHeight) : insideScaleHeight;
                    }
                }
                else
                {
                    if (actualLabelPosition == GaugeLabelsPosition.Outside)
                    {
                        scaleHeight = labelsLayoutHeight > outsideScaleHeight ? labelsLayoutHeight : outsideScaleHeight;
                        scaleHeight += actualScaleLineThickness + insideScaleHeight;
                    }
                    else
                    {
                        scaleHeight = labelsLayoutHeight > insideScaleHeight ? labelsLayoutHeight : insideScaleHeight;
                        scaleHeight += actualScaleLineThickness + outsideScaleHeight;
                    }
                }
            }
            else
            {
                if (actualLabelPosition == GaugeLabelsPosition.Inside && actualTickPosition == GaugeElementPosition.Inside)
                {
                    scaleHeight = actualScaleLineThickness + outsideScaleHeight;
                    scaleHeight += insideScaleHeight > tickAndLabelHeight ? insideScaleHeight : tickAndLabelHeight;
                }
                else if (actualLabelPosition == GaugeLabelsPosition.Outside && actualTickPosition == GaugeElementPosition.Outside)
                {
                    scaleHeight = actualScaleLineThickness + insideScaleHeight;
                    scaleHeight += outsideScaleHeight > tickAndLabelHeight ? outsideScaleHeight : tickAndLabelHeight;
                }
                else
                {
                    scaleHeight = actualScaleLineThickness;
                    if (actualLabelPosition == GaugeLabelsPosition.Outside)
                    {
                        scaleHeight += outsideScaleHeight < labelsLayoutHeight ? labelsLayoutHeight : outsideScaleHeight;
                    }

                    if (actualLabelPosition == GaugeLabelsPosition.Inside)
                    {
                        scaleHeight += insideScaleHeight < labelsLayoutHeight ? labelsLayoutHeight : insideScaleHeight;
                    }

                    if (actualTickPosition == GaugeElementPosition.Outside)
                    {
                        scaleHeight += outsideScaleHeight < ticksLayoutHeight ? ticksLayoutHeight : outsideScaleHeight;
                    }

                    if (actualTickPosition == GaugeElementPosition.Inside)
                    {
                        scaleHeight += insideScaleHeight < ticksLayoutHeight ? ticksLayoutHeight : insideScaleHeight;
                    }
                }
            }

            return this.Orientation == GaugeOrientation.Horizontal
                ? new Size(this.ScaleAvailableSize.Width, scaleHeight)
                : new Size(scaleHeight, this.ScaleAvailableSize.Height);
        }



        /// <summary>
        /// To get the range heights based on positions.
        /// </summary>
        /// <param name="outsideRangeHeight">Outside positioned scale height.</param>
        /// <param name="insideRangeHeight">Inside positioned scale height.</param>
        private void GetRangeHeights(ref double outsideRangeHeight, ref double insideRangeHeight)
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

            double actualScaleLineThickness = this.GetActualScaleLineThickness();
            if (fillRangeHeight > actualScaleLineThickness)
            {
                double diffWidth = (fillRangeHeight - actualScaleLineThickness) / 2;
                insideRangeHeight = Math.Max(diffWidth, insideRangeHeight);
                outsideRangeHeight = Math.Max(diffWidth, outsideRangeHeight);
            }

            if (this.IsMirrored)
            {
                Utility.Swap(ref insideRangeHeight, ref outsideRangeHeight);
            }
        }

        /// <summary>
        /// To get the bar pointers heights based on offset positions.
        /// </summary>
        /// <param name="outsidePointerHeight">Outside positioned scale height.</param>
        /// <param name="insidePointerHeight">Inside positioned scale height.</param>
        private void GetBarPointersHeight(ref double outsidePointerHeight, ref double insidePointerHeight)
        {
            double fillPointerHeight = 0d;
            double actualScaleLineThickness = this.GetActualScaleLineThickness();
            foreach (BarPointer barPointer in this.BarPointers)
            {
                if (barPointer.Offset == 0)
                {
                    fillPointerHeight = Math.Max(barPointer.PointerSize, fillPointerHeight);
                }
                else if (barPointer.Offset > 0)
                {
                    insidePointerHeight = Math.Max((barPointer.PointerSize / 2) + barPointer.Offset - (actualScaleLineThickness / 2), insidePointerHeight);
                    if (barPointer.PointerSize > actualScaleLineThickness)
                    {
                        if (barPointer.Offset < (barPointer.PointerSize - actualScaleLineThickness))
                        {
                            outsidePointerHeight = Math.Max(((barPointer.PointerSize - actualScaleLineThickness) / 2) - barPointer.Offset, outsidePointerHeight);
                        }
                    }
                }
                else if (barPointer.Offset < 0)
                {
                    outsidePointerHeight = Math.Max((barPointer.PointerSize / 2) + Math.Abs(barPointer.Offset) - (actualScaleLineThickness / 2), outsidePointerHeight);
                }
            }

            if (fillPointerHeight > actualScaleLineThickness)
            {
                double diffWidth = (fillPointerHeight - actualScaleLineThickness) / 2;
                insidePointerHeight = Math.Max(diffWidth, insidePointerHeight);
                outsidePointerHeight = Math.Max(diffWidth, outsidePointerHeight);
            }

            if (this.IsMirrored)
            {
                Utility.Swap(ref insidePointerHeight, ref outsidePointerHeight);
            }
        }

        /// <summary>
        /// To get the marker pointers heights based on offset positions.
        /// </summary>
        /// <param name="outsidePointerHeight">Outside positioned axis height.</param>
        /// <param name="insidePointerHeight">Inside positioned axis height.</param>
        internal void GetMarkerPointersHeight(ref double outsidePointerHeight, ref double insidePointerHeight)
        {
            double actualAxisLineThickness = this.GetActualScaleLineThickness();

            foreach (LinearMarkerPointer markerPointer in this.MarkerPointers)
            {
                double pointerOffset = this.Orientation == GaugeOrientation.Horizontal
                    ? markerPointer.OffsetPoint.Y : markerPointer.OffsetPoint.X;
                GaugeAlignment pointerPosition = this.Orientation == GaugeOrientation.Horizontal
                    ? markerPointer.VerticalAlignment : markerPointer.HorizontalAlignment;
                double markerSize = 0d;

                if (markerPointer is ShapePointer shapePointer)
                {
                    markerSize = this.Orientation == GaugeOrientation.Horizontal
                        ? shapePointer.ShapeHeight
                        : shapePointer.ShapeWidth;
                }
                else if (markerPointer is ContentPointer contentPointer)
                {
                    if (contentPointer.Content != null)
                    {
                        markerSize = this.Orientation == GaugeOrientation.Horizontal
                                        ? contentPointer.Content.DesiredSize.Height
                                        : contentPointer.Content.DesiredSize.Width;
                    }
                }

                if (pointerOffset == 0)
                {
                    markerPointer.MarkerPointersHeightWithoutOffset(ref outsidePointerHeight, ref insidePointerHeight, actualAxisLineThickness, pointerPosition, markerSize);
                }
                else if (pointerOffset > 0)
                {
                    markerPointer.MarkerPointerHeightWithPositiveOffset(ref outsidePointerHeight, ref insidePointerHeight, actualAxisLineThickness, pointerOffset, pointerPosition, markerSize);
                }
                else if (pointerOffset < 0)
                {
                    markerPointer.MarkerPointersHeightWithNegativeOffset(ref outsidePointerHeight, ref insidePointerHeight, actualAxisLineThickness, pointerOffset, pointerPosition, markerSize);
                }
            }

            if (this.IsMirrored)
                Utility.Swap(ref insidePointerHeight, ref outsidePointerHeight);
        }

        #endregion

        #endregion
    }
}
