using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleBrowser.Maui.SfListView
{
    public class PizzaInfo : INotifyPropertyChanged
    {
        #region Fields

        private string pizzaName;
        private ImageSource pizzaImage;

        #endregion

        #region Constructor

        public PizzaInfo()
        {

        }

        #endregion

        #region Properties

        public string PizzaName
        {
            get { return pizzaName; }
            set
            {
                pizzaName = value;
                OnPropertyChanged("PizzaName");
            }
        }

        public ImageSource PizzaImage
        {
            get
            {
                return pizzaImage;
            }

            set
            {
                pizzaImage = value;
                OnPropertyChanged("PizzaImage");
            }
        }

        #endregion

        #region Interface Member

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string name)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(name));
        }

        #endregion
    }
}
