using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using Syncfusion.Maui.Graphics.Internals;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;

namespace Syncfusion.Maui.Gauges
{
    /// <summary>
    /// Create the range to add color bar in the gauge.
    /// <see cref="LinearRange"/> is a visual that helps to quickly visualize
    /// where a value falls on the scale.
    /// </summary>
    public class LinearRange : BindableObject
    {
        #region Bindable properties

        /// <summary>
        /// Identifies the <see cref="StartValue"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="StartValue"/> bindable property.
        /// </value>
        public static readonly BindableProperty StartValueProperty =
            BindableProperty.Create(nameof(StartValue), typeof(double), typeof(LinearRange), 0d, propertyChanged: OnStartEndValueChanged);

        /// <summary>
        /// Identifies the <see cref="EndValue"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="EndValue"/> bindable property.
        /// </value>
        public static readonly BindableProperty EndValueProperty =
            BindableProperty.Create(nameof(EndValue), typeof(double), typeof(LinearRange), 0d, propertyChanged: OnStartEndValueChanged);

        /// <summary>
        /// Identifies the <see cref="StartWidth"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="StartWidth"/> bindable property.
        /// </value>
        public static readonly BindableProperty StartWidthProperty =
            BindableProperty.Create(nameof(StartWidth), typeof(double), typeof(LinearRange), 10d, propertyChanged: OnPropertyChanged);

        /// <summary>
        /// Identifies the <see cref="MidWidth"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="MidWidth"/> bindable property.
        /// </value>
        public static readonly BindableProperty MidWidthProperty =
            BindableProperty.Create(nameof(MidWidth), typeof(double), typeof(LinearRange), double.NaN, propertyChanged: OnPropertyChanged);

        /// <summary>
        /// Identifies the <see cref="EndWidth"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="EndWidth"/> bindable property.
        /// </value>
        public static readonly BindableProperty EndWidthProperty =
            BindableProperty.Create(nameof(EndWidth), typeof(double), typeof(LinearRange), 10d, propertyChanged: OnPropertyChanged);

        /// <summary>
        /// Identifies the <see cref="RangePosition"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="RangePosition"/> bindable property.
        /// </value>
        public static readonly BindableProperty RangePositionProperty =
            BindableProperty.Create(nameof(RangePosition), typeof(GaugeElementPosition), typeof(LinearRange), GaugeElementPosition.Outside, propertyChanged: OnPropertyChanged);

        /// <summary>
        /// Identifies the <see cref="Fill"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="Fill"/> bindable property.
        /// </value>
        public static readonly BindableProperty FillProperty =
            BindableProperty.Create(nameof(Fill), typeof(Brush), typeof(LinearRange), null, defaultValueCreator: bindable => new SolidColorBrush(Color.FromRgb(67, 160, 71)), propertyChanged: OnFillPropertyChanged);

        /// <summary>
        /// Identifies the <see cref="GradientStops"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="GradientStops"/> bindable property.
        /// </value>
        public static readonly BindableProperty GradientStopsProperty =
         BindableProperty.Create(nameof(GradientStops), typeof(ObservableCollection<GaugeGradientStop>), typeof(LinearRange), null, propertyChanged: OnGradientStopsPropertyChanged);

        /// <summary>
        /// Identifies the <see cref="Child"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="Child"/> bindable property.
        /// </value>
        public static readonly BindableProperty ChildProperty =
          BindableProperty.Create(nameof(Child), typeof(View), typeof(LinearRange), null, propertyChanged: OnChildPropertyChanged);

        #endregion

        #region Fields

        private PathF? rangePath;
        private LinearGradientBrush? linearGradientBrush;

