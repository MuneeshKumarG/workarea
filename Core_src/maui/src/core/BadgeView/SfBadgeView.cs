// <copyright file="SfBadgeView.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

[assembly: Microsoft.Maui.Controls.Internals.Preserve(AllMembers = true)]

namespace Syncfusion.Maui.Core
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using Microsoft.Maui;
    using Microsoft.Maui.Controls;
    using Microsoft.Maui.Graphics;

    /// <summary>
    /// The .NET MAUI Badge View control allows you to show a notification badge with a text over any controls. Adding a control in a Badge View control, you can show a notification badge value over the control.
    /// </summary>
    /// <example>
    /// The following examples show how to initialize the badge view.
    /// # [XAML](#tab/tabid-1).
    /// <code><![CDATA[
    /// <badge:SfBadgeView HorizontalOptions="Center"
    ///                    VerticalOptions="Center"
    ///                    BadgeText="20">
    ///
    ///     <badge:SfBadgeView.Content>
    ///         <Button Text="Primary"
    ///                 WidthRequest="120"
    ///                 HeightRequest="60"/>
    ///     </badge:SfBadgeView.Content>
    ///
    /// </badge:SfBadgeView>
    /// ]]></code>
    /// # [C#](#tab/tabid-2).
    /// <code><![CDATA[
    /// SfBadgeView sfBadgeView = new SfBadgeView();
    /// sfBadgeView.HorizontalOptions = LayoutOptions.Center;
    /// sfBadgeView.VerticalOptions = LayoutOptions.Center;
    /// sfBadgeView.BadgeText = "20";
    ///
    /// Button button = new Button();
    /// button.Text = "Primary";
    /// button.WidthRequest = 120;
    /// button.HeightRequest = 60;
    ///
    /// sfBadgeView.Content = button;
    /// this.Content = sfBadgeView;
    /// ]]></code>
    /// ***.
    /// </example>
    [DesignTimeVisible(true)]
    [ContentProperty(nameof(Content))]
    public class SfBadgeView : ContentView
    {
        #region Bindable properties

        /// <summary>
        /// Identifies the <see cref="Content"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="Content"/> bindable property.
        /// </value>
        public static new readonly BindableProperty ContentProperty =
            BindableProperty.Create(nameof(Content), typeof(View), typeof(SfBadgeView), null, BindingMode.OneWay, null, OnContentPropertyChanged);

        /// <summary>
        /// Identifies the <see cref="BadgeText"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="BadgeText"/> bindable property.
        /// </value>
        public static readonly BindableProperty BadgeTextProperty =
            BindableProperty.Create(nameof(BadgeText), typeof(string), typeof(SfBadgeView), string.Empty, BindingMode.OneWay, null, OnBadgeTextPropertyChanged);

        /// <summary>
        /// Identifies the <see cref="BadgeSettings"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="BadgeSettings"/> bindable property.
        /// </value>
        public static readonly BindableProperty BadgeSettingsProperty =
            BindableProperty.Create(nameof(BadgeSettings), typeof(BadgeSettings), typeof(SfBadgeView), null, BindingMode.OneWay, null, OnBadgeSettingsPropertyChanged, null);

        #endregion

        #region Fields

        private BadgeLabelView? badgeLabelView;

        // TODO: Need to replace with SfLayout once implemented.
        private Grid mainGrid;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="SfBadgeView"/> class.
        /// </summary>
        public SfBadgeView()
        {
            this.mainGrid = new Grid();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the View. Over this control, Badge View will be shown.
        /// </summary>
        /// <value>
        /// Specifies the content of the badge view. The default value is null.
        /// </value>
        /// <example>
        /// The following example shows how to apply the content for the badge view.
        /// <code><![CDATA[
        /// <badge:SfBadgeView>
        ///     <badge:SfBadgeView.Content>
        ///         <Button Text="Primary"
        ///                 WidthRequest="120"
        ///                 HeightRequest="60"/>
        ///     </badge:SfBadgeView.Content>
        /// </badge:SfBadgeView>
        /// ]]></code>
        /// </example>
        public new View Content
        {
            get { return (View)this.GetValue(ContentProperty); }
            set { this.SetValue(ContentProperty, value); }
        }

        /// <summary>
        /// Gets or sets the text for the badge. The property will be set only if BadgeIcon is set to BadgeIcon.None.
        /// </summary>
        /// <value>
        /// Specifies the badge text of the badge. The default value is an empty string.
        /// </value>
        /// <example>
        /// The following example shows how to apply the badge text for a badge.
        /// <code><![CDATA[
        /// <badge:SfBadgeView BadgeText="20">
        ///     <badge:SfBadgeView.Content>
        ///         <Button Text="Primary"
        ///                 WidthRequest="120"
        ///                 HeightRequest="60"/>
        ///     </badge:SfBadgeView.Content>
        /// </badge:SfBadgeView>
        /// ]]></code>
        /// </example>
        public string BadgeText
        {
            get { return (string)this.GetValue(BadgeTextProperty); }
            set { this.SetValue(BadgeTextProperty, value); }
        }

        /// <summary>
        /// Gets or sets the value for badge settings.
        /// </summary>
        /// <value>
        /// Specifies the badge settings of the badge. The default value is null.
        /// </value>
        /// <example>
        /// The following example shows how to apply the badge settings for a badge.
        /// <code><![CDATA[
        /// <badge:SfBadgeView BadgeText="20">
        ///     <badge:SfBadgeView.Content>
        ///         <Button Text="Primary"
        ///                 WidthRequest="120"
        ///                 HeightRequest="60"/>
        ///     </badge:SfBadgeView.Content>
        ///     <badge:SfBadgeView.BadgeSettings>
        ///         <badge:BadgeSettings Stroke="Orange"
        ///                              BorderWidth="2"/>
        ///     </badge:SfBadgeView.BadgeSettings>
        /// </badge:SfBadgeView>
        /// ]]></code>
        /// </example>
        public BadgeSettings? BadgeSettings
        {
            get { return (BadgeSettings)this.GetValue(BadgeSettingsProperty); }
            set { this.SetValue(BadgeSettingsProperty, value); }
        }

        /// <summary>
        /// Gets or sets the value that defines the badge label view.
        /// </summary>
        internal BadgeLabelView? BadgeLabelView
        {
            get { return this.badgeLabelView; }
            set { this.badgeLabelView = value; }
        }

        #endregion

        #region Internal Methods

        /// <summary>
        /// Method used to initialize the badge layer.
        /// </summary>
        internal void InitializeBadgeLayer()
        {
            if (this.BadgeLabelView == null)
            {
                this.BadgeLabelView = new BadgeLabelView();
            }

            this.BadgeLabelView.Text = this.BadgeText;
            this.mainGrid.Children.Add(this.BadgeLabelView);
            base.Content = this.mainGrid;
            if (this.BadgeSettings == null)
            {
                this.BadgeSettings = new BadgeSettings();
            }
            else
            {
                this.BadgeSettings.ApplySettingstoUpdatedBadgeView();
            }

            this.BadgeLabelView.TextElement = this.BadgeSettings;
        }

        #endregion

        #region Override methods

        /// <summary>
        /// Called when the binding context is changed.
        /// </summary>
        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
            if (this.BadgeSettings != null)
            {
                SfBadgeView.SetInheritedBindingContext(this.BadgeSettings, this.BindingContext);
            }
        }

        #endregion

        #region Property changed

        /// <summary>
        /// Invoked whenever the <see cref="ContentProperty"/> is set for badge view.
        /// </summary>
        /// <param name="bindable">The bindable.</param>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        private static void OnContentPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var badgeView = bindable as SfBadgeView;

            if (badgeView != null)
            {
                badgeView.mainGrid.Children.Clear();
                if (newValue != null)
                {
                    badgeView.mainGrid.Children.Add((View)newValue);
                    badgeView.InitializeBadgeLayer();
                }
            }
        }

        /// <summary>
        /// Invoked whenever the <see cref="BadgeTextProperty"/> is set for badge view.
        /// </summary>
        /// <param name="bindable">The bindable.</param>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        private static void OnBadgeTextPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var badgeView = bindable as SfBadgeView;

            if (badgeView != null && badgeView.BadgeLabelView != null)
            {
                badgeView.BadgeLabelView.Text = (string)newValue;
                badgeView.BadgeLabelView.CalculateBadgeBounds();
            }
        }

        /// <summary>
        /// Invoked whenever the <see cref="BadgeSettingsProperty"/> is set for badge view.
        /// </summary>
        /// <param name="bindable">The bindable.</param>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        private static void OnBadgeSettingsPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var badgeView = bindable as SfBadgeView;

            if (oldValue != null)
            {
                var previousSetting = oldValue as BadgeSettings;

                if (previousSetting != null)
                {
                    previousSetting.BindingContext = null;
                    previousSetting.Parent = null;
                }
            }

            if (newValue != null)
            {
                var currentSetting = newValue as BadgeSettings;

                if (currentSetting != null && badgeView != null)
                {
                    currentSetting.Parent = badgeView;
                    if (badgeView.BadgeLabelView != null)
                    {
                        badgeView.BadgeLabelView.TextElement = currentSetting;
                    }

                    SetInheritedBindingContext(badgeView.BadgeSettings, badgeView.BindingContext);
                }
            }
        }

        #endregion
    }
}
