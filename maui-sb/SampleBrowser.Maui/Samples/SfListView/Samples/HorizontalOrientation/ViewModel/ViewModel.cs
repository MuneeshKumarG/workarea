using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleBrowser.Maui.SfListView
{
    public class ListViewOrientationViewModel
    {
        #region Fields

        private ObservableCollection<PizzaInfo> pizzaInfo;
        private ObservableCollection<PizzaInfo> pizzaInfo1;
        

        #endregion

        #region Constructor

        public ListViewOrientationViewModel()
        {
            GenerateSource();
        }

        #endregion

        #region Properties

        public ObservableCollection<PizzaInfo> PizzaInfo
        {
            get { return pizzaInfo; }
            set { this.pizzaInfo = value; }
        }

        public ObservableCollection<PizzaInfo> PizzaInfo1
        {
            get { return pizzaInfo1; }
            set { this.pizzaInfo1 = value; }
        }
      
        #endregion

        #region Generate Source

        private void GenerateSource()
        {
            PizzaInfoRepository pizzainfo = new PizzaInfoRepository();
            pizzaInfo = pizzainfo.GetPizzaInfo();
            pizzaInfo1 = pizzainfo.GetPizzaInfo1();
            
        }
        
        #endregion
    }
}