        internal LinearRangeView RangeView;
        internal SfLinearGauge? Scale;
        internal double ActualStartValue, ActualEndValue;
        
        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="LinearRange"/> class.
        /// </summary>
        public LinearRange()
        {
            this.RangeView = new LinearRangeView(this);
            this.GradientStops = new ObservableCollection<GaugeGradientStop>();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the value that specifies the range start value.
        /// </summary>
        /// <value>
        /// It defines the start value of the range. The default value is <c>0</c>.
        /// </value>
        public double StartValue
        {
            get { return (double)this.GetValue(StartValueProperty); }
            set { this.SetValue(StartValueProperty, value); }
        }

        /// <summary>
        /// Gets or sets the value that specifies the range end value.
        /// </summary>
        /// <value>
        /// It defines the end value of the range. The default value is <c>0</c>.
        /// </value>
        public double EndValue
        {
            get { return (double)this.GetValue(EndValueProperty); }
            set { this.SetValue(EndValueProperty, value); }
        }

        /// <summary>
        /// Gets or sets the value that specifies the range start width. Specify the value in logical pixel.
        /// </summary>
        /// <value>
        /// It defines the start width of the range. The default value is <c>10</c>.
        /// </value>
        public double StartWidth
        {
            get { return (double)this.GetValue(StartWidthProperty); }
            set { this.SetValue(StartWidthProperty, value); }
        }

        /// <summary>
        /// Gets or sets the value that specifies the range mid width. Specify the value in logical pixel.
        /// </summary>
        /// <value>
        /// It defines the end width of the range. The default value is <c>10</c>.
        /// </value>
        public double MidWidth
        {
            get { return (double)this.GetValue(MidWidthProperty); }
            set { this.SetValue(MidWidthProperty, value); }
        }

        /// <summary>
        /// Gets or sets the value that specifies the range start width. Specify the value in logical pixel.
        /// </summary>
        /// <value>
        /// It defines the end width of the range. The default value is <c>10</c>.
        /// </value>
        public double EndWidth
        {
            get { return (double)this.GetValue(EndWidthProperty); }
            set { this.SetValue(EndWidthProperty, value); }
        }

        /// <summary>
        /// Gets or sets the value that specifies the range position. Specify the value in <see cref="GaugeElementPosition"/>.
        /// </summary>
        /// <value>
        /// The default value is <see cref="GaugeElementPosition.Outside"/>.
        /// </value>
        public GaugeElementPosition RangePosition
        {
            get { return (GaugeElementPosition)this.GetValue(RangePositionProperty); }
            set { this.SetValue(RangePositionProperty, value); }
        }

        /// <summary>
        /// Gets or sets the Color that paints the interior area of the range.
        /// </summary>
        /// <value>
        /// A <c>Color</c> that specifies how the range is painted.
        /// </value>
        public Brush Fill
        {
            get { return (Brush)this.GetValue(FillProperty); }
            set { this.SetValue(FillProperty, value); }
        }

        /// <summary>
        /// Gets or sets a collection of <see cref="GaugeGradientStop"/> to fill the gradient brush to the gauge range.
        /// </summary>
        /// <value>
        /// A collection of the <see cref="GaugeGradientStop"/> objects associated with the brush, each of which specifies a color and an offset along the scale.
        /// The default is an empty collection.
        /// </value>
        public ObservableCollection<GaugeGradientStop> GradientStops
        {
            get { return (ObservableCollection<GaugeGradientStop>)this.GetValue(GradientStopsProperty); }
            set { this.SetValue(GradientStopsProperty, value); }
        }

        /// <summary>
        /// Gets or sets the child content of a <see cref="BarPointer"/>. 
        /// </summary>
        /// <value>
        /// An object that contains the pointer's visual child content. The default value is <c>null</c>.
        /// </value>
        public View Child
        {
            get { return (View)this.GetValue(ChildProperty); }
            set { this.SetValue(ChildProperty, value); }
        }

        #endregion

        #region Override methods

        /// <summary>
        /// Invoked whenever the binding context of the View changes.
        /// </summary>
        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();

            if(this.GradientStops != null)
            {
                foreach (var gradientStop in this.GradientStops)
                    SetInheritedBindingContext(gradientStop, BindingContext);
            }
        }

        #endregion

        #region Internal methods

