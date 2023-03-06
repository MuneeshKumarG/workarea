﻿using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using System.Collections.Generic;

namespace Syncfusion.Maui.Charts
{
    /// <summary>
    /// The <see cref="RangeAxisBase"/> is the base class for all types of range axis.
    /// </summary>
    public partial class RangeAxisBase
    {
        #region Bindable Properties
        /// <summary>
        /// Identifies the <see cref="MinorTicksPerInterval"/> bindable property.
        /// </summary>        
        public static readonly BindableProperty MinorTicksPerIntervalProperty = BindableProperty.Create(
            nameof(MinorTicksPerInterval),
            typeof(int),
            typeof(RangeAxisBase),
            0,
            BindingMode.Default,
            null,
            OnMinorTicksPerIntervalPropertyChanged);

        /// <summary>
        /// Identifies the <see cref="EdgeLabelsVisibilityMode"/> bindable property.
        /// </summary>        
        public static readonly BindableProperty EdgeLabelsVisibilityModeProperty = BindableProperty.Create(
            nameof(EdgeLabelsVisibilityMode),
            typeof(EdgeLabelsVisibilityMode),
            typeof(RangeAxisBase),
            EdgeLabelsVisibilityMode.Default,
            BindingMode.Default,
            null,
            OnEdgeLabelsVisibilityModeChanged);

        /// <summary>
        /// Identifies the <see cref="MinorGridLineStyle"/> bindable property.
        /// </summary>        
        public static readonly BindableProperty MinorGridLineStyleProperty = BindableProperty.Create(
            nameof(MinorGridLineStyle),
            typeof(ChartLineStyle),
            typeof(RangeAxisBase),
            null,
            BindingMode.Default,
            null,
            OnMinorGridLineStylePropertyChanged);

        /// <summary>
        /// Identifies the <see cref="MinorTickStyle"/> bindable property.
        /// </summary>   
        public static readonly BindableProperty MinorTickStyleProperty = BindableProperty.Create(
            nameof(MinorTickStyle),
            typeof(ChartAxisTickStyle),
            typeof(RangeAxisBase),
            null,
            BindingMode.Default,
            null,
            OnMinorTickStylePropertyChanged);

        /// <summary>
        /// Identifies the <see cref="ShowMinorGridLines"/> bindable property.
        /// </summary>  
        public static readonly BindableProperty ShowMinorGridLinesProperty = BindableProperty.Create(
            nameof(ShowMinorGridLines),
            typeof(bool),
            typeof(RangeAxisBase),
            true,
            BindingMode.Default,
            null,
            OnShowMinorGridlinesPropertyChanged);
        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="RangeAxisBase"/>.
        /// </summary>
        public RangeAxisBase()
        {
            SmallTickPoints = new List<double>();
            MinorGridLineStyle = new ChartLineStyle();
            MinorGridLineStyle.Stroke = new SolidColorBrush(Color.FromArgb("#EDEFF1"));
            MinorGridLineStyle.StrokeWidth = 0.5f;
            MinorTickStyle = new ChartAxisTickStyle();
            MinorTickStyle.TickSize = 4d;
        }

        #endregion

        #region Public Properties
        /// <summary>
        /// Gets or sets the edge labels visibility mode for hiding the edge labels on zooming.
        /// </summary>
        /// <value>It accepts the <see cref="Charts.EdgeLabelsVisibilityMode"/> values and its default value is <see cref="Charts.EdgeLabelsVisibilityMode.Default"/>.</value>
        /// <example>
        /// # [MainPage.xaml](#tab/tabid-1)
        /// <code><![CDATA[
        /// <chart:SfCartesianChart>
        ///  
        ///     <chart:SfCartesianChart.XAxes>
        ///         <chart:NumericalAxis EdgeLabelsVisibilityMode ="Visible" />
        ///     </chart:SfCartesianChart.XAxes>
        /// 
        /// </chart:SfCartesianChart>
        /// ]]>
        /// </code>
        /// # [MainPage.xaml.cs](#tab/tabid-2)
        /// <code><![CDATA[
        /// SfCartesianChart chart = new SfCartesianChart();
        /// 
        /// NumericalAxis xaxis = new NumericalAxis()
        /// {
        ///    EdgeLabelsVisibilityMode = EdgeLabelsVisibilityMode.Visible,
        /// };
        /// chart.XAxes.Add(xaxis);
        /// 
        /// ]]>
        /// </code>
        /// *** 
        /// </example>
        public EdgeLabelsVisibilityMode EdgeLabelsVisibilityMode
        {
            get { return (EdgeLabelsVisibilityMode)GetValue(EdgeLabelsVisibilityModeProperty); }
            set { SetValue(EdgeLabelsVisibilityModeProperty, value); }
        }

