using System;
using Microsoft.UI.Xaml;
using Windows.Foundation;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Printing;
using Windows.Graphics.Printing;
using Windows.Graphics.Printing.OptionDetails;
using Windows.UI.ViewManagement;
using ChartTrackBallBehavior = Syncfusion.UI.Xaml.Charts.ChartTrackballBehavior;
using ChartCrossHairBehavior = Syncfusion.UI.Xaml.Charts.ChartCrosshairBehavior;
using Windows.UI.Core;

namespace Syncfusion.UI.Xaml.Charts
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1805: Do not initialize unnecessarily")]
    internal class Printing
    {
        /// <summary>
        /// Print the chart.
        /// </summary>
        /// <param name="chart"></param>
        public Printing(ChartBase chart)
        {
            this.Chart = chart;
        }

        private ChartBase chart;

        /// <summary>
        /// Gets or sets the chart
        /// </summary>
        public ChartBase Chart 
        {
            get { return chart; }
            set { chart = value; }
        }

#if NETFX_CORE
        public static FrameworkElement Layout(FrameworkElement element, Size PrintableArea, string Document, HorizontalAlignment HorizontalAlignment, VerticalAlignment VerticalAlignment, Thickness PageMargin, bool PrintLandscape, bool ShrinkToFit)
#endif
        {

            Size elementSize = new Size(element.Width, element.Height);

            if (double.IsNaN(element.ActualWidth) || double.IsNaN(element.ActualHeight))
            {
                throw new Exception(ChartLocalizationResourceAccessor.Instance.GetLocalizedStringResource("PrintingExceptionMessage"));
            }

            TransformGroup transformGroup = new TransformGroup();
            ScaleTransform scaleTransform = new ScaleTransform();
            ScaleTransform horizonatlStretch = null;
            ScaleTransform verticalStrecth = null;
            //First move to middle of page...
            transformGroup.Children.Add(new TranslateTransform()
            {
                X = (PrintableArea.Width - elementSize.Width) / 2,
                Y = (PrintableArea.Height - elementSize.Height) / 2
            });
            double scaleX = 1;
            double scaleY = 1;
            if (PrintLandscape)
            {
                //Then, rotate around the center
                transformGroup.Children.Add(new RotateTransform()
                {
                    Angle = 90,
                    CenterX = PrintableArea.Width / 2,
                    CenterY = PrintableArea.Height / 2
                });

                if (ShrinkToFit)
                {
                    if ((elementSize.Width + PageMargin.Left +
                         PageMargin.Right) > PrintableArea.Height)
                    {
                        //elementSize.Width = PrintableArea.Width;
                        scaleX = Math.Round(PrintableArea.Height /
                                            (elementSize.Width + PageMargin.Left + PageMargin.Right), 2);
                    }
                    if ((elementSize.Height + PageMargin.Top + PageMargin.Bottom) >
                        PrintableArea.Width)
                    {
                        double scale2 = Math.Round(PrintableArea.Width /
                                                   (elementSize.Height + PageMargin.Top + PageMargin.Bottom), 2);
                        scaleY = (scale2 < scaleY) ? scale2 : scaleY;
                    }
                }
            }
            else if (ShrinkToFit)
            {
                //Scale down to fit the page + margin

                if ((elementSize.Width + PageMargin.Left +
                     PageMargin.Right) > PrintableArea.Width)
                {
                    //elementSize.Width = PrintableArea.Width;
                    scaleX = Math.Round(PrintableArea.Width /
                                        (elementSize.Width + PageMargin.Left + PageMargin.Right), 2);
                }
                if ((elementSize.Height + PageMargin.Top + PageMargin.Bottom) >
                    PrintableArea.Height)
                {
                    double scale2 = Math.Round(PrintableArea.Height /
                                               (elementSize.Height + PageMargin.Top + PageMargin.Bottom), 2);
                    scaleY = (scale2 < scaleY) ? scale2 : scaleY;
                }
            }


            scaleTransform = new ScaleTransform()
            {
                ScaleX = scaleX,
                ScaleY = scaleY,
                CenterX = PrintableArea.Width / 2,
                CenterY = PrintableArea.Height / 2
            };


            if (VerticalAlignment == VerticalAlignment.Top)
            {
                //Now move to Top
                if (PrintLandscape)
                {
                    transformGroup.Children.Add(new TranslateTransform()
                    {
                        X = 0,
                        Y = PageMargin.Top - (PrintableArea.Height -
                                                  (elementSize.Width * scaleY)) / 2
                    });
                }
                else
                {
                    transformGroup.Children.Add(new TranslateTransform()
                    {
                        X = 0,
                        Y = PageMargin.Top - (PrintableArea.Height -
                                                  (elementSize.Height * scaleX)) / 2
                    });
                }
            }
            else if (VerticalAlignment == VerticalAlignment.Bottom)
            {
                //Now move to Bottom
                if (PrintLandscape)
                {
                    transformGroup.Children.Add(new TranslateTransform()
                    {
                        X = 0,
                        Y = ((PrintableArea.Height -
                                  (elementSize.Width * scaleY)) / 2) - PageMargin.Bottom
                    });
                }
                else
                {
                    transformGroup.Children.Add(new TranslateTransform()
                    {
                        X = 0,
                        Y = ((PrintableArea.Height -
                                  (elementSize.Height * scaleY)) / 2) - PageMargin.Bottom
                    });
                }
            }
            else if (VerticalAlignment == VerticalAlignment.Stretch)
            {
                scaleY = Math.Round(PrintableArea.Height / (elementSize.Height + PageMargin.Top + PageMargin.Bottom), 2);
                verticalStrecth = new ScaleTransform()
                {
                    ScaleX = scaleX,
                    ScaleY =
                            Math.Round(PrintableArea.Height / (elementSize.Height + PageMargin.Top + PageMargin.Bottom), 2),
                    CenterX = PrintableArea.Width / 2,
                    CenterY = PrintableArea.Height / 2
                };

            }
            if (HorizontalAlignment == HorizontalAlignment.Left)
            {
                //Now move to Left
                if (PrintLandscape)
                {
                    transformGroup.Children.Add(new TranslateTransform()
                    {
                        X = PageMargin.Left - (PrintableArea.Width -
                                                   (elementSize.Height * scaleY)) / 2,
                        Y = 0
                    });
                }
                else
                {
                    transformGroup.Children.Add(new TranslateTransform()
                    {
                        X = PageMargin.Left - (PrintableArea.Width -
                                                   (elementSize.Width * scaleY)) / 2,
                        Y = 0
                    });
                }
            }
            else if (HorizontalAlignment == HorizontalAlignment.Stretch)
            {
                scaleX = Math.Round(PrintableArea.Width / (elementSize.Width + PageMargin.Left + PageMargin.Right), 2);
                horizonatlStretch = new ScaleTransform()
                {
                    ScaleX = Math.Round(PrintableArea.Width / (elementSize.Width + PageMargin.Left + PageMargin.Right), 2),
                    ScaleY = scaleY,
                    CenterX = PrintableArea.Width / 2,
                    CenterY = PrintableArea.Height / 2
                };

            }
            else if (HorizontalAlignment == HorizontalAlignment.Right)
            {
                //Now move to Right
                if (PrintLandscape)
                {
                    transformGroup.Children.Add(new TranslateTransform()
                    {
                        X = ((PrintableArea.Width -
                                  (elementSize.Height * scaleX)) / 2) - PageMargin.Right,
                        Y = 0
                    });
                }
                else
                {
                    transformGroup.Children.Add(new TranslateTransform()
                    {
                        X = ((PrintableArea.Width -
                                  (elementSize.Width * scaleX)) / 2) - PageMargin.Right,
                        Y = 0
                    });
                }
            }
            if (verticalStrecth != null)
            {
                transformGroup.Children.Add(verticalStrecth);
            }
            if (horizonatlStretch != null)
            {
                if (transformGroup.Children.Contains(verticalStrecth))
                    transformGroup.Children.Remove(verticalStrecth);
                transformGroup.Children.Add(horizonatlStretch);
            }
            if (horizonatlStretch == null && verticalStrecth == null)
            {
                transformGroup.Children.Add(scaleTransform);
            }
#if NETFX_CORE
            element.RenderTransform = transformGroup;
            return element;
#endif
        }

        //Print Chart in WinRT
#if NETFX_CORE
        private ChartBase printChart;
        PrintManager printManager;
        private PrintDocument document;
        private Size? _pageSize;
        private UIElement page;
        private Thickness margin;
        private HorizontalAlignment horizontalAllignment;
        private VerticalAlignment verticalAllignment;
        private bool isLandscape = false;
        private bool isRegisteredForPrinting;

        public void RegisterPrinting()
        {
            this.printChart = (ChartBase)this.chart.Clone();
            printManager = PrintManager.GetForCurrentView();
            printManager.PrintTaskRequested += printManager_PrintTaskRequested;
            margin = new Thickness().GetThickness(0, 0, 0, 0);

            horizontalAllignment = HorizontalAlignment.Stretch;
            verticalAllignment = VerticalAlignment.Stretch;

        }
        public void UnRegisterPrinting()
        {
            printManager.PrintTaskRequested -= printManager_PrintTaskRequested;
            if (document == null)
                return;
            document.Paginate -= document_Paginate;
            document.GetPreviewPage -= document_GetPreviewPage;
            document.AddPages -= document_AddPages;
            page = null;
        }

        void AddCustomPrintOption(PrintTask printTask)
        {
            PrintTaskOptionDetails Options = PrintTaskOptionDetails.GetFromPrintTaskOptions(printTask.Options);
            PrintCustomItemListOptionDetails horizontalOptions = Options.CreateItemListOption("horizontalAllignment", "HorizontalAllignment");
            var horizontaloptions = Enum.GetNames(typeof(HorizontalAlignment));
            foreach (var item in horizontaloptions)
            {
                horizontalOptions.AddItem(item, item);
            }
            horizontalOptions.TrySetValue(horizontaloptions[3]);
            PrintCustomItemListOptionDetails verticalOptions = Options.CreateItemListOption("verticalAllignment", "VerticalAllignment");
            var verticaloptions = Enum.GetNames(typeof(VerticalAlignment));
            foreach (var item in verticaloptions)
            {
                verticalOptions.AddItem(item, item);
            }
            verticalOptions.TrySetValue(verticaloptions[3]);
            Options.CreateTextOption("marginText", "Margin");
            Options.DisplayedOptions.Add("horizontalAllignment");
            Options.DisplayedOptions.Add("verticalAllignment");
            Options.DisplayedOptions.Add("marginText");
            Options.OptionChanged += details_OptionChanged;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "action", Justification = "Reviewed")]
        void details_OptionChanged(PrintTaskOptionDetails sender, PrintTaskOptionChangedEventArgs args)
        {
            if (args.OptionId != null)
            {

                if ((string)args.OptionId == "horizontalAllignment")
                {
                    PrintTaskOptionDetails optionDetails = (PrintTaskOptionDetails)sender;
                    string value = optionDetails.Options["horizontalAllignment"].Value as string;

                    if (!string.IsNullOrEmpty(value))
                    {
#if WinUI_Desktop
                        Chart.DispatcherQueue.TryEnqueue(
#else
                        IAsyncAction action = Chart.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
#endif
                            () =>
                            {
                                if (value == "Left")
                                    horizontalAllignment = HorizontalAlignment.Left;
                                if (value == "Right")
                                    horizontalAllignment = HorizontalAlignment.Right;
                                if (value == "Center")
                                    horizontalAllignment = HorizontalAlignment.Center;
                                if (value == "Stretch")
                                    horizontalAllignment = HorizontalAlignment.Stretch;
                                document.InvalidatePreview();
                            });
                    }
                }

                if ((string)args.OptionId == "verticalAllignment")
                {
                    PrintTaskOptionDetails optionDetails = (PrintTaskOptionDetails)sender;
                    string value = optionDetails.Options["verticalAllignment"].Value as string;

                    if (!string.IsNullOrEmpty(value))
                    {
#if WinUI_Desktop
                        Chart.DispatcherQueue.TryEnqueue(
#else
                        IAsyncAction action = Chart.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
#endif
                            () =>
                            {
                                if (value == "Bottom")
                                    verticalAllignment = VerticalAlignment.Bottom;
                                if (value == "Top")
                                    verticalAllignment = VerticalAlignment.Top;
                                if (value == "Center")
                                    verticalAllignment = VerticalAlignment.Center;
                                if (value == "Stretch")
                                    verticalAllignment = VerticalAlignment.Stretch;
                                document.InvalidatePreview();
                            });
                    }
                }
                if ((string)args.OptionId == "marginText")
                {
                    PrintTaskOptionDetails optionDetails = (PrintTaskOptionDetails)sender;
                    string value = optionDetails.Options["marginText"].Value as string;

                    if (!string.IsNullOrEmpty(value))
                    {
#if WinUI_Desktop
                        Chart.DispatcherQueue.TryEnqueue(
#else
                        IAsyncAction action = Chart.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
#endif
                            () =>
                            {
                                double marginValue = 0;

                                if (double.TryParse(value, out marginValue))
                                {
                                    margin = new Thickness().GetThickness(marginValue, marginValue, marginValue, marginValue);
                                }
                                document.InvalidatePreview();
                            });
                    }
                }
            }

        }

        void printManager_PrintTaskRequested(PrintManager sender, PrintTaskRequestedEventArgs args)
        {
            var deferral = args.Request.GetDeferral();
            PrintTask printTask = args.Request.CreatePrintTask("Printing", OnPrintTaskSourceRequestedHandler);
            printTask.Completed += OnPrintTaskCompleted;
            this.AddCustomPrintOption(printTask);
            deferral.Complete();
        }

