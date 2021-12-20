
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Graphics.Native;
using Microsoft.Maui.Handlers;
using System;

namespace Syncfusion.Maui.Graphics.Internals
{
	public partial class DrawableViewHandler : ViewHandler<IDrawableView, NativeGraphicsView>
	{
		#region Methods

		protected override NativeGraphicsView CreateNativeView()
		{
			return new NativeGraphicsView(Context, VirtualView);
		}

		public void Invalidate()
		{
			this.NativeView?.Invalidate();
		}

		#endregion
	}
}
