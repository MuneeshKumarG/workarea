using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Collections.Generic;
using Syncfusion.Maui.Core.Internals;
using System;
using Syncfusion.Maui.Core;
using Microsoft.Maui.Layouts;
namespace Syncfusion.Maui.Charts
{
    /// <summary>
    /// The <see cref="ChartBase"/> class is the base for <see cref="SfCartesianChart"/> and <see cref="SfCircularChart"/> types.
    /// </summary>
    public abstract class ChartBase : View, IContentView, IChart
    {
        #region Fields
        internal readonly LegendLayout LegendLayout;
        private ChartSelectionBehavior? defaultSelectionBehavior;
        private ChartTooltipBehavior? defaultToolTipBehavior;
        private ChartSelectionBehavior? selectionBehavior;
        private ChartTooltipBehavior? toolTipBehavior;
        private AreaBase area;
        private SfTooltip? tooltipView;
        private Rect actualSeriesClipRect;
        #endregion

        #region Bindable Properties

        /// <summary>
        /// Identifies the <see cref="Title"/> bindable property.
        /// </summary>        
        /// <value>
        /// The identifier for <see cref="Title"/> bindable property.
        /// </value>        
        public static readonly BindableProperty TitleProperty =
            BindableProperty.Create(nameof(Title), typeof(object), typeof(ChartBase), null,
                                    propertyChanged: OnTitlePropertyChanged);

        /// <summary>
        /// Identifies the <see cref="Legend"/> bindable property.
        /// </summary>        
        /// <value>
        /// The identifier for <see cref="Legend"/> bindable property.
        /// </value>        
        public static readonly BindableProperty LegendProperty =
           BindableProperty.Create(nameof(Legend), typeof(ChartLegend), typeof(ChartBase), null,
                                   propertyChanged: OnLegendPropertyChanged);

        /// <summary>
        /// Identifies the <see cref="TooltipBehavior"/> bindable property.
        /// </summary>        
        /// <value>
        /// The identifier for <see cref="TooltipBehavior"/> bindable property.
        /// </value>        
        public static readonly BindableProperty TooltipBehaviorProperty = BindableProperty.Create(nameof(TooltipBehavior), typeof(ChartTooltipBehavior), typeof(ChartBase), null, BindingMode.Default, null, OnTooltipBehaviorPropertyChanged);

