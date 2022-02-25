using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SampleBrowser.Maui.SfListView
{
    public class PizzaInfoRepository
    {
        #region Constructor

        public PizzaInfoRepository()
        {

        }

        #endregion

        #region Properties

        internal ObservableCollection<PizzaInfo> GetPizzaInfo()
        {
            var categoryInfo = new ObservableCollection<PizzaInfo>();
            for (int i = 0; i < PizzaNames.Count(); i++)
            {
                var info = new PizzaInfo() { PizzaName = PizzaNames[i] };

                if (i == 9)
                    info.PizzaImage ="pizza3.jpg";
                else
                    info.PizzaImage = "pizza" + i + ".jpg";

                categoryInfo.Add(info);
            }
            return categoryInfo;
        }

        internal ObservableCollection<PizzaInfo> GetPizzaInfo1()
        {
            var categoryInfo = new ObservableCollection<PizzaInfo>();

            for (int i = 0; i < PizzaNames1.Count(); i++)
            {
                var info = new PizzaInfo() { PizzaName = PizzaNames1[i] };

                if (i == 9)
                    info.PizzaImage = "pizza12.jpg";
                else
                    info.PizzaImage = "pizza" + (i + 9) + ".jpg";

                categoryInfo.Add(info);
            }
            return categoryInfo;
        }

        #endregion

        #region CategoryInfo

        string[] PizzaNames = new string[]
        {
            "Supreme",
            "GodFather",
            "Ciao-ciao",
            "Frutti di mare",
            "Kebabpizza",
            "Napolitana",
            "Apricot Chicken",
            "Lamb Tzatziki",
            "Mr Wedge",
            "Vegorama",
        };

        string[] PizzaNames1 = new string[]
        {
            "Margherita",
            "Funghi",
            "Capriciosa",
            "Stagioni",
            "Vegetariana",
            "Formaggi",
            "Marinara",
            "Peperoni",
            "apolitana",
            "Hawaii"
        };

        #endregion
    }
}
