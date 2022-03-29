The Syncfusion.Maui.Charts package contains the Syncfusion Chart component for .NET MAUI application. 

## Prerequisites

### Integrated Development Environment

By using the following IDEs, you can develop .NET MAUI applications

* Visual Studio 2022 Preview

### Framework & SDK

The below tool is required to run the .NET MAUI application.

  * [.NET 6.0 SDK](https://dotnet.microsoft.com/download/dotnet/6.0)
  * Android 5.0 (API 21) or higher.
  * iOS 10 or higher.
  * macOS 10.13 or higher.
  * Windows desktop and the Universal Windows Platform (UWP), using Windows UI Library (WinUI) 3.
  
## Key Features

* **Chart types** - Cartesian and Circular charts that represent data in a unique style with greater UI visualization, and is user friendly.
* **Interaction** - The .NET MAUI Charts are interactive with features such as tooltip, selection, zooming, and panning.
* **Data binding** - Map data from a specified path by using the data binding concept.
* **Multiple series** - Render multiple series at the same time with options to compare and visualize two different series simultaneously.
* **Customization** - User friendly and provides various options to customize chart features like title, axis, legends, and data labels.

## Getting Started

To initialize the Chart control add the below code to your xaml file.

```xaml
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui" 
        xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
        x:Class="ChartExample.MainPage"
        xmlns:local="using:ChartExample"
        xmlns:chart="clr-namespace:Syncfusion.Maui.Charts;assembly=Syncfusion.Maui.Charts">
	<ContentPage.Content>
        <chart:SfCartesianChart HorizontalOptions="FillAndExpand" VerticalOptions="FillAndExpand">
            <!--Setting BindingContext-->
            <chart:SfCartesianChart.BindingContext>
                <local:ViewModel/>
            </chart:SfCartesianChart.BindingContext>

             <!--Initialize the horizontal axis for the .NET MAUI Chart-->
            <chart:SfCartesianChart.XAxes>
                <chart:CategoryAxis />
            </chart:SfCartesianChart.XAxes>

            <!--Initialize the vertical axis for the .NET MAUI Chart-->
            <chart:SfCartesianChart.YAxes>
                <chart:NumericalAxis />
            </chart:SfCartesianChart.YAxes>

            <!--Adding Column Series to the .NET MAUI Chart-->
            <chart:SfCartesianChart.Series>
                <chart:ColumnSeries 
                    ItemsSource="{Binding Data}" 
                    XBindingPath="Month"
                    YBindingPath="Target">
                </chart:ColumnSeries>
            </chart:SfCartesianChart.Series>    
        </chart:SfCartesianChart>
    </ContentPage.Content>
</ContentPage>
```

## Resources
* **Learn more about .NET MAUI controls:** [Syncfusion .NET MAUI](https://www.syncfusion.com/maui-controls?utm_source=nuget&utm_medium=listing&utm_campaign=maui-chart-nuget)
* **Documentation:** [Syncfusion .NET MAUI controls](https://help.syncfusion.com/maui/cartesian-charts/getting-started/?utm_source=nuget&utm_medium=listing&utm_campaign=maui-chart-nuget)

## Support and feedback
* For any other queries, reach our [Syncfusion support team](https://www.syncfusion.com/support/directtrac/incidents/newincident?utm_source=nuget&utm_medium=listing&utm_campaign=maui-chart-nuget) or post the queries through the [community forums](https://www.syncfusion.com/forums/maui?utm_source=nuget&utm_medium=listing&utm_campaign=maui-chart-nuget).
* Provide feature request through the [syncfusion feedback portal](https://www.syncfusion.com/feedback/maui?utm_source=nuget&utm_medium=listing&utm_campaign=maui-chart-nuget)

## License
This is a commercial product and requires a paid license for possession or use. Syncfusion’s licensed software, including this component, is subject to the terms and conditions of [Syncfusion's EULA](https://www.syncfusion.com/eula/es/?utm_source=nuget&utm_medium=listing&utm_campaign=maui-chart-nuget). You can purchase a license [here]( https://www.syncfusion.com/sales/products?utm_source=nuget&utm_medium=listing&utm_campaign=maui-chart-nuget) or start a free 30-day trial [here](https://www.syncfusion.com/account/manage-trials/start-trials?utm_source=nuget&utm_medium=listing&utm_campaign=maui-chart-nuget).

## About Syncfusion
Founded in 2001 and headquartered in Research Triangle Park, N.C., Syncfusion has more than 26,000+ customers and more than 1 million users, including large financial institutions, Fortune 500 companies, and global IT consultancies.
 
Today, we provide 1600+ components and frameworks for web ([Blazor](https://www.syncfusion.com/blazor-components?utm_source=nuget&utm_medium=listing&utm_campaign=maui-chart-nuget), [ASP.NET Core](https://www.syncfusion.com/aspnet-core-ui-controls?utm_source=nuget&utm_medium=listing&utm_campaign=maui-chart-nuget), [ASP.NET MVC](https://www.syncfusion.com/aspnet-mvc-ui-controls?utm_source=nuget&utm_medium=listing&utm_campaign=maui-chart-nuget), [ASP.NET WebForms](https://www.syncfusion.com/jquery/aspnet-webforms-ui-controls?utm_source=nuget&utm_medium=listing&utm_campaign=maui-chart-nuget), [JavaScript](https://www.syncfusion.com/javascript-ui-controls?utm_source=nuget&utm_medium=listing&utm_campaign=maui-chart-nuget), [Angular](https://www.syncfusion.com/angular-ui-components?utm_source=nuget&utm_medium=listing&utm_campaign=maui-chart-nuget), [React](https://www.syncfusion.com/react-ui-components?utm_source=nuget&utm_medium=listing&utm_campaign=maui-chart-nuget), [Vue](https://www.syncfusion.com/vue-ui-components?utm_source=nuget&utm_medium=listing&utm_campaign=maui-chart-nuget), and [Flutter](https://www.syncfusion.com/flutter-widgets?utm_source=nuget&utm_medium=listing&utm_campaign=maui-chart-nuget)), mobile ([Xamarin](https://www.syncfusion.com/xamarin-ui-controls?utm_source=nuget&utm_medium=listing&utm_campaign=maui-chart-nuget), [Flutter](https://www.syncfusion.com/flutter-widgets?utm_source=nuget&utm_medium=listing&utm_campaign=maui-chart-nuget), [UWP](https://www.syncfusion.com/uwp-ui-controls?utm_source=nuget&utm_medium=listing&utm_campaign=maui-chart-nuget), and [JavaScript](https://www.syncfusion.com/javascript-ui-controls?utm_source=nuget&utm_medium=listing&utm_campaign=maui-chart-nuget)), and desktop development ([WinForms](https://www.syncfusion.com/winforms-ui-controls?utm_source=nuget&utm_medium=listing&utm_campaign=maui-chart-nuget), [WPF](https://www.syncfusion.com/wpf-ui-controls?utm_source=nuget&utm_medium=listing&utm_campaign=maui-chart-nuget), [WinUI](https://www.syncfusion.com/winui-controls?utm_source=nuget&utm_medium=listing&utm_campaign=maui-chart-nuget), [Flutter](https://www.syncfusion.com/flutter-widgets?utm_source=nuget&utm_medium=listing&utm_campaign=maui-chart-nuget) and [UWP](https://www.syncfusion.com/uwp-ui-controls?utm_source=nuget&utm_medium=listing&utm_campaign=maui-chart-nuget)). We provide ready-to-deploy enterprise software for dashboards, reports, data integration, and big data processing. Many customers have saved millions in licensing fees by deploying our software.

___

[sales@syncfusion.com](mailto:sales@syncfusion.com?Subject=Syncfusion%20Maui%20Chart%20-%20NuGet) | [www.syncfusion.com](https://www.syncfusion.com?utm_source=nuget&utm_medium=listing&utm_campaign=maui-chart-nuget) | Toll Free: 1-888-9 DOTNET
