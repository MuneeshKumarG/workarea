using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Layouts;
using Syncfusion.Maui.Core.Internals;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;

namespace Syncfusion.Maui.Gauges
{
    /// <summary>
    /// Represents a radial gauge control that is used to displays numerical values on a circular scale. It has a rich set of features such as axes, ranges, pointers, and annotations that are fully customizable and extendable. Use it to create speedometers, temperature monitors, dashboards, meter gauges, multi axis clocks, watches, activity gauges, compasses, and more. 
    /// </summary>
    /// <example>
    /// The below examples shows, how to initialize the radial gauge.
    /// <code><![CDATA[
    /// <gauge:SfRadialGauge>
    ///     <gauge:SfRadialGauge.Axes>
    ///         <gauge:RadialAxis>
    ///             <gauge:RadialAxis.Pointers>
    ///                 <gauge:NeedlePointer x:Name="pointer" Value="60"/>
    ///             </ gauge:RadialAxis.Pointers>
    ///             <gauge:RadialAxis.Ranges>
    ///                 <gauge:RadialRange StartValue="0" EndValue="100">
    ///                     <gauge:RadialRange.GradientStops>
    ///                         <gauge:GaugeGradientStop Value="0" Color="Green" />
    ///                         <gauge:GaugeGradientStop Value="50" Color="Orange" />
    ///                         <gauge:GaugeGradientStop Value="100" Color="Red" />
    ///                     </gauge:RadialRange.GradientStops>
    ///                 </gauge:RadialRange>
    ///             </gauge:RadialAxis.Ranges>
    ///             <gauge:RadialAxis.Annotations>
    ///                 <gauge:GaugeAnnotation DirectionUnit="Angle" DirectionValue="90" PositionFactor="0.7" >
    ///                     <gauge:GaugeAnnotation.Content>
    ///                         <Label Text="{Binding Source={x:Reference pointer},Path=Value}" />
    ///                     </gauge:GaugeAnnotation.Content>
    ///                 </gauge:GaugeAnnotation>
    ///             </gauge:RadialAxis.Annotations>
    ///         </gauge:RadialAxis>
    ///     </gauge:SfRadialGauge.Axes>
    /// </gauge:SfRadialGauge>
    /// ]]></code>
    /// </example>
    [ContentProperty(nameof(Axes))]
    public class SfRadialGauge : View, IContentView, ITouchListener, IVisualTreeElement
    {
        #region Fields
        private bool isTouchHandled;
        private Grid parentGrid;
        
        #endregion

        #region Bindable properties

        /// <summary>
        /// Identifies the <see cref="Axes"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="Axes"/> bindable property.
        /// </value>
        public static readonly BindableProperty AxesProperty =
          BindableProperty.Create(nameof(Axes), typeof(ObservableCollection<RadialAxis>), typeof(SfRadialGauge),
              null, propertyChanged: OnAxesPropertyChanged);

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="SfRadialGauge"/> class.
        /// </summary>
        /// <example>
        /// The below examples shows, how to creates a radial gauge with required axis.
        /// <code><![CDATA[
        /// <gauge:SfRadialGauge>
        ///     <gauge:SfRadialGauge.Axes>
        ///         <gauge:RadialAxis />
        ///     </gauge:SfRadialGauge.Axes>
        /// </gauge:SfRadialGauge>
        /// ]]></code>
        /// </example>
        public SfRadialGauge()
        {
            Axes = new ObservableCollection<RadialAxis>();
            parentGrid = new Grid();
            this.AddTouchListener(this);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets a collection of <see cref="RadialAxis"/>.
        /// </summary>
        /// <value>
        /// The collection of axes to display multiple axis. The default value is empty collection.
        /// </value>
        /// <example>
        /// The below examples shows, how to add a collection of radial axis to the gauge and customize each axis by adding it to the axes collection.
        /// <code><![CDATA[
        /// <gauge:SfRadialGauge>
        ///     <gauge:SfRadialGauge.Axes>
        ///         <gauge:RadialAxis>
        ///             <gauge:RadialAxis.Pointers>
        ///                 <gauge:NeedlePointer Value="60" />
        ///             </ gauge:RadialAxis.Pointers>
        ///         </gauge:RadialAxis>
        ///         <gauge:RadialAxis RadiusFactor="0.5" />
        ///     </ gauge:SfRadialGauge.Axes>
        /// </gauge:SfRadialGauge>
        /// ]]></code>
        /// </example>
        public ObservableCollection<RadialAxis> Axes
        {
            get { return (ObservableCollection<RadialAxis>)this.GetValue(AxesProperty); }
            set { this.SetValue(AxesProperty, value); }
        }

        /// <summary>
        /// Gets the boolean value indicating to pass the touches on either the parent or child view.
        /// </summary>
        bool ITouchListener.IsTouchHandled
        {
            get
            {
                return isTouchHandled;
            }
        }

        /// <summary>
        /// Gets the root content.
        /// </summary>
        object IContentView.Content
        {
            get
            {
                return parentGrid;
            }
        }

        /// <summary>
        /// Gets the root content.
        /// </summary>
        IView? IContentView.PresentedContent
        {
            get
            {
                return parentGrid;
            }
        }

        /// <summary>
        /// Gets the content padding.
        /// </summary>
        Thickness IPadding.Padding
        {
            get
            {
                return new Thickness(0);
            }
        }

        #endregion
        
        #region Override methods

        /// <inheritdoc />
        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();

            if (parentGrid != null)
            {
                SetInheritedBindingContext(this.parentGrid, BindingContext);
            }
        }

