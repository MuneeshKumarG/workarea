

using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using Syncfusion.Maui.Core;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Syncfusion.Maui.Charts
{
    /// <summary>
    /// Represents the legend for the <see cref="SfCartesianChart"/>, <see cref="SfCircularChart"/>, <see cref="SfFunnelChart"/> and <see cref="SfPyramidChart"/> classes.
    /// </summary>
    /// <remarks>
    /// The items in the legend contain the key information about the <see cref="ChartSeries"/>. The legend has all abilities such as docking, enabling, or disabling the desired series.
    ///</remarks>
    /// <example>
    /// # [Xaml](#tab/tabid-1)
    /// <code><![CDATA[
    ///     <chart:SfCartesianChart>
    ///
    ///           <chart:SfCartesianChart.Legend>
    ///               <chart:ChartLegend/>
    ///           </chart:SfCartesianChart.Legend>
    ///           
    ///     </chart:SfCartesianChart>
    /// ]]></code>
    /// # [C#](#tab/tabid-2)
    /// <code><![CDATA[
    ///     SfCartesianChart chart = new SfCartesianChart();
    ///     chart.Legend = new ChartLegend();
    ///     
    /// ]]></code>
    /// ***
    /// </example>
    public class ChartLegend : BindableObject, ILegend
    {
        #region Bindable Properties

        /// <summary>
        /// Identifies the <see cref="Placement"/> bindable property. 
        /// </summary>        
        public static readonly BindableProperty PlacementProperty = BindableProperty.Create(nameof(Placement), typeof(LegendPlacement), typeof(ChartLegend), LegendPlacement.Top, BindingMode.Default, null, null, null, null);

        /// <summary>
        /// Identifies the <see cref="ToggleSeriesVisibility"/> bindable property. 
        /// </summary>        
        public static readonly BindableProperty ToggleSeriesVisibilityProperty = BindableProperty.Create(nameof(ToggleSeriesVisibility), typeof(bool), typeof(ChartLegend), false, BindingMode.Default, null, null, null, null);

        /// <summary>
        /// Identifies the <see cref="ItemsLayout"/> bindable property. 
        /// </summary>        
        internal static readonly BindableProperty ItemsLayoutProperty = BindableProperty.Create(nameof(ItemsLayout), typeof(IItemsLayout), typeof(ChartLegend), null, BindingMode.Default, null, null, null, null);

        /// <summary>
        /// Identifies the <see cref="ItemMargin"/> bindable property. 
        /// </summary>        
        internal static readonly BindableProperty ItemMarginProperty = BindableProperty.Create(nameof(ItemMargin), typeof(Thickness), typeof(ChartLegend), new Thickness(double.NaN), BindingMode.Default, null, null, null, null);

        /// <summary>
        /// Identifies the <see cref="ItemTemplate"/> bindable property. 
        /// </summary>
        public static readonly BindableProperty ItemTemplateProperty = BindableProperty.Create(nameof(ItemTemplate), typeof(DataTemplate), typeof(ChartLegend), default(DataTemplate));

        /// <summary>
        /// Identifies the <see cref="IsVisible"/> bindable property. 
        /// </summary>        
        public static readonly BindableProperty IsVisibleProperty = BindableProperty.Create(nameof(IsVisible), typeof(bool), typeof(ChartLegend), true, BindingMode.Default, null, null, null, null);

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets a value that indicates whether the legend is visible or not.
        /// </summary>
        /// <value>It accepts bool values and the default value is <c>True</c></value>
        /// <example>
        /// # [MainPage.xaml](#tab/tabid-3)
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
        /// # [MainPage.xaml.cs](#tab/tabid-4)
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
        /// Gets or sets the placement for the legend in a chart.
        /// </summary>
        /// <remarks>Legends can be placed left, right, top, or bottom around the chart area.</remarks>
        /// <value>It accepts <see cref="LegendPlacement"/> values and the default value is <see cref="LegendPlacement.Top"/>.</value>
        /// <example>
        /// # [MainPage.xaml](#tab/tabid-5)
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
        /// # [MainPage.xaml.cs](#tab/tabid-6)
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
        /// 
        /// </summary>
        internal IItemsLayout ItemsLayout
        {
            get { return (IItemsLayout)GetValue(ItemsLayoutProperty); }
            set { SetValue(ItemsLayoutProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the chart series visibility by tapping the legend item.
        /// </summary>
        /// <value>It accepts bool values and the default value is <c>False</c></value>
        /// <example>
        /// # [MainPage.xaml](#tab/tabid-7)
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
        /// # [MainPage.xaml.cs](#tab/tabid-8)
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
        /// 
        /// </summary>
        internal Thickness ItemMargin
        {
            get { return (Thickness)GetValue(ItemMarginProperty); }
            set { SetValue(ItemMarginProperty, value); }
        }

        /// <summary>
        /// Gets or sets a template to customize the appearance of each legend item.
        /// </summary>
        /// <value>It accepts <see cref="DataTemplate"/> value.</value>
        /// <example>
        /// # [MainPage.xaml](#tab/tabid-28)
        /// <code><![CDATA[
        /// <chart:SfCircularChart>
        ///
        ///     <chart:SfCircularChart.Legend>
        ///        <chart:ChartLegend>
        ///            <chart:ChartLegend.ItemTemplate>
        ///                <DataTemplate>
        ///                    <StackLayout Orientation="Horizontal" >
        ///                        <Rectangle HeightRequest="12" WidthRequest="12" Margin="3"
        ///                                   Background="{Binding IconBrush}"/>
        ///                        <Label Text="{Binding Text}" Margin="3"/>
        ///                    </StackLayout>
        ///                </DataTemplate>
        ///            </chart:ChartLegend.ItemTemplate>
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
        public DataTemplate ItemTemplate
        {
            get { return (DataTemplate)GetValue(ItemTemplateProperty); }
            set { SetValue(ItemTemplateProperty, value); }
        }


        IEnumerable? itemsSource;
#pragma warning disable CS8603 // Possible null reference return.
        IEnumerable ILegend.ItemsSource { get => itemsSource; set => itemsSource = value; }
#pragma warning restore CS8603 // Possible null reference return.
        IItemsLayout ILegend.ItemsLayout { get => ItemsLayout; set => ItemsLayout = value; }
        bool ILegend.ToggleVisibility { get => ToggleSeriesVisibility; set => ToggleSeriesVisibility = value; }
        DataTemplate ILegend.ItemTemplate { get => ItemTemplate; set => ItemTemplate = value; }

        #endregion

        #region Methods

        internal void Dispose()
        {

        }

        SfLegend ILegend.CreateLegendView()
        {
            return new SfLegendExt();
        }
        #endregion
    }


    internal class SfLegendExt : SfLegend
    {
        protected override SfShapeView CreateShapeView()
        {
            return new ChartLegendShapeView();
        }
    }

    internal class ChartLegendShapeView : SfShapeView
    {

        public override void DrawShape(ICanvas canvas, Rect rect, Core.ShapeType shapeType, Brush strokeColor, float strokeWidth, Brush fillColor)
        {
            if (BindingContext is LegendItem legendItem)
            {
                if (legendItem.IconType == Core.ShapeType.HorizontalLine || legendItem.IconType == Core.ShapeType.VerticalLine)
                {
                    //TODO: Reason for strokeWidth 0, Default legend icon not has stroke support.
                    base.DrawShape(canvas, rect, legendItem.IconType, strokeColor, 1, fillColor);

                    if (legendItem.Item is IMarkerDependent markerDependent && markerDependent.ShowMarkers)
                    {
                        canvas.SaveState();
                        if (markerDependent.MarkerSettings is ChartMarkerSettings setting)
                        {
                            shapeType = ChartUtils.GetShapeType(setting.Type);
                            var center = rect.Center;
                            var iconHeight = (float)setting.Height;
                            var iconWidth = (float)setting.Width;
                            rect = new Rect(center.X - (iconWidth / 2), center.Y - (iconHeight / 2), iconWidth, iconHeight);
                            strokeWidth = (float)setting.StrokeWidth;
                            strokeColor = setting.Stroke;
                            fillColor = setting.Fill ?? fillColor;
                        }

                        base.DrawShape(canvas, rect, shapeType, strokeColor, strokeWidth, fillColor);
                        canvas.RestoreState();
                    }
                }
                else
                {
                    base.DrawShape(canvas, rect, legendItem.IconType, strokeColor, 0, fillColor);
                }
            }
        }
    }
}
