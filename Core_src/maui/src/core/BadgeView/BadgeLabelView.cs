// <copyright file="BadgeLabelView.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Syncfusion.Maui.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.Maui;
    using Microsoft.Maui.Controls;
    using Microsoft.Maui.Graphics;
    using Syncfusion.Maui.Graphics.Internals;
    using CanvasExtensions = Syncfusion.Maui.Graphics.Internals.CanvasExtensions;

    /// <summary>
    /// Represents the BadgeLabelView class.
    /// </summary>
    internal class BadgeLabelView : DrawableView
    {
        #region Fields

        private bool animationEnabled;

        private ITextElement? textElement;

        private double sizeRatio = 1;

        private View? content;

        private BadgeIcon badgeIcon = BadgeIcon.None;

        private Color stroke = Colors.Transparent;

        private double borderWidth;

        private Brush badgeBackground = new SolidColorBrush(Colors.Transparent);

        private Color textColor = Colors.Transparent;

        private string text = string.Empty;
		
		private string screenReaderText = String.Empty;

        private Thickness textPadding;

        private CornerRadius cornerRadius;

        private float fontSize = 12;

        private string fontFamily = string.Empty;

        private FontAttributes fontAttributes = FontAttributes.None;

        private bool autoHide;

        private Size textSize;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="BadgeLabelView"/> class.
        /// </summary>
        public BadgeLabelView()
        {
            this.VerticalOptions = LayoutOptions.Center;
            this.HorizontalOptions = LayoutOptions.Center;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets a value indicating whether the animation is enabled or not.
        /// </summary>
        internal bool AnimationEnabled
        {
            get { return this.animationEnabled; }
            set { this.animationEnabled = value; }
        }

        /// <summary>
        /// Gets or sets the value that defines the text element.
        /// </summary>
        internal ITextElement? TextElement
        {
            get { return this.textElement; }
            set { this.textElement = value; }
        }

        /// <summary>
        /// Gets or sets the value that defines the content.
        /// </summary>
        internal View? Content
        {
            get { return this.content; }
            set { this.content = value; }
        }

        /// <summary>
        /// Gets or sets the value that defines the badge icon.
        /// </summary>
        internal BadgeIcon BadgeIcon
        {
            get
            {
                return this.badgeIcon;
            }

            set
            {
                this.badgeIcon = value;
                this.InvalidateDrawable();
            }
        }

        /// <summary>
        /// Gets or sets the value that defines the border width.
        /// </summary>
        internal double BorderWidth
        {
            get
            {
                return this.borderWidth;
            }

            set
            {
                this.borderWidth = value;
                this.InvalidateDrawable();
            }
        }

        /// <summary>
        /// Gets or sets the value that defines the stroke color.
        /// </summary>
        internal Color Stroke
        {
            get
            {
                return this.stroke;
            }

            set
            {
                this.stroke = value;
                this.InvalidateDrawable();
            }
        }

        /// <summary>
        /// Gets or sets the value that defines the background color of the badge.
        /// </summary>
        internal Brush BadgeBackground
        {
            get
            {
                return this.badgeBackground;
            }

            set
            {
                this.badgeBackground = value;
                this.InvalidateDrawable();
            }
        }

        /// <summary>
        /// Gets or sets the value that defines the text color.
        /// </summary>
        internal Color TextColor
        {
            get
            {
                return this.textColor;
            }

            set
            {
                this.textColor = value;
                this.InvalidateDrawable();
            }
        }

        /// <summary>
        /// Gets or sets the value that defines the badge text.
        /// </summary>
        internal string Text
        {
            get
            {
                return this.text;
            }

            set
            {
                this.text = value;
                this.ShowBadgeBasedOnText(this.text, this.AutoHide);
                this.InvalidateDrawable();
				this.UpdateSemanticProperties();
            }
        }


        /// <summary>
        /// Gets or sets the value for screen reader text.
        /// </summary>
        internal string ScreenReaderText
        {
            get
            {
                return this.screenReaderText;
            }
            set
            {
                this.screenReaderText = value;
                this.UpdateSemanticProperties();
            }
        }

        /// <summary>
        /// Gets or sets the value that defines the text padding.
        /// </summary>
        internal Thickness TextPadding
        {
            get
            {
                return this.textPadding;
            }

            set
            {
                this.textPadding = value;
                this.InvalidateDrawable();
            }
        }

        /// <summary>
        /// Gets or sets the value that defines the corner radius.
        /// </summary>
        internal CornerRadius CornerRadius
        {
            get
            {
                return this.cornerRadius;
            }

            set
            {
                this.cornerRadius = value;
                this.InvalidateDrawable();
            }
        }

        /// <summary>
        /// Gets or sets the value that defines the font size.
        /// </summary>
        internal float FontSize
        {
            get
            {
                return this.fontSize;
            }

            set
            {
                this.fontSize = value;
                this.InvalidateDrawable();
            }
        }

        /// <summary>
        /// Gets or sets the value that defines the font family.
        /// </summary>
        internal string FontFamily
        {
            get
            {
                return this.fontFamily;
            }

            set
            {
                this.fontFamily = value;
                this.InvalidateDrawable();
            }
        }

        /// <summary>
        /// Gets or sets the value that defines the font attributes.
        /// </summary>
        internal FontAttributes FontAttributes
        {
            get
            {
                return this.fontAttributes;
            }

            set
            {
                this.fontAttributes = value;
                this.InvalidateDrawable();
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the badge text is AutoHide or not.
        /// </summary>
        internal bool AutoHide
        {
            get
            {
                return this.autoHide;
            }

            set
            {
                this.autoHide = value;
                this.ShowBadgeBasedOnText(this.Text, this.autoHide);
            }
        }

        #endregion

        #region Override methods

        /// <summary>
        /// Method used to draw the badge.
        /// </summary>
        /// <param name="canvas">canvas.</param>
        /// <param name="dirtyRect">dirtyRect.</param>
        public override void Draw(ICanvas canvas, RectangleF dirtyRect)
        {
            base.Draw(canvas, dirtyRect);
            this.DrawBadge(canvas);
        }

        #endregion

        #region Internal methods

        /// <summary>
        /// To get the start point of the text.
        /// </summary>
        /// <param name="viewBounds">viewBounds.</param>
        /// <returns>It returns the start point of the text.</returns>
        internal PointF GetTextStartPoint(Rectangle viewBounds)
        {
            PointF startPoint = default(PointF);
            startPoint.X = (float)(viewBounds.Center.X - (this.textSize.Width / 2));
            startPoint.Y = (float)(viewBounds.Center.Y - (this.textSize.Height / 2));
            if (!string.IsNullOrEmpty(this.Text)) 
            { 
#if __ANDROID__
            startPoint.Y = (float)(viewBounds.Center.Y + (this.textSize.Height / 2));
#elif __IOS__ || __MACCATALYST__
            startPoint.Y = (float)(viewBounds.Center.Y - (this.textSize.Height / 2) - 3);
#endif
            }

            return startPoint;
        }

        /// <summary>
        /// To calculate the badge bounds.
        /// </summary>
        /// <returns>It returns the badge bounds.</returns>
        internal RectangleF CalculateBadgeBounds()
        {
            RectangleF rect = default(RectangleF);

            if (!string.IsNullOrEmpty(this.Text))
            {
                var textMeasurer = TextMeasurer.CreateTextMeasurer();
                this.textSize = textMeasurer.MeasureText(this.Text, this.GetProccessedFontSize(), this.FontAttributes, this.FontFamily);
            }
            else if (this.BadgeIcon != BadgeIcon.None && this.BadgeIcon != BadgeIcon.Dot)
            {
                this.textSize = new Size(this.GetProccessedFontSize(), this.GetProccessedFontSize());
            }
            
            rect.Width = (float)(this.textSize.Width + this.TextPadding.Left + this.TextPadding.Right);
            rect.Height = (float)(this.textSize.Height + this.TextPadding.Top + this.TextPadding.Bottom);

            if (this.BadgeIcon == BadgeIcon.Dot && (this.Text == null || (this.Text != null && this.Text.Length == 0)))
            {
                rect.Width = rect.Height = 10;
            }
            else if (this.Text != null)
            {
                if (this.Text.Length == 1 && this.TextPadding.Left == this.TextPadding.Right &&
                    this.TextPadding.Top == this.TextPadding.Bottom &&
                    this.TextPadding.Left == this.TextPadding.Top)
                {
                    if (rect.Width > rect.Height)
                    {
                        rect.Height = rect.Width;
                    }
                    else
                    {
                        rect.Width = rect.Height;
                    }
                }
                else if (this.Text.Length > 1)
                {
                    // Added some default padding for two or more digits.
                    rect.Width += 10;
                }
            }

            if (this.sizeRatio >= 1)
            {
                this.WidthRequest = rect.Width;
                this.HeightRequest = rect.Height;
            }
            else
            {
                rect.Width = (float)(rect.Width * this.sizeRatio);
                rect.Height = (float)(rect.Height * this.sizeRatio);
                rect.X = (float)(this.WidthRequest - rect.Width) / 2;
                rect.Y = (float)(this.HeightRequest - rect.Height) / 2;
            }

            return rect;
        }

        /// <summary>
        /// Method used to show the badge.
        /// </summary>
        internal void Show()
        {
            if (this.AnimationEnabled)
            {
                this.IsVisible = true;
                this.StartShowHideAnimation(0, 1);
            }
            else
            {
                this.IsVisible = true;
            }
        }

        /// <summary>
        /// Method used to hide the badge.
        /// </summary>
        internal void Hide()
        {
            if (this.AnimationEnabled)
            {
                this.StartShowHideAnimation(1, 0);
            }
            else
            {
                this.IsVisible = false;
            }
        }

        /// <summary>
        /// To update the badge animation.
        /// </summary>
        /// <param name="value">value.</param>
        internal void OnShowHideAnimationUpdate(double value)
        {
            this.sizeRatio = value;
            this.InvalidateDrawable();
        }

        /// <summary>
        /// To update the visibility of the badge when the animation is completed.
        /// </summary>
        /// <param name="value">value.</param>
        /// <param name="isCompleted">isCompleted.</param>
        internal void OnShowHideAnimationEnded(double value, bool isCompleted)
        {
            if (value == 0 && isCompleted)
            {
                this.IsVisible = false;
            }
        }

        /// <summary>
        /// Method used to hide the badge text when the value is empty or zero.
        /// </summary>
        /// <param name="value">value.</param>
        /// <param name="canHide">canHide.</param>
        internal void ShowBadgeBasedOnText(string value, bool canHide)
        {
            if (string.IsNullOrEmpty(value) || value == "0")
            {
                if (canHide)
                {
                    // TODO: Need to enable once below issue resolved.
                    // TODO: 'Unable to find IAnimationManager for 'Syncfusion.Maui.Core.BadgeLabelView'. (Parameter 'animatable')' exception occuring when parent not set. Need to check on next Preview (Preview 11) and remove this try catch once its fixed.
                    // this.Hide();
                    this.IsVisible = false;
                }
                else
                {
                    this.Show();
                }
            }
            else
            {
                this.Show();
            }
        }

        #endregion

        #region Private methods

        private void DrawBadge(ICanvas canvas)
        {
            var currentBounds = this.CalculateBadgeBounds();
            var fillBounds = default(RectangleF);
            fillBounds.X = (float)(currentBounds.X + (this.BorderWidth / 3));
            fillBounds.Y = (float)(currentBounds.Y + (this.BorderWidth / 3));
            fillBounds.Right = (float)(currentBounds.Right - (this.BorderWidth / 3));
            fillBounds.Bottom = (float)(currentBounds.Bottom - (this.BorderWidth / 3));

            var backgroundBounds = default(RectangleF);
            backgroundBounds.X = (float)(currentBounds.X + (this.BorderWidth / 2));
            backgroundBounds.Y = (float)(currentBounds.Y + (this.BorderWidth / 2));
            backgroundBounds.Right = (float)(currentBounds.Right - (this.BorderWidth / 2));
            backgroundBounds.Bottom = (float)(currentBounds.Bottom - (this.BorderWidth / 2));

            canvas.SetFillPaint(this.BadgeBackground, fillBounds);
            canvas.FillRoundedRectangle(fillBounds, this.CornerRadius.TopLeft, this.CornerRadius.TopRight, this.CornerRadius.BottomLeft, this.CornerRadius.BottomRight);
            canvas.StrokeSize = (float)this.BorderWidth;
            canvas.StrokeColor = this.Stroke;
            if (this.BorderWidth > 0)
            {
                canvas.DrawRoundedRectangle(backgroundBounds, this.CornerRadius.TopLeft, this.CornerRadius.TopRight, this.CornerRadius.BottomLeft, this.CornerRadius.BottomRight);
            }

            PointF startPoint = this.GetTextStartPoint(currentBounds);
            if (!string.IsNullOrEmpty(this.text))
            {
                canvas.DrawText(this.text, startPoint.X, startPoint.Y, this.TextElement!);
            }
            else
            {
                var rect = new RectangleF(startPoint.X, startPoint.Y, this.GetProccessedFontSize(), this.GetProccessedFontSize());
                canvas.StrokeColor = this.TextColor;
                canvas.StrokeSize = 1.5f;
                canvas.SetFillPaint(new SolidColorBrush(this.TextColor), rect);
                GetBadgeIcon(canvas, rect, this.BadgeIcon);
            }
        }

        /// <summary>
        /// To get the badge icon.
        /// </summary>
        private static void GetBadgeIcon(ICanvas canvas, RectangleF rect, BadgeIcon value)
        {
            float x = rect.X;
            float y = rect.Y;
            float width = x + rect.Width;
            float height = y + rect.Height;
            float midHeight = y + (rect.Height / 2);

            switch (value)
            {
                case BadgeIcon.Add:
                    rect.X += rect.Width / 12;
                    rect.Y += rect.Height / 12;
                    rect.Width -= rect.Width / 6;
                    rect.Height -= rect.Height / 6;
                    CanvasExtensions.DrawPlus(canvas, rect, false, rect.Width + 4);
                    break;

                case BadgeIcon.Available:
                    CanvasExtensions.DrawTick(canvas, rect);
                    break;

                case BadgeIcon.Away:
                    CanvasExtensions.DrawAwaySymbol(canvas, rect);
                    break;

                case BadgeIcon.Busy:
                    canvas.DrawLine(new PointF(x, midHeight), new PointF(width, midHeight));
                    break;

                case BadgeIcon.Delete:
                    rect.X += rect.Width / 6;
                    rect.Y += rect.Height / 6;
                    rect.Width -= rect.Width / 3;
                    rect.Height -= rect.Height / 3;
                    CanvasExtensions.DrawCross(canvas, rect, false, rect.Width);
                    break;

                case BadgeIcon.Prohibit1:
                    canvas.DrawLine(new PointF(width - 1, y + 1), new PointF(x + 1, height - 1));
                    break;

                case BadgeIcon.Prohibit2:
                    canvas.DrawLine(new PointF(x + 1, y + 1), new PointF(width - 1, height - 1));
                    break;
            }
        }

        private void UpdateSemanticProperties()
        {
            if (String.IsNullOrEmpty(this.ScreenReaderText))
            {
                SemanticProperties.SetDescription(this, this.Text);
            } 
            else
            {
                SemanticProperties.SetDescription(this, this.ScreenReaderText);
            }
        }
		
        private float GetProccessedFontSize()
        {
            var textSize = (float)(this.FontSize * this.sizeRatio);

            if (textSize <= 1)
            {
                textSize = 1;
            }

            return textSize;
        }

        private void StartShowHideAnimation(double startValue, double endValue)
        {
            // TODO: 'Unable to find IAnimationManager for 'Syncfusion.Maui.Core.BadgeLabelView'. (Parameter 'animatable')' exception occuring when parent not set. Need to check on next Preview (Preview 11) and remove this try catch once its fixed.
            try
            {
                var fadeOutAnimation = new Animation(this.OnShowHideAnimationUpdate, startValue, endValue);
                fadeOutAnimation.Commit(
                    this,
                    "showAnimator",
                    length: 250U,
                    easing: Easing.Linear,
                    finished: this.OnShowHideAnimationEnded,
                    repeat: () => false);
            }
            catch (ArgumentException)
            {
            }
        }

        #endregion
    }
}
