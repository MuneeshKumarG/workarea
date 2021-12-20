using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Shapes;
using Microsoft.Maui.Graphics;
using Syncfusion.Maui.Graphics.Internals;
using System;
using System.Collections.Generic;

namespace Syncfusion.Maui.Core
{
    /// <summary>
    /// Class implementation of badge setting.
    /// </summary>
    public class BadgeSettings : Element, ITextElement
    {
        #region Fields

        internal SfBadgeView? badgeView;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the value for badge view.
        /// </summary>
        internal SfBadgeView? BadgeView
        {
            get
            {
                return this.badgeView;
            }
            set
            {
                this.badgeView = value;
                this.ApplySettingstoUpdatedBadgeView();
            }
        }

        /// <summary>
        /// Gets or sets the background for the badge. The property will be set only if BadgeType is set to BadgeType.None. This is a bindable property.
        /// </summary>
        public Brush Background
        {
            get { return (Brush)this.GetValue(BackgroundProperty); }
            set { this.SetValue(BackgroundProperty, value); }
        }

        /// <summary>
        /// Gets or sets the border width for the badge. The border width will be visible only if border color is set not transparent. This is a bindable property.
        /// </summary>
        public double BorderWidth
        {
            get { return (double)this.GetValue(BorderWidthProperty); }
            set { this.SetValue(BorderWidthProperty, value); }
        }

        /// <summary>
        /// Gets or sets the corner radius of the badge. This is a bindable property.
        /// </summary>
        public CornerRadius CornerRadius
        {
            get { return (CornerRadius)GetValue(CornerRadiusProperty); }
            set { this.SetValue(CornerRadiusProperty, value); }
        }

        /// <summary>
        /// Gets or sets the border color for the badge. This is a bindable property.
        /// </summary>
        public Color Stroke
        {
            get { return (Color)this.GetValue(StrokeProperty); }
            set { this.SetValue(StrokeProperty, value); }
        }


        /// <summary>
        /// Gets or sets the position where the badge will be shown relative to the Content. This is a bindable property.
        /// </summary>
        public BadgePosition Position
        {
            get { return (BadgePosition)GetValue(PositionProperty); }
            set { this.SetValue(PositionProperty, value); }
        }

        //TODO: Need to fix the font icon issue in library or need to draw the icons
        /// <summary>
        /// Gets or sets the Icon to be displayed in the badge. BadgeText and BadgeSetting.FontFamily is not applied when this property set other than BadgeIcon.None. This is a bindable property.
        /// </summary>
        internal BadgeIcon Icon
        {
            get { return (BadgeIcon)GetValue(IconProperty); }
            set { this.SetValue(IconProperty, value); }
        }

        /// <summary>
        /// Gets or sets the value for offset.
        /// </summary>
        public Point Offset
        {
            get { return (Point)GetValue(OffsetProperty); }
            set { this.SetValue(OffsetProperty, value); }
        }

        /// <summary>
        /// Gets or sets the animation that should be used when the badge is shown. The animation only occurs if IsAnimated is set to true. This is a bindable property.
        /// </summary>
        public BadgeAnimation Animation
        {
            get { return (BadgeAnimation)GetValue(AnimationProperty); }
            set { this.SetValue(AnimationProperty, value); }
        }

        /// <summary>
        /// Gets or sets the type of the badge. BadgeSetting.BackgroundColor is not applied when this property set other than BadgeIcon.None. This is a bindable property.
        /// </summary>
        public BadgeType Type
        {
            get { return (BadgeType)GetValue(TypeProperty); }
            set { this.SetValue(TypeProperty, value); }
        }

        /// <summary>
        /// Gets or sets the padding around the text of the badge. This is a bindable property.
        /// </summary>
        public Thickness TextPadding
        {
            get { return (Thickness)GetValue(TextPaddingProperty); }
            set { this.SetValue(TextPaddingProperty, value); }
        }

        /// <summary>
        /// Gets or sets the value for auto hide.
        /// </summary>
        public bool AutoHide
        {
            get { return (bool)GetValue(AutoHideProperty); }
            set { this.SetValue(AutoHideProperty, value); }
        }

