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
            BindableProperty.Create(nameof(EndValue), typeof(double), typeof(LinearRange), 100d, propertyChanged: OnStartEndValueChanged);

        /// <summary>
        /// Identifies the <see cref="StartWidth"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="StartWidth"/> bindable property.
        /// </value>
        public static readonly BindableProperty StartWidthProperty =
            BindableProperty.Create(nameof(StartWidth), typeof(double), typeof(LinearRange), 5d, propertyChanged: OnPropertyChanged);

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
            BindableProperty.Create(nameof(EndWidth), typeof(double), typeof(LinearRange), 5d, propertyChanged: OnPropertyChanged);

        /// <summary>
        /// Identifies the <see cref="Position"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="Position"/> bindable property.
        /// </value>
        public static readonly BindableProperty PositionProperty =
            BindableProperty.Create(nameof(Position), typeof(GaugeElementPosition), typeof(LinearRange), GaugeElementPosition.Outside, propertyChanged: OnPropertyChanged);

        /// <summary>
        /// Identifies the <see cref="Fill"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="Fill"/> bindable property.
        /// </value>
        public static readonly BindableProperty FillProperty =
            BindableProperty.Create(nameof(Fill), typeof(Brush), typeof(LinearRange), null, defaultValueCreator: bindable => new SolidColorBrush(Color.FromRgb(244, 86, 86)), propertyChanged: OnFillPropertyChanged);

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
            AbsoluteLayout.SetLayoutBounds(this.RangeView, new Rectangle(0, 0, 1, 1));
            AbsoluteLayout.SetLayoutFlags(this.RangeView, Microsoft.Maui.Layouts.AbsoluteLayoutFlags.All);
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
        /// It defines the end width of the range. The default value is <c>double.NaN</c>.
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
        public GaugeElementPosition Position
        {
            get { return (GaugeElementPosition)this.GetValue(PositionProperty); }
            set { this.SetValue(PositionProperty, value); }
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
        /// Gets or sets the child content of a <see cref="LinearRange"/>. 
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

            if (this.Child != null)
                SetInheritedBindingContext(Child, this);
        }

        #endregion

        /// <summary>
        /// Gets the range path with mid width point drawn. 
        /// </summary>
        /// <param name="pathF"></param>
        /// <param name="startPoint"></param>
        /// <param name="midPoint"></param>
        /// <param name="endPoint"></param>
        /// <returns></returns>
        protected virtual void UpdateMidRangePath(PathF pathF, PointF startPoint, PointF midPoint, PointF endPoint)
        {
            pathF.LineTo(midPoint.X, midPoint.Y);
        }

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

            bool isAddedMidWidth = double.IsNaN(this.MidWidth);
            float maxRangeWidth = (float)(isAddedMidWidth ? Math.Max(this.StartWidth, this.EndWidth) :
                Math.Max(Math.Max(this.StartWidth, this.MidWidth), this.EndWidth));

            ActualStartValue = Math.Clamp(this.StartValue, this.Scale.ActualMinimum, this.Scale.ActualMaximum);
            ActualEndValue = Math.Clamp(this.EndValue, this.Scale.ActualMinimum, this.Scale.ActualMaximum);
            Utility.ValidateMinimumMaximumValue(ref ActualStartValue, ref ActualEndValue);
            float actualEndWidth = this.EndWidth > 0 ? (float)this.EndWidth : 0f;
            float actualStartWidth = this.StartWidth > 0 ? (float)this.StartWidth : 0f;
            float actualMidWidth = 0f;
            if (!isAddedMidWidth)
            {
                actualMidWidth = this.MidWidth > 0 ? (float)this.MidWidth : 0f;
            }
            float lineThickness = (float)this.Scale.GetActualScaleLineThickness();
            float rangeStartPosition = (float)this.Scale.GetPositionFromValue(ActualStartValue);
            float rangeEndPosition = (float)this.Scale.GetPositionFromValue(ActualEndValue);
            float rangeMidPosition = (rangeStartPosition + rangeEndPosition) / 2;
            float scaleLinePositionX = (float)this.Scale.ScalePosition.X;
            float scaleLinePositionY = (float)this.Scale.ScalePosition.Y;
            rangePath = new PathF();

            switch (this.Scale.GetActualElementPosition(this.Position))
            {
                case GaugeElementPosition.Outside:
                    this.Scale.MoveToPath(rangePath, scaleLinePositionX + rangeStartPosition, scaleLinePositionY);
                    this.Scale.LineToPath(rangePath, scaleLinePositionX + rangeEndPosition, scaleLinePositionY);
                    this.Scale.LineToPath(rangePath, scaleLinePositionX + rangeEndPosition, scaleLinePositionY - actualEndWidth);
                    if (!isAddedMidWidth)
                    {
                        this.UpdateMidRangePath(scaleLinePositionX + rangeEndPosition, scaleLinePositionY - actualEndWidth,
                            scaleLinePositionX + rangeMidPosition, scaleLinePositionY - actualMidWidth,
                            scaleLinePositionX + rangeStartPosition, scaleLinePositionY - actualStartWidth);
                    }
                    this.Scale.LineToPath(rangePath, scaleLinePositionX + rangeStartPosition, scaleLinePositionY - actualStartWidth);
                    break;
                case GaugeElementPosition.Cross:
                    if (this.Scale.IsMirrored)
                    {
                        float pathYPosition = scaleLinePositionY + (lineThickness / 2) - (maxRangeWidth / 2);
                        this.Scale.MoveToPath(rangePath, scaleLinePositionX + rangeStartPosition, pathYPosition);
                        this.Scale.LineToPath(rangePath, scaleLinePositionX + rangeEndPosition, pathYPosition);
                        this.Scale.LineToPath(rangePath, scaleLinePositionX + rangeEndPosition, pathYPosition + actualEndWidth);
                        if (!isAddedMidWidth)
                        {
                            this.UpdateMidRangePath(scaleLinePositionX + rangeEndPosition, pathYPosition + actualEndWidth,
                                scaleLinePositionX + rangeMidPosition, pathYPosition + actualMidWidth,
                                scaleLinePositionX + rangeStartPosition, pathYPosition + actualStartWidth);
                        }
                        this.Scale.LineToPath(rangePath, scaleLinePositionX + rangeStartPosition, pathYPosition + actualStartWidth);
                    }
                    else
                    {
                        float pathYPosition = scaleLinePositionY + (lineThickness / 2) + (maxRangeWidth / 2);
                        this.Scale.MoveToPath(rangePath, scaleLinePositionX + rangeStartPosition, pathYPosition - actualStartWidth);
                        if (!isAddedMidWidth)
                        {
                            this.UpdateMidRangePath(scaleLinePositionX + rangeStartPosition, pathYPosition - actualStartWidth,
                            scaleLinePositionX + rangeMidPosition, pathYPosition - actualMidWidth,
                            scaleLinePositionX + rangeEndPosition, pathYPosition - actualEndWidth);
                        }
                        this.Scale.LineToPath(rangePath, scaleLinePositionX + rangeEndPosition, pathYPosition - actualEndWidth);
                        this.Scale.LineToPath(rangePath, scaleLinePositionX + rangeEndPosition, pathYPosition);
                        this.Scale.LineToPath(rangePath, scaleLinePositionX + rangeStartPosition, pathYPosition);
                    }
                    break;
                case GaugeElementPosition.Inside:
                    float yPosition = scaleLinePositionY + lineThickness;
                    this.Scale.MoveToPath(rangePath, scaleLinePositionX + rangeStartPosition, yPosition);
                    this.Scale.LineToPath(rangePath, scaleLinePositionX + rangeEndPosition, yPosition);
                    this.Scale.LineToPath(rangePath, scaleLinePositionX + rangeEndPosition, yPosition + actualEndWidth);
                    if (!isAddedMidWidth)
                    {
                        this.UpdateMidRangePath(scaleLinePositionX + rangeEndPosition, yPosition + actualEndWidth,
                            scaleLinePositionX + rangeMidPosition, yPosition + actualMidWidth,
                            scaleLinePositionX + rangeStartPosition, yPosition + actualStartWidth);
                    }
                    this.Scale.LineToPath(rangePath, scaleLinePositionX + rangeStartPosition, yPosition + actualStartWidth);
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
            if (this.rangePath != null)
            {
                if (this.linearGradientBrush != null)
                    canvas.SetFillPaint(this.linearGradientBrush, rangePath.Bounds);
                else
                    canvas.SetFillPaint(this.Fill, rangePath.Bounds);

                canvas.FillPath(this.rangePath);
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
                if (linearRange.Scale != null && linearRange.Scale.RangeLayout.Contains(linearRange.RangeView))
                {
                    linearRange.Scale.RangeChildUpdate(oldValue, newValue);

                    if (newValue is View newChild)
                    {
                        SetInheritedBindingContext(newChild, linearRange);

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

        /// <summary>
        /// Method to get range path with mid width shape. 
        /// </summary>
        /// <param name="x1"></param>
        /// <param name="y1"></param>
        /// <param name="x2"></param>
        /// <param name="y2"></param>
        /// <param name="x3"></param>
        /// <param name="y3"></param>
        private void UpdateMidRangePath(float x1, float y1, float x2, float y2, float x3, float y3)
        {
            if (this.Scale != null && this.rangePath != null)
            {
                if (this.Scale.Orientation == GaugeOrientation.Horizontal)
                    UpdateMidRangePath(rangePath, new PointF(x1, y1), new PointF(x2, y2), new Point(x3, y3));
                else
                    UpdateMidRangePath(rangePath, new PointF(y1, x1), new PointF(y2, x2), new Point(y3, x3));
            }
        }

        #endregion
    }
}
