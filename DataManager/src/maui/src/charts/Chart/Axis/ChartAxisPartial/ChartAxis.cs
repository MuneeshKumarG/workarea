﻿using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using System;
using System.Collections.Generic;

namespace Syncfusion.Maui.Charts
{
    /// <summary>
    /// The ChartAxis is the base class for all types of axes.
    /// </summary>
    /// <remarks>
    /// <para>The ChartAxis is used to locate a data point inside the chart area. Charts typically have two axes that are used to measure and categorize data.</para>
    /// <para>The Vertical(Y) axis always uses numerical scale.</para>
    /// <para>The Horizontal(X) axis supports the Category, Numeric and Date-time.</para>  
    /// </remarks>
    public partial class ChartAxis
    {
        #region Bindable Properties

        /// <summary>
        /// Identifies the <see cref="IsVisible"/> bindable property.
        /// </summary>        
        public static readonly BindableProperty IsVisibleProperty = BindableProperty.Create(
            nameof(IsVisible),
            typeof(bool),
            typeof(ChartAxis),
            true,
            BindingMode.Default,
            null,
            OnIsVisibleChanged);

        /// <summary>
        /// Identifies the <see cref="AxisLineOffset"/> bindable property.
        /// </summary>        
        public static readonly BindableProperty AxisLineOffsetProperty = BindableProperty.Create(
            nameof(AxisLineOffset),
            typeof(double),
            typeof(ChartAxis),
            0d,
            BindingMode.Default,
            null,
            OnAxisLineOffsetChanged);

        /// <summary>
        /// Identifies the <see cref="LabelRotation"/> bindable property.
        /// </summary>        
        public static readonly BindableProperty LabelRotationProperty = BindableProperty.Create(
            nameof(LabelRotation),
            typeof(double),
            typeof(ChartAxis),
            0d,
            BindingMode.Default,
            null,
            OnLabelRotationAngleChanged);

        /// <summary>
        /// Identifies the <see cref="LabelStyle"/> bindable property.
        /// </summary>        
        public static readonly BindableProperty LabelStyleProperty = BindableProperty.Create(
            nameof(LabelStyle),
            typeof(ChartAxisLabelStyle),
            typeof(ChartAxis),
            null,
            BindingMode.Default,
            null,
            OnLabelStyleChanged);

        /// <summary>
        /// Identifies the <see cref="AxisLineStyle"/> bindable property.
        /// </summary>        
        public static readonly BindableProperty AxisLineStyleProperty = BindableProperty.Create(
            nameof(AxisLineStyle),
            typeof(ChartLineStyle),
            typeof(ChartAxis),
            null,
            BindingMode.Default,
            null,
            OnAxisLineStyleChanged);

        /// <summary>
        /// Identifies the <see cref="CrossesAt"/> bindable property.
        /// </summary>        
        public static readonly BindableProperty CrossesAtProperty = BindableProperty.Create(
            nameof(CrossesAt),
            typeof(object),
            typeof(ChartAxis),
            double.NaN,
            BindingMode.Default,
            null,
            OnCrossesChanged);

        /// <summary>
        /// Identifies the <see cref="RenderNextToCrossingValue"/> bindable property.
        /// </summary>
        public static readonly BindableProperty RenderNextToCrossingValueProperty = BindableProperty.Create(
            nameof(RenderNextToCrossingValue),
            typeof(bool),
            typeof(ChartAxis),
            true,
            BindingMode.Default,
            null,
            OnRenderNextToCrossingValuePropertyChanged);

        /// <summary>
        /// Identifies the <see cref="CrossAxisName"/> bindable property.
        /// </summary>
        public static readonly BindableProperty CrossAxisNameProperty = BindableProperty.Create(
            nameof(CrossAxisName),
            typeof(string),
            typeof(ChartAxis),
            null,
            BindingMode.Default,
            null,
            OnCrossAxisNameChanged);

        /// <summary>
        /// Identifies the <see cref="Title"/> bindable property.
        /// </summary>        
        public static readonly BindableProperty TitleProperty = BindableProperty.Create(
            nameof(Title),
            typeof(ChartAxisTitle),
            typeof(ChartAxis),
            null,
            BindingMode.Default,
            null,
            OnTitleChanged);

        /// <summary>
        /// Identifies the <see cref="IsInversed"/> bindable property.
        /// </summary>        
        public static readonly BindableProperty IsInversedProperty = BindableProperty.Create(
            nameof(IsInversed),
            typeof(bool),
            typeof(ChartAxis),
            false,
            BindingMode.Default,
            null,
            OnInversedChanged);

        /// <summary>
        /// Identifies the <see cref="EdgeLabelsDrawingMode"/> bindable property.
        /// </summary>        
        public static readonly BindableProperty EdgeLabelsDrawingModeProperty = BindableProperty.Create(
            nameof(EdgeLabelsDrawingMode),
            typeof(EdgeLabelsDrawingMode),
            typeof(ChartAxis),
            EdgeLabelsDrawingMode.Center,
            BindingMode.Default,
            null,
            OnEdgeLabelsDrawingModeChanged);

        /// <summary>
        /// Identifies the <see cref="MajorGridLineStyle"/> bindable property.
        /// </summary>        
        public static readonly BindableProperty MajorGridLineStyleProperty = BindableProperty.Create(
            nameof(MajorGridLineStyle),
            typeof(ChartLineStyle),
            typeof(ChartAxis),
            null,
            BindingMode.Default,
            null,
            OnMajorGridLineStyleChanged);

        /// <summary>
        /// Identifies the <see cref="MajorTickStyle"/> bindable property.
        /// </summary>        
        public static readonly BindableProperty MajorTickStyleProperty = BindableProperty.Create(
            nameof(MajorTickStyle),
            typeof(ChartAxisTickStyle),
            typeof(ChartAxis),
            null,
            BindingMode.Default,
            null,
            OnMajorTickStyleChanged);

        /// <summary>
        /// Identifies the <see cref="ZoomPosition"/> bindable property.
        /// </summary>        
        public static readonly BindableProperty ZoomPositionProperty = BindableProperty.Create(
            nameof(ZoomPosition),
            typeof(double),
            typeof(ChartAxis),
            0d,
            BindingMode.TwoWay,
            null,
            OnZoomPositionChanged);

        /// <summary>
        ///Identifies the <see cref="ZoomFactor"/> bindable property.
        /// </summary>        
        public static readonly BindableProperty ZoomFactorProperty = BindableProperty.Create(
            nameof(ZoomFactor),
            typeof(double),
            typeof(ChartAxis),
            1d,
            BindingMode.TwoWay,
            null,
            OnZoomFactorChanged);

        /// <summary>
        /// Identifies the <see cref="ShowMajorGridLines"/> bindable property.
        /// </summary>        
        public static readonly BindableProperty ShowMajorGridLinesProperty = BindableProperty.Create(
            nameof(ShowMajorGridLines),
            typeof(bool),
            typeof(ChartAxis),
            true,
            BindingMode.Default,
            null,
            OnShowMajorGridLinesChanged);

        /// <summary>
        /// Identifies the <see cref="Name"/> bindable property.
        /// </summary>
        public static readonly BindableProperty NameProperty = BindableProperty.Create(
            nameof(Name),
            typeof(string),
            typeof(ChartAxis),
            string.Empty,
            BindingMode.Default,
            null,
            OnNameChanged);

        /// <summary>
        /// Identifies the <see cref="PlotOffsetStart"/> bindable property.
        /// </summary>        
        public static readonly BindableProperty PlotOffsetStartProperty = BindableProperty.Create(
            nameof(PlotOffsetStart),
            typeof(double),
            typeof(ChartAxis),
            0d,
            BindingMode.Default,
            null,
            OnPlotOffsetStartChanged);

        /// <summary>
        /// Identifies the <see cref="PlotOffsetEnd"/> bindable property.
        /// </summary>        
        public static readonly BindableProperty PlotOffsetEndProperty = BindableProperty.Create(
            nameof(PlotOffsetEnd),
            typeof(double),
            typeof(ChartAxis),
            0d,
            BindingMode.Default,
            null,
            OnPlotOffsetEndChanged);

        /// <summary>
        /// Identifies the <see cref="EnableAutoIntervalOnZooming"/> bindable property.
        /// </summary>        
        public static readonly BindableProperty EnableAutoIntervalOnZoomingProperty = BindableProperty.Create(
            nameof(EnableAutoIntervalOnZooming),
            typeof(bool),
            typeof(ChartAxis),
            true,
            BindingMode.Default,
            null,
            OnEnableAutoIntervalOnZoomingPropertyChanged);

        /// <summary>
        /// Identifies the <see cref="ShowTrackballLabel"/> bindable property.
        /// </summary>        
        public static readonly BindableProperty ShowTrackballLabelProperty =
            BindableProperty.Create(
                nameof(ShowTrackballLabel),
                typeof(bool),
                typeof(ChartAxis),
                false,
                BindingMode.Default,
                null);

        /// <summary>
        /// Identifies the <see cref="TrackballLabelStyle"/> bindable property.
        /// </summary>       
        public static readonly BindableProperty TrackballLabelStyleProperty =
            BindableProperty.Create(
                nameof(TrackballLabelStyle),
                typeof(ChartLabelStyle),
                typeof(ChartAxis),
                null,
                BindingMode.Default,
                null);

