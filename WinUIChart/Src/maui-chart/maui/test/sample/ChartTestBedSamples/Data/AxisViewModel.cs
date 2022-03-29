using Microsoft.Maui.Controls;
using System;
using System.Collections.ObjectModel;

namespace ChartTestBedSamples
{
    public class AxisViewModel : ContentPage
    {

        public ObservableCollection<Model> CatPositiveEdge
        {
            get;
            set;
        }

        public ObservableCollection<Model> CatPositive
        {
            get;
            set;
        }

        public ObservableCollection<Model> CatNegative
        {
            get;
            set;
        }
        public ObservableCollection<Model> CatEmptyPoint
        {
            get;
            set;
        }

        public ObservableCollection<Model> CatEmptyNegPos
        {
            get;
            set;
        }

        public ObservableCollection<Model> CatFirstLastEmpty
        {
            get;
            set;
        }

        public ObservableCollection<Model> CatSingleEmpty
        {
            get;
            set;
        }

        public ObservableCollection<Model> CatSingleNegative
        {
            get;
            set;
        }

        public ObservableCollection<Model> CatSinglePositive
        {
            get;
            set;
        }

        public ObservableCollection<Model> NumPositive
        {
            get;
            set;
        }

        public ObservableCollection<Model> NumNegative
        {
            get;
            set;
        }

        public ObservableCollection<Model> NumEmptyPoint
        {
            get;
            set;
        }

        public ObservableCollection<Model> NumEmptyNegPos
        {
            get;
            set;
        }

        public ObservableCollection<Model> NumFirstLastEmpty
        {
            get;
            set;
        }

        public ObservableCollection<Model> NumSingleEmpty
        {
            get;
            set;
        }

        public ObservableCollection<Model> NumSingleNegative
        {
            get;
            set;
        }

        public ObservableCollection<Model> NumSinglePositive
        {
            get;
            set;
        }

        public ObservableCollection<Model> NumFirstNextLastPrevEmpty
        {
            get;
            set;
        }

        public ObservableCollection<Model> DateMillisecond
        {
            get;
            set;
        }


        public ObservableCollection<Model> DatePositive
        {
            get;
            set;
        }

        public ObservableCollection<Model> MoreDataPoints
        {
            get;
            set;
        }

        public ObservableCollection<Model> DateNegative
        {
            get;
            set;
        }

        public ObservableCollection<Model> DateEmptyPoint
        {
            get;
            set;
        }

        public ObservableCollection<Model> DateEmptyNegPos
        {
            get;
            set;
        }

        public ObservableCollection<Model> DateFirstLastEmpty
        {
            get;
            set;
        }

        public ObservableCollection<Model> DateSingleEmpty
        {
            get;
            set;
        }

        public ObservableCollection<Model> DateSingleNegative
        {
            get;
            set;
        }

        public ObservableCollection<Model> DateSinglePositive
        {
            get;
            set;
        }

        public ObservableCollection<Model> WithoutData
        {
            get;
            set;
        }

        public ObservableCollection<Model> NullSource
        {
            get;
            set;
        }

        public ObservableCollection<Model> AllZero
        {
            get;
            set;
        }

        public ObservableCollection<Model> LongXVal
        {
            get;
            set;
        }

        public ObservableCollection<Model> EmptyData
        {
            get;
            set;
        }

        public ObservableCollection<Model> PositiveData
        {
            get;
            set;
        }

