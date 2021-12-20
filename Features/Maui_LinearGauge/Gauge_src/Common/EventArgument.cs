using System;

namespace Syncfusion.Maui.Gauges
{
    /// <summary>
    /// Provides event data for axis LabelCreated event. 
    /// </summary>
    public class LabelCreatedEventArgs
    {
#nullable disable
        /// <summary>
        /// Gets or sets the label text.
        /// </summary>
        /// <value>
        /// The axis label text.
        /// </value>
        public string Text { get; set; }

        /// <summary>
        /// To set the font style to gauge label text.
        /// </summary>
        public GaugeLabelStyle Style { get; set; }
#nullable enable

    }

    /// <summary>
    /// Provides event data for the ValueChanged event.
    /// </summary>
    public class ValueChangedEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the updated value of pointer.
        /// </summary>
        /// <value>
        /// The current value.
        /// </value>
        public double Value
        {
            get
            {
                return this.CurrentValue;
            }
        }

        /// <summary>
        /// Gets or sets the current drag value.
        /// </summary>
        internal double CurrentValue { get; set; }
    }

    /// <summary>
    /// Provides event data for the ValueChanging event. 
    /// </summary>
    public class ValueChangingEventArgs : EventArgs
    {
        /// <summary>
        /// Gets or sets a value that indicates whether to restrict the updating pointer value and cancel the dragging.
        /// </summary>
        /// <value>
        /// <b>true</b> if event is cancelled; otherwise, <b>false</b>.The default value is <b>false</b>.
        /// </value>
        public bool Cancel { get; set; }

        /// <summary>
        /// Gets the new pointer value which is updating the pointer value.
        /// </summary>
        /// <value>
        /// The new value.
        /// </value>
        public double NewValue
        {
            get
            {
                return this.CurrentValue;
            }
        }

        /// <summary>
        /// Gets the old value of pointer which is updated by pointer dragging.
        /// </summary>
        /// <value>
        /// The old value.
        /// </value>
        public double OldValue
        {
            get
            {
                return this.PreviousValue;
            }
        }

        /// <summary>
        /// Gets or sets a value indicates whether the current drag value.
        /// </summary>
        internal double CurrentValue { get; set; }

        /// <summary>
        /// Gets or sets a value indicates whether the previous drag value.
        /// </summary>
        internal double PreviousValue { get; set; }
    }
}