        /// <summary>
        ///  Gets or sets the value that defines the number of minor tick/grid lines to be drawn between the adjacent major tick/grid lines.
        /// </summary>
        /// <value>It accepts the <c>integer</c> values and its default value is 0.</value>
        /// <example>
        /// # [MainPage.xaml](#tab/tabid-3)
        /// <code><![CDATA[
        /// <chart:SfCartesianChart>
        /// 
        ///         <chart:SfCartesianChart.XAxes>
        ///             <chart:NumericalAxis MinorTicksPerInterval="3"  />
        ///         </chart:SfCartesianChart.XAxes>
        /// 
        /// </chart:SfCartesianChart>
        /// ]]>
        /// </code>
        /// # [MainPage.xaml.cs](#tab/tabid-4)
        /// <code><![CDATA[
        /// SfCartesianChart chart = new SfCartesianChart();
        /// 
        /// NumericalAxis xaxis = new NumericalAxis();
        /// xaxis.MinorTicksPerInterval= 3;
        /// chart.XAxes.Add(xaxis);
        /// 
        /// ]]>
        /// </code>
        /// ***
        /// </example>
        public int MinorTicksPerInterval
        {
            get { return (int)GetValue(MinorTicksPerIntervalProperty); }
            set { SetValue(MinorTicksPerIntervalProperty, value); }
        }

        /// <summary>
        /// Gets or sets the ChartLineStyle to customize the appearance of the minor grid lines.
        /// </summary>
        /// <value>It accepts the <see cref="ChartLineStyle"/> value.</value>
        /// <example>
        /// # [MainWindow.xaml](#tab/tabid-5)
        /// <code><![CDATA[
        ///     <chart:SfCartesianChart>
        ///      
        ///          <chart:SfCartesianChart.Resources>
        ///              <DoubleCollection x:Key="dashArray">
        ///                  <x:Double>3</x:Double>
        ///                  <x:Double>3</x:Double>
        ///              </DoubleCollection>
        ///          </chart:SfCartesianChart.Resources>
        ///
        ///           <chart:SfCartesianChart.XAxes>
        ///               <chart:NumericalAxis ShowMinorGridLines="True">
        ///                   <chart:NumericalAxis.MajorGridLineStyle>
        ///                       <chart:ChartLineStyle StrokeDashArray="{StaticResource dashArray}" Stroke="Black" StrokeWidth="0.8"/>
        ///                   </chart:NumericalAxis.MajorGridLineStyle>
        ///               </chart:NumericalAxis>
        ///           </chart:SfCartesianChart.XAxes>
        ///
        ///           <chart:SfCartesianChart.YAxes>
        ///               <chart:NumericalAxis/>
        ///           </chart:SfCartesianChart.YAxes>
        ///
        ///     </chart:SfCartesianChart>
        /// ]]></code>
        /// # [MainWindow.cs](#tab/tabid-6)
        /// <code><![CDATA[
        ///     SfCartesianChart chart = new SfCartesianChart();
        ///     
        ///     NumericalAxis xAxis = new NumericalAxis();
        ///     chart.XAxes.Add(xAxis);
        ///     
        ///     DoubleCollection doubleCollection = new DoubleCollection();
        ///     doubleCollection.Add(3);
        ///     doubleCollection.Add(3);
        ///     
        ///     NumericalAxis yAxis = new NumericalAxis();
        ///     ChartLineStyle axisLineStyle = new ChartLineStyle();
        ///     axisLineStyle.Stroke = SolidColorBrush.Black;
        ///     axisLineStyle.StrokeWidth = 0.8;
        ///     axisLineStyle.StrokeDashArray = doubleCollection
        ///     yAxis.MinorGridLineStyle = axisLineStyle;
        ///     chart.YAxes.Add(yAxis);
        /// ]]></code>
        /// ***
        /// </example>
        public ChartLineStyle MinorGridLineStyle
        {
            get { return (ChartLineStyle)GetValue(MinorGridLineStyleProperty); }
            set { SetValue(MinorGridLineStyleProperty, value); }
        }

