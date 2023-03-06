using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
namespace Syncfusion.Maui.Charts
{
    /// <summary>
    /// The <see cref="ErrorBarSeries"/> indicate the uncertainty or error in data points, making it easy to identify patterns and trends in the data.
    /// </summary>
    /// <remarks>
    /// <para>To render a series, create an instance of ErrorBarSeries, and add it to the <see cref="SfCartesianChart.Series"/> collection.</para>
    /// <para>The <see cref="ErrorBarSeries"/> had no tooltip, data label, animation, and selection support.</para>
    /// </remarks>
    /// <example>
    /// # [Xaml](#tab/tabid-1)
    /// <code><![CDATA[
    ///     <chart:SfCartesianChart>
    ///
    ///           <chart:SfCartesianChart.XAxes>
    ///               <chart:CategoryAxis/>
    ///           </chart:SfCartesianChart.XAxes>
    ///
    ///           <chart:SfCartesianChart.YAxes>
    ///               <chart:NumericalAxis/>
    ///           </chart:SfCartesianChart.YAxes>
    ///
    ///           <chart:SfCartesianChart.Series>
    ///              <chart:ErrorBarSeries ItemsSource="{Binding ThermalExpansion}"   
    ///                                                     XBindingPath="Name"   
    ///                                                     YBindingPath="Value"/>
    ///           </chart:SfCartesianChart.Series>  
    ///     </chart:SfCartesianChart>
    /// ]]></code>
    /// # [C#](#tab/tabid-2)
    /// <code><![CDATA[
    ///     SfCartesianChart chart = new SfCartesianChart();
    ///     
    ///     NumericalAxis xAxis = new CategoryAxis();
    ///     NumericalAxis yAxis = new NumericalAxis();
    ///     
    ///     chart.XAxes.Add(xAxis);
    ///     chart.YAxes.Add(yAxis);
    ///     
    ///     ViewModel viewModel = new ViewModel();
    /// 
    ///     ErrorBarSeries series = new ErrorBarSeries();
    ///     series.ItemsSource = viewModel.ThermalExpansion;
    ///     series.XBindingPath = "Name";
    ///     series.YBindingPath = "Value";
    ///     chart.Series.Add(series);
    ///     
    /// ]]></code>
    /// # [ViewModel](#tab/tabid-3)
    /// <code><![CDATA[
    ///     public ObservableCollection<Model> ThermalExpansion { get; set; }
    /// 
    ///     public ViewModel()
    ///     {
    ///        ThermalExpansion = new ObservableCollection<Model>();
    ///        ThermalExpansion.Add(new Model() { Name="Erbium",Value=8.2,High=7.6 });
    ///        ThermalExpansion.Add(new Model() { Name="Samarium",Value=8.15,High=5.7 });
    ///        ThermalExpansion.Add(new Model() { Name="Yttritium",Value=7.15,High=6.8 });
    ///        ThermalExpansion.Add(new Model() { Name="Carbide",Value=6.45,High=5.9 });
    ///        ThermalExpansion.Add(new Model() { Name="Uranium",Value=7.45,High=7.1 });
    ///        ThermalExpansion.Add(new Model() { Name="Iron",Value=6.7,High=5 });
    ///        ThermalExpansion.Add(new Model() { Name="Thuilium",Value=8.45,High=7.1 });
    ///        ThermalExpansion.Add(new Model() { Name="Steel",Value=9.7,High=8.6});
    ///        ThermalExpansion.Add(new Model() { Name="Tin",Value=14.6,High=10.8 });
    ///        ThermalExpansion.Add(new Model() { Name="Uranium",Value=7.45,High=7.1 });
    ///     }
    /// ]]></code>
    /// ***
    /// </example>
    /// <seealso cref="ErrorBarSegment"/>
    /// <para>In <see cref="ErrorBarSeries"/>, for the rendering of the chart, we have to provide the XBindingPath, YBindingPath.</para>
    public class ErrorBarSeries : XYDataSeries
    {
        #region Properties

        #region Bindable Properties

        /// <summary>
        /// Identifies the <see cref="HorizontalErrorPath"/> bindable property.
        /// </summary>
        public static readonly BindableProperty HorizontalErrorPathProperty =
            BindableProperty.Create(
                nameof(HorizontalErrorPath),
                typeof(string),
                typeof(ErrorBarSeries),
                null,
                BindingMode.Default, propertyChanged: OnHorizontalErrorPathChanged);

        /// <summary>
        /// Identifies the <see cref="VerticalErrorPath"/> bindable property.
        /// </summary>
        public static readonly BindableProperty VerticalErrorPathProperty =
            BindableProperty.Create(
                nameof(VerticalErrorPath),
                typeof(string),
                typeof(ErrorBarSeries),
                null,
                BindingMode.Default, propertyChanged: OnVerticalErrorPathChanged);

