using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using Syncfusion.Maui.Charts;
using System.Linq;

namespace Syncfusion.Maui.Charts
{
    /// <summary>
    /// ChartSeries is the base class for all chart types.
    /// </summary>
    public abstract partial class ChartSeries : BindableObject, IDatapointSelectionDependent, ITooltipDependent, IDataManagerDependent
    {

        #region Bindable Properties
        /// <summary>
        /// Identifies the <see cref="ItemsSource"/> bindable property.
        /// </summary>        
        public static readonly BindableProperty ItemsSourceProperty =
            BindableProperty.Create(nameof(ItemsSource), typeof(object), typeof(ChartSeries), null, BindingMode.Default, null, OnItemsSourceChanged);

        /// <summary>
        /// Identifies the <see cref="XBindingPath"/> bindable property.
        /// </summary>        
        public static readonly BindableProperty XBindingPathProperty =
            BindableProperty.Create(nameof(XBindingPath), typeof(string), typeof(ChartSeries), null, BindingMode.Default, null, OnXBindingPathChanged);

        /// <summary>
        /// Identifies the <see cref="Fill"/> bindable property.
        /// </summary>        
        public static readonly BindableProperty FillProperty =
            BindableProperty.Create(nameof(Fill), typeof(Brush), typeof(ChartSeries), null, BindingMode.Default, null, OnFillPropertyChanged);

        /// <summary>
        /// Identifies the <see cref="PaletteBrushes"/> bindable property.
        /// </summary>        
        public static readonly BindableProperty PaletteBrushesProperty =
            BindableProperty.Create(nameof(PaletteBrushes), typeof(IList<Brush>), typeof(ChartSeries), null, BindingMode.Default, null, OnPaletteBrushesChanged);

        /// <summary>
        /// Identifies the <see cref="IsVisible"/> bindable property.
        /// </summary>        
        public static readonly BindableProperty IsVisibleProperty =
            BindableProperty.Create(nameof(IsVisible), typeof(bool), typeof(ChartSeries), true, BindingMode.Default, null, OnVisiblePropertyChanged);

        /// <summary>
        /// Identifies the <see cref="Opacity"/> bindable property.
        /// </summary>        
        public static readonly BindableProperty OpacityProperty =
            BindableProperty.Create(nameof(Opacity), typeof(double), typeof(ChartSeries), 1d, BindingMode.Default, null, OnOpacityPropertyChanged);

        /// <summary>
        /// Identifies the <see cref="EnableAnimation"/> bindable property.
        /// </summary>        
        public static readonly BindableProperty EnableAnimationProperty =
            BindableProperty.Create(nameof(EnableAnimation), typeof(bool), typeof(ChartSeries), false, BindingMode.Default, null, OnEnableAnimationPropertyChanged);

        /// <summary>
        /// Identifies the <see cref="EnableTooltip"/> bindable property.
        /// </summary>     
        public static readonly BindableProperty EnableTooltipProperty =
           BindableProperty.Create(nameof(EnableTooltip), typeof(bool), typeof(ChartSeries), false, BindingMode.Default, null, OnEnableTooltipPropertyChanged);

        /// <summary>
        /// Identifies the <see cref="TooltipTemplate"/> bindable property.
        /// </summary>  
        public static readonly BindableProperty TooltipTemplateProperty =
           BindableProperty.Create(nameof(TooltipTemplate), typeof(DataTemplate), typeof(ChartSeries), null, BindingMode.Default, null, null);

        /// <summary>
        /// Identifies the <see cref="ShowDataLabels"/> bindable property.
        /// </summary>
        public static readonly BindableProperty ShowDataLabelsProperty =
            BindableProperty.Create(nameof(ShowDataLabels), typeof(bool), typeof(ChartSeries), false, BindingMode.Default, null, OnShowDataLabelsChanged);

        /// <summary>
        /// Identifies the <see cref="LegendIcon"/> bindable property.
        /// </summary>        
        public static readonly BindableProperty LegendIconProperty =
            BindableProperty.Create(nameof(LegendIcon), typeof(ChartLegendIconType), typeof(ChartSeries), ChartLegendIconType.Circle, BindingMode.Default, null, OnLegendIconChanged);

        /// <summary>
        /// Identifies the <see cref="ListenPropertyChange"/> bindable property.
        /// </summary>
        internal static readonly BindableProperty ListenPropertyChangeProperty =
           BindableProperty.Create(nameof(ListenPropertyChange), typeof(bool), typeof(ChartSeries), false, BindingMode.Default, null, OnListenPropertyChanged);
        
        /// <summary>
        /// Identifies the <see cref="IsVisibleOnLegend"/> bindable property.
        /// </summary>        
        public static readonly BindableProperty IsVisibleOnLegendProperty =
            BindableProperty.Create(
                nameof(IsVisibleOnLegend),
                typeof(bool),
                typeof(ChartSeries),
                true,
                BindingMode.Default,
                null,
                null);

        /// <summary>
        /// Identifies the <see cref="SelectionBehavior"/> bindable property.
        /// </summary> 

        public static readonly BindableProperty SelectionBehaviorProperty =
            BindableProperty.Create(nameof(SelectionBehavior), typeof(DataPointSelectionBehavior), typeof(ChartSeries), null, BindingMode.Default, null, OnSelectionBehaviorPropertyChanged);

        /// <summary>
        /// 
        /// </summary>        
        internal static readonly BindableProperty AnimationDurationProperty =
            BindableProperty.Create(nameof(AnimationDuration), typeof(double), typeof(ChartSeries), 1d, BindingMode.Default, null, null);
        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="ChartSeries"/>.
        /// </summary>
        public ChartSeries()
        {
            Segments = new ObservableCollection<ChartSegment>();
            Segments.CollectionChanged += Segments_CollectionChanged;
        }

        #endregion