        #endregion

        #region Property changed

#nullable disable
        /// <summary>
        /// Called when <see cref="Axes"/> property changed.
        /// </summary>
        /// <param name="bindable">The BindableObject.</param>
        /// <param name="oldValue">Old value.</param>
        /// <param name="newValue">New value.</param>
        private static void OnAxesPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is SfRadialGauge radialGauge)
            {
                if (oldValue is ObservableCollection<RadialAxis> oldAxisCollection)
                {
                    oldAxisCollection.CollectionChanged -= radialGauge.Axes_CollectionChanged;
                }

                if (newValue is ObservableCollection<RadialAxis> newAxisCollection)
                {
                    newAxisCollection.CollectionChanged += radialGauge.Axes_CollectionChanged;
                }
            }
        }
#nullable enable

        #endregion

        #region Private methods

        /// <summary>
        /// Method used to get touch action and point. 
        /// </summary>
        /// <param name="e">The TouchListenerEventArgs args.</param>
        void ITouchListener.OnTouch(TouchEventArgs e)
        {
            for (int i = Axes.Count - 1; i >= 0; i--)
            {
                RadialAxis axis = Axes[i];
                axis.OnTouchListener(e, ref isTouchHandled);

                if (Axes.Count > 1 && e.Action == TouchActions.Pressed && axis.Pointers.Any(pointer => pointer.IsPressed))
                {
                    break;
                }
            }
        }

        /// <summary>
        /// Measure the content.
        /// </summary>
        /// <param name="widthConstraint"></param>
        /// <param name="heightConstraint"></param>
        /// <returns></returns>
        Size IContentView.CrossPlatformMeasure(double widthConstraint, double heightConstraint)
        {
            return this.MeasureContent(widthConstraint, heightConstraint);
        }

        /// <summary>
        /// Arrange the content.
        /// </summary>
        /// <param name="bounds"></param>
        /// <returns></returns>
        Size IContentView.CrossPlatformArrange(Rectangle bounds)
        {
            this.ArrangeContent(bounds);
            return bounds.Size;
        }

        /// <summary>
        /// Axis collection added in the visual tree elements for hot reload case. 
        /// </summary>
        /// <returns></returns>
        IReadOnlyList<IVisualTreeElement> IVisualTreeElement.GetVisualChildren()
        {
            if (this.Axes != null)
            {
                return this.Axes.ToList().AsReadOnly();
            }
            return new List<IVisualTreeElement>();
        }

        /// <summary>
        /// Called when <see cref="Axes"/> collection got changed.
        /// </summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="e">The NotifyCollectionChangedEventArgs.</param>
        private void Axes_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                case NotifyCollectionChangedAction.Remove:
                case NotifyCollectionChangedAction.Replace:
                    if (e.OldItems != null && e.OldItems.Count > 0)
                    {
                        foreach (RadialAxis axis in e.OldItems)
                        {
                            axis.RangesGrid.Children.Clear();
                            axis.PointersGrid.Children.Clear();
                            axis.AnnotationsLayout.Children.Clear();
                            axis.ParentGrid.Children.Clear();
                            if (this.parentGrid.Children.Contains(axis))
                            {
                                this.parentGrid.Children.Remove(axis);
                            }
                        }
                    }

                    if (e.NewItems != null && e.NewItems.Count > 0)
                    {
                        foreach (RadialAxis axis in e.NewItems)
                        {
                            if (!this.parentGrid.Children.Contains(axis))
                            {
                                axis.RemoveTouchListener(axis);
                                this.parentGrid.Children.Add(axis);
                            }
                        }
                    }
                    break;

                case NotifyCollectionChangedAction.Reset:

                    foreach (RadialAxis axis in this.parentGrid.Children)
                    {
                        axis.RangesGrid.Children.Clear();
                        axis.PointersGrid.Children.Clear();
                        axis.AnnotationsLayout.Children.Clear();
                        axis.ParentGrid.Children.Clear();
                    }

                    this.parentGrid.Children.Clear();

                    break;
                default:
                    break;
            }
        }

        #endregion
    }
}
