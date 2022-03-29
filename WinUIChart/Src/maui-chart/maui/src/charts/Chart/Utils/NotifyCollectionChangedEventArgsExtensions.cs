using System;
using System.Collections.Specialized;

namespace Syncfusion.Maui.Charts
{
#nullable disable
    /// <summary>
    /// Extension for common NotifyCollectionChanges. 
    /// </summary>
    internal static class NotifyCollectionChangedEventArgsExtensions
    {
        public static void ApplyCollectionChanges(this NotifyCollectionChangedEventArgs self, Action<int, object> insertAction, Action<int, object> removeAction, Action resetAction)
        {
            switch (self.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    if (self.NewStartingIndex < 0)
                        goto case NotifyCollectionChangedAction.Reset;

                    for (var i = 0; i < self.NewItems.Count; i++)
                        insertAction(i + self.NewStartingIndex, self.NewItems[i]);

                    break;

                case NotifyCollectionChangedAction.Move:
                    if (self.NewStartingIndex < 0 || self.OldStartingIndex < 0)
                        goto case NotifyCollectionChangedAction.Reset;

                    for (var i = 0; i < self.OldItems.Count; i++)
                        removeAction(self.OldStartingIndex, self.OldItems[i]);

                    int insertIndex = self.NewStartingIndex;
                    if (self.OldStartingIndex < self.NewStartingIndex)
                        insertIndex -= self.OldItems.Count - 1;

                    for (var i = 0; i < self.OldItems.Count; i++)
                        insertAction(insertIndex + i, self.OldItems[i]);

                    break;

                case NotifyCollectionChangedAction.Remove:
                    if (self.OldStartingIndex < 0)
                        goto case NotifyCollectionChangedAction.Reset;

                    for (var i = 0; i < self.OldItems.Count; i++)
                        removeAction(self.OldStartingIndex, self.OldItems[i]);
                    break;

                case NotifyCollectionChangedAction.Replace:
                    if (self.OldStartingIndex < 0 || self.OldItems.Count != self.NewItems.Count)
                        goto case NotifyCollectionChangedAction.Reset;

                    for (var i = 0; i < self.OldItems.Count; i++)
                    {
                        removeAction(i + self.OldStartingIndex, self.OldItems[i]);
                        insertAction(i + self.OldStartingIndex, self.NewItems[i]);
                    }
                    break;

                case NotifyCollectionChangedAction.Reset:
                    {
                        resetAction();
                        break;
                    }
            }
        }
    }
}
