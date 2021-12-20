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
    /// 
    /// </summary>
    internal class BadgeLabelView : DrawableView
    {
        #region Fields

        internal bool AnimationEnabled = false;

        private double sizeRatio = 1;

        private View? content;

        private BadgeIcon badgeIcon = BadgeIcon.None;

        private Color stroke = Colors.Transparent;

        private double borderWidth = 0;

        private Brush badgeBackground = new SolidColorBrush(Colors.Transparent);

        private Color textColor = Colors.Transparent;

        private String text = String.Empty;

        private Thickness textPadding = new Thickness();

        private CornerRadius cornerRadius = new CornerRadius();

        private float fontSize = 12;

        private String fontFamily = String.Empty;

        private FontAttributes fontAttributes = FontAttributes.None;

        private bool autoHide = false;

        private Size textSize;

        internal ITextElement? TextElement;

        #endregion

        #region Properties

        internal View? Content
        {
            get
            {
                return content;
            }
            set
            {
                content = value;
            }
        }

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

        internal String Text
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
            }
        }

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

        internal String FontFamily
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

        #region Constructor

        public BadgeLabelView()
        {
            this.VerticalOptions = LayoutOptions.Center;
            this.HorizontalOptions = LayoutOptions.Center;
        }

        #endregion

        #region Override Methods

        public override void Draw(ICanvas canvas, RectangleF dirtyRect)
        {
            base.Draw(canvas, dirtyRect);
            this.DrawBadge(canvas);
        }

        #endregion

        #region Methods

        private void DrawBadge(ICanvas canvas)
        {
            var currentBounds = this.CalculateBadgeBounds();
            var fillBounds = new RectangleF();
            fillBounds.X = (float)((currentBounds.X + this.BorderWidth / 3));
            fillBounds.Y = (float)((currentBounds.Y + this.BorderWidth / 3));
            fillBounds.Right = (float)((currentBounds.Right - this.BorderWidth / 3));
            fillBounds.Bottom = (float)((currentBounds.Bottom - this.BorderWidth / 3));

            var backgroundBounds = new RectangleF();
            backgroundBounds.X = (float)(currentBounds.X + this.BorderWidth / 2);
            backgroundBounds.Y = (float)(currentBounds.Y + this.BorderWidth / 2);
            backgroundBounds.Right = (float)(currentBounds.Right - this.BorderWidth / 2);
            backgroundBounds.Bottom = (float)(currentBounds.Bottom - this.BorderWidth / 2);

            canvas.SetFillPaint(this.BadgeBackground, fillBounds);
            canvas.FillRoundedRectangle(fillBounds, this.CornerRadius.TopLeft, this.CornerRadius.TopRight, this.CornerRadius.BottomLeft, this.CornerRadius.BottomRight);
            canvas.StrokeSize = (float)this.BorderWidth;
            canvas.StrokeColor = this.Stroke;
            if (this.BorderWidth > 0)
            {
                canvas.DrawRoundedRectangle(backgroundBounds, this.CornerRadius.TopLeft, this.CornerRadius.TopRight, this.CornerRadius.BottomLeft, this.CornerRadius.BottomRight);
            }

            PointF startPoint = GetTextStartPoint(currentBounds);
            canvas.DrawText(this.text, startPoint.X, startPoint.Y, TextElement!);
        }

        internal PointF GetTextStartPoint(Rectangle viewBounds)
        {
            PointF startPoint = new PointF();
            startPoint.X = (float)(viewBounds.Center.X - textSize.Width / 2);

#if __ANDROID__
           startPoint.Y = (float)(viewBounds.Center.Y + (textSize.Height / 2));
#elif __IOS__ || __MACCATALYST__
           startPoint.Y = (float)(viewBounds.Center.Y - (textSize.Height / 2) - 3);
#else
            startPoint.Y = (float)(viewBounds.Center.Y - (textSize.Height / 2));
#endif
            return startPoint;
        }

        internal RectangleF CalculateBadgeBounds()
        {
            RectangleF rect = new RectangleF();

            var textMeasurer = TextMeasurer.CreateTextMeasurer();
            textSize = textMeasurer.MeasureText(this.Text, this.GetProccessedFontSize(), this.FontAttributes, this.FontFamily);
            rect.Width = (float)(textSize.Width + this.TextPadding.Left + this.TextPadding.Right);
            rect.Height = (float)(textSize.Height + this.TextPadding.Top + this.TextPadding.Bottom);

            if (this.BadgeIcon == BadgeIcon.Dot || this.Text == null || (this.Text != null && this.Text.Length == 0))
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
                    //Added some default padding for two or more digits
                    rect.Width += 10;
                }
            }

            if (sizeRatio >= 1)
            {
                this.WidthRequest = rect.Width;
                this.HeightRequest = rect.Height;
            }
            else
            {     
                rect.Width = (float)(rect.Width * sizeRatio);
                rect.Height = (float)(rect.Height * sizeRatio);
                rect.X = (float)(this.WidthRequest - rect.Width) / 2;
                rect.Y = (float)(this.HeightRequest - rect.Height) / 2;
            }

            return rect;
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

        internal void Show()
        {
            if (this.AnimationEnabled)
            {

                this.IsVisible = true;
                StartShowHideAnimation(0, 1);
            }
            else
            {
                this.IsVisible = true;
            }
        }

        internal void Hide()
        {
            if (this.AnimationEnabled)
            {
                StartShowHideAnimation(1, 0);
            }
            else
            {
                this.IsVisible = false;
            }
        }

        private void StartShowHideAnimation(double startValue, double endValue)
        {
            // TODO: 'Unable to find IAnimationManager for 'Syncfusion.Maui.Core.BadgeLabelView'. (Parameter 'animatable')' exception occuring when parent not set. Need to check on next Preview (Preview 11) and remove this try catch once its fixed.
            try
            {
                var fadeOutAnimation = new Animation(OnShowHideAnimationUpdate, startValue, endValue);
                fadeOutAnimation.Commit(this, "showAnimator", length: (uint)250, easing: Easing.Linear,
                    finished:OnShowHideAnimationEnded , repeat: () => false);
                
            }
            catch
            {

            }
        }

        internal void OnShowHideAnimationUpdate(double value)
        {
            this.sizeRatio = value;
            this.InvalidateDrawable();
        }

        internal void OnShowHideAnimationEnded(double value, bool isCompleted)
        {
            if(value == 0 && isCompleted)
            {
                this.IsVisible = false;
            }
        }

        internal void ShowBadgeBasedOnText(string value, bool canHide)
        {
            if (value == String.Empty || value == "0")
            {
                if (canHide)
                {
                    //TODO: Need to enable once below issue resolved
                    // TODO: 'Unable to find IAnimationManager for 'Syncfusion.Maui.Core.BadgeLabelView'. (Parameter 'animatable')' exception occuring when parent not set. Need to check on next Preview (Preview 11) and remove this try catch once its fixed.
                    //this.Hide();
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
    }
}