        /// <summary>
        /// Gets or sets the font size of the text to be displayed in badge over the control. This is a bindable property.
        /// </summary>
        [System.ComponentModel.TypeConverter(typeof(FontSizeConverter))]
        public double FontSize
        {
            get { return (double)this.GetValue(FontSizeProperty); }
            set { this.SetValue(FontSizeProperty, value); }
        }

        /// <summary>
        /// Gets or sets the font attributes of the text to be displayed in badge over the control. This is a bindable property.
        /// </summary>
        public FontAttributes FontAttributes
        {
            get { return (FontAttributes)GetValue(FontAttributesProperty); }
            set { this.SetValue(FontAttributesProperty, value); }
        }

        /// <summary>
        /// Gets or sets the font family of the text to be displayed in badge over the control. The property will be set only if BadgeIcon is set to BadgeIcon.None. This is a bindable property.
        /// </summary>
        public string FontFamily
        {
            get { return (string)GetValue(FontFamilyProperty); }
            set { this.SetValue(FontFamilyProperty, value); }
        }

        /// <summary>
        /// Gets or sets the color of the text to be displayed in badge over the control. This is a bindable property.
        /// </summary>
        public Color TextColor
        {
            get { return (Color)this.GetValue(TextColorProperty); }
            set { this.SetValue(TextColorProperty, value); }
        }

        /// <summary>
        /// Bindable property for <see cref="FontSize"/> property.
        /// </summary>
        public static readonly BindableProperty FontSizeProperty = FontElement.FontSizeProperty;

        /// <summary>
        /// Bindable property for <see cref="FontFamily"/> property
        /// </summary>
        public static readonly BindableProperty FontFamilyProperty = FontElement.FontFamilyProperty;

        /// <summary>
        /// Bindable property for <see cref="FontAttributes"/> property
        /// </summary>
        public static readonly BindableProperty FontAttributesProperty = FontElement.FontAttributesProperty;

        /// <summary>
        /// Bindable property for <see cref="Font"/> property
        /// </summary>
        Font ITextElement.Font => (Font)GetValue(FontElement.FontProperty);

        /// <summary>
        /// Bindable property for <see cref="TextColor"/> property.
        /// </summary>
        public static readonly BindableProperty TextColorProperty =
            BindableProperty.Create(nameof(TextColor), typeof(Color), typeof(BadgeSettings), Colors.White, BindingMode.Default, null, OnTextColorPropertyChanged);

        /// <summary>
        /// Bindable property for <see cref="Background"/> property.
        /// </summary>
        public static readonly BindableProperty BackgroundProperty =
            BindableProperty.Create(nameof(Background), typeof(Brush), typeof(BadgeSettings), new SolidColorBrush(Colors.Transparent), BindingMode.Default, null, OnBackgroundPropertyChanged);

        /// <summary>
        /// Bindable property for <see cref="BorderWidth"/> property.
        /// </summary>
        public static readonly BindableProperty BorderWidthProperty =
            BindableProperty.Create(nameof(BorderWidth), typeof(double), typeof(BadgeSettings), 0d, BindingMode.Default, null, OnBorderWidthPropertyChanged);

        /// <summary>
        /// Bindable property for <see cref="CornerRadius"/> property.
        /// </summary>
        public static readonly BindableProperty CornerRadiusProperty =
            BindableProperty.Create(nameof(CornerRadius), typeof(CornerRadius), typeof(BadgeSettings), new CornerRadius(25), BindingMode.Default, null, null, OnCornerRadiusPropertyChanged);

        /// <summary>
        /// Bindable property for <see cref="Stroke"/>property.
        /// </summary>
        public static readonly BindableProperty StrokeProperty =
            BindableProperty.Create(nameof(Stroke), typeof(Color), typeof(BadgeSettings), Colors.Transparent, BindingMode.Default, null, OnStrokePropertyChanged);

        /// <summary>
        /// Bindable property for <see cref="Position"/> property.
        /// </summary>
        public static readonly BindableProperty PositionProperty =
            BindableProperty.Create(nameof(Position), typeof(BadgePosition), typeof(BadgeSettings), BadgePosition.TopRight, BindingMode.Default, null, null, OnBadgePositionPropertyChanged);

