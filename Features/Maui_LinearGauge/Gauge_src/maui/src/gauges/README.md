The Syncfusion.Maui.Gauges package contains the Syncfusion Radial Gauge component for .NET MAUI application. 

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

* **Axes** - The radial gauge axis is a circular arc in which a set of values are displayed along a linear or custom scale based on the design requirements. Axis elements, such as labels, ticks, and axis line, can be easily customized with built-in properties
* **Ranges** - Gauge range is a visual element that helps to quickly visualize a value where it falls on the axis.
* **Pointers** - Pointer is used to indicate values on an axis. Radial gauge has three types of pointers: needle pointer, marker pointer, and range pointer. All the pointers can be customized as needed.
* **Pointer animation** - Animates the pointer in a visually appealing way when the pointer moves from one value to another.
* **Pointer interaction** - Radial gauge provides an option to drag a pointer from one value to another. It is used to change the value at run time.
* **Annotations** - Add multiple controls such as text and image as an annotation to a specific point of interest in a radial gauge.

## Getting Started

To initialize the Radial Gauge control add the below code to your xaml file.

```csharp
<?xml version="1.0" encoding="utf-8" ?>
<ContentPage x:Class="RadialGaugeMauiSample.GettingStartedSample"
             xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             xmlns:gauge="http://schemas.syncfusion.com/maui"
             BackgroundColor="White">
	<ContentPage.Content>
       <gauge:SfRadialGauge>
            <gauge:SfRadialGauge.Axes>
                  <gauge:RadialAxis Minimum="0"
                        Maximum="150" >
                        <gauge:RadialAxis.Pointers>
                               <gauge:NeedlePointer Value="90" />
                        </gauge:RadialAxis.Pointers>
                  </gauge:RadialAxis>
            </gauge:SfRadialGauge.Axes>
       </gauge:SfRadialGauge>
</ContentPage>
```

## Resources
* **Learn more about .NET MAUI Radial Gauge controls:** [Syncfusion .NET MAUI Radial Gauge](https://www.syncfusion.com/maui-controls/maui-radial-gauge?utm_source=nuget&utm_medium=listing&utm_campaign=maui-radialgauge-nuget)
* **Documentation:** [Syncfusion .NET MAUI Radial Gauge control](https://help.syncfusion.com/maui/radialgauge/getting-started/?utm_source=nuget&utm_medium=listing&utm_campaign=maui-radialgauge-nuget)

## Support and feedback
* For any other queries, reach our [Syncfusion support team](https://www.syncfusion.com/support/directtrac/incidents/newincident?utm_source=nuget&utm_medium=listing&utm_campaign=maui-radialgauge-nuget) or post the queries through the [community forums](https://www.syncfusion.com/forums/maui?utm_source=nuget&utm_medium=listing&utm_campaign=maui-radialgauge-nuget).
* Provide feature request through the [syncfusion feedback portal](https://www.syncfusion.com/feedback/maui?utm_source=nuget&utm_medium=listing&utm_campaign=maui-radialgauge-nuget)

## License
This is a commercial product and requires a paid license for possession or use. Syncfusion’s licensed software, including this component, is subject to the terms and conditions of [Syncfusion's EULA](https://www.syncfusion.com/eula/es/?utm_source=nuget&utm_medium=listing&utm_campaign=maui-radialgauge-nuget). You can purchase a license [here]( https://www.syncfusion.com/sales/products?utm_source=nuget&utm_medium=listing&utm_campaign=maui-radialgauge-nuget) or start a free 30-day trial [here](https://www.syncfusion.com/account/manage-trials/start-trials?utm_source=nuget&utm_medium=listing&utm_campaign=maui-radialgauge-nuget).

## About Syncfusion
Founded in 2001 and headquartered in Research Triangle Park, N.C., Syncfusion has more than 26,000+ customers and more than 1 million users, including large financial institutions, Fortune 500 companies, and global IT consultancies.
 
Today, we provide 1600+ components and frameworks for web ([Blazor](https://www.syncfusion.com/blazor-components?utm_source=nuget&utm_medium=listing&utm_campaign=maui-radialgauge-nuget), [ASP.NET Core](https://www.syncfusion.com/aspnet-core-ui-controls?utm_source=nuget&utm_medium=listing&utm_campaign=maui-radialgauge-nuget), [ASP.NET MVC](https://www.syncfusion.com/aspnet-mvc-ui-controls?utm_source=nuget&utm_medium=listing&utm_campaign=maui-radialgauge-nuget), [ASP.NET WebForms](https://www.syncfusion.com/jquery/aspnet-webforms-ui-controls?utm_source=nuget&utm_medium=listing&utm_campaign=maui-radialgauge-nuget), [JavaScript](https://www.syncfusion.com/javascript-ui-controls?utm_source=nuget&utm_medium=listing&utm_campaign=maui-radialgauge-nuget), [Angular](https://www.syncfusion.com/angular-ui-components?utm_source=nuget&utm_medium=listing&utm_campaign=maui-radialgauge-nuget), [React](https://www.syncfusion.com/react-ui-components?utm_source=nuget&utm_medium=listing&utm_campaign=maui-radialgauge-nuget), [Vue](https://www.syncfusion.com/vue-ui-components?utm_source=nuget&utm_medium=listing&utm_campaign=maui-radialgauge-nuget), and [Flutter](https://www.syncfusion.com/flutter-widgets?utm_source=nuget&utm_medium=listing&utm_campaign=maui-radialgauge-nuget)), mobile ([Xamarin](https://www.syncfusion.com/xamarin-ui-controls?utm_source=nuget&utm_medium=listing&utm_campaign=maui-radialgauge-nuget), [Flutter](https://www.syncfusion.com/flutter-widgets?utm_source=nuget&utm_medium=listing&utm_campaign=maui-radialgauge-nuget), [UWP](https://www.syncfusion.com/uwp-ui-controls?utm_source=nuget&utm_medium=listing&utm_campaign=maui-radialgauge-nuget), and [JavaScript](https://www.syncfusion.com/javascript-ui-controls?utm_source=nuget&utm_medium=listing&utm_campaign=maui-radialgauge-nuget)), and desktop development ([WinForms](https://www.syncfusion.com/winforms-ui-controls?utm_source=nuget&utm_medium=listing&utm_campaign=maui-radialgauge-nuget), [WPF](https://www.syncfusion.com/wpf-ui-controls?utm_source=nuget&utm_medium=listing&utm_campaign=maui-radialgauge-nuget), [WinUI](https://www.syncfusion.com/winui-controls?utm_source=nuget&utm_medium=listing&utm_campaign=maui-radialgauge-nuget), [Flutter](https://www.syncfusion.com/flutter-widgets?utm_source=nuget&utm_medium=listing&utm_campaign=maui-radialgauge-nuget) and [UWP](https://www.syncfusion.com/uwp-ui-controls?utm_source=nuget&utm_medium=listing&utm_campaign=maui-radialgauge-nuget)). We provide ready-to-deploy enterprise software for dashboards, reports, data integration, and big data processing. Many customers have saved millions in licensing fees by deploying our software.

___

[sales@syncfusion.com](mailto:sales@syncfusion.com?Subject=Syncfusion%20Maui%20radialgauge%20-%20NuGet) | [www.syncfusion.com](https://www.syncfusion.com?utm_source=nuget&utm_medium=listing&utm_campaign=maui-radialgauge-nuget) | Toll Free: 1-888-9 DOTNET