        /// <summary>
        /// To create range arc. 
        /// </summary>
        internal void CreateRangePath()
        {
            if (this.Scale == null || this.Scale.ScaleAvailableSize.IsZero)
            {
                return;
            }

            double maxRangeWidth = double.IsNaN(this.MidWidth) ? Math.Max(this.StartWidth, this.EndWidth) :
                Math.Max(Math.Max(this.StartWidth, this.MidWidth), this.EndWidth);

            ActualStartValue = Math.Clamp(this.StartValue, this.Scale.ActualMinimum, this.Scale.ActualMaximum);
            ActualEndValue = Math.Clamp(this.EndValue, this.Scale.ActualMinimum, this.Scale.ActualMaximum);
            Utility.ValidateMinimumMaximumValue(ref ActualStartValue, ref ActualEndValue);
            double actualEndWidth = this.EndWidth > 0 ? this.EndWidth : 0d;
            double actualStartWidth = this.StartWidth > 0 ? this.StartWidth : 0d;
            double actualMidWidth = 0d;
            if (!double.IsNaN(this.MidWidth))
            {
                actualMidWidth = this.MidWidth > 0 ? this.MidWidth : 0d;
            }
            double lineThickness = this.Scale.GetActualScaleLineThickness();
            double rangeStartPosition = this.Scale.GetPositionFromValue(ActualStartValue);
            double rangeEndPosition = this.Scale.GetPositionFromValue(ActualEndValue);
            double rangeMidPosition = (rangeStartPosition + rangeEndPosition) / 2;
            double scaleLinePositionX = this.Scale.ScalePosition.X;
            double scaleLinePositionY = this.Scale.ScalePosition.Y;
            rangePath = new PathF();

            switch (this.Scale.GetActualElementPosition(this.RangePosition))
            {
                case GaugeElementPosition.Outside:
                    this.Scale.MoveToPath(rangePath, scaleLinePositionX + rangeStartPosition, scaleLinePositionY);
                    this.Scale.LineToPath(rangePath, scaleLinePositionX + rangeEndPosition, scaleLinePositionY);
                    this.Scale.LineToPath(rangePath, scaleLinePositionX + rangeEndPosition, scaleLinePositionY - actualEndWidth);
                    if (!double.IsNaN(this.MidWidth))
                    {
                        this.Scale.LineToPath(rangePath, scaleLinePositionX + rangeMidPosition, scaleLinePositionY - actualMidWidth);
                    }
                    this.Scale.LineToPath(rangePath, scaleLinePositionX + rangeStartPosition, scaleLinePositionY - actualStartWidth);
                    break;
                case GaugeElementPosition.Cross:
                    if (this.Scale.IsMirrored)
                    {
                        this.Scale.MoveToPath(rangePath, scaleLinePositionX + rangeStartPosition, scaleLinePositionY + (lineThickness / 2) - (maxRangeWidth / 2));
                        this.Scale.LineToPath(rangePath, scaleLinePositionX + rangeEndPosition, scaleLinePositionY + (lineThickness / 2) - (maxRangeWidth / 2));
                        this.Scale.LineToPath(rangePath, scaleLinePositionX + rangeEndPosition, scaleLinePositionY + (lineThickness / 2) - (maxRangeWidth / 2) + actualEndWidth);
                        if (!double.IsNaN(this.MidWidth))
                        {
                            this.Scale.LineToPath(rangePath, scaleLinePositionX + rangeMidPosition, scaleLinePositionY + (lineThickness / 2) - (maxRangeWidth / 2) + actualMidWidth);
                        }
                        this.Scale.LineToPath(rangePath, scaleLinePositionX + rangeStartPosition, scaleLinePositionY + (lineThickness / 2) - (maxRangeWidth / 2) + actualStartWidth);
                    }
                    else
                    {
                        this.Scale.MoveToPath(rangePath, scaleLinePositionX + rangeStartPosition, scaleLinePositionY + (lineThickness / 2) + (maxRangeWidth / 2) - actualStartWidth);
                        if (!double.IsNaN(this.MidWidth))
                        {
                            this.Scale.LineToPath(rangePath, scaleLinePositionX + rangeMidPosition, scaleLinePositionY + (lineThickness / 2) + (maxRangeWidth / 2) - actualMidWidth);
                        }
                        this.Scale.LineToPath(rangePath, scaleLinePositionX + rangeEndPosition, scaleLinePositionY + (lineThickness / 2) + (maxRangeWidth / 2) - actualEndWidth);
                        this.Scale.LineToPath(rangePath, scaleLinePositionX + rangeEndPosition, scaleLinePositionY + (lineThickness / 2) + (maxRangeWidth / 2));
                        this.Scale.LineToPath(rangePath, scaleLinePositionX + rangeStartPosition, scaleLinePositionY + (lineThickness / 2) + (maxRangeWidth / 2));
                    }
                    break;
                case GaugeElementPosition.Inside:
                    this.Scale.MoveToPath(rangePath, scaleLinePositionX + rangeStartPosition, scaleLinePositionY + lineThickness);
                    this.Scale.LineToPath(rangePath, scaleLinePositionX + rangeEndPosition, scaleLinePositionY + lineThickness);
                    this.Scale.LineToPath(rangePath, scaleLinePositionX + rangeEndPosition, scaleLinePositionY + lineThickness + actualEndWidth);
                    if (!double.IsNaN(this.MidWidth))
                    {
                        this.Scale.LineToPath(rangePath, scaleLinePositionX + rangeMidPosition, scaleLinePositionY + lineThickness + actualMidWidth);
                    }
                    this.Scale.LineToPath(rangePath, scaleLinePositionX + rangeStartPosition, scaleLinePositionY + lineThickness + actualStartWidth);
                    break;
            }
            rangePath.Close();

            CreateGradient();

            //Arrange child.
            if (this.Child != null)
            {
                Scale.UpdateChild(this.Child, rangePath.Bounds);
            }
        }