        //TODO: Need to fix the font icon issue in library or need to draw the icons
        /// <summary>
        /// Bindable property for <see cref="Icon"/> property.
        /// </summary>
        internal static readonly BindableProperty IconProperty =
            BindableProperty.Create(nameof(Icon), typeof(BadgeIcon), typeof(BadgeSettings), BadgeIcon.None, BindingMode.Default, null, null, OnBadgeIconPropertyChanged);

        /// <summary>
        /// Bindable property for <see cref="Offset"/> property.
        /// </summary>
        public static readonly BindableProperty OffsetProperty =
            BindableProperty.Create(nameof(Offset), typeof(Point), typeof(BadgeSettings), new Point(2, 2), BindingMode.Default, null, null, OnOffsetPropertyChanged);

        /// <summary>
        /// Bindable property for <see cref="Animation"/> property.
        /// </summary>
        public static readonly BindableProperty AnimationProperty =
            BindableProperty.Create(nameof(Animation), typeof(BadgeAnimation), typeof(BadgeSettings), BadgeAnimation.Scale, BindingMode.Default, null, null, OnBadgeAnimationPropertyChanged);

        /// <summary>
        /// Bindable property for <see cref="Type"/> property.
        /// </summary>
        public static readonly BindableProperty TypeProperty =
            BindableProperty.Create(nameof(Type), typeof(BadgeType), typeof(BadgeSettings), BadgeType.Primary, BindingMode.Default, null, null, OnBadgeTypePropertyChanged);

        /// <summary>
        /// Bindable property for <see cref="TextPadding"/> property.
        /// </summary>
        public static readonly BindableProperty TextPaddingProperty =
            BindableProperty.Create(nameof(TextPadding), typeof(Thickness), typeof(BadgeSettings), GetTextPadding(), BindingMode.Default, null, null, OnTextPaddingPropertyChanged);

        /// <summary>
        /// Bindable property for <see cref="AutoHide"/> property.
        /// </summary>
        public static readonly BindableProperty AutoHideProperty =
            BindableProperty.Create(nameof(AutoHide), typeof(bool), typeof(BadgeSettings), false, BindingMode.Default, null, null, OnAutoHidePropertyChanged);

        #endregion

        #region Constructor

        /// <summary>
        ///  Initializes a new instance of the <see cref="BadgeSettings" /> class.
        /// </summary>
        public BadgeSettings()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        protected override void OnParentSet()
        {
            base.OnParentSet();

            if (this.Parent != null && this.Parent is SfBadgeView)
            {
                this.BadgeView = this.Parent as SfBadgeView;
            }
        }

        #endregion

        #region Property Updates

        double ITextElement.FontSizeDefaultValueCreator()
        {
            return 12d;
        }

        void ITextElement.OnFontAttributesChanged(FontAttributes oldValue, FontAttributes newValue)
        {
            this.UpdateFontAttributes(newValue);
        }

        void ITextElement.OnFontChanged(Font oldValue, Font newValue)
        {

        }

        void ITextElement.OnFontFamilyChanged(string oldValue, string newValue)
        {
            this.UpdateFontFamily(newValue);
        }

        void ITextElement.OnFontSizeChanged(double oldValue, double newValue)
        {
            this.UpdateFontSize(newValue);
        }

        private void UpdateFontAttributes(FontAttributes newValue)
        {
            if (this.BadgeView != null && this.BadgeView.BadgeLabelView != null)
            {
                this.BadgeView.BadgeLabelView.FontAttributes = newValue;
                this.BadgeView.BadgeLabelView.CalculateBadgeBounds();
            }
        }

        private void UpdateFontFamily(string newValue)
        {
            if (this.BadgeView != null && this.BadgeView.BadgeLabelView != null)
            {
                this.BadgeView.BadgeLabelView.FontFamily = newValue;
                this.BadgeView.BadgeLabelView.CalculateBadgeBounds();
            }
        }

        private void UpdateFontSize(double newValue)
        {
            if (this.BadgeView != null && this.BadgeView.BadgeLabelView != null)
            {
                this.BadgeView.BadgeLabelView.FontSize = (float)newValue;
                this.BadgeView.BadgeLabelView.CalculateBadgeBounds();
            }
        }