        #region Internal Properties

        //Todo: Need to provide support for next release.

        /// <summary>
        /// Gets or sets an actions to be taken when two labels intersect in bounds. 
        /// </summary>
        internal static readonly BindableProperty LabelsIntersectActionProperty = BindableProperty.Create(
            nameof(LabelsIntersectAction),
            typeof(AxisLabelsIntersectAction),
            typeof(ChartAxis),
            AxisLabelsIntersectAction.Hide,
            BindingMode.Default,
            null,
            OnLabelsIntersectActionChanged);

        /// <summary>
        /// Identifies the <see cref="AutoScrollingMode"/> bindable property.
        /// </summary>
        public static readonly BindableProperty AutoScrollingModeProperty = BindableProperty.Create(
            nameof(AutoScrollingMode),
            typeof(ChartAutoScrollingMode),
            typeof(ChartAxis),
            ChartAutoScrollingMode.End,
            BindingMode.Default,
            null,
            OnAutoScrollingModeChanged);

        /// <summary>
		/// Gets or sets the value that determines the distance between the axis label and axis title. 
        /// </summary>
        internal static readonly BindableProperty LabelExtentProperty = BindableProperty.Create(
            nameof(LabelExtent),
            typeof(double),
            typeof(ChartAxis),
            0d,
            BindingMode.Default,
            null,
            OnLabelExtentChanged);

        /// <summary>
		/// Gets or sets the options for customizing for the axis range. 
        /// </summary>
        internal static readonly BindableProperty RangeStylesProperty = BindableProperty.Create(
            nameof(RangeStyles),
            typeof(ChartAxisRangeStyleCollection),
            typeof(ChartAxis),
            null,
            BindingMode.Default,
            null,
            OnRangeStylesPropertyChanged);

        /// <summary>
		/// Identifies the <see cref="AutoScrollingDelta"/> bindable property.
        /// </summary>
        public static readonly BindableProperty AutoScrollingDeltaProperty = BindableProperty.Create(
            nameof(AutoScrollingDelta),
            typeof(double),
            typeof(ChartAxis),
            double.NaN,
            BindingMode.Default,
            null,
            OnAutoScrollingDeltaPropertyChanged);

        /// <summary>
        /// Gets or sets the position of the axis tick lines. 
        /// </summary>
        internal static readonly BindableProperty TickPositionProperty = BindableProperty.Create(
            nameof(TickPosition),
            typeof(AxisElementPosition),
            typeof(ChartAxis),
            AxisElementPosition.Outside,
            BindingMode.Default,
            null,
            OnTickPositionChanged);

        /// <summary>
		/// Gets or sets the value that determines the number of labels to be displayed per 100 pixels. 
        /// </summary>
        internal static readonly BindableProperty MaximumLabelsProperty = BindableProperty.Create(
            nameof(MaximumLabels),
            typeof(int),
            typeof(ChartAxis),
            3,
            BindingMode.Default,
            null,
            OnMaximumLabelsChanged);

