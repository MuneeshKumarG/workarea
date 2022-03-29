using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using Syncfusion.Maui.Core;
using Syncfusion.Maui.Core.Internals;
using System.Collections.ObjectModel;

namespace Syncfusion.Maui.Charts
{
    /// <summary>
    /// 
    /// </summary>
    /// <example>
    /// # [Xaml](#tab/tabid-1)
    /// <code><![CDATA[
    ///     <chart:SfCartesianChart>
    ///
    ///           <chart:SfCartesianChart.ChartBehaviors>
    ///               <chart:ChartTooltipBehavior />
    ///           </chart:SfCartesianChart.ChartBehaviors>
    ///           
    ///     </chart:SfCartesianChart>
    /// ]]></code>
    /// # [C#](#tab/tabid-2)
    /// <code><![CDATA[
    ///     SfCartesianChart chart = new SfCartesianChart();
    ///     
    ///     ChartTooltipBehavior tooltipBehavior = new ChartTooltipBehavior();
    ///     chart.ChartBehaviors.Add(tooltipBehavior);
    ///     
    /// ]]></code>
    /// ***
    /// </example>
    public class ChartTooltipBehavior : ChartBehavior
    {
        private TooltipInfo? previousTooltipInfo = null;

        #region Bindable Properties

        /// <summary>
        /// 
        /// </summary> 
        public static readonly BindableProperty BackgroundProperty =
            BindableProperty.Create(nameof(Background), typeof(Brush), typeof(ChartTooltipBehavior), new SolidColorBrush(Colors.Black), BindingMode.Default, null);

        /// <summary>
        /// 
        /// </summary>        
        public static readonly BindableProperty DurationProperty =
            BindableProperty.Create(nameof(Duration), typeof(int), typeof(ChartTooltipBehavior), 2, BindingMode.Default, null);

        /// <summary>
        /// 
        /// </summary>
        public static readonly BindableProperty TextColorProperty = 
            BindableProperty.Create(nameof(TextColor), typeof(Color), typeof(ChartTooltipBehavior), Colors.White, BindingMode.Default, null);

        /// <summary>
        /// 
        /// </summary>
        public static readonly BindableProperty MarginProperty = 
            BindableProperty.Create(nameof(Margin), typeof(Thickness), typeof(ChartTooltipBehavior), new Thickness(0), BindingMode.Default, null);

        /// <summary>
        /// 
        /// </summary>   
        public static readonly BindableProperty FontSizeProperty = 
            BindableProperty.Create(nameof(FontSize), typeof(float), typeof(ChartTooltipBehavior), 14f, BindingMode.Default, null);

        /// <summary>
        /// 
        /// </summary>
        public static readonly BindableProperty FontFamilyProperty = 
            BindableProperty.Create(nameof(FontFamily), typeof(string), typeof(ChartTooltipBehavior), null, BindingMode.Default, null);

        /// <summary>
        /// 
        /// </summary>
        public static readonly BindableProperty FontAttributesProperty = 
            BindableProperty.Create(nameof(FontAttributes), typeof(FontAttributes), typeof(ChartTooltipBehavior), FontAttributes.None, BindingMode.Default, null);

        #endregion

        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public Brush Background
        {
            get { return (Brush)GetValue(BackgroundProperty); }
            set { SetValue(BackgroundProperty, value); }
        }

        /// <summary>
        /// 
        /// </summary>
        public int Duration
        {
            get { return (int)GetValue(DurationProperty); }
            set { SetValue(DurationProperty, value); }
        }

        /// <summary>
        /// 
        /// </summary>
        public Color TextColor
        {
            get { return (Color)GetValue(TextColorProperty); }
            set { SetValue(TextColorProperty, value); }
        }

        /// <summary>
		/// 
        /// </summary>
        public Thickness Margin
        {
            get { return (Thickness)GetValue(MarginProperty); }
            set { SetValue(MarginProperty, value); }
        }

        /// <summary>
        /// 
        /// </summary>
        public float FontSize
        {
            get { return (float)GetValue(FontSizeProperty); }
            set { SetValue(FontSizeProperty, value); }
        }

        /// <summary>
        /// 
        /// </summary>
        public string FontFamily
        {
            get { return (string)GetValue(FontFamilyProperty); }
            set { SetValue(FontFamilyProperty, value); }
        }

