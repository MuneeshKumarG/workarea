// <copyright file="BadgeSettings.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Syncfusion.Maui.Core
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Maui;
    using Microsoft.Maui.Controls;
    using Microsoft.Maui.Controls.Shapes;
    using Microsoft.Maui.Graphics;
    using Syncfusion.Maui.Graphics.Internals;
    using Font = Microsoft.Maui.Font;

    /// <summary>
    /// Represents the badge settings class.
    /// </summary>
    public class BadgeSettings : Element, ITextElement
    {
        #region Bindable properties

        /// <summary>
        /// Identifies the <see cref="FontSize"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="FontSize"/> bindable property.
        /// </value>
        public static readonly BindableProperty FontSizeProperty = FontElement.FontSizeProperty;

        /// <summary>
        /// Identifies the <see cref="FontFamily"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="FontFamily"/> bindable property.
        /// </value>
        public static readonly BindableProperty FontFamilyProperty = FontElement.FontFamilyProperty;

        /// <summary>
        /// Identifies the <see cref="FontAttributes"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="FontAttributes"/> bindable property.
        /// </value>
        public static readonly BindableProperty FontAttributesProperty = FontElement.FontAttributesProperty;

        /// <summary>
        /// Identifies the <see cref="TextColor"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="TextColor"/> bindable property.
        /// </value>
        public static readonly BindableProperty TextColorProperty =
            BindableProperty.Create(nameof(TextColor), typeof(Color), typeof(BadgeSettings), Colors.White, BindingMode.Default, null, OnTextColorPropertyChanged);

        /// <summary>
        /// Identifies the <see cref="Background"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="Background"/> bindable property.
        /// </value>
        public static readonly BindableProperty BackgroundProperty =
            BindableProperty.Create(nameof(Background), typeof(Brush), typeof(BadgeSettings), new SolidColorBrush(Colors.Transparent), BindingMode.Default, null, OnBackgroundPropertyChanged);

        /// <summary>
        /// Identifies the <see cref="BorderWidth"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="BorderWidth"/> bindable property.
        /// </value>
        public static readonly BindableProperty BorderWidthProperty =
            BindableProperty.Create(nameof(BorderWidth), typeof(double), typeof(BadgeSettings), 0d, BindingMode.Default, null, OnBorderWidthPropertyChanged);

        /// <summary>
        /// Identifies the <see cref="CornerRadius"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="CornerRadius"/> bindable property.
        /// </value>
        public static readonly BindableProperty CornerRadiusProperty =
            BindableProperty.Create(nameof(CornerRadius), typeof(CornerRadius), typeof(BadgeSettings), new CornerRadius(25), BindingMode.Default, null, null, OnCornerRadiusPropertyChanged);

        /// <summary>
        /// Identifies the <see cref="Stroke"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="Stroke"/> bindable property.
        /// </value>
        public static readonly BindableProperty StrokeProperty =
            BindableProperty.Create(nameof(Stroke), typeof(Color), typeof(BadgeSettings), Colors.Transparent, BindingMode.Default, null, OnStrokePropertyChanged);

        /// <summary>
        /// Identifies the <see cref="Position"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="Position"/> bindable property.
        /// </value>
        public static readonly BindableProperty PositionProperty =
            BindableProperty.Create(nameof(Position), typeof(BadgePosition), typeof(BadgeSettings), BadgePosition.TopRight, BindingMode.Default, null, null, OnBadgePositionPropertyChanged);

        /// <summary>
        /// Identifies the <see cref="Offset"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="Offset"/> bindable property.
        /// </value>
        public static readonly BindableProperty OffsetProperty =
            BindableProperty.Create(nameof(Offset), typeof(Point), typeof(BadgeSettings), new Point(2, 2), BindingMode.Default, null, null, OnOffsetPropertyChanged);

        /// <summary>
        /// Identifies the <see cref="Animation"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="Animation"/> bindable property.
        /// </value>
        public static readonly BindableProperty AnimationProperty =
            BindableProperty.Create(nameof(Animation), typeof(BadgeAnimation), typeof(BadgeSettings), BadgeAnimation.Scale, BindingMode.Default, null, null, OnBadgeAnimationPropertyChanged);

        /// <summary>
        /// Identifies the <see cref="Type"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="Type"/> bindable property.
        /// </value>
        public static readonly BindableProperty TypeProperty =
            BindableProperty.Create(nameof(Type), typeof(BadgeType), typeof(BadgeSettings), BadgeType.Primary, BindingMode.Default, null, null, OnBadgeTypePropertyChanged);

        /// <summary>
        /// Identifies the <see cref="TextPadding"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="TextPadding"/> bindable property.
        /// </value>
        public static readonly BindableProperty TextPaddingProperty =
            BindableProperty.Create(nameof(TextPadding), typeof(Thickness), typeof(BadgeSettings), GetTextPadding(), BindingMode.Default, null, null, OnTextPaddingPropertyChanged);

        /// <summary>
        /// Identifies the <see cref="AutoHide"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="AutoHide"/> bindable property.
        /// </value>
        public static readonly BindableProperty AutoHideProperty =
            BindableProperty.Create(nameof(AutoHide), typeof(bool), typeof(BadgeSettings), false, BindingMode.Default, null, null, OnAutoHidePropertyChanged);

        /// <summary>
        /// Identifies the <see cref="Icon"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="Icon"/> bindable property.
        /// </value>
        // TODO: Need to fix the font icon issue in library or need to draw the icons.
        internal static readonly BindableProperty IconProperty =
            BindableProperty.Create(nameof(Icon), typeof(BadgeIcon), typeof(BadgeSettings), BadgeIcon.None, BindingMode.Default, null, null, OnBadgeIconPropertyChanged);

        #endregion

        #region Fields

        /// <summary>
        /// Specifies the badge view.
        /// </summary>
        private SfBadgeView? badgeView;

        #endregion

        #region Constructor

        /// <summary>
        ///  Initializes a new instance of the <see cref="BadgeSettings" /> class.
        /// </summary>
        public BadgeSettings()
        {
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the background for the badge. The property will be set only if BadgeType is set to BadgeType.None.
        /// </summary>
        /// <value>
        /// Specifies the background color of the badge. The default value is Colors.Transparent.
        /// </value>
        /// <example>
        /// The following example shows how to apply the background color for a badge.
        /// <code><![CDATA[
        /// <badge:SfBadgeView BadgeText="20">
        ///     <badge:SfBadgeView.Content>
        ///         <Button Text="Primary"
        ///                 WidthRequest="120"
        ///                 HeightRequest="60"/>
        ///     </badge:SfBadgeView.Content>
        ///     <badge:SfBadgeView.BadgeSettings>
        ///         <badge:BadgeSettings Background="Green"/>
        ///     </badge:SfBadgeView.BadgeSettings>
        /// </badge:SfBadgeView>
        /// ]]></code>
        /// </example>
        public Brush Background
        {
            get { return (Brush)this.GetValue(BackgroundProperty); }
            set { this.SetValue(BackgroundProperty, value); }
        }

        /// <summary>
        /// Gets or sets the border width for the badge. The border width will be visible only if the border color is not set to transparent.
        /// </summary>
        /// <value>
        /// Specifies the border width of the badge. The default value is 0d.
        /// </value>
        /// <example>
        /// The following example shows how to apply the border width for a badge.
        /// <code><![CDATA[
        /// <badge:SfBadgeView BadgeText="20">
        ///     <badge:SfBadgeView.Content>
        ///         <Button Text="Primary"
        ///                 WidthRequest="120"
        ///                 HeightRequest="60"/>
        ///     </badge:SfBadgeView.Content>
        ///     <badge:SfBadgeView.BadgeSettings>
        ///         <badge:BadgeSettings BorderWidth="2"/>
        ///     </badge:SfBadgeView.BadgeSettings>
        /// </badge:SfBadgeView>
        /// ]]></code>
        /// </example>
        public double BorderWidth
        {
            get { return (double)this.GetValue(BorderWidthProperty); }
            set { this.SetValue(BorderWidthProperty, value); }
        }

        /// <summary>
        /// Gets or sets the corner radius of the badge.
        /// </summary>
        /// <value>
        /// Specifies the corner radius of the badge. The default value is CornerRadius(25).
        /// </value>
        /// <example>
        /// The following example shows how to apply the corner radius for a badge.
        /// <code><![CDATA[
        /// <badge:SfBadgeView BadgeText="20">
        ///     <badge:SfBadgeView.Content>
        ///         <Button Text="Primary"
        ///                 WidthRequest="120"
        ///                 HeightRequest="60"/>
        ///     </badge:SfBadgeView.Content>
        ///     <badge:SfBadgeView.BadgeSettings>
        ///         <badge:BadgeSettings CornerRadius="5,5,5,5"/>
        ///     </badge:SfBadgeView.BadgeSettings>
        /// </badge:SfBadgeView>
        /// ]]></code>
        /// </example>
        public CornerRadius CornerRadius
        {
            get { return (CornerRadius)this.GetValue(CornerRadiusProperty); }
            set { this.SetValue(CornerRadiusProperty, value); }
        }

        /// <summary>
        /// Gets or sets the border color for the badge.
        /// </summary>
        /// <value>
        /// Specifies the border color of the badge. The default value is Colors.Transparent.
        /// </value>
        /// <example>
        /// The following example shows how to apply the border color for a badge.
        /// <code><![CDATA[
        /// <badge:SfBadgeView BadgeText="20">
        ///     <badge:SfBadgeView.Content>
        ///         <Button Text="Primary"
        ///                 WidthRequest="120"
        ///                 HeightRequest="60"/>
        ///     </badge:SfBadgeView.Content>
        ///     <badge:SfBadgeView.BadgeSettings>
        ///         <badge:BadgeSettings Stroke="Orange"/>
        ///     </badge:SfBadgeView.BadgeSettings>
        /// </badge:SfBadgeView>
        /// ]]></code>
        /// </example>
        public Color Stroke
        {
            get { return (Color)this.GetValue(StrokeProperty); }
            set { this.SetValue(StrokeProperty, value); }
        }

        /// <summary>
        /// Gets or sets the position where the badge will be shown relative to the Content.
        /// </summary>
        /// <value>
        /// One of the <see cref="Syncfusion.Maui.Core.BadgePosition"/> enumeration that specifies the position of the badge relative to the content. The default value is <see cref="Syncfusion.Maui.Core.BadgePosition.TopRight"/>.
        /// </value>
        /// <example>
        /// The following example shows how to apply the position for a badge.
        /// <code><![CDATA[
        /// <badge:SfBadgeView BadgeText="20">
        ///     <badge:SfBadgeView.Content>
        ///         <Button Text="Primary"
        ///                 WidthRequest="120"
        ///                 HeightRequest="60"/>
        ///     </badge:SfBadgeView.Content>
        ///     <badge:SfBadgeView.BadgeSettings>
        ///         <badge:BadgeSettings Position="BottomRight"/>
        ///     </badge:SfBadgeView.BadgeSettings>
        /// </badge:SfBadgeView>
        /// ]]></code>
        /// </example>
        public BadgePosition Position
        {
            get { return (BadgePosition)this.GetValue(PositionProperty); }
            set { this.SetValue(PositionProperty, value); }
        }

        /// <summary>
        /// Gets or sets the value for offset.
        /// </summary>
        /// <value>
        /// Specifies the offset of the badge. The default value is Point(2, 2).
        /// </value>
        /// <example>
        /// The following example shows how to apply the offset for a badge.
        /// <code><![CDATA[
        /// <badge:SfBadgeView BadgeText="20">
        ///     <badge:SfBadgeView.Content>
        ///         <Button Text="Primary"
        ///                 WidthRequest="120"
        ///                 HeightRequest="60"/>
        ///     </badge:SfBadgeView.Content>
        ///     <badge:SfBadgeView.BadgeSettings>
        ///         <badge:BadgeSettings Offset="10,10"/>
        ///     </badge:SfBadgeView.BadgeSettings>
        /// </badge:SfBadgeView>
        /// ]]></code>
        /// </example>
        public Point Offset
        {
            get { return (Point)this.GetValue(OffsetProperty); }
            set { this.SetValue(OffsetProperty, value); }
        }

        /// <summary>
        /// Gets or sets the animation that should be used when the badge is shown. The animation only occurs if IsAnimated is set to true.
        /// </summary>
        /// <value>
        /// One of the <see cref="Syncfusion.Maui.Core.BadgeAnimation"/> enumeration that specifies the animation of the badge when the badge is shown. The default value is <see cref="Syncfusion.Maui.Core.BadgeAnimation.Scale"/>.
        /// </value>
        /// <example>
        /// The following example shows how to apply the animation for a badge.
        /// <code><![CDATA[
        /// <badge:SfBadgeView BadgeText="20">
        ///     <badge:SfBadgeView.Content>
        ///         <Button Text="Primary"
        ///                 WidthRequest="120"
        ///                 HeightRequest="60"/>
        ///     </badge:SfBadgeView.Content>
        ///     <badge:SfBadgeView.BadgeSettings>
        ///         <badge:BadgeSettings Animation="None"/>
        ///     </badge:SfBadgeView.BadgeSettings>
        /// </badge:SfBadgeView>
        /// ]]></code>
        /// </example>
        public BadgeAnimation Animation
        {
            get { return (BadgeAnimation)this.GetValue(AnimationProperty); }
            set { this.SetValue(AnimationProperty, value); }
        }

        /// <summary>
        /// Gets or sets the type of the badge. BadgeSetting.BackgroundColor is not applied when this property set other than BadgeIcon.None.
        /// </summary>
        /// <value>
        /// One of the <see cref="Syncfusion.Maui.Core.BadgeType"/> enumeration that specifies the type of the badge. The default value is <see cref="Syncfusion.Maui.Core.BadgeType.Primary"/>.
        /// </value>
        /// <example>
        /// The following example shows how to apply the type for a badge.
        /// <code><![CDATA[
        /// <badge:SfBadgeView BadgeText="20">
        ///     <badge:SfBadgeView.Content>
        ///         <Button Text="Primary"
        ///                 WidthRequest="120"
        ///                 HeightRequest="60"/>
        ///     </badge:SfBadgeView.Content>
        ///     <badge:SfBadgeView.BadgeSettings>
        ///         <badge:BadgeSettings Type="Success"/>
        ///     </badge:SfBadgeView.BadgeSettings>
        /// </badge:SfBadgeView>
        /// ]]></code>
        /// </example>
        public BadgeType Type
        {
            get { return (BadgeType)this.GetValue(TypeProperty); }
            set { this.SetValue(TypeProperty, value); }
        }

        /// <summary>
        /// Gets or sets the padding around the text of the badge.
        /// </summary>
        /// <value>
        /// Specifies the text padding around the text of the badge. The default value is Thickness(3) for UWP and Thickness(5) for other platforms.
        /// </value>
        /// <example>
        /// The following example shows how to apply the text padding for a badge.
        /// <code><![CDATA[
        /// <badge:SfBadgeView BadgeText="20">
        ///     <badge:SfBadgeView.Content>
        ///         <Button Text="Primary"
        ///                 WidthRequest="120"
        ///                 HeightRequest="60"/>
        ///     </badge:SfBadgeView.Content>
        ///     <badge:SfBadgeView.BadgeSettings>
        ///         <badge:BadgeSettings TextPadding="10"/>
        ///     </badge:SfBadgeView.BadgeSettings>
        /// </badge:SfBadgeView>
        /// ]]></code>
        /// </example>
        public Thickness TextPadding
        {
            get { return (Thickness)this.GetValue(TextPaddingProperty); }
            set { this.SetValue(TextPaddingProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the badge text is AutoHide or not.
        /// </summary>
        /// <value>
        /// Specifies the value for auto-hide of the badge when the value is zero or empty. The default value is false.
        /// </value>
        /// <example>
        /// The following example shows how to apply the auto hide for a badge.
        /// <code><![CDATA[
        /// <badge:SfBadgeView BadgeText="20">
        ///     <badge:SfBadgeView.Content>
        ///         <Button Text="Primary"
        ///                 WidthRequest="120"
        ///                 HeightRequest="60"/>
        ///     </badge:SfBadgeView.Content>
        ///     <badge:SfBadgeView.BadgeSettings>
        ///         <badge:BadgeSettings AutoHide="True"/>
        ///     </badge:SfBadgeView.BadgeSettings>
        /// </badge:SfBadgeView>
        /// ]]></code>
        /// </example>
        public bool AutoHide
        {
            get { return (bool)this.GetValue(AutoHideProperty); }
            set { this.SetValue(AutoHideProperty, value); }
        }

        /// <summary>
        /// Gets or sets the font size of the text to be displayed in badge over the control.
        /// </summary>
        /// <value>
        /// Specifies the font size of the badge text. The default value is -1d.
        /// </value>
        /// <example>
        /// The following example shows how to apply the font size for the badge text.
        /// <code><![CDATA[
        /// <badge:SfBadgeView BadgeText="20">
        ///     <badge:SfBadgeView.Content>
        ///         <Button Text="Primary"
        ///                 WidthRequest="120"
        ///                 HeightRequest="60"/>
        ///     </badge:SfBadgeView.Content>
        ///     <badge:SfBadgeView.BadgeSettings>
        ///         <badge:BadgeSettings FontSize="32"/>
        ///     </badge:SfBadgeView.BadgeSettings>
        /// </badge:SfBadgeView>
        /// ]]></code>
        /// </example>
        [System.ComponentModel.TypeConverter(typeof(FontSizeConverter))]
        public double FontSize
        {
            get { return (double)this.GetValue(FontSizeProperty); }
            set { this.SetValue(FontSizeProperty, value); }
        }

        /// <summary>
        /// Gets or sets the font attributes of the text to be displayed in badge over the control.
        /// </summary>
        /// <value>
        /// One of the <see cref="Microsoft.Maui.Controls.FontAttributes"/> enumeration that specifies the font attributes of badge text. The default mode is <see cref="Microsoft.Maui.Controls.FontAttributes.None"/>.
        /// </value>
        /// <example>
        /// The following example shows how to apply the font attributes for the badge text.
        /// <code><![CDATA[
        /// <badge:SfBadgeView BadgeText="20">
        ///     <badge:SfBadgeView.Content>
        ///         <Button Text="Primary"
        ///                 WidthRequest="120"
        ///                 HeightRequest="60"/>
        ///     </badge:SfBadgeView.Content>
        ///     <badge:SfBadgeView.BadgeSettings>
        ///         <badge:BadgeSettings FontAttributes="Bold"/>
        ///     </badge:SfBadgeView.BadgeSettings>
        /// </badge:SfBadgeView>
        /// ]]></code>
        /// </example>
        public FontAttributes FontAttributes
        {
            get { return (FontAttributes)this.GetValue(FontAttributesProperty); }
            set { this.SetValue(FontAttributesProperty, value); }
        }

        /// <summary>
        /// Gets or sets the font family of the text to be displayed in badge over the control. The property will be set only if BadgeIcon is set to BadgeIcon.None.
        /// </summary>
        /// <value>
        /// Specifies the font family of the badge text. The default value is an empty string.
        /// </value>
        /// <example>
        /// The following example shows how to apply the font family for the badge text.
        /// <code><![CDATA[
        /// <badge:SfBadgeView BadgeText="20">
        ///     <badge:SfBadgeView.Content>
        ///         <Button Text="Primary"
        ///                 WidthRequest="120"
        ///                 HeightRequest="60"/>
        ///     </badge:SfBadgeView.Content>
        ///     <badge:SfBadgeView.BadgeSettings>
        ///         <badge:BadgeSettings FontFamily="OpenSansRegular"/>
        ///     </badge:SfBadgeView.BadgeSettings>
        /// </badge:SfBadgeView>
        /// ]]></code>
        /// </example>
        public string FontFamily
        {
            get { return (string)this.GetValue(FontFamilyProperty); }
            set { this.SetValue(FontFamilyProperty, value); }
        }

        /// <summary>
        /// Gets or sets the color of the text to be displayed in badge over the control.
        /// </summary>
        /// <value>
        /// Specifies the text color of the badge text. The default value is Colors.White.
        /// </value>
        /// <example>
        /// The following example shows how to apply the text color for the badge text.
        /// <code><![CDATA[
        /// <badge:SfBadgeView BadgeText="20">
        ///     <badge:SfBadgeView.Content>
        ///         <Button Text="Primary"
        ///                 WidthRequest="120"
        ///                 HeightRequest="60"/>
        ///     </badge:SfBadgeView.Content>
        ///     <badge:SfBadgeView.BadgeSettings>
        ///         <badge:BadgeSettings TextColor="Red"/>
        ///     </badge:SfBadgeView.BadgeSettings>
        /// </badge:SfBadgeView>
        /// ]]></code>
        /// </example>
        public Color TextColor
        {
            get { return (Color)this.GetValue(TextColorProperty); }
            set { this.SetValue(TextColorProperty, value); }
        }

        /// <summary>
        /// Gets the font of the badge text.
        /// </summary>
        Font ITextElement.Font => (Font)this.GetValue(FontElement.FontProperty);

        /// <summary>
        /// Gets or sets the Icon to be displayed in the badge. BadgeText and BadgeSetting.FontFamily is not applied when this property is set other than BadgeIcon.None.
        /// </summary>
        /// <value>
        /// One of the <see cref="Syncfusion.Maui.Core.BadgeIcon"/> enumeration that specifies the icon of the badge. The default value is <see cref="Syncfusion.Maui.Core.BadgeIcon.None"/>.
        /// </value>
        /// <example>
        /// The following example shows how to apply the icon for a badge.
        /// <code><![CDATA[
        /// <badge:SfBadgeView BadgeText="20">
        ///     <badge:SfBadgeView.Content>
        ///         <Button Text="Primary"
        ///                 WidthRequest="120"
        ///                 HeightRequest="60"/>
        ///     </badge:SfBadgeView.Content>
        ///     <badge:SfBadgeView.BadgeSettings>
        ///         <badge:BadgeSettings Position="BottomRight"/>
        ///     </badge:SfBadgeView.BadgeSettings>
        /// </badge:SfBadgeView>
        /// ]]></code>
        /// </example>
        // TODO: Need to fix the font icon issue in library or need to draw the icons.
        internal BadgeIcon Icon
        {
            get { return (BadgeIcon)this.GetValue(IconProperty); }
            set { this.SetValue(IconProperty, value); }
        }

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

        #endregion

        #region Public methods

        /// <summary>
        /// Invoked when the <see cref="FontSizeProperty"/> changed.
        /// </summary>
        /// <returns>It returns the font size of the badge text.</returns>
        double ITextElement.FontSizeDefaultValueCreator()
        {
            return 12d;
        }

        /// <summary>
        /// Invoked when the <see cref="FontAttributesProperty"/> changed.
        /// </summary>
        /// <param name="oldValue">oldValue.</param>
        /// <param name="newValue">newValue.</param>
        void ITextElement.OnFontAttributesChanged(FontAttributes oldValue, FontAttributes newValue)
        {
            this.UpdateFontAttributes(newValue);
        }

        /// <summary>
        /// Invoked when the <see cref="Font"/> property changed.
        /// </summary>
        /// <param name="oldValue">oldValue.</param>
        /// <param name="newValue">newValue.</param>
        void ITextElement.OnFontChanged(Font oldValue, Font newValue)
        {
        }

        /// <summary>
        /// Invoked when the <see cref="FontFamilyProperty"/> changed.
        /// </summary>
        /// <param name="oldValue">oldValue.</param>
        /// <param name="newValue">newValue.</param>
        void ITextElement.OnFontFamilyChanged(string oldValue, string newValue)
        {
            this.UpdateFontFamily(newValue);
        }

        /// <summary>
        /// Invoked when the <see cref="FontSizeProperty"/> changed.
        /// </summary>
        /// <param name="oldValue">oldValue.</param>
        /// <param name="newValue">newValue.</param>
        void ITextElement.OnFontSizeChanged(double oldValue, double newValue)
        {
            this.UpdateFontSize(newValue);
        }

        #endregion

        #region Internal methods

        /// <summary>
        /// To get the font icon text for badge icon.
        /// </summary>
        /// <returns>It returns the font icon text.</returns>
        internal static string GetFontIconText()
        {
            return string.Empty;
        }

        /// <summary>
        /// To update the badge offset.
        /// </summary>
        /// <param name="position">The badge position.</param>
        /// <param name="badgeView">The badge view.</param>
        /// <param name="offset">The offset.</param>
        internal static void UpdateOffset(BadgePosition position, SfBadgeView badgeView, Point offset)
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

        /// <summary>
        /// Method used to update the badge settings to the updated badge view.
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
                this.UpdateFontAttributes(this.FontAttributes);
                this.UpdateFontFamily(this.FontFamily);
                this.UpdateFontSize(this.FontSize);
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
        /// To get the color for badge view based on badge type.
        /// </summary>
        /// <param name="value">The badge type value.</param>
        /// <returns>It returns the background color of the badge.</returns>
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

        #endregion

        #region Override methods

        /// <summary>
        /// Invoked whenever the Parent of an element is set.
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

        #region Property changed

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
                UpdateOffset(settings.Position, settings.BadgeView, point);
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
                settings.BadgeView.BadgeLabelView.Text = GetFontIconText();
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

        #region Private methods

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

        #endregion
    }
}
