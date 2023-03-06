using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using System.Collections;
using System.Collections.ObjectModel;

namespace Syncfusion.UI.Xaml.Charts
{
    /// <summary>
    /// Represents the legend for the <see cref="SfCartesianChart"/>, <see cref="SfCircularChart"/>, <see cref="SfPolarChart"/>, <see cref="SfFunnelChart"/>, and <see cref="SfPyramidChart"/> classes.
    /// </summary>
    /// <remarks>
    /// The items in the legend contain the key information about the <see cref="ChartSeries"/>. The legend has all abilities such as docking, enabling, or disabling the desired series.
    ///</remarks>
    /// <example>
    /// # [MainPage.xaml](#tab/tabid-1)
    /// <code><![CDATA[
    /// <chart:SfCircularChart>
    ///
    ///     <chart:SfCircularChart.Legend>
    ///        <chart:ChartLegend CheckBoxVisibility="Visible"/>
    ///     </chart:SfCircularChart.Legend>
    ///
    ///     <chart:PieSeries ItemsSource="{Binding Data}"
    ///                      XBindingPath="XValue"
    ///                      YBindingPath="YValue"
    ///                      Label="PieSeries"/>
    ///     
    /// </chart:SfCircularChart>
    ///
    /// ]]>
    /// </code>
    /// # [MainPage.xaml.cs](#tab/tabid-2)
    /// <code><![CDATA[
    /// SfCircularChart chart = new SfCircularChart();
    /// ViewModel viewModel = new ViewModel();
    ///
    /// chart.Legend = new ChartLegend();
    /// 
    /// PieSeries series = new PieSeries()
    /// {
    ///    ItemsSource = viewModel.Data,
    ///    XBindingPath = "XValue",
    ///    YBindingPath = "YValue",
    ///    Label = "PieSeries",
    /// };
    /// chart.Series.Add(series);
    ///
    /// ]]>
    /// </code>
    /// ***
    /// </example>
    public class ChartLegend : DependencyObject, ILegend
    {
        #region Dependency properties

        /// <summary>
        /// The DependencyProperty for <see cref="Placement"/> property.
        /// </summary>
        public static readonly DependencyProperty PlacementProperty =
            DependencyProperty.Register(nameof(Placement), typeof(LegendPlacement), typeof(ChartLegend), new PropertyMetadata(LegendPlacement.Top));

        /// <summary>
        /// The DependencyProperty for <see cref="ToggleSeriesVisibility"/> property.
        /// </summary>
        public static readonly DependencyProperty ToggleSeriesVisibilityProperty =
            DependencyProperty.Register(nameof(ToggleSeriesVisibility), typeof(bool), typeof(ChartLegend), new PropertyMetadata(false));

        /// <summary>
        /// The DependencyProperty for <see cref="IsVisible"/> property.
        /// </summary>
        public static readonly DependencyProperty IsVisibleProperty = 
            DependencyProperty.Register(nameof(IsVisible), typeof(bool), typeof(ChartLegend), new PropertyMetadata(true));

        /// <summary>
        /// The DependencyProperty for <see cref="Header"/> property.
        /// </summary>
        public static readonly DependencyProperty HeaderProperty =
            DependencyProperty.Register(nameof(Header), typeof(object), typeof(ChartLegend), null);

        /// <summary>
        /// The DependencyProperty for <see cref="HeaderTemplate"/> property.
        /// </summary>
        public static readonly DependencyProperty HeaderTemplateProperty =
            DependencyProperty.Register(nameof(HeaderTemplate), typeof(DataTemplate), typeof(ChartLegend), null);

        /// <summary>
        /// The DependencyProperty for <see cref="CornerRadius"/> property.
        /// </summary>
        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register(nameof(CornerRadius), typeof(CornerRadius), typeof(ChartLegend),
                new PropertyMetadata(new CornerRadius(0)));

        /// <summary>
        /// Identifies the CheckBoxVisibility dependency property.
        /// The DependencyProperty for <see cref="CheckBoxVisibility"/> property.
        /// </summary>
        public static readonly DependencyProperty CheckBoxVisibilityProperty =
          DependencyProperty.Register(nameof(CheckBoxVisibility), typeof(Visibility), typeof(ChartLegend),
              new PropertyMetadata(Visibility.Collapsed));