        /// <summary>
        /// Identifies the <see cref="HorizontalErrorValue"/> bindable property.
        /// </summary>
        public static readonly BindableProperty HorizontalErrorValueProperty =
            BindableProperty.Create(
                nameof(HorizontalErrorValue),
                typeof(double),
                typeof(ErrorBarSeries),
                0.0,
                BindingMode.Default, propertyChanged: OnHorizontalErrorValueChanged);

        /// <summary>
        /// Identifies the <see cref="VerticalErrorValue"/> bindable property.
        /// </summary>
        public static readonly BindableProperty VerticalErrorValueProperty =
            BindableProperty.Create(
                nameof(VerticalErrorValue),
                typeof(double),
                typeof(ErrorBarSeries),
                0.0, BindingMode.Default, propertyChanged: OnVerticalErrorValueChanged);

        /// <summary>
        /// Identifies the <see cref="Mode"/> bindable property.
        /// </summary>
        public static readonly BindableProperty ModeProperty =
            BindableProperty.Create(
                nameof(Mode),
                typeof(ErrorBarMode),
                typeof(ErrorBarSeries),
                ErrorBarMode.Both, BindingMode.Default, propertyChanged: OnModePropertyChanged);

        /// <summary>
        /// Identifies the <see cref="Type"/> bindable property.
        /// </summary>
        public static readonly BindableProperty TypeProperty =
            BindableProperty.Create(
                nameof(Type),
                typeof(ErrorBarType),
                typeof(ErrorBarSeries),
                ErrorBarType.Fixed, BindingMode.Default, propertyChanged: OnTypePropertyChanged);

        /// <summary>
        /// Identifies the <see cref="HorizontalDirection"/> bindable property.
        /// </summary>
        public static readonly BindableProperty HorizontalDirectionProperty =
            BindableProperty.Create(
                nameof(HorizontalDirection),
                typeof(ErrorBarDirection),
                typeof(ErrorBarSeries),
                ErrorBarDirection.Both, BindingMode.Default, propertyChanged: OnHorizontalDirectionChaged);

        /// <summary>
        /// Identifies the <see cref="VerticalDirection"/> bindable property.
        /// </summary>
        public static readonly BindableProperty VerticalDirectionProperty =
            BindableProperty.Create(
                nameof(VerticalDirection),
                typeof(ErrorBarDirection),
                typeof(ErrorBarSeries),
                ErrorBarDirection.Both, BindingMode.Default, propertyChanged: OnVerticalDirectionChanged);

        /// <summary>
        /// Identifies the <see cref="HorizontalLineStyle"/> bindable property.
        ///</summary>
        public static readonly BindableProperty HorizontalLineStyleProperty =
            BindableProperty.Create(
                nameof(HorizontalLineStyle),
                typeof(ErrorBarLineStyle),
                typeof(ErrorBarSeries),
                null, BindingMode.Default,
                propertyChanged: OnHorizontalLineStyleChanged);

        /// <summary>
        /// Identifies the <see cref="VerticalLineStyle"/> bindable property.
        /// </summary>
        public static readonly BindableProperty VerticalLineStyleProperty =
            BindableProperty.Create(
                nameof(VerticalLineStyle),
                typeof(ErrorBarLineStyle),
                typeof(ErrorBarSeries),
                null, BindingMode.Default,
                propertyChanged: OnVerticalLineStyleChanged);

        /// <summary>
        /// Identifies the <see cref="HorizontalCapLineStyle"/> bindable property.
        /// </summary>
        public static readonly BindableProperty HorizontalCapLineStyleProperty =
            BindableProperty.Create(
                nameof(HorizontalCapLineStyle),
                typeof(ErrorBarCapLineStyle),
                typeof(ErrorBarSeries),
                null, BindingMode.Default, propertyChanged: OnHorizontalCapLineStyleChanged);

        /// <summary>
        /// Identifies the <see cref="VerticalCapLineStyle"/> bindable property.
        /// </summary>
        public static readonly BindableProperty VerticalCapLineStyleProperty =
            BindableProperty.Create(
                nameof(VerticalCapLineStyle),
                typeof(ErrorBarCapLineStyle),
                typeof(ErrorBarSeries),
                null, BindingMode.Default, propertyChanged: OnVerticalCapLineStyleChanged);

        #endregion

        #region Public  Properties

