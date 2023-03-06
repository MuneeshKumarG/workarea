using SampleBrowser.Maui.Base;

namespace SampleBrowser.Maui.PyramidChart.SfPyramidChart
{
    public partial class DefaultPyramid : SampleView
    {
        public DefaultPyramid()
        {
            InitializeComponent();
        }

        public override void OnDisappearing()
        {
            base.OnDisappearing();
            Chart.Handler?.DisconnectHandler();
        }
    }
}
