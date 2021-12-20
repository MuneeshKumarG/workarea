using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using System;
using System.Collections.Generic;
using Rectangle = Microsoft.Maui.Graphics.Rectangle;

namespace Syncfusion.Maui.Core
{
    /// <summary>
    /// This class represents a content view to show tooltip in absolute layout.
    /// </summary>
    internal partial class SfTooltip
    {
        #region Fields

        const string tooltipAnimation = "Animation";

        const string durationAnimation = "Duration";

        private bool isTooltipActivate = false;

        private bool isDisappeared = false;

        private readonly MaterialToolTipRenderer tooltipRenderer;

        #endregion

        #region Public properties

        /// <summary>
        /// Gets or sets a value that indicates the position of tooltip.
        /// </summary>
        public TooltipPosition Position { get; set; } = TooltipPosition.Auto;

        /// <summary>
        /// Gets or sets the duration of the tooltip in seconds.
        /// </summary>
        public int Duration { get; set; } = 2;

        #endregion

        #region Internal properties

        internal TooltipPosition ActualPosition { get; set; } = TooltipPosition.Auto;

        #endregion

        #region Events

        /// <summary>
        /// It represents the tooltip closed event handler. This tooltip closed event is hooked when tooltip is disappear from the visibility.
        /// </summary>
        public event EventHandler<TooltipClosedEventArgs>? TooltipClosed;

        #endregion

        #region Constructor

        /// <summary>
        /// Initialize a new instance of the <see cref="SfTooltip"/> class.
        /// </summary>
        public SfTooltip()
        {
            Background = new SolidColorBrush(Colors.Black);
            tooltipRenderer = new MaterialToolTipRenderer();
            Margin = new Thickness(0, 2);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Show the tooltip based on target rectangle.
        /// </summary>
        /// <param name="containerRect"></param>
        /// <param name="targetRect"></param>
        /// <param name="animated"></param>
        public void Show(Rectangle containerRect, Rectangle targetRect, bool animated)
        {
            if (containerRect.IsEmpty || targetRect.IsEmpty || Content == null) return;

            if (isTooltipActivate)
            {
                isDisappeared = false;
                Hide();
            }

            //Workaround fix for the layout bounds while hide the tooltip. It will remove after the stable release in MAUI.
            if (this.Opacity == 0f)
                this.Opacity = 1f;

            ActualPosition = Position;

            if (Content.Margin.IsEmpty)
                Padding = new Thickness(5, 3);

            var contentSize = Content.Measure(double.PositiveInfinity, double.PositiveInfinity, MeasureFlags.IncludeMargins).Request;

            if (Content.Margin.IsEmpty)
                contentSize = new Size(contentSize.Width + Padding.Left + Padding.Right, contentSize.Height + Padding.Top + Padding.Bottom);

#if WINDOWS
            //TODO: MAUI-690 - Facing content bottom corping at windows while tooltip having margin, So measured the content size with margin for windows alone.
            contentSize = new Size(contentSize.Width + Margin.Left + Margin.Right, contentSize.Height + Margin.Top + Margin.Bottom);
#endif
            tooltipRenderer.ContentSize = contentSize;

            Rectangle tooltipRect = tooltipRenderer.GetToolTipRect(Position, targetRect, containerRect);
            SetLayoutOption();

            if (Position == TooltipPosition.Auto && !this.IsTargetRectIntersectsWith(tooltipRect, targetRect))
            {
                List<TooltipPosition> list = new List<TooltipPosition>() { TooltipPosition.Top, TooltipPosition.Bottom, TooltipPosition.Left, TooltipPosition.Right };

                foreach (TooltipPosition position in list)
                {
                    var newFrame = tooltipRenderer.GetToolTipRect(position, targetRect, containerRect);

                    if (this.IsTargetRectIntersectsWith(newFrame, targetRect))
                    {
                        tooltipRect = newFrame;
                        ActualPosition = position;
                        break;
                    }
                }
            }


            tooltipRenderer.SetNosePoint(ActualPosition,targetRect, tooltipRect);

            //Todo: Commented the below lines due to break in clip geometry after MAUI preview 9 release
            //PointCollection points = tooltipRenderer.GetGeometryPoints(ActualPosition, tooltipRect);

            //Clip = GetClipPathGeometry(points, tooltipRect);

            AbsoluteLayout.SetLayoutBounds(this, tooltipRect);

            isTooltipActivate = true;
            if (animated)
            {
                StartAnimation();
            }
            else
            {
                AutoHide();
            }
        }

        /// <summary>
        /// Hides the tooltip.
        /// </summary>
        public void Hide()
        {
            this.AbortAnimation(tooltipAnimation);
            this.AbortAnimation(durationAnimation);
            //Todo: Workaround fix for the layout bounds while hide the tooltip. It will remove after the stable release in MAUI.
            this.Opacity = 0f;
            AbsoluteLayout.SetLayoutBounds(this, new Rectangle(0,0,1,1));
            isTooltipActivate = false;
            this.TooltipClosed?.Invoke(this, new TooltipClosedEventArgs() { IsCompleted = isDisappeared});
        }

        private void StartAnimation()
        {
            var parentAnimation = new Animation();
            var scaleXAnimation = new Animation(v => this.ScaleX = v, 0.5, 1, Easing.Linear);
            var scaleYAnimation = new Animation(v => this.ScaleY = v, 0.5, 1, Easing.Linear);
            parentAnimation.Add(0, 1, scaleXAnimation);
            parentAnimation.Add(0, 1, scaleYAnimation);
            parentAnimation.Commit(this, tooltipAnimation, 16, 100, null, OnAnimationEnd, () => false);
        }

        private void OnAnimationEnd(double value, bool isCompleted)
        {
            ScaleX = 1;
            ScaleY = 1;
            AutoHide();
        }

        private void AutoHide()
        {
            Animation animation = new Animation();
            animation.Commit(this, durationAnimation, 16, (uint)(Duration * 1000), null, Hide, () => false);
        }

        private void Hide(double value, bool isCompleted)
        {
            isDisappeared = !isCompleted;

            if (!isCompleted)
                Hide();
        }

        private bool IsTargetRectIntersectsWith(Rectangle tooltipRect, Rectangle targetRect)
        {
            return !tooltipRect.IntersectsWith(targetRect);
        }

        private void SetLayoutOption()
        {
            switch (Position)
            {
                case TooltipPosition.Top:
                case TooltipPosition.Auto:
#if MONOANDROID
                    Content.VerticalOptions = LayoutOptions.End;
#else
                    Content.VerticalOptions = LayoutOptions.Start;
#endif
                    Content.HorizontalOptions = LayoutOptions.End;
                    break;

                case TooltipPosition.Bottom:
                    Content.VerticalOptions = LayoutOptions.End;
                    Content.HorizontalOptions = LayoutOptions.End;
                    break;

                case TooltipPosition.Right:
                    Content.VerticalOptions = LayoutOptions.End;
                    Content.HorizontalOptions = LayoutOptions.End;
                    break;

                case TooltipPosition.Left:
                    Content.VerticalOptions = LayoutOptions.End;
                    Content.HorizontalOptions = LayoutOptions.Start;
                    break;
            }
        }

#endregion
    }
}
