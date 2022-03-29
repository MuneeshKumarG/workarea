using Microsoft.UI.Xaml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syncfusion.UI.Xaml.Charts
{
    public sealed class CircularChartDataLabel : ChartDataLabel
    {
    }

    public sealed class CartesianChartDataLabel : ChartDataLabel
    {

        /// <summary>
        /// The DependencyProperty for <see cref="ConnectorHeight"/> property.
        /// </summary>
        public static readonly DependencyProperty ConnectorHeightProperty =
            DependencyProperty.Register(
                "ConnectorHeight",
                typeof(double),
                typeof(ChartDataLabelBase),
                new PropertyMetadata(0d, OnAdornmentPositionChanged)); //WPF-14304 ConnectorRotationAngle and connectorHeight properties not working dynamically. 

        /// <summary>
        /// The DependencyProperty for <see cref="ConnectorRotationAngle"/> property.
        /// </summary>
        public static readonly DependencyProperty ConnectorRotationAngleProperty =
            DependencyProperty.Register(
                "ConnectorRotationAngle",
                typeof(double),
                typeof(ChartDataLabelBase),
                new PropertyMetadata(double.NaN, OnAdornmentPositionChanged)); //WPF-14304 ConnectorRotationAngle and connectorHeight properties not working dynamically. 

        /// <summary>
        /// The DependencyProperty for <see cref="ConnectorLineStyle"/> property.
        /// </summary>
        public static readonly DependencyProperty ConnectorLineStyleProperty =
            DependencyProperty.Register(
                "ConnectorLineStyle",
                typeof(Style),
                typeof(ChartDataLabelBase),
                new PropertyMetadata(null, OnShowConnectingLine));


        /// <summary>
        /// The DependencyProperty for <see cref="ShowConnectorLine"/> property.
        /// </summary>
        public static readonly DependencyProperty ShowConnectorLineProperty =
            DependencyProperty.Register(
                "ShowConnectorLine",
                typeof(bool),
                typeof(ChartAdornmentInfoBase),
                new PropertyMetadata(false, OnShowConnectingLine));



    }

    public sealed class PolarChartDataLabel : ChartDataLabel
    {

    }
}
