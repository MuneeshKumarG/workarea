using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleBrowser.Maui.SfListView
{
    
    public class ListViewBookInfo : INotifyPropertyChanged
    {
        #region Fields

        private string bookName;
        private string bookDesc;
        private string bookAuthor;
        private ImageSource _authorImage;
        private bool indicatorIsVisible;
        private int gridvisible;

        #endregion

        #region Constructor

        public ListViewBookInfo()
        {

        }

        #endregion

        #region Properties

        public string BookName
        {
            get { return bookName; }
            set
            {
                bookName = value;
                OnPropertyChanged("BookName");
            }
        }
        public bool IndicatorIsVisible
        {
            get { return indicatorIsVisible; }
            set
            {
                this.indicatorIsVisible = value;
            }
        }
        public int GridIsVisible
        { get { return gridvisible; } set { this.gridvisible = value; } }
        public string BookDescription
        {
            get { return bookDesc; }
            set
            {
                bookDesc = value;
                OnPropertyChanged("BookDescription");
            }
        }

        public string BookAuthor
        {
            get { return bookAuthor; }
            set
            {
                bookAuthor = value;
                OnPropertyChanged("BookAuthor");
            }
        }

        public ImageSource AuthorImage
        {
            get { return _authorImage; }
            set
            {
                _authorImage = value;
                OnPropertyChanged("AuthorImage");
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
