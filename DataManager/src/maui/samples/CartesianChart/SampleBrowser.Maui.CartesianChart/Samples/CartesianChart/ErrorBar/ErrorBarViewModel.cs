using System.Collections.ObjectModel;
namespace SampleBrowser.Maui.CartesianChart.SfCartesianChart
{
    public class ErrorBarViewModel:BaseViewModel
    {
        public ObservableCollection<ChartDataModel> EnergyProductions { get; set; }
        public ObservableCollection<ChartDataModel> ThermalExpansion { get; set; }
        public string[] ErrorBarType => new string[] { "Fixed","Percentage","Standard Error","Standard Deviation" };
        public string[] ErrorBarMode => new string[] {  "Vertical", "Horizontal", "Both"};
        public string[] ErrorBarDirection => new string[] { "Both", "Plus", "Minus" };

        public ErrorBarViewModel()
        {
            EnergyProductions = new ObservableCollection<ChartDataModel>()
            {
                new ChartDataModel{Name="IND",Value=24},
                new ChartDataModel{Name="AUS",Value=20},
                new ChartDataModel{Name="USA",Value=35},
                new ChartDataModel{Name="DEU",Value=27},
                new ChartDataModel{Name="ITA",Value=30},
                new ChartDataModel{Name="UK",Value=41},
                new ChartDataModel{Name="RUS",Value=26},
            };

            ThermalExpansion = new ObservableCollection<ChartDataModel>()
            {
                new ChartDataModel{Name="Erbium",Value=8.2,High=7.6},
                new ChartDataModel{Name="Samarium",Value=8.15,High=5.7},
                new ChartDataModel{Name="Yttritium",Value=7.15,High=6.8},
                new ChartDataModel{Name="Carbide",Value=6.45,High=5.9},
                new ChartDataModel{Name="Uranium",Value=7.45,High=7.1},
                new ChartDataModel{Name="Iron",Value=6.7,High=5},
                new ChartDataModel{Name="Thuilium",Value=8.45,High=7.1},
                new ChartDataModel{Name="Steel",Value=9.7,High=8.6},
                new ChartDataModel{Name="Tin",Value=14.6,High=10.8},
                new ChartDataModel{Name="Gallium",Value=12.2,High=11.6}
            };
        }
    }
}
