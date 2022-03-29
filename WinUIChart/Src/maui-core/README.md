This repository contains various helper classes, extension classes and converters for common usage for all teams in WinUI.

Open source library usage approval task: https://syncfusion.atlassian.net/browse/OSL-343

# COMMON

Name space : `Syncfusion.UI.Xaml.Core`

| Class   |      Comments      |  Example |
|:----------|:-------------|------|
| `DelegateCommand<T>` |  Abstract class implements `ICommand`. Can be used in application level for defining commands. |  |
| `DelegateCommand` | Abstract class implements `DelegateCommand<object>`. Can be used in application level for defining commands. |  |
| `NotificationObject` | Abstract class implements `INotifyPropertyChanged` interface. Use this class in sample and source where `INotifyPropertyChanged` implentation is needed. |  |

# Markup Extensions

## Binding Enum Values in XAML

Use `EnumValuesExtension` markup extension to get list of enum values as below.

```xml
<Page x:Class="WinUIRadialGaugeTestbed.MainPage"
      xmlns:gauge="using:Syncfusion.UI.Xaml.Gauges"
      xmlns:coreextension="using:Syncfusion.UI.Xaml.Core.Extensions">
    <Page.Resources>
        <coreextension:EnumDisplayNameConverter x:Key="EnumDisplayNameConverter"/>
    </Page.Resources>
    <Grid x:Name="MainGrid">
        <!-- Can be used with of enums defined in our controls  -->
        <ComboBox ItemsSource="{coreextension:EnumValuesExtension Type=gauge:AnnotationDirection">
            <ComboBox.ItemTemplate>
                <DataTemplate>    
                    <TextBlock Text={Binding Converter={StaticResource EnumDisplayNameConverter}}/>
                </DataTemplate>
            </ComboBox.ItemTemplate>
        </ComboBox>
    </Grid>
</Page>
```

# LOGICAL & VISUAL TREE EXTENSION

Name space : `Syncfusion.UI.Xaml.Core.Extensions`

| Class   |      Comments      |  Example |
|:----------|:-------------|------|
| Logical Tree Extensions |  The LogicalTree extensions provide a collection of extensions methods for UI controls. | [Example](https://docs.microsoft.com/en-us/windows/communitytoolkit/extensions/logicaltree) |
| Visual Tree Extensions | The VisualTree extensions provide a collection of extensions methods for UI controls. | [Example](https://docs.microsoft.com/en-us/windows/communitytoolkit/extensions/visualtree) |

> Logical and Visual tree extensions in core libary ported from UWP Community toolkit. So, you can refer the example from microsoft docs. But ensure to use the extension classes in core library.

# CONVERTERS

Name space : `Syncfusion.UI.Xaml.Core.Converters`

> All the below converters code are taken from UWP community toolkit.

| Class   |      Comments      |  Example |
|:----------|:-------------|------|
| `BoolNegationConverter` |  Value converter that applies NOT operator to a <see cref="bool"/> value. | Use this converter when you want to inverse bool value in XAML. |
| `BoolToObjectConverter` | This class converts a boolean value into an other object. | `BoolToObjectConverter` has `TrueValue` and `FalseValue` properties. If value is `true` then returns `TrueValue`, otherwise returns `FalseValue`. You can inverse this operation by passing `true` in `CommandParameter`. [More details](https://docs.microsoft.com/en-us/dotnet/api/microsoft.toolkit.uwp.ui.converters.booltoobjectconverter) |
| `BoolToVisibilityConverter` | This class converts a boolean value into a Visibility enumeration. | If value is `true`, then returns `Visibility.Visible`. Otherwise, returns `Visibility.Collapsed`. You can inverse this operation by passing `true` in `CommandParameter`. When you pass true in command parameter then, converter will return `Visibility.Collapsed` for `true` value. [More details](https://docs.microsoft.com/en-us/dotnet/api/microsoft.toolkit.uwp.ui.converters.booltovisibilityconverter) |
| `VisibilityToBoolConverter` | This converter converts `Visibility` to `bool`. Returns `true` if value is `Visibility.Visible`. Otherwise, returns `false`. | |
| `EmptyObjectToObjectConverter` | This converter class exposes `EmptyValue` and `NotEmptyValue` properties and this converter returns `EmptyValue` when value is `null` in converter. | You can inverse this converter operation by passing `true` in `CommandParameter`. [More details](https://docs.microsoft.com/en-us/dotnet/api/microsoft.toolkit.uwp.ui.converters.emptyobjecttoobjectconverter) |
| `EmptyCollectionToObjectConverter` | This converter class exposes `EmptyValue` and `NotEmptyValue` properties and this converter returns `EmptyValue` when value is `IEnumerable` and has zero items or value is not `IEnumerable`. | You can inverse this converter operation by passing `true` in `CommandParameter`. [More details](https://docs.microsoft.com/en-us/dotnet/api/microsoft.toolkit.uwp.ui.converters.emptycollectiontoobjectconverter) |
| `EmptyStringToObjectConverter` | This converter class exposes `EmptyValue` and `NotEmptyValue` properties and this converter returns `EmptyValue` when value is null or empty. `(string.IsNullOrEmpty)`. | You can inverse this converter operation by passing `true` in `CommandParameter`. [More details](https://docs.microsoft.com/en-us/dotnet/api/microsoft.toolkit.uwp.ui.converters.emptystringtoobjectconverter) |
| `StringVisibilityConverter` | This converter returns `Visibility.Collapsed` when strin is null or empty. Otherwise, returns `Visibility.Collapse`.  | You can inverse this converter operation by passing `true` in `CommandParameter`. |
| `FormatStringConverter` | Value converter that converts an object (of type `IFormattable`) to a formatted string. You need to pass `Format` in command parameter. | |
| `StringFormatConverter` | Value converter that converts an object to formatted string. You need to pass `Format` in command parameter. | You can use this converter in binding to format the values. [More details of about formats](https://docs.microsoft.com/en-us/dotnet/api/system.string.format?view=netcore-3.1#Starting) |