        /// <summary>
        /// Identifies the <see cref="SelectionBehavior"/> bindable property.
        /// </summary>        
        /// <value>
        /// The identifier for <see cref="SelectionBehavior"/> bindable property.
        /// </value>        
        public static readonly BindableProperty SelectionBehaviorProperty = BindableProperty.Create(nameof(SelectionBehavior), typeof(ChartSelectionBehavior), typeof(ChartBase), null, BindingMode.Default, null, OnSelectionBehaviorPropertyChanged);
        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the title for chart. It supports the string or any view as title.
        /// </summary>
        /// <value>Default value is null.</value>
        /// 
        /// <remarks>
        /// 
        /// <para>Example code for string as title.</para>
        /// 
        /// # [MainPage.xaml](#tab/tabid-1)
        /// <code><![CDATA[
        ///     <chart:SfCartesianChart Title="Average High/Low Temperature">
        ///           
        ///     </chart:SfCartesianChart>
        /// ]]></code>
        /// 
        /// # [MainPage.xaml.cs](#tab/tabid-2)
        /// <code><![CDATA[
        ///     SfCartesianChart chart = new SfCartesianChart();
        ///     chart.Title = "Average High / Low Temperature";
        /// ]]></code>
        /// ***
        /// 
        /// <para>Example code for View as title.</para>
        /// 
        /// # [MainPage.xaml](#tab/tabid-1)
        /// <code><![CDATA[
        ///     <chart:SfCartesianChart>
        ///
        ///           <chart:SfCartesianChart.Title>
        ///               <Label Text = "Average High/Low Temperature" HorizontalOptions="Fill" HorizontalTextAlignment="Center" VerticalOptions="Center" FontSize="16" TextColor="Black"/>
        ///           </chart:SfCartesianChart.Title>
        ///           
        ///     </chart:SfCartesianChart>
        /// ]]></code>
        /// 
        /// # [MainPage.xaml.cs](#tab/tabid-2)
        /// <code><![CDATA[
        ///     SfCartesianChart chart = new SfCartesianChart();
        ///     chart.Title = new Label() { Text = "Average High / Low Temperature", HorizontalOptions = LayoutOptions.Fill, HorizontalTextAlignment = TextAlignment.Center, VerticalOptions = LayoutOptions.Center, FontSize = 16, TextColor = Colors.Black };
        /// ]]></code>
        /// ***
        /// </remarks>
        public object Title
        {
            get { return (object)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        /// <summary>
        /// Gets or sets the legend that helps to identify the corresponding series or data point in chart.
        /// </summary>
        /// <value>This property takes a <see cref="ChartLegend"/> instance as value and its default value is null.</value>
        /// <example>
        /// # [MainPage.xaml](#tab/tabid-1)
        /// <code> <![CDATA[
        /// <chart:SfCircularChart>
        ///
        ///        <chart:SfCircularChart.BindingContext>
        ///            <local:ViewModel/>
        ///        </chart:SfCircularChart.BindingContext>
        ///        
        ///        <chart:SfCircularChart.Legend>
        ///            <chart:ChartLegend/>
        ///        </chart:SfCircularChart.Legend>
        ///
        ///        <chart:SfCircularChart.Series>
        ///            <chart:PieSeries ItemsSource="{Binding Data}" XBindingPath="XValue" YBindingPath="YValue"/>
        ///        </chart:SfCircularChart.Series>
        ///        
        /// </chart:SfCircularChart>
        /// ]]>
        /// </code>
        /// # [MainPage.xaml.cs](#tab/tabid-2)
        /// <code><![CDATA[
        /// SfCircularChart chart = new SfCircularChart();
        /// 
        /// ViewModel viewModel = new ViewModel();
        ///	chart.BindingContext = viewModel;
        ///	
        /// chart.Legend = new ChartLegend();
        /// 
        /// PieSeries series = new PieSeries()
        /// {
        ///     ItemsSource = viewmodel.Data,
        ///     XBindingPath = "XValue",
        ///     YBindingPath = "YValue",
        /// };
        /// chart.Series.Add(series);
        /// 
        /// ]]>
        /// </code>
        /// ***
        /// </example>
        /// 
        ///<remarks>
        /// <para>To render a legend, create an instance of <see cref="ChartLegend"/>, and assign it to the <see cref="Legend"/> property. </para>
        ///</remarks>
        public ChartLegend? Legend
        {
            get { return (ChartLegend)GetValue(LegendProperty); }
            set { SetValue(LegendProperty, value); }
        }

        /// <summary>
        /// Gets or sets a tooltip behavior that allows to customize the default tooltip appearance in the chart. 
        /// </summary>
        /// <value>This property takes <see cref="ChartTooltipBehavior"/> instance as value and its default value is null.</value>
        /// <remarks>
        /// 
        /// <para>To display the tooltip on the chart, set the <see cref="ChartSeries.EnableTooltip"/> property as <b>true</b> in <b>ChartSeries</b>.</para>
        /// 
        /// <para>To customize the appearance of the tooltip elements like Background, TextColor and Font, create an instance of <see cref="ChartTooltipBehavior"/> class, modify the values, and assign it to the chart’s <see cref="TooltipBehavior"/> property. </para>
        /// 
        /// # [MainPage.xaml](#tab/tabid-1)
        /// <code><![CDATA[
        /// <chart:SfCircularChart>
        /// 
        ///         <chart:SfCircularChart.BindingContext>
        ///             <local:ViewModel/>
        ///         </chart:SfCircularChart.BindingContext>
        /// 
        ///         <chart:SfCircularChart.TooltipBehavior>
        ///             <chart:ChartTooltipBehavior/>
        ///         </chart:SfCircularChart.TooltipBehavior>
        /// 
        ///         <chart:SfCircularChart.Series>
        ///             <chart:PieSeries EnableTooltip="True" ItemsSource="{Binding Data}" XBindingPath="XValue" YBindingPath="YValue1"/>
        ///         </chart:SfCircularChart.Series>
        /// 
        /// </chart:SfCircularChart>
        /// ]]>
        /// </code>
        /// # [MainPage.xaml.cs](#tab/tabid-2)
        /// <code><![CDATA[
        ///  SfCircularChart chart = new SfCircularChart();
        ///  
        ///  ViewModel viewModel = new ViewModel();
        ///	 chart.BindingContext = viewModel;
        ///  
        ///  chart.TooltipBehavior = new ChartTooltipBehavior();
        ///  
        ///  PieSeries series = new PieSeries()
        ///  {
        ///     ItemsSource = viewmodel.Data,
        ///     XBindingPath = "XValue",
        ///     YBindingPath = "YValue1",
        ///     EnableTooltip = true
        ///  };
        ///  chart.Series.Add(series);
        ///
        /// ]]>
        /// </code>
        /// ***
        /// 
        /// </remarks>
        /// <seealso cref="ChartSeries.EnableTooltip"/>
        public ChartTooltipBehavior? TooltipBehavior
        {
            get { return (ChartTooltipBehavior)GetValue(TooltipBehaviorProperty); }
            set { SetValue(TooltipBehaviorProperty, value); }
        }

        /// <summary>
        /// Gets or sets a selection behavior that enables you to select or highlight a segment in series.
        /// </summary>
        /// <value>This property takes <see cref="ChartSelectionBehavior"/> instance as value and its default value is null.</value>
        /// 
        /// <remarks><para>To highlight the selected the segment, set the value for <see cref="ChartSeries.SelectionBrush"/> property in <see cref="ChartSeries"/> class.</para>
        /// 
        /// # [MainPage.xaml](#tab/tabid-1)
        /// <code><![CDATA[
        /// <chart:SfCircularChart>
        /// 
        ///         <chart:SfCircularChart.BindingContext>
        ///             <local:ViewModel/>
        ///         </chart:SfCircularChart.BindingContext>
        /// 
        ///         <chart:SfCircularChart.SelectionBehavior>
        ///             <chart:ChartSelectionBehavior/>
        ///         </chart:SfCircularChart.SelectionBehavior>
        /// 
        ///         <chart:SfCircularChart.Series>
        ///             <chart:PieSeries SelectionBrush="Blue" ItemsSource="{Binding Data}" XBindingPath="XValue" YBindingPath="YValue1"/>
        ///         </chart:SfCircularChart.Series>
        /// 
        /// </chart:SfCircularChart>
        /// ]]>
        /// </code>
        /// 
        /// # [MainPage.xaml.cs](#tab/tabid-2)
        /// <code><![CDATA[
        ///  SfCircularChart chart = new SfCircularChart();
        ///  
        ///  ViewModel viewModel = new ViewModel();
        ///	 chart.BindingContext = viewModel;
        ///  
        ///  chart.SelectionBehavior = new ChartSelectionBehavior();
        ///  
        ///  DoughnutSeries series = new DoughnutSeries()
        ///  {
        ///     ItemsSource = viewmodel.Data,
        ///     XBindingPath = "XValue",
        ///     YBindingPath = "YValue1",
        ///     SelectionBrush = Colors.Blue
        ///  };
        ///  chart.Series.Add(series);
        ///
        /// ]]>
        /// </code>
        /// ***
        /// 
        /// </remarks>
        /// <seealso cref="ChartSeries.SelectedIndex"/>
        /// <seealso cref="ChartSeries.SelectionBrush"/>
        /// <seealso cref="SelectionChanging"/>
        /// <seealso cref="SelectionChanged"/>
        public ChartSelectionBehavior? SelectionBehavior
        {
            get { return (ChartSelectionBehavior)GetValue(SelectionBehaviorProperty); }
            set { SetValue(SelectionBehaviorProperty, value); }
        }

        #endregion

        #region Public Events

        /// <summary>
        /// Occurs when the user clicks on the series segment or sets the value for <see cref="ChartSeries.SelectedIndex"/> property in <see cref="ChartSeries"/>. This event is triggered before the data point is selected. 
        /// </summary>
        /// <remarks>Restrict a data point from being selected, by canceling this event, by setting <see cref="ChartSelectionChangingEventArgs.Cancel"/> property to true in the event argument.</remarks>
        public event EventHandler<ChartSelectionChangingEventArgs>? SelectionChanging;

        /// <summary>
        /// Occurs when the user clicks on series segment or sets the value for the <see cref="ChartSeries.SelectedIndex"/> property in <see cref="ChartSeries"/>. Here you can get the corresponding series, current selected index, and previous selected index. 
        /// </summary>
        public event EventHandler<ChartSelectionChangedEventArgs>? SelectionChanged;

        #endregion

        #region Internal Properties
        internal View? Content { get; set; }

        internal ChartTitleView TitleView { get; set; }

        internal AbsoluteLayout BehaviorLayout { get; set; }

        #endregion

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="ChartBase"/> class.
        /// </summary>
        public ChartBase()
        {
            TitleView = new ChartTitleView();
            BehaviorLayout = new AbsoluteLayout();
            area = CreateChartArea();
            LegendLayout = new LegendLayout(area);
            Content = CreateTemplate(LegendLayout);
        }

        internal virtual AreaBase CreateChartArea()
        {
            throw new ArgumentNullException("Chart area cannot be null");
        }

        private View CreateTemplate(LegendLayout legendLayout)
        {
            Grid grid = new Grid();
            grid.AddRowDefinition(new RowDefinition() { Height = GridLength.Auto });
            grid.AddRowDefinition(new RowDefinition() { Height = GridLength.Auto });
            Grid.SetRow(TitleView, 0);
            grid.Add(TitleView);
            Grid.SetRow(legendLayout, 1);
            grid.Add(legendLayout);

            return grid;
        }
        #endregion

        #region Protected Methods

        /// <summary>
        /// Invoked when binding context changed.
        /// </summary>
        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();

            if (Content != null)
            {
                SetInheritedBindingContext(Content, BindingContext);
            }

            if (this.Title != null && Title is View view)
            {
                SetInheritedBindingContext(view, BindingContext);
            }
        }
        #endregion