        /// <summary>
        /// Gets or sets a path value on the source object to serve a horizontal error value to the series.
        /// </summary>
        /// <value>It accepts <see cref="string"/> and the default <c>String.Empty</c>.</value>
        /// <remarks>
        /// If the <see cref="HorizontalErrorPath"/> is set, the <see cref="HorizontalErrorValue"/> will be ignored when type is <see cref="ErrorBarType.Custom"/>.
        /// </remarks>
        /// <example>
        /// # [Xaml](#tab/tabid-6)
        /// <code><![CDATA[
        ///     <chart:SfCartesianChart>
        ///
        ///     <!-- ... Eliminated for simplicity-->
        ///
        ///           <chart:ErrorBarSeries ItemsSource="{Binding ThermalExpansion}"   
        ///                                                     XBindingPath="Name" 
        ///                                                     YBindingPath="Value"
        ///                                                     HorizontalErrorPath="Low"/>
        ///     </chart:SfCartesianChart>
        /// ]]></code>
        /// # [C#](#tab/tabid-7)
        /// <code><![CDATA[
        ///     SfCartesianChart chart = new SfCartesianChart();
        ///     ViewModel viewModel = new ViewModel();
        ///
        ///     // Eliminated for simplicity
        ///
        ///     ErrorBarSeries errorBarSeries = new ErrorBarSeries()
        ///     {
        ///           ItemsSource = viewModel.Data,
        ///           XBindingPath = "Name",
        ///           YBindingPath="Value",
        ///           HorizontalErrorPath="Low"
        ///     };
        ///     
        ///     chart.Series.Add(errorBarSeries);
        ///
        /// ]]></code>
        /// ***
        /// </example>
        public string HorizontalErrorPath
        {
            get { return (string)GetValue(HorizontalErrorPathProperty); }
            set { SetValue(HorizontalErrorPathProperty, value); }
        }

        /// <summary>
        /// Gets or sets a path value on the source object to serve a vertical error value to the series.
        /// </summary>
        /// <value>It accepts <see cref="string"/>and the default is <c>String.Empty</c>.</value>
        /// <remarks>
        /// If the <see cref="VerticalErrorPath"/> is set, the <see cref="VerticalErrorValue"/> will be ignored when type is <see cref="ErrorBarType.Custom"/>.
        /// </remarks>
        /// <example>
        /// # [Xaml](#tab/tabid-6)
        /// <code><![CDATA[
        ///     <chart:SfCartesianChart>
        ///
        ///     <!-- ... Eliminated for simplicity-->
        ///
        ///      <chart:ErrorBarSeries ItemsSource="{Binding ThermalExpansion}"    
        ///                                                  YBindingPath="Value"
        ///                                                  XBindingPath = "Name"
        ///                                                  VerticalErrorPath="High"/>
        ///
        ///     </chart:SfCartesianChart>
        /// ]]></code>
        /// # [C#](#tab/tabid-7)
        /// <code><![CDATA[
        ///     SfCartesianChart chart = new SfCartesianChart();
        ///     ViewModel viewModel = new ViewModel();
        ///
        ///     // Eliminated for simplicity
        ///
        ///     ErrorBarSeries errorBarSeries = new ErrorBarSeries()
        ///     {
        ///           ItemsSource = viewModel.Data,
        ///           XBindingPath = "Name",
        ///           YBindingPath = "Value",
        ///           VerticalErrorPath="High"
        ///     };
        ///     
        ///     chart.Series.Add(errorBarSeries);
        ///
        /// ]]></code>
        /// ***
        /// </example>
        public string VerticalErrorPath
        {
            get { return (string)GetValue(VerticalErrorPathProperty); }
            set { SetValue(VerticalErrorPathProperty, value); }
        }

        /// <summary>
        /// Gets or sets the value of the horizontal errors of the series.
        /// </summary>
        /// <remarks>
        /// The <see cref="HorizontalErrorValue"/> works when there is no <see cref="HorizontalErrorPath"/> set and the <see cref="Mode"/> is <see cref="ErrorBarMode.Both"/> or <see cref="ErrorBarMode.Horizontal"/>.
        /// </remarks>
        /// <value>It accepts <see cref="double"/> and the default is 0.</value>
        /// <example>
        /// # [Xaml](#tab/tabid-6)
        /// <code><![CDATA[
        ///     <chart:SfCartesianChart>
        ///
        ///     <!-- ... Eliminated for simplicity-->
        ///
        ///           <chart:ErrorBarSeries ItemsSource="{Binding ThermalExpansion}"   
        ///                                                     XBindingPath="Name"   
        ///                                                     YBindingPath="Value"
        ///                                                    HorizontalErrorValue="0.25"/>
        ///
        ///     </chart:SfCartesianChart>
        /// ]]></code>
        /// # [C#](#tab/tabid-7)
        /// <code><![CDATA[
        ///     SfCartesianChart chart = new SfCartesianChart();
        ///     ViewModel viewModel = new ViewModel();
        ///
        ///     // Eliminated for simplicity
        ///
        ///     ErrorBarSeries errorBarSeries = new ErrorBarSeries()
        ///     {
        ///           ItemsSource = viewModel.Data,
        ///           XBindingPath = "Name",
        ///           YBindingPath = "Value",
        ///           HorizontalErrorValue="0.25",
        ///     };
        ///     
        ///     chart.Series.Add(errorBarSeries);
        ///
        /// ]]></code>
        /// ***
        /// </example>
        public double HorizontalErrorValue
        {
            get { return (double)GetValue(HorizontalErrorValueProperty); }
            set { SetValue(HorizontalErrorValueProperty, value); }
        }

