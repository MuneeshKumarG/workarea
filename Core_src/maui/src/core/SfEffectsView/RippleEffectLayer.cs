using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using Syncfusion.Maui.Graphics.Internals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syncfusion.Maui.Core
{
	/// <summary>
	/// RippleEffectLayer
	/// </summary>
	internal class RippleEffectLayer : DrawableView
	{
		#region fields

		private float rippleDiameter = 0;
		private string rippleAnimatorName = "RippleAnimator";
		private string fadeOutName = "RippleFadeOut";
		private Point touchPoint;
		private double animationAreaLength;
		private float alphaValue;
		private bool fadeOutRipple;
		private Brush rippleColor = new SolidColorBrush(Colors.Black);
		private double rippleAnimationDuration;
		private float minAnimationDuration = 1f;
		private bool removeRippleAnimation;
		private const float rippleTransparencyFactor = 0.12f;

		#endregion

		#region constructor

		/// <summary>
		/// RippleEffectLayer.
		/// </summary>
		/// <param name="_rippleColor">The RippleColor.</param>
		/// <param name="rippleDuration">The RippleDuration.</param>
		public RippleEffectLayer(Brush _rippleColor, double rippleDuration)
		{
			this.rippleColor = _rippleColor;
			this.rippleAnimationDuration = rippleDuration;
			this.IsEnabled = false;
			this.alphaValue = rippleTransparencyFactor;
		}

		#endregion

		/// <summary>
		/// Gets or sets a value indicating whether to clear the animation . 
		/// </summary>
		internal bool CanRemoveRippleAnimation
		{
			get { return removeRippleAnimation; }
			set { removeRippleAnimation = value; }
		}


		/// <summary>
		/// Gets the ripple fade in and fade out animation duration in milliseconds.
		/// </summary>
		private float RippleFadeInOutAnimationDuration
		{
			get
			{
				return (float)((this.rippleAnimationDuration < this.minAnimationDuration
					? this.minAnimationDuration : this.rippleAnimationDuration) / 4);
			}
		}


		#region methods

		/// <summary>
		/// The Draw method.
		/// </summary>
		/// <param name="canvas">The canvas.</param>
		/// <param name="dirtyRect">The rectangle.</param>
		public override void Draw(ICanvas canvas, RectangleF dirtyRect)
		{
			if(this.rippleColor!=null)
			{
				canvas.Alpha = this.alphaValue;
				canvas.SetFillPaint(this.rippleColor, dirtyRect);
				this.ExpandRippleEllipse(canvas);
			}
		}

		/// <summary>
		/// Method used to initial radius for ripple.
		/// </summary>
		/// <param name="initialRippleFactor"></param>
		/// <returns>Returns the converted radius value</returns>
		private float GetRippleRadiusFromFactor(float initialRippleFactor)
		{
			if (this.Width > 0 && this.Height > 0)
			{
				return (float)(Math.Min(this.Width, this.Height) / 2 * initialRippleFactor);
			}
			else if (this.Parent != null && this.Parent is Grid && (this.Parent as Grid)?.Width > 0 && (this.Parent as Grid)?.Height > 0)
			{
				float parentWidth = (float)((Grid)this.Parent).Width;
				float parentHeight = (float)((Grid)this.Parent).Height;
				return (float)(Math.Min(parentWidth, parentHeight) / 2 * initialRippleFactor);
			}
			return 0;
		}

		/// <summary>
		/// Get the maximum radius based on the pythagoras theorem in the view.
		/// </summary>
		/// <param name="pivot">The touch point.</param>
		/// <returns>Final radius</returns>
		private float GetFinalRadius(Point pivot)
		{
			if (this.Width > 0 && this.Height > 0)
			{
				float width = (float)(pivot.X > this.Width / 2 ? pivot.X : this.Width - pivot.X);
				float height = (float)(pivot.Y > this.Height / 2 ? pivot.Y : this.Height - pivot.Y);
				return (float)Math.Sqrt((width * width) + (height * height));
			}
			else if (this.Parent != null && this.Parent is Grid && (this.Parent as Grid)?.Width > 0 && (this.Parent as Grid)?.Height > 0)
			{
				float parentWidth = (float)((Grid)this.Parent).Width;
				float parentHeight = (float)((Grid)this.Parent).Height;
				float width = (float)(pivot.X > parentWidth / 2 ? pivot.X : parentWidth - pivot.X);
				float height = (float)(pivot.Y > parentHeight / 2 ? pivot.Y : parentHeight - pivot.Y);
				return (float)Math.Sqrt((width * width) + (height * height));
			}
			else
			{
				return (float)Math.Sqrt((pivot.X * pivot.X) + (pivot.Y * pivot.Y));
			}
		}

		/// <summary>
		/// Expand ripple ellipse method.
		/// </summary>
		/// <param name="canvas">The canvas.</param>
		private void ExpandRippleEllipse(ICanvas canvas)
		{
			canvas.FillCircle((float)this.touchPoint.X, (float)this.touchPoint.Y, this.rippleDiameter);
		}

		/// <summary>
		/// Ripple Animation finished method.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <param name="isCompleted">The completed property.</param>
		private void OnRippleFinished(double value, bool isCompleted)
		{
			if (this.CanRemoveRippleAnimation)
			{
				AnimationExtensions.AbortAnimation(this, this.rippleAnimatorName);
				this.rippleDiameter = 0;
				this.InvalidateDrawable();
				if (this.Parent != null && ((this.Parent as Grid)?.Parent as SfEffectsView) != null)
				{
					SfEffectsView? effectsView = (this.Parent as Grid)?.Parent as SfEffectsView;
					if (effectsView != null && ((effectsView.TouchUpEffects == SfEffects.None || effectsView.AutoResetEffects.GetAllItems().Contains (AutoResetEffects.Ripple) || effectsView.TouchUpEffects == SfEffects.Ripple || effectsView.TouchUpEffects.GetAllItems().Contains(SfEffects.Ripple) || effectsView.TouchUpEffects.GetAllItems().Contains(SfEffects.None)) &&
						(effectsView.LongPressEffects.GetAllItems().Contains(SfEffects.None) || !effectsView.LongPressHandled || effectsView.LongPressEffects.GetAllItems().Contains(SfEffects.Ripple))))
					{
						effectsView?.InvokeAnimationCompletedEvent();
					}
				}
			}

		}

		/// <summary>
		/// Start ripple animation method.
		/// </summary>
		/// <param name="point">The touch point.</param>
		/// <param name="_rippleColor">The ripple color.</param>
		/// <param name="_rippleAnimationDuration">The ripple aniamtion duration.</param>
		/// <param name="initialRippleFactor">The initial ripple factor value.</param>
		/// <param name="fadeoutRipple">The fadeout ripple property.</param>
		/// <param name="canRepeat">The can repeat value.</param>
		internal void StartRippleAnimation(Point point, Brush _rippleColor, double _rippleAnimationDuration, float initialRippleFactor, bool fadeoutRipple, bool canRepeat = false)
		{
			this.touchPoint = point;
			this.rippleColor = _rippleColor;
			this.rippleAnimationDuration = _rippleAnimationDuration;
			this.fadeOutRipple = fadeoutRipple;
			this.alphaValue = rippleTransparencyFactor;
			double initialRippleRadius = GetRippleRadiusFromFactor(initialRippleFactor);
			this.animationAreaLength = GetFinalRadius(point);

			var rippleRadiusAnimation = new Animation(OnRippleAnimationUpdate, initialRippleRadius, animationAreaLength);
			rippleRadiusAnimation.Commit(this, this.rippleAnimatorName, length: (uint)rippleAnimationDuration, easing: Easing.Linear,
				finished: OnRippleFinished, repeat: () => canRepeat);

			if (fadeoutRipple)
			{
				var fadeOutAnimation = new Animation(OnFadeAnimationUpdate, 0, this.alphaValue);
				fadeOutAnimation.Commit(this, this.fadeOutName, length: (uint)RippleFadeInOutAnimationDuration, easing: Easing.Linear,
					finished: null, repeat: () => canRepeat);
				rippleRadiusAnimation.WithConcurrent(fadeOutAnimation);
			}
		}

		/// <summary>
		/// Fadeanimation update method.
		/// </summary>
		/// <param name="value">The animation update value.</param>
		internal void OnFadeAnimationUpdate(double value)
		{
			this.alphaValue = (float)value;
			this.InvalidateDrawable();
		}

		/// <summary>
		/// Ripple animation update method.
		/// </summary>
		/// <param name="value">Animation update value.</param>
		internal void OnRippleAnimationUpdate(double value)
		{
			this.rippleDiameter = (float)value;
			this.InvalidateDrawable();
		}

		/// <summary>
		/// Ripple animation finished method
		/// </summary>
		/// <param name="value">The value.</param>
		/// <param name="isCompleted">The completed property.</param>
		internal void OnRippleAnimationFinished(double value, bool isCompleted)
		{
			AnimationExtensions.AbortAnimation(this, this.rippleAnimatorName);
			this.rippleDiameter = 0;
			this.InvalidateDrawable();
		}
		#endregion
	}
}
