using Microsoft.Maui.Controls;
using System;

namespace Syncfusion.Maui.Charts
{

    /// <summary>
    /// Represent the line style for the <see cref="ErrorBarSeries"/> 
    /// </summary>
    public class ErrorBarLineStyle : ChartLineStyle
    {
        /// <summary>
        /// Identifies the <see cref="StrokeCap"/> bindable property.
        /// </summary>
        public static readonly BindableProperty StrokeCapProperty =
            BindableProperty.Create(
                nameof(StrokeCap),
                typeof(ErrorBarStrokeCap),
                typeof(ErrorBarLineStyle),
                ErrorBarStrokeCap.Flat);

        /// <summary>
        /// Gets or sets the stroke cap style to customize the stroke cap appearance.
        /// </summary>
        /// <value>Defaults to <see cref="ErrorBarStrokeCap.Flat"></see> </value>
        /// <example>
        /// # [Xaml](#tab/tabid-6)
        /// <code><![CDATA[
        ///     <chart:SfCartesianChart>
        ///     <!-- ... Eliminated for simplicity-->
        ///           <chart:ErrorBarSeries ItemsSource="{Binding ThermalExpansion}"   
        ///                                                     XBindingPath="Name"   
        ///                                                     YBindingPath="Value" />
        ///                 <chart.ErrorBarSeries.HorizontalLineStyle>
        ///                                <chart:ErrorBarLineStyle StrokeCap=Round />
        ///                 </chart.ErrorBarSeries.HorizontalLineStyle>                                    
        ///     </chart:SfCartesianChart>
        /// ]]></code>
        /// # [C#](#tab/tabid-7)
        /// <code><![CDATA[
        ///     SfCartesianChart chart = new SfCartesianChart();
        ///     ViewModel viewModel = new ViewModel();
        ///     // Eliminated for simplicity
        ///     ErrorBarSeries errorBarSeries = new ErrorBarSeries()
        ///     {
        ///           ItemsSource = viewModel.Data,
        ///           XBindingPath = "Name",
        ///           YBindingPath = "Value",
        ///     };
        ///     errorBarSeries.HorizontalLineStyle = new ErrorBarLineStyle()
        ///     {
        ///          StrokeCap=ErrorBarStrokeCap.Round;
        ///      }
        ///     chart.Series.Add(errorBarSeries);
        ///
        /// ]]></code>
        /// ***
        /// </example>
        public ErrorBarStrokeCap StrokeCap
        {
            get { return (ErrorBarStrokeCap)GetValue(StrokeCapProperty); }
            set { SetValue(StrokeCapProperty, value); }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorBarLineStyle"/> class.
        /// </summary>
        public ErrorBarLineStyle()
        {
            Stroke = Brush.Black;
            StrokeWidth = 1;
        }
    }

    /// <summary>
    /// Represents the cap style for the <see cref="ErrorBarSeries"/>
    /// </summary>
    public class ErrorBarCapLineStyle : ChartLineStyle
    {
        /// <summary>
        /// Identifies the <see cref="CapLineSize"/> bindable property.
        /// </summary>
        public static readonly BindableProperty CapLineSizeProperty =
            BindableProperty.Create(
                nameof(CapLineSize),
                typeof(double),
                typeof(ErrorBarCapLineStyle),
                1d);

        /// <summary>
        /// Identifies the <see cref="IsVisible"/> bindable property.
        /// </summary>
        public static readonly BindableProperty IsVisibleProperty =
            BindableProperty.Create(
                nameof(IsVisible),
                typeof(bool),
                typeof(ErrorBarCapLineStyle),
                true);

        /// <summary>
        /// Gets or sets the stroke cap size to customize the size of the stroke caps.
        /// </summary>
        /// <value>It accepts <see cref="double"/> values and the default value is 10</value>
        /// <example>
        /// # [Xaml](#tab/tabid-6)
        /// <code><![CDATA[
        ///     <chart:SfCartesianChart>
        ///     <!-- ... Eliminated for simplicity-->
        ///           <chart:ErrorBarSeries ItemsSource="{Binding ThermalExpansion}"   
        ///                                                     XBindingPath="Name"   
        ///                                                     YBindingPath="Value" />
        ///                 <chart.ErrorBarSeries.HorizontalCapLineStyle>
        ///                                <chart:ErrorBarCapLineStyle CapLineSize=15 />
        ///                 </chart.ErrorBarSeries.HorizontalCapLineStyle>                                      
        ///     </chart:SfCartesianChart>
        /// ]]></code>
        /// # [C#](#tab/tabid-7)
        /// <code><![CDATA[
        ///     SfCartesianChart chart = new SfCartesianChart();
        ///     ViewModel viewModel = new ViewModel();
        ///     // Eliminated for simplicity
        ///     ErrorBarSeries errorBarSeries = new ErrorBarSeries()
        ///     {
        ///           ItemsSource = viewModel.Data,
        ///           XBindingPath = "Name",
        ///           YBindingPath = "Value",
        ///     };
        ///     errorBarSeries.HorizontalCapLineStyle = new ErrorBarCapLineStyle()
        ///     {
        ///          CapLineSize=15;
        ///      }
        ///     chart.Series.Add(errorBarSeries);
        /// ]]></code>
        /// ***
        /// </example>
        public double CapLineSize
        {
            get { return (double)GetValue(CapLineSizeProperty); }
            set { SetValue(CapLineSizeProperty, value); }
        }

        /// <summary>
        /// Gets or sets the stroke cap visibility to customize the stroke cap appearance.
        /// </summary>
        /// <value>It accepts <see cref="bool"/> and the default  is true</value>
        /// <example>
        /// # [Xaml](#tab/tabid-6)
        /// <code><![CDATA[
        ///     <chart:SfCartesianChart>
        ///     <!-- ... Eliminated for simplicity-->
        ///           <chart:ErrorBarSeries ItemsSource="{Binding ThermalExpansion}"   
        ///                                                     XBindingPath="Name"   
        ///                                                     YBindingPath="Value" />
        ///                 <chart.ErrorBarSeries.HorizontalCapLineStyle>
        ///                                <chart:ErrorBarCapLineStyle IsVisible="False" />
        ///                 </chart.ErrorBarSeries.HorizontalCapLineStyle>                                      
        ///     </chart:SfCartesianChart>
        /// ]]></code>
        /// # [C#](#tab/tabid-7)
        /// <code><![CDATA[
        ///     SfCartesianChart chart = new SfCartesianChart();
        ///     ViewModel viewModel = new ViewModel();
        ///     // Eliminated for simplicity
        ///     ErrorBarSeries errorBarSeries = new ErrorBarSeries()
        ///     {
        ///           ItemsSource = viewModel.Data,
        ///           XBindingPath = "Name",
        ///           YBindingPath = "Value",
        ///     };
        ///     errorBarSeries.HorizontalCapLineStyle = new ErrorBarCapLineStyle()
        ///     {
        ///          IsVisible=False;
        ///      }
        ///     chart.Series.Add(errorBarSeries);
        /// ]]></code>
        /// ***
        /// </example>
        public bool IsVisible
        {
            get { return (bool)GetValue(IsVisibleProperty); }
            set { SetValue(IsVisibleProperty, value); }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorBarCapLineStyle"/> class.
        /// </summary>
        public ErrorBarCapLineStyle()
        {
            Stroke = Brush.Black;
            StrokeWidth = 1;
            CapLineSize = 10;
        }
    }
}