        /// <summary>
        /// Gets or sets the value of the vertical errors of the series.
        /// </summary>
        ///  <value>It accepts <see cref="double"/> and the default is 0.</value>
        /// <remarks>
        /// The <see cref="VerticalErrorValue"/> works when there is no <see cref="VerticalErrorPath"/> set and the <see cref="Mode"/> is <see cref="ErrorBarMode.Both"/> or <see cref="ErrorBarMode.Vertical"/>.
        /// </remarks>
        /// <example>
        /// # [Xaml](#tab/tabid-6)
        /// <code><![CDATA[
        ///     <chart:SfCartesianChart>
        ///
        ///     <!-- ... Eliminated for simplicity-->
        ///
        ///           <chart:ErrorBarSeries ItemsSource="{Binding ThermalExpansion}"   
        ///                                                     XBindingPath="Name"   
        ///                                                     YBindingPath="Value"
        ///                                                     VerticalErrorValue="5"/>
        ///     </chart:SfCartesianChart>
        /// ]]></code>
        /// # [C#](#tab/tabid-7)
        /// <code><![CDATA[
        ///     SfCartesianChart chart = new SfCartesianChart();
        ///     ViewModel viewModel = new ViewModel();
        ///
        ///     // Eliminated for simplicity
        ///
        ///     ErrorBarSeries errorBarSeries = new ErrorBarSeries()
        ///     {
        ///           ItemsSource = viewModel.Data,
        ///           XBindingPath = "Name",
        ///           YBindingPath = "Value",
        ///           VerticalErrorValue="5"
        ///     };
        ///     
        ///     chart.Series.Add(errorBarSeries);
        ///
        /// ]]></code>
        /// ***
        /// </example>
        public double VerticalErrorValue
        {
            get { return (double)GetValue(VerticalErrorValueProperty); }
            set { SetValue(VerticalErrorValueProperty, value); }
        }

        /// <summary>
        /// Gets or sets the mode of the error bar to be displayed, whether horizontal, vertical, or both.
        /// </summary>
        /// <value>Its defaults is <see cref="ErrorBarMode.Both"></see></value>
        /// <example>
        /// # [Xaml](#tab/tabid-6)
        /// <code><![CDATA[
        ///     <chart:SfCartesianChart>
        ///
        ///     <!-- ... Eliminated for simplicity-->
        ///
        ///      <chart:ErrorBarSeries ItemsSource="{Binding ThermalExpansion}"   
        ///                                                  XBindingPath="Name"   
        ///                                                  YBindingPath="Value"
        ///                                                  HorizontalErrorValue=0.25
        ///                                                  VerticalErrorValue=5
        ///                                                  Mode="Both"/>
        ///     </chart:SfCartesianChart>
        /// ]]></code>
        /// # [C#](#tab/tabid-7)
        /// <code><![CDATA[
        ///     SfCartesianChart chart = new SfCartesianChart();
        ///     ViewModel viewModel = new ViewModel();
        ///
        ///     // Eliminated for simplicity
        ///
        ///     ErrorBarSeries errorBarSeries = new ErrorBarSeries()
        ///     {
        ///           ItemsSource = viewModel.Data,
        ///           XBindingPath = "Name",
        ///           YBindingPath = "Value",
        ///           HorizontalErrorValue=0.25,
        ///           VerticalErrorValue="5",
        ///           Mode=ErrorBarMode.Both
        ///     };
        ///     
        ///     chart.Series.Add(errorBarSeries);
        ///
        /// ]]></code>
        /// ***
        /// </example>
        public ErrorBarMode Mode
        {
            get { return (ErrorBarMode)GetValue(ModeProperty); }
            set { SetValue(ModeProperty, value); }
        }

