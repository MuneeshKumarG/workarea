using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using System;
using System.Collections.Generic;
using System.ComponentModel;

[assembly: Microsoft.Maui.Controls.Internals.Preserve(AllMembers = true)]
namespace Syncfusion.Maui.Core
{
    /// <summary>
    /// The .NET MAUI Badge View control allows you to show a notification badge with a text over any controls. Adding a control in a Badge View control, you can show a notification badge value over the control. 
    /// </summary>
    [DesignTimeVisible(true)]
    [ContentProperty(nameof(Content))]
    public class SfBadgeView : ContentView
    {
        #region Fields

        internal BadgeLabelView? BadgeLabelView;

        //TODO: Need to replace with SfLayout once implemented.
        private Grid mainGrid;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the View. Over this control, Badge View will be shown. This is a bindable property.
        /// </summary>
        public new View Content
        {
            get { return (View)this.GetValue(ContentProperty); }
            set { this.SetValue(ContentProperty, value); }
        }

        /// <summary>
        /// Bindable property for <see cref="Content"/> property.
        /// </summary>
        public new static readonly BindableProperty ContentProperty =
            BindableProperty.Create(nameof(Content), typeof(View), typeof(SfBadgeView), null, BindingMode.OneWay, null, OnContentPropertyChanged);

        /// <summary>
        /// Gets or sets the text for the badge. The property will be set only if BadgeIcon is set to BadgeIcon.None. This is a bindable property.
        /// </summary>
        public string BadgeText
        {
            get { return (string)this.GetValue(BadgeTextProperty); }
            set { this.SetValue(BadgeTextProperty, value); }
        }

        /// <summary>
        /// Bindable property for <see cref="BadgeText"/> property.
        /// </summary>
        public static readonly BindableProperty BadgeTextProperty =
            BindableProperty.Create(nameof(BadgeText), typeof(string), typeof(SfBadgeView), string.Empty, BindingMode.OneWay, null, OnBadgeTextPropertyChanged);

        /// <summary>
        /// Gets or sets the value for badge settings.
        /// </summary>
        public BadgeSettings? BadgeSettings
        {
            get { return (BadgeSettings)this.GetValue(BadgeSettingsProperty); }
            set { this.SetValue(BadgeSettingsProperty, value); }
        }

        /// <summary>
        /// Bindable property of <see cref="BadgeSettings"/> property.
        /// </summary>
        public static readonly BindableProperty BadgeSettingsProperty =
            BindableProperty.Create(nameof(BadgeSettings), typeof(BadgeSettings), typeof(SfBadgeView), null, BindingMode.OneWay, null, OnBadgeSettingsPropertyChanged, null);

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

        #region Property Updates

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
                    SfBadgeView.SetInheritedBindingContext(badgeView.BadgeSettings, badgeView.BindingContext);
                }
            }
        }

        #endregion

        #region Internal Methods

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
                this.BadgeSettings = new BadgeSettings();
            else
                this.BadgeSettings.ApplySettingstoUpdatedBadgeView();
            this.BadgeLabelView.TextElement = this.BadgeSettings;
        }

        #endregion

    }
}
