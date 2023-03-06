using System;
using System.Collections.Generic;
using Microsoft.Maui.Controls;
using Syncfusion.Maui.Charts;

namespace ChartTestBedSamples
{
    public class ListPage : ContentPage
    {
        public SfCartesianChart cartesianChart { get; set; }
        public SfCircularChart circularChart { get; set; }
        public Label Label { get; set; }
        public Label TestingLabel { get; set; }

        #region CircularSeries

        public Pie PieProperties { get; set; }
        public Doughnut DoughnutProperties { get; set; }

        #endregion

        public ListPage(SfCartesianChart chart1, SfCircularChart chart2, Label label, Label testingLabel)
        {
            this.Label = label;
            this.TestingLabel = testingLabel;

            if (chart1 != null)
                cartesianChart = chart1;
            else if (chart2 is SfCircularChart)
                circularChart = chart2;

            var items = new List<Item>
            {
                new Item {Name = "Column"},
                new Item {Name = "Bar"},
                new Item {Name = "Line"},
                new Item {Name = "Spline"},
                new Item {Name = "Area"},
                new Item {Name = "SplineArea"},
                new Item {Name = "Scatter"},
                new Item {Name = "Pie"},
                new Item {Name = "Doughnut"},
                new Item {Name = "CategoryAxis"},
                new Item {Name = "LogarithmicAxis"},
                new Item {Name = "NumericalAxis"},
                new Item {Name = "DateTimeAxis"},
                new Item {Name = "ChartBehaviors"},
                new Item {Name = "Legend"},
                new Item {Name = "MultipleSeries"}
            };

            var listview = new ListView()
            {
                ItemsSource = items,
            };

            listview.ItemSelected += Listview_ItemSelected;
            listview.ItemTemplate = new DataTemplate(typeof(TextCell));
            listview.ItemTemplate.SetBinding(TextCell.TextProperty, "Name");
            //  listview.HeightRequest = 100;

            Content = listview;
        }

        private void Listview_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var item = e.SelectedItem as Item;

            if (item == null)
                return;

            if (item.Name.Contains("Behaviors"))
            {
                Label.IsVisible = true;
            }
            else if (item.Name.Contains("Multiple") || item.Name.Contains("Legend"))
            {
                Label.IsVisible = false;
            }
            else if (!item.Name.Contains("Axis"))
            {
                Label.IsVisible = false;
                cartesianChart?.Series.Clear();
                circularChart?.Series.Clear();
            }

            switch (item.Name)
            {

                case "Pie":
                    if (PieProperties == null)
                        PieProperties = new Pie(circularChart);
                    circularChart.Series.Add(PieProperties.Series);
                    Navigation.PushAsync(PieProperties);
                    break;
                    //case "Doughnut":
                    //    if (DoughnutProperties == null)
                    //        DoughnutProperties = new Doughnut(chart);
                    //    chart.Series.Add(DoughnutProperties.Series);
                    //    Navigation.PushAsync(DoughnutProperties);
                    //    break;
            }
        }

        public class Item
        {
            public string Name { get; set; }
            public Type ItemType { get; set; }
            public Command ItemCommand { get; set; }
        }
    }
}