        /// <summary>
        /// Gets or sets the error bar type, whether it is specified as a standard deviation, a standard error, a percentage, or a fixed value.
        /// </summary>
        ///  <value>Its defaults is <see cref="ErrorBarType.Fixed"></see></value>
        /// <example>
        /// # [Xaml](#tab/tabid-6)
        /// <code><![CDATA[
        ///     <chart:SfCartesianChart>
        ///
        ///     <!-- ... Eliminated for simplicity-->
        ///
        ///           <chart:ErrorBarSeries ItemsSource="{Binding ThermalExpansion}"   
        ///                                                     XBindingPath="Name"   
        ///                                                     YBindingPath="Value"
        ///                                                     HorizontalErrorValue=0.25
        ///                                                     VerticalErrorValue=5
        ///                                                     Type="Fixed"/>
        ///     </chart:SfCartesianChart>
        /// ]]></code>
        /// # [C#](#tab/tabid-7)
        /// <code><![CDATA[
        ///     SfCartesianChart chart = new SfCartesianChart();
        ///     ViewModel viewModel = new ViewModel();
        ///
        ///     // Eliminated for simplicity
        ///
        ///     ErrorBarSeries errorBarSeries = new ErrorBarSeries()
        ///     {
        ///           ItemsSource = viewModel.Data,
        ///           XBindingPath = "Name",
        ///           YBindingPath = "Value",
        ///           HorizontalErrorValue=0.25,
        ///           VerticalErrorValue="5",
        ///           Type=ErrorBarType.Fixed
        ///     };
        ///     
        ///     chart.Series.Add(errorBarSeries);
        ///
        /// ]]></code>
        /// ***
        /// </example>
        public ErrorBarType Type
        {
            get { return (ErrorBarType)GetValue(TypeProperty); }
            set { SetValue(TypeProperty, value); }
        }

        /// <summary>
        /// Gets or sets the direction to specify whether to show positive, negative, or both directions of horizontal error values to display.
        /// </summary>
        /// <value>Its defaults is <see cref="ErrorBarDirection.Both"></see></value>
        /// <example>
        /// # [Xaml](#tab/tabid-6)
        /// <code><![CDATA[
        ///     <chart:SfCartesianChart>
        ///
        ///     <!-- ... Eliminated for simplicity-->
        ///
        ///           <chart:ErrorBarSeries ItemsSource="{Binding ThermalExpansion}"   
        ///                                                     XBindingPath="Name"   
        ///                                                     YBindingPath="Value"
        ///                                                     HorizontalErrorValue=0.25,
        ///                                                     VerticalErrorValue="5"
        ///                                                     HorizontalDirection="Both"/>
        ///
        ///     </chart:SfCartesianChart>
        /// ]]></code>
        /// # [C#](#tab/tabid-7)
        /// <code><![CDATA[
        ///     SfCartesianChart chart = new SfCartesianChart();
        ///     ViewModel viewModel = new ViewModel();
        ///
        ///     // Eliminated for simplicity
        ///
        ///     ErrorBarSeries errorBarSeries = new ErrorBarSeries()
        ///     {
        ///           ItemsSource = viewModel.Data,
        ///           XBindingPath = "Name",
        ///           YBindingPath = "Value",
        ///           HorizontalErrorValue=0.25,
        ///           VerticalErrorValue="5"
        ///           HorizontalDirection=ErrorBarDirection.Both
        ///     };
        ///     
        ///     chart.Series.Add(errorBarSeries);
        ///
        /// ]]></code>
        /// ***
        /// </example>
        public ErrorBarDirection HorizontalDirection
        {
            get { return (ErrorBarDirection)GetValue(HorizontalDirectionProperty); }
            set { SetValue(HorizontalDirectionProperty, value); }
        }

        /// <summary>
        /// Gets or sets the direction to specify whether to show positive, negative, or both directions of vertical error values to display.
        /// </summary>
        /// <value>Its defaults to <see cref="ErrorBarDirection.Both"></see></value>
        /// <example>
        /// # [Xaml](#tab/tabid-6)
        /// <code><![CDATA[
        ///     <chart:SfCartesianChart>
        ///
        ///     <!-- ... Eliminated for simplicity-->
        ///
        ///           <chart:ErrorBarSeries ItemsSource="{Binding ThermalExpansion}"   
        ///                                                     XBindingPath="Name"   
        ///                                                     YBindingPath="Value"
        ///                                                     HorizontalErrorValue=0.25,
        ///                                                     VerticalErrorValue="5"
        ///                                                     VerticalDirection="Both"/>
        ///
        ///     </chart:SfCartesianChart>
        /// ]]></code>
        /// # [C#](#tab/tabid-7)
        /// <code><![CDATA[
        ///     SfCartesianChart chart = new SfCartesianChart();
        ///     ViewModel viewModel = new ViewModel();
        ///
        ///     // Eliminated for simplicity
        ///
        ///     ErrorBarSeries errorBarSeries = new ErrorBarSeries()
        ///     {
        ///           ItemsSource = viewModel.Data,
        ///           XBindingPath = "Name",
        ///           YBindingPath = "Value",
        ///           HorizontalErrorValue=0.25,
        ///           VerticalErrorValue="5"
        ///           VerticalDirection=ErrorBarDirection.Both
        ///     };
        ///     
        ///     chart.Series.Add(errorBarSeries);
        ///
        /// ]]></code>
        /// ***
        /// </example>
        public ErrorBarDirection VerticalDirection
        {
            get { return (ErrorBarDirection)GetValue(VerticalDirectionProperty); }
            set { SetValue(VerticalDirectionProperty, value); }
        }

