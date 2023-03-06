using SampleBrowser.Maui.Base;
using Syncfusion.Maui.Charts;

namespace SampleBrowser.Maui.CartesianChart.SfCartesianChart
{
    public partial class CartesianTrackball : SampleView
    {
        public CartesianTrackball()
        {
            InitializeComponent();
        }

        public override void OnDisappearing()
        {
            base.OnDisappearing();
            trackballChart.Handler?.DisconnectHandler();
        }

        private void picker_SelectedIndexChanged(object sender, EventArgs e)
        {
            var picker = (Picker)sender;
            int selectedIndex = picker.SelectedIndex;
            if (selectedIndex == 0)
            {
                trackball.DisplayMode = LabelDisplayMode.FloatAllPoints;
            }
            else if (selectedIndex == 1)
            {
                trackball.DisplayMode = LabelDisplayMode.NearestPoint;
            }
        }
    }
}
