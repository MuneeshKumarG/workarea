using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace SampleBrowser.Maui.SfListView
{
    public class ListViewAutoFitContentViewModel
    {
        #region Fields

        private ObservableCollection<ListViewBookInfo> bookInfo;
        public bool indicatorIsVisible;
        public int gridvisible;

        #endregion

        #region Constructor

        public ListViewAutoFitContentViewModel()
        {
            GenerateSource();
        }

        #endregion

        #region Properties

        public ObservableCollection<ListViewBookInfo> BookInfo
        {
            get { return bookInfo; }
            set { this.bookInfo = value; }
        }

        public bool IndicatorIsVisible 
        { get { return indicatorIsVisible; } set {
                this.indicatorIsVisible = value;
                    }}

        public int GridIsVisible 
            { get { return gridvisible; }  set { this.gridvisible = value; } }

        #endregion

        #region Generate Source

        public void GenerateSource()
        {
            ListViewBookInfoRepository bookInfoRepository = new ListViewBookInfoRepository();
            bookInfo = bookInfoRepository.GetBookInfo();
        }

        #endregion
    }
}
