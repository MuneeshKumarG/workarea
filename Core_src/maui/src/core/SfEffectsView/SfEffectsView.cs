// <copyright file="SfEffectsView.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Syncfusion.Maui.Core
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows.Input;
    using Microsoft.Maui;
    using Microsoft.Maui.Controls;
    using Microsoft.Maui.Graphics;
    using Syncfusion.Maui.Core.Internals;
    using Syncfusion.Maui.Graphics.Internals;

    /// <summary>
    /// <see cref="SfEffectsView"/> is a container control that provides the out-of-the-box effects
    /// such as highlight, ripple, selection, scaling, and rotation.
    /// </summary>
    /// <example>
    /// The following examples show how to initialize the badge view.
    /// # [XAML](#tab/tabid-1).
    /// <code><![CDATA[
    /// <effectsView:SfEffectsView>
    ///
    ///     <effectsView:SfEffectsView.Content>
    ///         <Button Text="Content"
    ///                 WidthRequest="120"
    ///                 HeightRequest="60"/>
    ///     </effectsView:SfEffectsView.Content>
    ///
    /// </effectsView:SfEffectsView>
    /// ]]></code>
    /// # [C#](#tab/tabid-2).
    /// <code><![CDATA[
    /// SfEffectsView effectsView = new SfEffectsView();
    ///
    /// Button button = new Button();
    /// button.Text = "Content";
    /// button.WidthRequest = 120;
    /// button.HeightRequest = 60;
    ///
    /// effectsView.Content = button;
    /// this.Content = effectsView;
    /// ]]></code>
    /// ***.
    /// </example>
    [DesignTimeVisible(true)]
    [ContentProperty(nameof(Content))]
    public class SfEffectsView : ContentView, ITouchListener, ITapGestureListener, ILongPressGestureListener
    {
        #region Bindable properties

        /// <summary>
        /// Identifies the <see cref="RippleAnimationDuration"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="RippleAnimationDuration"/> bindable property.
        /// </value>
        public static readonly BindableProperty RippleAnimationDurationProperty = BindableProperty.Create(nameof(RippleAnimationDuration), typeof(double), typeof(SfEffectsView), 275d, BindingMode.Default);

        /// <summary>
        /// Identifies the <see cref="ScaleAnimationDuration"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="ScaleAnimationDuration"/> bindable property.
        /// </value>
        public static readonly BindableProperty ScaleAnimationDurationProperty = BindableProperty.Create(nameof(ScaleAnimationDuration), typeof(double), typeof(SfEffectsView), 150d, BindingMode.Default);

        /// <summary>
        /// Identifies the <see cref="RotationAnimationDuration"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="RotationAnimationDuration"/> bindable property.
        /// </value>
        public static readonly BindableProperty RotationAnimationDurationProperty = BindableProperty.Create(nameof(RotationAnimationDuration), typeof(double), typeof(SfEffectsView), 200d, BindingMode.Default);

        /// <summary>
        /// Identifies the <see cref="InitialRippleFactor"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="InitialRippleFactor"/> bindable property.
        /// </value>
        public static readonly BindableProperty InitialRippleFactorProperty = BindableProperty.Create(nameof(InitialRippleFactor), typeof(double), typeof(SfEffectsView), 0.25d, BindingMode.Default);

        /// <summary>
        /// Identifies the <see cref="ScaleFactor"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="ScaleFactor"/> bindable property.
        /// </value>
        public static readonly BindableProperty ScaleFactorProperty = BindableProperty.Create(nameof(ScaleFactor), typeof(double), typeof(SfEffectsView), 1d, BindingMode.Default);

        /// <summary>
        /// Identifies the <see cref="HighlightBackground"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="HighlightBackground"/> bindable property.
        /// </value>
        public static readonly BindableProperty HighlightBackgroundProperty = BindableProperty.Create(nameof(HighlightBackground), typeof(Brush), typeof(SfEffectsView), new SolidColorBrush(Colors.Black), BindingMode.Default);

        /// <summary>
        /// Identifies the <see cref="RippleBackground"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="RippleBackground"/> bindable property.
        /// </value>
        public static readonly BindableProperty RippleBackgroundProperty = BindableProperty.Create(nameof(RippleBackground), typeof(Brush), typeof(SfEffectsView), new SolidColorBrush(Colors.Black), BindingMode.Default);

        /// <summary>
        /// Identifies the <see cref="SelectionBackground"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="SelectionBackground"/> bindable property.
        /// </value>
        public static readonly BindableProperty SelectionBackgroundProperty = BindableProperty.Create(nameof(SelectionBackground), typeof(Brush), typeof(SfEffectsView), new SolidColorBrush(Colors.Black), BindingMode.Default, null, OnSelectionBackgroundPropertyChanged);

        /// <summary>
        /// Identifies the <see cref="Angle"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="Angle"/> bindable property.
        /// </value>
        public static readonly BindableProperty AngleProperty = BindableProperty.Create(nameof(Angle), typeof(int), typeof(SfEffectsView), 0, BindingMode.Default);

        /// <summary>
        /// Identifies the <see cref="FadeOutRipple"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="FadeOutRipple"/> bindable property.
        /// </value>
        public static readonly BindableProperty FadeOutRippleProperty = BindableProperty.Create(nameof(FadeOutRipple), typeof(bool), typeof(SfEffectsView), false, BindingMode.Default);

        /// <summary>
        /// Identifies the <see cref="AutoResetEffects"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="AutoResetEffects"/> bindable property.
        /// </value>
        public static readonly BindableProperty AutoResetEffectsProperty = BindableProperty.Create(nameof(AutoResetEffects), typeof(AutoResetEffects), typeof(SfEffectsView), AutoResetEffects.None, BindingMode.Default, null);

        /// <summary>
        /// Identifies the <see cref="TouchDownEffects"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="TouchDownEffects"/> bindable property.
        /// </value>
        public static readonly BindableProperty TouchDownEffectsProperty = BindableProperty.Create(nameof(TouchDownEffects), typeof(SfEffects), typeof(SfEffectsView), SfEffects.Ripple, BindingMode.Default);

        /// <summary>
        /// Identifies the <see cref="TouchUpEffects"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="TouchUpEffects"/> bindable property.
        /// </value>
        public static readonly BindableProperty TouchUpEffectsProperty = BindableProperty.Create(nameof(TouchUpEffects), typeof(SfEffects), typeof(SfEffectsView), SfEffects.None, BindingMode.Default);

        /// <summary>
        /// Identifies the <see cref="LongPressEffects"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="LongPressEffects"/> bindable property.
        /// </value>
        public static readonly BindableProperty LongPressEffectsProperty = BindableProperty.Create(nameof(LongPressEffects), typeof(SfEffects), typeof(SfEffectsView), SfEffects.None, BindingMode.Default);

        /// <summary>
        /// Identifies the <see cref="LongPressedCommand"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="LongPressedCommand"/> bindable property.
        /// </value>
        public static readonly BindableProperty LongPressedCommandProperty = BindableProperty.Create(nameof(LongPressedCommand), typeof(ICommand), typeof(SfEffectsView), null, BindingMode.Default);

        /// <summary>
        /// Identifies the <see cref="LongPressedCommandParameter"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="LongPressedCommandParameter"/> bindable property.
        /// </value>
        public static readonly BindableProperty LongPressedCommandParameterProperty = BindableProperty.Create(nameof(LongPressedCommandParameter), typeof(object), typeof(SfEffectsView), null, BindingMode.Default);

        /// <summary>
        /// Identifies the <see cref="IsSelected"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="IsSelected"/> bindable property.
        /// </value>
        public static readonly BindableProperty IsSelectedProperty = BindableProperty.Create(nameof(IsSelected), typeof(bool), typeof(SfEffectsView), false, BindingMode.TwoWay, null, OnIsSelectedPropertyChanged);

        /// <summary>
        /// Identifies the <see cref="Content"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="Content"/> bindable property.
        /// </value>
        public static new readonly BindableProperty ContentProperty = BindableProperty.Create(nameof(Content), typeof(View), typeof(SfEffectsView), null, BindingMode.Default, null, OnContentChanged);

        /// <summary>
        /// Identifies the <see cref="ShouldIgnoreTouches"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="ShouldIgnoreTouches"/> bindable property.
        /// </value>
        public static readonly BindableProperty ShouldIgnoreTouchesProperty = BindableProperty.Create(nameof(ShouldIgnoreTouches), typeof(bool), typeof(SfEffectsView), false, BindingMode.Default, null, OnShouldIgnorePropertyChanged);

        /// <summary>
        /// Identifies the <see cref="TouchDownCommand"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="TouchDownCommand"/> bindable property.
        /// </value>
        public static readonly BindableProperty TouchDownCommandProperty = BindableProperty.Create(nameof(TouchDownCommand), typeof(ICommand), typeof(SfEffectsView), null, BindingMode.Default);

        /// <summary>
        /// Identifies the <see cref="TouchUpCommand"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="TouchUpCommand"/> bindable property.
        /// </value>
        public static readonly BindableProperty TouchUpCommandProperty = BindableProperty.Create(nameof(TouchUpCommand), typeof(ICommand), typeof(SfEffectsView), null, BindingMode.Default);

        /// <summary>
        /// Identifies the <see cref="TouchDownCommandParameter"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="TouchDownCommandParameter"/> bindable property.
        /// </value>
        public static readonly BindableProperty TouchDownCommandParameterProperty = BindableProperty.Create(nameof(TouchDownCommandParameter), typeof(object), typeof(SfEffectsView), null, BindingMode.Default);

        /// <summary>
        /// Identifies the <see cref="TouchUpCommandParameter"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="TouchUpCommandParameter"/> bindable property.
        /// </value>
        public static readonly BindableProperty TouchUpCommandParameterProperty = BindableProperty.Create(nameof(TouchUpCommandParameter), typeof(object), typeof(SfEffectsView), null, BindingMode.Default);

        #endregion

        #region Fields

        private const float AnchorValue = 0.5005f;

        private bool longPressHandled;

        private bool forceReset;

        private Grid? mainGrid;

        private HighlightEffectLayer? highlightEffectLayer;

        private SelectionEffectLayer? selectionEffectLayer;

        private RippleEffectLayer? rippleEffectLayer;

        private bool isSelect;

        private bool isSelectedCalled;

        private bool canRepeat;

        private double tempScaleFactor;

        private string rotationAnimation = "Rotation";

        private string scaleAnimation = "Scaling";

        private string highlightAnimation = "Highlight";

        private Point touchDownPoint;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="SfEffectsView"/> class.
        /// </summary>
        /// <example>
        /// Create SfEffectsView with the effects animation.
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

        #region Properties

        /// <summary>
        /// Gets or sets the content.
        /// </summary>
        /// <value>
        /// Specifies the content of the effects view. The default value is null.
        /// </value>
        /// <example>
        /// The following example shows how to apply the content for the effects view.
        /// <code><![CDATA[
        /// <effectsView:SfEffectsView>
        ///     <effectsView:SfEffectsView.Content>
        ///         <Button Text="Content"
        ///                 WidthRequest="120"
        ///                 HeightRequest="60"/>
        ///     </effectsView:SfEffectsView.Content>
        /// </effectsView:SfEffectsView>
        /// ]]></code>
        /// </example>
        public new View? Content
        {
            get { return (View)this.GetValue(ContentProperty); }
            set { this.SetValue(ContentProperty, value); }
        }

        /// <summary>
        /// Gets or sets the duration of the ripple animation in milliseconds.
        /// </summary>
        /// <value>
        /// Specifies the duration of the ripple animation. The default value is 275d.
        /// </value>
        /// <example>
        /// The following example shows how to apply the ripple animation duration for the effects view.
        /// <code><![CDATA[
        /// <effectsView:SfEffectsView RippleAnimationDuration="1200">
        ///     <effectsView:SfEffectsView.Content>
        ///         <Button Text="Content"
        ///                 WidthRequest="120"
        ///                 HeightRequest="60"/>
        ///     </effectsView:SfEffectsView.Content>
        /// </effectsView:SfEffectsView>
        /// ]]></code>
        /// </example>
        public double RippleAnimationDuration
        {
            get { return (double)this.GetValue(RippleAnimationDurationProperty); }
            set { this.SetValue(RippleAnimationDurationProperty, value); }
        }

        /// <summary>
        /// Gets or sets the duration of the scale animation in milliseconds.
        /// </summary>
        /// <value>
        /// Specifies the duration of the scale animation. The default value is 150d.
        /// </value>
        /// <example>
        /// The following example shows how to apply the scale animation duration for the effects view.
        /// <code><![CDATA[
        /// <effectsView:SfEffectsView ScaleAnimationDuration="1200">
        ///     <effectsView:SfEffectsView.Content>
        ///         <Button Text="Content"
        ///                 WidthRequest="120"
        ///                 HeightRequest="60"/>
        ///     </effectsView:SfEffectsView.Content>
        /// </effectsView:SfEffectsView>
        /// ]]></code>
        /// </example>
        public double ScaleAnimationDuration
        {
            get { return (double)this.GetValue(ScaleAnimationDurationProperty); }
            set { this.SetValue(ScaleAnimationDurationProperty, value); }
        }

        /// <summary>
        /// Gets or sets the duration of the rotation animation in milliseconds.
        /// </summary>
        /// <value>
        /// Specifies the duration of the rotation animation. The default value is 200d.
        /// </value>
        /// <example>
        /// The following example shows how to apply the rotation animation duration for the effects view.
        /// <code><![CDATA[
        /// <effectsView:SfEffectsView RotationAnimationDuration="1200">
        ///     <effectsView:SfEffectsView.Content>
        ///         <Button Text="Content"
        ///                 WidthRequest="120"
        ///                 HeightRequest="60"/>
        ///     </effectsView:SfEffectsView.Content>
        /// </effectsView:SfEffectsView>
        /// ]]></code>
        /// </example>
        public double RotationAnimationDuration
        {
            get { return (double)this.GetValue(RotationAnimationDurationProperty); }
            set { this.SetValue(RotationAnimationDurationProperty, value); }
        }

        /// <summary>
        /// Gets or sets the initial radius factor of ripple effect.
        /// </summary>
        /// <value>
        /// Specifies the initial radius factor of ripple effect. The default value is 0.25d.
        /// </value>
        /// <example>
        /// The following example shows how to apply the initial ripple factor for the effects view.
        /// <code><![CDATA[
        /// <effectsView:SfEffectsView InitialRippleFactor="0.75">
        ///     <effectsView:SfEffectsView.Content>
        ///         <Button Text="Content"
        ///                 WidthRequest="120"
        ///                 HeightRequest="60"/>
        ///     </effectsView:SfEffectsView.Content>
        /// </effectsView:SfEffectsView>
        /// ]]></code>
        /// </example>
        public double InitialRippleFactor
        {
            get { return (double)this.GetValue(InitialRippleFactorProperty); }
            set { this.SetValue(InitialRippleFactorProperty, value); }
        }

        /// <summary>
        /// Gets or sets the scale factor used for scale effect.
        /// </summary>
        /// <value>
        /// Specifies the scale factor of the scale effect. The default value is 1d.
        /// </value>
        /// <example>
        /// The following example shows how to apply the scale factor for the effects view.
        /// <code><![CDATA[
        /// <effectsView:SfEffectsView ScaleFactor="0.5">
        ///     <effectsView:SfEffectsView.Content>
        ///         <Button Text="Content"
        ///                 WidthRequest="120"
        ///                 HeightRequest="60"/>
        ///     </effectsView:SfEffectsView.Content>
        /// </effectsView:SfEffectsView>
        /// ]]></code>
        /// </example>
        public double ScaleFactor
        {
            get { return (double)this.GetValue(ScaleFactorProperty); }
            set { this.SetValue(ScaleFactorProperty, value); }
        }

        /// <summary>
        /// Gets or sets the color to highlight the effects view.
        /// </summary>
        /// <value>
        /// Specifies the highlight color of the effects view. The default value is SolidColorBrush(Colors.Black).
        /// </value>
        /// <example>
        /// The following example shows how to apply the highlight background for the effects view.
        /// <code><![CDATA[
        /// <effectsView:SfEffectsView HighlightBackground="Red">
        ///     <effectsView:SfEffectsView.Content>
        ///         <Button Text="Content"
        ///                 WidthRequest="120"
        ///                 HeightRequest="60"/>
        ///     </effectsView:SfEffectsView.Content>
        /// </effectsView:SfEffectsView>
        /// ]]></code>
        /// </example>
        public Brush HighlightBackground
        {
            get { return (Brush)this.GetValue(HighlightBackgroundProperty); }
            set { this.SetValue(HighlightBackgroundProperty, value); }
        }

        /// <summary>
        /// Gets or sets the color of the ripple.
        /// </summary>
        /// <value>
        /// Specifies the color of the ripple effect. The default value is SolidColorBrush(Colors.Black).
        /// </value>
        /// <example>
        /// The following example shows how to apply the ripple background for the effects view.
        /// <code><![CDATA[
        /// <effectsView:SfEffectsView RippleBackground="Red">
        ///     <effectsView:SfEffectsView.Content>
        ///         <Button Text="Content"
        ///                 WidthRequest="120"
        ///                 HeightRequest="60"/>
        ///     </effectsView:SfEffectsView.Content>
        /// </effectsView:SfEffectsView>
        /// ]]></code>
        /// </example>
        public Brush RippleBackground
        {
            get { return (Brush)this.GetValue(RippleBackgroundProperty); }
            set { this.SetValue(RippleBackgroundProperty, value); }
        }

        /// <summary>
        /// Gets or sets the color applied when the view is on selected state.
        /// </summary>
        /// <value>
        /// Specifies the selection color of the effects view. The default value is SolidColorBrush(Colors.Black).
        /// </value>
        /// <example>
        /// The following example shows how to apply the selection background for the effects view.
        /// <code><![CDATA[
        /// <effectsView:SfEffectsView SelectionBackground="Red">
        ///     <effectsView:SfEffectsView.Content>
        ///         <Button Text="Content"
        ///                 WidthRequest="120"
        ///                 HeightRequest="60"/>
        ///     </effectsView:SfEffectsView.Content>
        /// </effectsView:SfEffectsView>
        /// ]]></code>
        /// </example>
        public Brush SelectionBackground
        {
            get { return (Brush)this.GetValue(SelectionBackgroundProperty); }
            set { this.SetValue(SelectionBackgroundProperty, value); }
        }

        /// <summary>
        /// Gets or sets the rotation angle.
        /// </summary>
        /// <value>
        /// Specifies the rotation angle of the effects view. The default value is 0.
        /// </value>
        /// <example>
        /// The following example shows how to apply the angle for the effects view.
        /// <code><![CDATA[
        /// <effectsView:SfEffectsView Angle="180">
        ///     <effectsView:SfEffectsView.Content>
        ///         <Button Text="Content"
        ///                 WidthRequest="120"
        ///                 HeightRequest="60"/>
        ///     </effectsView:SfEffectsView.Content>
        /// </effectsView:SfEffectsView>
        /// ]]></code>
        /// </example>
        public int Angle
        {
            get { return (int)this.GetValue(AngleProperty); }
            set { this.SetValue(AngleProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether or not the ripple color should fade out as it grows.
        /// </summary>
        /// <value>
        /// Specifies the value whether or not the ripple color should fade out as it grows. The default value is false.
        /// </value>
        /// <example>
        /// The following example shows how to apply the fade out ripple for the effects view.
        /// <code><![CDATA[
        /// <effectsView:SfEffectsView FadeOutRipple="True">
        ///     <effectsView:SfEffectsView.Content>
        ///         <Button Text="Content"
        ///                 WidthRequest="120"
        ///                 HeightRequest="60"/>
        ///     </effectsView:SfEffectsView.Content>
        /// </effectsView:SfEffectsView>
        /// ]]></code>
        /// </example>
        public bool FadeOutRipple
        {
            get { return (bool)this.GetValue(FadeOutRippleProperty); }
            set { this.SetValue(FadeOutRippleProperty, value); }
        }

        /// <summary>
        /// Gets or sets the effect that was start rendering on touch down and start removing on touch up in Android and UWP platforms.
        /// </summary>
        /// <value>
        /// One of the <see cref="Syncfusion.Maui.Core.AutoResetEffects"/> enumeration that specifies the auto reset effect of the effects view. The default value is <see cref="Syncfusion.Maui.Core.AutoResetEffects.None"/>.
        /// </value>
        /// <example>
        /// The following example shows how to apply the auto reset effect for the effects view.
        /// <code><![CDATA[
        /// <effectsView:SfEffectsView AutoResetEffects="Highlight">
        ///     <effectsView:SfEffectsView.Content>
        ///         <Button Text="Content"
        ///                 WidthRequest="120"
        ///                 HeightRequest="60"/>
        ///     </effectsView:SfEffectsView.Content>
        /// </effectsView:SfEffectsView>
        /// ]]></code>
        /// </example>
        public AutoResetEffects AutoResetEffects
        {
            get { return (AutoResetEffects)this.GetValue(AutoResetEffectsProperty); }
            set { this.SetValue(AutoResetEffectsProperty, value); }
        }

        /// <summary>
        /// Gets or sets the pointer-down effect.
        /// </summary>
        /// <value>
        /// One of the <see cref="Syncfusion.Maui.Core.SfEffects"/> enumeration that specifies the touch down effect of the effects view. The default value is <see cref="Syncfusion.Maui.Core.SfEffects.Ripple"/>.
        /// </value>
        /// <example>
        /// The following example shows how to apply the touch down effect for the effects view.
        /// <code><![CDATA[
        /// <effectsView:SfEffectsView TouchDownEffects="Highlight">
        ///     <effectsView:SfEffectsView.Content>
        ///         <Button Text="Content"
        ///                 WidthRequest="120"
        ///                 HeightRequest="60"/>
        ///     </effectsView:SfEffectsView.Content>
        /// </effectsView:SfEffectsView>
        /// ]]></code>
        /// </example>
        public SfEffects TouchDownEffects
        {
            get { return (SfEffects)this.GetValue(TouchDownEffectsProperty); }
            set { this.SetValue(TouchDownEffectsProperty, value); }
        }

        /// <summary>
        /// Gets or sets the pointer-up effect.
        /// </summary>
        /// <value>
        /// One of the <see cref="Syncfusion.Maui.Core.SfEffects"/> enumeration that specifies the touch up effect of the effects view. The default value is <see cref="Syncfusion.Maui.Core.SfEffects.None"/>.
        /// </value>
        /// <example>
        /// The following example shows how to apply the touch up effect for the effects view.
        /// <code><![CDATA[
        /// <effectsView:SfEffectsView TouchUpEffects="Ripple">
        ///     <effectsView:SfEffectsView.Content>
        ///         <Button Text="Content"
        ///                 WidthRequest="120"
        ///                 HeightRequest="60"/>
        ///     </effectsView:SfEffectsView.Content>
        /// </effectsView:SfEffectsView>
        /// ]]></code>
        /// </example>
        public SfEffects TouchUpEffects
        {
            get { return (SfEffects)this.GetValue(TouchUpEffectsProperty); }
            set { this.SetValue(TouchUpEffectsProperty, value); }
        }

        /// <summary>
        /// Gets or sets the long-press effect.
        /// </summary>
        /// <value>
        /// One of the <see cref="Syncfusion.Maui.Core.SfEffects"/> enumeration that specifies the long press effect of the effects view. The default value is <see cref="Syncfusion.Maui.Core.SfEffects.None"/>.
        /// </value>
        /// <example>
        /// The following example shows how to apply the long press effect for the effects view.
        /// <code><![CDATA[
        /// <effectsView:SfEffectsView LongPressEffects="Ripple">
        ///     <effectsView:SfEffectsView.Content>
        ///         <Button Text="Content"
        ///                 WidthRequest="120"
        ///                 HeightRequest="60"/>
        ///     </effectsView:SfEffectsView.Content>
        /// </effectsView:SfEffectsView>
        /// ]]></code>
        /// </example>
        public SfEffects LongPressEffects
        {
            get { return (SfEffects)this.GetValue(LongPressEffectsProperty); }
            set { this.SetValue(LongPressEffectsProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to set the view state as selected.
        /// </summary>
        /// <value>
        /// Specifies the value that indicates whether the view state should be set to selected or not. The default value is false.
        /// </value>
        /// <example>
        /// The following example shows how to apply the IsSelected for the effects view.
        /// <code><![CDATA[
        /// <effectsView:SfEffectsView IsSelected="True">
        ///     <effectsView:SfEffectsView.Content>
        ///         <Button Text="Content"
        ///                 WidthRequest="120"
        ///                 HeightRequest="60"/>
        ///     </effectsView:SfEffectsView.Content>
        /// </effectsView:SfEffectsView>
        /// ]]></code>
        /// </example>
        public bool IsSelected
        {
            get { return (bool)this.GetValue(IsSelectedProperty); }
            set { this.SetValue(IsSelectedProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to ignore the touches in EffectsView.
        /// </summary>
        /// <value>
        /// Specifies the value which indicates whether to ignore the touches in EffectsView. The default value is false.
        /// </value>
        /// <example>
        /// The following example shows how to apply the ShouldIgnoreTouches for the effects view.
        /// <code><![CDATA[
        /// <effectsView:SfEffectsView ShouldIgnoreTouches="True">
        ///     <effectsView:SfEffectsView.Content>
        ///         <Button Text="Content"
        ///                 WidthRequest="120"
        ///                 HeightRequest="60"/>
        ///     </effectsView:SfEffectsView.Content>
        /// </effectsView:SfEffectsView>
        /// ]]></code>
        /// </example>
        public bool ShouldIgnoreTouches
        {
            get { return (bool)this.GetValue(ShouldIgnoreTouchesProperty); }
            set { this.SetValue(ShouldIgnoreTouchesProperty, value); }
        }

        /// <summary>
        /// Gets or sets the command to invoke when handling long press.
        /// </summary>
        /// <value>
        /// Specifies the command to invoke when handling long press in EffectsView. The default value is null.
        /// </value>
        /// <example>
        /// The following example shows how to apply the long pressed command for the effects view.
        /// <code><![CDATA[
        /// <effectsView:SfEffectsView LongPressedCommand="{Binding LongPressedCommand}">
        ///     <effectsView:SfEffectsView.Content>
        ///         <Button Text="Content"
        ///                 WidthRequest="120"
        ///                 HeightRequest="60"/>
        ///     </effectsView:SfEffectsView.Content>
        /// </effectsView:SfEffectsView>
        /// ]]></code>
        /// </example>
        public ICommand LongPressedCommand
        {
            get { return (ICommand)this.GetValue(LongPressedCommandProperty); }
            set { this.SetValue(LongPressedCommandProperty, value); }
        }

        /// <summary>
        /// Gets or sets the command to invoke when handling touch down.
        /// </summary>
        /// <value>
        /// Specifies the command to invoke when handling touch down in EffectsView. The default value is null.
        /// </value>
        /// <example>
        /// The following example shows how to apply the touch down command for the effects view.
        /// <code><![CDATA[
        /// <effectsView:SfEffectsView TouchDownCommand="{Binding TouchDownCommand}">
        ///     <effectsView:SfEffectsView.Content>
        ///         <Button Text="Content"
        ///                 WidthRequest="120"
        ///                 HeightRequest="60"/>
        ///     </effectsView:SfEffectsView.Content>
        /// </effectsView:SfEffectsView>
        /// ]]></code>
        /// </example>
        public ICommand TouchDownCommand
        {
            get { return (ICommand)this.GetValue(TouchDownCommandProperty); }
            set { this.SetValue(TouchDownCommandProperty, value); }
        }

        /// <summary>
        /// Gets or sets the command to invoke when handling touch up.
        /// </summary>
        /// <value>
        /// Specifies the command to invoke when handling touch up in EffectsView. The default value is null.
        /// </value>
        /// <example>
        /// The following example shows how to apply the touch up command for the effects view.
        /// <code><![CDATA[
        /// <effectsView:SfEffectsView TouchUpCommand="{Binding TouchUpCommand}">
        ///     <effectsView:SfEffectsView.Content>
        ///         <Button Text="Content"
        ///                 WidthRequest="120"
        ///                 HeightRequest="60"/>
        ///     </effectsView:SfEffectsView.Content>
        /// </effectsView:SfEffectsView>
        /// ]]></code>
        /// </example>
        public ICommand TouchUpCommand
        {
            get { return (ICommand)this.GetValue(TouchUpCommandProperty); }
            set { this.SetValue(TouchUpCommandProperty, value); }
        }

        /// <summary>
        /// Gets or sets the parameter to pass to the <see cref="TouchDownCommand"/>.
        /// </summary>
        /// <value>
        /// Specifies the parameter of the touch down command in EffectsView. The default value is null.
        /// </value>
        /// <example>
        /// The following example shows how to apply the touch down command parameter for the effects view.
        /// <code><![CDATA[
        /// <effectsView:SfEffectsView x:Name="sfEffectsView"
        ///                            TouchDownCommand="{Binding TouchDownCommand}"
        ///                            TouchDownCommandParameter="{x:Reference sfEffectsView}">
        ///     <effectsView:SfEffectsView.Content>
        ///         <Button Text="Content"
        ///                 WidthRequest="120"
        ///                 HeightRequest="60"/>
        ///     </effectsView:SfEffectsView.Content>
        /// </effectsView:SfEffectsView>
        /// ]]></code>
        /// </example>
        public object TouchDownCommandParameter
        {
            get { return (object)this.GetValue(TouchDownCommandParameterProperty); }
            set { this.SetValue(TouchDownCommandParameterProperty, value); }
        }

        /// <summary>
        /// Gets or sets the parameter to pass to the <see cref="LongPressedCommand"/>.
        /// </summary>
        /// <value>
        /// Specifies the parameter of the long pressed command in EffectsView. The default value is null.
        /// </value>
        /// <example>
        /// The following example shows how to apply the long pressed command parameter for the effects view.
        /// <code><![CDATA[
        /// <effectsView:SfEffectsView x:Name="sfEffectsView"
        ///                            LongPressedCommand="{Binding LongPressedCommand}"
        ///                            LongPressedCommandParameter="{x:Reference sfEffectsView}">
        ///     <effectsView:SfEffectsView.Content>
        ///         <Button Text="Content"
        ///                 WidthRequest="120"
        ///                 HeightRequest="60"/>
        ///     </effectsView:SfEffectsView.Content>
        /// </effectsView:SfEffectsView>
        /// ]]></code>
        /// </example>
        public object LongPressedCommandParameter
        {
            get { return (object)this.GetValue(LongPressedCommandParameterProperty); }
            set { this.SetValue(LongPressedCommandParameterProperty, value); }
        }

        /// <summary>
        /// Gets or sets the parameter to pass to the <see cref="TouchUpCommand"/>.
        /// </summary>
        /// <value>
        /// Specifies the parameter of the touch up command in EffectsView. The default value is null.
        /// </value>
        /// <example>
        /// The following example shows how to apply the touch up command parameter for the effects view.
        /// <code><![CDATA[
        /// <effectsView:SfEffectsView x:Name="sfEffectsView"
        ///                            TouchUpCommand="{Binding TouchUpCommand}"
        ///                            TouchUpCommandParameter="{x:Reference sfEffectsView}">
        ///     <effectsView:SfEffectsView.Content>
        ///         <Button Text="Content"
        ///                 WidthRequest="120"
        ///                 HeightRequest="60"/>
        ///     </effectsView:SfEffectsView.Content>
        /// </effectsView:SfEffectsView>
        /// ]]></code>
        /// </example>
        public object TouchUpCommandParameter
        {
            get { return (object)this.GetValue(TouchUpCommandParameterProperty); }
            set { this.SetValue(TouchUpCommandParameterProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to set the view state as selected.
        /// </summary>
        internal bool IsSelection
        {
            get
            {
                return this.isSelect;
            }

            set
            {
                if (this.isSelect != value)
                {
                    this.isSelect = value;
                    if (value)
                    {
                        this.selectionEffectLayer?.UpdateSelectionBounds(this.Width, this.Height, this.SelectionBackground);
                    }
                    else
                    {
                        this.RemoveSelection();
                    }

                    this.InvokeSelectionChangedEvent();
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the long press handled or not.
        /// </summary>
        internal bool LongPressHandled
        {
            get { return this.longPressHandled; }
            set { this.longPressHandled = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether we want to reset the animation or not.
        /// </summary>
        internal bool ForceReset
        {
            get { return this.forceReset; }
            set { this.forceReset = value; }
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
            if (this.rippleEffectLayer != null)
            {
                this.rippleEffectLayer.CanRemoveRippleAnimation = this.rippleEffectLayer.AnimationIsRunning("RippleAnimator");
                if (!this.rippleEffectLayer.CanRemoveRippleAnimation || this.ForceReset)
                {
                    this.rippleEffectLayer.OnRippleAnimationFinished();
                }
            }

            if (this.highlightEffectLayer != null)
            {
                this.highlightEffectLayer.UpdateHighlightBounds();
            }

            if (this.selectionEffectLayer != null && this.IsSelected)
            {
                this.IsSelected = false;
                this.InvokeSelectionChangedEvent();
            }

            if (this.TouchDownEffects == SfEffects.Scale || this.TouchUpEffects == SfEffects.Scale || this.LongPressEffects == SfEffects.Scale)
            {
                if (this.Content != null)
                {
                    this.Content.Scale = 1;
                }

                this.OnScaleAnimationEnd(0, true);
            }

            if (this.TouchUpEffects == SfEffects.Rotation || this.TouchDownEffects == SfEffects.Rotation || this.LongPressEffects == SfEffects.Rotation)
            {
                if (this.Content != null)
                {
                    this.Content.Rotation = 0;
                }

                this.OnRotationAnimationEnd(0, true);
            }
        }

        /// <summary>
        /// LongPress method.
        /// </summary>
        /// <param name="e">The Long press event arguments.</param>
        public void OnLongPress(LongPressEventArgs e)
        {
            if (!this.ShouldIgnoreTouches)
            {
                this.InvokeLongPressedEventAndCommand();
                this.LongPressHandled = true;

                if (this.AutoResetEffects == AutoResetEffects.None && e != null)
                {
                    this.AddEffects(this.LongPressEffects.ComplementsOf(this.TouchDownEffects), e.TouchPoint);
                }
            }
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
        public void ApplyEffects(SfEffects effects = SfEffects.Ripple, RippleStartPosition rippleStartPosition = RippleStartPosition.Default, System.Drawing.Point? rippleStartPoint = null, bool repeat = false)
        {
            if (this.rippleEffectLayer != null)
            {
                this.rippleEffectLayer.CanRemoveRippleAnimation = false;
            }

            this.canRepeat = repeat;
            float x = (float)(this.Width / 2), y = (float)(this.Height / 2);

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
                x = (float)this.Width;
            }

            if (rippleStartPosition == RippleStartPosition.Bottom)
            {
                y = (float)this.Height;
            }

            if (rippleStartPosition == RippleStartPosition.TopLeft)
            {
                x = 0;
                y = 0;
            }

            if (rippleStartPosition == RippleStartPosition.TopRight)
            {
                x = (float)this.Width;
                y = 0;
            }

            if (rippleStartPosition == RippleStartPosition.BottomLeft)
            {
                x = 0;
                y = (float)this.Height;
            }

            if (rippleStartPosition == RippleStartPosition.BottomRight)
            {
                x = (float)this.Width;
                y = (float)this.Height;
            }

            if (rippleStartPosition == RippleStartPosition.Default)
            {
                if (rippleStartPoint != null)
                {
                    x = rippleStartPoint.Value.X;
                    y = rippleStartPoint.Value.Y;
                }
            }

            this.AddEffects(effects, new Point(x, y));
        }

        /// <summary>
        /// Touch Action method.
        /// </summary>
        /// <param name="e"> The touch event arguments.</param>
        public void OnTouch(TouchEventArgs e)
        {
            if (!this.ShouldIgnoreTouches && e != null)
            {
                if (e.Action == TouchActions.Pressed)
                {
                    this.touchDownPoint = e.TouchPoint;
                    this.LongPressHandled = false;

                    if (this.rippleEffectLayer != null)
                    {
                        this.rippleEffectLayer.CanRemoveRippleAnimation = false;
                    }

                    this.InvokeTouchDownEventAndCommand();

                    if (this.AutoResetEffects != AutoResetEffects.None)
                    {
                        this.AddResetEffects(this.AutoResetEffects, e.TouchPoint);
                    }
                    else
                    {
                        this.AddEffects(this.TouchDownEffects, e.TouchPoint);
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
                        if (this.TouchDownEffects == SfEffects.Highlight || this.TouchDownEffects.GetAllItems().Contains(SfEffects.Highlight))
                        {
                            this.highlightEffectLayer?.UpdateHighlightBounds();
                            if ((this.rippleEffectLayer != null && !this.rippleEffectLayer.AnimationIsRunning("RippleAnimator")) || (this.rippleEffectLayer == null && (this.TouchUpEffects.GetAllItems().Contains(SfEffects.None) || this.TouchUpEffects.GetAllItems().Contains(SfEffects.Highlight)) && (this.LongPressEffects.GetAllItems().Contains(SfEffects.None) || !this.LongPressHandled || this.LongPressEffects.GetAllItems().Contains(SfEffects.Highlight))))
                            {
                                this.InvokeAnimationCompletedEvent();
                            }
                        }

                        if (!this.IsSelected || (!this.IsSelected && (this.TouchDownEffects != SfEffects.Selection || !this.TouchDownEffects.GetAllItems().Contains(SfEffects.Selection))))
                        {
                            this.RemoveSelection();
                        }

                        if (this.TouchDownEffects.GetAllItems().Contains(SfEffects.Ripple) || this.TouchUpEffects.GetAllItems().Contains(SfEffects.Ripple) || this.LongPressEffects.GetAllItems().Contains(SfEffects.Ripple))
                        {
                            if (this.rippleEffectLayer != null)
                            {
                                this.rippleEffectLayer.CanRemoveRippleAnimation = this.rippleEffectLayer.AnimationIsRunning("RippleAnimator");
                            }
                        }

                        if (this.TouchUpEffects != SfEffects.Highlight || !this.TouchUpEffects.GetAllItems().Contains(SfEffects.Highlight))
                        {
                            this.RemoveHighlightEffect();
                        }
                        else
                        {
                            AnimationExtensions.Animate(this.highlightEffectLayer, this.highlightAnimation, this.OnHighlightAnimationUpdate, 16, 250, Easing.Linear, this.OnAnimationFinished, null);
                        }

                        if ((this.TouchUpEffects != SfEffects.Ripple || !this.TouchUpEffects.GetAllItems().Contains(SfEffects.Ripple)) && this.rippleEffectLayer != null && !this.rippleEffectLayer.AnimationIsRunning("RippleAnimator"))
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
                else if (e.Action == TouchActions.Moved)
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
        /// Tap method.
        /// </summary>
        /// <param name="e"> Tap event arguments.</param>
        public void OnTap(TapEventArgs e)
        {
            if (!this.ShouldIgnoreTouches)
            {
                this.LongPressHandled = false;

                if (this.TouchUpEffects != SfEffects.None && e != null)
                {
                    this.AddEffects(this.TouchUpEffects, e.TapPoint);
                }

                if (this.TouchUpEffects.GetAllItems().Contains(SfEffects.Scale))
                {
                    this.StartScaleAnimation();
                }
            }
        }

        #endregion

        #region Internal methods

        /// <summary>
        /// Invokes animation completed event.
        /// </summary>
        /// <param name="eventArgs">Animation completed events argument.</param>
        internal void RaiseAnimationCompletedEvent(EventArgs eventArgs)
        {
            this.AnimationCompleted?.Invoke(this, eventArgs);
        }

        /// <summary>
        /// Invokes when this view selected.
        /// </summary>
        /// <param name="eventArgs">Selected events argument.</param>
        internal void RaiseSelectedEvent(EventArgs eventArgs)
        {
            this.SelectionChanged?.Invoke(this, eventArgs);
        }

        /// <summary>
        /// Invokes <see cref="SelectionChanged"/> event.
        /// </summary>
        internal void InvokeSelectionChangedEvent()
        {
            this.RaiseSelectedEvent(EventArgs.Empty);
        }

        /// <summary>
        /// Invokes <see cref="LongPressed"/> when handling long press.
        /// </summary>
        internal void InvokeLongPressedEventAndCommand()
        {
            this.LongPressed?.Invoke(this, EventArgs.Empty);
            if (this.LongPressedCommand != null && this.LongPressedCommand.CanExecute(this.LongPressedCommandParameter))
            {
                this.LongPressedCommand.Execute(this.LongPressedCommandParameter);
            }
        }

        /// <summary>
        /// Invokes <see cref="TouchDown"/> when handling touch down.
        /// </summary>
        internal void InvokeTouchDownEventAndCommand()
        {
            this.TouchDown?.Invoke(this, EventArgs.Empty);
            if (this.TouchDownCommand != null && this.TouchDownCommand.CanExecute(this.TouchDownCommandParameter))
            {
                this.TouchDownCommand.Execute(this.TouchDownCommandParameter);
            }
        }

        /// <summary>
        /// Invokes <see cref="AnimationCompleted"/> event.
        /// </summary>
        internal void InvokeAnimationCompletedEvent()
        {
            this.RaiseAnimationCompletedEvent(EventArgs.Empty);
        }

        /// <summary>
        ///  Invokes <see cref="TouchUp"/> when handling touch up.
        /// </summary>
        internal void InvokeTouchUpEventAndCommand()
        {
            this.TouchUp?.Invoke(this, EventArgs.Empty);
            if (this.TouchUpCommand != null && this.TouchUpCommand.CanExecute(this.TouchUpCommandParameter))
            {
                this.TouchUpCommand.Execute(this.TouchUpCommandParameter);
            }
        }

        #endregion

        #region Override methods

        /// <summary>
        /// MeasureOverride method.
        /// </summary>
        /// <param name="widthConstraint"> The width.</param>
        /// <param name="heightConstraint"> The height.</param>
        /// <returns>The size.</returns>
        // TODO: To avoid argument width and height lesser than zero exception when not setting width and height to the control.
        protected override Size MeasureOverride(double widthConstraint, double heightConstraint)
        {
            if (this.rippleEffectLayer != null)
            {
                if (widthConstraint != double.PositiveInfinity && widthConstraint != double.NegativeInfinity)
                {
                    this.rippleEffectLayer.WidthRequest = widthConstraint;
                }

                if (heightConstraint != double.PositiveInfinity && heightConstraint != double.NegativeInfinity)
                {
                    this.rippleEffectLayer.HeightRequest = heightConstraint;
                }
            }

            if (this.highlightEffectLayer != null)
            {
                if (widthConstraint != double.PositiveInfinity && widthConstraint != double.NegativeInfinity)
                {
                    this.highlightEffectLayer.WidthRequest = widthConstraint;
                }

                if (heightConstraint != double.PositiveInfinity && heightConstraint != double.NegativeInfinity)
                {
                    this.highlightEffectLayer.HeightRequest = heightConstraint;
                }
            }

            if (this.selectionEffectLayer != null)
            {
                if (widthConstraint != double.PositiveInfinity && widthConstraint != double.NegativeInfinity)
                {
                    this.selectionEffectLayer.WidthRequest = widthConstraint;
                }

                if (heightConstraint != double.PositiveInfinity && heightConstraint != double.NegativeInfinity)
                {
                    this.selectionEffectLayer.HeightRequest = heightConstraint;
                }
            }

            return base.MeasureOverride(widthConstraint, heightConstraint);
        }

        /// <summary>
        /// ArrangeOverride method.
        /// </summary>
        /// <param name="bounds">The bounds.</param>
        /// <returns>The size.</returns>
        // TODO: To avoid argument width and height lesser than zero exception when not setting width and height to the control.
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
                if (this.IsSelected)
                {
                    this.selectionEffectLayer?.UpdateSelectionBounds(bounds.Width, bounds.Height, this.SelectionBackground);
                    if (!this.isSelectedCalled)
                    {
                        this.InvokeSelectionChangedEvent();
                        this.isSelectedCalled = true;
                    }
                }
            }

            return base.ArrangeOverride(bounds);
        }

        #endregion

        #region Property changed

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
                    // TODO:To avoid the touch issue in parent, this code has been  added.This code need to be removed once touch issue fixed.
                    effectsView.IsEnabled = !(bool)newValue;
                }
            }
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Updateselection method.
        /// </summary>
        /// <param name="selectionBackground">The selection background.</param>
        private void UpdateSelectionBackground(Brush selectionBackground)
        {
            if (this.IsSelected)
            {
                this.selectionEffectLayer?.UpdateSelectionBounds(this.Width, this.Height, selectionBackground);
            }
        }

        /// <summary>
        /// Remove the selection effect.
        /// </summary>
        private void RemoveSelection()
        {
            this.selectionEffectLayer?.UpdateSelectionBounds();
        }

        /// <summary>
        /// Remove the highlight effect.
        /// </summary>
        private void RemoveHighlightEffect()
        {
            if (this.highlightEffectLayer != null)
            {
                this.highlightEffectLayer.UpdateHighlightBounds();
            }
        }

        /// <summary>
        /// Method used to add respective auto reset effects.
        /// </summary>
        /// <param name="effects"> The effects value.</param>
        /// <param name="touchPoint"> The touch point.</param>
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
                    AnimationExtensions.Animate(this.highlightEffectLayer, this.highlightAnimation, this.OnHighlightAnimationUpdate, 16, 250, Easing.Linear, this.OnAnimationFinished, null);
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
        /// <param name="sfEffect">The Effects.</param>
        /// <param name="touchPoint">The touch point.</param>
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
                    this.rippleEffectLayer?.StartRippleAnimation(touchPoint, this.RippleBackground, this.RippleAnimationDuration, (float)this.InitialRippleFactor, this.FadeOutRipple, this.canRepeat);
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
            this.rippleEffectLayer?.OnRippleAnimationFinished();
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
                this.Content.AnchorX = AnchorValue;
                this.Content.AnchorY = AnchorValue;
                this.tempScaleFactor = this.ScaleFactor;
                AnimationExtensions.Animate(
                    this.Content,
                    this.scaleAnimation,
                    this.OnScaleAnimationUpdate,
                    this.Content.Scale,
                    this.ScaleFactor,
                    16,
                    (uint)this.ScaleAnimationDuration,
                    Easing.Linear,
                    this.OnScaleAnimationEnd,
                    null);
            }
        }

        /// <summary>
        /// Rotation animation method.
        /// </summary>
        private void StartRotationAnimation()
        {
            if (this.Content != null)
            {
                this.Content.AnchorX = AnchorValue;
                this.Content.AnchorY = AnchorValue;

                AnimationExtensions.Animate(
                    this.Content,
                    this.rotationAnimation,
                    this.OnAnimationUpdate,
                    this.Content.Rotation,
                    this.Angle,
                    16,
                    (uint)this.RotationAnimationDuration,
                    Easing.Linear,
                    this.OnRotationAnimationEnd,
                    null);
            }
        }

        /// <summary>
        /// Animation ended method.
        /// </summary>
        /// <param name="value"> The animation value.</param>
        /// <param name="finished"> The finished.</param>
        private void OnRotationAnimationEnd(double value, bool finished)
        {
            AnimationExtensions.AbortAnimation(this, this.rotationAnimation);

            if ((this.rippleEffectLayer != null && !this.rippleEffectLayer.AnimationIsRunning("RippleAnimator")) || (this.rippleEffectLayer == null && (this.TouchUpEffects.GetAllItems().Contains(SfEffects.None) || this.TouchUpEffects.GetAllItems().Contains(SfEffects.Rotation)) && (this.LongPressEffects.GetAllItems().Contains(SfEffects.None) || !this.LongPressHandled || this.LongPressEffects.GetAllItems().Contains(SfEffects.Rotation))))
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
            AnimationExtensions.AbortAnimation(this, this.scaleAnimation);
            if ((this.rippleEffectLayer != null && !this.rippleEffectLayer.AnimationIsRunning("RippleAnimator")) || (this.rippleEffectLayer == null && (this.TouchUpEffects.GetAllItems().Contains(SfEffects.None) || this.TouchUpEffects.GetAllItems().Contains(SfEffects.Scale)) && (this.LongPressEffects.GetAllItems().Contains(SfEffects.None) || !this.LongPressHandled || this.LongPressEffects.GetAllItems().Contains(SfEffects.Scale))))
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
        /// Add content to the main view.
        /// </summary>
        /// <param name="view">The view.</param>
        private void AddContent(View view)
        {
            if (view != null)
            {
                this.mainGrid?.Insert(0, view);
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
        /// <param name="completed">Completed property.</param>
        private void OnAnimationFinished(double value, bool completed)
        {
            this.highlightEffectLayer?.UpdateHighlightBounds();
            if ((this.rippleEffectLayer != null && !this.rippleEffectLayer.AnimationIsRunning("RippleAnimator")) || (this.rippleEffectLayer == null && (this.TouchUpEffects.GetAllItems().Contains(SfEffects.None) || this.TouchUpEffects.GetAllItems().Contains(SfEffects.Highlight)) && (this.LongPressEffects.GetAllItems().Contains(SfEffects.None) || !this.LongPressHandled || this.LongPressEffects.GetAllItems().Contains(SfEffects.Highlight))))
            {
                this.InvokeAnimationCompletedEvent();
            }
        }

        #endregion
    }
}
