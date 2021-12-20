using Microsoft.Maui.Graphics.Win2D;
using Microsoft.Maui.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Versioning;
using System.Text;
using System.Threading.Tasks;

namespace Syncfusion.Maui.Graphics.Internals
{
	public partial class DrawableViewHandler : ViewHandler<IDrawableView, W2DGraphicsView>
	{
		#region Methods

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		protected override W2DGraphicsView CreateNativeView()
		{
			var nativeGraphicsView = new W2DGraphicsView();
			nativeGraphicsView.Drawable = VirtualView;
			return nativeGraphicsView;
		}

		/// <summary>
		/// 
		/// </summary>
		public void Invalidate()
		{
			this.NativeView?.Invalidate();
		}

		#endregion
	}
}
