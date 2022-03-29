using Microsoft.Maui.Controls;
using Microsoft.Maui;

namespace Syncfusion.Maui.Charts 
{
    /// <summary>
    /// 
    /// </summary>
    public class CircularDataLabelSettings : ChartDataLabelSettings
    {
        #region Bindable Properties

        /// <summary>
        /// 
        /// </summary>        
        public static readonly BindableProperty ConnectorTypeProperty =
            BindableProperty.Create(nameof(ConnectorType), typeof(ConnectorType), typeof(CircularDataLabelSettings), ConnectorType.Line, BindingMode.Default, null, null);

        #endregion

        #region Constructor

        /// <summary>
        /// 
        /// </summary>
        public CircularDataLabelSettings()
        {
            LabelStyle = new ChartDataLabelStyle() { FontSize = 14, Margin = new Thickness(8, 6) };
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// 
        /// </summary>
        public ConnectorType ConnectorType
        {
            get { return (ConnectorType)GetValue(ConnectorTypeProperty); }
            set { SetValue(ConnectorTypeProperty, value); }
        }

        #endregion
    }
}