        /// <summary>
        /// Gets or sets the style for horizontal line and it is often used to customize the appearance of horizontal error bar for visual purposes.
        /// </summary>
        ///  <value>Its defaulf is null.</value>
        /// <example>
        /// # [Xaml](#tab/tabid-6)
        /// <code><![CDATA[
        ///     <chart:SfCartesianChart>
        ///
        ///     <!-- ... Eliminated for simplicity-->
        ///
        ///           <chart:ErrorBarSeries ItemsSource="{Binding ThermalExpansion}"   
        ///                                                     XBindingPath="Name"   
        ///                                                     YBindingPath="Value"
        ///                                                     HorizontalErrorValue=0.25,
        ///                                                     VerticalErrorValue="5"/>
        ///                                                     
        ///                    <chart:ErrorBarSeries.HorizontalLineStyle>
        ///                   <chart:ErrorBarLineStyle Stroke = "Black" ></ chart:ErrorBarLineStyle>
        ///          </chart:ErrorBarSeries.HorizontalLineStyle>
        ///
        ///     </chart:SfCartesianChart>
        /// ]]></code>
        /// # [C#](#tab/tabid-7)
        /// <code><![CDATA[
        ///     SfCartesianChart chart = new SfCartesianChart();
        ///     ViewModel viewModel = new ViewModel();
        ///
        ///     // Eliminated for simplicity
        ///
        ///     ErrorBarSeries errorBarSeries = new ErrorBarSeries()
        ///     {
        ///           ItemsSource = viewModel.Data,
        ///           XBindingPath = "Name",
        ///           YBindingPath = "Value",
        ///           HorizontalErrorValue=0.25,
        ///           VerticalErrorValue="5"
        ///     };
        ///     errorBarSeries.HorizontalLineStyle = new ErrorBarLineStyle()
        ///     {
        ///          Stroke = new SolidColorBrush(Colors.Black),
        ///      }
        ///     chart.Series.Add(errorBarSeries);
        ///
        /// ]]></code>
        /// ***
        /// </example>
        public ErrorBarLineStyle HorizontalLineStyle
        {
            get { return (ErrorBarLineStyle)GetValue(HorizontalLineStyleProperty); }
            set { SetValue(HorizontalLineStyleProperty, value); }
        }

        /// <summary>
        /// Gets or sets the style for vertical line and it is often used to customize the appearance of vertical error bar for visual purposes.
        /// </summary>
        ///  <value>Its defaulf is null.</value>
        /// <example>
        /// # [Xaml](#tab/tabid-6)
        /// <code><![CDATA[
        ///     <chart:SfCartesianChart>
        ///
        ///     <!-- ... Eliminated for simplicity-->
        ///
        ///           <chart:ErrorBarSeries ItemsSource="{Binding ThermalExpansion}"   
        ///                                                     XBindingPath="Name"   
        ///                                                     YBindingPath="Value"
        ///                                                     HorizontalErrorValue=0.25
        ///                                                     VerticalErrorValue="5"/>
        ///                                                      
        ///                    <chart:ErrorBarSeries.VerticalLineStyle>
        ///                   <chart:ErrorBarLineStyle Stroke = "Black" ></ chart:ErrorBarLineStyle>
        ///          </chart:ErrorBarSeries.VerticalLineStyle>
        ///
        ///     </chart:SfCartesianChart>
        /// ]]></code>
        /// # [C#](#tab/tabid-7)
        /// <code><![CDATA[
        ///     SfCartesianChart chart = new SfCartesianChart();
        ///     ViewModel viewModel = new ViewModel();
        ///
        ///     // Eliminated for simplicity
        ///
        ///     ErrorBarSeries errorBarSeries = new ErrorBarSeries()
        ///     {
        ///           ItemsSource = viewModel.Data,
        ///           XBindingPath = "Name",
        ///           YBindingPath = "Value",
        ///           HorizontalErrorValue=0.25,
        ///           VerticalErrorValue="5"
        ///     };
        ///     errorBarSeries.VerticalLineStyle = new ErrorBarLineStyle()
        ///     {
        ///          Stroke = new SolidColorBrush(Colors.Black),
        ///      }
        ///     
        ///     chart.Series.Add(errorBarSeries);
        ///
        /// ]]></code>
        /// ***
        /// </example>
        public ErrorBarLineStyle VerticalLineStyle
        {
            get { return (ErrorBarLineStyle)GetValue(VerticalLineStyleProperty); }
            set { SetValue(VerticalLineStyleProperty, value); }
        }

