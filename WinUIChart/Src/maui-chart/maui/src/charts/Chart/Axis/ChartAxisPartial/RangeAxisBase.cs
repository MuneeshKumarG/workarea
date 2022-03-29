using Microsoft.Maui.Controls;

namespace Syncfusion.Maui.Charts
{
    /// <summary>
    /// 
    /// </summary>
    public partial class RangeAxisBase
    {
        #region Bindable Properties
        /// <summary>
        /// 
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
        /// 
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
        /// 
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
        /// 
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
        /// 
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

        #region Public Properties
        /// <summary>
        /// 
        /// </summary>
        public EdgeLabelsVisibilityMode EdgeLabelsVisibilityMode
        {
            get { return (EdgeLabelsVisibilityMode)GetValue(EdgeLabelsVisibilityModeProperty); }
            set { SetValue(EdgeLabelsVisibilityModeProperty, value); }
        }

        /// <summary>
        ///  
        /// </summary>
        public int MinorTicksPerInterval
        {
            get { return (int)GetValue(MinorTicksPerIntervalProperty); }
            set { SetValue(MinorTicksPerIntervalProperty, value); }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <example>
        /// # [MainWindow.xaml](#tab/tabid-1)
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
        /// # [MainWindow.cs](#tab/tabid-2)
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
        /// 
        /// </summary>
        /// <example>
        /// # [MainWindow.xaml](#tab/tabid-1)
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
        /// # [MainWindow.cs](#tab/tabid-2)
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
        /// 
        /// </summary>
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
        #endregion

        #region Private Methods
        private static void OnMinorTicksPerIntervalPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var axis = bindable as RangeAxisBase;
            if(axis != null)
            {
                var minorTicksPerInterval = (int)newValue;
                axis.SmallTickRequired = minorTicksPerInterval > 0;
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
            if(axis == null)
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
