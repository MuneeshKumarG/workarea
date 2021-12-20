using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using Syncfusion.Maui.Core.Internals;
using Syncfusion.Maui.Graphics.Internals;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Syncfusion.Maui.Core
{

	/// <summary>
	/// <see cref="SfEffectsView"/> is a container control that provides the out-of-the-box effects 
	/// such as highlight, ripple, selection, scaling, and rotation.
	/// </summary>
	[DesignTimeVisible(true)]
	[ContentProperty(nameof(Content))]
	public class SfEffectsView : ContentView, ITouchListener, ITapGestureListener, ILongPressGestureListener
	{
		#region Fields

		private Grid? mainGrid;

		private HighlightEffectLayer? highlightEffectLayer;

		private SelectionEffectLayer? selectionEffectLayer;

		private RippleEffectLayer? rippleEffectLayer;

		private bool isSelect;

		private bool isSelectedCalled;

		private bool canRepeat;

		private double tempScaleFactor;

        private const float anchorValue = 0.5005f;

		private string rotationAnimation = "Rotation";

		private string scaleAnimation = "Scaling";

		private string highlightAnimation = "Highlight";

		private Point touchDownPoint;

		internal bool LongPressHandled;

		internal bool ForceReset = false;

		#endregion

		#region Bindable Properties

		/// <summary>
		/// Backing store for the <see cref="RippleAnimationDuration"/> bindable property.
		/// </summary>
		public static readonly BindableProperty RippleAnimationDurationProperty = BindableProperty.Create(nameof(RippleAnimationDuration), typeof(double), typeof(SfEffectsView), 275d, BindingMode.Default);

		/// <summary>
		/// Backing store for the <see cref="ScaleAnimationDuration"/> bindable property.
		/// </summary>
		public static readonly BindableProperty ScaleAnimationDurationProperty = BindableProperty.Create(nameof(ScaleAnimationDuration), typeof(double), typeof(SfEffectsView), 150d, BindingMode.Default);

		/// <summary>
		/// Backing store for the <see cref="RotationAnimationDuration"/> bindable property.
		/// </summary>
		public static readonly BindableProperty RotationAnimationDurationProperty = BindableProperty.Create(nameof(RotationAnimationDuration), typeof(double), typeof(SfEffectsView), 200d, BindingMode.Default);

		/// <summary>
		/// Backing store for the <see cref="InitialRippleFactor"/> bindable property.
		/// </summary>
		public static readonly BindableProperty InitialRippleFactorProperty = BindableProperty.Create(nameof(InitialRippleFactor), typeof(double), typeof(SfEffectsView), 0.25d, BindingMode.Default);

		/// <summary>
		/// Backing store for the <see cref="ScaleFactor"/> bindable property.
		/// </summary>
		public static readonly BindableProperty ScaleFactorProperty = BindableProperty.Create(nameof(ScaleFactor), typeof(double), typeof(SfEffectsView), 1d, BindingMode.Default);

		/// <summary>
		/// Backing store for the <see cref="HighlightBackground"/> bindable property.
		/// </summary>
		public static readonly BindableProperty HighlightBackgroundProperty = BindableProperty.Create(nameof(HighlightBackground), typeof(Brush), typeof(SfEffectsView), new SolidColorBrush(Colors.Black), BindingMode.Default);

		/// <summary>
		/// Backing store for the <see cref="RippleBackground"/> bindable property.
		/// </summary>
		public static readonly BindableProperty RippleBackgroundProperty = BindableProperty.Create(nameof(RippleBackground), typeof(Brush), typeof(SfEffectsView), new SolidColorBrush(Colors.Black), BindingMode.Default);

		/// <summary>
		/// Backing store for the <see cref="SelectionBackground"/> bindable property.
		/// </summary>
		public static readonly BindableProperty SelectionBackgroundProperty = BindableProperty.Create(nameof(SelectionBackground), typeof(Brush), typeof(SfEffectsView), new SolidColorBrush(Colors.Black), BindingMode.Default, null, OnSelectionBackgroundPropertyChanged);

		/// <summary>
		/// Property changed for selection color.
		/// </summary>
		/// <param name="bindable">bindable value. </param>
		/// <param name="oldValue">old value.</param>
		/// <param name="newValue">new value.</param>
		private static void OnSelectionBackgroundPropertyChanged(BindableObject bindable, object oldValue, object newValue)
		{
			if (bindable != null && (bindable as SfEffectsView) != null && newValue != null)
			{
				(bindable as SfEffectsView)?.UpdateSelectionBackground((Brush)newValue);
			}
		}

		/// <summary>
		/// Backing store for the <see cref="Angle"/> bindable property.
		/// </summary>
		public static readonly BindableProperty AngleProperty = BindableProperty.Create(nameof(Angle), typeof(int), typeof(SfEffectsView), 0, BindingMode.Default);

		/// <summary>
		/// Backing store for the <see cref="FadeOutRipple"/> bindable property.
		/// </summary>
		public static readonly BindableProperty FadeOutRippleProperty = BindableProperty.Create(nameof(FadeOutRipple), typeof(bool), typeof(SfEffectsView), false, BindingMode.Default);

		/// <summary>
		/// Backing store for the <see cref="AutoResetEffects"/> bindable property.
		/// </summary>
		public static readonly BindableProperty AutoResetEffectsProperty = BindableProperty.Create(nameof(AutoResetEffects), typeof(AutoResetEffects), typeof(SfEffectsView), AutoResetEffects.None, BindingMode.Default, null);

		/// <summary>
		/// Backing store for the <see cref="TouchDownEffects"/> bindable property.
		/// </summary>
		public static readonly BindableProperty TouchDownEffectsProperty = BindableProperty.Create(nameof(TouchDownEffects), typeof(SfEffects), typeof(SfEffectsView), SfEffects.Ripple, BindingMode.Default);

		/// <summary>
		/// Backing store for the <see cref="TouchUpEffects"/> bindable property.
		/// </summary>
		public static readonly BindableProperty TouchUpEffectsProperty = BindableProperty.Create(nameof(TouchUpEffects), typeof(SfEffects), typeof(SfEffectsView), SfEffects.None, BindingMode.Default);

		/// <summary>
		/// Backing store for the <see cref="LongPressEffects"/> bindable property.
		/// </summary>
		public static readonly BindableProperty LongPressEffectsProperty = BindableProperty.Create(nameof(LongPressEffects), typeof(SfEffects), typeof(SfEffectsView), SfEffects.None, BindingMode.Default);

		/// <summary>
		/// Backing store for the <see cref="LongPressedCommand"/> bindable property.
		/// </summary>
		public static readonly BindableProperty LongPressedCommandProperty = BindableProperty.Create(nameof(LongPressedCommand), typeof(ICommand), typeof(SfEffectsView), null, BindingMode.Default);

		/// <summary>
		/// Backing store for the <see cref="LongPressedCommandParameter"/> bindable property.
		/// </summary>
		public static readonly BindableProperty LongPressedCommandParameterProperty = BindableProperty.Create(nameof(LongPressedCommandParameter), typeof(object), typeof(SfEffectsView), null, BindingMode.Default);

		/// <summary>
		/// Backing store for the <see cref="IsSelected"/> bindable property.
		/// </summary>
		public static readonly BindableProperty IsSelectedProperty = BindableProperty.Create(nameof(IsSelected), typeof(bool), typeof(SfEffectsView), false, BindingMode.TwoWay, null, OnIsSelectedPropertyChanged);

		/// <summary>
		/// Property changed for isselected.
		/// </summary>
		/// <param name="bindable">The bindable value.</param>
		/// <param name="oldValue">The old value.</param>
		/// <param name="newValue">The new value.</param>
		private static void OnIsSelectedPropertyChanged(BindableObject bindable, object oldValue, object newValue)
		{
			if (bindable != null && (bindable as SfEffectsView) != null && newValue != null)
			{
				var effectsView = bindable as SfEffectsView;
				if (effectsView != null)
				{
					effectsView.IsSelection = (bool)newValue;
				}

			}
		}

		/// <summary>
		/// Backing store for the <see cref="Content"/> bindable property.
		/// </summary>
		public new static readonly BindableProperty ContentProperty = BindableProperty.Create(nameof(Content), typeof(View), typeof(SfEffectsView), null, BindingMode.Default, null, OnContentChanged);

		/// <summary>
		/// Property changed for content.
		/// </summary>
		/// <param name="bindable">Binable value.</param>
		/// <param name="oldValue">Old value.</param>
		/// <param name="newValue">New value.</param>
		private static void OnContentChanged(BindableObject bindable, object oldValue, object newValue)
		{
			if (bindable != null && (bindable as SfEffectsView) != null)
			{
				if ((View)newValue != null)
				{
					(bindable as SfEffectsView)?.AddContent((View)newValue);
				}
			}
		}

		/// <summary>
		/// Backing store for the <see cref="ShouldIgnoreTouches"/> bindable property.
		/// </summary>
		public static readonly BindableProperty ShouldIgnoreTouchesProperty = BindableProperty.Create(nameof(ShouldIgnoreTouches), typeof(bool), typeof(SfEffectsView), false, BindingMode.Default,null,OnShouldIgnorePropertyChanged);

                /// <summary>
		/// Property changed for should ignore touch.
		/// </summary>
		/// <param name="bindable">Binable value.</param>
		/// <param name="oldValue">Old value.</param>
		/// <param name="newValue">New value.</param>
		private static void OnShouldIgnorePropertyChanged(BindableObject bindable, object oldValue, object newValue)
		{
			if (bindable != null && (bindable as SfEffectsView) != null && newValue != null)
			{
				var effectsView = bindable as SfEffectsView;
				if (effectsView != null)
				{
				     //ToDO:To avoid the touch issue in parent, this code has been  added.This code need to be removed once touch issue fixed.
					effectsView.IsEnabled = !(bool)newValue;
				}

			}
		}

		/// <summary>
		/// Backing store for the <see cref="TouchDownCommand"/> bindable property.
		/// </summary>
		public static readonly BindableProperty TouchDownCommandProperty = BindableProperty.Create(nameof(TouchDownCommand), typeof(ICommand), typeof(SfEffectsView), null, BindingMode.Default);

		/// <summary>
		/// Backing store for the <see cref="TouchUpCommand"/> bindable property.
		/// </summary>
		public static readonly BindableProperty TouchUpCommandProperty = BindableProperty.Create(nameof(TouchUpCommand), typeof(ICommand), typeof(SfEffectsView), null, BindingMode.Default);

		/// <summary>
		/// Backing store for the <see cref="TouchDownCommandParameter"/> bindable property.
		/// </summary>
		public static readonly BindableProperty TouchDownCommandParameterProperty = BindableProperty.Create(nameof(TouchDownCommandParameter), typeof(object), typeof(SfEffectsView), null, BindingMode.Default);

		/// <summary>
		/// Backing store for the <see cref="TouchUpCommandParameter"/> bindable property.
		/// </summary>
		public static readonly BindableProperty TouchUpCommandParameterProperty = BindableProperty.Create(nameof(TouchUpCommandParameter), typeof(object), typeof(SfEffectsView), null, BindingMode.Default);

		#endregion

		#region Property

		/// <summary>
		/// Gets or sets the contnet 
		/// </summary>
		public new View? Content
		{
			get { return (View)GetValue(ContentProperty); }
			set { SetValue(ContentProperty, value); }

		}

		/// <summary>
		/// Gets or sets the duration of the ripple animation in milliseconds.
		/// </summary>
		public double RippleAnimationDuration
		{
			get { return (double)GetValue(RippleAnimationDurationProperty); }
			set { SetValue(RippleAnimationDurationProperty, value); }
		}

		/// <summary>
		/// Gets or sets the duration of the scale animation in milliseconds.
		/// </summary>
		public double ScaleAnimationDuration
		{
			get { return (double)GetValue(ScaleAnimationDurationProperty); }
			set { SetValue(ScaleAnimationDurationProperty, value); }
		}

		/// <summary>
		/// Gets or sets the duration of the rotation animation in milliseconds.
		/// </summary>
		public double RotationAnimationDuration
		{
			get { return (double)GetValue(RotationAnimationDurationProperty); }
			set { SetValue(RotationAnimationDurationProperty, value); }
		}

		/// <summary>
		/// Gets or sets the ripple effect of initial radius factor.
		/// </summary>
		public double InitialRippleFactor
		{
			get { return (double)GetValue(InitialRippleFactorProperty); }
			set { SetValue(InitialRippleFactorProperty, value); }
		}

		/// <summary>
		/// Gets or sets the scale factor used for scale effect.
		/// </summary>
		public double ScaleFactor
		{
			get { return (double)GetValue(ScaleFactorProperty); }
			set { SetValue(ScaleFactorProperty, value); }
		}

		/// <summary>
		/// Gets or sets the color to highlight the effects view.
		/// </summary>
		public Brush HighlightBackground
		{
			get { return (Brush)GetValue(HighlightBackgroundProperty); }
			set { SetValue(HighlightBackgroundProperty, value); }
		}

		/// <summary>
		/// Gets or sets the color of the ripple.
		/// </summary>
		public Brush RippleBackground
		{
			get { return (Brush)GetValue(RippleBackgroundProperty); }
			set { SetValue(RippleBackgroundProperty, value); }
		}

		/// <summary>
		/// Gets or sets the color applied when the view is on selected state.
		/// </summary>
		public Brush SelectionBackground
		{
			get { return (Brush)GetValue(SelectionBackgroundProperty); }
			set { SetValue(SelectionBackgroundProperty, value); }
		}

		/// <summary>
		/// Gets or sets the rotation angle. 
		/// </summary>
		public int Angle
		{
			get { return (int)GetValue(AngleProperty); }
			set { SetValue(AngleProperty, value); }
		}

		/// <summary>
		/// Gets or sets a value indicating whether to fade out ripple color when it gross.
		/// </summary>
		public bool FadeOutRipple
		{
			get { return (bool)GetValue(FadeOutRippleProperty); }
			set { SetValue(FadeOutRippleProperty, value); }
		}

		/// <summary>
		/// Gets or sets the effect that was start rendering on touch down and start removing on touch up in Android and UWP platforms.
		/// </summary>
		/// <remarks>If it is other than None then TouchDownEffects, LongPressEffects and TouchUpEffects will not work.</remarks>
		public AutoResetEffects AutoResetEffects
		{
			get { return (AutoResetEffects)GetValue(AutoResetEffectsProperty); }
			set { SetValue(AutoResetEffectsProperty, value); }
		}

		/// <summary>
		/// Gets or sets the pointer-down effect.
		/// </summary>
		public SfEffects TouchDownEffects
		{
			get { return (SfEffects)GetValue(TouchDownEffectsProperty); }
			set { SetValue(TouchDownEffectsProperty, value); }
		}

		/// <summary>
		/// Gets or sets the pointer-up effect.
		/// </summary>
		public SfEffects TouchUpEffects
		{
			get { return (SfEffects)GetValue(TouchUpEffectsProperty); }
			set { SetValue(TouchUpEffectsProperty, value); }
		}

		/// <summary>
		/// Gets or sets the long-press effect.
		/// </summary>
		public SfEffects LongPressEffects
		{
			get { return (SfEffects)GetValue(LongPressEffectsProperty); }
			set { SetValue(LongPressEffectsProperty, value); }
		}


		/// <summary>
		/// Gets or sets a value indicating whether to set the view state as selected. 
		/// </summary>
		public bool IsSelected
		{
			get { return (bool)GetValue(IsSelectedProperty); }
			set { SetValue(IsSelectedProperty, value); }
		}

		/// <summary>
		/// Gets or sets a value indicating whether to ignore the touches in EffectsView. 
		/// </summary>
		public bool ShouldIgnoreTouches
		{
			get { return (bool)GetValue(ShouldIgnoreTouchesProperty); }
			set { SetValue(ShouldIgnoreTouchesProperty, value); }
		}


		/// <summary>
		/// Gets or sets the command to invoke when handling long press.
		/// </summary>
		public ICommand LongPressedCommand
		{
			get { return (ICommand)GetValue(LongPressedCommandProperty); }
			set { SetValue(LongPressedCommandProperty, value); }
		}

		/// <summary>
		/// Gets or sets the command to invoke when handling touch down.
		/// </summary>
		public ICommand TouchDownCommand
		{
			get { return (ICommand)GetValue(TouchDownCommandProperty); }
			set { SetValue(TouchDownCommandProperty, value); }
		}

		/// <summary>
		/// Gets or sets the command to invoke when handling touch up.
		/// </summary>
		public ICommand TouchUpCommand
		{
			get { return (ICommand)GetValue(TouchUpCommandProperty); }
			set { SetValue(TouchUpCommandProperty, value); }
		}

		/// <summary>
		/// Gets or sets the parameter to pass to the <see cref="TouchDownCommand"/>.
		/// </summary>
		public object TouchDownCommandParameter
		{
			get { return (object)GetValue(TouchDownCommandParameterProperty); }
			set { SetValue(TouchDownCommandParameterProperty, value); }
		}

		/// <summary>
		/// Gets or sets the parameter to pass to the <see cref="LongPressedCommand"/>.
		/// </summary>
		public object LongPressedCommandParameter
		{
			get { return (object)GetValue(LongPressedCommandParameterProperty); }
			set { SetValue(LongPressedCommandParameterProperty, value); }
		}

		/// <summary>
		/// Gets or sets the parameter to pass to the <see cref="TouchUpCommand"/>.
		/// </summary>
		public object TouchUpCommandParameter
		{
			get { return (object)GetValue(TouchUpCommandParameterProperty); }
			set { SetValue(TouchUpCommandParameterProperty, value); }
		}

		/// <summary>
		/// Gets or sets whether to set the view state as selected. 
		/// </summary>
		internal bool IsSelection
		{
			get { return isSelect; }
			set
			{
				if (isSelect != value)
				{
					isSelect = value;
					if (value)
					{
						selectionEffectLayer?.UpdateSelectionBounds(this.Width, this.Height, this.SelectionBackground);
					}
					else
					{
						RemoveSelection();
					}

					InvokeSelectionChangedEvent();
				}
			}
		}

		#endregion

		#region Events

		/// <summary>
		/// The <see cref="AnimationCompleted"/> event occurs on direct interaction and when programmatically applied, 
		/// it only occurs when touch-up is called on direct interaction and after the effects have been completed. 
		/// It will not trigger the selection effect. 
		/// </summary>
		public event EventHandler? AnimationCompleted;

		/// <summary>
		/// The <see cref="SelectionChanged"/> event triggers both the rendering of <see cref="SfEffects.Selection"/> by direct interaction 
		/// and the <see cref="IsSelected"/> property changed. 
		/// </summary>
		public event EventHandler? SelectionChanged;

		/// <summary> 
		/// Occurs when handling touch down.
		/// </summary>
		public event EventHandler? TouchDown;

		/// <summary>
		/// Occurs when handling touch up.
		/// </summary>
		public event EventHandler? TouchUp;

		/// <summary>
		/// Occurs when handling long press.
		/// </summary>
		public event EventHandler? LongPressed;

		#endregion

		#region Constructor

		/// <summary>
		/// Initializes a new instance of the <see cref="SfEffectsView"/> class.
		/// </summary>
		/// <example>
		/// Create SfEffectsView with the effects animation
		/// <code><![CDATA[
		///   <effectsView:SfEffectsView  />
		///  ]]></code>
		/// </example>      
		public SfEffectsView()
		{
			this.InitializeEffects();
			this.AddGestureListener(this);
			this.AddTouchListener(this);
		}

		#endregion

		#region Public methods

		/// <summary>
		/// Removes the ripple and highlight effects.
		/// </summary>
		public void Reset()
		{
			this.canRepeat = false;
			this.LongPressHandled = false;
			if (rippleEffectLayer != null)
			{
				rippleEffectLayer.CanRemoveRippleAnimation = rippleEffectLayer.AnimationIsRunning("RippleAnimator");
				if (!rippleEffectLayer.CanRemoveRippleAnimation || this.ForceReset)
				{
					rippleEffectLayer.OnRippleAnimationFinished(0, true);
				}
			}

			if (highlightEffectLayer != null)
			{
				highlightEffectLayer.UpdateHighlightBounds();
			}
			if (selectionEffectLayer != null && IsSelected)
			{
				this.IsSelected = false;
				InvokeSelectionChangedEvent();
			}
			if (TouchDownEffects == SfEffects.Scale || TouchUpEffects == SfEffects.Scale || LongPressEffects == SfEffects.Scale)
			{
				if (this.Content != null)
				{
					this.Content.Scale = 1;
				}

				this.OnScaleAnimationEnd(0, true);
			}
			if (TouchUpEffects == SfEffects.Rotation || TouchDownEffects == SfEffects.Rotation || LongPressEffects == SfEffects.Rotation)
			{
				if (this.Content != null)
				{
					this.Content.Rotation = 0;
				}
				this.OnRotationAnimationEnd(0, true);
			}

			for (int i = 0; i < this.mainGrid?.Children.Count; i++)
			{
				if ((this.mainGrid.Children[i] is RippleEffectLayer && rippleEffectLayer != null && !rippleEffectLayer.AnimationIsRunning("RippleAnimator")) || mainGrid.Children[i] is HighlightEffectLayer || mainGrid.Children[i] is SelectionEffectLayer)
				{
					this.mainGrid.Children.RemoveAt(i);
				}
			}
		}

		// TODO: To avoid argument width and height lesser than zero exception when not setting width and height to the control.
		/// <summary>
		/// MeasureOverride method
		/// </summary>
		/// <param name="widthConstraint"> The width.</param>
		/// <param name="heightConstraint"> The height.</param>
		/// <returns>The size.</returns>
		protected override Size MeasureOverride(double widthConstraint, double heightConstraint)
		{
			if (this.rippleEffectLayer != null)
			{
				if (widthConstraint != double.PositiveInfinity && widthConstraint != double.NegativeInfinity)
					this.rippleEffectLayer.WidthRequest = widthConstraint;
				if (heightConstraint != double.PositiveInfinity && heightConstraint != double.NegativeInfinity)
					this.rippleEffectLayer.HeightRequest = heightConstraint;
			}
			if (this.highlightEffectLayer != null)
			{
				if (widthConstraint != double.PositiveInfinity && widthConstraint != double.NegativeInfinity)
					this.highlightEffectLayer.WidthRequest = widthConstraint;
				if (heightConstraint != double.PositiveInfinity && heightConstraint != double.NegativeInfinity)
					this.highlightEffectLayer.HeightRequest = heightConstraint;

			}
			if (this.selectionEffectLayer != null)
			{
				if (widthConstraint != double.PositiveInfinity && widthConstraint != double.NegativeInfinity)
					this.selectionEffectLayer.WidthRequest = widthConstraint;
				if (heightConstraint != double.PositiveInfinity && heightConstraint != double.NegativeInfinity)
					this.selectionEffectLayer.HeightRequest = heightConstraint;

			}
			return base.MeasureOverride(widthConstraint, heightConstraint);
		}

		// TODO: To avoid argument width and height lesser than zero exception when not setting width and height to the control.
		/// <summary>
		/// ArrangeOverride method
		/// </summary>
		/// <param name="bounds">The bounds.</param>
		/// <returns>The size</returns>
		protected override Size ArrangeOverride(Rectangle bounds)
		{
			if (this.highlightEffectLayer != null)
			{
				this.highlightEffectLayer.WidthRequest = bounds.Width;
				this.highlightEffectLayer.HeightRequest = bounds.Height;
				this.highlightEffectLayer.Measure(bounds.Width, bounds.Height, MeasureFlags.IncludeMargins);
			}
			if (this.rippleEffectLayer != null)
			{
				this.rippleEffectLayer.WidthRequest = bounds.Width;
				this.rippleEffectLayer.HeightRequest = bounds.Height;
				this.rippleEffectLayer.Measure(bounds.Width, bounds.Height, MeasureFlags.IncludeMargins);
			}
			if (this.selectionEffectLayer != null)
			{
				this.selectionEffectLayer.WidthRequest = bounds.Width;
				this.selectionEffectLayer.HeightRequest = bounds.Height;
				this.selectionEffectLayer.Measure(bounds.Width, bounds.Height, MeasureFlags.IncludeMargins);
				if (IsSelected)
				{
					selectionEffectLayer?.UpdateSelectionBounds(bounds.Width, bounds.Height, this.SelectionBackground);
					if (!isSelectedCalled)
					{
						this.InvokeSelectionChangedEvent();
						isSelectedCalled = true;
					}
				}
			}

			return base.ArrangeOverride(bounds);
		}

		/// <summary>
		/// Used to trigger the invoke the effects.
		/// </summary>
		/// <param name="effects"><see cref="SfEffects"/> that need to apply.</param>
		/// <param name="rippleStartPosition">Position at where the ripple animation start growing.
		/// By default, value ripple start from center.</param>
		/// <param name="rippleStartPoint">Point at which the ripple animation start growing. 
		/// By default, value is null.</param>
		/// <param name="repeat">To set whether to ripple animation need to repeat or not.</param>
		public void ApplyEffects(SfEffects effects = SfEffects.Ripple,
			RippleStartPosition rippleStartPosition = RippleStartPosition.Default,
			System.Drawing.Point? rippleStartPoint = null,
			bool repeat = false)
		{
			if (rippleEffectLayer != null)
			{
				rippleEffectLayer.CanRemoveRippleAnimation = false;
			}

			canRepeat = repeat;
			float x = (float)(Width / 2), y = (float)(Height / 2);
			
				if (rippleStartPosition == RippleStartPosition.Left)
				{
					x = 0;
				}

				if (rippleStartPosition == RippleStartPosition.Top)
				{
					y = 0;
				}

				if (rippleStartPosition == RippleStartPosition.Right)
				{
					x = (float)Width;
				}

				if (rippleStartPosition == RippleStartPosition.Bottom)
				{
					y = (float)Height;
				}
				if (rippleStartPosition == RippleStartPosition.TopLeft)
				{
					x = 0;
					y = 0;
				}
				if (rippleStartPosition == RippleStartPosition.TopRight)
				{
					x = (float)Width;
					y = 0;
				}
				if (rippleStartPosition == RippleStartPosition.BottomLeft)
				{
					x = 0;
					y = (float)Height;
				}

				if (rippleStartPosition == RippleStartPosition.BottomRight)
				{
					x = (float)Width; 
					y = (float)Height;
				}

				if (rippleStartPosition == RippleStartPosition.Default)
				{
					if (rippleStartPoint != null)
					{
						x = rippleStartPoint.Value.X;
						y = rippleStartPoint.Value.Y;
					}
				}
			

			AddEffects(effects, new Point(x, y));
		}

		#endregion

		#region Methods

		/// <summary>
		/// Invokes animation completed event.
		/// </summary>
		/// <param name="eventArgs">Animation completed events argument.</param>
		internal void RaiseAnimationCompletedEvent(EventArgs eventArgs)
		{
			AnimationCompleted?.Invoke(this, eventArgs);
		}

		/// <summary>
		/// Invokes when this view selected.
		/// </summary>
		/// <param name="eventArgs">Selected events argument.</param>
		internal void RaiseSelectedEvent(EventArgs eventArgs)
		{
			SelectionChanged?.Invoke(this, eventArgs);
		}

		/// <summary>
		/// Invokes <see cref="SelectionChanged"/> event.
		/// </summary>
		internal void InvokeSelectionChangedEvent()
		{
			RaiseSelectedEvent(EventArgs.Empty);
		}

		/// <summary>
		/// Invokes <see cref="LongPressed"/> when handling long press.
		/// </summary>
		internal void InvokeLongPressedEventAndCommand()
		{
			LongPressed?.Invoke(this, EventArgs.Empty);
			if (LongPressedCommand != null && LongPressedCommand.CanExecute(LongPressedCommandParameter))
			{
				LongPressedCommand.Execute(LongPressedCommandParameter);
			}
		}

		/// <summary>
		/// Invokes <see cref="TouchDown"/> when handling touch down.
		/// </summary>
		internal void InvokeTouchDownEventAndCommand()
		{
			TouchDown?.Invoke(this, EventArgs.Empty);
			if (TouchDownCommand != null && TouchDownCommand.CanExecute(TouchDownCommandParameter))
			{
				TouchDownCommand.Execute(TouchDownCommandParameter);
			}
		}

		/// <summary>
		/// Invokes <see cref="AnimationCompleted"/> event.
		/// </summary>
		internal void InvokeAnimationCompletedEvent()
		{
			RaiseAnimationCompletedEvent(EventArgs.Empty);
		}

		/// <summary>
		///  Invokes <see cref="TouchUp"/> when handling touch up.
		/// </summary>
		internal void InvokeTouchUpEventAndCommand()
		{
			TouchUp?.Invoke(this, EventArgs.Empty);
			if (TouchUpCommand != null && TouchUpCommand.CanExecute(TouchUpCommandParameter))
			{
				TouchUpCommand.Execute(TouchUpCommandParameter);
			}
		}

		/// <summary>
		/// Updateselection method.
		/// </summary>
		/// <param name="SelectionBackground"></param>
		private void UpdateSelectionBackground(Brush SelectionBackground)
		{
			if (this.IsSelected)
			{
				selectionEffectLayer?.UpdateSelectionBounds(this.Width, this.Height, SelectionBackground);
			}

		}

		/// <summary>
		/// Remove the selection effect.
		/// </summary>
		private void RemoveSelection()
		{
			selectionEffectLayer?.UpdateSelectionBounds();
		}

		/// <summary>
		/// Remove the highlight effect.
		/// </summary>
		private void RemoveHighlightEffect()
		{
			if (highlightEffectLayer != null)
			{
				highlightEffectLayer.UpdateHighlightBounds();
			}
		}

		/// <summary>
		/// Method used to add respective auto reset effects.
		/// </summary>
		/// <param name="effects"> The effects value</param>
		/// <param name="touchPoint"> The touch point</param>
		private void AddResetEffects(AutoResetEffects effects, Point touchPoint)
		{
			foreach (AutoResetEffects effect in effects.GetAllItems())
			{
				if (effect == AutoResetEffects.None)
				{
					continue;
				}
				if (effect == AutoResetEffects.Highlight)
				{
					this.highlightEffectLayer?.UpdateHighlightBounds(this.Width, this.Height, this.HighlightBackground);
					AnimationExtensions.Animate(this.highlightEffectLayer, this.highlightAnimation, OnHighlightAnimationUpdate, 16, 250, Easing.Linear, OnAnimationFinished, null);

				}

				if (effect == AutoResetEffects.Ripple)
				{
					this.rippleEffectLayer?.StartRippleAnimation(touchPoint, this.RippleBackground, this.RippleAnimationDuration, (float)this.InitialRippleFactor, this.FadeOutRipple);
				}

				if (effect == AutoResetEffects.Scale)
				{
					this.StartScaleAnimation();
				}
			}

		}


		/// <summary>
		/// Method used to add respective effects.
		/// </summary>
		/// <param name="touchPoint">The touch point</param>
		/// <param name="sfEffect">The Effects</param>
		private void AddEffects(SfEffects sfEffect, Point touchPoint)
		{
			foreach (SfEffects effect in sfEffect.GetAllItems())
			{
				if (effect == SfEffects.None)
				{
					continue;
				}

				if (effect == SfEffects.Highlight)
				{
					this.highlightEffectLayer?.UpdateHighlightBounds(this.Width, this.Height, this.HighlightBackground);
				}

				if (effect == SfEffects.Ripple)
				{
					this.rippleEffectLayer?.StartRippleAnimation(touchPoint, this.RippleBackground, this.RippleAnimationDuration, (float)this.InitialRippleFactor, this.FadeOutRipple, canRepeat);
				}

				if (effect == SfEffects.Selection)
				{
					this.selectionEffectLayer?.UpdateSelectionBounds(this.Width, this.Height, this.SelectionBackground);
					if (!this.IsSelected)
					{
						this.IsSelected = true;
					}
				}

				if (effect == SfEffects.Scale)
				{
					this.StartScaleAnimation();
				}

				if (effect == SfEffects.Rotation)
				{
					this.StartRotationAnimation();
				}
			}
		}

		/// <summary>
		/// Remove ripple effect.
		/// </summary>
		private void RemoveRippleEffect()
		{
			this.rippleEffectLayer?.OnRippleAnimationFinished(0, true);
		}

		/// <summary>
		/// Initialize method.
		/// </summary>
		private void InitializeEffects()
		{
			this.mainGrid = new Grid();
			base.Content = this.mainGrid;

            this.rippleEffectLayer = new RippleEffectLayer(this.RippleBackground, this.RippleAnimationDuration);
            this.mainGrid?.Children.Add(this.rippleEffectLayer);

            this.highlightEffectLayer = new HighlightEffectLayer(this.HighlightBackground);
            this.mainGrid?.Children.Add(this.highlightEffectLayer);

            this.selectionEffectLayer = new SelectionEffectLayer(this.SelectionBackground);
            this.mainGrid?.Children.Add(this.selectionEffectLayer);
        }

		/// <summary>
		/// Scale animation method.
		/// </summary>
		private void StartScaleAnimation()
		{
			if (this.Content != null && this.tempScaleFactor != this.ScaleFactor)
			{
				this.Content.AnchorX = anchorValue;
				this.Content.AnchorY = anchorValue;
				this.tempScaleFactor = this.ScaleFactor;
				AnimationExtensions.Animate(this.Content, scaleAnimation, this.OnScaleAnimationUpdate, this.Content.Scale, ScaleFactor,
			 16, (uint)ScaleAnimationDuration, Easing.Linear, this.OnScaleAnimationEnd, null);
			}
		}

		/// <summary>
		/// Rotation animation method.
		/// </summary>
		private void StartRotationAnimation()
		{
			if (this.Content != null)
			{
				this.Content.AnchorX = anchorValue;
				this.Content.AnchorY = anchorValue;

				AnimationExtensions.Animate(this.Content, rotationAnimation, this.OnAnimationUpdate, this.Content.Rotation, Angle,
				 16, (uint)RotationAnimationDuration, Easing.Linear, this.OnRotationAnimationEnd, null);

			}
		}

		/// <summary>
		/// Animation ended method.
		/// </summary>
		/// <param name="value"> The animation value.</param>
		/// <param name="finished"> The finished.</param>

		private void OnRotationAnimationEnd(double value, bool finished)
		{
			AnimationExtensions.AbortAnimation(this, rotationAnimation);

			if ((this.rippleEffectLayer != null && !this.rippleEffectLayer.AnimationIsRunning("RippleAnimator")) || (rippleEffectLayer == null && (TouchUpEffects.GetAllItems().Contains(SfEffects.None) || TouchUpEffects.GetAllItems().Contains(SfEffects.Rotation)) && (LongPressEffects.GetAllItems().Contains(SfEffects.None) || !LongPressHandled || LongPressEffects.GetAllItems().Contains(SfEffects.Rotation))))
			{
				this.InvokeAnimationCompletedEvent();
			}

		}

		/// <summary>
		/// Animation update method.
		/// </summary>
		/// <param name="value">The animation value.</param>

		private void OnAnimationUpdate(double value)
		{
			if (this.Content != null)
			{
				this.Content.Rotation = value;
			}
		}

		/// <summary>
		/// Scale animation ended method.
		/// </summary>
		/// <param name="value">The animation value.</param>
		/// <param name="finished">The finished value.</param>
		private void OnScaleAnimationEnd(double value, bool finished)
		{
			AnimationExtensions.AbortAnimation(this, scaleAnimation);
			if ((this.rippleEffectLayer != null && !this.rippleEffectLayer.AnimationIsRunning("RippleAnimator")) || (rippleEffectLayer == null && (TouchUpEffects.GetAllItems().Contains(SfEffects.None) || TouchUpEffects.GetAllItems().Contains(SfEffects.Scale)) && (LongPressEffects.GetAllItems().Contains(SfEffects.None) || !LongPressHandled || LongPressEffects.GetAllItems().Contains(SfEffects.Scale))))
			{
				this.InvokeAnimationCompletedEvent();
			}
		}

		/// <summary>
		/// Scale aniamtion update method.
		/// </summary>
		/// <param name="value">Animation update value.</param>
		private void OnScaleAnimationUpdate(double value)
		{
			if (this.Content != null)
			{
				this.Content.Scale = value;
			}
		}

		/// <summary>
		/// Add content to the main view
		/// </summary>
		/// <param name="view"></param>
		private void AddContent(View view)
		{
			if (view != null)
			{
				this.mainGrid?.Insert(0, view);
			}
		}

		/// <summary>
		/// Touch Action method
		/// </summary>
		/// <param name="e"> The touch event arguments.</param>
		public void OnTouch(TouchEventArgs e)
		{
			if (!this.ShouldIgnoreTouches)
			{
				if (e.Action == TouchActions.Pressed)
				{
					this.touchDownPoint = e.TouchPoint;
					LongPressHandled = false;

					if (rippleEffectLayer != null)
					{
						rippleEffectLayer.CanRemoveRippleAnimation = false;
					}

					this.InvokeTouchDownEventAndCommand();

					if (this.AutoResetEffects != AutoResetEffects.None)
					{
						this.AddResetEffects(AutoResetEffects, e.TouchPoint);
					}
					else
					{
						this.AddEffects(TouchDownEffects, e.TouchPoint);
					}
				}
				if (e.Action == TouchActions.Released)
				{
					this.InvokeTouchUpEventAndCommand();

					if (this.AutoResetEffects.GetAllItems().Contains(AutoResetEffects.Ripple))
					{
						if (this.rippleEffectLayer != null)
						{
							this.rippleEffectLayer.CanRemoveRippleAnimation = this.rippleEffectLayer.AnimationIsRunning("RippleAnimator");
						}
					}
					else if (this.AutoResetEffects == AutoResetEffects.None)
					{
						if (TouchDownEffects == SfEffects.Highlight || TouchDownEffects.GetAllItems().Contains(SfEffects.Highlight))
						{
							this.highlightEffectLayer?.UpdateHighlightBounds();
							if ((this.rippleEffectLayer != null && !this.rippleEffectLayer.AnimationIsRunning("RippleAnimator")) || (rippleEffectLayer == null && (TouchUpEffects.GetAllItems().Contains(SfEffects.None) || TouchUpEffects.GetAllItems().Contains(SfEffects.Highlight))) && (LongPressEffects.GetAllItems().Contains(SfEffects.None) || !LongPressHandled || LongPressEffects.GetAllItems().Contains(SfEffects.Highlight)))
							{
								this.InvokeAnimationCompletedEvent();
							}

						}

						if (!IsSelected || (!IsSelected && (TouchDownEffects != SfEffects.Selection || !TouchDownEffects.GetAllItems().Contains(SfEffects.Selection))))
						{
							this.RemoveSelection();
						}


						if (TouchDownEffects.GetAllItems().Contains(SfEffects.Ripple) || TouchUpEffects.GetAllItems().Contains(SfEffects.Ripple) || LongPressEffects.GetAllItems().Contains(SfEffects.Ripple))
						{
							if (this.rippleEffectLayer != null)
							{
								this.rippleEffectLayer.CanRemoveRippleAnimation = this.rippleEffectLayer.AnimationIsRunning("RippleAnimator");
							}

						}

						if (TouchUpEffects != SfEffects.Highlight || !TouchUpEffects.GetAllItems().Contains(SfEffects.Highlight))
						{
							this.RemoveHighlightEffect();
						}
						else
						{

							AnimationExtensions.Animate(this.highlightEffectLayer, highlightAnimation, OnHighlightAnimationUpdate, 16, 250, Easing.Linear, OnAnimationFinished, null);
						}

						if ((TouchUpEffects != SfEffects.Ripple || !TouchUpEffects.GetAllItems().Contains(SfEffects.Ripple)) && this.rippleEffectLayer != null && !this.rippleEffectLayer.AnimationIsRunning("RippleAnimator"))
						{
							this.RemoveRippleEffect();
						}
					}
				}
				else if (e.Action == TouchActions.Cancelled)
				{
					this.LongPressHandled = false;
					this.RemoveRippleEffect();
					this.RemoveHighlightEffect();
					this.RemoveSelection();
				}
				else if(e.Action == TouchActions.Moved)
                {
					double diffX = Math.Abs(this.touchDownPoint.X - e.TouchPoint.X);
					double diffY = Math.Abs(this.touchDownPoint.Y - e.TouchPoint.Y);

					if (diffX >= 20 || diffY >= 20)
					{
						this.RemoveRippleEffect();
						this.RemoveHighlightEffect();
					}
				}
			}
		}

		/// <summary>
		/// Highlight Animation update method.
		/// </summary>
		/// <param name="value">Animation update value.</param>
		private void OnHighlightAnimationUpdate(double value)
		{

		}

		/// <summary>
		/// Animation ended method.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <param name="completed">Completed property</param>
		private void OnAnimationFinished(double value, bool completed)
		{
			this.highlightEffectLayer?.UpdateHighlightBounds();
			if ((this.rippleEffectLayer != null && !this.rippleEffectLayer.AnimationIsRunning("RippleAnimator")) || (rippleEffectLayer == null && (TouchUpEffects.GetAllItems().Contains(SfEffects.None) || TouchUpEffects.GetAllItems().Contains(SfEffects.Highlight))) && (LongPressEffects.GetAllItems().Contains(SfEffects.None) || !LongPressHandled || LongPressEffects.GetAllItems().Contains(SfEffects.Highlight)))
			{
				this.InvokeAnimationCompletedEvent();
			}
		}

		/// <summary>
		/// Tap method.
		/// </summary>
		/// <param name="e"> Tap event arguments.</param>
		public void OnTap(TapEventArgs e)
		{
			if (!ShouldIgnoreTouches)
			{
				this.LongPressHandled = false;

				if (TouchUpEffects != SfEffects.None)
				{
					this.AddEffects(TouchUpEffects, e.TapPoint);
				}
				if (TouchUpEffects.GetAllItems().Contains(SfEffects.Scale))
				{
					this.StartScaleAnimation();
				}
			}

		}

		/// <summary>
		/// LongPress method.
		/// </summary>
		/// <param name="e">The Long press event arguments.</param>
		public void OnLongPress(LongPressEventArgs e)
		{
			if (!ShouldIgnoreTouches)
			{
				InvokeLongPressedEventAndCommand();
				LongPressHandled = true;

				if (AutoResetEffects == AutoResetEffects.None)
				{
					AddEffects(LongPressEffects.ComplementsOf(TouchDownEffects), e.TouchPoint);
				}
			}

		}

		#endregion
	}
}
