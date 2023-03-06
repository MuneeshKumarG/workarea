using SampleBrowser.Maui.Base;

namespace SampleBrowser.Maui.FunnelChart.SfFunnelChart
{
    public partial class DefaultFunnel : SampleView
    {
        public DefaultFunnel()
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