        /// <summary>
        /// Gets or sets the style for horizontal caps and it is often used to customize the appearance of horizontal caps in the error bar for visual purposes.
        /// </summary>
        ///  <value>Its defaulf is null.</value>
        /// <example>
        /// # [Xaml](#tab/tabid-6)
        /// <code><![CDATA[
        ///     <chart:SfCartesianChart>
        ///
        ///     <!-- ... Eliminated for simplicity-->
        ///
        ///           <chart:ErrorBarSeries ItemsSource="{Binding ThermalExpansion}"   
        ///                                                     XBindingPath="Name"   
        ///                                                     YBindingPath="Value"
        ///                                                     HorizontalErrorValue=0.25
        ///                                                     VerticalErrorValue="5"/>                   
        ///                    <chart:ErrorBarSeries.HorizontalCapLineStyle>
        ///                   <chart:ErrorBarCapLineStyle Stroke="Black"></chart:ErrorBarCapLineStyle>
        ///          </chart:ErrorBarSeries.HorizontalCapLineStyle>
        ///
        ///     </chart:SfCartesianChart>
        /// ]]></code>
        /// # [C#](#tab/tabid-7)
        /// <code><![CDATA[
        ///     SfCartesianChart chart = new SfCartesianChart();
        ///     ViewModel viewModel = new ViewModel();
        ///
        ///     // Eliminated for simplicity
        ///
        ///     ErrorBarSeries errorBarSeries = new ErrorBarSeries()
        ///     {
        ///           ItemsSource = viewModel.Data,
        ///           XBindingPath = "Name",
        ///           YBindingPath = "Value",
        ///           HorizontalErrorValue=0.25,
        ///           VerticalErrorValue="5"
        ///     };
        ///     errorBarSeries.HorizontalCapLineStyle = new ErrorBarCapLineStyle()
        ///     {
        ///          Stroke = new SolidColorBrush(Colors.Black),
        ///      }
        ///     
        ///     chart.Series.Add(errorBarSeries);
        ///
        /// ]]></code>
        /// ***
        /// </example>
        public ErrorBarCapLineStyle HorizontalCapLineStyle
        {
            get { return (ErrorBarCapLineStyle)GetValue(HorizontalCapLineStyleProperty); }
            set { SetValue(HorizontalCapLineStyleProperty, value); }
        }

        /// <summary>
        /// Gets or sets the style for vertical caps and it is often used to customize the appearance of vertical caps in the error bar for visual purposes.
        /// </summary>
        ///  <value>Its defaulf is null.</value>
        /// <example>
        /// # [Xaml](#tab/tabid-6)
        /// <code><![CDATA[
        ///     <chart:SfCartesianChart>
        ///
        ///     <!-- ... Eliminated for simplicity-->
        ///
        ///           <chart:ErrorBarSeries ItemsSource="{Binding ThermalExpansion}"   
        ///                                                     XBindingPath="Name"   
        ///                                                     YBindingPath="Value"
        ///                                                     HorizontalErrorValue=0.25
        ///                                                     VerticalErrorValue="5"/>
        ///                                                      
        ///                    <chart:ErrorBarSeries.VerticalCapLineStyle>
        ///                   <chart:ErrorBarCapLineStyle Stroke="Black" CapSize="20"></chart:ErrorBarCapLineStyle>
        ///          </chart:ErrorBarSeries.VerticalCapLineStyle>
        ///
        ///     </chart:SfCartesianChart>
        /// ]]></code>
        /// # [C#](#tab/tabid-7)
        /// <code><![CDATA[
        ///     SfCartesianChart chart = new SfCartesianChart();
        ///     ViewModel viewModel = new ViewModel();
        ///
        ///     // Eliminated for simplicity
        ///
        ///     ErrorBarSeries errorBarSeries = new ErrorBarSeries()
        ///     {
        ///           ItemsSource = viewModel.Data,
        ///           XBindingPath = "Name",
        ///           YBindingPath = "Value",
        ///           HorizontalErrorValue=0.25,
        ///           VerticalErrorValue="5"
        ///     };
        ///     errorBarSeries.VerticalCapLineStyle = new ErrorBarCapLineStyle()
        ///     {
        ///          Stroke = new SolidColorBrush(Colors.Black),
        ///          CapSize=20;
        ///      }
        ///     
        ///     chart.Series.Add(errorBarSeries);
        ///
        /// ]]></code>
        /// ***
        /// </example>
        public ErrorBarCapLineStyle VerticalCapLineStyle
        {
            get { return (ErrorBarCapLineStyle)GetValue(VerticalCapLineStyleProperty); }
            set { SetValue(VerticalCapLineStyleProperty, value); }
        }

        #endregion

        #region Internal Properties

        internal IList<double> HorizontalErrorValues { get; set; }
        internal IList<double> VerticalErrorValues { get; set; }

        #endregion

        #endregion

        #region Constructor

        #region Public  Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorBarSeries"/> class.
        /// </summary>
        public ErrorBarSeries()
        {
            HorizontalErrorValues = new List<double>();
            VerticalErrorValues = new List<double>();

            HorizontalLineStyle = new ErrorBarLineStyle();
            VerticalLineStyle = new ErrorBarLineStyle();
            HorizontalCapLineStyle = new ErrorBarCapLineStyle();
            VerticalCapLineStyle = new ErrorBarCapLineStyle();
        }