        private void CreateGradient()
        {
            if (this.Scale != null && this.GradientStops != null)
            {
                linearGradientBrush = this.Scale.GetLinearGradient(this.GradientStops, ActualStartValue, ActualEndValue);
            }
            else
                linearGradientBrush = null;
        }

        /// <summary>
        /// Draw Range Arc.
        /// </summary>
        /// <param name="canvas">canvas</param>
        internal void DrawRange(ICanvas canvas)
        {
            if (this.Scale != null)
            {
                canvas.SaveState();

                if (this.rangePath != null)
                {
                    if (this.linearGradientBrush != null)
                        canvas.SetFillPaint(this.linearGradientBrush, rangePath.Bounds);
                    else
                        canvas.SetFillPaint(this.Fill, rangePath.Bounds);

                    canvas.FillPath(this.rangePath);
                }

                canvas.RestoreState();
            }
        }

        /// <summary>
        /// Invalidate range view.
        /// </summary>
        internal void InvalidateDrawable()
        {
            if (this.RangeView != null)
                this.RangeView.InvalidateDrawable();
        }

        #endregion

        #region Property changed

        /// <summary>
        /// Called when <see cref="StartValue"/> or <see cref="EndValue"/> changed
        /// </summary>
        /// <param name="bindable">The BindableObject.</param>
        /// <param name="oldValue">Old value.</param>
        /// <param name="newValue">New value.</param>
        private static void OnStartEndValueChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is LinearRange linearRange)
            {
                linearRange.CreateRangePath();
                if (linearRange.Scale != null && linearRange.Scale.UseRangeColorForAxis)
                {
                    linearRange.Scale.InvalidateDrawable();
                }
                linearRange.InvalidateDrawable();
            }
        }

        /// <summary>
        /// Called when <see cref="StartWidth"/> or <see cref="MidWidth"/> or <see cref="EndWidth"/> or changed.
        /// </summary>
        /// <param name="bindable">The BindableObject.</param>
        /// <param name="oldValue">Old value.</param>
        /// <param name="newValue">New value.</param>
        private static void OnPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is LinearRange linearRange && linearRange.Scale != null)
            {
                linearRange.Scale.ScaleInvalidateMeasureOverride();
            }
        }

        /// <summary>
        /// Called when <see cref="Fill"/> changed.
        /// </summary>
        /// <param name="bindable">The BindableObject.</param>
        /// <param name="oldValue">Old value.</param>
        /// <param name="newValue">New value.</param>
        private static void OnFillPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is LinearRange linearRange)
            {
                if (linearRange.Scale != null)
                {
                    linearRange.InvalidateDrawable();
                }
            }
        }

        /// <summary>
        /// Called when bar pointer <see cref="Child"/> changed.
        /// </summary>
        /// <param name="bindable">The BindableObject.</param>
        /// <param name="oldValue">Old value.</param>
        /// <param name="newValue">New value.</param>
        private static void OnChildPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is LinearRange linearRange)
            {
                if (linearRange.Scale != null)
                {
                    linearRange.Scale.RangeChildUpdate(oldValue, newValue);

                    if (newValue is View newChild)
                    {
                        newChild.BindingContext = linearRange;

                        if (linearRange.rangePath != null)
                            linearRange.Scale.UpdateChild(newChild, linearRange.rangePath.Bounds);
                    }
                }
            }
        }

#nullable disable
        /// <summary>
        /// Called when <see cref="GradientStops"/> property changed.
        /// </summary>
        /// <param name="bindable">The BindableObject.</param>
        /// <param name="oldValue">Old value.</param>
        /// <param name="newValue">New value.</param>
        private static void OnGradientStopsPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is LinearRange linearRange)
            {
                if (oldValue != null)
                {
                    if (oldValue is ObservableCollection<GaugeGradientStop> gradientStops)
                        gradientStops.CollectionChanged -= linearRange.GradientStops_CollectionChanged;
                }

                if (newValue != null)
                {
                    if (newValue is ObservableCollection<GaugeGradientStop> gradientStops)
                        gradientStops.CollectionChanged += linearRange.GradientStops_CollectionChanged;
                }

                linearRange.CreateGradient();
                linearRange.InvalidateDrawable();
            }
        }
#nullable enable

        #endregion

        #region Private methods
        
        /// <summary>
        /// Called when <see cref="GradientStops"/> collection got changed.
        /// </summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="e">The NotifyCollectionChangedEventArgs.</param>
        private void GradientStops_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            this.CreateGradient();
            this.InvalidateDrawable();
        }

        #endregion
    }
}
