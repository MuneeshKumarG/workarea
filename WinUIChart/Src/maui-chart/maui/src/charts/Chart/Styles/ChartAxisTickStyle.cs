using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;

namespace Syncfusion.Maui.Charts
{
    /// <summary>
    /// 
    /// </summary>
    public class ChartAxisTickStyle : BindableObject
    {
        #region BindableProperties

        /// <summary>
        /// 
        /// </summary>        
        public static readonly BindableProperty TickSizeProperty = BindableProperty.Create(nameof(TickSize), typeof(double), typeof(ChartAxisTickStyle), 8d, BindingMode.Default, null, OnTickSizeChanged);

        /// <summary>
        /// 
        /// </summary>        
        public static readonly BindableProperty StrokeProperty = BindableProperty.Create(nameof(Stroke), typeof(Brush), typeof(ChartAxisTickStyle), new SolidColorBrush(Color.FromArgb("#D1D8DB")), BindingMode.Default, null, OnStrokeColorChanged);

        /// <summary>
        /// 
        /// </summary>        
        public static readonly BindableProperty StrokeWidthProperty = BindableProperty.Create(nameof(StrokeWidth), typeof(double), typeof(ChartAxisTickStyle), 1d, BindingMode.Default, null, OnStrokeWidthChanged);

        #endregion

        #region Public Properties

        /// <summary>
        /// 
        /// </summary>
        public double TickSize
        {
            get { return (double)GetValue(TickSizeProperty); }
            set { SetValue(TickSizeProperty, value); }
        }

        /// <summary>
		///
        /// </summary>
        public Brush Stroke
        {
            get { return (Brush)GetValue(StrokeProperty); }
            set { SetValue(StrokeProperty, value); }
        }

        /// <summary>
		/// 
        /// </summary>
        public double StrokeWidth
        {
            get { return (double)GetValue(StrokeWidthProperty); }
            set { SetValue(StrokeWidthProperty, value); }
        }

        #endregion

        #region Constructor 
        /// <summary>
        /// 
        /// </summary>
        public ChartAxisTickStyle()
        {

        }
        #endregion

        #region Internal Properties

        internal bool CanDraw()
        {
            return StrokeWidth > 0 && TickSize > 0;
        }

        #endregion

        #region Callback
        private static void OnTickSizeChanged(BindableObject bindable, object oldValue, object newValue)
        {
        }

        private static void OnStrokeColorChanged(BindableObject bindable, object oldValue, object newValue)
        {
        }

        private static void OnStrokeWidthChanged(BindableObject bindable, object oldValue, object newValue)
        {
        }

        #endregion

    }
}