        public AxisViewModel()
        {
            CatPositiveEdge = new ObservableCollection<Model>();
            CatPositiveEdge.Add(new Model("Syncfusion Inc Xamarin", 34));
            CatPositiveEdge.Add(new Model("devexpress Xamarin", 23));
            CatPositiveEdge.Add(new Model("Telerik Xamarin", 20));
            CatPositiveEdge.Add(new Model("Shinobi Xamarin", 45));
            CatPositiveEdge.Add(new Model("infragistics Xamarin", 10));


            MoreDataPoints = new ObservableCollection<Model>();
            MoreDataPoints.Add(new Model("A", 34));
            MoreDataPoints.Add(new Model("B", 23));
            MoreDataPoints.Add(new Model("C", 20));
            MoreDataPoints.Add(new Model("D", 45));
            MoreDataPoints.Add(new Model("E", 10));
            MoreDataPoints.Add(new Model("F", 38));
            MoreDataPoints.Add(new Model("G", 22));
            MoreDataPoints.Add(new Model("H", 25));
            MoreDataPoints.Add(new Model("I", 35));
            MoreDataPoints.Add(new Model("J", 20));
            MoreDataPoints.Add(new Model("K", 32));
            MoreDataPoints.Add(new Model("L", 21));
            MoreDataPoints.Add(new Model("M", 23));

            CatPositive = new ObservableCollection<Model>();
            CatPositive.Add(new Model("A", 34));
            CatPositive.Add(new Model("B", 23));
            CatPositive.Add(new Model("C", 20));
            CatPositive.Add(new Model("D", 45));
            CatPositive.Add(new Model("E", 10));

            CatNegative = new ObservableCollection<Model>();
            CatNegative.Add(new Model("A", 34));
            CatNegative.Add(new Model("B", 23));
            CatNegative.Add(new Model("C", -20));
            CatNegative.Add(new Model("D", 45));
            CatNegative.Add(new Model("E", 10));

            CatEmptyPoint = new ObservableCollection<Model>();
            CatEmptyPoint.Add(new Model("A", 34));
            CatEmptyPoint.Add(new Model("B", 23));
            CatEmptyPoint.Add(new Model("C", double.NaN));
            CatEmptyPoint.Add(new Model("D", 45));
            CatEmptyPoint.Add(new Model("E", 10));

            CatEmptyNegPos = new ObservableCollection<Model>();
            CatEmptyNegPos.Add(new Model("A", -34));
            CatEmptyNegPos.Add(new Model("B", 23));
            CatEmptyNegPos.Add(new Model("C", 20));
            CatEmptyNegPos.Add(new Model("D", 45));
            CatEmptyNegPos.Add(new Model("E", -10));

            CatFirstLastEmpty = new ObservableCollection<Model>();
            CatFirstLastEmpty.Add(new Model("A", double.NaN));
            CatFirstLastEmpty.Add(new Model("B", 23));
            CatFirstLastEmpty.Add(new Model("C", 20));
            CatFirstLastEmpty.Add(new Model("D", 45));
            CatFirstLastEmpty.Add(new Model("E", double.NaN));

            CatSingleEmpty = new ObservableCollection<Model>();
            CatSingleEmpty.Add(new Model("A", double.NaN));

            CatSinglePositive = new ObservableCollection<Model>();
            CatSinglePositive.Add(new Model("A", 20));

            CatSingleNegative = new ObservableCollection<Model>();
            CatSingleNegative.Add(new Model("A", -20));

            AllZero = new ObservableCollection<Model>();
            AllZero.Add(new Model(1, 0));
            AllZero.Add(new Model(2, 0));
            AllZero.Add(new Model(3, 0));
            AllZero.Add(new Model(4, 0));
            AllZero.Add(new Model(5, 0));

            LongXVal = new ObservableCollection<Model>();
            LongXVal.Add(new Model((long)6000000000000001, 30));
            LongXVal.Add(new Model((long)6000000000000002, 20));
            LongXVal.Add(new Model((long)6000000000000003, 10));
            LongXVal.Add(new Model((long)6000000000000004, 40));
            LongXVal.Add(new Model((long)6000000000000005, 50));


            NumPositive = new ObservableCollection<Model>();
            NumPositive.Add(new Model(1, 34));
            NumPositive.Add(new Model(2, 23));
            NumPositive.Add(new Model(3, 20));
            NumPositive.Add(new Model(4, 45));
            NumPositive.Add(new Model(5, 10));

            NumNegative = new ObservableCollection<Model>();
            NumNegative.Add(new Model(1, 34));
            NumNegative.Add(new Model(2, 23));
            NumNegative.Add(new Model(3, -20));
            NumNegative.Add(new Model(4, 45));
            NumNegative.Add(new Model(5, 10));

            NumEmptyPoint = new ObservableCollection<Model>();
            NumEmptyPoint.Add(new Model(1, 34));
            NumEmptyPoint.Add(new Model(2, 23));
            NumEmptyPoint.Add(new Model(3, double.NaN));
            NumEmptyPoint.Add(new Model(4, 45));
            NumEmptyPoint.Add(new Model(5, 10));

            NumEmptyNegPos = new ObservableCollection<Model>();
            NumEmptyNegPos.Add(new Model(1, -34));
            NumEmptyNegPos.Add(new Model(2, 23));
            NumEmptyNegPos.Add(new Model(3, 20));
            NumEmptyNegPos.Add(new Model(4, 45));
            NumEmptyNegPos.Add(new Model(5, -10));

            NumFirstLastEmpty = new ObservableCollection<Model>();
            NumFirstLastEmpty.Add(new Model(1, double.NaN));
            NumFirstLastEmpty.Add(new Model(2, 23));
            NumFirstLastEmpty.Add(new Model(3, 20));
            NumFirstLastEmpty.Add(new Model(4, 45));
            NumFirstLastEmpty.Add(new Model(5, double.NaN));

            NumFirstNextLastPrevEmpty = new ObservableCollection<Model>();
            NumFirstNextLastPrevEmpty.Add(new Model(1, 15));
            NumFirstNextLastPrevEmpty.Add(new Model(2, double.NaN));
            NumFirstNextLastPrevEmpty.Add(new Model(3, 20));
            NumFirstNextLastPrevEmpty.Add(new Model(4, double.NaN));
            NumFirstNextLastPrevEmpty.Add(new Model(5, 12));

            NumSingleEmpty = new ObservableCollection<Model>();
            NumSingleEmpty.Add(new Model(1, double.NaN));

            NumSinglePositive = new ObservableCollection<Model>();
            NumSinglePositive.Add(new Model(1, 20));

            NumSingleNegative = new ObservableCollection<Model>();
            NumSingleNegative.Add(new Model(1, -20));

            EmptyData = new ObservableCollection<Model>();
            EmptyData.Add(new Model(1, Double.NaN));
            EmptyData.Add(new Model(2, 3000000));
            EmptyData.Add(new Model(3, 7500000));
            EmptyData.Add(new Model(4, 6000000));
            EmptyData.Add(new Model(5, Double.NaN));

            PositiveData = new ObservableCollection<Model>();
            PositiveData.Add(new Model(1, 15000000));
            PositiveData.Add(new Model(2, 2000000));
            PositiveData.Add(new Model(3, 3000000));
            PositiveData.Add(new Model(4, 65000000));
            PositiveData.Add(new Model(5, 10000000));

            DateMillisecond = new ObservableCollection<Model>();
            DateMillisecond.Add(new Model(new DateTime(2006, 1, 20, 10, 11, 12, 600), 34));
            DateMillisecond.Add(new Model(new DateTime(2006, 1, 20, 10, 11, 12, 650), 23));
            DateMillisecond.Add(new Model(new DateTime(2006, 1, 20, 10, 11, 12, 700), 20));
            DateMillisecond.Add(new Model(new DateTime(2006, 1, 20, 10, 11, 12, 750), 45));
            DateMillisecond.Add(new Model(new DateTime(2006, 1, 20, 10, 11, 12, 800), 10));


            DatePositive = new ObservableCollection<Model>();
            DatePositive.Add(new Model(new DateTime(2006, 1, 20), 34));
            DatePositive.Add(new Model(new DateTime(2007, 1, 20), 23));
            DatePositive.Add(new Model(new DateTime(2008, 1, 20), 20));
            DatePositive.Add(new Model(new DateTime(2009, 1, 20), 45));
            DatePositive.Add(new Model(new DateTime(2010, 1, 20), 10));

            DateNegative = new ObservableCollection<Model>();
            DateNegative.Add(new Model(new DateTime(2006, 1, 20), 34));
            DateNegative.Add(new Model(new DateTime(2007, 1, 20), 23));
            DateNegative.Add(new Model(new DateTime(2008, 1, 20), -20));
            DateNegative.Add(new Model(new DateTime(2009, 1, 20), 45));
            DateNegative.Add(new Model(new DateTime(2010, 1, 20), 10));

            DateEmptyPoint = new ObservableCollection<Model>();
            DateEmptyPoint.Add(new Model(new DateTime(2006, 1, 20), 34));
            DateEmptyPoint.Add(new Model(new DateTime(2007, 1, 20), 23));
            DateEmptyPoint.Add(new Model(new DateTime(2008, 1, 20), double.NaN));
            DateEmptyPoint.Add(new Model(new DateTime(2009, 1, 20), 45));
            DateEmptyPoint.Add(new Model(new DateTime(2010, 1, 20), 10));

            DateEmptyNegPos = new ObservableCollection<Model>();
            DateEmptyNegPos.Add(new Model(new DateTime(2006, 1, 20), -34));
            DateEmptyNegPos.Add(new Model(new DateTime(2007, 1, 20), 23));
            DateEmptyNegPos.Add(new Model(new DateTime(2008, 1, 20), 20));
            DateEmptyNegPos.Add(new Model(new DateTime(2009, 1, 20), 45));
            DateEmptyNegPos.Add(new Model(new DateTime(2010, 1, 20), -10));

            DateFirstLastEmpty = new ObservableCollection<Model>();
            DateFirstLastEmpty.Add(new Model(new DateTime(2006, 1, 20), double.NaN));
            DateFirstLastEmpty.Add(new Model(new DateTime(2007, 1, 20), 23));
            DateFirstLastEmpty.Add(new Model(new DateTime(2008, 1, 20), 20));
            DateFirstLastEmpty.Add(new Model(new DateTime(2009, 1, 20), 45));
            DateFirstLastEmpty.Add(new Model(new DateTime(2010, 1, 20), double.NaN));

            DateSingleEmpty = new ObservableCollection<Model>();
            DateSingleEmpty.Add(new Model(new DateTime(2010, 1, 20), double.NaN));

            DateSinglePositive = new ObservableCollection<Model>();
            DateSinglePositive.Add(new Model(new DateTime(2010, 1, 20), 20));

            DateSingleNegative = new ObservableCollection<Model>();
            DateSingleNegative.Add(new Model(new DateTime(2010, 1, 20), -20));

            WithoutData = new ObservableCollection<Model>();

        }
    }
}