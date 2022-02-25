﻿using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleBrowser.Maui.SfCartesianChart
{
    public class SplineSeriesViewModel : BaseViewModel
    {
        private bool canStopTimer;
        public ObservableCollection<ChartDataModel> SplineData1 { get; set; }
        public ObservableCollection<ChartDataModel> DashedData { get; set; }

        private ObservableCollection<ChartDataModel> motionAnimation;
        public ObservableCollection<ChartDataModel> MotionAnimation
        {
            get { return motionAnimation; }
            set
            {
                motionAnimation = value;
                OnPropertyChanged("MotionAnimation");
            }
        }

        public SplineSeriesViewModel()
        {
            var r = new Random();
            var data = new ObservableCollection<ChartDataModel>();
            for (int i = 0; i < 10; i++)
            {
                data.Add(new ChartDataModel(i, r.Next(5, 90)));
            }

            MotionAnimation = data;

            DashedData = new ObservableCollection<ChartDataModel>()
            {
                 new ChartDataModel(1997,
          17.79,
           20.32,
           22.44, 0),
       new ChartDataModel(
           1998,
           18.20,
           21.46,
           25.18, 0),
       new ChartDataModel(
           1999,
           17.44,
           21.72,
           24.15,0),
       new ChartDataModel(
           2000,  19,  22.86,  25.83,0),
       new ChartDataModel(
           2001,
           18.93,
           22.87,
           25.69,0),
       new ChartDataModel(
           2002,
           17.58,
           21.87,
           24.75,0),
       new ChartDataModel(
           2003,
           16.83,
           21.67,
           27.38,0),
       new ChartDataModel(
           2004,
           17.93,
           21.65,
           25.31,0)
            };

            SplineData1 = new ObservableCollection<ChartDataModel>
            {
                 new ChartDataModel("Jan", 43, 37),
                 new ChartDataModel("Feb", 45, 37),
                 new ChartDataModel("Mar", 50, 39),
                 new ChartDataModel("Apr", 55, 43),
                 new ChartDataModel("May", 63, 48),
                 new ChartDataModel("Jun", 68, 54),
                 new ChartDataModel("Jul", 72, 57),
                 new ChartDataModel("Aug", 70, 57),
                 new ChartDataModel("Sep", 66, 54),
                 new ChartDataModel("Oct", 57, 48),
                 new ChartDataModel("Nov", 50, 43),
                 new ChartDataModel("Dec", 45, 37),
            };
        }

        public void StopTimer()
        {
            canStopTimer = true;
        }

        public async void StartTimer()
        {
            StopTimer();
            await Task.Delay(500);
            Device.StartTimer(new TimeSpan(0, 0, 0, 2, 0), UpdateData);

            canStopTimer = false;
        }

        private bool UpdateData()
        {
            if (canStopTimer) return false;

            var r = new Random();
            var data = new ObservableCollection<ChartDataModel>();
            for (int i = 0; i < 10; i++)
            {
                data.Add(new ChartDataModel(i, r.Next(5, 90)));
            }

            MotionAnimation = data;

            return true;
        }
    }
}
