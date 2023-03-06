// <copyright file="ChartAdornmentContainer.cs" company="Syncfusion. Inc">
// Copyright Syncfusion Inc. 2001 - 2017. All rights reserved.
// Use of this code is subject to the terms of our license.
// A copy of the current license can be obtained at any time by e-mailing
// licensing@syncfusion.com. Any infringement will be prosecuted under
// applicable laws. 
// </copyright>
namespace Syncfusion.UI.Xaml.Charts
{
    using System;
    using Microsoft.UI.Xaml;
    using Microsoft.UI.Xaml.Data;
    using Microsoft.UI.Xaml.Controls;
    using Microsoft.UI.Xaml.Media;
    using Windows.Foundation;
    using Microsoft.UI;

    /// <summary>
    /// Represents the panel which contains all the ChartAdornment elements.
    /// </summary>
    /// <remarks>
    /// The elements inside the panel comprises of adornment labels, marker symbols and connector lines to connect the labels.
    /// </remarks>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1805: Do not initialize unnecessarily")]
    internal class ChartDataMarkerContainer : Panel
    {
#region Dependency Property Registration

        /// <summary>
        /// The DependencyProperty for <see cref="LabelVerticalAlignment"/> property.  
        /// </summary>
        public static readonly DependencyProperty LabelVerticalAlignmentProperty =
          DependencyProperty.Register(
              "LabelVerticalAlignment",
              typeof(VerticalAlignment),
              typeof(ChartDataMarkerContainer),
              new PropertyMetadata(VerticalAlignment.Center));

        /// <summary>
        /// The DependencyProperty for <see cref="LabelHorizontalAlignment"/> property.  
        /// </summary>
        public static readonly DependencyProperty LabelHorizontalAlignmentProperty =
          DependencyProperty.Register(
              "LabelHorizontalAlignment",
              typeof(HorizontalAlignment),
              typeof(ChartDataMarkerContainer),
              new PropertyMetadata(HorizontalAlignment.Center));

        /// <summary>
        /// The DependencyProperty for <see cref="MarkerType"/> property.  
        /// </summary>
        public static readonly DependencyProperty MarkerTypeProperty =
            DependencyProperty.Register(
                "MarkerType",
                typeof(Object),
                typeof(ChartDataMarkerContainer),
                new PropertyMetadata(ShapeType.Custom, new PropertyChangedCallback(OnAdornmentsInfoChanged)));

        #endregion

        #region Fields

        private Point m_symbolOffset = new Point();

        /// <summary>
        /// Initializes m_symbolPresenter.
        /// </summary>
        private ContentPresenter m_symbolPresenter = new ContentPresenter();

        private ChartDataLabel adornment;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the label vertical alignment.
        /// </summary>
        /// <value>The label vertical alignment.</value>
        public VerticalAlignment LabelVerticalAlignment
        {
            get { return (VerticalAlignment)GetValue(LabelVerticalAlignmentProperty); }
            set { SetValue(LabelVerticalAlignmentProperty, value); }
        }

        /// <summary>
        /// Gets or sets the label horizontal alignment.
        /// </summary>
        /// <value>The label horizontal alignment.</value>       
        public HorizontalAlignment LabelHorizontalAlignment
        {
            get { return (HorizontalAlignment)GetValue(LabelHorizontalAlignmentProperty); }
            set { SetValue(LabelHorizontalAlignmentProperty, value); }
        }

        /// <summary>
        /// Gets or sets the chart symbol
        /// </summary>      
        public ShapeType MarkerType
        {
            get { return (ShapeType)GetValue(MarkerTypeProperty); }
            set { SetValue(MarkerTypeProperty, value); }
        }

        /// <summary>
        /// Gets the symbol offset.
        /// </summary>
        /// <value>The symbol offset.</value>       
        public Point MarkerOffset
        {
            get { return this.m_symbolOffset; }
        }

        /// <summary>
        /// Gets or sets the <see cref="ChartDataLabel"/>.
        /// </summary>
        internal ChartDataLabel Adornment
        {
            get
            {
                return adornment;
            }
            set
            {
                if (value != adornment)
                {
                    adornment = value;
                    UpdateContainers(true);
                }
                else
                {
                    UpdateContainers(false);
                }

            }
        }

