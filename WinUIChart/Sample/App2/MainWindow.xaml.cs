using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Syncfusion.UI.Xaml.Charts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace App2
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.InitializeComponent();
            //SfChart chart = new SfChart() { Header = "Chart", Height = 300, Width = 500 };

            ////Adding horizontal axis to the chart 
            //CategoryAxis primaryAxis = new CategoryAxis();
            //primaryAxis.Header = "Name";
            //primaryAxis.FontSize = 14;
            //chart.PrimaryAxis = primaryAxis;

            ////Adding vertical axis to the chart 
            //NumericalAxis secondaryAxis = new NumericalAxis();
            //secondaryAxis.Header = "Height(in cm)";
            //secondaryAxis.FontSize = 14;
            //chart.SecondaryAxis = secondaryAxis;

            ////Adding Legends for the chart
            //ChartLegend legend = new ChartLegend();
            //chart.Legend = legend;

            ////Initializing column series
            //ColumnSeries series = new ColumnSeries();
            //series.ItemsSource = (new ViewModel()).Data;
            //series.XBindingPath = "Name";
            //series.YBindingPath = "Height";
            //series.ShowTooltip = true;
            //series.Label = "Heights";
            //series.ShowDataLabels = true;
            //series.DataLabelSettings = new CartesianDataLabelSettings()
            //{
            //    Position = DataLabelPosition.Inner,
            //};

            ////Adding Series to the Chart Series Collection
            //chart.Series.Add(series);
            //grid.Children.Add(chart);


            SfCartesianChart chart = new SfCartesianChart() { Width=500,Height=350};
            chart.Background = new SolidColorBrush(Colors.Yellow);
            chart.PlotAreaBackground = new Border()
            {
                BorderBrush = new SolidColorBrush(Colors.Red),
                BorderThickness=new Thickness(2),
                Child = new TextBlock()
                {
                    Text = "Syncfusion",
                    FontSize = 32,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                }
            };
        
            chart.Header = "Chart Header";
            chart.Legend = new ChartLegend();
            grid.Children.Add(chart);

            TextBlock textBlock = new TextBlock();
            textBlock.FontWeight= Microsoft.UI.Text.FontWeights.Normal;
        }

    }

    public class Person
    {
        public string Name { get; set; }

        public double Height { get; set; }
    }

    public class ViewModel
    {
        public List<Person> Data { get; set; }

        public ViewModel()
        {
            Data = new List<Person>()
            {
                new Person { Name = "David", Height = 180 },
                new Person { Name = "Michael", Height = 170 },
                new Person { Name = "Steve", Height = 160 },
                new Person { Name = "Joel", Height = 182 }
            };
        }
    }
}
