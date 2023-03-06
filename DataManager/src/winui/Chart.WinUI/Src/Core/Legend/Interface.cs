using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;
using System.Collections.ObjectModel;

namespace Syncfusion.UI.Xaml.Charts
{
    internal interface IPlotArea
    {
        ILegend Legend { get; set; }
        bool ShouldPopulateLegendItems { get; set; }
        void PopulateLegendItems();

        ObservableCollection<ILegendItem> LegendItems { get; }
    }

    internal interface ILegend
    {
        bool ToggleSeriesVisibility { get; set; }

        LegendPlacement Placement { get; set; }

        bool IsVisible { get; set; }

        object Header { get; set; }

        DataTemplate HeaderTemplate { get; set; }

        HorizontalAlignment HorizontalHeaderAlignment { get; set; }

        CornerRadius CornerRadius { get; set; }

        Visibility CheckBoxVisibility { get; set; }

        Visibility IconVisibility { get; set; }

        double IconWidth { get; set; }  

        double IconHeight { get; set; }

        Thickness ItemMargin { get; set; }

        object ItemsSource { get; set; }

        Brush BorderBrush { get; set; }

        Brush Background { get; set; }

        Thickness BorderThickness { get; set; }

        Thickness Padding { get; set; } 

        DataTemplate ItemTemplate { get; set; }

    }

    internal interface ILegendItem
    {
        string Text { get; set; }

        Brush IconBrush { get; set; }

        double IconWidth { get; set; }

        double IconHeight { get; set; }

        Visibility IconVisibility { get; set; }

        Visibility CheckBoxVisibility { get; set; }

        int Index { get; }

        object Item { get; }

        bool IsToggled { get; }
    }
}
