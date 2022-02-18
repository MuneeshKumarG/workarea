
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Graphics.Platform;
using Microsoft.Maui.Handlers;
using System;

namespace Syncfusion.Maui.Graphics.Internals
{
	public partial class DrawableViewHandler : ViewHandler<IDrawableView, PlatformGraphicsView>
	{
		#region Methods

		protected override PlatformGraphicsView CreateNativeView()
		{
			return new PlatformGraphicsView(Context, VirtualView);
		}

		public void Invalidate()
		{
			this.NativeView?.Invalidate();
		}

		#endregion
	}
}
