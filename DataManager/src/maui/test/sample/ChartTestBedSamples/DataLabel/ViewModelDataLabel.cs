using ChartTestBedSamples.Axis;
using Microsoft.Maui.Graphics;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChartTestBedSamples
{
    public class ViewModelDataLabel
    {
        public ObservableCollection<ChartDataModel> ColumnData { get; set; }
        public ObservableCollection<ChartDataModel> LineData { get; set; }
        public ObservableCollection<Model1> SplineData { get; set; }
        public ObservableCollection<AreaChartModel> AreaData { get; set; }
        public ObservableCollection<Model1> ScatterData { get; set; }
        public ObservableCollection<Color> CustomColors { get; set; }

        public ViewModelDataLabel()
        {
            ColumnData = new ObservableCollection<ChartDataModel>()
            {
                new ChartDataModel("Adidas", 25),
                new ChartDataModel("Nike", 17),
                new ChartDataModel("Reebok", 30),
                new ChartDataModel("File", 18),
                new ChartDataModel("Puma", 10),
            };

            LineData = new ObservableCollection<ChartDataModel>()
            {
                new ChartDataModel(2001, 20, 28),
                new ChartDataModel(2002, 24, 44),
                new ChartDataModel(2003, 36, 48),
                new ChartDataModel(2004, 30, 40),
                new ChartDataModel(2005, 54, 66),
                new ChartDataModel(2006, 59, 78),
                new ChartDataModel(2007, 70, 84),
            };

            SplineData = new ObservableCollection<Model1>()
            {
                new Model1(){XValue = 1, YValue = 52},
                new Model1(){XValue = 2, YValue = 21},
                new Model1(){XValue = 3, YValue = 60},
                new Model1(){XValue = 4, YValue = 62},
                new Model1(){XValue = 5, YValue = 18},
                new Model1(){XValue = 6, YValue = 78},
                new Model1(){XValue = 7, YValue = 55},
                new Model1(){XValue = 8, YValue = 20},
                new Model1(){XValue = 9, YValue = 72},
                new Model1(){XValue = 10, YValue = 60},
            };

            AreaData = new ObservableCollection<AreaChartModel>
            {
                new AreaChartModel() { Date = 2005, Automation = 23, Application = 16, Web = 5 },
                new AreaChartModel() { Date = 2006, Automation = 40, Application = 25, Web = 13 },
                new AreaChartModel() { Date = 2007, Automation = 15, Application = 22, Web = 43 },
                new AreaChartModel() { Date = 2008, Automation = 10, Application = 55, Web = 25 },
                new AreaChartModel() { Date = 2009, Automation = 62, Application = 6, Web = 39 },
                new AreaChartModel() { Date = 2010, Automation = 10, Application = 40, Web = 19 },
                new AreaChartModel() { Date = 2011, Automation = 29, Application = 22, Web = 59 },
                new AreaChartModel() { Date = 2012, Automation = 74, Application = 54, Web = 40 },
                new AreaChartModel() { Date = 2013, Automation = 20, Application = 62, Web = 45 }
            };

            ScatterData = new ObservableCollection<Model1>()
            {
                new Model1(){XValue = 1, YValue = 69},
                new Model1(){XValue = 2, YValue = 19},
                new Model1(){XValue = 3, YValue = 46},
                new Model1(){XValue = 4, YValue = 42},
                new Model1(){XValue = 5, YValue = 67},
                new Model1(){XValue = 6, YValue = 72},
                new Model1(){XValue = 7, YValue = 82},
                new Model1(){XValue = 8, YValue = 9},
                new Model1(){XValue = 9, YValue = 80},
                new Model1(){XValue = 10, YValue = 59},
            };

            CustomColors = new ObservableCollection<Color>()
            {
                new Color(0  / 255.0f, 120/ 255.0f, 222/ 255.0f, 255/ 255.0f),
                new Color(0  / 255.0f, 204/ 255.0f, 106/ 255.0f, 255/ 255.0f),
                new Color(177/ 255.0f,  70/ 255.0f, 194/ 255.0f, 255/ 255.0f),
                new Color(255/ 255.0f, 185/ 255.0f,   0/ 255.0f, 255/ 255.0f),
                new Color(122/ 255.0f, 117/ 255.0f, 116/ 255.0f, 255/ 255.0f),
                new Color(226/ 255.0f, 24 / 255.0f,  47/ 255.0f, 255/ 255.0f),
                new Color(114/ 255.0f,  0 / 255.0f, 230/ 255.0f, 255/ 255.0f),
                new Color(0  / 255.0f, 127/ 255.0f,   0/ 255.0f, 255/ 255.0f),
                new Color(227/ 255.0f, 35 / 255.0f, 111/ 255.0f, 255/ 255.0f),
                new Color(250/ 255.0f, 153/ 255.0f,   1/ 255.0f, 255/ 255.0f),
            };
        }
    }

    public class AreaChartModel
    {
        public double Automation
        {
            get;
            set;
        }

        public double Application
        {
            get;
            set;
        }

        public double Web
        {
            get;
            set;
        }

        public double Date
        {
            get;
            set;
        }
    }

    public class Model1
    {
        public double XValue { get; set; }
        public double YValue { get; set; }
        public string String { get; set; }
    }

}