        #endregion

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets a value indicating whether to show/hide the chart axis.
        /// </summary>
        /// <value>It accepts bool values and its default value is <c>True</c>.</value>
        /// <example>
        /// # [MainPage.xaml](#tab/tabid-5)
        /// <code><![CDATA[
        /// <chart:SfCartesianChart>
        ///  
        ///     <chart:SfCartesianChart.XAxes>
        ///         <chart:CategoryAxis IsVisible = "False" />
        ///     </chart:SfCartesianChart.XAxes>
        /// 
        /// </chart:SfCartesianChart>
        /// ]]>
        /// </code>
        /// # [MainPage.xaml.cs](#tab/tabid-6)
        /// <code><![CDATA[
        /// SfCartesianChart chart = new SfCartesianChart();
        /// 
        /// CategoryAxis xaxis = new CategoryAxis()
        /// {
        ///    IsVisible = false
        /// };
        /// chart.XAxes.Add(xaxis);
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
        /// Gets or sets a value to provide padding to the axis at the start position.
        /// </summary>
        /// <value>It accepts <c>double</c> values and its default value is 0.</value>
        /// <remarks>
        /// <see cref="PlotOffsetStart"/> applies padding at the start of a plot area where the axis and its elements are rendered in a chart with padding at the start.
        /// </remarks>
        /// <example>
        /// # [MainPage.xaml](#tab/tabid-1)
        /// <code><![CDATA[
        /// <chart:SfCartesianChart>
        ///  
        ///     <chart:SfCartesianChart.XAxes>
        ///         <chart:CategoryAxis PlotOffsetStart = "30" />
        ///     </chart:SfCartesianChart.XAxes>
        ///
        ///     <chart:SfCartesianChart.YAxes>
        ///         <chart:NumericalAxis PlotOffsetStart = "30" />
        ///     </chart:SfCartesianChart.YAxes>
        /// 
        /// </chart:SfCartesianChart>
        /// ]]>
        /// </code>
        /// # [MainPage.xaml.cs](#tab/tabid-2)
        /// <code><![CDATA[
        /// SfCartesianChart chart = new SfCartesianChart();
        /// 
        /// CategoryAxis xaxis = new CategoryAxis()
        /// {
        ///    PlotOffsetStart = 30
        /// };
        /// 
        /// NumericalAxis yaxis = new NumericalAxis()
        /// {
        ///    PlotOffsetStart = 30
        /// };
        ///
        /// chart.XAxes.Add(xaxis);
        /// chart.YAxes.Add(yaxis);
        /// 
        /// ]]>
        /// </code>
        /// *** 
        /// </example>
        /// <see cref="PlotOffsetEnd"/>
        public double PlotOffsetStart
        {
            get { return (double)GetValue(PlotOffsetStartProperty); }
            set { SetValue(PlotOffsetStartProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value to provide padding to the axis at end position.
        /// </summary>
        /// <value>It accepts <c>double</c> values and its default value is 0.</value>
        /// <remarks>
        /// <see cref="PlotOffsetEnd"/> applies padding at end of the plot area where the axis and its elements are rendered in the chart with padding at the end.
        /// </remarks>
        /// <example>
        /// # [MainPage.xaml](#tab/tabid-3)
        /// <code><![CDATA[
        /// <chart:SfCartesianChart>
        ///  
        ///     <chart:SfCartesianChart.XAxes>
        ///         <chart:CategoryAxis PlotOffsetEnd = "30" />
        ///     </chart:SfCartesianChart.XAxes>
        ///
        ///     <chart:SfCartesianChart.YAxes>
        ///         <chart:NumericalAxis PlotOffsetEnd = "30" />
        ///     </chart:SfCartesianChart.YAxes>
        /// 
        /// </chart:SfCartesianChart>
        /// ]]>
        /// </code>
        /// # [MainPage.xaml.cs](#tab/tabid-4)
        /// <code><![CDATA[
        /// SfCartesianChart chart = new SfCartesianChart();
        /// 
        /// CategoryAxis xaxis = new CategoryAxis()
        /// {
        ///    PlotOffsetEnd = 30
        /// };
        /// 
        /// NumericalAxis yaxis = new NumericalAxis()
        /// {
        ///    PlotOffsetEnd = 30
        /// };
        ///
        /// chart.XAxes.Add(xaxis);
        /// chart.YAxes.Add(yaxis);
        /// 
        /// ]]>
        /// </code>
        /// *** 
        /// </example>
        /// <see cref="PlotOffsetStart"/>
        public double PlotOffsetEnd
        {
            get { return (double)GetValue(PlotOffsetEndProperty); }
            set { SetValue(PlotOffsetEndProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value to provide padding to the axis line.
        /// </summary>
        /// <value>It accepts <c>double</c> values and its default value is 0.</value>
        /// <example>
        /// # [MainPage.xaml](#tab/tabid-5)
        /// <code><![CDATA[
        /// <chart:SfCartesianChart>
        ///  
        ///     <chart:SfCartesianChart.XAxes>
        ///         <chart:CategoryAxis AxisLineOffset = "30" />
        ///     </chart:SfCartesianChart.XAxes>
        /// 
        /// </chart:SfCartesianChart>
        /// ]]>
        /// </code>
        /// # [MainPage.xaml.cs](#tab/tabid-6)
        /// <code><![CDATA[
        /// SfCartesianChart chart = new SfCartesianChart();
        /// 
        /// CategoryAxis xaxis = new CategoryAxis()
        /// {
        ///    AxisLineOffset = 30
        /// };
        /// chart.XAxes.Add(xaxis);
        /// 
        /// ]]>
        /// </code>
        /// *** 
        /// </example>
        public double AxisLineOffset
        {
            get { return (double)GetValue(AxisLineOffsetProperty); }
            set { SetValue(AxisLineOffsetProperty, value); }
        }

        /// <summary>
        /// Gets or sets the value for the rotation angle of the axis labels.
        /// </summary>
        /// <value>It accepts the <c>double</c> values and the default value is 0.</value>
        /// <remarks>
        /// Label rotation angle can be set from -90 to 90 degrees.
        /// </remarks>
        /// <example>
        /// # [MainPage.xaml](#tab/tabid-7)
        /// <code><![CDATA[
        /// <chart:SfCartesianChart>
        ///  
        ///     <chart:SfCartesianChart.XAxes>
        ///         <chart:CategoryAxis LabelRotation = "90" />
        ///     </chart:SfCartesianChart.XAxes>
        /// 
        /// </chart:SfCartesianChart>
        /// ]]>
        /// </code>
        /// # [MainPage.xaml.cs](#tab/tabid-8)
        /// <code><![CDATA[
        /// SfCartesianChart chart = new SfCartesianChart();
        /// 
        /// CategoryAxis xaxis = new CategoryAxis()
        /// {
        ///    LabelRotation = 90,
        /// };
        /// chart.XAxes.Add(xaxis);
        /// 
        /// ]]>
        /// </code>
        /// *** 
        /// </example>
        public double LabelRotation
        {
            get { return (double)GetValue(LabelRotationProperty); }
            set { SetValue(LabelRotationProperty, value); }
        }

        /// <summary>
        /// Gets or sets the value to customize the appearance of chart axis labels. 
        /// </summary>
        /// <value>It accepts the <see cref="Charts.ChartAxisLabelStyle"/> value.</value>
        /// <remarks>
        /// To customize the axis labels appearance, you need to create an instance of the <see cref="ChartAxisLabelStyle"/> class and set to the <see cref="LabelStyle"/> property.
        /// </remarks>
        /// <example>
        /// # [MainPage.xaml](#tab/tabid-9)
        /// <code><![CDATA[
        /// <chart:SfCartesianChart>
        ///  
        ///     <chart:SfCartesianChart.XAxes>
        ///         <chart:CategoryAxis>
        ///            <chart:CategoryAxis.LabelStyle>
        ///                <chart:ChartAxisLabelStyle TextColor = "Red" FontSize="14"/>
        ///            </chart:CategoryAxis.LabelStyle>
        ///        </chart:CategoryAxis>
        ///     </chart:SfCartesianChart.XAxes>
        /// 
        /// </chart:SfCartesianChart>
        /// ]]>
        /// </code>
        /// # [MainPage.xaml.cs](#tab/tabid-10)
        /// <code><![CDATA[
        /// SfCartesianChart chart = new SfCartesianChart();
        /// 
        /// CategoryAxis xaxis = new CategoryAxis();
        /// xaxis.LabelStyle = new ChartAxisLabelStyle()
        /// {
        ///     TextColor = Colors.Red,
        ///     FontSize = 14,
        /// };
        /// chart.XAxes.Add(xaxis);
        ///
        /// ]]>
        /// </code>
        /// *** 
        /// </example> 
        public ChartAxisLabelStyle LabelStyle
        {
            get { return (ChartAxisLabelStyle)GetValue(LabelStyleProperty); }
            set { SetValue(LabelStyleProperty, value); }
        }

        /// <summary>
        /// Gets or sets the value to customize the appearance of the chart axis line.
        /// </summary>
        /// <value>This property accepts the <see cref="ChartLineStyle"/> value.</value>
        /// <remarks>
        /// To customize the axis line appearance, you need to create an instance of the <see cref="ChartLineStyle"/> class and set to the <see cref="AxisLineStyle"/> property.
        /// </remarks>
        /// <example>
        /// # [MainWindow.xaml](#tab/tabid-11)
        /// <code><![CDATA[
        ///     <chart:SfCartesianChart>
        ///
        ///           <chart:SfCartesianChart.XAxes>
        ///               <chart:NumericalAxis>
        ///                   <chart:NumericalAxis.AxisLineStyle>
        ///                       <chart:ChartLineStyle StrokeWidth="2" Stroke="Red"/>
        ///                   </chart:NumericalAxis.AxisLineStyle>
        ///               </chart:NumericalAxis>
        ///           </chart:SfCartesianChart.XAxes>
        ///
        ///           <chart:SfCartesianChart.YAxes>
        ///               <chart:NumericalAxis/>
        ///           </chart:SfCartesianChart.YAxes>
        ///
        ///     </chart:SfCartesianChart>
        /// ]]></code>
        /// # [MainWindow.cs](#tab/tabid-12)
        /// <code><![CDATA[
        ///     SfCartesianChart chart = new SfCartesianChart();
        ///     
        ///     NumericalAxis xAxis = new NumericalAxis();
        ///     ChartLineStyle axisLineStyle = new ChartLineStyle();
        ///     axisLineStyle.Stroke = SolidColorBrush.Red;
        ///     axisLineStyle.StrokeWidth = 2;
        ///     xAxis.AxisLineStyle = axisLineStyle;
        ///     chart.XAxes.Add(xAxis);
        ///     
        ///     NumericalAxis yAxis = new NumericalAxis();
        ///     chart.YAxes.Add(yAxis);
        /// ]]></code>
        /// ***
        /// </example>
        public ChartLineStyle AxisLineStyle
        {
            get { return (ChartLineStyle)GetValue(AxisLineStyleProperty); }
            set { SetValue(AxisLineStyleProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value that can be used to position an axis anywhere in the chart area. 
        /// </summary>
        /// <value>The default value is double.NaN.</value>
        /// <example>
        /// # [MainWindow.xaml](#tab/tabid-13)
        /// <code><![CDATA[
        ///     <chart:SfCartesianChart>
        ///
        ///           <chart:SfCartesianChart.XAxes>
        ///               <chart:NumericalAxis CrossesAt="{Static x:Double.MaxValue}"/>
        ///           </chart:SfCartesianChart.XAxes>
        ///
        ///           <chart:SfCartesianChart.YAxes>
        ///               <chart:NumericalAxis/>
        ///           </chart:SfCartesianChart.YAxes>
        ///
        ///     </chart:SfCartesianChart>
        /// ]]></code>
        /// # [MainWindow.cs](#tab/tabid-14)
        /// <code><![CDATA[
        ///     SfCartesianChart chart = new SfCartesianChart();
        ///     
        ///     NumericalAxis xAxis = new NumericalAxis();
        ///     xAxis.CrossesAt = double.MaxValue;
        ///     chart.XAxes.Add(xAxis);
        ///     
        ///     NumericalAxis yAxis = new NumericalAxis();
        ///     chart.YAxes.Add(yAxis);
        /// ]]></code>
        /// ***
        /// </example>
        public object CrossesAt
        {
            get { return (object)GetValue(CrossesAtProperty); }
            set { SetValue(CrossesAtProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value that determines whether the crossing axis should be placed at the crossing position or not.
        /// </summary>
        /// <value>It accepts bool values and the default value is <c>True</c>.</value>
        /// <example>
        /// # [MainWindow.xaml](#tab/tabid-13)
        /// <code><![CDATA[
        ///     <chart:SfCartesianChart>
        ///
        ///           <chart:SfCartesianChart.XAxes>
        ///               <chart:NumericalAxis RenderNextToCrossingValue="False"/>
        ///           </chart:SfCartesianChart.XAxes>
        ///
        ///           <chart:SfCartesianChart.YAxes>
        ///               <chart:NumericalAxis/>
        ///           </chart:SfCartesianChart.YAxes>
        ///
        ///     </chart:SfCartesianChart>
        /// ]]></code>
        /// # [MainWindow.cs](#tab/tabid-14)
        /// <code><![CDATA[
        ///     SfCartesianChart chart = new SfCartesianChart();
        ///     
        ///     NumericalAxis xAxis = new NumericalAxis();
        ///     xAxis.RenderNextToCrossingValue = false;
        ///     chart.XAxes.Add(xAxis);
        ///     
        ///     NumericalAxis yAxis = new NumericalAxis();
        ///     chart.YAxes.Add(yAxis);
        /// ]]></code>
        /// ***
        /// </example>
        public bool RenderNextToCrossingValue
        {
            get { return (bool)GetValue(RenderNextToCrossingValueProperty); }
            set { SetValue(RenderNextToCrossingValueProperty, value); }
        }

        /// <summary>
        /// Gets or sets the value for the CrossAxisName of chart axis.
        ///</summary>
        ///<value>It accepts string value and the defaul value is null.</value>
        /// <example>
        /// # [MainWindow.xaml](#tab/tabid-15)
        /// <code><![CDATA[
        ///     <chart:SfCartesianChart>
        ///
        ///           <chart:SfCartesianChart.XAxes>
        ///               <chart:NumericalAxis CrossesAt="5" CrossingAxisName="yAxis"/>
        ///           </chart:SfCartesianChart.XAxes>
        ///
        ///           <chart:SfCartesianChart.YAxes>
        ///               <chart:NumericalAxis Name="yAxis"/>
        ///           </chart:SfCartesianChart.YAxes>
        ///
        ///     </chart:SfCartesianChart>
        /// ]]></code>
        /// # [MainWindow.cs](#tab/tabid-16)
        /// <code><![CDATA[
        ///     SfCartesianChart chart = new SfCartesianChart();
        ///     
        ///     NumericalAxis yAxis = new NumericalAxis();
        ///     NumericalAxis.Name = "yAxis";
        ///     chart.YAxes.Add(yAxis);;
        ///     
        ///     NumericalAxis xAxis = new NumericalAxis();
        ///     xAxis.CrossesAt = 5;
        ///     xAxis.CrossingAxisName = "yAxis";
        ///     chart.XAxes.Add(xAxis);
        ///     
        /// ]]></code>
        /// ***
        /// </example>
        public string CrossAxisName
        {
            get { return (string)GetValue(CrossAxisNameProperty); }
            set { SetValue(CrossAxisNameProperty, value); }
        }

        /// <summary>
        /// Gets or sets the title for the chart axis.
        /// </summary>
        /// <remarks>The <see cref="ChartAxisTitle"/> provides options to customize the text and font of axis title.</remarks>
        /// <value>It accepts <see cref="ChartAxisTitle"/> value.</value>
        /// <example>
        /// # [MainPage.xaml](#tab/tabid-17)
        /// <code><![CDATA[
        /// <chart:SfCartesianChart>
        /// 
        ///     <chart:SfCartesianChart.XAxes>
        ///         <chart:CategoryAxis>
        ///            <chart:CategoryAxis.Title>
        ///                <chart:ChartAxisTitle Text="Category"/>
        ///            </chart:CategoryAxis.Title>
        ///        </chart:CategoryAxis>
        ///     </chart:SfCartesianChart.XAxes>
        /// 
        /// </chart:SfCartesianChart>
        /// ]]>
        /// </code>
        /// # [MainPage.xaml.cs](#tab/tabid-18)
        /// <code><![CDATA[
        /// SfCartesianChart chart = new SfCartesianChart();
        /// 
        /// CategoryAxis xaxis = new CategoryAxis();
        /// xaxis.Title = new ChartAxisTitle()
        /// {
        ///     Text = "Category"
        /// };
        /// chart.XAxes.Add(xaxis);
        ///
        /// ]]>
        /// </code>
        /// *** 
        /// </example> 
        public ChartAxisTitle Title
        {
            get { return (ChartAxisTitle)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        /// <summary>
        /// Gets or sets the value that indicates whether the axis' visible range is inversed.
        /// </summary>
        /// <value>It accepts the bool values and its default value is <c>False</c>.</value>
        /// <remarks>When the axis is inversed, it will render points from right to left for the horizontal axis, and top to bottom for the vertical axis.</remarks>
        /// <example>
        /// # [MainPage.xaml](#tab/tabid-19)
        /// <code><![CDATA[
        /// <chart:SfCartesianChart>
        ///  
        ///     <chart:SfCartesianChart.XAxes>
        ///         <chart:CategoryAxis IsInversed ="True" />
        ///     </chart:SfCartesianChart.XAxes>
        /// 
        /// </chart:SfCartesianChart>
        /// ]]>
        /// </code>
        /// # [MainPage.xaml.cs](#tab/tabid-20)
        /// <code><![CDATA[
        /// SfCartesianChart chart = new SfCartesianChart();
        /// 
        /// CategoryAxis xaxis = new CategoryAxis()
        /// {
        ///    IsInversed = true,
        /// };
        /// chart.XAxes.Add(xaxis);
        /// 
        /// ]]>
        /// </code>
        /// *** 
        /// </example>
        public bool IsInversed
        {
            get { return (bool)GetValue(IsInversedProperty); }
            set { SetValue(IsInversedProperty, value); }
        }

        /// <summary>
        ///  Gets or sets a value to customize the rendering position of the edge labels. 
        /// </summary>
        /// <value>It accepts the <see cref="Charts.EdgeLabelsDrawingMode"/> value and its default value is <see cref="EdgeLabelsDrawingMode.Center"/>.</value>
        /// <example>
        /// # [MainPage.xaml](#tab/tabid-21)
        /// <code><![CDATA[
        /// <chart:SfCartesianChart>
        ///  
        ///     <chart:SfCartesianChart.XAxes>
        ///         <chart:CategoryAxis EdgeLabelsDrawingMode="Fit" />
        ///     </chart:SfCartesianChart.XAxes>
        /// 
        /// </chart:SfCartesianChart>
        /// ]]>
        /// </code>
        /// # [MainPage.xaml.cs](#tab/tabid-22)
        /// <code><![CDATA[
        /// SfCartesianChart chart = new SfCartesianChart();
        /// 
        /// CategoryAxis xaxis = new CategoryAxis()
        /// {
        ///    EdgeLabelsDrawingMode= EdgeLabelsDrawingMode.Fit,
        /// };
        /// chart.XAxes.Add(xaxis);
        /// 
        /// ]]>
        /// </code>
        /// *** 
        /// </example>
        public EdgeLabelsDrawingMode EdgeLabelsDrawingMode
        {
            get { return (EdgeLabelsDrawingMode)GetValue(EdgeLabelsDrawingModeProperty); }
            set { SetValue(EdgeLabelsDrawingModeProperty, value); }
        }

        /// <summary>
        /// Gets or sets the <see cref="ChartLineStyle"/> to customize the appearance of the major grid lines.
        /// </summary>
        /// <value>It accepts the <see cref="ChartLineStyle"/>.</value>
        /// <example>
        /// # [MainWindow.xaml](#tab/tabid-23)
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
        ///               <chart:NumericalAxis>
        ///                   <chart:NumericalAxis.MajorGridLineStyle>
        ///                       <chart:ChartLineStyle StrokeDashArray="{StaticResource dashArray}" Stroke="Black" StrokeWidth="2" />
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
        /// # [MainWindow.cs](#tab/tabid-24)
        /// <code><![CDATA[
        ///     SfCartesianChart chart = new SfCartesianChart();
        ///     
        ///     DoubleCollection doubleCollection = new DoubleCollection();
        ///     doubleCollection.Add(3);
        ///     doubleCollection.Add(3);
        ///     
        ///     NumericalAxis xAxis = new NumericalAxis();
        ///     ChartLineStyle axisLineStyle = new ChartLineStyle();
        ///     axisLineStyle.Stroke = SolidColorBrush.Black;
        ///     axisLineStyle.StrokeWidth = 2;
        ///     axisLineStyle.StrokeDashArray = doubleCollection
        ///     xAxis.MajorGridLineStyle = axisLineStyle;
        ///     chart.XAxes.Add(xAxis);
        ///     
        ///     NumericalAxis yAxis = new NumericalAxis();
        ///     chart.YAxes.Add(yAxis);
        /// ]]></code>
        /// ***
        /// </example>
        public ChartLineStyle MajorGridLineStyle
        {
            get { return (ChartLineStyle)GetValue(MajorGridLineStyleProperty); }
            set { SetValue(MajorGridLineStyleProperty, value); }
        }

        /// <summary>
        /// Gets or sets the <see cref="ChartAxisTickStyle"/> to customize the appearance of the major tick lines.
        /// </summary>
        /// <value>It accepts the <see cref="ChartAxisTickStyle"/> value.</value>
        /// <example>
        /// # [MainWindow.xaml](#tab/tabid-25)
        /// <code><![CDATA[
        ///     <chart:SfCartesianChart>
        ///      
        ///           <chart:SfCartesianChart.XAxes>
        ///               <chart:NumericalAxis MinorTicksPerInterval="4">
        ///                   <chart:NumericalAxis.MajorTickStyle>
        ///                       <chart:ChartAxisTickStyle Stroke="Red" StrokeWidth="1"/>
        ///                   </chart:NumericalAxis.MajorTickStyle>
        ///               </chart:NumericalAxis>
        ///           </chart:SfCartesianChart.XAxes>
        ///
        ///           <chart:SfCartesianChart.YAxes>
        ///               <chart:NumericalAxis/>
        ///           </chart:SfCartesianChart.YAxes>
        ///
        ///     </chart:SfCartesianChart>
        /// ]]></code>
        /// # [MainWindow.cs](#tab/tabid-26)
        /// <code><![CDATA[
        ///     SfCartesianChart chart = new SfCartesianChart();
        ///     
        ///     NumericalAxis xAxis = new NumericalAxis();
        ///     xAxis.MajorTickStyle.StrokeWidth = 1;
        ///     xAxis.MajorTickStyle.Stroke = SolidColorBrush.Red;
        ///     chart.XAxes.Add(xAxis);
        ///     
        ///     NumericalAxis yAxis = new NumericalAxis();
        ///     chart.YAxes.Add(yAxis)
        /// ]]></code>
        /// ***
        /// </example>
        public ChartAxisTickStyle MajorTickStyle
        {
            get { return (ChartAxisTickStyle)GetValue(MajorTickStyleProperty); }
            set { SetValue(MajorTickStyleProperty, value); }
        }

        /// <summary>
        /// Gets or sets the value that defines the zoom position for the actual range of the axis.
        /// </summary>
        /// <remarks> The value must be between 0 and 1.</remarks>
        /// <value>It accepts the double values and its default value is 0.</value>
        /// <example>
        /// # [MainPage.xaml](#tab/tabid-27)
        /// <code><![CDATA[
        /// <chart:SfCartesianChart>
        ///
        ///         <chart:SfCartesianChart.XAxes>
        ///             <chart:CategoryAxis ZoomFactor="0.3" ZoomPosition="0.5" />
        ///         </chart:SfCartesianChart.XAxes>
        ///
        ///         <chart:SfCartesianChart.ZoomPanBehavior>
        ///              <chart:ChartZoomPanBehavior/>
        ///         </chart:SfCartesianChart.ZoomPanBehavior>
        /// 
        /// </chart:SfCartesianChart>
        /// ]]>
        /// </code>
        /// # [MainPage.xaml.cs](#tab/tabid-28)
        /// <code><![CDATA[
        /// SfCartesianChart chart = new SfCartesianChart();
        /// 
        /// CategoryAxis xaxis = new CategoryAxis(){  ZoomFactor = 0.3, ZoomPosition = 0.5  };
        /// chart.XAxes.Add(xaxis);
        /// 
        /// chart.ZoomPanBehavior = new ChartZoomPanBehavior();
        /// 
        /// ]]>
        /// </code>
        /// *** 
        /// </example>
        public double ZoomPosition
        {
            get { return (double)GetValue(ZoomPositionProperty); }
            set { SetValue(ZoomPositionProperty, value); }
        }

        /// <summary>
        /// Gets or sets the value that defines the percentage of the visible range from the total range of axis values.
        /// </summary>
        /// <remarks>The value must be between 0 and 1.</remarks>
        /// <value>It accepts the double values and its default value is 1.</value> 
        /// <summary>
        /// Gets or sets the value that defines the position for the actual range of the axis.
        /// </summary>
        /// <remarks> The value must be between 0 and 1.</remarks>
        /// <value>It accepts the double values and its default value is 0.</value>
        /// <example>
        /// # [MainPage.xaml](#tab/tabid-29)
        /// <code><![CDATA[
        /// <chart:SfCartesianChart>
        ///  
        ///         <chart:SfCartesianChart.XAxes>
        ///             <chart:CategoryAxis ZoomFactor="0.3" ZoomPosition="0.5" />
        ///         </chart:SfCartesianChart.XAxes>
        ///         
        ///         <chart:SfCartesianChart.ZoomPanBehavior>
        ///              <chart:ChartZoomPanBehavior/>
        ///         </chart:SfCartesianChart.ZoomPanBehavior>
        /// 
        /// </chart:SfCartesianChart>
        /// ]]>
        /// </code>
        /// # [MainPage.xaml.cs](#tab/tabid-30)
        /// <code><![CDATA[
        /// SfCartesianChart chart = new SfCartesianChart();
        /// 
        /// CategoryAxis xaxis = new CategoryAxis(){  ZoomFactor = 0.3, ZoomPosition = 0.5  };
        /// chart.XAxes.Add(xaxis);	
        /// 
        /// ChartZoomPanBehavior zoomPanBehavior = new ChartZoomPanBehavior();
        /// chart.ZoomPanBehavior = zoomPanBehavior;
        /// 
        /// ]]>
        /// </code>
        /// *** 
        /// </example>
        public double ZoomFactor
        {
            get { return (double)GetValue(ZoomFactorProperty); }
            set { SetValue(ZoomFactorProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the axis grid lines can be displayed or not.
        /// </summary>
        /// <value>It accepts the bool value and its default value is <c>True</c>.</value>
        /// <example>
        /// # [MainPage.xaml](#tab/tabid-31)
        /// <code><![CDATA[
        /// <chart:SfCartesianChart>
        ///  
        ///     <chart:SfCartesianChart.XAxes>
        ///         <chart:CategoryAxis ShowMajorGridLines = "False" />
        ///     </chart:SfCartesianChart.XAxes>
        /// 
        /// </chart:SfCartesianChart>
        /// ]]>
        /// </code>
        /// # [MainPage.xaml.cs](#tab/tabid-32)
        /// <code><![CDATA[
        /// SfCartesianChart chart = new SfCartesianChart();
        /// 
        /// CategoryAxis xaxis = new CategoryAxis()
        /// {
        ///    ShowMajorGridLines = false,
        /// };
        /// chart.XAxes.Add(xaxis);
        /// 
        /// ]]>
        /// </code>
        /// *** 
        /// </example>
        public bool ShowMajorGridLines
        {
            get { return (bool)GetValue(ShowMajorGridLinesProperty); }
            set { SetValue(ShowMajorGridLinesProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to calculate the axis intervals on zooming. 
        /// </summary>
        /// <value>It accepts the bool values and its default value is <c>True</c>.</value>
        /// <example>
        /// # [MainPage.xaml](#tab/tabid-33)
        /// <code><![CDATA[
        /// <chart:SfCartesianChart>
        ///  
        ///     <chart:SfCartesianChart.XAxes>
        ///         <chart:CategoryAxis EnableAutoIntervalOnZooming = "False" />
        ///     </chart:SfCartesianChart.XAxes>
        /// 
        /// </chart:SfCartesianChart>
        /// ]]>
        /// </code>
        /// # [MainPage.xaml.cs](#tab/tabid-34)
        /// <code><![CDATA[
        /// SfCartesianChart chart = new SfCartesianChart();
        /// 
        /// CategoryAxis xaxis = new CategoryAxis()
        /// {
        ///    EnableAutoIntervalOnZooming = false,
        /// };
        /// chart.XAxes.Add(xaxis);
        /// 
        /// ]]>
        /// </code>
        /// *** 
        /// </example>
        public bool EnableAutoIntervalOnZooming
        {
            get { return (bool)GetValue(EnableAutoIntervalOnZoomingProperty); }
            set { SetValue(EnableAutoIntervalOnZoomingProperty, value); }
        }

        /// <summary>
        /// This event occurs when the axis label is created.
        /// </summary>
        /// <remarks>The <see cref="ChartAxisLabelEventArgs"/> contains the information of AxisLabel.</remarks>
        /// <example>
        /// # [MainPage.xaml](#tab/tabid-35)
        /// <code><![CDATA[
        /// <chart:SfCartesianChart>
        ///  
        ///     <chart:SfCartesianChart.XAxes>
        ///         <chart:CategoryAxis LabelCreated="OnLabelCreated" />
        ///     </chart:SfCartesianChart.XAxes>
        /// 
        /// </chart:SfCartesianChart>
        /// ]]>
        /// </code>
        /// # [MainPage.xaml.cs](#tab/tabid-36)
        /// <code><![CDATA[
        /// SfCartesianChart chart = new SfCartesianChart();
        /// 
        /// CategoryAxis xaxis = new CategoryAxis();
        /// xaxis.LabelCreated += OnLabelCreated;
        /// chart.XAxes.Add(xaxis);
        /// 
        /// private void OnLabelCreated(object sender, ChartAxisLabelEventArgs e)
        /// {
        ///      // You can customize the content of the axis label.
        /// }
        /// 
        /// ]]>
        /// </code>
        /// *** 
        /// </example>
        public event EventHandler<ChartAxisLabelEventArgs>? LabelCreated;

        /// <summary>
        /// This event occurs when the actual range is changed.
        /// </summary>
        /// <remarks>
        /// The <see cref="ActualRangeChangedEventArgs"/> contains information on the chart axis' minimum and maximum values.
        /// </remarks>
        /// <example>
        /// # [MainWindow.xaml](#tab/tabid-37)
        /// <code><![CDATA[
        ///     <chart:SfCartesianChart>
        ///
        ///           <chart:SfCartesianChart.XAxes>
        ///               <chart:NumericalAxis ActualRangeChanged="xAxis_ActualRangeChanged"/>
        ///           </chart:SfCartesianChart.XAxes>
        ///
        ///           <chart:SfCartesianChart.YAxes>
        ///               <chart:NumericalAxis/>
        ///           </chart:SfCartesianChart.YAxes>
        ///
        ///     </chart:SfCartesianChart>
        /// ]]></code>
        /// # [MainWindow.cs](#tab/tabid-38)
        /// <code><![CDATA[
        ///     SfCartesianChart chart = new SfCartesianChart();
        ///     
        ///     NumericalAxis xAxis = new NumericalAxis();
        ///     NumericalAxis yAxis = new NumericalAxis();
        ///     
        ///     chart.XAxes.Add(xAxis);
        ///     chart.YAxes.Add(yAxis);
        ///     
        ///     xAxis.ActualRangeChanged += xAxis_ActualRangeChanged;
        ///     
        ///     private void xAxis_ActualRangeChanged(object sender, Syncfusion.Maui.Charts.ActualRangeChangedEventArgs e)
        ///     {
        ///         e.VisibleMinimum = 2;
        ///         e.VisibleMaximum = 5;
        ///         e.ActualMinimum = 0;
        ///         e.ActualMaximum = 8;
        ///     } 
        /// ]]></code>
        /// ***
        /// </example>
        public event EventHandler<ActualRangeChangedEventArgs>? ActualRangeChanged;

        /// <summary>
        /// Gets or sets the value that indicates whether to show the trackball axis label.
        /// </summary>
        /// <value>It accepts the bool values and its default value is <c>True</c>.</value>
        /// <example>
        /// # [MainPage.xaml](#tab/tabid-33)
        /// <code><![CDATA[
        /// <chart:SfCartesianChart>
        ///  
        ///     <chart:SfCartesianChart.XAxes>
        ///         <chart:CategoryAxis ShowTrackballLabel = "True" />
        ///     </chart:SfCartesianChart.XAxes>
        /// 
        /// </chart:SfCartesianChart>
        /// ]]>
        /// </code>
        /// # [MainPage.xaml.cs](#tab/tabid-34)
        /// <code><![CDATA[
        /// SfCartesianChart chart = new SfCartesianChart();
        /// 
        /// CategoryAxis xaxis = new CategoryAxis()
        /// {
        ///    ShowTrackballLabel = true,
        /// };
        /// chart.XAxes.Add(xaxis);
        /// 
        /// ]]>
        /// </code>
        /// *** 
        /// </example>
        public bool ShowTrackballLabel
        {
            get { return (bool)GetValue(ShowTrackballLabelProperty); }
            set { SetValue(ShowTrackballLabelProperty, value); }
        }

        /// <summary>
        /// Gets or sets option for customize the trackball axis label.
        /// </summary>
        /// <value>It accepts the bool values and its default value is <c>True</c>.</value>
        /// <example>
        /// # [MainPage.xaml](#tab/tabid-33)
        /// <code><![CDATA[
        /// <chart:SfCartesianChart>
        ///  
        ///     <chart:SfCartesianChart.XAxes>
        ///         <chart:CategoryAxis ShowTrackballLabel="True">
        ///            <chart:CategoryAxis.TrackballLabelStyle>
        ///                <chart:ChartLabelStyle Background = "Black" TextColor="White"/>
        ///           </chart:CategoryAxis.TrackballLabelStyle>
        ///        </chart:CategoryAxis>
        ///     </chart:SfCartesianChart.XAxes>
        /// 
        /// </chart:SfCartesianChart>
        /// ]]>
        /// </code>
        /// # [MainPage.xaml.cs](#tab/tabid-34)
        /// <code><![CDATA[
        /// SfCartesianChart chart = new SfCartesianChart();
        /// var trackballLabelStyle = new ChartLabelStyle()
        /// { 
        ///     Background = new SolidColorBrush(Colors.Black),
        ///     TextColor = Colors.White,
        /// };
        /// CategoryAxis xaxis = new CategoryAxis()
        /// {
        ///    ShowTrackballLabel = true,
        ///    TrackballLabelStyle = trackballLabelStyle,
        /// };
        /// chart.XAxes.Add(xaxis);
        /// 
        /// ]]>
        /// </code>
        /// *** 
        /// </example>
        public ChartLabelStyle TrackballLabelStyle
        {
            get { return (ChartLabelStyle)GetValue(TrackballLabelStyleProperty); }
            set { SetValue(TrackballLabelStyleProperty, value); }
        }

        #region Internal Properties

        //Todo: Need to provide support for next release.

        /// <summary>
        /// Gets or sets an actions to be taken when two labels intersect in bounds. 
        /// </summary>
        /// <value>This property takes the <see cref="AxisLabelsIntersectAction"/> as its value.</value>
        internal AxisLabelsIntersectAction LabelsIntersectAction
        {
            get { return (AxisLabelsIntersectAction)GetValue(LabelsIntersectActionProperty); }
            set { SetValue(LabelsIntersectActionProperty, value); }
        }

        /// <summary>
        /// Gets or sets the mode for automatic scrolling of the chart when new data points are added.
        /// </summary>
        /// <remarks>
        /// By default, the mode is set to <see cref="ChartAutoScrollingMode.End"/>, which means that the chart will always display the most recent data points, and the horizontal axis will be automatically scrolled to the right to show the new data. 
        /// </remarks>
        /// <example>
        /// # [MainPage.xaml](#tab/tabid-7)
        /// <code><![CDATA[
        /// <chart:SfCartesianChart>
        ///    <chart:SfCartesianChart.XAxes>
        ///        <chart:CategoryAxis AutoScrollingMode="Start"/>
        ///    </chart:SfCartesianChart.XAxes>
        /// </chart:SfCartesianChart>
        /// ]]>
        /// </code>
        /// # [MainPage.xaml.cs](#tab/tabid-8)
        /// <code><![CDATA[
        /// SfCartesianChart chart = new SfCartesianChart();
        /// 
        /// CategoryAxis xAxis = new CategoryAxis();
        /// xAxis.AutoScrollingMode = ChartAutoScrollingMode.Start;
        /// 
        /// chart.XAxes.Add(xAxis);	
        /// chart.YAxes.Add(yAxis);	
        /// ]]>
        /// </code>
        /// ***
        /// </example>
        public ChartAutoScrollingMode AutoScrollingMode
        {
            get { return (ChartAutoScrollingMode)GetValue(AutoScrollingModeProperty); }
            set { SetValue(AutoScrollingModeProperty, value); }
        }

        /// <summary>
		/// Gets or sets the value that determines the distance between the axis label and axis title. 
        /// </summary>
        /// <value>This property take double value.</value>
        /// TODO: Validating for remove support.
        internal double LabelExtent
        {
            get { return (double)GetValue(LabelExtentProperty); }
            set { SetValue(LabelExtentProperty, value); }
        }

        /// <summary>
        /// Gets or sets the collection of the ChartAxisRangeStyle to customize the axis GridLine, TickLine and LabelStyle for specific range. 
        /// </summary>
        /// <value>This property takes the <see cref="ChartAxisRangeStyleCollection"/> as its value.</value>
        internal ChartAxisRangeStyleCollection RangeStyles
        {
            get { return (ChartAxisRangeStyleCollection)GetValue(RangeStylesProperty); }
            set { SetValue(RangeStylesProperty, value); }
        }

        /// <summary>
		/// Gets or sets the unique name of the axis, which will be used to identify the segment axis of the strip line. 
        /// </summary>
        /// <value>This property takes the string value.</value>
        public string Name
        {
            get { return (string)GetValue(NameProperty); }
            set { SetValue(NameProperty, value); }
        }

        /// <summary>
        /// Gets or sets the delta value that the specified range of data is always visible in the chart.
        /// </summary>
        /// <remarks>
        /// It represents the amount of range that the chart will scroll by when new data points are added.
        /// </remarks>
        /// <value>Its default is <c>double.NaN</c>.</value>
        /// <example>
        /// # [MainPage.xaml](#tab/tabid-7)
        /// <code><![CDATA[
        /// <chart:SfCartesianChart>
        ///    <chart:SfCartesianChart.XAxes>
        ///        <chart:CategoryAxis AutoScrollingDelta="3"/>
        ///    </chart:SfCartesianChart.XAxes>
        ///    <chart:SfCartesianChart.YAxes>
        ///        <chart:LogarithmicAxis AutoScrollingDelta="2"/>
        ///    </chart:SfCartesianChart.YAxes>
        /// </chart:SfCartesianChart>
        /// ]]>
        /// </code>
        /// # [MainPage.xaml.cs](#tab/tabid-8)
        /// <code><![CDATA[
        /// SfCartesianChart chart = new SfCartesianChart();
        /// 
        /// CategoryAxis xAxis = new CategoryAxis();
        /// xAxis.AutoScrollingDelta = 3;
        /// 
        /// LogarithmicAxis yAxis = new LogarithmicAxis();
        /// yaxis.AutoScrollingDelta = 2;
        /// 
        /// chart.XAxes.Add(xAxis);	
        /// chart.YAxes.Add(yAxis);	
        /// ]]>
        /// </code>
        /// ***
        /// </example>
        public double AutoScrollingDelta
        {
            get { return (double)GetValue(AutoScrollingDeltaProperty); }
            set { SetValue(AutoScrollingDeltaProperty, value); }
        }

        /// <summary>
		/// Gets or sets the position of the axis tick lines. 
        /// </summary>
        /// <value>This property takes the <see cref="AxisElementPosition"/> as its value.</value>
        internal AxisElementPosition TickPosition
        {
            get { return (AxisElementPosition)GetValue(TickPositionProperty); }
            set { SetValue(TickPositionProperty, value); }
        }

        /// <summary>
		/// Gets or sets the value that determines the number of labels to be displayed per 100 pixels. 
        /// </summary>
        /// <value>This property takes the integer value.</value>
        /// <remarks>This property used to give constrain over the auto generated labels, which reduces the number elements rendering in view.</remarks>
        ///TODO: Provide alter way for this support. 
        internal int MaximumLabels
        {
            get { return (int)GetValue(MaximumLabelsProperty); }
            set { SetValue(MaximumLabelsProperty, value); }
        }

        #endregion

        #endregion

        #region Internal Properties

        internal double ActualCrossingValue { get; set; } = double.NaN;

        internal CartesianAxisLabelsRenderer? AxisLabelsRenderer { get; set; }

        internal CartesianAxisElementRenderer? AxisElementRenderer { get; set; }

        internal CartesianAxisRenderer? AxisRenderer { get; set; }

        internal List<CartesianSeries> RegisteredSeries { get; set; }

        internal int SideBySideSeriesCount { get; set; }


        internal bool IsOpposed()
        {
            var crossAxis = GetCrossingAxis(cartesianArea);
            if (crossAxis != null)
            {
                var isInversedAxis = crossAxis.IsInversed;
                return ActualCrossingValue == double.MaxValue && !isInversedAxis ||
                    ((double.IsNaN(ActualCrossingValue) || ActualCrossingValue == double.MinValue) && isInversedAxis);
            }

            return false;
        }
        #endregion

        #region Methods

        #region Internal Methods
        /// <summary>
        /// Method used to get exact value in hit point.
        /// </summary>
        /// <param name="point">The point.</param>
        /// <returns>The value.</returns>
        internal float PointToValue(PointF point)
        {
            if (point == PointF.Zero)
            {
                return float.NaN;
            }

            return (float)PointToValue(point.X, point.Y);
        }

        /// <summary>
        /// Calculate axis desired size.
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        internal void ComputeSize(Size size)
        {
            AvailableSize = size;

            var plotSize = GetPlotSize(size);

            CalculateRangeAndInterval(plotSize);
            if (this.IsVisible)
            {
                UpdateRenderers();
                UpdateLabels(); //Generate visible labels
                ComputedDesiredSize = ComputeDesiredSize(size);
            }
            else
            {
                //TODO: Need to validate desired size.
                UpdateLabels(); //Generate visible labels
                ActualPlotOffsetStart = double.IsNaN(PlotOffsetStart) ? 0f : PlotOffsetStart;
                ActualPlotOffsetEnd = double.IsNaN(PlotOffsetEnd) ? 0f : PlotOffsetEnd;
                ActualPlotOffset = ActualPlotOffsetStart + ActualPlotOffsetEnd;
                InsidePadding = 0;
                AxisRenderer = null;
                ComputedDesiredSize = !IsVertical ? new Size(size.Width, 0) : new Size(0, size.Height);
            }
        }

        internal bool RenderRectContains(float x, float y)
        {
            Rect rect;
            var labelsRect = AxisLabelsRenderer?.LabelLayout?.LabelsRect;

            if (labelsRect == null || labelsRect.Count == 0)
            {
                return false;
            }

            int count = labelsRect.Count;

            if (IsVertical)
            {
                rect = new Rect(ArrangeRect.Left, ArrangeRect.Top - (labelsRect[count - 1].Height / 2), ArrangeRect.Width, ArrangeRect.Height + (labelsRect[count - 1].Height / 2) + (labelsRect[0].Height / 2));
            }
            else
            {
                rect = new Rect(ArrangeRect.Left - (labelsRect[0].Width / 2), ArrangeRect.Top, ArrangeRect.Width + (labelsRect[0].Width / 2) + (labelsRect[count - 1].Width / 2), ArrangeRect.Height);
            }

            if (LabelStyle.LabelsPosition == AxisElementPosition.Inside)
            {
                var isOpposed = IsOpposed();
                if (IsVertical && !isOpposed)
                {
                    rect.Width += InsidePadding;
                }
                else if (!IsVertical && isOpposed)
                {
                    rect.Height += InsidePadding;
                }
            }

            return rect.Contains(x, y);
        }

        internal void UpdateLayout()
        {
            if (Area != null)
            {
                Area.NeedsRelayout = true;
                Area.ScheduleUpdateArea();
            }
        }

        internal bool CanRenderNextToCrossingValue()
        {
            return RenderNextToCrossingValue
                && !double.IsNaN(ActualCrossingValue)
                && ActualCrossingValue != double.MinValue
                && ActualCrossingValue != double.MaxValue;
        }

        internal ChartAxis? GetCrossingAxis(CartesianChartArea? area)
        {
            if (area == null) return null;

            if (CrossAxisName != null && CrossAxisName.Length != 0)
            {
                var axislayout = area.AxisLayout;
                var axes = IsVertical ? axislayout.HorizontalAxes : axislayout.VerticalAxes;
                foreach (var axis in axes)
                {
                    if (axis.Name != null && axis.Name.Equals(CrossAxisName))
                    {
                        return axis;
                    }
                }
            }

            if (AssociatedAxes.Count > 0)
            {
                return AssociatedAxes[0];
            }
            else
            {
                return isVertical ^ (RegisteredSeries.Count > 0 && area.IsTransposed)
                                                ? area.PrimaryAxis
                                                    : area.SecondaryAxis;
            }
        }

        internal bool CanDrawMajorGridLines()
        {
            return ShowMajorGridLines && MajorGridLineStyle.CanDraw();
        }

        //Axes change on register series change. 
        internal void AddRegisteredSeries(CartesianSeries series)
        {
            if (!RegisteredSeries.Contains(series))
            {
                RegisteredSeries.Add(series);
            }
        }

        internal void ClearRegisteredSeries()
        {
            if (Area != null)
            {
                RegisteredSeries.Clear();
                AssociatedAxes.Clear();
            }
        }

        internal void RemoveRegisteredSeries(CartesianSeries series)
        {
            RegisteredSeries.Remove(series);
            if (series != null)
            {
                var xAxis = series.ActualXAxis;
                var area = cartesianArea;
                if (area != null && xAxis != null && series.ActualYAxis is RangeAxisBase yAxis)
                {
                    if (!area.YAxes.Contains(yAxis))
                    {
                        xAxis.AssociatedAxes.Remove(yAxis);
                    }

                    if (!area.XAxes.Contains(xAxis))
                    {
                        yAxis.AssociatedAxes.Remove(xAxis);
                    }
                }
            }
        }

        internal virtual void Dispose()
        {
            if (Title != null)
            {
                Title.Axis = null;
            }

            if (AxisLineStyle != null)
            {
                AxisLineStyle.PropertyChanged -= Style_PropertyChanged;
            }

            if (LabelStyle != null)
            {
                LabelStyle.PropertyChanged -= Style_PropertyChanged;
            }

            if (MajorGridLineStyle != null)
            {
                MajorGridLineStyle.PropertyChanged -= Style_PropertyChanged;
            }

            if (MajorTickStyle != null)
            {
                MajorTickStyle.PropertyChanged -= Style_PropertyChanged;
            }

            if (RegisteredSeries != null)
            {
                RegisteredSeries.Clear();
            }

            if (AssociatedAxes != null)
            {
                AssociatedAxes.Clear();
            }

            AxisLabelsRenderer = null;
            AxisElementRenderer = null;
            Area = null;
        }

        /// <summary>
        /// 
        /// </summary>
        protected virtual Size ComputeDesiredSize(Size availableSize)
        {
            if (AxisRenderer != null)
                return AxisRenderer.ComputeDesiredSize(availableSize);
            return availableSize;
        }

		#endregion

		#region Protected Methods
		/// <summary>
		/// 
		/// </summary>
		/// <param name="canvas"></param>
		/// <param name="arrangeRect"></param>
		protected internal virtual void DrawAxis(ICanvas canvas, Rect arrangeRect)
        {
            if (AxisRenderer != null)
            {
                UpdateLayoutTranslate(canvas, AxisRenderer, arrangeRect);
                AxisRenderer.OnDraw(canvas);
            }
        }

        /// <summary>
        /// Translate the axis position to edge of series clip when axis crosses enable. 
        /// </summary>
        private void UpdateLayoutTranslate(ICanvas canvas, CartesianAxisRenderer axisRenderer, Rect arrangeRect)
        {
            var axisArrangeRect = new Rect(arrangeRect.Location, arrangeRect.Size);
            if (CanRenderNextToCrossingValue() && cartesianArea != null)
            {
                Rect clip = cartesianArea.ActualSeriesClipRect;

                if (IsVertical)
                {
                    if (axisArrangeRect.Left < clip.Left)
                    {
                        axisArrangeRect.Left = clip.Left;
                    }

                    if (axisArrangeRect.Right > clip.Right)
                    {
                        axisArrangeRect.Left = clip.Right - arrangeRect.Width;
                    }
                }
                else
                {
                    if (axisArrangeRect.Top < clip.Top)
                    {
                        axisArrangeRect.Top = clip.Top;
                    }

                    if (axisArrangeRect.Bottom > clip.Bottom)
                    {
                        axisArrangeRect.Top = clip.Bottom - arrangeRect.Height;
                    }
                }
            }

            if (axisArrangeRect != arrangeRect)
            {
                //Hide line and ticks when axis tries to render out side of series clip.
                axisRenderer.UpdateRendererVisible(false);
                canvas.Translate((float)axisArrangeRect.Left, (float)axisArrangeRect.Top);
            }
            else
            {
                axisRenderer.UpdateRendererVisible(true);
                canvas.Translate((float)arrangeRect.Left, (float)arrangeRect.Top);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        protected internal virtual void DrawMajorTick(ICanvas canvas, double tickPosition, PointF point1, PointF point2)
        {
            canvas.DrawLine(point1, point2);
        }

        /// <summary>
        /// 
        /// </summary>
        protected internal virtual void DrawMinorTick(ICanvas canvas, double tickPosition, PointF point1, PointF point2)
        {
            canvas.DrawLine(point1, point2);
        }

        /// <summary>
        /// 
        /// </summary>
        protected internal virtual void DrawAxisLine(ICanvas canvas, float x1, float y1, float x2, float y2)
        {
            canvas.DrawLine(x1, y1, x2, y2);
        }

        /// <summary>
        /// 
        /// </summary>
        protected internal virtual void DrawGridLine(ICanvas canvas, double position, float x1, float y1, float x2, float y2)
        {
            canvas.DrawLine(x1, y1, x2, y2);
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();

            if (AxisLineStyle != null)
            {
                SetInheritedBindingContext(AxisLineStyle, BindingContext);
            }

            if (MajorGridLineStyle != null)
            {
                SetInheritedBindingContext(MajorGridLineStyle, BindingContext);
            }

            if (MajorTickStyle != null)
            {
                SetInheritedBindingContext(MajorTickStyle, BindingContext);
            }

            if (LabelStyle != null)
            {
                SetInheritedBindingContext(LabelStyle, BindingContext);
            }

            if (Title != null)
            {
                SetInheritedBindingContext(Title, BindingContext);
            }
        }

    #endregion

        #region Private Methods

        private static void OnShowMajorGridLinesChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var axis = bindable as ChartAxis;
            if (axis != null)
            {
                axis.UpdateLayout();
            }
        }

        private static void OnMajorTickStyleChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var axis = bindable as ChartAxis;
            if (axis == null)
            {
                return;
            }

            if (oldValue != null)
            {
                var style = newValue as ChartAxisTickStyle;
                if (style != null)
                {
                    style.PropertyChanged -= axis.Style_PropertyChanged;
                }
            }

            if (newValue != null)
            {
                var tickStyle = newValue as ChartAxisTickStyle;
                if (tickStyle != null)
                {
                    SetInheritedBindingContext(tickStyle, axis.BindingContext);
                    tickStyle.PropertyChanged += axis.Style_PropertyChanged; ;
                }
            }

            axis?.UpdateLayout();
        }

        private static void OnMajorGridLineStyleChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var axis = bindable as ChartAxis;
            if(axis == null)
            {
                return;
            }

            if (oldValue != null)
            {
                var style = newValue as ChartLineStyle;
                if (style != null)
                {
                    style.PropertyChanged -= axis.Style_PropertyChanged;
                }
            }

            if (newValue != null)
            {
                var tickStyle = newValue as ChartLineStyle;
                if (tickStyle != null)
                {
                    SetInheritedBindingContext(tickStyle, axis.BindingContext);
                    tickStyle.PropertyChanged += axis.Style_PropertyChanged;
                }
            }

            axis?.UpdateLayout();
        }

        private static void OnEdgeLabelsDrawingModeChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var axis = bindable as ChartAxis;
            if (axis != null)
            {
                axis.UpdateLayout();
            }
        }

        private static void OnInversedChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var axis = bindable as ChartAxis;
            if (axis != null)
            {
                axis.UpdateLayout();
            }
        }

        private static void OnTitleChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var axis = bindable as ChartAxis;
            if (axis != null)
            {
                var title = newValue as ChartAxisTitle;
                if (title != null)
                {
                    SetInheritedBindingContext(title, axis.BindingContext);
                    axis.InitTitle(title);
                }
            }
        }

        private static void OnCrossesChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var axis = bindable as ChartAxis;
            if (axis != null && newValue != null)
            {
                axis.ActualCrossingValue = ChartUtils.ConvertToDouble(newValue);
                axis.UpdateLayout();
            }
        }

        private static void OnRenderNextToCrossingValuePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var axis = bindable as ChartAxis;
            if (axis != null)
            {
                axis.UpdateLayout();
            }
        }

        private static void OnCrossAxisNameChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var axis = bindable as ChartAxis;
            if (axis != null)
            {
                axis.UpdateLayout();
            }
        }

        private static void OnAxisLineStyleChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var axis = bindable as ChartAxis;
            if (axis == null)
            {
                return;
            }

            if (oldValue != null)
            {
                var style = newValue as ChartLineStyle;
                if (style != null)
                {
                    style.PropertyChanged -= axis.Style_PropertyChanged;
                }
            }

            if (newValue != null)
            {
                var lineStyle = newValue as ChartLineStyle;
                if (lineStyle != null)
                {
                    SetInheritedBindingContext(lineStyle, axis.BindingContext);
                    lineStyle.PropertyChanged += axis.Style_PropertyChanged;
                }
            }

            axis?.UpdateLayout();
        }

        private static void OnRangeStylesPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if(oldValue != null)
            {
                //TODO: Unhook collection changed
            }

            if (newValue != null)
            {
                //TODO: Hook collection changed.
            }

            (bindable as ChartAxis)?.UpdateLayout();
        }

        private static void OnLabelStyleChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var axis = bindable as ChartAxis;
            if (axis == null)
            {
                return;
            }

            if (oldValue != null)
            {
                var style = newValue as ChartAxisLabelStyle;
                if (style != null)
                {
                    style.PropertyChanged -= axis.Style_PropertyChanged;
                }
            }

            if (newValue != null && axis != null)
            {
                var style = newValue as ChartAxisLabelStyle;
                if (style != null)
                {
                    SetInheritedBindingContext(style, axis.BindingContext);
                    style.PropertyChanged += axis.Style_PropertyChanged;
                }
            }

            axis?.UpdateLayout();
        }

        private static void OnLabelRotationAngleChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var axis = bindable as ChartAxis;
            if (axis != null)
            {
                axis.UpdateLayout();
            }
        }

        private static void OnLabelExtentChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var axis = bindable as ChartAxis;
            if (axis != null)
            {
                axis.UpdateLayout();
            }
        }

        private static void OnAutoScrollingModeChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var axis = bindable as ChartAxis;
            if (axis != null)
            {
                axis.CanAutoScroll = true;
                axis.UpdateLayout();
            }
        }

        private static void OnLabelsIntersectActionChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var axis = bindable as ChartAxis;
            if (axis != null)
            {
                axis.UpdateLayout();
            }
        }

        private static void OnAxisLineOffsetChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var axis = bindable as ChartAxis;
            if (axis != null)
            {
                axis.UpdateLayout();
            }
        }

        private static void OnPlotOffsetStartChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var axis = bindable as ChartAxis;
            if (axis != null)
            {
                axis.UpdateActualPlotOffsetStart((double)newValue);
                axis.UpdateLayout();
            }
        }

        private static void OnPlotOffsetEndChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var axis = bindable as ChartAxis;
            if (axis != null)
            {
                axis.UpdateActualPlotOffsetEnd((double)newValue);
                axis.UpdateLayout();
            }
        }

        private static void OnIsVisibleChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var axis = bindable as ChartAxis;
            if (axis != null)
            {
                axis.UpdateLayout();
            }
        }

        private static void OnMaximumLabelsChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var axis = bindable as ChartAxis;
            if (axis != null)
            {
                axis.UpdateLayout();
            }
        }

        private static void OnTickPositionChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var axis = bindable as ChartAxis;
            if (axis != null)
            {
                axis.UpdateLayout();
            }
        }

        private static void OnAutoScrollingDeltaPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is ChartAxis axis && newValue != null)
            {
                var delta = Convert.ToDouble(newValue ?? double.NaN);
                bool needAutoScroll = !double.IsNaN(delta) && delta > 0;
                if (needAutoScroll)
                {
                    axis.ActualAutoScrollDelta = delta;
                    axis.CanAutoScroll = true;
                    axis.UpdateLayout();
                }
            }
        }

        private static void OnNameChanged(BindableObject bindable, object oldValue, object newValue)
        {
        }

        private static void OnEnableAutoIntervalOnZoomingPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
        }

        private static void OnZoomFactorChanged(BindableObject bindable, object oldValue, object newValue)
        {
            //TODO: zoom factor need to set between 0 to 1
            var axis = bindable as ChartAxis;
            if (axis != null)
            {

                var oldFactor = oldValue == null ? 1 : (double)oldValue;
                var newFactor = newValue == null ? 1 : (double)newValue > 1 ? 1 : (double)newValue < 0 ? 0 : (double)newValue;

                if (oldFactor != newFactor)
                {
                    //TODO: Set new factor to actual value;
                    axis.UpdateLayout();
                }
            }
        }

        private static void OnZoomPositionChanged(BindableObject bindable, object oldValue, object newValue)
        {
            //TODO: Zoompostion need to set between 0 to 1
            var axis = bindable as ChartAxis;
            if (axis != null)
            {

                var oldPosition = oldValue == null ? 1 : (double)oldValue;
                var newPosition = newValue == null ? 1 : (double)newValue > 1 ? 1 : (double)newValue < 0 ? 0 : (double)newValue;

                if (oldPosition != newPosition)
                {
                    //TODO: Set new position to actual value;
                    axis.UpdateLayout();
                }
            }
        }

        private void Style_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            this.UpdateLayout();
        }

        private void InitializeConstructor()
        {
            //Todo: Remove this code, After ClipsToBounds works in iOS and Windows.
            EdgeLabelsDrawingMode = EdgeLabelsDrawingMode.Shift;
            AxisLineStyle = new ChartLineStyle();
            LabelStyle = new ChartAxisLabelStyle();
            MajorGridLineStyle = new ChartLineStyle();
            MajorGridLineStyle.Stroke = new SolidColorBrush(Color.FromArgb("#EDEFF1"));
            MajorTickStyle = new ChartAxisTickStyle();
        }

        private void UpdateRenderers()
        {
            if (AxisLabelsRenderer == null)
            {
                AxisLabelsRenderer = new CartesianAxisLabelsRenderer(this);
            }

            if (AxisElementRenderer == null)
            {
                AxisElementRenderer = new CartesianAxisElementRenderer(this);
            }

            if (AxisRenderer != null)
            {
                AxisRenderer.LayoutCalculators.Clear();
            }
            else
            {
                AxisRenderer = new CartesianAxisRenderer(this);
            }

            AxisRenderer.LayoutCalculators.Add(AxisLabelsRenderer);
            AxisRenderer.LayoutCalculators.Add(AxisElementRenderer);
        }

        private Size GetPlotSize(Size availableSize)
        {
            if (!IsVertical)
            {
                return new Size(availableSize.Width - GetActualPlotOffset(), availableSize.Height);
            }
            else
            {
                return new Size(availableSize.Width, availableSize.Height - GetActualPlotOffset());
            }
        }

        private void InitTitle(ChartAxisTitle title)
        {
            title.Axis = this;
        }

        private void UpdateAxisLayout()
        {
            if (AxisRenderer != null)
            {
                AxisRenderer.Layout(new Size(arrangeRect.Width, arrangeRect.Height));
            }
        }

        #endregion

        #endregion
    }
}