        /// <summary>
        /// Invoked when the <see cref="TextColorProperty"/> is set for badge control.
        /// </summary>
        /// <param name="bindable">The bindable.</param>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        private static void OnTextColorPropertyChanged(BindableObject bindable, object? oldValue, object newValue)
        {
            var settings = bindable as BadgeSettings;

            if (settings != null && settings.BadgeView != null && settings.BadgeView.BadgeLabelView != null)
            {
                settings.BadgeView.BadgeLabelView.TextColor = (Color)newValue;
            }
        }

        /// <summary>
        /// Invoked when the <see cref="BackgroundProperty"/> is set for badge control.
        /// </summary>
        /// <param name="bindable">The bindable.</param>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        private static void OnBackgroundPropertyChanged(BindableObject bindable, object? oldValue, object newValue)
        {
            var settings = bindable as BadgeSettings;

            if (settings != null && settings.BadgeView != null && settings.BadgeView.BadgeLabelView != null)
            {
                settings.BadgeView.BadgeLabelView.BadgeBackground = (Brush)newValue;
            }
        }

        /// <summary>
        /// Invoked when the <see cref="StrokeProperty"/> is set for badge control.
        /// </summary>
        /// <param name="bindable">The bindable.</param>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        private static void OnStrokePropertyChanged(BindableObject bindable, object? oldValue, object newValue)
        {
            var settings = bindable as BadgeSettings;

            if (settings != null && settings.BadgeView != null && settings.BadgeView.BadgeLabelView != null)
            {
                settings.BadgeView.BadgeLabelView.Stroke = (Color)newValue;
            }
        }

        /// <summary>
        /// Invoked when the <see cref="BorderWidthProperty"/> is set for badge control.
        /// </summary>
        /// <param name="bindable">The bindable.</param>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        private static void OnBorderWidthPropertyChanged(BindableObject bindable, object? oldValue, object newValue)
        {
            var settings = bindable as BadgeSettings;

            if (settings != null && settings.BadgeView != null && settings.BadgeView.BadgeLabelView != null)
            {
                settings.BadgeView.BadgeLabelView.BorderWidth = (double)newValue;
            }
        }

        /// <summary>
        /// Invoked when the <see cref="TextPaddingProperty"/> is set for badge control.
        /// </summary>
        /// <param name="bindable">The bindable.</param>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        private static void OnTextPaddingPropertyChanged(BindableObject bindable, object? oldValue, object newValue)
        {
            var settings = bindable as BadgeSettings;

            if (settings != null && settings.BadgeView != null && settings.BadgeView.BadgeLabelView != null)
            {
                settings.BadgeView.BadgeLabelView.TextPadding = (Thickness)newValue;
                settings.BadgeView.BadgeLabelView.CalculateBadgeBounds();
            }
        }

        /// <summary>
        /// Invoked when the <see cref="OffsetProperty"/> is set for badge control.
        /// </summary>
        /// <param name="bindable">The bindable.</param>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        private static void OnOffsetPropertyChanged(BindableObject bindable, object? oldValue, object newValue)
        {
            var settings = bindable as BadgeSettings;

            if (settings != null && settings.BadgeView != null && settings.BadgeView.BadgeLabelView != null)
            {
                var point = (Point)newValue;
                settings.UpdateOffset(settings.Position, settings.BadgeView, point);
            }
        }

        /// <summary>
        /// Invoked when the <see cref="CornerRadiusProperty"/> is set for badge control.
        /// </summary>
        /// <param name="bindable">The bindable.</param>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        private static void OnCornerRadiusPropertyChanged(BindableObject bindable, object? oldValue, object newValue)
        {
            var settings = bindable as BadgeSettings;

            if (settings != null && settings.BadgeView != null && settings.BadgeView.BadgeLabelView != null)
            {
                settings.BadgeView.BadgeLabelView.CornerRadius = (CornerRadius)newValue;
            }
        }

