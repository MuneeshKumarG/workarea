using Microsoft.UI.Xaml.Data;
using System;
using System.Collections.Generic;
#if WinUI_Desktop
using System.ComponentModel;
#endif
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syncfusion.UI.Xaml.Core
{
    /// <summary>
    /// Base class for items that support property notification.
    /// </summary>
    /// <remarks>
    /// This class provides basic support for implementing the <see cref="INotifyPropertyChanged"/> interface and for
    /// marshalling execution to the UI thread.
    /// </remarks>
    public abstract class NotificationObject : INotifyPropertyChanged
    {
        /// <summary>
        /// Raised when a property on this object has a new value.
        /// </summary>        
        public event PropertyChangedEventHandler PropertyChanged;


        /// <summary>
        /// Raises this object's PropertyChanged event.
        /// </summary>
        /// <param name="propertyName">The property that has a new value.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1030:Use events where appropriate", Justification = "<Pending>")]
        protected virtual void RaisePropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }


        /// <summary>
        /// Raises this object's PropertyChanged event for each of the properties.
        /// </summary>
        /// <param name="propertyNames">The properties that have a new value.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1030:Use events where appropriate", Justification = "<Pending>")]
        protected void RaisePropertyChanged(params string[] propertyNames)
        {
            if (propertyNames == null) throw new ArgumentNullException(nameof(propertyNames));

            foreach (var name in propertyNames)
            {
                this.RaisePropertyChanged(name);
            }
        }
    }
}
