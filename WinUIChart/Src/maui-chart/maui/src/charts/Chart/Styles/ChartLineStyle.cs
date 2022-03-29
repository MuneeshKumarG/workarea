using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;

namespace Syncfusion.Maui.Charts
{
    /// <summary>
    /// 
    /// </summary>
    public class ChartLineStyle : BindableObject
    {
        #region Bindable Properties

        /// <summary>
        /// 
        /// </summary>        
        public static readonly BindableProperty StrokeProperty = BindableProperty.Create(nameof(Stroke), typeof(Brush), typeof(ChartLineStyle), new SolidColorBrush(Color.FromArgb("#D1D8DB")), BindingMode.Default, null, OnStrokeColorChanged);

        /// <summary>
        /// 
        /// </summary>        
        public static readonly BindableProperty StrokeWidthProperty = BindableProperty.Create(nameof(StrokeWidth), typeof(double), typeof(ChartLineStyle), 1d , BindingMode.Default, null, OnStrokeWidthChanged);

        /// <summary>
        /// 
        /// </summary>        
        public static readonly BindableProperty StrokeDashArrayProperty = BindableProperty.Create(nameof(StrokeDashArray), typeof(DoubleCollection), typeof(ChartLineStyle), null, BindingMode.Default, null, OnStrokeDashArrayChanged);

        #endregion

        #region Constructor
        /// <summary>
        /// 
        /// </summary>
        public ChartLineStyle()
        {
        }
        #endregion

        #region Properties

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

        /// <summary>
        /// 
        /// </summary>
        public DoubleCollection StrokeDashArray
        {
            get { return (DoubleCollection)GetValue(StrokeDashArrayProperty); }
            set { SetValue(StrokeDashArrayProperty, value); }
        }

        #endregion

        #region Methods

        internal bool CanDraw()
        {
            return StrokeWidth > 0 && !ChartColor.IsEmpty(Stroke.ToColor());
        }

        private static void OnStrokeWidthChanged(BindableObject bindable, object oldValue, object newValue)
        {
        }

        private static void OnStrokeColorChanged(BindableObject bindable, object oldValue, object newValue)
        {
        }

        private static void OnStrokeDashArrayChanged(BindableObject bindable, object oldValue, object newValue)
        {
        }

        #endregion
    }
}