#if WinUI_Desktop
        void OnPrintTaskCompleted(PrintTask sender, PrintTaskCompletedEventArgs args)
#else
       async void OnPrintTaskCompleted(PrintTask sender, PrintTaskCompletedEventArgs args)
#endif
        {
            if (isRegisteredForPrinting)
            {
#if WinUI_Desktop
                Chart.DispatcherQueue.TryEnqueue(() => { UnRegisterPrinting(); });
#else
                await Chart.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, UnRegisterPrinting);
#endif
                isRegisteredForPrinting = false;
                printChart = null;
            }
        }

#if WinUI_Desktop
        void OnPrintTaskSourceRequestedHandler(PrintTaskSourceRequestedArgs args)
#else
        async void OnPrintTaskSourceRequestedHandler(PrintTaskSourceRequestedArgs args)
#endif
        {
            var deferral = args.GetDeferral();

#if WinUI_Desktop
            Chart.DispatcherQueue.TryEnqueue((
#else
            await Chart.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, new DispatchedHandler(
#endif
                 () =>
                 {
                     document = new PrintDocument();
                     document.GetPreviewPage += document_GetPreviewPage;
                     document.AddPages += document_AddPages;
                     document.Paginate += document_Paginate;
                     args.SetSource(document.DocumentSource);
                 }));

            deferral.Complete();
        }

#if WinUI_Desktop
        void document_Paginate(object sender, PaginateEventArgs e)
#else
        async void document_Paginate(object sender, PaginateEventArgs e)
#endif
        {
#if WinUI_Desktop
            Chart.DispatcherQueue.TryEnqueue(
#else
            await Chart.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
#endif
             () =>
             {
                 this.GetPageSize(e);
                 document.SetPreviewPageCount(1, PreviewPageCountType.Intermediate);
             });
        }

#if WinUI_Desktop
        void document_AddPages(object sender, AddPagesEventArgs e)
#else
        async void document_AddPages(object sender, AddPagesEventArgs e)
#endif
        {
#if WinUI_Desktop
            Chart.DispatcherQueue.TryEnqueue(
#else
            await Chart.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
#endif
                 () =>
                 {
                     document.AddPage(printChart);
                     document.AddPagesComplete();
                 });
        }

#if WinUI_Desktop
        void document_GetPreviewPage(object sender, GetPreviewPageEventArgs e)
#else
        async void document_GetPreviewPage(object sender, GetPreviewPageEventArgs e)
#endif
        {
#if WinUI_Desktop
            Chart.DispatcherQueue.TryEnqueue(
#else
            await Chart.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
#endif
                () =>
                {
                    PrintDocument printDoc = (PrintDocument)sender;
                    printChart.Height = printChart.Height > _pageSize.Value.Height ? _pageSize.Value.Height : printChart.Height;
                    printChart.Width = printChart.Width > _pageSize.Value.Width ? _pageSize.Value.Width : printChart.Width;

                    page = Layout(printChart, new Size(_pageSize.Value.Width, _pageSize.Value.Height), "Printing", horizontalAllignment, verticalAllignment, margin, isLandscape, true);
                    printDoc.SetPreviewPage(e.PageNumber, page);

                    var chartLegend = printChart.Legend as ChartLegend;

                    // Update legend in print prview layout.
                    if (chartLegend != null)
                    {
                        chartLegend.ChangeOrientation();
                    }

                    if (printChart is ChartBase)
                    {
                        var sfChart = printChart as ChartBase;

                        for (int i = 0; i < sfChart.Behaviors.Count; i++)
                        {
                            if (sfChart.Behaviors[i] is ChartTrackBallBehavior)
                            {
                                (sfChart.Behaviors[i] as ChartTrackBallBehavior).IsActivated = true;
                                (sfChart.Behaviors[i] as ChartTrackBallBehavior).CurrentPoint = ((chart as ChartBase).Behaviors[i] as ChartTrackBallBehavior).CurrentPoint;
                                (sfChart.Behaviors[i] as ChartTrackBallBehavior).OnPointerPositionChanged();
                            }
                            if (sfChart.Behaviors[i] is ChartCrossHairBehavior)
                            {
                                (sfChart.Behaviors[i] as ChartCrossHairBehavior).SetPosition(((chart as ChartBase).Behaviors[i] as ChartCrossHairBehavior).CurrentPoint);
                            }
                        }
#if WinUI_Desktop
                        if (!sfChart.isRenderSeriesDispatched)
#else
                            if (sfChart.renderSeriesAction != null)
#endif
                        {
                            sfChart.RenderSeries();
                        }
                    }

                    printDoc.SetPreviewPage(e.PageNumber, page);
                });
        }

        void GetPageSize(PaginateEventArgs e)
        {
            PrintPageDescription description = e.PrintTaskOptions.GetPageDescription(
                   (uint)e.CurrentPreviewPageNumber);

            this._pageSize = description.PageSize;
        }

        async public void Print()
        {
            if (!isRegisteredForPrinting)
            {
                this.RegisterPrinting();
                isRegisteredForPrinting = true;
            }
#pragma warning disable 618
            if (ApplicationView.Value != ApplicationViewState.Snapped)
#pragma warning restore 618
            {
                try
                {
                    //Pop print charm window.
                    await Windows.Graphics.Printing.PrintManager.ShowPrintUIAsync();
                }
                catch
                {
                    //Pop Error Message if printing is failed. 
                    PrintErrorMessage();
                }
            }
        }

        async static private void PrintErrorMessage()
        {
            var messageDialog = new Windows.UI.Popups.MessageDialog(ChartLocalizationResourceAccessor.Instance.GetLocalizedStringResource("PrintErrorMessage"));
            await messageDialog.ShowAsync();
        }
#endif

    }
  
}