        #region Interface Overrides

        object? IContentView.Content => Content;

        /// <summary>
        /// Gets the presented content value.
        /// </summary>
        IView? IContentView.PresentedContent => Content;

        /// <summary>
        /// Gets the padding value.
        /// </summary>
        Thickness IPadding.Padding => Thickness.Zero;

        ChartSelectionBehavior? IChart.ActualSelectionBehavior
        {
            get
            {
                if (SelectionBehavior == null)
                {
                    if (defaultSelectionBehavior == null)
                    {
                        defaultSelectionBehavior = new ChartSelectionBehavior();
                        defaultSelectionBehavior.IsDefault = true;
                    }

                    return defaultSelectionBehavior;
                }

                return selectionBehavior;
            }
            set
            {
                selectionBehavior = value;
            }
        }

        ChartTooltipBehavior? IChart.ActualTooltipBehavior
        {
            get
            {
                if (TooltipBehavior == null)
                {
                    if (defaultToolTipBehavior == null)
                    {
                        defaultToolTipBehavior = new ChartTooltipBehavior();
                        // defaultToolTipBehavior.IsDefault = true;
                    }

                    return defaultToolTipBehavior;
                }

                return toolTipBehavior;
            }
            set
            {
                toolTipBehavior = value;
            }
        }