        /// <summary>
        /// Invoked when the <see cref="PositionProperty"/> is set for badge control.
        /// </summary>
        /// <param name="bindable">The bindable.</param>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        private static void OnBadgePositionPropertyChanged(BindableObject bindable, object? oldValue, object newValue)
        {
            var settings = bindable as BadgeSettings;

            if (settings != null && settings.BadgeView != null && settings.BadgeView.BadgeLabelView != null)
            {
                var position = (BadgePosition)newValue;

                if (position == BadgePosition.TopLeft)
                {
                    settings.BadgeView.BadgeLabelView.VerticalOptions = LayoutOptions.Start;
                    settings.BadgeView.BadgeLabelView.HorizontalOptions = LayoutOptions.Start;
                }
                else if (position == BadgePosition.TopRight)
                {
                    settings.BadgeView.BadgeLabelView.VerticalOptions = LayoutOptions.Start;
                    settings.BadgeView.BadgeLabelView.HorizontalOptions = LayoutOptions.End;
                }
                else if (position == BadgePosition.BottomLeft)
                {
                    settings.BadgeView.BadgeLabelView.VerticalOptions = LayoutOptions.End;
                    settings.BadgeView.BadgeLabelView.HorizontalOptions = LayoutOptions.Start;
                }
                else if (position == BadgePosition.BottomRight)
                {
                    settings.BadgeView.BadgeLabelView.VerticalOptions = LayoutOptions.End;
                    settings.BadgeView.BadgeLabelView.HorizontalOptions = LayoutOptions.End;
                }
            }
        }

        /// <summary>
        /// Invoked when the <see cref="TypeProperty"/> is set for badge control.
        /// </summary>
        /// <param name="bindable">The bindable.</param>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        private static void OnBadgeTypePropertyChanged(BindableObject bindable, object? oldValue, object newValue)
        {
            var settings = bindable as BadgeSettings;

            if (settings != null && settings.BadgeView != null && settings.BadgeView.BadgeLabelView != null && (BadgeType)newValue != BadgeType.None)
            {
                settings.BadgeView.BadgeLabelView.BadgeBackground = settings.GetBadgeBackground((BadgeType)newValue);
            }
        }

        /// <summary>
        /// Invoked when the <see cref="AnimationProperty"/> property changed.
        /// </summary>
        /// <param name="bindable">The bindable.</param>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        private static void OnBadgeAnimationPropertyChanged(BindableObject bindable, object? oldValue, object newValue)
        {
            var settings = bindable as BadgeSettings;

            if (settings != null && settings.BadgeView != null && settings.BadgeView.BadgeLabelView != null)
            {
                if ((BadgeAnimation)newValue == BadgeAnimation.Scale)
                {
                    settings.BadgeView.BadgeLabelView.AnimationEnabled = true;
                }
                else
                {
                    settings.BadgeView.BadgeLabelView.AnimationEnabled = false;
                }
            }
        }

        /// <summary>
        /// Invoked when the <see cref="IconProperty"/> property changed.
        /// </summary>
        /// <param name="bindable">The bindable.</param>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        private static void OnBadgeIconPropertyChanged(BindableObject bindable, object? oldValue, object newValue)
        {
            var settings = bindable as BadgeSettings;

            if (settings != null && settings.BadgeView != null && settings.BadgeView.BadgeLabelView != null && (BadgeIcon)newValue != BadgeIcon.None)
            {
                settings.BadgeView.BadgeLabelView.BadgeIcon = (BadgeIcon)newValue;
                settings.BadgeView.BadgeLabelView.Text = settings.GetFontIconText((BadgeIcon)newValue);
                settings.BadgeView.BadgeLabelView.FontFamily = "BadgeIcons";
            }
        }