        /// <summary>
        /// 
        /// </summary>
        public FontAttributes FontAttributes
        {
            get { return (FontAttributes)GetValue(FontAttributesProperty); }
            set { SetValue(FontAttributesProperty, value); }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// 
        /// </summary>
        public ChartTooltipBehavior()
        {
        }

        #endregion

        #region Methods

        internal override void OnSingleTap(IChart chart, float pointX, float pointY)
        {
            base.OnSingleTap(chart, pointX, pointY);

            if (chart != null)
            {
                Show(chart, pointX, pointY, false);
            }
        }

        internal override void OnTouchMove(IChart chart, float pointX, float pointY, PointerDeviceType pointerDeviceType)
        {
            if (pointerDeviceType == PointerDeviceType.Mouse)
            {
                Show(chart, pointX, pointY, false);
            }
        }

        /// <summary>
        /// Method used to show tooltip in view. 
        /// </summary>
        /// <param name="chart"></param>
        /// <param name="pointX"></param>
        /// <param name="pointY"></param>
        /// <param name="canAnimate"></param>
        internal void Show(IChart chart, float pointX, float pointY, bool canAnimate)
        {
            GenerateTooltip(chart, pointX, pointY, canAnimate);
        }

        /// <summary>
        /// Hides the tooltip.
        /// </summary>
        internal void Hide(IChart chart)
        {
            if (chart.TooltipView != null && chart.BehaviorLayout.Children.Contains(chart.TooltipView))
            {
                chart.BehaviorLayout.Children.Remove(chart.TooltipView);
                chart.TooltipView = null;
            }

            previousTooltipInfo = null;
        }

        private void GenerateTooltip(IChart chart, float x, float y, bool canAnimate)
        {
            Rect seriesBounds = chart.ActualSeriesClipRect;

            if (seriesBounds.Contains(x, y))
            {
                TooltipInfo? tooltipInfo = GetTooltipInfo(chart, x, y);

                if (tooltipInfo != null && (previousTooltipInfo == null || previousTooltipInfo.Index != tooltipInfo.Index || previousTooltipInfo.Series != tooltipInfo.Series))
                {
                    Hide(chart);
                    tooltipInfo.Series?.SetTooltipTargetRect(tooltipInfo, seriesBounds);
                    SfTooltip tooltip = new SfTooltip();
                    tooltip.BindingContext = tooltipInfo;
                    tooltip.Duration = Duration;
                    tooltip.Position = tooltipInfo.Position;
                    tooltip.SetBinding(SfTooltip.BackgroundProperty, nameof(TooltipInfo.Background));
                    tooltip.Content = GetTooltipTemplate(tooltipInfo);
                    chart.TooltipView = tooltip;
                    tooltip.TooltipClosed += Tooltip_TooltipClosed;
                    chart.BehaviorLayout.Add(chart.TooltipView);
                    chart.TooltipView.Show(seriesBounds, tooltipInfo.TargetRect, canAnimate);
                    previousTooltipInfo = tooltipInfo;
                }
            }
        }

        private void Tooltip_TooltipClosed(object? sender, TooltipClosedEventArgs e)
        {
            previousTooltipInfo = null;
        }

        /// <summary>
        /// Method used to get the default tooltip template.
        /// </summary>
        private View? GetTooltipTemplate(TooltipInfo tooltipInfo)
        {
            View? view = null;

            if (tooltipInfo.Series != null && tooltipInfo.Series.TooltipTemplate != null)
            {
                var layout = tooltipInfo.Series.TooltipTemplate.CreateContent();
                view = layout is ViewCell ? (layout as ViewCell)?.View : layout as View;
            }
            else
            {
                var layout = GetDefaultTooltipTemplate().CreateContent();
                view = layout is ViewCell ? (layout as ViewCell)?.View : layout as View;
            }

            if (view != null)
            {
                var size = view.Measure(double.PositiveInfinity, double.PositiveInfinity).Request;
                view.Layout(new Rect(0, 0, size.Width, size.Height));
            }

            return view;
        }

        private DataTemplate GetDefaultTooltipTemplate()
        {
            var template = new DataTemplate(() =>
            {
                Label label = new Label();
                label.VerticalOptions = LayoutOptions.Fill;
                label.HorizontalOptions = LayoutOptions.Fill;
                label.VerticalTextAlignment = TextAlignment.Center;
                label.HorizontalTextAlignment = TextAlignment.Center;
                label.SetBinding(Label.TextProperty, nameof(TooltipInfo.Text));
                label.SetBinding(Label.TextColorProperty, nameof(TooltipInfo.TextColor));
                label.SetBinding(Label.MarginProperty, nameof(TooltipInfo.Margin));
                label.SetBinding(Label.FontSizeProperty, nameof(TooltipInfo.FontSize));
                label.SetBinding(Label.FontFamilyProperty, nameof(TooltipInfo.FontFamily));
                label.SetBinding(Label.FontAttributesProperty, nameof(TooltipInfo.FontAttributes));

                return new ViewCell { View = label };
            });

            return template;
        }

        private TooltipInfo? GetTooltipInfo(IChart chart, float x, float y)
        {
            var visibleSeries = (chart.Area as IChartArea)?.VisibleSeries;

            if (visibleSeries != null)
            {
                for (int i = visibleSeries.Count - 1; i >= 0; i--)
                {
                    ChartSeries chartSeries = visibleSeries[i];

                    if (!chartSeries.EnableTooltip || chartSeries.PointsCount <= 0)
                    {
                        continue;
                    }

                    TooltipInfo? tooltipInfo = chartSeries.GetTooltipInfo(this, x, y);

                    if (tooltipInfo != null)
                    {
                        return tooltipInfo;
                    }
                }
            }

            return null;
        }

        #endregion
    }
}
