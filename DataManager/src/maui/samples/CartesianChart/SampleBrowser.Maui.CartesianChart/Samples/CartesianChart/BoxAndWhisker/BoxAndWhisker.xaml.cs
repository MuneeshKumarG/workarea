using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SampleBrowser.Maui.Base;
using Syncfusion.Maui.Charts;

namespace SampleBrowser.Maui.CartesianChart.SfCartesianChart
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BoxAndWhisker : SampleView
    {
        public BoxAndWhisker()
        {
            InitializeComponent();
        }

        public override void OnAppearing()
        {
            base.OnAppearing();

            hyperLinkLayout.IsVisible = !IsCardView;
            if (!IsCardView)
            {
                Chart.Title = (Label)layout.Resources["title"];
                xAxis.Title = new ChartAxisTitle() { Text = "Machine" };
                yAxis.Title = new ChartAxisTitle() { Text = "Energy" };
            }
        }

        private void ModePicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            var picker = (Picker)sender;
            int selectedIndex = picker.SelectedIndex;

            if (selectedIndex == 0)
            {
                BoxAndWhisker1.BoxPlotMode = BoxPlotMode.Exclusive;
            }
            else
            {
                BoxAndWhisker1.BoxPlotMode = BoxPlotMode.Inclusive;
            }
        }
    }
}