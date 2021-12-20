using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syncfusion.Maui.Core
{
	/// <summary>
	/// Specifies the type of the animation for the views in <see cref="SfEffectsView" />.
	/// </summary>
	/// <remarks>This enumeration has a Flags attribute that allows a bitwise combination of its member values.</remarks>
	[Flags]
	public enum SfEffects
	{
		/// <summary>
		/// No effect.
		/// </summary>
		None = 1,

		/// <summary>
		/// Smooth transition on the background color of the view.
		/// </summary>
		Highlight = 2,

		/// <summary>
		/// Ripple is a growable circle, and it grows till the whole layout is filled.
		/// It depends on <see cref="SfEffectsView.InitialRippleFactor"/>.
		/// </summary>
		Ripple = 4,

		/// <summary>
		/// Scale is smooth scaling transition from actual size to the specified
		/// <see cref="SfEffectsView.ScaleFactor"/> in pixels.
		/// </summary>
		Scale = 8,

		/// <summary>
		/// Selection is smooth color transition to denote the view state as Selected.
		/// </summary>
		Selection = 16,

		/// <summary>
		/// Rotation is a circular movement of the view based on the specified <see cref="SfEffectsView.Angle"/>.
		/// </summary>
		Rotation = 32,
	}

	/// <summary>
	/// Specifies the start position of the ripple effects.
	/// </summary>
	public enum RippleStartPosition
	{
		/// <summary>
		/// Ripple starts from the left of the view.
		/// </summary>
		Left = 1,

		/// <summary>
		/// Ripple starts from the top of the view.
		/// </summary>
		Top = 2,

		/// <summary>
		/// Ripple starts from the right of the view.
		/// </summary>
		Right = 3,

		/// <summary>
		/// Ripple starts from the bottom of the view.
		/// </summary>
		Bottom = 4,

		/// <summary>
		/// Ripple starts from the center of the view.
		/// </summary>
		Default = 5,

		/// <summary>
		/// Ripple starts from the TopLeft of the view.
		/// </summary>
		TopLeft = 6,

		/// <summary>
		/// Ripple starts from the TopRight of the view.
		/// </summary>
		TopRight = 7,

		/// <summary>
		/// Ripple starts from the BottomLeft of the view.
		/// </summary>
		BottomLeft = 8,

		/// <summary>
		/// Ripple starts from the BottomRight of the view.
		/// </summary>
		BottomRight = 9,
	}

	/// <summary>
	/// Specifies the effect for AutoResetEffect.
	/// </summary>
   [Flags]
	public enum AutoResetEffects
	{
		/// <summary>
		/// No effect.
		/// </summary>
		None,

		/// <summary>
		/// Smooth transition on the background color of the view.
		/// </summary>
		Highlight,

		/// <summary>
		/// Ripple is a growable circle, and it grows till the whole layout is filled.
		/// It depends on <see cref="SfEffectsView.InitialRippleFactor"/>.
		/// </summary>
		Ripple,

		/// <summary>
		/// Scale is smooth scaling transition from actual size to the specified
		/// <see cref="SfEffectsView.ScaleFactor"/> in pixels.
		/// </summary>
		Scale,
	}
}
