using System.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Reflection;
using System.ComponentModel;

#if WinUI
namespace Syncfusion.UI.Xaml.Charts
#else
namespace Syncfusion.Maui.Charts
#endif
{
    internal class DataManagerPropertyAccessorCache
    {
        #region Properties

        internal List<IDataManagerDependent> DependentObjectCollection = new List<IDataManagerDependent>();

        internal Dictionary<string, PropertyValueAccessor<string>> stringAccessor = new Dictionary<string, PropertyValueAccessor<string>>();
        internal Dictionary<string, PropertyValueAccessor<double>> doubleAccessor = new Dictionary<string, PropertyValueAccessor<double>>();
        internal Dictionary<string, PropertyValueAccessor<float>> floatAccessor = new Dictionary<string, PropertyValueAccessor<float>>();
        internal Dictionary<string, PropertyValueAccessor<long>> longAccessor = new Dictionary<string, PropertyValueAccessor<long>>();
        internal Dictionary<string, PropertyValueAccessor<DateTime>> dateTimeAccessor = new Dictionary<string, PropertyValueAccessor<DateTime>>();
        internal Dictionary<string, PropertyValueAccessor<int>> integerAccessor = new Dictionary<string, PropertyValueAccessor<int>>();
        internal Dictionary<string, PropertyValueAccessor<bool>> booleanAccessor = new Dictionary<string, PropertyValueAccessor<bool>>();
        internal Dictionary<string, PropertyValueAccessor<object>> objectAccessor = new Dictionary<string, PropertyValueAccessor<object>>();
        
        internal Dictionary<string, double[]> linearDataChecker = new Dictionary<string, double[]>();

        readonly object itemsSource;

        internal bool listenPropertyChange = false;

        bool isPropertyChangedHooked = false;

        #endregion

        #region Constructor

        public DataManagerPropertyAccessorCache(IDataManagerDependent origin)
        {
            itemsSource = origin.ItemsSource;
            AddDependantObject(origin);
            HookAndUnhookCollectionChangedEvent(null, itemsSource);
        }

        #endregion

        #region Methods

        internal void AddDependantObject(IDataManagerDependent dataManagerDependent)
        {
            if (!DependentObjectCollection.Contains(dataManagerDependent))
            {
                listenPropertyChange = listenPropertyChange? listenPropertyChange: dataManagerDependent.ListenPropertyChange;
                DependentObjectCollection.Add(dataManagerDependent);
                GenerateAccessorValues(dataManagerDependent);
            }
        }

        private void InitializeDependent(IDataManagerDependent dependent)
        {
            dependent.ActualData = new List<object>();
            dependent.XDateTimeValues = new List<DateTime>();
            dependent.XDoubleValues = new List<double>();
            dependent.XIndexedList = new List<double>();
            dependent.XStringValues = new List<string>();
            dependent.YDoubleValues = new List<List<double>>();
        }

        internal void GenerateAccessorValues(IDataManagerDependent dataManagerDependent)
        {
            var source = dataManagerDependent.ItemsSource as IEnumerable;
           
            var enumerator = source?.GetEnumerator();
            if (enumerator != null && enumerator.MoveNext())
            {
                var currObj = enumerator.Current;
                var yPaths = dataManagerDependent.YPaths;
                var xPath = dataManagerDependent.XPath;
                if (xPath == null || yPaths == null)
                    return;

                var info = GetPropertyInfo(currObj, xPath);
                GenerateAccessor(xPath, info);
                if (!linearDataChecker.ContainsKey(xPath))
                    linearDataChecker.Add(xPath, new double[] { 1, double.NaN });

                for (int i = 0; i < yPaths.Count(); i++)
                {
                    info = GetPropertyInfo(currObj, yPaths[i]);
                    GenerateAccessor(yPaths[i], info);
                }
            }
        }

        internal static PropertyInfo? GetPropertyInfo(object obj, string path)
        {
            if (obj != null)
                return obj.GetType()?.GetRuntimeProperty(path);
            else
                return null;
        }

        internal void GenerateList()
        {
            if (itemsSource is IEnumerable source)
            {
                var enumerator = source.GetEnumerator();
                if (enumerator != null && enumerator.MoveNext())
                {
                    var stringValues = new List<List<string>>();
                    var doubleValues = new List<List<double>>();
                    var floatValues = new List<List<double>>();
                    var longValues = new List<List<double>>();
                    var objectDoubleValues = new List<List<double>>();
                    var dateTimeValues = new List<List<DateTime>>();
                    var dateTimeDoubleValues = new List<List<double>>();
                    var integerValues = new List<List<double>>();
                    var booleanValues = new List<List<double>>();
                    var objectValues = new List<List<object>>();
                    var groupedValues = new List<List<List<double>>>();

                    var stringKeys = stringAccessor.Keys.ToList();
                    var doubleKeys = doubleAccessor.Keys.ToList();
                    var floatKeys = floatAccessor.Keys.ToList();
                    var longKeys = longAccessor.Keys.ToList();
                    var integerKeys = integerAccessor.Keys.ToList();
                    var dateTimeKeys = dateTimeAccessor.Keys.ToList();
                    var booleanKeys = booleanAccessor.Keys.ToList();
                    var objectKeys = objectAccessor.Keys.ToList();
                    var doubleAccessorCount = doubleAccessor.Count;
                    var longAccessorCount = longAccessor.Count;
                    var stringAccessorCount = stringAccessor.Count;
                    var dateTimeAccessorCount = dateTimeAccessor.Count;
                    var integerAccessorCount = integerAccessor.Count;
                    var booleanAccessorCount = booleanAccessor.Count;
                    var objectAccessorCount = objectAccessor.Count;
                    var floatAccessorCount = floatAccessor.Count;
                    var actualData = new List<object>();
                    var indexedList = new List<double>();
                    int index = 0;

                    for (int i = 0; i < stringAccessorCount; i++)
                    {
                        stringValues.Add(new List<string>());
                    }

                    for (int i = 0; i < doubleAccessorCount; i++)
                    {
                        doubleValues.Add(new List<double>());
                    }

                    for (int i = 0; i < dateTimeAccessorCount; i++)
                    {
                        dateTimeValues.Add(new List<DateTime>());
                        dateTimeDoubleValues.Add(new List<double>());
                    }

                    for (int i = 0; i < integerAccessorCount; i++)
                    {
                        integerValues.Add(new List<double>());
                    }

                    for (int i = 0; i < floatAccessorCount; i++)
                    {
                        floatValues.Add(new List<double>());
                    }

                    for (int i = 0; i < longAccessorCount; i++)
                    {
                        longValues.Add(new List<double>());
                    }

                    for (int i = 0; i < booleanAccessorCount; i++)
                    {
                        booleanValues.Add(new List<double>());
                    }

                    for (int i = 0; i < objectAccessorCount; i++)
                    {
                        objectValues.Add(new List<object>());
                        groupedValues.Add(new List<List<double>>());
                        objectDoubleValues.Add(new List<double>());
                    }

                    do
                    {
                        var currentObj = enumerator.Current;

                        if (listenPropertyChange && !isPropertyChangedHooked)
                            HookPropertyChangedEvent(currentObj);

                        actualData.Add(currentObj);
                        indexedList.Add(index);
                        index++;

                        for (int i = 0; i < stringAccessorCount; i++)
                        {
                            var accessor = stringAccessor[stringKeys[i]];
                            stringValues[i].Add(accessor.GetValue(currentObj));
                        }

                        for (int i = 0; i < doubleAccessorCount; i++)
                        {
                            string key = doubleKeys[i];
                            var accessor = doubleAccessor[key];
                            double xValue = accessor.GetValue(currentObj);
                            doubleValues[i].Add(xValue);

                            if (linearDataChecker.ContainsKey(key))
                            {
                                var linearValues = linearDataChecker[key];
                                if (linearValues[0] == 1 && xValue <= linearValues[1])
                                {
                                    linearValues[0] = 0;
                                }
                                linearValues[1] = xValue;
                            }
                        }

                        for (int i = 0; i < dateTimeAccessorCount; i++)
                        {
                            var accessor = dateTimeAccessor[dateTimeKeys[i]];
                            DateTime dateTime = accessor.GetValue(currentObj);
                            dateTimeValues[i].Add(dateTime);
                            dateTimeDoubleValues[i].Add(dateTime.ToOADate());
                        }

                        for (int i = 0; i < integerAccessorCount; i++)
                        {
                            string key = integerKeys[i];
                            var accessor = integerAccessor[key];

                            int xValue = accessor.GetValue(currentObj);
                            integerValues[i].Add(xValue);

                            if (linearDataChecker.ContainsKey(key))
                            {
                                var linearValues = linearDataChecker[key];
                                if (linearValues[0] == 1 && xValue <= linearValues[1])
                                {
                                    linearValues[0] = 0;
                                }
                                linearValues[1] = xValue;
                            }
                        }

                        for (int i = 0; i < floatAccessorCount; i++)
                        {
                            string key = floatKeys[i];
                            var accessor = floatAccessor[key];
                            double xValue = accessor.GetValue(currentObj);
                            floatValues[i].Add(xValue);

                            if (linearDataChecker.ContainsKey(key))
                            {
                                var linearValues = linearDataChecker[key];
                                if (linearValues[0] == 1 && xValue <= linearValues[1])
                                {
                                    linearValues[0] = 0;
                                }
                                linearValues[1] = xValue;
                            }
                        }

                        for (int i = 0; i < longAccessorCount; i++)
                        {
                            string key = longKeys[i];
                            var accessor = longAccessor[key];
                            double xValue = accessor.GetValue(currentObj);
                            longValues[i].Add(xValue);

                            if (linearDataChecker.ContainsKey(key))
                            {
                                var linearValues = linearDataChecker[key];
                                if (linearValues[0] == 1 && xValue <= linearValues[1])
                                {
                                    linearValues[0] = 0;
                                }
                                linearValues[1] = xValue;
                            }
                        }

                        for (int i = 0; i < objectAccessorCount; i++)
                        {
                            string key = objectKeys[i];
                            var accessor = objectAccessor[key];

                            object value = accessor.GetObjectValue(currentObj);
                            objectValues[i].Add(value);

                            if(value is IEnumerable enumerable)
                            {
                                if (groupedValues[i] == null)
                                {
                                    groupedValues[i] = new List<List<double>>();
                                }

                                List<double> doubleList = new List<double>();

                                foreach (var item in enumerable)
                                {
                                    if (double.TryParse(item.ToString(), out double result))
                                    {
                                        doubleList.Add(result);
                                    }
                                }

                                groupedValues[i].Add(doubleList);
                            }
                            else
                            {
                                double objectDoubleValue = Convert.ToDouble(value);
                                objectDoubleValues[i].Add(objectDoubleValue);

                                if (linearDataChecker.ContainsKey(key))
                                {
                                    var linearValues = linearDataChecker[key];
                                    if (linearValues[0] == 1 && objectDoubleValue <= linearValues[1])
                                    {
                                        linearValues[0] = 0;
                                    }
                                    linearValues[1] = objectDoubleValue;
                                }
                            }
                        }

                        for (int i = 0; i < booleanAccessorCount; i++)
                        {
                            var accessor = booleanAccessor[booleanKeys[i]];
                            booleanValues[i].Add(accessor.GetValue(currentObj) ? 1 : 0);
                        }

                    } while (enumerator.MoveNext());

                    if (listenPropertyChange)
                        isPropertyChangedHooked = true;

                    for (int k = 0; k < DependentObjectCollection.Count; k++)
                    {
                        var origin = DependentObjectCollection[k];
                        var xPath = origin.XPath;
                        var yPaths = origin.YPaths;
                        if (xPath == null || yPaths == null)
                            return;
                        for (int i = 0; i < stringKeys.Count; i++)
                        {
                            if (stringKeys[i] == xPath)
                            {
                                origin.XStringValues = stringValues[i].ToList();
                                origin.XIndexedList = indexedList.ToList();
                                origin.XValueType = ValueType.String;
                                break;
                            }
                        }

                        for (int i = 0; i < doubleKeys.Count; i++)
                        {
                            if (doubleKeys[i] == xPath)
                            {
                                origin.XDoubleValues = doubleValues[i].ToList();
                                origin.XIndexedList = indexedList.ToList();
                                origin.XValueType = ValueType.Double;
                                break;
                            }
                        }

                        for (int i = 0; i < dateTimeKeys.Count; i++)
                        {
                            if (dateTimeKeys[i] == xPath)
                            {
                                origin.XDoubleValues = dateTimeDoubleValues[i].ToList();
                                origin.XDateTimeValues = dateTimeValues[i].ToList();
                                origin.XValueType = ValueType.DateTime;
                                origin.XIndexedList = indexedList.ToList();
                                break;
                            }
                        }

                        for (int i = 0; i < integerValues.Count; i++)
                        {
                            if (integerKeys[i] == xPath)
                            {
                                origin.XDoubleValues = integerValues[i].ToList();
                                origin.XIndexedList = indexedList.ToList();
                                origin.XValueType = ValueType.Int;
                                break;
                            }
                        }

                        for (int i = 0; i < floatKeys.Count; i++)
                        {
                            if (floatKeys[i] == xPath)
                            {
                                origin.XDoubleValues = floatValues[i].ToList();
                                origin.XIndexedList = indexedList.ToList();
                                origin.XValueType = ValueType.Float;
                                break;
                            }
                        }

                        for (int i = 0; i < longKeys.Count; i++)
                        {
                            if (longKeys[i] == xPath)
                            {
                                origin.XDoubleValues = longValues[i].ToList();
                                origin.XIndexedList = indexedList.ToList();
                                origin.XValueType = ValueType.Long;
                                break;
                            }
                        }
                        for (int i = 0; i < objectKeys.Count; i++)
                        {
                            if (objectKeys[i] == xPath)
                            {
                                origin.XDoubleValues = objectDoubleValues[i].ToList();
                                origin.XIndexedList = indexedList.ToList();
                                origin.XValueType = ValueType.Double;
                                break;
                            }
                        }

                        var yDoubleList = new List<List<double>>();
                        var yCount = yPaths.Count();

                        for (int i = 0; i < yCount; i++)
                        {
                            var yPath = yPaths[i];
                            for (int j = 0; j < doubleKeys.Count; j++)
                            {
                                if (doubleKeys[j] == yPath)
                                {
                                    yDoubleList.Add(doubleValues[j].ToList());
                                    break;
                                }
                            }

                            for (int j = 0; j < integerKeys.Count; j++)
                            {
                                if (integerKeys[j] == yPath)
                                {
                                    yDoubleList.Add(integerValues[j].ToList());
                                    break;
                                }
                            }

                            for (int j = 0; j < floatKeys.Count; j++)
                            {
                                if (floatKeys[j] == yPath)
                                {
                                    yDoubleList.Add(floatValues[j].ToList());
                                    break;
                                }
                            }

                            for (int j = 0; j < longKeys.Count; j++)
                            {
                                if (longKeys[j] == yPath)
                                {
                                    yDoubleList.Add(longValues[j].ToList());
                                    break;
                                }
                            }

                            for (int j = 0; j < booleanKeys.Count; j++)
                            {
                                if (booleanKeys[j] == yPath)
                                {
                                    yDoubleList.Add(booleanValues[j].ToList());
                                    break;
                                }
                            }

                            if (origin.IsGroupedYPath)
                            {
                                for (int j = 0; j < objectKeys.Count; j++)
                                {
                                    if (objectKeys[j] == yPath)
                                    {
                                        yDoubleList = groupedValues[j];
                                        break;
                                    }
                                }
                            }
                            else
                            {
                                for (int j = 0; j < objectKeys.Count; j++)
                                {
                                    if (objectKeys[j] == yPath)
                                    {
                                        yDoubleList.Add(objectDoubleValues[j].ToList());
                                        break;
                                    }
                                }
                            }
                        }

                        origin.YDoubleValues = yDoubleList;
                        origin.ActualData = actualData.ToList();
                        origin.PointsCount = actualData.Count;
                        if (linearDataChecker.ContainsKey(xPath))
                        {
                            var linearValues = linearDataChecker[xPath];
                            origin.IsLinearData = linearValues[0] == 1;
                        }
                    }
                }
                else
                {
                    foreach (var dependent in DependentObjectCollection)
                    {
                        InitializeDependent(dependent);
                    }
                }
            }
        }

        internal void GenerateDataDependentList(IDataManagerDependent dependent)
        {
            if (itemsSource is IEnumerable source)
            {
                var enumerator = source.GetEnumerator();
                if (enumerator != null && enumerator.MoveNext())
                {
                    var doubleValues = new List<List<double>>();
                    var floatValues = new List<List<double>>();
                    var longValues = new List<List<double>>();
                    var dateTimeValues = new List<List<DateTime>>();
                    var dateTimeDoubleValues = new List<List<double>>();
                    var objectDoubleValues = new List<List<double>>();
                    var integerValues = new List<List<double>>();
                    var booleanValues = new List<List<double>>();
                    var objectValues = new List<List<object>>();
                    var groupedValues = new List<List<List<double>>>();
                    var xIndexedValues = new List<List<double>>();

                    var stringKeys = stringAccessor.Keys.ToList();
                    var doubleKeys = doubleAccessor.Keys.ToList();
                    var integerKeys = integerAccessor.Keys.ToList();
                    var dateTimeKeys = dateTimeAccessor.Keys.ToList();
                    var booleanKeys = booleanAccessor.Keys.ToList();
                    var objectKeys = objectAccessor.Keys.ToList();
                    var floatKeys = floatAccessor.Keys.ToList();
                    var longKeys = longAccessor.Keys.ToList();

                    var doubleAccessorCount = doubleAccessor.Count;
                    var stringAccessorCount = stringAccessor.Count;
                    var dateTimeAccessorCount = dateTimeAccessor.Count;
                    var integerAccessorCount = integerAccessor.Count;
                    var booleanAccessorCount = booleanAccessor.Count;
                    var objectAccessorCount = objectAccessor.Count;
                    var floatAccessorCount = floatAccessor.Count;
                    var longAccessorCount = longAccessor.Count;
                    var actualData = new List<object>();
                    var indexedList = new List<double>();
                    int index = 0;

                    if (stringKeys.Contains(dependent.XPath))
                    {
                        if (dependent.XStringValues == null)
                            dependent.XStringValues = new List<string>();
                        else
                            dependent.XStringValues.Clear();
                    }

                    for (int i = 0; i < doubleAccessorCount; i++)
                    {
                        doubleValues.Add(new List<double>());
                    }

                    for (int i = 0; i < dateTimeAccessorCount; i++)
                    {
                        dateTimeValues.Add(new List<DateTime>());
                        dateTimeDoubleValues.Add(new List<double>());
                    }

                    for (int i = 0; i < integerAccessorCount; i++)
                    {
                        integerValues.Add(new List<double>());
                    }

                    for (int i = 0; i < floatAccessorCount; i++)
                    {
                        floatValues.Add(new List<double>());
                    }

                    for (int i = 0; i < longAccessorCount; i++)
                    {
                        longValues.Add(new List<double>());
                    }

                    for (int i = 0; i < booleanAccessorCount; i++)
                    {
                        booleanValues.Add(new List<double>());
                    }

                    for (int i = 0; i < objectAccessorCount; i++)
                    {
                        objectValues.Add(new List<object>());
                        groupedValues.Add(new List<List<double>>());
                        objectDoubleValues.Add(new List<double>());
                    }
                    
                    do
                    {
                        var currentObj = enumerator.Current;

                        if (listenPropertyChange && !isPropertyChangedHooked)
                            HookPropertyChangedEvent(currentObj);
                        
                        actualData.Add(currentObj);
                        indexedList.Add(index);
                        index++;

                        for (int i = 0; i < stringAccessorCount; i++)
                        {
                            if (dependent.XPath == stringKeys[i])
                            {
                                var accessor = stringAccessor[stringKeys[i]];
                                dependent.XStringValues.Add(accessor.GetValue(currentObj));
                            }
                        }

                        for (int i = 0; i < doubleAccessorCount; i++)
                        {
                            string key = doubleKeys[i];
                            if (dependent.XPath == key || dependent.YPaths.Contains(key))
                            {
                                var accessor = doubleAccessor[key];
                                double xValue = accessor.GetValue(currentObj);
                                doubleValues[i].Add(xValue);
                                if (linearDataChecker.ContainsKey(key))
                                {
                                    var linearValues = linearDataChecker[key];
                                    if (linearValues[0] == 1 && xValue <= linearValues[1])
                                    {
                                        linearValues[0] = 0;
                                    }
                                    linearValues[1] = xValue;
                                }
                            }
                        }

                        for (int i = 0; i < dateTimeAccessorCount; i++)
                        {
                            if (dependent.XPath.Equals(dateTimeKeys[i]))
                            {
                                var accessor = dateTimeAccessor[dateTimeKeys[i]];
                                DateTime dateTime = accessor.GetValue(currentObj);
                                dateTimeValues[i].Add(dateTime);
                                dateTimeDoubleValues[i].Add(dateTime.ToOADate());
                            }
                        }

                        for (int i = 0; i < integerAccessorCount; i++)
                        {
                            string key = integerKeys[i];
                            if (dependent.XPath == key || dependent.YPaths.Contains(key))
                            {
                                var accessor = integerAccessor[key];
                                int xValue = accessor.GetValue(currentObj);
                                integerValues[i].Add(xValue);

                                if (linearDataChecker.ContainsKey(key))
                                {
                                    var linearValues = linearDataChecker[key];
                                    if (linearValues[0] == 1 && xValue <= linearValues[1])
                                    {
                                        linearValues[0] = 0;
                                    }
                                    linearValues[1] = xValue;
                                }
                            }
                        }

                        for (int i = 0; i < floatAccessorCount; i++)
                        {
                            string key = floatKeys[i];
                            if (dependent.XPath == key || dependent.YPaths.Contains(key))
                            {
                                var accessor = floatAccessor[key];
                                double xValue = accessor.GetValue(currentObj);
                                floatValues[i].Add(xValue);

                                if (linearDataChecker.ContainsKey(key))
                                {
                                    var linearValues = linearDataChecker[key];
                                    if (linearValues[0] == 1 && xValue <= linearValues[1])
                                    {
                                        linearValues[0] = 0;
                                    }
                                    linearValues[1] = xValue;
                                }
                            }
                        }

                        for (int i = 0; i < longAccessorCount; i++)
                        {
                            string key = longKeys[i];
                            if (dependent.XPath == key || dependent.YPaths.Contains(key))
                            {
                                var accessor = longAccessor[key];
                                double xValue = accessor.GetValue(currentObj);
                                longValues[i].Add(xValue);
                                if (linearDataChecker.ContainsKey(key))
                                {
                                    var linearValues = linearDataChecker[key];
                                    if (linearValues[0] == 1 && xValue <= linearValues[1])
                                    {
                                        linearValues[0] = 0;
                                    }
                                    linearValues[1] = xValue;
                                }
                            }
                        }
                        
                        for (int i = 0; i < booleanAccessorCount; i++)
                        {
                            if (dependent.XPath.Equals(booleanKeys[i]) || dependent.YPaths.Contains(booleanKeys[i]))
                            {
                                var accessor = booleanAccessor[booleanKeys[i]];
                                booleanValues[i].Add(accessor.GetValue(currentObj) ? 1 : 0);
                            }
                        }

                        for (int i = 0; i < objectAccessorCount; i++)
                        {
                            string key = objectKeys[i];
                            if (dependent.XPath == key || dependent.YPaths.Contains(key))
                            {
                                var accessor = objectAccessor[key];
                                object xValue = accessor.GetObjectValue(currentObj);


                                if (xValue is IEnumerable enumerable)
                                {
                                    if (groupedValues[i] == null)
                                    {
                                        groupedValues[i] = new List<List<double>>();
                                    }

                                    List<double> doubleList = new List<double>();

                                    foreach (var item in enumerable)
                                    {
                                        if (double.TryParse(item.ToString(), out double result))
                                        {
                                            doubleList.Add(result);
                                        }
                                    }

                                    groupedValues[i].Add(doubleList);
                                }
                                else
                                {
                                    double objectDoubleValue = Convert.ToDouble(xValue);
                                    objectDoubleValues[i].Add(objectDoubleValue);

                                    if (linearDataChecker.ContainsKey(key))
                                    {
                                        var linearValues = linearDataChecker[key];
                                        if (linearValues[0] == 1 && objectDoubleValue <= linearValues[1])
                                        {
                                            linearValues[0] = 0;
                                        }
                                        linearValues[1] = objectDoubleValue;
                                    }
                                }
                            }
                        }

                    } while (enumerator.MoveNext());

                    if (listenPropertyChange)
                        isPropertyChangedHooked = true;

                    var xPath = dependent.XPath;
                    var yPaths = dependent.YPaths;
                    
                    for (int i = 0; i < stringKeys.Count; i++)
                    {
                        if (stringKeys[i] == xPath)
                        {
                            dependent.XIndexedList = indexedList.ToList();
                            dependent.XValueType = ValueType.String;
                            break;
                        }
                    }

                    for (int i = 0; i < doubleKeys.Count; i++)
                    {
                        if (doubleKeys[i] == xPath)
                        {
                            dependent.XDoubleValues = doubleValues[i].ToList();
                            dependent.XIndexedList = indexedList.ToList();
                            dependent.XValueType = ValueType.Double;
                            break;
                        }
                    }

                    for (int i = 0; i < dateTimeKeys.Count; i++)
                    {
                        if (dateTimeKeys[i] == xPath)
                        {
                            dependent.XDoubleValues = dateTimeDoubleValues[i].ToList();
                            dependent.XDateTimeValues = dateTimeValues[i].ToList();
                            dependent.XIndexedList = indexedList.ToList();
                            dependent.XValueType = ValueType.DateTime;
                            break;
                        }
                    }

                    for (int i = 0; i < integerValues.Count; i++)
                    {
                        if (integerKeys[i] == xPath)
                        {
                            dependent.XDoubleValues = integerValues[i].ToList();
                            dependent.XIndexedList = indexedList.ToList();
                            dependent.XValueType = ValueType.Int;
                            break;
                        }
                    }

                    for (int i = 0; i < floatKeys.Count; i++)
                    {
                        if (floatKeys[i] == xPath)
                        {
                            dependent.XDoubleValues = floatValues[i].ToList();
                            dependent.XIndexedList = indexedList.ToList();
                            dependent.XValueType = ValueType.Float;
                            break;
                        }
                    }

                    for (int i = 0; i < longKeys.Count; i++)
                    {
                        if (longKeys[i] == xPath)
                        {
                            dependent.XDoubleValues = longValues[i].ToList();
                            dependent.XIndexedList = indexedList.ToList();
                            dependent.XValueType = ValueType.Long;
                            break;
                        }
                    }
                    
                    for (int i = 0; i < objectKeys.Count; i++)
                    {
                        if (objectKeys[i] == xPath)
                        {
                            dependent.XDoubleValues = objectDoubleValues[i].ToList();
                            dependent.XIndexedList = indexedList.ToList();
                            dependent.XValueType = ValueType.Double;
                            break;
                        }
                    }
                    
                    var yDoubleList = new List<List<double>>();
                    var yCount = yPaths.Count();

                    for (int i = 0; i < yCount; i++)
                    {
                        var yPath = yPaths[i];
                        for (int j = 0; j < doubleKeys.Count; j++)
                        {
                            if (doubleKeys[j] == yPath)
                            {
                                yDoubleList.Add(doubleValues[j].ToList());
                                break;
                            }
                        }

                        for (int j = 0; j < integerKeys.Count; j++)
                        {
                            if (integerKeys[j] == yPath)
                            {
                                yDoubleList.Add(integerValues[j].ToList());
                                break;
                            }
                        }

                        for (int j = 0; j < floatKeys.Count; j++)
                        {
                            if (floatKeys[j] == yPath)
                            {
                                yDoubleList.Add(floatValues[j].ToList());
                                break;
                            }
                        }

                        for (int j = 0; j < longKeys.Count; j++)
                        {
                            if (longKeys[j] == yPath)
                            {
                                yDoubleList.Add(longValues[j].ToList());
                                break;
                            }
                        }

                        for (int j = 0; j < booleanValues.Count; j++)
                        {
                            if (booleanKeys[j] == yPath)
                            {
                                yDoubleList.Add(booleanValues[j].ToList());
                                break;
                            }
                        }

                        if (dependent.IsGroupedYPath)
                        {
                            for (int j = 0; j < groupedValues.Count; j++)
                            {
                                if (objectKeys[j] == yPath)
                                {
                                    yDoubleList = groupedValues[j];
                                    break;
                                }
                            }
                        }
                        else
                        {
                            for (int j = 0; j < objectValues.Count; j++)
                            {
                                if (objectKeys[j] == yPath)
                                {
                                    yDoubleList.Add(objectDoubleValues[j].ToList());
                                    break;
                                }
                            }
                        }
                    }

                    dependent.YDoubleValues = yDoubleList;
                    dependent.ActualData = actualData.ToList();
                    dependent.PointsCount = actualData.Count;
                    if (linearDataChecker.ContainsKey(xPath))
                    {
                        var linearValues = linearDataChecker[xPath];
                        dependent.IsLinearData = linearValues[0] == 1;
                    }
                }
                else
                {
                    InitializeDependent(dependent);
                }
            }
        }

        internal void GenerateAccessor(string path, PropertyInfo? info)
        {
            if (info == null)
                return;
            switch (Type.GetTypeCode(info.PropertyType))
            {
                case TypeCode.Double:
                    if (!doubleAccessor.ContainsKey(path))
                        doubleAccessor.Add(path, new PropertyValueAccessor<double>(info));
                    break;
                case TypeCode.String:
                    if (!stringAccessor.ContainsKey(path))
                        stringAccessor.Add(path, new PropertyValueAccessor<string>(info));
                    break;
                case TypeCode.DateTime:
                    if (!dateTimeAccessor.ContainsKey(path))
                        dateTimeAccessor.Add(path, new PropertyValueAccessor<DateTime>(info));
                    break;
                case TypeCode.Int32:
                case TypeCode.Int16:
                    if (!integerAccessor.ContainsKey(path))
                        integerAccessor.Add(path, new PropertyValueAccessor<int>(info));
                    break;
                case TypeCode.Boolean:
                    if (!booleanAccessor.ContainsKey(path))
                        booleanAccessor.Add(path, new PropertyValueAccessor<bool>(info));
                    break;
                case TypeCode.Single:
                    if (!floatAccessor.ContainsKey(path))
                        floatAccessor.Add(path, new PropertyValueAccessor<float>(info));
                    break;
                case TypeCode.Int64:
                    if (!longAccessor.ContainsKey(path))
                        longAccessor.Add(path, new PropertyValueAccessor<long>(info));
                    break;
                default:
                    if (!objectAccessor.ContainsKey(path))
                        objectAccessor.Add(path, new PropertyValueAccessor<object>(info));
                    break;
            }
        }

        internal void GeneratePathAccessor(IDataManagerDependent dataManagerDependent, object path, bool isXPath)
        {
            var source = dataManagerDependent.ItemsSource as IEnumerable;

            var enumerator = source?.GetEnumerator();
            if (enumerator != null && enumerator.MoveNext())
            {
                var currObj = enumerator.Current;
                var newPath = path.ToString();
                var info = GetPropertyInfo(currObj, newPath);
                GenerateAccessor(newPath, info);
                if (isXPath && !linearDataChecker.ContainsKey(newPath))
                    linearDataChecker.Add(newPath, new double[] { 1, double.NaN });
            }
        }
        
        #region Add and Remove data
        internal virtual void HookAndUnhookCollectionChangedEvent(object? oldValue, object? newValue)
        {
            if (newValue != null)
            {
                var newCollectionValue = newValue as INotifyCollectionChanged;
                if (newCollectionValue != null)
                {
                    newCollectionValue.CollectionChanged += OnDataSource_CollectionChanged;
                }
            }

            if (oldValue != null)
            {
                var oldCollectionValue = oldValue as INotifyCollectionChanged;
                if (oldCollectionValue != null)
                {
                    oldCollectionValue.CollectionChanged -= OnDataSource_CollectionChanged;
                }
            }
        }

        void OnDataSource_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            ApplyCollectionChanges(e, (index, obj) => AddDataPoint(index, obj, false), (index, obj) => RemoveData(index), ResetDataPoint);
        }

        private void UpdateLinearData(IDataManagerDependent origin, double xValue)
        {
            if (origin.IsLinearData && origin.XDoubleValues.Count > 0)
            {
                double previousXValue = origin.XDoubleValues.Last();
                origin.IsLinearData = xValue >= previousXValue;
            }
        }
        internal void AddXDataPoint(IDataManagerDependent origin, string xPath, object data, int index, bool replace)
        {
            if (doubleAccessor.ContainsKey(xPath))
            {
                var acccessor = doubleAccessor[xPath];
                double xValue = acccessor.GetValue(data);
                origin.XValueType = ValueType.Double;
                if (!replace)
                {
                    UpdateLinearData(origin, xValue);
                    origin.XDoubleValues.Insert(index, xValue);
                }
                else if (origin.XDoubleValues[index] != xValue)
                {
                    origin.XDoubleValues[index] = xValue;
                    UpdateLinearData(origin, xValue);
                }
            }
            else if (objectAccessor.ContainsKey(xPath))
            {
                origin.XValueType = ValueType.Double;
                var acccessor = objectAccessor[xPath];
                object xValue = acccessor.GetObjectValue(data);
                double objectXDoubleValue = Convert.ToDouble(xValue);

                if (!replace)
                {
                    UpdateLinearData(origin, objectXDoubleValue);
                    origin.XDoubleValues.Insert(index, objectXDoubleValue);
                }
                else if (origin.XDoubleValues[index] != objectXDoubleValue)
                {
                    origin.XDoubleValues[index] = objectXDoubleValue;
                    UpdateLinearData(origin, objectXDoubleValue);
                }
            }
            else if(stringAccessor.ContainsKey(xPath))
            {
                origin.XValueType = ValueType.String;
                var acccessor = stringAccessor[xPath];
                string xValue = acccessor.GetValue(data);

                if (!replace)
                {
                    origin.XStringValues.Insert(index, xValue);
                }
                else
                {
                    origin.XStringValues[index] = xValue;
                }
            }
            else if(dateTimeAccessor.ContainsKey(xPath))
            {
                origin.XValueType = ValueType.DateTime;
                var acccessor = dateTimeAccessor[xPath];
                var xDateTimeValue = acccessor.GetValue(data);
                double xDateTimeDoubleValue = xDateTimeValue.ToOADate();

                if (!replace)
                {
                    UpdateLinearData(origin, xDateTimeDoubleValue);
                    origin.XDateTimeValues.Insert(index, xDateTimeValue);
                    origin.XDoubleValues.Insert(index, xDateTimeDoubleValue);
                }
                else if (origin.XDateTimeValues[index] != xDateTimeValue)
                {
                    origin.XDateTimeValues[index] = xDateTimeValue;
                    origin.XDoubleValues[index] = xDateTimeDoubleValue;
                    UpdateLinearData(origin, xDateTimeDoubleValue);
                }
            }
            else if(integerAccessor.ContainsKey(xPath))
            {
                origin.XValueType = ValueType.Int;
                var acccessor = integerAccessor[xPath];
                double xValue = acccessor.GetValue(data);

                if (!replace)
                {
                    UpdateLinearData(origin, xValue);
                    origin.XDoubleValues.Insert(index, xValue);
                }
                else if (origin.XDoubleValues[index] != xValue)
                {
                    origin.XDoubleValues[index] = xValue;
                    UpdateLinearData(origin, xValue);
                }
            }
            else if(floatAccessor.ContainsKey(xPath))
            {
                origin.XValueType = ValueType.Float;
                var acccessor = floatAccessor[xPath];
                double xValue = acccessor.GetValue(data);

                if (!replace)
                {
                    UpdateLinearData(origin, xValue);
                    origin.XDoubleValues.Insert(index, xValue);
                }
                else if (origin.XDoubleValues[index] != xValue)
                {
                    origin.XDoubleValues[index] = xValue;
                    UpdateLinearData(origin, xValue);
                }
            }
            else if(longAccessor.ContainsKey(xPath))
            {
                origin.XValueType = ValueType.Long;
                var acccessor = longAccessor[xPath];
                double xValue = acccessor.GetValue(data);

                if (!replace)
                {
                    UpdateLinearData(origin, xValue);
                    origin.XDoubleValues.Insert(index, xValue);
                }
                else if (origin.XDoubleValues[index] != xValue)
                {
                    origin.XDoubleValues[index] = xValue;
                    UpdateLinearData(origin, xValue);
                }
            }
        }

        internal void AddYDataPoint(IDataManagerDependent origin, List<string> yPaths, object data, int index, bool replace)
        {
            var yCount = yPaths.Count();
            for (int k = 0; k < yCount; k++)
            {
                var yPath = yPaths[k];

                if (origin.YDoubleValues.Count <= k)
                    origin.YDoubleValues.Add(new List<double>());

                if (doubleAccessor.ContainsKey(yPath))
                {
                    var acccessor = doubleAccessor[yPath];
                    double yValue = acccessor.GetValue(data);
                    UpdateYDoubleValues(origin.YDoubleValues[k], index, yValue, replace);
                }
                else if (integerAccessor.ContainsKey(yPath))
                {
                    var acccessor = integerAccessor[yPath];
                    double yValue = acccessor.GetValue(data);
                    UpdateYDoubleValues(origin.YDoubleValues[k], index, yValue, replace);
                }
                else if (floatAccessor.ContainsKey(yPath))
                {
                    var acccessor = floatAccessor[yPath];
                    double yValue = acccessor.GetValue(data);
                    UpdateYDoubleValues(origin.YDoubleValues[k], index, yValue, replace);
                }
                else if (longAccessor.ContainsKey(yPath))
                {
                    var acccessor = longAccessor[yPath];
                    double yValue = acccessor.GetValue(data);
                    UpdateYDoubleValues(origin.YDoubleValues[k], index, yValue, replace);
                }
                else if (booleanAccessor.ContainsKey(yPath))
                {
                    var acccessor = booleanAccessor[yPath];
                    double yValue = acccessor.GetValue(data) ? 1 : 0;
                    UpdateYDoubleValues(origin.YDoubleValues[k], index, yValue, replace);
                }
                else if (objectAccessor.ContainsKey(yPath))
                {
                    var acccessor = objectAccessor[yPath];
                    object yValue = acccessor.GetObjectValue(data);

                    if (yValue is IEnumerable enumerable)
                    {
                        List<double> doubleList = new List<double>();

                        foreach (var item in enumerable)
                        {
                            if (double.TryParse(item.ToString(), out double result))
                            {
                                doubleList.Add(result);
                            }
                        }

                        if (!replace)
                        {
                            origin.YDoubleValues.Insert(index, doubleList);
                        }
                        else if (origin.YDoubleValues[index] != doubleList)
                        {
                            origin.YDoubleValues[index] = doubleList;
                        }
                    }
                    else
                    {
                        double objectYDoubleValue = (double)Convert.ChangeType(yValue, typeof(double));
                        UpdateYDoubleValues(origin.YDoubleValues[k], index, objectYDoubleValue, replace);
                    }
                }
            }
        }

        internal void UpdateYDoubleValues(List<double> yDoubleList, int index, double objectYDoubleValue, bool replace)
        {
            if (!replace)
            {
                yDoubleList.Insert(index, objectYDoubleValue);
            }
            else if (yDoubleList[index] != objectYDoubleValue)
            {
                yDoubleList[index] = objectYDoubleValue;
            }
        }

        internal void UpdateDependentCollection(IDataManagerDependent dependent)
        {
            var xPath = dependent.XPath;
            var yPaths = dependent.YPaths;
            if (xPath == null || yPaths == null)
                return;

            IDataManagerDependent? xPathDataDependent = null;
            IDataManagerDependent? yPathDataDependent = null;
            foreach (var series in DependentObjectCollection)
            {
                if (series.XPath.Equals(dependent.XPath))
                {
                    xPathDataDependent = series;
                }

                for (int k = 0; k < yPaths.Count; k++)
                {
                    var yPath = yPaths[k];

                    if(series.YPaths.Contains(yPath))
                    {
                        yPathDataDependent = series;
                        break;
                    }
                }
            }

            if (xPathDataDependent != null && yPathDataDependent != null)
            {
                dependent.XIndexedList = xPathDataDependent.XIndexedList.ToList();
                dependent.ActualData = xPathDataDependent.ActualData.ToList();
                dependent.PointsCount = xPathDataDependent.PointsCount;

                switch (xPathDataDependent.XValueType)
                {
                    case ValueType.String:
                        dependent.XStringValues = xPathDataDependent.XStringValues.ToList();
                        break;
                    case ValueType.Double:
                    case ValueType.Int:
                    case ValueType.Float:
                    case ValueType.Long:
                        dependent.XDoubleValues = xPathDataDependent.XDoubleValues.ToList();
                        break;
                    case ValueType.DateTime:
                        dependent.XDateTimeValues = xPathDataDependent.XDateTimeValues.ToList();
                        dependent.XDoubleValues = xPathDataDependent.XDoubleValues.ToList();
                        break;
                    default:
                        break;
                }

                var yDoubleList = new List<List<double>>();

                foreach (var yValues in yPathDataDependent.YDoubleValues)
                {
                    yDoubleList.Add(yValues.ToList());
                }
                dependent.YDoubleValues = yDoubleList;
            }
            else
            {
                AddDependantObject(dependent);
                GenerateDataDependentList(dependent);
            }
        }

        private void AddDataPoint(int index, object? data, bool replace)
        {
            if (data == null)
                return;

            foreach (var dependentObject in DependentObjectCollection)
            {
                var xPath = dependentObject.XPath;
                var yPaths = dependentObject.YPaths;

                if (xPath == null || yPaths == null)
                    return;

                if (!replace)
                {
                    if (dependentObject.PointsCount == 0)
                        GenerateAccessorValues(dependentObject);

                    dependentObject.ActualData?.Insert(index, data);
                    dependentObject.PointsCount++;
                    double value = 0;

                    if (dependentObject.XIndexedList.Count > 0)
                        value = dependentObject.XIndexedList.Last() + 1;

                    dependentObject.XIndexedList.Add(value);
                }
                else if (dependentObject.ActualData != null)
                {
                    if (!dependentObject.ListenPropertyChange)
                        continue;
                    
                    dependentObject.ActualData[index] = data;
                }

                AddXDataPoint(dependentObject, xPath, data, index, replace);

                AddYDataPoint(dependentObject, yPaths, data, index, replace);
                dependentObject.UpdateArea();
            }
        }

        internal void RemoveDataPoint(IDataManagerDependent origin, string xPath, int index)
        {
            switch (origin.XValueType)
            {
                case ValueType.Double:
                case ValueType.Int:
                case ValueType.Float:
                case ValueType.Long:
                    {
                        origin.XDoubleValues.RemoveAt(index);
                        break;
                    }
                case ValueType.String:
                    {
                        origin.XStringValues.RemoveAt(index);
                        break;
                    }
                case ValueType.DateTime:
                    {
                        origin.XDateTimeValues.RemoveAt(index);
                        origin.XDoubleValues.RemoveAt(index);
                        break;
                    }
            }
        }

        private void RemoveData(int index)
        {
            foreach (var dependentObject in DependentObjectCollection)
            {
                var xPath = dependentObject.XPath;
                var yPaths = dependentObject.YPaths;

                if (stringAccessor.ContainsKey(xPath) || doubleAccessor.ContainsKey(xPath)
                      || dateTimeAccessor.ContainsKey(xPath) || integerAccessor.ContainsKey(xPath)
                      || floatAccessor.ContainsKey(xPath) || longAccessor.ContainsKey(xPath)
                      || booleanAccessor.ContainsKey(xPath) || objectAccessor.ContainsKey(xPath))
                {
                    RemoveDataPoint(dependentObject, xPath, index);
                    dependentObject.XIndexedList.Remove(dependentObject.XIndexedList.Last());
                }

                var yCount = yPaths.Count();
                for (int k = 0; k < yCount; k++)
                {
                    var yPath = yPaths[k];

                    if (dependentObject.IsGroupedYPath)
                    {
                        dependentObject.YDoubleValues.RemoveAt(index);
                    }
                    else
                    {
                        dependentObject.YDoubleValues[k].RemoveAt(index);
                    }
                }

                dependentObject.ActualData.RemoveAt(index);
                dependentObject.PointsCount--;
                dependentObject.UpdateArea();
            }
        }

        #endregion

        #region Listen Notifty Change
        internal void HookOrUnhookPropertyChangedEvent()
        {
            if (itemsSource is IEnumerable source)
            {
                IEnumerator enumerator = source.GetEnumerator();
                if (!enumerator.MoveNext())
                {
                    return;
                }

                if (listenPropertyChange)
                {
                    do
                    {
                        HookPropertyChangedEvent(enumerator.Current);
                    }
                    while (enumerator.MoveNext());
                    isPropertyChangedHooked = true;
                }
                else
                {
                    do
                    {
                        UnHookPropertyChangedEvent(enumerator.Current);
                    }
                    while (enumerator.MoveNext());
                    isPropertyChangedHooked = false;
                }
            }
        }

        private void UnHookPropertyChangedEvent(object currentEnumerator)
        {
            var currentItem = currentEnumerator as INotifyPropertyChanged;
            if (currentItem != null)
            {
                currentItem.PropertyChanged -= OnItemPropertyChanged;
            }
        }

        private void HookPropertyChangedEvent(object currentEnumerator)
        {
            var currentItem = currentEnumerator as INotifyPropertyChanged;
            if (currentItem != null)
            {
                currentItem.PropertyChanged -= OnItemPropertyChanged;
                currentItem.PropertyChanged += OnItemPropertyChanged;
            }
        }

        void OnItemPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (itemsSource is IEnumerable source)
            {
                int index = -1;
                foreach (object obj in source)
                {
                    index++;

                    if (obj == sender)
                        break;
                }

                if (index != -1)
                {
                    AddDataPoint(index, sender, true);
                }
            }
        }

        #endregion

        void ApplyCollectionChanges(NotifyCollectionChangedEventArgs self, Action<int, object?> insertAction, Action<int, object?> removeAction, Action resetAction)
        {
            switch (self.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    if (self.NewItems != null)
                        for (var i = 0; i < self.NewItems.Count; i++)
                        {
                            insertAction(i + self.NewStartingIndex, self.NewItems[i]);
                        }
                    break;

                case NotifyCollectionChangedAction.Move:
                    if (self.NewStartingIndex < 0 || self.OldStartingIndex < 0)
                        goto case NotifyCollectionChangedAction.Reset;

                    int insertIndex = self.NewStartingIndex;

                    if (self.OldItems != null)
                    {
                        for (var i = 0; i < self.OldItems.Count; i++)
                            removeAction(self.OldStartingIndex, self.OldItems[i]);

                        if (self.OldStartingIndex < self.NewStartingIndex)
                            insertIndex -= self.OldItems.Count - 1;

                        for (var i = 0; i < self.OldItems.Count; i++)
                            insertAction(insertIndex + i, self.OldItems[i]);
                    }
                    break;

                case NotifyCollectionChangedAction.Remove:
                    if (self.OldStartingIndex < 0)
                        goto case NotifyCollectionChangedAction.Reset;

                    if (self.OldItems != null)
                        for (var i = 0; i < self.OldItems.Count; i++)
                        removeAction(self.OldStartingIndex, self.OldItems[i]);
                    break;

                case NotifyCollectionChangedAction.Replace:

                    if (self.OldItems != null && self.NewItems != null)
                    {
                        if (self.OldStartingIndex < 0 || self.OldItems.Count != self.NewItems.Count)
                            goto case NotifyCollectionChangedAction.Reset;

                        for (var i = 0; i < self.OldItems.Count; i++)
                        {
                            removeAction(i + self.OldStartingIndex, self.OldItems[i]);
                            insertAction(i + self.OldStartingIndex, self.NewItems[i]);
                        }
                    }
                    break;

                case NotifyCollectionChangedAction.Reset:
                    {
                        resetAction();
                        break;
                    }
            }
        }
        #endregion

        #region Dispose

        internal void ClearSeriesDataPoints(IDataManagerDependent origin)
        {
            origin.PointsCount = 0;
            origin.XIndexedList?.Clear();
            origin.XDoubleValues?.Clear();
            origin.XStringValues?.Clear();
            origin.XDateTimeValues?.Clear();

            if (origin.YDoubleValues != null)
            {
                foreach (var doubleList in origin.YDoubleValues)
                {
                    doubleList.Clear();
                }

                origin.YDoubleValues.Clear();
            }

            if (origin.ActualData != null)
                origin.ActualData.Clear();
        }

        private void ResetDataPoint()
        {
            foreach (var dependentObject in DependentObjectCollection)
            {
                ClearSeriesDataPoints(dependentObject);
                dependentObject.UpdateArea();
            }
        }

        internal void DisposePropertyAccessor()
        {
            foreach (var dependent in DependentObjectCollection)
            {
                ClearSeriesDataPoints(dependent);
                DisposeDependentCollections(dependent);
                RemoveDependantAccessors(dependent);
            }

            DisposeDependentAccessors();
            DependentObjectCollection.Clear();
            DependentObjectCollection = null;
            linearDataChecker.Clear();
            linearDataChecker = null;
            listenPropertyChange = false;
            HookAndUnhookCollectionChangedEvent(itemsSource, null);
            HookOrUnhookPropertyChangedEvent();
        }

        // Remove PropertyValueAccessor in the exisitng accessor based on X and Y paths.
        internal void RemoveDependantAccessors(IDataManagerDependent dataManagerDependent)
        {
            string xPath = dataManagerDependent.XPath;
            if (xPath != null)
                RemoveAccessor(xPath);

            foreach (var yPath in dataManagerDependent.YPaths)
            {
                RemoveAccessor(yPath);
            }
        }

        internal void RemoveAccessor(string path)
        {
            if (stringAccessor.ContainsKey(path))
                stringAccessor.Remove(path);

            if (doubleAccessor.ContainsKey(path))
                doubleAccessor.Remove(path);

            if (integerAccessor.ContainsKey(path))
                integerAccessor.Remove(path);

            if (dateTimeAccessor.ContainsKey(path))
                dateTimeAccessor.Remove(path);

            if (booleanAccessor.ContainsKey(path))
                booleanAccessor.Remove(path);

            if (floatAccessor.ContainsKey(path))
                floatAccessor.Remove(path);

            if (longAccessor.ContainsKey(path))
                longAccessor.Remove(path);

            if (objectAccessor.ContainsKey(path))
                objectAccessor.Remove(path);
        }

        private void DisposeDependentCollections(IDataManagerDependent origin)
        {
            origin.XIndexedList = null;
            origin.XDoubleValues = null;
            origin.XStringValues = null;
            origin.XDateTimeValues = null;
            origin.YDoubleValues = null;
            origin.ActualData = null;
        }

        private void DisposeDependentAccessors()
        {
            stringAccessor = null;
            doubleAccessor = null;
            integerAccessor = null;
            dateTimeAccessor = null;
            booleanAccessor = null;
            floatAccessor = null;
            longAccessor = null;
            objectAccessor = null;
        }

        #endregion
    }
}
