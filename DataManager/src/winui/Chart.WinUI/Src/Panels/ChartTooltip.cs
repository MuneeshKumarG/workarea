// <copyright file="ChartTooltip.cs" company="Syncfusion. Inc">
// Copyright Syncfusion Inc. 2001 - 2017. All rights reserved.
// Use of this code is subject to the terms of our license.
// A copy of the current license can be obtained at any time by e-mailing
// licensing@syncfusion.com. Any infringement will be prosecuted under
// applicable laws. 
// </copyright>
namespace Syncfusion.UI.Xaml.Charts
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using Microsoft.UI.Xaml;
    using Microsoft.UI.Xaml.Controls;
    using Microsoft.UI.Xaml.Interop;
    using Microsoft.UI.Xaml.Media;
    using Windows.Foundation;

    /// <summary>
    /// Represents a content control that display a information about focused element. 
    /// </summary>
    /// <seealso cref="ContentControl" />
    public class ChartTooltip : ContentControl
    {
        #region Dependency Property Registration

        /// <summary>
        ///  The DependencyProperty for Duration property.
        /// </summary>
        public static readonly DependencyProperty DurationProperty =
            DependencyProperty.RegisterAttached(
                "Duration",
                typeof(int),
                typeof(ChartTooltip),
                new PropertyMetadata(1000));

        /// <summary>
        ///  The DependencyProperty for InitialShowDelay property.
        /// </summary>
        public static readonly DependencyProperty InitialShowDelayProperty =
            DependencyProperty.RegisterAttached(
                "InitialShowDelay",
                typeof(int),
                typeof(ChartTooltip),
                new PropertyMetadata(0));

        /// <summary>
        ///  The DependencyProperty for HorizontalOffset property.
        /// </summary>
        public static readonly DependencyProperty HorizontalOffsetProperty =
            DependencyProperty.RegisterAttached(
                "HorizontalOffset",
                typeof(double),
                typeof(ChartTooltip),
                new PropertyMetadata(0d));

        /// <summary>
        ///  The DependencyProperty for VerticalOffset property.
        /// </summary>
        public static readonly DependencyProperty VerticalOffsetProperty =
            DependencyProperty.RegisterAttached(
                "VerticalOffset",
                typeof(double),
                typeof(ChartTooltip),
                new PropertyMetadata(0d));

        /// <summary>
        /// The DependencyProperty for <see cref="HorizontalAlignment"/> property.
        /// </summary>
        public static new readonly DependencyProperty HorizontalAlignmentProperty =
            DependencyProperty.RegisterAttached(
                "HorizontalAlignment",
                typeof(HorizontalAlignment),
                typeof(ChartTooltip),
                new PropertyMetadata(HorizontalAlignment.Center));

        /// <summary>
        /// The DependencyProperty for <see cref="VerticalAlignment"/> property.
        /// </summary>
        public static new readonly DependencyProperty VerticalAlignmentProperty =
            DependencyProperty.RegisterAttached(
                "VerticalAlignment",
                typeof(VerticalAlignment),
                typeof(ChartTooltip),
                new PropertyMetadata(VerticalAlignment.Top));

        /// <summary>
        /// The DependencyProperty for EnableAnimation property.
        /// </summary>
        public static readonly DependencyProperty EnableAnimationProperty =
            DependencyProperty.RegisterAttached(
                "EnableAnimation",
                typeof(bool),
                typeof(ChartTooltip),
                new PropertyMetadata(true));

        /// <summary>
        /// The DependencyProperty for TooltipMargin property.
        /// </summary>
        public static readonly DependencyProperty TooltipMarginProperty =
            DependencyProperty.RegisterAttached(
                "TooltipMargin",
                typeof(Thickness),
                typeof(ChartTooltip),
                new PropertyMetadata(new Thickness().GetThickness(0, 0, 0, 0)));

        /// <summary>
        ///  The DependencyProperty for <see cref="TopOffset"/> property.
        /// </summary>
        internal static readonly DependencyProperty TopOffsetProperty =
            DependencyProperty.Register(
                "TopOffset",
                typeof(double),
                typeof(ChartTooltip),
                new PropertyMetadata(0d));

        /// <summary>
        ///  The DependencyProperty for <see cref="LeftOffset"/> property.
        /// </summary>
        internal static readonly DependencyProperty LeftOffsetProperty =
            DependencyProperty.Register(
                "LeftOffset",
                typeof(double),
                typeof(ChartTooltip),
                new PropertyMetadata(0d));

        internal static readonly DependencyProperty PolygonPathProperty =
            DependencyProperty.Register("PolygonPath", typeof(string), typeof(ChartTooltip), new PropertyMetadata(" "));

        internal static readonly DependencyProperty BackgroundStyleProperty =
            DependencyProperty.Register("BackgroundStyle", typeof(Style), typeof(ChartTooltip), new PropertyMetadata(null));

        internal static readonly DependencyProperty LabelStyleProperty =
          DependencyProperty.Register("LabelStyle", typeof(Style), typeof(ChartTooltip), new PropertyMetadata(null));

        #endregion

        #region Fields

        private bool isAnnotationTooltip;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="ChartTooltip"/> class.
        /// </summary>
        public ChartTooltip()
        {
            DefaultStyleKey = typeof(ChartTooltip);
            this.IsHitTestVisible = false;
            Canvas.SetLeft(this, 0d);
            Canvas.SetTop(this, 0d);
        }

        internal ChartTooltip(bool annotationTooltip)
        {
            isAnnotationTooltip = annotationTooltip;
            this.IsHitTestVisible = false;
            Canvas.SetLeft(this, 0d);
            Canvas.SetTop(this, 0d);
        }

        #endregion

        #region Properties

        internal string PolygonPath
        {
            get { return (string)GetValue(PolygonPathProperty); }
            set { SetValue(PolygonPathProperty, value); }
        }

        internal Style BackgroundStyle
        {
            get { return (Style)GetValue(BackgroundStyleProperty); }
            set { SetValue(BackgroundStyleProperty, value); }
        }

        internal Style LabelStyle
        {
            get { return (Style)GetValue(LabelStyleProperty); }
            set { SetValue(LabelStyleProperty, value); }
        }

        /// <summary>
        /// Gets or sets the left offset.
        /// </summary>
        internal double LeftOffset
        {
            get { return (double)GetValue(LeftOffsetProperty); }
            set { SetValue(LeftOffsetProperty, value); }
        }

        /// <summary>
        /// Gets or sets the top offset.
        /// </summary>
        internal double TopOffset
        {
            get { return (double)GetValue(TopOffsetProperty); }
            set { SetValue(TopOffsetProperty, value); }
        }

        internal HorizontalPosition HorizontalPosition { get; set; }

        internal VerticalPosition VerticalPosition { get; set; }

        internal ChartSeries PreviousSeries { get; set; }

        #endregion

        #region Methods

        #region Public Methods

        /// <summary>
        /// Gets whether animation  is enabled for tooltip or not.
        /// </summary>
        /// <param name="obj">The Object</param>
        /// <returns>Returns a value indicating whether the animation is enabled.</returns>
        public static bool GetEnableAnimation(UIElement obj)
        {
            return (bool)obj.GetValue(EnableAnimationProperty);
        }

        /// <summary>
        /// Sets the value to enable/disable the tooltip animation.
        /// </summary>
        /// <param name="obj">The Object</param>
        /// <param name="value">The Value</param>
        public static void SetEnableAnimation(UIElement obj, bool value)
        {
            obj.SetValue(EnableAnimationProperty, value);
        }

        /// <summary>
        /// Gets the horizontal alignment for the tooltip.
        /// </summary>        /// 
        /// <param name="obj">The Object</param>
        /// <returns>Returns the horizontal alignment.</returns>
        public static HorizontalAlignment GetHorizontalAlignment(UIElement obj)
        {
            return (HorizontalAlignment)obj.GetValue(HorizontalAlignmentProperty);
        }

        /// <summary>
        /// Sets the horizontal alignment to the tooltip.
        /// </summary>
        /// <param name="obj">The Object</param>
        /// <param name="value">The Value</param>
        public static void SetHorizontalAlignment(UIElement obj, HorizontalAlignment value)
        {
            obj.SetValue(HorizontalAlignmentProperty, value);
        }

        /// <summary>
        /// Gets the vertical alignment for the tooltip.
        /// </summary>
        /// <param name="obj">The Object</param>
        /// <returns>Returns the vertical alignment.</returns>
        public static VerticalAlignment GetVerticalAlignment(UIElement obj)
        {
            return (VerticalAlignment)obj.GetValue(VerticalAlignmentProperty);
        }

        /// <summary>
        /// Sets the vertical alignment to the tooltip.
        /// </summary>
        /// <param name="obj">The Object</param>
        /// <param name="value">The Value</param>
        public static void SetVerticalAlignment(UIElement obj, VerticalAlignment value)
        {
            obj.SetValue(VerticalAlignmentProperty, value);
        }

        /// <summary>
        /// Gets the margin of the tooltip.
        /// </summary>
        /// <param name="obj">The Object</param>
        /// <returns>Returns the <see cref="ChartTooltip"/> margin.</returns>
        public static Thickness GetTooltipMargin(UIElement obj)
        {
            return (Thickness)obj.GetValue(TooltipMarginProperty);
        }

        /// <summary>
        /// Sets the margin to the tooltip.
        /// </summary>
        /// <param name="obj">The Object</param>
        /// <param name="value">The Value</param>
        public static void SetTooltipMargin(UIElement obj, Thickness value)
        {
            obj.SetValue(TooltipMarginProperty, value);
        }

        /// <summary>
        /// Gets the duration of the tooltip text in seconds.
        /// </summary>
        /// <param name="obj">The Dependency Object</param>
        /// <returns>Returns the show duration.</returns>
        public static int GetDuration(DependencyObject obj)
        {
            return (int)obj.GetValue(DurationProperty);
        }

        /// <summary>
        /// Sets the duration to the tooltip.
        /// </summary>
        /// <param name="obj">The Dependency Object</param>
        /// <param name="value">The Value</param>
        public static void SetDuration(DependencyObject obj, int value)
        {
            obj.SetValue(DurationProperty, value);
        }

        /// <summary>
        /// Gets the initial delay value to show the tooltip.
        /// </summary>
        /// <param name="obj">The Dependency Object</param>
        /// <returns>Returns the show delay.</returns>
        public static int GetInitialShowDelay(DependencyObject obj)
        {
            return (int)obj.GetValue(InitialShowDelayProperty);
        }

        /// <summary>
        /// Sets the initial delay value to show the tooltip.
        /// </summary>
        /// <param name="obj">The Dependency Object</param>
        /// <param name="value">The Value</param>
        public static void SetInitialShowDelay(DependencyObject obj, int value)
        {
            obj.SetValue(InitialShowDelayProperty, value);
        }

        /// <summary>
        /// Gets the horizontal offset value to position the tooltip.
        /// </summary>
        /// <param name="obj">The Dependency Object</param>
        /// <returns>Returns the horizontal offset.</returns>
        public static double GetHorizontalOffset(DependencyObject obj)
        {
            return (double)obj.GetValue(HorizontalOffsetProperty);
        }

        /// <summary>
        /// Sets the horizontal offset value to position the tooltip.
        /// </summary>
        /// <param name="obj">The Dependency Object</param>
        /// <param name="value">The Value</param>
        public static void SetHorizontalOffset(DependencyObject obj, double value)
        {
            obj.SetValue(HorizontalOffsetProperty, value);
        }

        /// <summary>
        /// Gets the vertical offset value to position the tooltip.
        /// </summary>
        /// <param name="obj">The Dependency Object</param>
        /// <returns>Returns the vertical offset.</returns>
        public static double GetVerticalOffset(DependencyObject obj)
        {
            return (double)obj.GetValue(VerticalOffsetProperty);
        }

        /// <summary>
        /// Sets the vertical offset value to position the tooltip.
        /// </summary>
        /// <param name="obj">The Dependency Object</param>
        /// <param name="value">The Value</param>
        public static void SetVerticalOffset(DependencyObject obj, double value)
        {
            obj.SetValue(VerticalOffsetProperty, value);
        }

        #endregion

        #region Internal Methods

        internal static HorizontalAlignment GetActualHorizontalAlignment(ChartTooltipBehavior tooltipBehavior, HorizontalAlignment horizontalAlignment)
        {
            if (tooltipBehavior == null)
            {
                return horizontalAlignment;
            }
            return horizontalAlignment != HorizontalAlignment.Center ? horizontalAlignment : tooltipBehavior.HorizontalAlignment;
        }

        internal static VerticalAlignment GetActualVerticalAlignment(ChartTooltipBehavior tooltipBehavior, VerticalAlignment verticalAlignment)
        {
            if (tooltipBehavior == null)
            {
                return verticalAlignment;
            }
            return verticalAlignment != VerticalAlignment.Top ? verticalAlignment : tooltipBehavior.VerticalAlignment;
        }

        internal static double GetActualHorizontalOffset(ChartTooltipBehavior tooltipBehavior, double horizontalOffset)
        {
            if (tooltipBehavior == null)
            {
                return horizontalOffset;
            }
            return horizontalOffset != 0 ? horizontalOffset : tooltipBehavior.HorizontalOffset;
        }

        internal static double GetActualVerticalOffset(ChartTooltipBehavior tooltipBehavior, double verticalOffset)
        {
            if (tooltipBehavior == null)
            {
                return verticalOffset;
            }
            return verticalOffset != 0 ? verticalOffset : tooltipBehavior.VerticalOffset;
        }

        internal static int GetActualDuration(ChartTooltipBehavior tooltipBehavior, int duration)
        {
            if (tooltipBehavior == null)
            {
                return duration;
            }

            return duration != 1000 ? duration : tooltipBehavior.Duration;
        }

        internal static int GetActualInitialShowDelay(ChartTooltipBehavior tooltipBehavior, int initialShowDelay)
        {
            if (tooltipBehavior == null)
            {
                return initialShowDelay;
            }
            return initialShowDelay != 0 ? initialShowDelay : tooltipBehavior.InitialShowDelay;
        }

        internal static bool GetActualEnableAnimation(ChartTooltipBehavior tooltipBehavior, bool enableAnimation)
        {
            if (tooltipBehavior == null)
            {
                return enableAnimation;
            }
            return enableAnimation != true ? enableAnimation : tooltipBehavior.EnableAnimation;
        }

        internal static Thickness GetActualTooltipMargin(ChartTooltipBehavior tooltipBehavior, Thickness margin)
        {
            if (tooltipBehavior == null)
            {
                return margin;
            }
            return !margin.Equals(new Thickness().GetThickness(0, 0, 0, 0)) ? margin : tooltipBehavior.Margin;
        }

        #endregion

        #endregion
    }
}