        /// <summary>
        /// Invoked when the <see cref="AutoHideProperty"/> property changed.
        /// </summary>
        /// <param name="bindable">The bindable.</param>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        private static void OnAutoHidePropertyChanged(BindableObject bindable, object? oldValue, object newValue)
        {
            var settings = bindable as BadgeSettings;

            if (settings != null && settings.BadgeView != null && settings.BadgeView.BadgeLabelView != null)
            {
                settings.BadgeView.BadgeLabelView.AutoHide = (bool)newValue;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// 
        /// </summary>
        internal void ApplySettingstoUpdatedBadgeView()
        {
            if (this.BadgeView != null)
            {
                OnBackgroundPropertyChanged(this, null, this.Background);
                OnBadgeAnimationPropertyChanged(this, null, this.Animation);
                OnBadgeIconPropertyChanged(this, null, this.Icon);
                OnBadgePositionPropertyChanged(this, null, this.Position);
                OnBadgeTypePropertyChanged(this, null, this.Type);
                OnCornerRadiusPropertyChanged(this, null, this.CornerRadius);
                UpdateFontAttributes(this.FontAttributes);
                UpdateFontFamily(this.FontFamily);
                UpdateFontSize(this.FontSize);
                OnOffsetPropertyChanged(this, null, this.Offset);
                OnStrokePropertyChanged(this, null, this.Stroke);
                OnBorderWidthPropertyChanged(this, null, this.BorderWidth);
                OnTextColorPropertyChanged(this, null, this.TextColor);
                OnTextPaddingPropertyChanged(this, null, this.TextPadding);
                OnAutoHidePropertyChanged(this, null, this.AutoHide);

                if (this.badgeView != null && this.badgeView.BadgeLabelView != null)
                {
                    this.badgeView.BadgeLabelView.InvalidateDrawable();
                }
            }
        }

        /// <summary>
        /// Get default value of text padding.
        /// </summary>
        /// <returns>returns default value for badge text padding.</returns>
        private static Thickness GetTextPadding()
        {
            switch (Device.RuntimePlatform)
            {
                case Device.UWP:
                    return new Thickness(3);
                default:
                    return new Thickness(5);
            }
        }

        /// <summary>
        /// To get the color for badge view based on badge type
        /// </summary>
        /// <returns>badge color</returns>
        internal Brush GetBadgeBackground(BadgeType value)
        {
            Brush badgeColor = new SolidColorBrush(Colors.Transparent);

            switch (value)
            {
                case BadgeType.Error:
                    {
                        badgeColor = new SolidColorBrush(Color.FromRgba(220, 53, 69, 255));
                    }

                    break;

                case BadgeType.Dark:
                    {
                        badgeColor = new SolidColorBrush(Color.FromRgba(52, 58, 64, 255));
                    }

                    break;

                case BadgeType.Info:
                    {
                        badgeColor = new SolidColorBrush(Color.FromRgba(23, 162, 184, 255));
                    }

                    break;

                case BadgeType.Light:
                    {
                        badgeColor = new SolidColorBrush(Color.FromRgba(248, 249, 250, 255));
                    }

                    break;

                case BadgeType.Primary:
                    {
                        badgeColor = new SolidColorBrush(Color.FromRgba(0, 123, 255, 255));
                    }

                    break;

                case BadgeType.Secondary:
                    {
                        badgeColor = new SolidColorBrush(Color.FromRgba(108, 117, 125, 255));
                    }

                    break;

                case BadgeType.Success:
                    {
                        badgeColor = new SolidColorBrush(Color.FromRgba(40, 167, 69, 255));
                    }

                    break;

                case BadgeType.Warning:
                    {
                        badgeColor = new SolidColorBrush(Color.FromRgba(255, 193, 7, 255));
                    }

                    break;
                case BadgeType.None:
                    {
                        badgeColor = this.Background;
                    }
                    break;
            }

            return badgeColor;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        internal string GetFontIconText(BadgeIcon value)
        {
            return string.Empty;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="position"></param>
        /// <param name="badgeView"></param>
        /// <param name="offset"></param>
        internal void UpdateOffset(BadgePosition position, SfBadgeView badgeView, Point offset)
        {
            if (badgeView != null && badgeView.BadgeLabelView != null)
            {
                if (position == BadgePosition.BottomLeft)
                {
                    badgeView.BadgeLabelView.Margin = new Thickness(offset.X, 0, 0, offset.Y);
                }
                else if (position == BadgePosition.BottomRight)
                {
                    badgeView.BadgeLabelView.Margin = new Thickness(0, 0, offset.X, offset.Y);
                }
                else if (position == BadgePosition.TopLeft)
                {
                    badgeView.BadgeLabelView.Margin = new Thickness(offset.X, offset.Y, 0, 0);
                }
                else if (position == BadgePosition.TopRight)
                {
                    badgeView.BadgeLabelView.Margin = new Thickness(0, offset.Y, offset.X, 0);
                }
            }
        }

        #endregion
    }
}
