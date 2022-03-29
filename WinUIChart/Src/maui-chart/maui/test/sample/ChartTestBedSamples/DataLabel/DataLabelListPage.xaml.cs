using System;
using System.Collections.ObjectModel;
using Microsoft.Maui.Controls;
using Syncfusion.Maui.Charts;

namespace ChartTestBedSamples
{
	public partial class DataLabelListPage : ContentPage
	{
		public DataLabelListPage()
		{
			InitializeComponent();
            listview.ItemSelected += Listview_ItemSelected;
            listview.ItemsSource = new ObservableCollection<string>() { "Column", "Line", "Spline", "Scatter", "Area", "SplineArea", "Pie", "Doughnut", "Custom Label", "Pie Outside", "Doughnut Outside" };
		}

        private void Listview_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            int index = e.SelectedItemIndex;

            if (index == 0)
            {
                Navigation.PushAsync(new ColumnLabel() { Title = "Column DataLabel" });
            }
            else if (index == 1)
            {
                Navigation.PushAsync(new LineLabel() { Title = "Line DataLabel" });
            }
            else if (index == 2)
            {
               Navigation.PushAsync(new SplineLabel() { Title = "Spline DataLabel" });
            }
            else if (index == 3)
            {
                Navigation.PushAsync(new ScatterLabel() { Title = "Scatter DataLabel" });
            }
            else if (index == 4)
            {
                Navigation.PushAsync(new AreaLabel() { Title = "Area DataLabel" });
            }
            else if (index == 5)
            {
                Navigation.PushAsync(new SplineAreaLabel() { Title = "SplineArea DataLabel" });
            }
            else if (index == 6)
            {
                Navigation.PushAsync(new PieLabel() { Title = "Pie DataLabel" });
            }
            else if (index == 7)
            {
                Navigation.PushAsync(new DoughnutLabel() { Title = "Doughnut DataLabel" });
            }
            else if (index == 8)
            {
                Navigation.PushAsync(new Custom_DataLabel() { Title = "Custom DataLabel" });
            }
            else if (index == 9)
            {
                Navigation.PushAsync(new PieLabelOutSide() { Title = "Pie Outside DataLabel" });
            }
            else if (index == 10)
            {
                Navigation.PushAsync(new DoughnutLabelOutSide() { Title = "Doughnut Outside DataLabel" });
            }

        }
    }
}