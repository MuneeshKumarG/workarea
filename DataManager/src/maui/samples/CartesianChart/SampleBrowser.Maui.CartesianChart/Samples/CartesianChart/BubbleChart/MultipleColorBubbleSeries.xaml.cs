using SampleBrowser.Maui.Base;
using Syncfusion.Maui.Charts;

namespace SampleBrowser.Maui.CartesianChart.SfCartesianChart;
public partial class MultipleColorBubbleSeries : SampleView
{
	public MultipleColorBubbleSeries()
	{
		InitializeComponent();
	}

    public override void OnAppearing()
    {
        base.OnAppearing();
        hyperLinkLayout.IsVisible = !IsCardView;
#if IOS
            if (IsCardView)
            {
                Chart2.WidthRequest = 350;
                Chart2.HeightRequest = 400;
                Chart2.VerticalOptions = LayoutOptions.Start;
            }
#endif
        if (!IsCardView)
        {
            Chart2.Title = (Label)layout.Resources["title"];
            yAxis.Title = new ChartAxisTitle() { Text = "Gross Amount in millions" }; xAxis.Title = new ChartAxisTitle() { Text = "Movie Budget" };
        }
    }

    public override void OnDisappearing()
    {
        base.OnDisappearing();
        Chart2.Handler?.DisconnectHandler();
    }
}