        #region Public Properties
        /// <summary>
        /// Gets or sets a data points collection that will be used to plot a chart.
        /// </summary>
        /// <example>
        /// # [Xaml](#tab/tabid-1)
        /// <code><![CDATA[
        ///     <chart:SfCartesianChart>
        ///
        ///     <!-- ... Eliminated for simplicity-->
        ///
        ///          <chart:ColumnSeries ItemsSource="{Binding Data}"
        ///                            XBindingPath="XValue"
        ///                            YBindingPath="YValue" />
        ///
        ///     </chart:SfCartesianChart>
        /// ]]></code>
        /// # [C#](#tab/tabid-2)
        /// <code><![CDATA[
        ///     SfCartesianChart chart = new SfCartesianChart();
        ///     ViewModel viewModel = new ViewModel();
        ///
        ///     // Eliminated for simplicity
        ///
        ///     ColumnSeries columnSeries = new ColumnSeries()
        ///     {
        ///           ItemsSource = viewModel.Data,
        ///           XBindingPath = "XValue",
        ///           YBindingPath = "YValue",
        ///     };
        ///     
        ///     chart.Series.Add(columnSeries);
        ///
        /// ]]></code>
        /// ***
        /// </example>
        public object ItemsSource
        {
            get { return (object)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        /// <summary>
        /// Gets or sets a path value on the source object to serve a x value to the series.
        /// </summary>
        /// <value>
        /// The string that represents the property name for the x plotting data, and its default value is null.
        /// </value>
        /// <example>
        /// # [Xaml](#tab/tabid-3)
        /// <code><![CDATA[
        ///     <chart:SfCartesianChart>
        ///
        ///     <!-- ... Eliminated for simplicity-->
        ///
        ///          <chart:ColumnSeries ItemsSource="{Binding Data}"
        ///                            XBindingPath="XValue"
        ///                            YBindingPath="YValue" />
        ///
        ///     </chart:SfCartesianChart>
        /// ]]></code>
        /// # [C#](#tab/tabid-4)
        /// <code><![CDATA[
        ///     SfCartesianChart chart = new SfCartesianChart();
        ///     ViewModel viewModel = new ViewModel();
        ///
        ///     // Eliminated for simplicity
        ///
        ///     ColumnSeries columnSeries = new ColumnSeries()
        ///     {
        ///           ItemsSource = viewModel.Data,
        ///           XBindingPath = "XValue",
        ///           YBindingPath = "YValue",
        ///     };
        ///     
        ///     chart.Series.Add(columnSeries);
        ///
        /// ]]></code>
        /// ***
        /// </example>
        public string XBindingPath
        {
            get { return (string)GetValue(XBindingPathProperty); }
            set { SetValue(XBindingPathProperty, value); }
        }

        /// <summary>
        /// Gets or sets a brush value to customize the series appearance.
        /// </summary>
        /// <value>It accepts a <see cref="Brush"/> value and its default value is null.</value>
        /// <example>
        /// # [Xaml](#tab/tabid-5)
        /// <code><![CDATA[
        ///     <chart:SfCartesianChart>
        ///
        ///     <!-- ... Eliminated for simplicity-->
        ///
        ///          <chart:ColumnSeries ItemsSource="{Binding Data}"
        ///                            XBindingPath="XValue"
        ///                            YBindingPath="YValue"
        ///                            Fill = "Red"/>
        ///
        ///     </chart:SfCartesianChart>
        /// ]]></code>
        /// # [C#](#tab/tabid-6)
        /// <code><![CDATA[
        ///     SfCartesianChart chart = new SfCartesianChart();
        ///     ViewModel viewModel = new ViewModel();
        ///
        ///     // Eliminated for simplicity
        ///
        ///     ColumnSeries columnSeries = new ColumnSeries()
        ///     {
        ///           ItemsSource = viewModel.Data,
        ///           XBindingPath = "XValue",
        ///           YBindingPath = "YValue",
        ///           Fill = new SolidColorBrush(Colors.Red),
        ///     };
        ///     
        ///     chart.Series.Add(columnSeries);
        ///
        /// ]]></code>
        /// ***
        /// </example>
        public Brush Fill
        {
            get { return (Brush)GetValue(FillProperty); }
            set { SetValue(FillProperty, value); }
        }

        /// <summary>
        /// Gets or sets the list of brushes that can be used to customize the appearance of the series.
        /// </summary>
        /// <value>This property accepts a list of brushes as input and comes with a set of predefined brushes by default.</value>
        /// <example>
        /// # [Xaml](#tab/tabid-5)
        /// <code><![CDATA[
        ///     <chart:SfCartesianChart>
        ///
        ///     <!-- ... Eliminated for simplicity-->
        ///
        ///          <chart:ColumnSeries ItemsSource="{Binding Data}"
        ///                            XBindingPath="XValue"
        ///                            YBindingPath="YValue"
        ///                            PaletteBrushes = "{Binding CustomBrushes}"/>
        ///
        ///     </chart:SfCartesianChart>
        /// ]]></code>
        /// # [C#](#tab/tabid-6)
        /// <code><![CDATA[
        ///     SfCartesianChart chart = new SfCartesianChart();
        ///     ViewModel viewModel = new ViewModel();
        ///     List<Brush> CustomBrushes = new List<Brush>();
        ///     CustomBrushes.Add(new SolidColorBrush(Color.FromRgb(77, 208, 225)));
        ///     CustomBrushes.Add(new SolidColorBrush(Color.FromRgb(38, 198, 218)));
        ///     CustomBrushes.Add(new SolidColorBrush(Color.FromRgb(0, 188, 212)));
        ///     CustomBrushes.Add(new SolidColorBrush(Color.FromRgb(0, 172, 193)));
        ///     CustomBrushes.Add(new SolidColorBrush(Color.FromRgb(0, 151, 167)));
        ///     CustomBrushes.Add(new SolidColorBrush(Color.FromRgb(0, 131, 143)));
        ///
        ///     // Eliminated for simplicity
        ///
        ///     ColumnSeries columnSeries = new ColumnSeries()
        ///     {
        ///           ItemsSource = viewModel.Data,
        ///           XBindingPath = "XValue",
        ///           YBindingPath = "YValue",
        ///           PaletteBrushes = CustomBrushes;
        ///     };
        ///     
        ///     chart.Series.Add(columnSeries);
        ///
        /// ]]></code>
        /// ***
        /// </example>
        public IList<Brush> PaletteBrushes
        {
            get { return (IList<Brush>)GetValue(PaletteBrushesProperty); }
            set { SetValue(PaletteBrushesProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value that indicates whether the series is visible or not.
        /// </summary>
        /// <value>It accepts bool values and its default value is <c>True</c>.</value>
        /// <example>
        /// # [Xaml](#tab/tabid-7)
        /// <code><![CDATA[
        ///     <chart:SfCartesianChart>
        ///
        ///     <!-- ... Eliminated for simplicity-->
        ///
        ///          <chart:ColumnSeries ItemsSource="{Binding Data}"
        ///                            XBindingPath="XValue"
        ///                            YBindingPath="YValue"
        ///                            IsVisible="False"/>
        ///
        ///     </chart:SfCartesianChart>
        /// ]]></code>
        /// # [C#](#tab/tabid-8)
        /// <code><![CDATA[
        ///     SfCartesianChart chart = new SfCartesianChart();
        ///     ViewModel viewModel = new ViewModel();
        ///
        ///     // Eliminated for simplicity
        ///
        ///     ColumnSeries columnSeries = new ColumnSeries()
        ///     {
        ///           ItemsSource = viewModel.Data,
        ///           XBindingPath = "XValue",
        ///           YBindingPath = "YValue",
        ///           IsVisible = false,
        ///     };
        ///     
        ///     chart.Series.Add(columnSeries);
        ///
        /// ]]></code>
        /// ***
        /// </example>
        public bool IsVisible
        {
            get { return (bool)GetValue(IsVisibleProperty); }
            set { SetValue(IsVisibleProperty, value); }
        }

        /// <summary>
        /// 
        /// </summary>
        internal bool ListenPropertyChange
        {
            get { return (bool)GetValue(ListenPropertyChangeProperty); }
            set { SetValue(ListenPropertyChangeProperty, value); }
        }

        /// <summary>
        /// Gets or sets opacity of the chart series.
        /// </summary>
        /// <value>It accepts double values and the default value is 1. Here, the value ranges from 0 to 1.</value>
        /// <example>
        /// # [Xaml](#tab/tabid-7)
        /// <code><![CDATA[
        ///     <chart:SfCartesianChart>
        ///
        ///     <!-- ... Eliminated for simplicity-->
        ///
        ///          <chart:ColumnSeries ItemsSource="{Binding Data}"
        ///                            XBindingPath="XValue"
        ///                            YBindingPath="YValue"
        ///                            Opacity="0.5"/>
        ///
        ///     </chart:SfCartesianChart>
        /// ]]></code>
        /// # [C#](#tab/tabid-8)
        /// <code><![CDATA[
        ///     SfCartesianChart chart = new SfCartesianChart();
        ///     ViewModel viewModel = new ViewModel();
        ///
        ///     // Eliminated for simplicity
        ///
        ///     ColumnSeries columnSeries = new ColumnSeries()
        ///     {
        ///           ItemsSource = viewModel.Data,
        ///           XBindingPath = "XValue",
        ///           YBindingPath = "YValue",
        ///           Opacity = 0.5,
        ///     };
        ///     
        ///     chart.Series.Add(columnSeries);
        ///
        /// ]]></code>
        /// ***
        /// </example> 
        public double Opacity
        {
            get { return (double)GetValue(OpacityProperty); }
            set { SetValue(OpacityProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to animate the chart series on loading.
        /// </summary>
        /// <value> It accepts bool values and its default value is <c>False</c>.</value>
        /// <example>
        /// # [Xaml](#tab/tabid-9)
        /// <code><![CDATA[
        ///     <chart:SfCartesianChart>
        ///
        ///     <!-- ... Eliminated for simplicity-->
        ///
        ///          <chart:ColumnSeries ItemsSource="{Binding Data}"
        ///                            XBindingPath="XValue"
        ///                            YBindingPath="YValue"
        ///                            EnableAnimation = "True"/>
        ///
        ///     </chart:SfCartesianChart>
        /// ]]></code>
        /// # [C#](#tab/tabid-10)
        /// <code><![CDATA[
        ///     SfCartesianChart chart = new SfCartesianChart();
        ///     ViewModel viewModel = new ViewModel();
        ///
        ///     // Eliminated for simplicity
        ///
        ///     ColumnSeries columnSeries = new ColumnSeries()
        ///     {
        ///           ItemsSource = viewModel.Data,
        ///           XBindingPath = "XValue",
        ///           YBindingPath = "YValue",
        ///           EnableAnimation = true,
        ///     };
        ///     
        ///     chart.Series.Add(columnSeries);
        ///
        /// ]]></code>
        /// ***
        /// </example>
        public bool EnableAnimation
        {
            get { return (bool)GetValue(EnableAnimationProperty); }
            set { SetValue(EnableAnimationProperty, value); }
        }

        /// <summary>
        /// Gets or sets a boolean value indicating whether the tooltip for series should be shown or hidden.
        /// </summary>
        /// <remarks>The series tooltip will appear when you click or tap the series area.</remarks>
        /// <value>It accepts bool values and its default value is <c>False</c>.</value>
        /// <example>
        /// # [Xaml](#tab/tabid-11)
        /// <code><![CDATA[
        ///     <chart:SfCartesianChart>
        ///
        ///     <!-- ... Eliminated for simplicity-->
        ///
        ///          <chart:ColumnSeries ItemsSource="{Binding Data}"
        ///                            XBindingPath="XValue"
        ///                            YBindingPath="YValue"
        ///                            EnableTooltip="True"/>
        ///
        ///     </chart:SfCartesianChart>
        /// ]]></code>
        /// # [C#](#tab/tabid-12)
        /// <code><![CDATA[
        ///     SfCartesianChart chart = new SfCartesianChart();
        ///     ViewModel viewModel = new ViewModel();
        ///
        ///     // Eliminated for simplicity
        ///
        ///     ColumnSeries columnSeries = new ColumnSeries()
        ///     {
        ///           ItemsSource = viewModel.Data,
        ///           XBindingPath = "XValue",
        ///           YBindingPath = "YValue",
        ///           EnableTooltip = true,
        ///     };
        ///     
        ///     chart.Series.Add(columnSeries);
        ///
        /// ]]></code>
        /// ***
        /// </example>
        public bool EnableTooltip
        {
            get { return (bool)GetValue(EnableTooltipProperty); }
            set { SetValue(EnableTooltipProperty, value); }
        }

        /// <summary>
        /// Gets or sets the DataTemplate that can be used to customize the appearance of the tooltip.
        /// </summary>
        /// <value>
        /// It accepts a <see cref="DataTemplate"/> value.
        /// </value>
        /// <example>
        /// # [MainWindow.xaml](#tab/tabid-13)
        /// <code><![CDATA[
        ///     <chart:SfCartesianChart>
        ///
        ///           <chart:SfCartesianChart.Resources>
        ///               <DataTemplate x:Key="tooltipTemplate1">
        ///                  <StackLayout Orientation = "Horizontal" >
        ///                     <Label Text="{Binding Item.XValue}" 
        ///                            TextColor="Black"
        ///                            FontAttributes="Bold"
        ///                            FontSize="12"
        ///                            HorizontalOptions="Center"
        ///                            VerticalOptions="Center"/>
        ///                     <Label Text = " : "
        ///                            TextColor="Black"
        ///                            FontAttributes="Bold"
        ///                            FontSize="12"
        ///                            HorizontalOptions="Center"
        ///                            VerticalOptions="Center"/>
        ///                     <Label Text = "{Binding Item.YValue}"
        ///                            TextColor="Black"
        ///                            FontAttributes="Bold"
        ///                            FontSize="12"
        ///                            HorizontalOptions="Center"
        ///                            VerticalOptions="Center"/>
        ///                  </StackLayout>
        ///               </DataTemplate>
        ///           </chart:SfCartesianChart.Resources>
        /// 
        ///           <chart:SfCartesianChart.XAxes>
        ///               <chart:NumericalAxis/>
        ///           </chart:SfCartesianChart.XAxes>
        ///
        ///           <chart:SfCartesianChart.YAxes>
        ///               <chart:NumericalAxis/>
        ///           </chart:SfCartesianChart.YAxes>
        ///
        ///           <chart:SfCartesianChart.Series>
        ///               <chart:ColumnSeries ItemsSource="{Binding Data}"
        ///                                   XBindingPath="XValue"
        ///                                   YBindingPath="YValue"
        ///                                   EnableTooltip="True"
        ///                                   TooltipTemplate="{StaticResource tooltipTemplate1}">
        ///               </chart:ColumnSeries> 
        ///           </chart:SfCartesianChart.Series>
        ///           
        ///     </chart:SfCartesianChart>
        /// ]]></code>
        /// ***
        /// </example>
        public DataTemplate TooltipTemplate
        {
            get { return (DataTemplate)GetValue(TooltipTemplateProperty); }
            set { SetValue(TooltipTemplateProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value that indicates to enable the data labels for the series..
        /// </summary>
        /// <value>It accepts bool values and the default value is <c>False</c>.</value>
        /// <example>
        /// # [MainWindow.xaml](#tab/tabid-14)
        /// <code><![CDATA[
        ///     <chart:SfCartesianChart>
        ///
        ///           <chart:SfCartesianChart.XAxes>
        ///               <chart:NumericalAxis/>
        ///           </chart:SfCartesianChart.XAxes>
        ///
        ///           <chart:SfCartesianChart.YAxes>
        ///               <chart:NumericalAxis/>
        ///           </chart:SfCartesianChart.YAxes>
        ///
        ///           <chart:SfCartesianChart.Series>
        ///               <chart:ColumnSeries ItemsSource="{Binding Data}"
        ///                                   XBindingPath="XValue"
        ///                                   YBindingPath="YValue"
        ///                                   ShowDataLabels="True">
        ///               </chart:ColumnSeries> 
        ///           </chart:SfCartesianChart.Series>
        ///           
        ///     </chart:SfCartesianChart>
        /// ]]></code>
        /// # [MainWindow.cs](#tab/tabid-15)
        /// <code><![CDATA[
        ///     SfCartesianChart chart = new SfCartesianChart();
        ///     
        ///     NumericalAxis xAxis = new NumericalAxis();
        ///     NumericalAxis yAxis = new NumericalAxis();
        ///     
        ///     chart.XAxes.Add(xAxis);
        ///     chart.YAxes.Add(yAxis);
        ///     
        ///     ColumnSeries series = new ColumnSeries();
        ///     series.ItemsSource = viewmodel.Data;
        ///     series.XBindingPath = "XValue";
        ///     series.YBindingPath = "YValue";
        ///     series.ShowDataLabels = "True";
        ///     chart.Series.Add(series);
        ///     
        /// ]]></code>
        /// ***
        /// </example>
        public bool ShowDataLabels
        {
            get { return (bool)GetValue(ShowDataLabelsProperty); }
            set { SetValue(ShowDataLabelsProperty, value); }
        }

        /// <summary>
        /// Gets or sets a legend icon that will be displayed in the associated legend item.
        /// </summary>
        /// <value> It accepts <see cref="ChartLegendIconType"/> values and its default value is <see cref="ChartLegendIconType.Circle"/>.</value>
        /// <example>
        /// # [Xaml](#tab/tabid-16)
        /// <code><![CDATA[
        ///     <chart:SfCartesianChart>
        ///
        ///     <!-- ... Eliminated for simplicity-->
        ///     
        ///         <chart:SfCartesianChart.Legend>
        ///            <chart:ChartLegend />
        ///         </chart:SfCartesianChart.Legend>
        ///
        ///          <chart:ColumnSeries ItemsSource="{Binding Data}"
        ///                            XBindingPath="XValue"
        ///                            YBindingPath="YValue"
        ///                            LegendIcon = "Diamond"/>
        ///
        ///     </chart:SfCartesianChart>
        /// ]]></code>
        /// # [C#](#tab/tabid-17)
        /// <code><![CDATA[
        ///     SfCartesianChart chart = new SfCartesianChart();
        ///     chart.Legend = new ChartLegend();
        ///     ViewModel viewModel = new ViewModel();
        ///
        ///     // Eliminated for simplicity
        ///
        ///     ColumnSeries columnSeries = new ColumnSeries()
        ///     {
        ///           ItemsSource = viewModel.Data,
        ///           XBindingPath = "XValue",
        ///           YBindingPath = "YValue",
        ///           LegendIcon = ChartLegendIconType.Diamond,
        ///     };
        ///     
        ///     chart.Series.Add(columnSeries);
        ///
        /// ]]></code>
        /// ***
        /// </example>
        public ChartLegendIconType LegendIcon
        {
            get { return (ChartLegendIconType)GetValue(LegendIconProperty); }
            set { SetValue(LegendIconProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value that indicates whether to show a legend item for this series.
        /// </summary>
        /// <value> It accepts bool values and its default value is <c>True</c>.</value>
        /// <example>
        /// # [Xaml](#tab/tabid-18)
        /// <code><![CDATA[
        ///     <chart:SfCartesianChart>
        ///
        ///     <!-- ... Eliminated for simplicity-->
        ///     
        ///         <chart:SfCartesianChart.Legend>
        ///            <chart:ChartLegend />
        ///         </chart:SfCartesianChart.Legend>
        ///
        ///          <chart:ColumnSeries ItemsSource="{Binding Data}"
        ///                            XBindingPath="XValue"
        ///                            YBindingPath="YValue"
        ///                            IsVisibleOnLegend = "False"/>
        ///
        ///     </chart:SfCartesianChart>
        /// ]]></code>
        /// # [C#](#tab/tabid-19)
        /// <code><![CDATA[
        ///     SfCartesianChart chart = new SfCartesianChart();
        ///     chart.Legend = new ChartLegend();
        ///     ViewModel viewModel = new ViewModel();
        ///
        ///     // Eliminated for simplicity
        ///
        ///     ColumnSeries columnSeries = new ColumnSeries()
        ///     {
        ///           ItemsSource = viewModel.Data,
        ///           XBindingPath = "XValue",
        ///           YBindingPath = "YValue",
        ///           IsVisibleOnLegend = false,
        ///     };
        ///     
        ///     chart.Series.Add(columnSeries);
        ///
        /// ]]></code>
        /// ***
        /// </example>
        public bool IsVisibleOnLegend
        {
            get { return (bool)GetValue(IsVisibleOnLegendProperty); }
            set { SetValue(IsVisibleOnLegendProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value for initiating selection or highlighting of a single or multiple data points in the series.
        /// </summary>
        /// <example>
        /// # [MainPage.xaml](#tab/tabid-20)
        /// <code><![CDATA[
        /// <chart:SfCartesianChart>
        /// 
        ///         <chart:SfCartesianChart.Series>
        ///             <chart:ColumnSeries ItemsSource="{Binding Data}" XBindingPath="XValue" YBindingPath="YValue1">
        ///                 <chart:ColumnSeries.SelectionBehavior>
        ///                     <chart:DataPointSelectionBehavior/>
        ///                 </chart:ColumnSeries.SelectionBehavior>
        ///             </chart:ColumnSeries>
        ///         </chart:SfCartesianChart.Series>
        /// 
        /// </chart:SfCartesianChart>
        /// ]]>
        /// </code>
        /// 
        /// # [MainPage.xaml.cs](#tab/tabid-21)
        /// <code><![CDATA[
        ///  SfCartesianChart chart = new SfCartesianChart();
        ///  
        ///  ViewModel viewModel = new ViewModel();
        ///  chart.BindingContext = viewModel;
        ///  
        ///  DataPointSelectionBehavior behavior = new DataPointSelectionBehavior();
        ///  
        ///  ColumnSeries series = new ColumnSeries()
        ///  {
        ///      ItemsSource = viewmodel.Data,
        ///      XBindingPath = "XValue",
        ///      YBindingPath = "YValue1",
        ///      SelectionBehavior = behavior
        ///  };
        ///  
        ///  chart.Series.Add(series);
        /// ]]>
        /// </code>
        /// *** 
        /// </example>
        /// <value>The default value is null.</value>
        public DataPointSelectionBehavior? SelectionBehavior
        {
            get { return (DataPointSelectionBehavior)GetValue(SelectionBehaviorProperty); }
            set { SetValue(SelectionBehaviorProperty, value); }
        }

        /// <summary>
        /// Gets the XRange values.
        /// </summary>
        public DoubleRange XRange { get; internal set; } = DoubleRange.Empty;

        /// <summary>
        /// Gets the YRange values.
        /// </summary>
        public DoubleRange YRange { get; internal set; } = DoubleRange.Empty;

        /// <summary>
        /// 
        /// </summary>
        internal double AnimationDuration
        {
            get { return (double)GetValue(AnimationDurationProperty); }
            set { SetValue(AnimationDurationProperty, value); }
        }
        #endregion

        #region Internal Virtual Properties

        internal virtual ChartDataLabelSettings? ChartDataLabelSettings
        {
            get
            {
                return null;
            }
        }

        #endregion

        #region Internal Properties

        private IChart? chart;

        internal readonly float DefaultSelectionStrokeWidth = 5;//Todo: check necessary this default value

        internal IChart? Chart
        {
            get
            {
                return chart;
            }
            set
            {
                if (chart != null)
                {
                    if (value == null)
                    {
                        chart = value;
                    }
                    else
                    {
                        //Todo:Need to get content review below exception message.
                        throw new ArgumentException("ChartSeries is already the child of another Chart.");
                    }
                }
                else
                {
                    chart = value;

                    if (chart != null)
                        chart.DataManager.UpdateDataManager(this, null, ItemsSource, chart.IsDataPopulated);
                }
            }
        }

        internal Rect AreaBounds => Chart != null ? Chart.ActualSeriesClipRect : Rect.Zero;

        internal float AnimationValue { get; set; } = 1;

        internal Animation? SeriesAnimation { get; set; }

        internal bool NeedToAnimateSeries { get; set; }

        internal bool NeedToAnimateDataLabel { get; set; }

        internal bool SegmentsCreated { get; set; }

        internal IList<Brush>? PaletteColors { get; set; }

        internal DoubleRange VisibleXRange { get; set; }

        internal DoubleRange VisibleYRange { get; set; }

        internal ObservableCollection<ChartSegment>? OldSegments { get; set; }

        internal DoubleRange PreviousXRange { get; set; }

        internal int TooltipDataPointIndex { get; set; }

        internal List<double> GroupedXValuesIndexes { get; set; } = new List<double>();

        internal List<object> GroupedActualData { get; set; } = new List<object>();

        internal List<string> GroupedXValues { get; set; } = new List<string>();

        #endregion

        #region Methods

        #region Public Methods

        /// <summary>
        /// Methods to get the index of data point.
        /// </summary>
        public virtual int GetDataPointIndex(float pointX, float pointY)
        {
            int selectedDataPointIndex = -1;
            RectF seriesBounds = AreaBounds;

            for (int i = 0; i < Segments.Count; i++)
            {
                ChartSegment chartSegment = Segments[i];
                selectedDataPointIndex = chartSegment.GetDataPointIndex(pointX - seriesBounds.Left, pointY - seriesBounds.Top);
                if (selectedDataPointIndex >= 0)
                {
                    return selectedDataPointIndex;
                }
            }

            return selectedDataPointIndex;
        }

        #endregion

        #region DataLabels methods

        internal virtual void DrawDataLabels(ICanvas canvas)
        {
            var dataLabelSettings = ChartDataLabelSettings;

            if (dataLabelSettings == null) return;

            ChartDataLabelStyle labelStyle = dataLabelSettings.LabelStyle;

            foreach (ChartSegment datalabel in Segments)
            {
                if (!datalabel.InVisibleRange || datalabel.IsEmpty) continue;
                UpdateDataLabelAppearance(canvas, datalabel, dataLabelSettings, labelStyle);
            }
        }

        internal void UpdateDataLabelAppearance(ICanvas canvas, ChartSegment datalabel, ChartDataLabelSettings dataLabelSettings, ChartDataLabelStyle labelStyle)
        {
            if (labelStyle.Angle != 0)
            {
                float angle = (float)(labelStyle.Angle > 360 ? labelStyle.Angle % 360 : labelStyle.Angle);
                canvas.SaveState();
                canvas.Rotate(angle, datalabel.LabelPositionPoint.X, datalabel.LabelPositionPoint.Y);
            }

            //Setting label background properties.
            //Todo:// Need to confirm colors
            canvas.StrokeSize = (float)labelStyle.StrokeWidth;
            canvas.StrokeColor = labelStyle.Stroke.ToColor();

            //Setting label properties.
            var fillcolor = labelStyle.IsBackgroundColorUpdated ? labelStyle.Background : dataLabelSettings.UseSeriesPalette ? datalabel.Fill : labelStyle.Background;
            DrawDataLabel(canvas, fillcolor, datalabel.DataLabel != null ? datalabel.DataLabel : string.Empty, datalabel.LabelPositionPoint, datalabel.Index);
        }

        internal virtual PointF GetDataLabelPosition(ChartSegment dataLabel, SizeF labelSize, PointF labelPosition, float padding)
        {
            var dataLabelSettings = ChartDataLabelSettings;

            if (dataLabelSettings == null) return labelPosition;

            if (dataLabelSettings.LabelPlacement == DataLabelPlacement.Outer)
            {
                labelPosition.Y = labelPosition.Y - (labelSize.Height / 2) - padding;
            }
            else if (dataLabelSettings.LabelPlacement == DataLabelPlacement.Inner || (dataLabelSettings.LabelPlacement == DataLabelPlacement.Auto && this is ColumnSeries))
            {
                labelPosition.Y = labelPosition.Y + (labelSize.Height / 2) + padding;
            }

            return labelPosition;
        }

        internal void OnDataLabelSettingsPropertyChanged(ChartDataLabelSettings? oldValue, ChartDataLabelSettings? newValue)
        {
            if (oldValue != null)
            {
                oldValue.PropertyChanged -= DataLabelSettings_PropertyChanged;

                if (oldValue.LabelStyle != null)
                    oldValue.LabelStyle.PropertyChanged -= LabelStyle_PropertyChanged;
            }

            if (newValue != null)
            {
                newValue.PropertyChanged += DataLabelSettings_PropertyChanged;
                SetInheritedBindingContext(newValue, BindingContext);

                if (newValue.LabelStyle != null)
                {
                    newValue.LabelStyle.PropertyChanged += LabelStyle_PropertyChanged;
                    SetInheritedBindingContext(newValue.LabelStyle, BindingContext);
                }
            }

            if (AreaBounds != Rect.Zero)
            {
                InvalidateMeasureDataLabel();
                InvalidateDataLabel();
            }
        }

        internal void DataLabelSettings_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            var dataLabelSettings = sender as ChartDataLabelSettings;

            if (dataLabelSettings == null) return;

            if (e.PropertyName != null && dataLabelSettings.IsNeedDataLabelMeasure.Contains(e.PropertyName))
            {
                InvalidateMeasureDataLabel();

                if (e.PropertyName == nameof(dataLabelSettings.LabelStyle))
                {
                    dataLabelSettings.LabelStyle.PropertyChanged += LabelStyle_PropertyChanged;
                }
            }

            InvalidateDataLabel();
        }

        internal void UnhookDataLabelEvents(ChartDataLabelSettings dataLabelSettings)
        {
            if (dataLabelSettings != null)
            {
                if (dataLabelSettings.LabelStyle != null)
                {
                    dataLabelSettings.LabelStyle.PropertyChanged -= LabelStyle_PropertyChanged;
                }

                dataLabelSettings.PropertyChanged -= DataLabelSettings_PropertyChanged;
            }
        }

        internal void InvalidateMeasureDataLabel()
        {
            foreach (var segment in Segments)
            {
                segment.OnDataLabelLayout();
            }
        }

        internal void InvalidateDataLabel()
        {
            var chartPlotArea = chart?.Area.PlotArea as ChartPlotArea;

            if (chartPlotArea != null)
            {
                chartPlotArea.DataLabelView?.InvalidateDrawable();
            }
        }

        #endregion

        #region Internal Virtual Methods

        internal virtual float TransformToVisibleX(double x, double y) => 0f;

        internal virtual float TransformToVisibleY(double x, double y) => 0f;

        internal virtual bool IsIndividualSegment()
        {
            return true;
        }

        internal virtual void OnSeriesLayout()
        {

        }

        internal virtual void UpdateLegendIconColor()
        {

        }

        internal virtual void UpdateLegendIconColor(int index)
        {

        }

        internal virtual void SetStrokeColor(ChartSegment segment)
        {
        }

        internal virtual void SetStrokeWidth(ChartSegment segment)
        {
        }

        internal virtual void SetDashArray(ChartSegment segment)
        {
        }

        internal virtual Brush? GetFillColor(int index)
        {
            Brush? fillColor = null;

            //Chart selection check. 
            fillColor = Chart?.GetSelectionBrush(this);

            //Series selection behavior check.
            if (fillColor == null)
                fillColor = GetSelectionBrush(index);

            if (fillColor == null)
            {
                if (Fill != null)
                {
                    fillColor = Fill;
                }
                else if (PaletteColors != null)
                {
                    fillColor = PaletteColors.Count > 0 ? PaletteColors[index % PaletteColors.Count] : new SolidColorBrush(Colors.Transparent);
                }
            }

            return fillColor;
        }

        internal virtual void UpdateSelectedItem(int index)
        {
            //Update selected segment.
            //While selection no need to update legend items for cartesian seires. So created override to circular alone.
            SetFillColor(Segments[index]);
        }

        internal virtual bool SeriesContainsPoint(PointF point)
        {
            if (PointsCount == 0)
            {
                return false;
            }

            TooltipDataPointIndex = GetDataPointIndex(point.X, point.Y);
            return TooltipDataPointIndex < PointsCount && TooltipDataPointIndex > -1;
        }

        internal virtual void Dispose()
        {
            HookAndUnhookCollectionChangedEvent(ItemsSource, null);

            if (SeriesAnimation != null)
            {
                SeriesAnimation = null;
            }

            var customBrushes = PaletteBrushes as ObservableCollection<Brush>;

            if (customBrushes != null)
            {
                customBrushes.CollectionChanged -= CustomBrushes_CollectionChanged;
            }
        }

        internal virtual TooltipInfo? GetTooltipInfo(ChartTooltipBehavior tooltipBehavior, float x, float y)
        {
            return null;
        }

        internal virtual void SetTooltipTargetRect(TooltipInfo tooltipInfo, Rect seriesBounds)
        {
            float xPosition = tooltipInfo.X;
            float yPosition = tooltipInfo.Y;
            float sizeValue = 1;
            float halfSizeValue = 0.5f;
            Rect targetRect = new Rect(xPosition - halfSizeValue, yPosition - halfSizeValue, sizeValue, sizeValue);
            tooltipInfo.TargetRect = targetRect;
        }

        internal virtual void GenerateSegments(SeriesView seriesView)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected abstract ChartSegment? CreateSegment();

        #endregion

        #region Internal Methods

        internal void UpdateColor()
        {
            foreach (var segment in Segments)
            {
                SetFillColor(segment);
            }
        }

        internal void UpdateAlpha()
        {
            foreach (var segment in Segments)
            {
                segment.Opacity = (float)Opacity;
            }
        }

        internal void UpdateStrokeColor()
        {
            foreach (var segment in Segments)
            {
                SetStrokeColor(segment);
            }
        }

        internal void UpdateStrokeWidth()
        {
            foreach (var segment in Segments)
            {
                SetStrokeWidth(segment);
            }
        }

        internal void UpdateDashArray()
        {
            foreach (var segment in Segments)
            {
                SetDashArray(segment);
            }
        }

        internal void AnimateSeries(Action<double> callback)
        {
            if (SeriesAnimation == null) return;

            Animation? customAnimation = CreateAnimation(callback);

            if (customAnimation != null)
                SeriesAnimation.Add(0, 1, customAnimation);
        }

        internal bool CanAnimate()
        {
            return EnableAnimation && NeedToAnimateSeries;
        }

        internal void AddDefaultBehaviors(IChart chart)
        {
            if (chart == null) return;

        }

        internal void InvalidateSeries()
        {
            var plotArea = Chart?.Area.PlotArea as ChartPlotArea;

            if (plotArea != null)
            {
                foreach (SeriesView seriesView in plotArea.SeriesViews.Children)
                {
                    if (seriesView != null && this == seriesView.Series)
                    {
                        seriesView.InvalidateDrawable();
                        break;
                    }
                }
            }
        }

        internal void Invalidate()
        {
            var plotArea = Chart?.Area.PlotArea as ChartPlotArea;

            if (plotArea != null)
            {
                foreach (SeriesView seriesView in plotArea.SeriesViews.Children)
                {
                    if (seriesView != null && this == seriesView.Series)
                    {
                        seriesView.Invalidate();
                        break;
                    }
                }
            }
        }

        internal void ScheduleUpdateChart()
        {
            var area = chart?.Area;
            if (area != null)
            {
                area.NeedsRelayout = true;
                area.ScheduleUpdateArea();
            }
        }

        internal void SetFillColor(ChartSegment segment)
        {
            if (segment == null)
            {
                return;
            }

            segment.Fill = GetFillColor(segment.Index) ?? Brush.Transparent;
        }

        internal Brush? GetSelectionBrush(int index)
        {
            if (SelectionBehavior != null)
            {
                return SelectionBehavior.GetSelectionBrush(index);
            }

            return null;
        }

        internal bool SelectionHitTest(float x, float y)
        {
            if (SelectionBehavior != null && SelectionBehavior.Type != 0)
            {
                return SelectionBehavior.OnTapped(x, y);
            }

            return false;
        }

        /// <summary>
        ///
        /// </summary>
        internal bool HitTest(float pointX, float pointY)
        {
            return GetDataPointIndex(pointX, pointY) >= 0;
        }

        void IDatapointSelectionDependent.SetFillColor(ChartSegment segment) => SetFillColor(segment);

        #endregion

        #region Protected Methods

        /// <summary>
        ///
        /// </summary>
        protected virtual Animation? CreateAnimation(Action<double> callback)
        {
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        internal void Animate()
        {
            if (EnableAnimation && Segments != null && Segments.Count > 0)
            {
                SeriesView? seriesView = Segments[0].SeriesView;

                if (seriesView != null)
                    seriesView.Animate();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        protected internal virtual void DrawDataLabel(ICanvas canvas, Brush? fillcolor, string label, PointF point, int index)
        {
            ChartDataLabelStyle? dataLabelStyle = ChartDataLabelSettings?.LabelStyle;
            var datalabel = Segments[index];

            if (!string.IsNullOrEmpty(label) && dataLabelStyle != null && ChartDataLabelSettings != null && datalabel != null)
            {
                dataLabelStyle.DrawBackground(canvas, label, fillcolor, point);

                ChartLabelStyle style = dataLabelStyle;
                Color fontColor = dataLabelStyle.TextColor;
                if (fontColor == null || fontColor == Colors.Transparent)
                {
                    fontColor = ChartDataLabelSettings.GetContrastTextColor(this, fillcolor, datalabel.Fill);
                    var textColor = fontColor.WithAlpha(NeedToAnimateDataLabel ? AnimationValue : 1);

                    //Created new font family, as need to pass contrast text color for native font family rendering.
                    style = dataLabelStyle.Clone();
                    style.TextColor = textColor;
                }

                style.DrawLabel(canvas, label, point);
            }

            if (dataLabelStyle?.Angle != 0)
            {
                canvas.RestoreState();
            }
        }

        internal virtual void OnAttachedToChart()
        {

        }

        internal virtual void OnDetachedToChart()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        protected internal virtual void DrawSeries(ICanvas canvas, ReadOnlyObservableCollection<ChartSegment> segments, RectF clipRect)
        {
            //TODO: Faced issue while having animation in WinUI.
#if WINDOWS
            foreach (var segment in segments)
            {
                canvas.SaveState();
                segment.Draw(canvas);
                canvas.RestoreState();
            }
#else
            canvas.SaveState();
            foreach (var segment in segments)
            {
                segment.Draw(canvas);
            }

            canvas.RestoreState();
#endif
        }

        /// <summary>
        /// 
        /// </summary>
        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
            if (SelectionBehavior != null)
            {
                SetInheritedBindingContext(SelectionBehavior, BindingContext);
            }
        }

        #endregion

        #region Private Static Methods

        private static void OnItemsSourceChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var chartSeries = bindable as ChartSeries;
            if (chartSeries != null)
            {
                chartSeries.OnItemsSourceChanged(oldValue, newValue);
            }
        }

        private static void OnXBindingPathChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var chartSeries = bindable as ChartSeries;
            if (chartSeries != null)
            {
                if (newValue != null && newValue is string)
                {
                    chartSeries.XComplexPaths = ((string)newValue).Split(new char[] { '.' });
                }

                if (chartSeries.Chart != null)
                {
                    chartSeries.Chart.DataManager.OnBindingPathChanged(chartSeries, (string)oldValue, newValue, chartSeries.Chart.IsDataPopulated, true);
                }

                chartSeries.OnBindingPathChanged();
            }
        }

        private static void OnFillPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var chartSeries = bindable as ChartSeries;

            if (chartSeries != null)
            {
                chartSeries.UpdateColor();
                chartSeries.InvalidateSeries();
                if (chartSeries.ShowDataLabels)
                {
                    chartSeries.InvalidateDataLabel();
                }
                chartSeries.UpdateLegendIconColor();
            }
        }

        private static void OnOpacityPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var chartSeries = bindable as ChartSeries;

            if (chartSeries != null)
            {
                chartSeries.UpdateAlpha();
                chartSeries.InvalidateSeries();
            }
        }

        private static void OnPaletteBrushesChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (Equals(oldValue, newValue))
            {
                return;
            }

            var chartSeries = bindable as ChartSeries;

            if (chartSeries != null)
            {
                chartSeries.PaletteColors = (IList<Brush>)newValue;

                chartSeries.OnCustomBrushesChanged(oldValue as ObservableCollection<Brush>, newValue as ObservableCollection<Brush>);

                if (chartSeries.AreaBounds != Rect.Zero)//Not to call at load time
                {
                    chartSeries.PaletteColorsChanged();
                }
            }
        }

        private static void OnVisiblePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var chartSeries = bindable as ChartSeries;

            //TODO:Need to move this code to CartesianSeries class.
            if (chartSeries != null)
            {
                chartSeries.InvalidateGroupValues();

                if (chartSeries.IsSideBySide && chartSeries.chart?.Area is CartesianChartArea chartArea)
                {
                    chartArea.ResetSBSSegments();
                }
            }

            chartSeries?.ScheduleUpdateChart();
        }

        private static void OnEnableTooltipPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
        }

        private static void OnEnableAnimationPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var chartSeries = bindable as ChartSeries;

            if (chartSeries != null)
            {
                chartSeries.OnAnimationPropertyChanged();
            }
        }

        private static void OnSelectionBehaviorPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is ChartSeries series)
            {
                if (newValue is DataPointSelectionBehavior selection)
                {
                    selection.Source = series;
                    SetInheritedBindingContext(selection, series.BindingContext);                    
                }
            }
        }

        private static void OnShowDataLabelsChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var chartSeries = bindable as ChartSeries;

            if (chartSeries != null && chartSeries.Chart != null)
            {
                if ((bool)newValue)
                {
                    chartSeries.InvalidateMeasureDataLabel();
                }

                chartSeries.InvalidateDataLabel();
            }
        }

        private static void OnLegendIconChanged(BindableObject bindable, object oldValue, object newValue)
        {
        }

        private static void OnListenPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is ChartSeries series && series.chart != null)
            {
                series.chart.DataManager.OnListenPropertyChanged(series, (bool)newValue);
            }
        }

        #endregion

        #region Private Methods

        private void OnItemsSourceChanged(object oldValue, object newValue)
        {
            if (Equals(oldValue, newValue))
            {
                return;
            }

            if (EnableAnimation && AnimationDuration > 0 && Segments != null && Segments.Count > 0)
            {
                OldSegments = new ObservableCollection<ChartSegment>(Segments);
                PreviousXRange = XRange;
                Segments[0].SeriesView?.AbortAnimation();
            }

            UpdateLegendItems();
            NeedToAnimateSeries = EnableAnimation;

            Chart?.DataManager?.UpdateDataManager(this, oldValue, newValue, Chart.IsDataPopulated);
            
            OnDataSourceChanged(oldValue, newValue);
            SegmentsCreated = false;
            ScheduleUpdateChart();
        }

        private void Segments_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            e.ApplyCollectionChanges((index, obj) => AddSegment(obj), (index, obj) => RemoveSegment(obj), ResetSegment);
        }

        private void AddSegment(object chartSegment)
        {
            var segment = chartSegment as ChartSegment;

            if (segment != null)
            {
                SetFillColor(segment);
                SetStrokeColor(segment);
                SetStrokeWidth(segment);
                SetDashArray(segment);
                segment.Opacity = (float)Opacity;
            }
        }

        private void RemoveSegment(object chartSegment)
        {
            //Todo: Need to consider this case later.
        }

        private void ResetSegment()
        {
            //Todo: Need to consider this case later.
        }

        private void OnAnimationPropertyChanged()
        {
            //Todo: Need to move this code in series propertychanged event.
            //if (EnableAnimation && SeriesAnimation == null)
            //{
            //    SeriesAnimation = new Animation(OnAnimationStart);
            //}
            //else if (!EnableAnimation && SeriesAnimation != null)
            //{
            //    AbortAnimation();
            //}
        }

        private void LabelStyle_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            var labelStyle = sender as ChartDataLabelStyle;

            if (e.PropertyName != null && labelStyle != null && labelStyle.NeedDataLabelMeasure(e.PropertyName))
            {
                InvalidateMeasureDataLabel();
            }

            InvalidateDataLabel();
        }

        private void UpdateLegendItems()
        {
            var area = chart?.Area;
            if (area != null && !area.AreaBounds.IsEmpty)
            {
                area.PlotArea.ShouldPopulateLegendItems = true;
            }
        }

        private void PaletteColorsChanged()
        {
            UpdateColor();
            InvalidateSeries();

            if (ShowDataLabels)
            {
                InvalidateDataLabel();
            }

            UpdateLegendIconColor();
        }

        private void OnCustomBrushesChanged(ObservableCollection<Brush>? oldValue, ObservableCollection<Brush>? newValue)
        {
            if (oldValue != null)
            {
                oldValue.CollectionChanged -= CustomBrushes_CollectionChanged;
            }

            if (newValue != null)
            {
                newValue.CollectionChanged += CustomBrushes_CollectionChanged;
            }
        }

        private void CustomBrushes_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            if (sender != null)
            {
                PaletteColorsChanged();
            }
        }

        #endregion

        #endregion

        #region Interface implementation

        ObservableCollection<ChartSegment> IDatapointSelectionDependent.Segments => Segments;

        Rect IDatapointSelectionDependent.AreaBounds => AreaBounds;

        void IDatapointSelectionDependent.Invalidate()
        {
            InvalidateSeries();
            if (ShowDataLabels)
                InvalidateDataLabel();
        }

        void IDatapointSelectionDependent.UpdateSelectedItem(int index)
        {
            UpdateSelectedItem(index);
        }

        void ITooltipDependent.SetTooltipTargetRect(TooltipInfo tooltipInfo, Rect seriesBounds)
        {
            SetTooltipTargetRect(tooltipInfo, seriesBounds);
        }

        DataTemplate? ITooltipDependent.GetDefaultTooltipTemplate(TooltipInfo info)
        {
            return GetDefaultTooltipTemplate(info);
        }

        internal virtual DataTemplate? GetDefaultTooltipTemplate(TooltipInfo info)
        {
            return ChartUtils.GetDefaultTooltipTemplate(info);
        }

        #endregion
    }
}