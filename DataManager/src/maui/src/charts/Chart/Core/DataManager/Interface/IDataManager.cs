using System;
using System.Collections.Generic;

#if WinUI
namespace Syncfusion.UI.Xaml.Charts
#else
namespace Syncfusion.Maui.Charts
#endif
{
    internal interface IDataManagerDependent
    {
        object ItemsSource { get; }

        string XPath { get; }

        List<string> YPaths { get;  }

        List<string> XStringValues { get; set; }

        List<double> XDoubleValues { get; set; }

        List<DateTime> XDateTimeValues { get; set; }

        List<double> XIndexedList { get; set; }

        ValueType XValueType { get; set; }

        List<List<double>> YDoubleValues { get; set; }

        List<object> ActualData { get; set; }

        bool ListenPropertyChange { get;  }

        int PointsCount { get;  set; }

        bool IsLinearData { get;  set; }

        void UpdateArea();
        
        bool IsGroupedYPath { get; }
    }

}