        /// <summary>
        /// Gets or sets the ChartAxisTickStyle to customize the appearance of the minor tick lines.
        /// </summary>
        /// <value>It accepts the <see cref="ChartAxisTickStyle"/> value.</value>
        /// <example>
        /// # [MainWindow.xaml](#tab/tabid-7)
        /// <code><![CDATA[
        ///     <chart:SfCartesianChart>
        ///      
        ///           <chart:SfCartesianChart.XAxes>
        ///               <chart:NumericalAxis MinorTicksPerInterval="4">
        ///                   <chart:NumericalAxis.MinorTickStyle>
        ///                       <chart:ChartAxisTickStyle Stroke="Red" StrokeWidth="1"/>
        ///                   </chart:NumericalAxis.MinorTickStyle>
        ///               </chart:NumericalAxis>
        ///           </chart:SfCartesianChart.XAxes>
        ///
        ///           <chart:SfCartesianChart.YAxes>
        ///               <chart:NumericalAxis/>
        ///           </chart:SfCartesianChart.YAxes>
        ///
        ///     </chart:SfCartesianChart>
        /// ]]></code>
        /// # [MainWindow.cs](#tab/tabid-8)
        /// <code><![CDATA[
        ///     SfCartesianChart chart = new SfCartesianChart();
        ///     
        ///     NumericalAxis xAxis = new NumericalAxis();
        ///     xAxis.MinorTicksPerInterval = 4;
        ///     xAxis.MinorTickStyle.StrokeWidth = 1;
        ///     xAxis.MinorTickStyle.Stroke = SolidColorBrush.Red;
        ///     chart.XAxes.Add(xAxis);
        ///     
        ///     NumericalAxis yAxis = new NumericalAxis();
        ///     chart.YAxes.Add(yAxis);
        /// ]]></code>
        /// ***
        /// </example>
        public ChartAxisTickStyle MinorTickStyle
        {
            get { return (ChartAxisTickStyle)GetValue(MinorTickStyleProperty); }
            set { SetValue(MinorTickStyleProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the axis minor grid lines can be displayed or not.
        /// </summary>
        /// <value>It accepts the bool value and its default value is <c>True</c>.</value>
        /// <example>
        /// # [MainPage.xaml](#tab/tabid-9)
        /// <code><![CDATA[
        /// <chart:SfCartesianChart>
        ///  
        ///     <chart:SfCartesianChart.XAxes>
        ///         <chart:NumericalAxis ShowMinorGridLines = "False" 
        ///                              MinorTicksPerInterval="2" />
        ///     </chart:SfCartesianChart.XAxes>
        /// 
        /// </chart:SfCartesianChart>
        /// ]]>
        /// </code>
        /// # [MainPage.xaml.cs](#tab/tabid-10)
        /// <code><![CDATA[
        /// SfCartesianChart chart = new SfCartesianChart();
        /// 
        /// NumericalAxis xaxis = new NumericalAxis()
        /// {
        ///    ShowMinorGridLines = false,
        ///    MinorTicksPerInterval = 2,
        /// };
        /// chart.XAxes.Add(xaxis);
        /// 
        /// ]]>
        /// </code>
        /// *** 
        /// </example>
        public bool ShowMinorGridLines
        {
            get { return (bool)GetValue(ShowMinorGridLinesProperty); }
            set { SetValue(ShowMinorGridLinesProperty, value); }
        }
        #endregion

        #region Methods

        #region Protected Methods

        /// <inheritdoc/>
        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();

            if (MinorGridLineStyle != null)
            {
                SetInheritedBindingContext(MinorGridLineStyle, BindingContext);
            }

            if (MinorTickStyle != null)
            {
                SetInheritedBindingContext(MinorTickStyle, BindingContext);
            }
        }

        #endregion

        #region Internal Methods
        internal override void Dispose()
        {
            if (MinorTickStyle != null)
            {
                MinorTickStyle.PropertyChanged -= LineStyle_PropertyChanged;
            }

            if (MinorGridLineStyle != null)
            {
                MinorGridLineStyle.PropertyChanged -= LineStyle_PropertyChanged;
            }

            base.Dispose();
        }

        internal bool CanDrawMinorGridLines()
        {
            return ShowMinorGridLines && MinorGridLineStyle.CanDraw();
        }

        #endregion

        #region Private Methods
        private static void OnMinorTicksPerIntervalPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var axis = bindable as RangeAxisBase;
            if (axis != null)
            {
                axis.UpdateSmallTickRequired((int)newValue);
                axis.UpdateLayout();
            }
        }

        private static void OnEdgeLabelsVisibilityModeChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var axis = bindable as RangeAxisBase;
            if (axis != null)
            {
                axis.UpdateLayout();
            }
        }

        private static void OnMinorGridLineStylePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var axis = bindable as RangeAxisBase;
            if (axis == null)
            {
                return;
            }

            if (oldValue != null)
            {
                var style = newValue as ChartLineStyle;
                if (style != null)
                {
                    style.PropertyChanged -= axis.LineStyle_PropertyChanged;
                }
            }

            if (newValue != null)
            {
                var lineStyle = newValue as ChartLineStyle;
                if (lineStyle != null)
                {
                    SetInheritedBindingContext(lineStyle, axis.BindingContext);
                    lineStyle.PropertyChanged += axis.LineStyle_PropertyChanged;
                }
            }

            axis?.UpdateLayout();
        }

        private static void OnMinorTickStylePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var axis = bindable as RangeAxisBase;
            if (axis == null)
            {
                return;
            }

            if (oldValue != null)
            {
                var style = newValue as ChartAxisTickStyle;
                if (style != null)
                {
                    style.PropertyChanged -= axis.LineStyle_PropertyChanged;
                }
            }

            if (newValue != null && axis != null)
            {
                var tickStyle = newValue as ChartAxisTickStyle;
                if (tickStyle != null)
                {
                    SetInheritedBindingContext(tickStyle, axis.BindingContext);
                    tickStyle.PropertyChanged += axis.LineStyle_PropertyChanged;
                }
            }

            axis?.UpdateLayout();
        }

        private static void OnShowMinorGridlinesPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var axis = bindable as RangeAxisBase;
            if (axis != null)
            {
                axis.UpdateLayout();
            }
        }

        private void LineStyle_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            this.UpdateLayout();
        }
        #endregion

        #endregion
    }
}
