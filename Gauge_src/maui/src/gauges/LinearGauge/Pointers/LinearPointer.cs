using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using System;

namespace Syncfusion.Maui.Gauges
{
    /// <summary>
    /// Base class of <see cref="LinearPointer"/> that properties for customizing gauge pointers.
    /// </summary>
    public abstract class LinearPointer : BindableObject
    {
        #region Bindable properties

        /// <summary>
        /// Identifies the <see cref="Value"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="Value"/> bindable property.
        /// </value>
        public static readonly BindableProperty ValueProperty =
            BindableProperty.Create(nameof(Value), typeof(double), typeof(LinearPointer), 0d, defaultBindingMode: BindingMode.TwoWay, propertyChanged: OnValueChanged);

        /// <summary>
        /// Identifies the <see cref="EnableAnimation"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="EnableAnimation"/> bindable property.
        /// </value>
        public static readonly BindableProperty EnableAnimationProperty =
            BindableProperty.Create(nameof(EnableAnimation), typeof(bool), typeof(LinearPointer), false);

        /// <summary>
        /// Identifies the <see cref="AnimationDuration"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="AnimationDuration"/> bindable property.
        /// </value>
        public static readonly BindableProperty AnimationDurationProperty =
            BindableProperty.Create(nameof(AnimationDuration), typeof(double), typeof(LinearPointer), 1000d);

        /// <summary>
        /// Identifies the <see cref="AnimationEasing"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="AnimationEasing"/> bindable property.
        /// </value>
        public static readonly BindableProperty AnimationEasingProperty =
           BindableProperty.Create(nameof(AnimationEasing), typeof(Easing), typeof(LinearPointer), Easing.Linear);

        /// <summary>
        /// Identifies the <see cref="IsInteractive"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="IsInteractive"/> bindable property.
        /// </value>
        public static readonly BindableProperty IsInteractiveProperty =
           BindableProperty.Create(nameof(IsInteractive), typeof(bool), typeof(LinearPointer), false);

        /// <summary>
        /// Identifies the <see cref="StepFrequency"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="StepFrequency"/> bindable property.
        /// </value>
        public static readonly BindableProperty StepFrequencyProperty =
            BindableProperty.Create(nameof(StepFrequency), typeof(double), typeof(LinearPointer), 0d);

        #endregion

        #region Fields

        private bool canAnimate;
        private const string animationName = "PointerAnimation";
        private double? animationValue;
        internal SfLinearGauge? Scale;

        /// <summary>
        /// Indicates pointer pressed in pointer or not. 
        /// </summary>
        internal bool IsPressed;

        /// <summary>
        /// Represents the pointer dragging rect. 
        /// </summary>
        internal RectangleF PointerRect;

        /// <summary>
        /// Holds dragging rect offset value. 
        /// </summary>
        internal const float DraggingOffset = 10;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="LinearPointer"/> class.
        /// </summary>
        public LinearPointer()
        {
            this.PointerView = new LinearPointerView(this);
        }

        #endregion

        #region Events

#nullable disable
        /// <summary>
        /// Called when the user is selecting a new value for the pointers by dragging.
        /// </summary>
        public event EventHandler<ValueChangedEventArgs> ValueChanged;

        /// <summary>
        /// Called before when the user is selecting a new value for the pointers by dragging.
        /// </summary>   
        public event EventHandler<ValueChangingEventArgs> ValueChanging;

        /// <summary>
        /// Called when the user starts selecting a new value of pointer by dragging.
        /// </summary>
        public event EventHandler<ValueChangedEventArgs> ValueChangeStarted;

