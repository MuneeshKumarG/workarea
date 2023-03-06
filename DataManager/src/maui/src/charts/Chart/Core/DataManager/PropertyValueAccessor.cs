using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq.Expressions;

#if WinUI
namespace Syncfusion.UI.Xaml.Charts
#else
namespace Syncfusion.Maui.Charts
#endif
{
    /// <summary>
    /// Returns the value of current enumurator
    /// </summary>
    /// <typeparam name="Ttype">The type of lamda will create</typeparam>
    /// # [Example](#tab/tabid-1)
    /// <code><![CDATA[
    /// var propetyInfo = currObj.GetType().GetRuntimeProperty("path");
    /// var type = propertyinfo.PropertyType;
    /// if(type == typeof(double))
    ///     {
    ///         PropertyValueAccessor<double> accessor = new PropertyValueAccessor<double>(propetyInfo);
    ///         do
    ///         {
    ///             var value = accessor.GetValue(enumerator.Current);
    ///             . . .
    ///         }
    ///         while (enumerator.MoveNext()) ;
    ///     }
    /// ]]>
    /// </code>
    internal class PropertyValueAccessor<Ttype> 
    {
        #region Properties
        internal Func<object, Ttype> GetMethod { get; set; }
        internal Func<object, object> GetObjectMethod { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// 
        /// </summary>
        /// <param name="propertyInfo"></param>
        public PropertyValueAccessor(PropertyInfo propertyInfo)
        {
            this.InitializeLamda(propertyInfo);
        }

        #endregion

        #region Methods

        internal Ttype GetValue(object item)
        {
            return GetMethod != null ? GetMethod(item) : (Ttype)item;
        }

        internal object GetObjectValue(object item)
        {
            return GetObjectMethod != null ? GetObjectMethod(item) : item;
        }

        private void InitializeLamda(PropertyInfo propertyInfo)
        {
            if (!propertyInfo.CanRead || propertyInfo.DeclaringType == null)
            {
                return;
            }

            // preparing parameter, object type
            var instance = Expression.Parameter(typeof(object), "instance");

            var methodInfo = propertyInfo.GetGetMethod();

            // non-instance for static method, or ((TInstance)instance)
            var instanceCast = methodInfo != null && methodInfo.IsStatic ? null :
                Expression.Convert(instance, propertyInfo.DeclaringType);

            var propertyAccess = Expression.Property(instanceCast, propertyInfo);

            GetLambda(propertyInfo, propertyAccess, instance);
        }

        internal void GetLambda(PropertyInfo info, MemberExpression propertyAccess, ParameterExpression instance)
        {
            switch (Type.GetTypeCode(info.PropertyType))
            {
                case TypeCode.Boolean:
                case TypeCode.Int16:
                case TypeCode.String:
                case TypeCode.Double:
                case TypeCode.Int32:
                case TypeCode.DateTime:
                case TypeCode.Single:
                case TypeCode.Int64:
                    {
                        var lambda = Expression.Lambda<Func<object, Ttype>>(propertyAccess, instance);
                        this.GetMethod = lambda.Compile();
                        break;
                    }
                default:
                    {
                        var castPropertyValue = Expression.Convert(propertyAccess, typeof(object));
                        var lambda = Expression.Lambda<Func<object, object>>(castPropertyValue, instance);
                        this.GetObjectMethod = lambda.Compile();
                        break;
                    }
            }
        }

        #endregion
    }
}
