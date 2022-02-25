using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using Microsoft.Maui;
using Syncfusion.Maui.ListView;
using SelectionMode = Syncfusion.Maui.ListView.SelectionMode;
using System.Drawing;
using SampleBrowser.Maui.Core;

namespace SampleBrowser.Maui.SfListView
{
    public class SfListViewGridLayoutBehavior : Behavior<SampleView>
    {
        #region Fields

        private Syncfusion.Maui.ListView.SfListView listView;
        private GridLayoutViewModel viewModel;

        #endregion

        #region Overrides

        protected override void OnAttachedTo(SampleView bindable)
        {
            listView = bindable.FindByName<Syncfusion.Maui.ListView.SfListView>("listView");
            viewModel = bindable.FindByName<GridLayoutViewModel>("viewModel");
            base.OnAttachedTo(bindable);

            // We must not measure or invalidate during a layout pass,
            // which might cause a indefinite loop as mentioned in the below doc
            // https://docs.microsoft.com/en-us/windows/apps/design/layout/custom-panels-overview#other-layout-api,
            // also as mentioned it is safer to use SizeChanged event.
#if (__ANDROID__ || __IOS__ || __MACCATALYST__)
            bindable.PropertyChanged += View_PropertyChanged;
#else
            bindable.HandlerChanged += Bindable_HandlerChanged;
#endif
        }

        protected override void OnDetachingFrom(SampleView bindable)
        {
#if (__ANDROID__ || __IOS__ || __MACCATALYST__)
            bindable.PropertyChanged -= View_PropertyChanged;
#else
            bindable.HandlerChanged -= Bindable_HandlerChanged;
#endif
            listView = null;
            viewModel = null;
            base.OnDetachingFrom(bindable);
        }

#endregion

#region CallBacks

#if !(__ANDROID__ || __IOS__ || __MACCATALYST__)
        private void Bindable_HandlerChanged(object sender, EventArgs e)
        {
            if (sender is GridLayout && (sender as GridLayout).Handler != null)
            {
                ((sender as GridLayout).Handler.NativeView as Microsoft.UI.Xaml.Controls.Panel).SizeChanged += this.GridSizeChanged;
            }
        }

        private void GridSizeChanged(object sender, Microsoft.UI.Xaml.SizeChangedEventArgs e)
        {
            double spanCount = (this.listView.ItemsLayout as Syncfusion.Maui.ListView.GridLayout).SpanCount;
            //Below calulation is to find the individual imageWidth
            this.viewModel.ImageHeightRequest = (e.NewSize.Width - (spanCount * this.listView.ItemSpacing.Left) - (spanCount * this.listView.ItemSpacing.Right)) / spanCount;
        }

#else
        private void View_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Width")
            {
                double spanCount = (this.listView.ItemsLayout as Syncfusion.Maui.ListView.GridLayout).SpanCount;
                //Below calulation is to find the individual imageWidth
                this.viewModel.ImageHeightRequest = ((sender as SampleView).Width - (spanCount * this.listView.ItemSpacing.Left) - (spanCount * this.listView.ItemSpacing.Right)) / spanCount;
            }
        }

#endif
#endregion

    }
}