        /// <summary>
        /// Called when the user is done selecting a new value of the pointer by dragging.
        /// </summary>
        public event EventHandler<ValueChangedEventArgs> ValueChangeCompleted;
#nullable enable

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the value that specifies the value of the pointer.
        /// </summary>
        /// <value>
        /// The default value is <c>0</c>.
        /// </value>
        public double Value
        {
            get { return (double)this.GetValue(ValueProperty); }
            set { this.SetValue(ValueProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether enables or disables the pointer animation.
        /// </summary>
        /// <value>
        /// <b>true</b> if animation is enabled; otherwise, <b>false</b>.The default value is <b>false</b>.
        /// </value>
        /// <remarks>
        /// <c>EnableAnimation</c> decides whether the pointer will be animated or not while its value get changed.
        /// If <c>EnableAnimation</c> is <c>true</c>, then it will be animated, otherwise not.
        /// </remarks>
        public bool EnableAnimation
        {
            get { return (bool)this.GetValue(EnableAnimationProperty); }
            set { this.SetValue(EnableAnimationProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value that specifies the pointer animation duration in milliseconds. 
        /// </summary>
        /// <value>
        /// The default value is <c>1000</c> milliseconds.
        /// </value>
        /// <remarks>
        /// It specifies how long the pointer animation will take to reach from old value to new value.
        /// </remarks>
        public double AnimationDuration
        {
            get { return (double)this.GetValue(AnimationDurationProperty); }
            set { this.SetValue(AnimationDurationProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value that specifies the pointer animation easing effect. 
        /// </summary>
        /// <value>
        /// The default value is <c>null/Linear</c>.
        /// </value>
        /// <remarks>
        /// It specifies how long the pointer animation will take to reach from old value to new value.
        /// </remarks>
        public Easing AnimationEasing
        {
            get { return (Easing)this.GetValue(AnimationEasingProperty); }
            set { this.SetValue(AnimationEasingProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value that allowing pointer value change through interaction.
        /// </summary>
        /// <value>
        /// <b>true</b> if pointer interaction is enabled; otherwise, <b>false</b>.The default value is <b>false</b>.
        /// </value>
        /// <remarks>
        /// It decides whether the gauge pointer will be interactive or not.
        /// If <see cref="IsInteractive"/> is <c>true</c>, the gauge pointer will be interactive, otherwise not.
        /// </remarks>
        public bool IsInteractive
        {
            get { return (bool)this.GetValue(IsInteractiveProperty); }
            set { this.SetValue(IsInteractiveProperty, value); }
        }

        /// <summary>
        /// Gets or sets the value that specifies the dragging step frequency of the pointer.
        /// </summary>
        /// <value>
        /// The default value is <c>0</c>.
        /// </value>
        public double StepFrequency
        {
            get { return (double)this.GetValue(StepFrequencyProperty); }
            set { this.SetValue(StepFrequencyProperty, value); }
        }

        /// <summary>
        /// Gets or sets animation value. 
        /// </summary>
        internal double? AnimationValue
        {
            get 
            {
                return animationValue;
            }
            set
            {
                animationValue = value;
                this.UpdatePointer();
            }
        }

        /// <summary>
        /// Gets or sets boolean value, that used to animate pointer. 
        /// </summary>
        internal bool CanAnimate
        {
            get
            {
                return this.EnableAnimation && this.canAnimate;
            }

            set
            {
                this.canAnimate = value;
            }
        }

        /// <summary>
        /// Represents the rendering view of pointer. 
        /// </summary>
        internal LinearPointerView PointerView { get; set; }

        #endregion

        #region Internal methods

        /// <summary>
        /// To animate pointer.
        /// </summary>
        /// <param name="animateFrom">From where the animation start.</param>
        /// <param name="animateTo">To where the animation stop.</param>
        internal void AnimatePointer(double animateFrom, double animateTo)
        {
            if (this.Scale != null && this.EnableAnimation && this.AnimationDuration > 0)
            {
                AnimationExtensions.Animate(this.PointerView, animationName, this.OnAnimationUpdate, animateFrom, animateTo,
                    16, (uint)this.AnimationDuration, this.AnimationEasing, this.OnAnimationFinished, null);
            }
        }

        /// <summary>
        /// Method used to update pointer to property change. 
        /// </summary>
        internal virtual void UpdatePointer()
        {
        }

        /// <summary>
        /// Method used to create pointer.
        /// </summary>
        internal virtual void CreatePointer()
        {
        }

        /// <summary>
        /// Method used to draw pointer.
        /// </summary>
        /// <param name="canvas">canvas</param>
        internal virtual void Draw(ICanvas canvas)
        {

        }

        /// <summary>
        /// Invalidate pointer view.
        /// </summary>
        internal void InvalidateDrawable()
        {
            if (this.PointerView != null)
                this.PointerView.InvalidateDrawable();
        }

        /// <summary>
        /// Method used to update pointer in pressed state. 
        /// </summary>
        /// <param name="point">Represents pressed point.</param>
        internal void UpdatePointerPressed(Point point)
        {
            if (this.PointerRect.Contains(point))
            {
                this.IsPressed = true;
                ValueChangedEventArgs args = new ValueChangedEventArgs
                {
                    CurrentValue = this.Value
                };
                this.RaiseOnValueChangeStarted(args);
            }
            else
            {
                this.IsPressed = false;
            }
        }

        /// <summary>
        /// Method used to pointer in released state. 
        /// </summary>
        internal void UpdatePointerReleased()
        {
            this.IsPressed = false;
            ValueChangedEventArgs args = new ValueChangedEventArgs
            {
                CurrentValue = this.Value
            };
            this.RaiseOnValueChangeCompleted(args);
        }

        /// <summary>
        /// To drag the pointer to current pointer position.
        /// </summary>
        /// <param name="currentPoint">The current pointer position.</param>
        internal void DragPointer(Point currentPoint)
        {
            if (this.Scale != null)
            {
                double currentPosition = this.Scale.Orientation == GaugeOrientation.Horizontal
                                        ? currentPoint.X : currentPoint.Y;

                double dragValue = this.Scale.GetValueFromPosition(currentPosition);

                if (StepFrequency > 0)
                {
                    dragValue = GetStepFrequencyValue(dragValue);
                }

                double actualValue = Math.Clamp(dragValue, this.Scale.ActualMinimum, this.Scale.ActualMaximum);
                ValueChangingEventArgs args = new ValueChangingEventArgs
                {
                    CurrentValue = actualValue,
                    PreviousValue = this.Value
                };
                this.RaiseOnValueChanging(args);
                if (!args.Cancel)
                {
                    this.Value = actualValue;
                }
            }
        }

        #endregion

        #region Property changed

        /// <summary>
        /// Called when <see cref="Value"/> property changed.
        /// </summary>
        /// <param name="bindable">The bindable object.</param>
        /// <param name="oldValue">The old value of bindable property changed.</param>
        /// <param name="newValue">The new value of bindable property changed.</param>
        private static void OnValueChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is LinearPointer linearPointer && linearPointer.Scale != null && !linearPointer.Scale.ScaleAvailableSize.IsZero)
            {
                ValueChangedEventArgs args = new ValueChangedEventArgs
                {
                    CurrentValue = (double)newValue
                };
                linearPointer.RaiseOnValueChanged(args);

                if (linearPointer.EnableAnimation)
                {
                    linearPointer.AnimatePointer((double)oldValue, (double)newValue);
                }
                else
                {
                    linearPointer.UpdatePointer();
                }

                linearPointer.InvalidateDrawable();
            }
        }

        #endregion

        #region Private methods

        /// <summary>
        /// To get the step frequency value from the start and end value of the pointer.
        /// </summary>
        /// <param name="currentValue">Specify the current value of pointer. </param>
        /// <returns>It returns the step value of the pointer</returns>
        private double GetStepFrequencyValue(double currentValue)
        {
            if (this.Scale == null) return currentValue;

            double minimum = Scale.Minimum;
            double delta = Scale.Maximum - minimum;
            double factor = Math.Round(Scale.ValueToFactor(currentValue) * (delta / StepFrequency)) / (delta / StepFrequency);
            return ((Scale.IsInversed ? 1d - factor : factor) * delta) + minimum;
        }

        /// <summary>
        /// Method used to update animation value. 
        /// </summary>
        /// <param name="value">Represents animation value.</param>
        internal void OnAnimationUpdate(double value)
        {
            this.AnimationValue = (float)value;
            this.InvalidateDrawable();
        }

        /// <summary>
        /// Called when animation finished. 
        /// </summary>
        /// <param name="value">Represents animation value.</param>
        /// <param name="isCompleted">Represents animation complete state.</param>
        private void OnAnimationFinished(double value, bool isCompleted)
        {
            AnimationExtensions.AbortAnimation(this.PointerView, animationName);
            this.AnimationValue = null;
        }

        /// <summary>
        /// This method is used to raise value changed event.
        /// </summary>
        /// <param name="args">The value changed event arguments.</param>
        private void RaiseOnValueChanged(ValueChangedEventArgs args)
        {
            this.ValueChanged?.Invoke(this, args);
        }

        /// <summary>
        /// This method is used to raise value changing event.
        /// </summary>
        /// <param name="args">The value changing event arguments.</param>
        private void RaiseOnValueChanging(ValueChangingEventArgs args)
        {
            this.ValueChanging?.Invoke(this, args);
        }

        /// <summary>
        /// This method is used to raise value change started event.
        /// </summary>
        /// <param name="args">The value changed event arguments.</param>
        private void RaiseOnValueChangeStarted(ValueChangedEventArgs args)
        {
            this.ValueChangeStarted?.Invoke(this, args);
        }

        /// <summary>
        /// This method is used to raise value change completed event.
        /// </summary>
        /// <param name="args">The value changed event arguments.</param>
        private void RaiseOnValueChangeCompleted(ValueChangedEventArgs args)
        {
            this.ValueChangeCompleted?.Invoke(this, args);
        }

        #endregion
    }
}
