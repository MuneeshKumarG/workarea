using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Markup;
using Microsoft.UI.Xaml.Media;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;

namespace Syncfusion.UI.Xaml.Charts
{
    public class SfCartesianChart : ChartBase
    {

        #region Dependency properties

        /// <summary>
        /// The DependencyProperty for <see cref="PlotAreaBackground"/> property.
        /// </summary>
        public static readonly DependencyProperty PlotAreaBackgroundProperty =
            DependencyProperty.Register(nameof(PlotAreaBackground), typeof(UIElement), typeof(SfCartesianChart), new PropertyMetadata(null));

        /// <summary>
        /// The DependencyProperty for <see cref="PlotAreaMargin"/> property.
        /// </summary>        
        internal static readonly DependencyProperty PlotAreaMarginProperty =
            DependencyProperty.Register(
                nameof(PlotAreaMargin),
                typeof(Thickness),
                typeof(ChartBase),
                new PropertyMetadata(new Thickness().GetThickness(0, 0, 0, 0)));

        #endregion

        #region Fields

        internal Canvas SyncfusionChartAxisPanel;

        #endregion

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the SfCartesianChart class.
        /// </summary>
        public SfCartesianChart()
        {
            DefaultStyleKey = typeof(SfCartesianChart);
        }

        #endregion

        #region Properties

        public UIElement PlotAreaBackground
        {
            get { return (UIElement)GetValue(PlotAreaBackgroundProperty); }
            set { SetValue(PlotAreaBackgroundProperty, value); }
        }

        internal Thickness PlotAreaMargin
        {
            get { return (Thickness)GetValue(PlotAreaMarginProperty); }
            set { SetValue(PlotAreaMarginProperty, value); }
        }

        #endregion

        #region Methods

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            SyncfusionChartAxisPanel = GetTemplateChild("SyncfusionChartAxisPanel") as Canvas;
        }

        #endregion

    }
}