        /// <summary>
        /// Gets or sets the pre-defined adornment symbol.
        /// </summary>
        internal SymbolControl PredefinedSymbol { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ChartDataMarkerContainer"/> class.
        /// </summary>
        public ChartDataMarkerContainer()
        {
            PredefinedSymbol = new SymbolControl();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ChartDataMarkerContainer"/> class.
        /// </summary>
        /// <param name="adornment"></param>
        public ChartDataMarkerContainer(ChartDataLabel adornment)
        {
            PredefinedSymbol = new SymbolControl();
            Adornment = adornment;
        }

        #endregion

        #region Methods

        #region Internal Methods    

        internal void Dispose()
        {
            m_symbolPresenter.SetValue(ContentPresenter.ContentTemplateProperty, null);
            m_symbolPresenter.SetValue(ContentPresenter.ContentProperty, null);

            adornment = null;
            PredefinedSymbol = null;
            m_symbolPresenter = null;
        }

        //WPF-13972 While dynamically change the symbol property, the previous symbol not clear in view and While changing the symbol property from pre-defined to custom dynamically ,custom symbol is not showing.
        /// <summary>
        /// Updates the adornment containers.
        /// </summary>
        internal void UpdateContainers(bool setBinding)
        {
            //Removed CanHideLabel validation for double.Nan and it is already addressed in SetSymbol method.
            if (Adornment != null)
            {
                var adornmentInfo = Adornment.Series.adornmentInfo;
                LabelVerticalAlignment = adornmentInfo.VerticalAlignment;
                LabelHorizontalAlignment = adornmentInfo.HorizontalAlignment;
                MarkerType = adornmentInfo.MarkerType;
                SetSymbol(MarkerType.ToString());

                if (setBinding)
                    SetContentBinding(Adornment);
            }
            else
            {
                if (Children.Contains(m_symbolPresenter))
                {
                    Children.Remove(m_symbolPresenter);
                }
                if (Children.Contains(PredefinedSymbol))
                {
                    Children.Remove(PredefinedSymbol);
                }
            }
        }

        #endregion

        #region Protected Methods

        /// <inheritdoc/>
        protected override Size ArrangeOverride(Size finalSize)
        {
            Rect arrangeRect = new Rect(new Point(), DesiredSize);
            Rect arrangePieRect = new Rect(new Point(), DesiredSize);
            switch (LabelHorizontalAlignment)
            {
                case HorizontalAlignment.Stretch:
                case HorizontalAlignment.Center:
                    {
                        m_symbolPresenter.HorizontalAlignment = HorizontalAlignment.Center;
                        PredefinedSymbol.HorizontalAlignment = HorizontalAlignment.Center;
                    }

                    break;

                case HorizontalAlignment.Left:
                    {
                        m_symbolPresenter.HorizontalAlignment = HorizontalAlignment.Right;
                        PredefinedSymbol.HorizontalAlignment = HorizontalAlignment.Right;
                    }

                    break;

                case HorizontalAlignment.Right:
                    {
                        m_symbolPresenter.HorizontalAlignment = HorizontalAlignment.Left;
                        PredefinedSymbol.HorizontalAlignment = HorizontalAlignment.Left;
                    }

                    break;
            }

            switch (this.LabelVerticalAlignment)
            {
                case VerticalAlignment.Bottom:
                    {
                        m_symbolPresenter.VerticalAlignment = VerticalAlignment.Top;
                        PredefinedSymbol.VerticalAlignment = VerticalAlignment.Top;
                    }

                    break;
                case VerticalAlignment.Stretch:
                case VerticalAlignment.Center:
                    {
                        m_symbolPresenter.VerticalAlignment = VerticalAlignment.Center;
                        PredefinedSymbol.VerticalAlignment = VerticalAlignment.Center;
                    }

                    break;
                case VerticalAlignment.Top:
                    {
                        m_symbolPresenter.VerticalAlignment = VerticalAlignment.Bottom;
                        PredefinedSymbol.VerticalAlignment = VerticalAlignment.Bottom;
                    }

                    break;
            }

            m_symbolPresenter.Arrange(arrangeRect);
            PredefinedSymbol.Arrange(arrangeRect);

            return this.DesiredSize;
        }

        /// <inheritdoc/>
        protected override Size MeasureOverride(Size availableSize)
        {
            if (Adornment == null)
                return ChartLayoutUtils.CheckSize(availableSize);
            PredefinedSymbol.Measure(availableSize);
            m_symbolPresenter.Measure(availableSize);

            Size resultSize = new Size();
            Size lblSz = Size.Empty;
            Size sblSz;

            if (Adornment.Series.adornmentInfo.MarkerType == ShapeType.Custom)
            {
                sblSz = m_symbolPresenter.DesiredSize;
            }
            else
            {
                sblSz = PredefinedSymbol.DesiredSize;
            }

            switch (LabelHorizontalAlignment)
            {
                case HorizontalAlignment.Stretch:
                case HorizontalAlignment.Center:
                    {
                        resultSize.Width = Math.Max(lblSz.Width, sblSz.Width);
                        m_symbolOffset.X = 0.5 * resultSize.Width;
                    }

                    break;

                case HorizontalAlignment.Left:
                    resultSize.Width = Math.Max(lblSz.Width, sblSz.Width);
                    m_symbolOffset.X = 0.5 * resultSize.Width;
                    break;

                case HorizontalAlignment.Right:
                    resultSize.Width = Math.Max(lblSz.Width, sblSz.Width);
                    m_symbolOffset.X = 0.5 * resultSize.Width;
                    break;
            }

            switch (LabelVerticalAlignment)
            {
                case VerticalAlignment.Bottom:
                    resultSize.Height = Math.Max(lblSz.Height, sblSz.Height);
                    m_symbolOffset.Y = 0.5 * resultSize.Height;

                    break;
                case VerticalAlignment.Stretch:
                case VerticalAlignment.Center:
                    {
                        resultSize.Height = Math.Max(lblSz.Height, sblSz.Height);
                        m_symbolOffset.Y = 0.5 * resultSize.Height;
                    }

                    break;
                case VerticalAlignment.Top:
                    resultSize.Height = Math.Max(lblSz.Height, sblSz.Height);
                    m_symbolOffset.Y = 0.5 * resultSize.Height;

                    break;
            }

            return resultSize;
        }

        #endregion

        #region Private Static Methods

        /// <summary>
        /// Updates the adornment symbol on <see cref="ChartDataLabelSettings"/> changed.
        /// </summary>
        /// <param name="d">The Dependency Object</param>
        /// <param name="e">The Event Arguments</param>
        private static void OnAdornmentsInfoChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var chartAdornmentContainer = d as ChartDataMarkerContainer;
            chartAdornmentContainer.SetSymbol(chartAdornmentContainer.MarkerType.ToString());
        }

        #endregion

        #region Private Methods
        /// <summary>
        /// Sets the binding between the adornment symbol and <see cref="ChartDataLabelSettings"/>
        /// </summary>
        /// <param name="adornment"></param>
        private void SetContentBinding(ChartDataLabel adornment)
        {
            ChartDataLabelSettings adornmentInfo = adornment.Series.adornmentInfo;

            if (adornmentInfo.ShowMarker)
            {
                object source = adornmentInfo; 

                SetBinding(MarkerTypeProperty
                        , CreateAdormentBinding("MarkerType", source));

                PredefinedSymbol.SetBinding(HeightProperty
                    , CreateAdormentBinding("MarkerHeight", source));

                PredefinedSymbol.SetBinding(WidthProperty
                    , CreateAdormentBinding("MarkerWidth", source));

                SeriesSelectionBehavior selectionBehavior = adornment.Series.ActualArea.GetSeriesSelectionBehavior();
                if (adornment.Series.ActualArea.SelectedSeriesCollection.Contains(adornment.Series)
                    && adornmentInfo.HighlightOnSelection
                    && adornment.Series.ActualArea.GetSeriesSelectionBrush(adornment.Series) != null
                    && selectionBehavior != null)
                    PredefinedSymbol.SetBinding(BackgroundProperty
                  , CreateAdormentBinding(nameof(selectionBehavior.SelectionBrush), selectionBehavior));
                else if (adornment.Series.SelectedSegmentsIndexes.Contains(adornment.Series.ActualData.IndexOf(adornment.Item))
                    && adornmentInfo.HighlightOnSelection
                    && adornment.Series.GetEnableSegmentSelection()
                    && adornment.Series.SelectionBehavior != null
                    && adornment.Series.SelectionBehavior.SelectionBrush != null)
                    PredefinedSymbol.SetBinding(BackgroundProperty
                   , CreateAdormentBinding("SelectionBrush", adornment.Series.SelectionBehavior));
                else if (adornmentInfo.MarkerInterior == null)
                    PredefinedSymbol.SetBinding(BackgroundProperty
                   , CreateAdormentBinding("Fill", adornment));
                else
                    PredefinedSymbol.SetBinding(Control.BackgroundProperty
                   , CreateAdormentBinding("MarkerInterior", adornmentInfo));

                SeriesSelectionBehavior seriesSelection = adornment.Series.ActualArea.GetSeriesSelectionBehavior();

                if (adornment.Series.ActualArea.SelectedSeriesCollection.Contains(adornment.Series)
                    && adornmentInfo.HighlightOnSelection
                    && adornment.Series.ActualArea.GetSeriesSelectionBrush(adornment.Series) != null
                    && seriesSelection != null)
                    PredefinedSymbol.SetBinding(Control.BorderBrushProperty
                 , CreateAdormentBinding(nameof(seriesSelection.SelectionBrush), seriesSelection));
                else if (adornment.Series.SelectedSegmentsIndexes.Contains(adornment.Series.ActualData.IndexOf(adornment.Item))
                     && adornmentInfo.HighlightOnSelection
                     && adornment.Series.GetEnableSegmentSelection()
                     && adornment.Series.SelectionBehavior != null
                     && adornment.Series.SelectionBehavior.SelectionBrush != null)
                    PredefinedSymbol.SetBinding(Control.BorderBrushProperty
                   , CreateAdormentBinding("SelectionBrush", adornment.Series.SelectionBehavior));
                else
                    PredefinedSymbol.SetBinding(Control.BorderBrushProperty
                   , CreateAdormentBinding("MarkerStroke", source));

                if (adornmentInfo.MarkerTemplate != null)
                    m_symbolPresenter.SetBinding(ContentPresenter.ContentTemplateProperty
                         , CreateAdormentBinding("MarkerTemplate", adornmentInfo));

                m_symbolPresenter.SetBinding(ContentPresenter.ContentProperty
                    , CreateAdormentBinding("", adornment));
            }
        }

        /// <summary>
        /// Helper method to create the binding between symbol and <see cref="ChartDataLabelSettings"/>.
        /// </summary>
        /// <param name="path">The Binding Path</param>
        /// <param name="source">The Binding Source</param>
        /// <returns></returns>
        private Binding CreateAdormentBinding(string path, object source)
        {
            Binding bindingProvider = new Binding();
            bindingProvider.Path = new PropertyPath(path);
            bindingProvider.Source = source;
            bindingProvider.Mode = BindingMode.OneWay;

            if (path ==  "SelectionBrush")
            {
                bindingProvider.Converter = new SeriesSelectionBrushConverter(adornment.Series);
                bindingProvider.ConverterParameter = adornment.Series.ActualData.IndexOf(adornment.Item);
            }

            return bindingProvider;
        }

        /// <summary>
        /// Updates the symbol.
        /// </summary>
        /// <param name="symbol">The Symbol String</param>
        private void SetSymbol(string symbol)
        {
            ChartDataLabelSettings adornmentInfo = adornment.Series.adornmentInfo;
            if (symbol != "Custom")
            {
                if (Children.Contains(m_symbolPresenter))
                {
                    Children.Remove(m_symbolPresenter);
                }

                if (!Children.Contains(PredefinedSymbol))
                    Children.Add(PredefinedSymbol);
               
                if (double.IsNaN(adornment.YData) || double.IsNaN(adornment.XData) 
                    || ((adornment.Series is TriangularSeriesBase || adornment.Series is CircularSeries) && adornment.YData == 0))
                {
                    Children.Remove(PredefinedSymbol);
                }
                
                PredefinedSymbol.DataContext = this;
                PredefinedSymbol.Template = ChartDictionaries.GenericSymbolDictionary[symbol] as ControlTemplate;
            }
            else
            {
                if (adornmentInfo.MarkerTemplate != null && !this.Children.Contains(m_symbolPresenter))
                {
                    Children.Add(m_symbolPresenter);
                }

                if (Children.Contains(PredefinedSymbol))
                {
                    Children.Remove(PredefinedSymbol);
                }

                if (Double.IsNaN(adornment.YData) || double.IsNaN(adornment.XData))
                {
                    Children.Remove(PredefinedSymbol);
                }
            }
        }

        #endregion

        #endregion
    }

    /// <summary>
    /// A control that represents symbol in chart adornments
    /// </summary>   
    internal class SymbolControl : Control
    {
        #region Dependency Property Registration

        /// <summary>
        /// The DependencyProperty for <see cref="Stroke"/> property.
        /// </summary>
        public static readonly DependencyProperty StrokeProperty =
            DependencyProperty.Register(
                "Stroke",
                typeof(Brush),
                typeof(SymbolControl),
                new PropertyMetadata(new SolidColorBrush(Colors.Black)));

        #endregion

        #region Properties

        #region Public Properties

        /// <summary>
        /// Gets or sets the stroke
        /// </summary>
        /// <value>
        /// The <see cref="Brush"/> value.
        /// </value>
        public Brush Stroke
        {
            get { return (Brush)GetValue(StrokeProperty); }
            set { SetValue(StrokeProperty, value); }
        }

        #endregion

        #endregion
    }
}
