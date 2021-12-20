using Microsoft.Maui;
using Microsoft.Maui.Handlers;
using System.Runtime.Versioning;

namespace Syncfusion.Maui.Graphics.Internals
{
	/// <summary>
	/// 
	/// </summary>
    public partial class DrawableViewHandler
    {
		/// <summary>
		/// 
		/// </summary>
		public DrawableViewHandler() : base(DrawableViewHandler.ViewMapper)
		{

		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="mapper"></param>
		public DrawableViewHandler(PropertyMapper mapper) : base(mapper)
		{
		}
	}
}
