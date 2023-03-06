using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syncfusion.Maui.Charts
{
    /// <summary>
    /// 
    /// </summary>
    internal static class ChartMarker
    {
        /// <summary>
        /// Identifies the marker <see cref="ShowMarkersProperty"/> bindable property.
        /// </summary>        
        public static readonly BindableProperty ShowMarkersProperty = BindableProperty.Create(nameof(IMarkerDependent.ShowMarkers) ,
            typeof(bool), typeof(IMarkerDependent), false, propertyChanged: OnShowMarkersChanged);

        /// <summary>
        /// Identifies the marker <see cref="MarkerSettingsProperty"/> bindable property.
        /// </summary>        
        public static readonly BindableProperty MarkerSettingsProperty = BindableProperty.Create(nameof(IMarkerDependent.MarkerSettings),
            typeof(ChartMarkerSettings), typeof(IMarkerDependent), propertyChanged: OnMarkerSettingsChanged);

        private static void OnShowMarkersChanged(BindableObject bindable, object oldValue, object newValue)
        {
            ((IMarkerDependent)bindable).OnShowMarkersChanged((bool)oldValue, (bool)newValue);
        }

        private static void OnMarkerSettingsChanged(BindableObject bindable, object oldValue, object newValue)
        {
            ((IMarkerDependent)bindable).OnMarkerSettingsChanged((object)oldValue, (object)newValue);
        }
    }
}
