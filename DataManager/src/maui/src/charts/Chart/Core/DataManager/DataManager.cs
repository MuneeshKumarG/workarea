using System.Collections;
using System.Collections.Generic;
using System.Linq;

#if WinUI
namespace Syncfusion.UI.Xaml.Charts
#else
namespace Syncfusion.Maui.Charts
#endif
{
    internal class DataManager
    {
        #region Poperties

        internal readonly Dictionary<IEnumerable, DataManagerPropertyAccessorCache> ItemsSourceDictionary;

        #endregion

        #region Constructor
        public DataManager()
        {
            ItemsSourceDictionary = new Dictionary<IEnumerable, DataManagerPropertyAccessorCache>();
        }
        #endregion

        #region Methods

        internal void PopulateData()
        {
            foreach (var item in ItemsSourceDictionary)
            {
                item.Value.GenerateList();
            }
        }

        internal void UpdateDataManager(IDataManagerDependent dependent, object oldValue, object newValue, bool updateData)
        {
            if (oldValue is IEnumerable oldSource)
            {
                if (ItemsSourceDictionary.ContainsKey(oldSource))
                {
                    var accessorCache = ItemsSourceDictionary[oldSource];

                    if (accessorCache.DependentObjectCollection.Count == 1)
                    {
                        DisposeItemsSource(oldSource);
                    }
                    else
                    {
                        accessorCache.DependentObjectCollection.Remove(dependent);
                        accessorCache.ClearSeriesDataPoints(dependent);
                        accessorCache.RemoveDependantAccessors(dependent);
                    }
                }
            }

            if (newValue is IEnumerable newSource)
            {
                if (dependent.XPath != null && dependent.YPaths.Count > 0)
                {
                    if (!ItemsSourceDictionary.TryGetValue(newSource, out var accessorCache))
                    {
                        accessorCache = new DataManagerPropertyAccessorCache(dependent);
                        ItemsSourceDictionary.Add(newSource, accessorCache);

                        if (updateData)
                            ItemsSourceDictionary[newSource].GenerateList();
                    }
                    else
                    {
                        if (updateData)
                            accessorCache.UpdateDependentCollection(dependent);
                        else
                            accessorCache.AddDependantObject(dependent);
                    }
                }
            }
        }

        internal void OnListenPropertyChanged(IDataManagerDependent dataManagerDependent, bool newValue)
        {
            if(dataManagerDependent.ItemsSource is IEnumerable source) 
            {
                if (ItemsSourceDictionary.ContainsKey(source))
                {
                    var accessorCache = ItemsSourceDictionary[source];
                    accessorCache.listenPropertyChange = newValue;
                    //TODO : check all series collection ListenProperty Change
                    accessorCache.HookOrUnhookPropertyChangedEvent();
                }
            }
        }

        #region BindingPathChanged
        
        internal void OnBindingPathChanged(IDataManagerDependent dataManagerDependent, string oldValue, object? newValue, bool populateData, bool isXPath)
        {
            if (populateData && oldValue != null && dataManagerDependent.ItemsSource is IEnumerable oldItemSource)
            {
                if (ItemsSourceDictionary.TryGetValue(oldItemSource, out var accessorCache))
                {
                    accessorCache.ClearSeriesDataPoints(dataManagerDependent);
                    IDataManagerDependent? pathDataDependent = null;
                    pathDataDependent = IsPathExist(accessorCache, dataManagerDependent, pathDataDependent, oldValue, isXPath);

                    if (pathDataDependent == null)
                    {
                        accessorCache.RemoveAccessor(oldValue);
                    }

                    if (!isXPath)
                        dataManagerDependent.YPaths.Remove(oldValue);
                }
            }

            if (newValue != null && dataManagerDependent.ItemsSource is IEnumerable itemSource)
            {
                if (ItemsSourceDictionary.TryGetValue(itemSource, out var accessorCache))
                {
                    accessorCache.GeneratePathAccessor(dataManagerDependent, newValue, false);
                }
                else
                {
                    accessorCache = new DataManagerPropertyAccessorCache(dataManagerDependent);
                    ItemsSourceDictionary.Add(itemSource, accessorCache);
                }

                if (populateData)
                {
                    accessorCache.ClearSeriesDataPoints(dataManagerDependent);
                    accessorCache.UpdateDependentCollection(dataManagerDependent);
                }
            }
        }

        internal IDataManagerDependent? IsPathExist(DataManagerPropertyAccessorCache accessorCache, IDataManagerDependent dataManagerDependent, IDataManagerDependent? pathDataDependent, string oldValue, bool isXPath)
        {
            foreach (var series in accessorCache.DependentObjectCollection)
            {
                if (series == dataManagerDependent)
                {
                    if (!isXPath && series.XPath.Equals(oldValue))
                    {
                        pathDataDependent = series;
                        break;
                    }
                    else if (isXPath && series.YPaths.Contains(oldValue))
                    {
                        pathDataDependent = series;
                        break;
                    }
                }
                else
                {
                    if (!isXPath && (series.XPath.Equals(oldValue) || series.YPaths.Contains(oldValue)))
                    {
                        pathDataDependent = series;
                        break;
                    }
                    else if (isXPath && (series.XPath.Equals(oldValue) || series.YPaths.Contains(oldValue)))
                    {
                        pathDataDependent = series;
                        break;
                    }
                }
            }
            return pathDataDependent;
        }
        #endregion

        #region Dispose

        internal void DisposeItemsSource(IEnumerable oldSource)
        {
            if (ItemsSourceDictionary.ContainsKey(oldSource))
            {
                var accessorCache = ItemsSourceDictionary[oldSource];

                accessorCache.DisposePropertyAccessor();

                ItemsSourceDictionary.Remove(oldSource);
            }
        }

        #endregion

        #endregion
    }
}