        /// <summary>
        /// Identifies the IconVisibility dependency property.
        /// The DependencyProperty for <see cref="IconVisibility"/> property.
        /// </summary>
        public static readonly DependencyProperty IconVisibilityProperty =
          DependencyProperty.Register(nameof(IconVisibility), typeof(Visibility), typeof(ChartLegend),
              new PropertyMetadata(Visibility.Visible));

        /// <summary>
        /// Identifies the IconWidth dependency property.
        /// The DependencyProperty for <see cref="IconWidth"/> property.
        /// </summary>
        public static readonly DependencyProperty IconWidthProperty =
          DependencyProperty.Register(nameof(IconWidth), typeof(double), typeof(ChartLegend),
         new PropertyMetadata(12d));

        /// <summary>
        /// Identifies the IconHeight dependency property.
        /// The DependencyProperty for <see cref="IconHeight"/> property.
        /// </summary>
        public static readonly DependencyProperty IconHeightProperty =
          DependencyProperty.Register(nameof(IconHeight), typeof(double), typeof(ChartLegend),
        new PropertyMetadata(12d));

        /// <summary>
        /// Identifies the ItemMargin dependency property.
        /// The DependencyProperty for <see cref="ItemMargin"/> property.
        /// </summary>
        public static readonly DependencyProperty ItemMarginProperty =
          DependencyProperty.Register(nameof(ItemMargin), typeof(Thickness), typeof(ChartLegend),
              new PropertyMetadata(new Thickness(0, 0, 0, 0)));

        /// <summary>
        /// Identifies the Background dependency property.
        /// The DependencyProperty for <see cref="Background"/> property.
        /// </summary>
        public static readonly DependencyProperty BackgroundProperty =
          DependencyProperty.Register(nameof(Background), typeof(Brush), typeof(ChartLegend), new PropertyMetadata(null));

        /// <summary>
        /// Identifies the BorderBrush dependency property.
        /// The DependencyProperty for <see cref="BorderBrush"/> property.
        /// </summary>
        public static readonly DependencyProperty BorderBrushProperty =
          DependencyProperty.Register(nameof(BorderBrush), typeof(Brush), typeof(ChartLegend), new PropertyMetadata(null));

        /// <summary>
        /// Identifies the BorderThickness dependency property.
        /// The DependencyProperty for <see cref="BorderThickness"/> property.
        /// </summary>
        public static readonly DependencyProperty BorderThicknessProperty =
          DependencyProperty.Register(nameof(BorderThickness), typeof(Thickness), typeof(ChartLegend), new PropertyMetadata(new Thickness(0)));

        /// <summary>
        /// Identifies the Padding dependency property.
        /// The DependencyProperty for <see cref="Padding"/> property.
        /// </summary>
        public static readonly DependencyProperty PaddingProperty =
          DependencyProperty.Register(nameof(Padding), typeof(Thickness), typeof(ChartLegend), new PropertyMetadata(new Thickness(0)));

        /// <summary>
        /// Identifies the HorizontalHeaderAlignment dependency property.
        /// The DependencyProperty for <see cref="HorizontalHeaderAlignment"/> property.
        /// </summary>
        public static readonly DependencyProperty HorizontalHeaderAlignmentProperty =
          DependencyProperty.Register(nameof(HorizontalHeaderAlignment), typeof(HorizontalAlignment), typeof(ChartLegend), new PropertyMetadata(HorizontalAlignment.Center));

        /// <summary>
        /// Identifies the ItemTemplate dependency property.
        /// The DependencyProperty for <see cref="ItemTemplate"/> property.
        /// </summary>
        public static readonly DependencyProperty ItemTemplateProperty =
          DependencyProperty.Register(nameof(ItemTemplate), typeof(DataTemplate), typeof(ChartLegend), new PropertyMetadata(null));

