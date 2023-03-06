using SampleBrowser.Maui.Base;
using Syncfusion.Maui.Charts;

namespace SampleBrowser.Maui.CartesianChart.SfCartesianChart;

	public partial class DefaultBubbleChart : SampleView
	{
		public DefaultBubbleChart()
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
            yAxis.Title = new ChartAxisTitle() { Text = "GDP growth rate" }; xAxis.Title= new ChartAxisTitle() { Text = "Literacy rate" };
        }
    }

    public override void OnDisappearing()
    {
        base.OnDisappearing();
        Chart.Handler?.DisconnectHandler();
    }
}