        #endregion

        #endregion

        #region Methods

        #region Internal Methods

        internal override void GenerateSegments(SeriesView seriesView)
        {
            var xValues = GetXValues();

            GeneratePathValues();
            
            if (xValues == null || xValues.Count == 0 || YDoubleList == null || YValues.Count == 0 
                || YDoubleList.Count == 0 || this.HorizontalErrorValues.Count == 0 || this.VerticalErrorValues.Count == 0)
            {
                return;
            }
            if (Segments.Count == 0)
            {
                var segment = CreateSegment() as ErrorBarSegment;
                if (segment != null)
                {
                    segment.Series = this;
                    segment.SeriesView = seriesView;
                    segment.SetData(xValues, (IList)YValues);
                    Segments.Add(segment);
                }
            }
        }

        internal void GeneratePathValues()
        {
            if (YDoubleList == null || YDoubleList.Count == 0) return;

            int horizontalPathIndex = YDataPaths.IndexOf(this.HorizontalErrorPath);
            if (horizontalPathIndex >= 0)
                this.HorizontalErrorValues = YDoubleList[horizontalPathIndex];

            int verticalPathIndex = YDataPaths.IndexOf(this.VerticalErrorPath);
            if (verticalPathIndex >= 0)
                this.VerticalErrorValues = YDoubleList[verticalPathIndex];
        }

        internal override bool IsMultipleYPathRequired
        {
            get
            {
                bool yPathDecision = Type is ErrorBarType.Custom ? true : false;
                return yPathDecision;
            }
        }

        #endregion

        #region Protected  Methods

        /// <summary>
        /// Creates the Error Bar segments.
        /// </summary>
        /// <returns></returns>
        protected override ChartSegment CreateSegment()
        {
            return new ErrorBarSegment();
        }

        #endregion

        #region Private Methods


        private static void OnHorizontalErrorPathChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is ErrorBarSeries series)
            {
                if (bindable is XYDataSeries xyDataSeries)
                {
                    OnYBindingPathChanged(bindable, oldValue, newValue);
                }
            }
        }

        private static void OnVerticalErrorPathChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is ErrorBarSeries series)
            {
                if (bindable is XYDataSeries xyDataSeries)
                {
                    OnYBindingPathChanged(bindable, oldValue, newValue);
                }
            }
        }

        private static void OnHorizontalErrorValueChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is ErrorBarSeries series)
            {
                series.SegmentsCreated = false;
                series.ScheduleUpdateChart();
            }
        }

        private static void OnVerticalErrorValueChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is ErrorBarSeries series)
            {
                series.SegmentsCreated = false;
                series.ScheduleUpdateChart();
            }
        }

        private static void OnModePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is ErrorBarSeries series)
            {
                series.SegmentsCreated = false;
                series.ScheduleUpdateChart();
            }
        }

        private static void OnTypePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is ErrorBarSeries series)
            {
                series.OnBindingPathChanged();
            }
        }

        private static void OnHorizontalDirectionChaged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is ErrorBarSeries series)
            {
                series.SegmentsCreated = false;
                series.ScheduleUpdateChart();
            }
        }

        private static void OnVerticalDirectionChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is ErrorBarSeries series)
            {
                series.SegmentsCreated = false;
                series.ScheduleUpdateChart();
            }
        }

        private static void OnHorizontalLineStyleChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is ErrorBarSeries series)
            {
                series.OnStylePropertyChanged(oldValue as ChartLineStyle, newValue as ChartLineStyle);
            }
        }

        private static void OnVerticalLineStyleChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is ErrorBarSeries series)
            {
                series.OnStylePropertyChanged(oldValue as ChartLineStyle, newValue as ChartLineStyle);
            }
        }

        private static void OnHorizontalCapLineStyleChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is ErrorBarSeries series)
            {
                series.OnStylePropertyChanged(oldValue as ChartLineStyle, newValue as ChartLineStyle);
            }
        }

        private static void OnVerticalCapLineStyleChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is ErrorBarSeries series)
            {
                series.OnStylePropertyChanged(oldValue as ChartLineStyle, newValue as ChartLineStyle);
            }
        }

        private void OnStylePropertyChanged(ChartLineStyle? oldValue, ChartLineStyle? newValue)
        {
            if (oldValue != null)
            {
                oldValue.PropertyChanged -= ErrorBarLineStyles_PropertyChanged;
            }

            if (newValue != null)
            {
                newValue.PropertyChanged += ErrorBarLineStyles_PropertyChanged; ;
                SetInheritedBindingContext(newValue, BindingContext);
            }

            if (AreaBounds != Rect.Zero)
            {
                InvalidateSeries();
            }
        }

        private void ErrorBarLineStyles_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            InvalidateSeries();
        }

        #endregion

        #endregion
    }
}
