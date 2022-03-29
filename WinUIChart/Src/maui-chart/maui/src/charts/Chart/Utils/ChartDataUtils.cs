using System.Reflection;

namespace Syncfusion.Maui.Charts
{
	/// <summary>
	/// Contains utility methods to manipulate data.
	/// </summary>
	internal class ChartDataUtils
	{
		private ChartDataUtils()
		{
		}

		/// <summary>
		/// Gets the property from the specified object.
		/// </summary>
		/// <param name="obj">Object to retrieve a property.</param>
		/// <param name="path">Property name</param>
		/// <returns>The property.</returns>
		internal static PropertyInfo? GetPropertyInfo(object obj, string path)
		{
			//TODO: consider if it needed.
            //return obj.GetType().GetTypeInfo().GetDeclaredProperty(path);
			return obj.GetType().GetRuntimeProperty(path);
		}
	}
}