        SfTooltip? IChart.TooltipView
        {
            get { return tooltipView; }

            set
            {
                tooltipView = value;
            }
        }

        Rect IChart.ActualSeriesClipRect
        {
            get { return actualSeriesClipRect; }

            set
            {
                actualSeriesClipRect = value;
            }
        }

        IArea IChart.Area => area;

        Color IChart.BackgroundColor => BackgroundColor;

        AbsoluteLayout IChart.BehaviorLayout => BehaviorLayout;

        double IChart.TitleHeight => TitleView != null ? TitleView.Height : 0;

        void IChart.RaiseSelectionChangedEvent(ChartSelectionChangedEventArgs args)
        {
            SelectionChanged?.Invoke(this, args);
        }

        void IChart.RaiseSelectionChangingEvent(ChartSelectionChangingEventArgs args)
        {
            SelectionChanging?.Invoke(this, args);
        }

        Size IContentView.CrossPlatformMeasure(double widthConstraint, double heightConstraint)
        {
            return this.MeasureContent(widthConstraint, heightConstraint);
        }

        Size IContentView.CrossPlatformArrange(Rect bounds)
        {
            this.ArrangeContent(bounds);
            return bounds.Size;
        }
        #endregion

        #region Property call back methods
        private static void OnTitlePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var chart = bindable as ChartBase;
            if (chart != null && chart.TitleView != null)
            {
                if (newValue != null)
                {
                    if (newValue is View view)
                    {
                        chart.TitleView.Content = view;
                    }
                    else
                    {
                        chart.TitleView.InitTitle(newValue.ToString());
                    }
                }
            }
        }

        private static void OnLegendPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is ChartBase chart)
            {
                var layout = chart.LegendLayout;
                if (layout != null && newValue is ILegend legend)
                {
                    layout.Legend = legend;
                    layout.AreaBase.ScheduleUpdateArea();
                }
            }
        }

        private static void OnTooltipBehaviorPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is ChartBase chart)
            {
                var tooltip = newValue as ChartTooltipBehavior;
                SetInheritedBindingContext(tooltip, chart.BindingContext);
                chart.toolTipBehavior = tooltip;
            }
        }

        private static void OnSelectionBehaviorPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is ChartBase chart)
            {
                var selection = newValue as ChartSelectionBehavior;
                SetInheritedBindingContext(selection, chart.BindingContext);
                chart.selectionBehavior = selection; 
            }
        }

        #endregion
    }
}