        /// <summary>
        /// Identifies the ItemsSource dependency property.
        /// The DependencyProperty for <see cref="ItemsSource"/> property.
        /// </summary>
        internal static readonly DependencyProperty ItemsSourceProperty =
          DependencyProperty.Register(nameof(ItemsSource), typeof(object), typeof(ChartLegend),
              new PropertyMetadata(null));
        #endregion


        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="ChartLegend"/> class.
        /// </summary>
        public ChartLegend()
        {
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the placement for the legend in a chart.
        /// </summary>
        /// <remarks>Legends can be placed left, right, top, or bottom around the chart area.</remarks>
        /// <value>It accepts <see cref="LegendPlacement"/> values and the default value is <see cref="LegendPlacement.Top"/>.</value>
        /// <example>
        /// # [MainPage.xaml](#tab/tabid-3)
        /// <code><![CDATA[
        /// <chart:SfCircularChart>
        ///
        ///     <chart:SfCircularChart.Legend>
        ///        <chart:ChartLegend Placement = "Right"/>
        ///     </chart:SfCircularChart.Legend>
        ///
        ///     <chart:PieSeries ItemsSource="{Binding Data}"
        ///                      XBindingPath="XValue"
        ///                      YBindingPath="YValue"/>
        ///     
        /// </chart:SfCircularChart>
        ///
        /// ]]>
        /// </code>
        /// # [MainPage.xaml.cs](#tab/tabid-4)
        /// <code><![CDATA[
        /// SfCircularChart chart = new SfCircularChart();
        /// ViewModel viewModel = new ViewModel();
        ///
        /// chart.Legend = new ChartLegend(){ Placement = LegendPlacement.Right };
        /// 
        /// PieSeries series = new PieSeries()
        /// {
        ///    ItemsSource = viewModel.Data,
        ///    XBindingPath = "XValue",
        ///    YBindingPath = "YValue",
        /// };
        /// chart.Series.Add(series);
        ///
        /// ]]>
        /// </code>
        /// ***
        /// </example>
        public LegendPlacement Placement
        {
            get { return (LegendPlacement)GetValue(PlacementProperty); }
            set { SetValue(PlacementProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the chart series visibility by tapping the legend item.
        /// </summary>
        /// <value>It accepts bool values and the default value is <c>True</c></value>
        /// <example>
        /// # [MainPage.xaml](#tab/tabid-5)
        /// <code><![CDATA[
        /// <chart:SfCircularChart>
        ///
        ///     <chart:SfCircularChart.Legend>
        ///        <chart:ChartLegend ToggleSeriesVisibility = "True"/>
        ///     </chart:SfCircularChart.Legend>
        ///
        ///     <chart:PieSeries ItemsSource="{Binding Data}"
        ///                      XBindingPath="XValue"
        ///                      YBindingPath="YValue"/>
        ///     
        /// </chart:SfCircularChart>
        ///
        /// ]]>
        /// </code>
        /// # [MainPage.xaml.cs](#tab/tabid-6)
        /// <code><![CDATA[
        /// SfCircularChart chart = new SfCircularChart();
        /// ViewModel viewModel = new ViewModel();
        ///
        /// chart.Legend = new ChartLegend(){ ToggleSeriesVisibility = true };
        /// 
        /// PieSeries series = new PieSeries()
        /// {
        ///    ItemsSource = viewModel.Data,
        ///    XBindingPath = "XValue",
        ///    YBindingPath = "YValue",
        /// };
        /// chart.Series.Add(series);
        ///
        /// ]]>
        /// </code>
        /// ***
        /// </example>
        public bool ToggleSeriesVisibility
        {
            get { return (bool)GetValue(ToggleSeriesVisibilityProperty); }
            set { SetValue(ToggleSeriesVisibilityProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value that indicates whether the legend is visible or not.
        /// </summary>
        /// <value>It accepts bool values and the default value is <c>True</c></value>
        /// <example>
        /// # [MainPage.xaml](#tab/tabid-7)
        /// <code><![CDATA[
        /// <chart:SfCircularChart>
        ///
        ///     <chart:SfCircularChart.Legend>
        ///        <chart:ChartLegend IsVisible = "True"/>
        ///     </chart:SfCircularChart.Legend>
        ///
        ///     <chart:PieSeries ItemsSource="{Binding Data}"
        ///                      XBindingPath="XValue"
        ///                      YBindingPath="YValue"/>
        ///     
        /// </chart:SfCircularChart>
        ///
        /// ]]>
        /// </code>
        /// # [MainPage.xaml.cs](#tab/tabid-8)
        /// <code><![CDATA[
        /// SfCircularChart chart = new SfCircularChart();
        /// ViewModel viewModel = new ViewModel();
        ///
        /// chart.Legend = new ChartLegend(){ IsVisible = true };
        /// 
        /// PieSeries series = new PieSeries()
        /// {
        ///    ItemsSource = viewModel.Data,
        ///    XBindingPath = "XValue",
        ///    YBindingPath = "YValue",
        /// };
        /// chart.Series.Add(series);
        ///
        /// ]]>
        /// </code>
        /// ***
        /// </example>
        public bool IsVisible
        {
            get { return (bool)GetValue(IsVisibleProperty); }
            set { SetValue(IsVisibleProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value for the header of the legend.
        /// </summary>
        /// <example>
        /// # [MainPage.xaml](#tab/tabid-9)
        /// <code><![CDATA[
        /// <chart:SfCircularChart>
        ///
        ///     <chart:SfCircularChart.Legend>
        ///        <chart:ChartLegend Header = "Data analysis"/>
        ///     </chart:SfCircularChart.Legend>
        ///
        ///     <chart:PieSeries ItemsSource="{Binding Data}"
        ///                      XBindingPath="XValue"
        ///                      YBindingPath="YValue"/>
        ///     
        /// </chart:SfCircularChart>
        ///
        /// ]]>
        /// </code>
        /// # [MainPage.xaml.cs](#tab/tabid-10)
        /// <code><![CDATA[
        /// SfCircularChart chart = new SfCircularChart();
        /// ViewModel viewModel = new ViewModel();
        ///
        /// chart.Legend = new ChartLegend(){ Header = "Data analysis" };
        /// 
        /// PieSeries series = new PieSeries()
        /// {
        ///    ItemsSource = viewModel.Data,
        ///    XBindingPath = "XValue",
        ///    YBindingPath = "YValue",
        /// };
        /// chart.Series.Add(series);
        ///
        /// ]]>
        /// </code>
        /// ***
        /// </example>
        public object Header
        {
            get { return (object)GetValue(HeaderProperty); }
            set { SetValue(HeaderProperty, value); }
        }

        /// <summary>
        /// Gets or sets a template to customize the header appearance of the legend.
        /// </summary>
        /// <value>It accepts <see cref="DataTemplate"/> value.</value>
        /// <example>
        /// # [MainPage.xaml](#tab/tabid-11)
        /// <code><![CDATA[
        /// <chart:SfCircularChart>
        ///
        ///     <chart:SfCircularChart.Legend>
        ///         <chart:ChartLegend Header="Data analysis">
        ///            <chart:ChartLegend.HeaderTemplate>
        ///                <DataTemplate>
        ///                    <Border BorderBrush = "Red" BorderThickness="1">
        ///                        <TextBlock Text = "{Binding}"
        ///                                   HorizontalAlignment="Center"
        ///                                   VerticalAlignment="Center"/>
        ///                    </Border>
        ///                </DataTemplate>
        ///            </chart:ChartLegend.HeaderTemplate>
        ///        </chart:ChartLegend>
        ///     </chart:SfCircularChart.Legend>
        ///
        ///     <chart:PieSeries ItemsSource="{Binding Data}"
        ///                      XBindingPath="XValue"
        ///                      YBindingPath="YValue"/>
        ///     
        /// </chart:SfCircularChart>
        ///
        /// ]]>
        /// </code>
        /// ***
        /// </example>
        public DataTemplate HeaderTemplate
        {
            get { return (DataTemplate)GetValue(HeaderTemplateProperty); }
            set { SetValue(HeaderTemplateProperty, value); }
        }

        /// <summary>
        /// Gets or sets the margin value for the legend item.
        /// </summary>
        /// <value>It accepts <see cref="Thickness"/> value.</value>
        /// <example>
        /// # [MainPage.xaml](#tab/tabid-12)
        /// <code><![CDATA[
        /// <chart:SfCircularChart>
        ///
        ///     <chart:SfCircularChart.Legend>
        ///        <chart:ChartLegend ItemMargin = "5"/>
        ///     </chart:SfCircularChart.Legend>
        ///
        ///     <chart:PieSeries ItemsSource="{Binding Data}"
        ///                      XBindingPath="XValue"
        ///                      YBindingPath="YValue"/>
        ///     
        /// </chart:SfCircularChart>
        ///
        /// ]]>
        /// </code>
        /// # [MainPage.xaml.cs](#tab/tabid-13)
        /// <code><![CDATA[
        /// SfCircularChart chart = new SfCircularChart();
        /// ViewModel viewModel = new ViewModel();
        ///
        /// chart.Legend = new ChartLegend(){ ItemMargin = new Thickness(5), };
        /// 
        /// PieSeries series = new PieSeries()
        /// {
        ///    ItemsSource = viewModel.Data,
        ///    XBindingPath = "XValue",
        ///    YBindingPath = "YValue",
        /// };
        /// chart.Series.Add(series);
        ///
        /// ]]>
        /// </code>
        /// ***
        /// </example>
        public Thickness ItemMargin
        {
            get
            {
                return (Thickness)this.GetValue(ChartLegend.ItemMarginProperty);
            }

            set
            {
                this.SetValue(ChartLegend.ItemMarginProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the value for the corner radius of the legend's border.
        /// </summary>
        /// <value>It accepts <see cref="Microsoft.UI.Xaml.CornerRadius"/> value.</value>
        /// <example>
        /// # [MainPage.xaml](#tab/tabid-14)
        /// <code><![CDATA[
        /// <chart:SfCircularChart>
        ///
        ///     <chart:SfCircularChart.Legend>
        ///        <chart:ChartLegend CornerRadius = "5"/>
        ///     </chart:SfCircularChart.Legend>
        ///
        ///     <chart:PieSeries ItemsSource="{Binding Data}"
        ///                      XBindingPath="XValue"
        ///                      YBindingPath="YValue"/>
        ///     
        /// </chart:SfCircularChart>
        ///
        /// ]]>
        /// </code>
        /// # [MainPage.xaml.cs](#tab/tabid-15)
        /// <code><![CDATA[
        /// SfCircularChart chart = new SfCircularChart();
        /// ViewModel viewModel = new ViewModel();
        ///
        /// chart.Legend = new ChartLegend(){ CornerRadius = new CornerRadius(5) };
        /// 
        /// PieSeries series = new PieSeries()
        /// {
        ///    ItemsSource = viewModel.Data,
        ///    XBindingPath = "XValue",
        ///    YBindingPath = "YValue",
        /// };
        /// chart.Series.Add(series);
        ///
        /// ]]>
        /// </code>
        /// ***
        /// </example>
        public CornerRadius CornerRadius
        {
            get
            {
                return (CornerRadius)GetValue(ChartLegend.CornerRadiusProperty);
            }
            set
            {
                SetValue(ChartLegend.CornerRadiusProperty, value);
            }
        }


        /// <summary>
        /// Gets or sets the value indicating whether the legend checkbox is visible or not.
        /// </summary>
        /// <remarks>The chart legend enables the checkbox for each legend item to be visible or collapse the associated series.</remarks>
        /// <value>It accepts <see cref="Visibility"/> value and the default value is <see cref="Visibility.Collapsed"/>.</value>
        /// <example>
        /// # [MainPage.xaml](#tab/tabid-16)
        /// <code><![CDATA[
        /// <chart:SfCircularChart>
        ///
        ///     <chart:SfCircularChart.Legend>
        ///        <chart:ChartLegend CheckBoxVisibility = "Visible"/>
        ///     </chart:SfCircularChart.Legend>
        ///
        ///     <chart:PieSeries ItemsSource="{Binding Data}"
        ///                      XBindingPath="XValue"
        ///                      YBindingPath="YValue"/>
        ///     
        /// </chart:SfCircularChart>
        ///
        /// ]]>
        /// </code>
        /// # [MainPage.xaml.cs](#tab/tabid-17)
        /// <code><![CDATA[
        /// SfCircularChart chart = new SfCircularChart();
        /// ViewModel viewModel = new ViewModel();
        ///
        /// chart.Legend = new ChartLegend(){ CheckBoxVisibility = Visibility.Visible };
        /// 
        /// PieSeries series = new PieSeries()
        /// {
        ///    ItemsSource = viewModel.Data,
        ///    XBindingPath = "XValue",
        ///    YBindingPath = "YValue",
        /// };
        /// chart.Series.Add(series);
        ///
        /// ]]>
        /// </code>
        /// ***
        /// </example>
        public Visibility CheckBoxVisibility
        {
            get
            {
                return (Visibility)this.GetValue(CheckBoxVisibilityProperty);
            }

            set
            {
                this.SetValue(CheckBoxVisibilityProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the value indicating whether the legend icon is visible or not.
        /// </summary>
        /// <value>It accepts <see cref="Visibility"/> value and the default value is <see cref="Visibility.Visible"/>.</value>
        /// <example>
        /// # [MainPage.xaml](#tab/tabid-18)
        /// <code><![CDATA[
        /// <chart:SfCircularChart>
        ///
        ///     <chart:SfCircularChart.Legend>
        ///        <chart:ChartLegend IconVisibility = "Visible"/>
        ///     </chart:SfCircularChart.Legend>
        ///
        ///     <chart:PieSeries ItemsSource="{Binding Data}"
        ///                      XBindingPath="XValue"
        ///                      YBindingPath="YValue"/>
        ///     
        /// </chart:SfCircularChart>
        ///
        /// ]]>
        /// </code>
        /// # [MainPage.xaml.cs](#tab/tabid-19)
        /// <code><![CDATA[
        /// SfCircularChart chart = new SfCircularChart();
        /// ViewModel viewModel = new ViewModel();
        ///
        /// chart.Legend = new ChartLegend(){ IconVisibility = Visibility.Visible };
        /// 
        /// PieSeries series = new PieSeries()
        /// {
        ///    ItemsSource = viewModel.Data,
        ///    XBindingPath = "XValue",
        ///    YBindingPath = "YValue",
        /// };
        /// chart.Series.Add(series);
        ///
        /// ]]>
        /// </code>
        /// ***
        /// </example>
        public Visibility IconVisibility
        {
            get
            {
                return (Visibility)this.GetValue(IconVisibilityProperty);
            }

            set
            {
                this.SetValue(IconVisibilityProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the value for the width of the legend icon.
        /// </summary>
        /// <value>It accepts double values and the default value is 12.</value>
        /// <example>
        /// # [MainPage.xaml](#tab/tabid-29)
        /// <code><![CDATA[
        /// <chart:SfCircularChart>
        ///
        ///     <chart:SfCircularChart.Legend>
        ///        <chart:ChartLegend IconWidth = "15"/>
        ///     </chart:SfCircularChart.Legend>
        ///
        ///     <chart:PieSeries ItemsSource="{Binding Data}"
        ///                      XBindingPath="XValue"
        ///                      YBindingPath="YValue"/>
        ///     
        /// </chart:SfCircularChart>
        ///
        /// ]]>
        /// </code>
        /// # [MainPage.xaml.cs](#tab/tabid-30)
        /// <code><![CDATA[
        /// SfCircularChart chart = new SfCircularChart();
        /// ViewModel viewModel = new ViewModel();
        ///
        /// chart.Legend = new ChartLegend(){ IconWidth = 15 };
        /// 
        /// PieSeries series = new PieSeries()
        /// {
        ///    ItemsSource = viewModel.Data,
        ///    XBindingPath = "XValue",
        ///    YBindingPath = "YValue",
        /// };
        /// chart.Series.Add(series);
        ///
        /// ]]>
        /// </code>
        /// ***
        /// </example>
        public double IconWidth
        {
            get
            {
                return (double)this.GetValue(ChartLegend.IconWidthProperty);
            }

            set
            {
                this.SetValue(IconWidthProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the value for the height of the legend icon.
        /// </summary>
        /// <value>It accepts double value and the default value is 12.</value>
        /// <example>
        /// # [MainPage.xaml](#tab/tabid-31)
        /// <code><![CDATA[
        /// <chart:SfCircularChart>
        ///
        ///     <chart:SfCircularChart.Legend>
        ///        <chart:ChartLegend IconHeight = "15"/>
        ///     </chart:SfCircularChart.Legend>
        ///
        ///     <chart:PieSeries ItemsSource="{Binding Data}"
        ///                      XBindingPath="XValue"
        ///                      YBindingPath="YValue"/>
        ///     
        /// </chart:SfCircularChart>
        ///
        /// ]]>
        /// </code>
        /// # [MainPage.xaml.cs](#tab/tabid-32)
        /// <code><![CDATA[
        /// SfCircularChart chart = new SfCircularChart();
        /// ViewModel viewModel = new ViewModel();
        ///
        /// chart.Legend = new ChartLegend(){ IconHeight = 15 };
        /// 
        /// PieSeries series = new PieSeries()
        /// {
        ///    ItemsSource = viewModel.Data,
        ///    XBindingPath = "XValue",
        ///    YBindingPath = "YValue",
        /// };
        /// chart.Series.Add(series);
        ///
        /// ]]>
        /// </code>
        /// ***
        /// </example>
        public double IconHeight
        {
            get
            {
                return (double)this.GetValue(ChartLegend.IconHeightProperty);
            }

            set
            {
                this.SetValue(ChartLegend.IconHeightProperty, value);
            }
        }


        /// <summary>
        /// Gets or sets the brush value to customize the appearance of the legend outer border.
        /// </summary>
        /// <value>It accepts <see cref="Brush"/> value.</value>
        /// <example>
        /// # [MainPage.xaml](#tab/tabid-20)
        /// <code><![CDATA[
        /// <chart:SfCircularChart>
        ///
        ///     <chart:SfCircularChart.Legend>
        ///        <chart:ChartLegend BorderBrush = "Red" BorderThickness = "2"/>
        ///     </chart:SfCircularChart.Legend>
        ///
        ///     <chart:PieSeries ItemsSource="{Binding Data}"
        ///                      XBindingPath="XValue"
        ///                      YBindingPath="YValue"/>
        ///     
        /// </chart:SfCircularChart>
        ///
        /// ]]>
        /// </code>
        /// # [MainPage.xaml.cs](#tab/tabid-21)
        /// <code><![CDATA[
        /// SfCircularChart chart = new SfCircularChart();
        /// ViewModel viewModel = new ViewModel();
        ///
        /// chart.Legend = new ChartLegend()
        /// {
        ///     BorderBrush = new SolidColorBrush(Colors.Red),
        ///     BorderThickness = 2
        /// };
        /// 
        /// PieSeries series = new PieSeries()
        /// {
        ///    ItemsSource = viewModel.Data,
        ///    XBindingPath = "XValue",
        ///    YBindingPath = "YValue",
        /// };
        /// chart.Series.Add(series);
        ///
        /// ]]>
        /// </code>
        /// ***
        /// </example>
        public Brush BorderBrush
        {
            get { return (Brush)GetValue(BorderBrushProperty); }
            set { SetValue(BorderBrushProperty, value); }
        }

        /// <summary>
        /// Gets or sets the brush value to customize the appearance of the legend background.
        /// </summary>
        /// <value>It accepts <see cref="Brush"/> value.</value>
        /// <example>
        /// # [MainPage.xaml](#tab/tabid-22)
        /// <code><![CDATA[
        /// <chart:SfCircularChart>
        ///
        ///     <chart:SfCircularChart.Legend>
        ///        <chart:ChartLegend Background = "Red"/>
        ///     </chart:SfCircularChart.Legend>
        ///
        ///     <chart:PieSeries ItemsSource="{Binding Data}"
        ///                      XBindingPath="XValue"
        ///                      YBindingPath="YValue"/>
        ///     
        /// </chart:SfCircularChart>
        ///
        /// ]]>
        /// </code>
        /// # [MainPage.xaml.cs](#tab/tabid-23)
        /// <code><![CDATA[
        /// SfCircularChart chart = new SfCircularChart();
        /// ViewModel viewModel = new ViewModel();
        ///
        /// chart.Legend = new ChartLegend()
        /// {
        ///     Background = new SolidColorBrush(Colors.Red),
        /// };
        /// 
        /// PieSeries series = new PieSeries()
        /// {
        ///    ItemsSource = viewModel.Data,
        ///    XBindingPath = "XValue",
        ///    YBindingPath = "YValue",
        /// };
        /// chart.Series.Add(series);
        ///
        /// ]]>
        /// </code>
        /// ***
        /// </example>
        public Brush Background
        {
            get { return (Brush)GetValue(BackgroundProperty); }
            set { SetValue(BackgroundProperty, value); }
        }

        /// <summary>
        /// Gets or sets the thickness value for the border thickness of the legend.
        /// </summary>
        /// <value>It accepts <see cref="Thickness"/> value.</value>
        /// <example>
        /// # [MainPage.xaml](#tab/tabid-24)
        /// <code><![CDATA[
        /// <chart:SfCircularChart>
        ///
        ///     <chart:SfCircularChart.Legend>
        ///        <chart:ChartLegend BorderBrush = "Red" BorderThickness = "2"/>
        ///     </chart:SfCircularChart.Legend>
        ///
        ///     <chart:PieSeries ItemsSource="{Binding Data}"
        ///                      XBindingPath="XValue"
        ///                      YBindingPath="YValue"/>
        ///     
        /// </chart:SfCircularChart>
        ///
        /// ]]>
        /// </code>
        /// # [MainPage.xaml.cs](#tab/tabid-25)
        /// <code><![CDATA[
        /// SfCircularChart chart = new SfCircularChart();
        /// ViewModel viewModel = new ViewModel();
        ///
        /// chart.Legend = new ChartLegend()
        /// {
        ///     BorderBrush = new SolidColorBrush(Colors.Red),
        ///     BorderThickness = 2
        /// };
        /// 
        /// PieSeries series = new PieSeries()
        /// {
        ///    ItemsSource = viewModel.Data,
        ///    XBindingPath = "XValue",
        ///    YBindingPath = "YValue",
        /// };
        /// chart.Series.Add(series);
        ///
        /// ]]>
        /// </code>
        /// ***
        /// </example>
        public Thickness BorderThickness
        {
            get { return (Thickness)GetValue(BorderThicknessProperty); }
            set { SetValue(BorderThicknessProperty, value); }
        }

        /// <summary>
        /// Gets or sets the thickness value for the legend's padding.
        /// </summary>
        /// <value>It accepts <see cref="Thickness"/> value.</value>
        /// <example>
        /// # [MainPage.xaml](#tab/tabid-26)
        /// <code><![CDATA[
        /// <chart:SfCircularChart>
        ///
        ///     <chart:SfCircularChart.Legend>
        ///        <chart:ChartLegend Padding = "5"/>
        ///     </chart:SfCircularChart.Legend>
        ///
        ///     <chart:PieSeries ItemsSource="{Binding Data}"
        ///                      XBindingPath="XValue"
        ///                      YBindingPath="YValue"/>
        ///     
        /// </chart:SfCircularChart>
        ///
        /// ]]>
        /// </code>
        /// # [MainPage.xaml.cs](#tab/tabid-27)
        /// <code><![CDATA[
        /// SfCircularChart chart = new SfCircularChart();
        /// ViewModel viewModel = new ViewModel();
        ///
        /// chart.Legend = new ChartLegend()
        /// {
        ///     Padding = new Thickness(5),
        /// };
        /// 
        /// PieSeries series = new PieSeries()
        /// {
        ///    ItemsSource = viewModel.Data,
        ///    XBindingPath = "XValue",
        ///    YBindingPath = "YValue",
        /// };
        /// chart.Series.Add(series);
        ///
        /// ]]>
        /// </code>
        /// ***
        /// </example>
        public Thickness Padding
        {
            get { return (Thickness)GetValue(PaddingProperty); }
            set { SetValue(PaddingProperty, value); }
        }

        /// <summary>
        /// Gets or sets the HorizontalAlignment value for the legend's header.
        /// </summary>
        /// <value>It accepts <see cref="HorizontalAlignment"/> value.</value>
        public HorizontalAlignment HorizontalHeaderAlignment
        {
            get { return (HorizontalAlignment)GetValue(HorizontalHeaderAlignmentProperty); }
            set { SetValue(HorizontalHeaderAlignmentProperty, value); }
        }

        /// <summary>
        /// Gets or sets a template to customize the appearance of the legend's item.
        /// </summary>
        /// <value>It accepts <see cref="DataTemplate"/> value.</value>
        /// <example>
        /// # [MainPage.xaml](#tab/tabid-28)
        /// <code><![CDATA[
        /// <Grid>
        /// <Grid.Resources>
        ///      <Style TargetType="chart:ChartLegend" >
        ///            <Setter Property="ItemTemplate">
        ///                <Setter.Value>
        ///                    <DataTemplate>
        ///                        <CheckBox IsChecked="{Binding IsSeriesVisible,Mode=TwoWay}"
        ///                                  Content="{Binding Label}"
        ///                                  Margin="5" />
        ///                    </DataTemplate>
        ///                </Setter.Value>
        ///            </Setter>
        ///        </Style>
        ///    </Grid.Resources>
        /// <chart:SfCircularChart>
        ///
        ///     <chart:SfCircularChart.Legend>
        ///        <chart:ChartLegend Padding = "5"/>
        ///     </chart:SfCircularChart.Legend>
        ///
        ///     <chart:PieSeries ItemsSource="{Binding Data}"
        ///                      XBindingPath="XValue"
        ///                      YBindingPath="YValue"/>
        ///     
        /// </chart:SfCircularChart>
        /// </Grid>
        ///
        /// ]]>
        /// </code>
        /// ***
        /// </example>
        public DataTemplate ItemTemplate
        {
            get { return (DataTemplate)GetValue(ItemTemplateProperty); }
            set { SetValue(ItemTemplateProperty, value); }
        }


        internal object ItemsSource
        {
            get
            {
                return (object)this.GetValue(ItemsSourceProperty);
            }

            set
            {
                this.SetValue(ItemsSourceProperty, value);
            }
        }

        object ILegend.ItemsSource
        {
            get { return ItemsSource; }
            set { ItemsSource = value; }
        }
        #endregion
    }
}
