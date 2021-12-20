using Microsoft.Maui;
using Microsoft.Maui.Controls;
using System.ComponentModel;

namespace Syncfusion.Maui.Graphics.Internals
{
	/// <summary>
	/// 
	/// </summary>
	[EditorBrowsable(EditorBrowsableState.Never)]
	public static class FontElement
	{
		/// <summary>
		/// 
		/// </summary>
		public static readonly BindableProperty FontProperty =
			BindableProperty.Create("Font", typeof(Font), typeof(ITextElement), default(Font),
									propertyChanged: OnFontPropertyChanged);
		/// <summary>
		/// 
		/// </summary>
		public static readonly BindableProperty FontFamilyProperty =
			BindableProperty.Create("FontFamily", typeof(string), typeof(ITextElement), default(string),
									propertyChanged: OnFontFamilyChanged);
		/// <summary>
		/// 
		/// </summary>
		public static readonly BindableProperty FontSizeProperty =
			BindableProperty.Create("FontSize", typeof(double), typeof(ITextElement), -1d,
									propertyChanged: OnFontSizeChanged,
									defaultValueCreator: FontSizeDefaultValueCreator);

		/// <summary>
		/// 
		/// </summary>
		public static readonly BindableProperty FontAttributesProperty =
			BindableProperty.Create("FontAttributes", typeof(FontAttributes), typeof(ITextElement), FontAttributes.None,
									propertyChanged: OnFontAttributesChanged);

		static readonly BindableProperty CancelEventsProperty =
			BindableProperty.Create("CancelEvents", typeof(bool), typeof(FontElement), false);


		static bool GetCancelEvents(BindableObject bindable) => (bool)bindable.GetValue(CancelEventsProperty);
		static void SetCancelEvents(BindableObject bindable, bool value)
		{
			bindable.SetValue(CancelEventsProperty, value);
		}

		static void OnFontPropertyChanged(BindableObject bindable, object oldValue, object newValue)
		{
			if (GetCancelEvents(bindable))
				return;

			SetCancelEvents(bindable, true);

			var font = (Font)newValue;
			if (font == Font.Default)
			{
				bindable.ClearValue(FontFamilyProperty);
				bindable.ClearValue(FontSizeProperty);
				bindable.ClearValue(FontAttributesProperty);
			}
			else
			{
				bindable.SetValue(FontFamilyProperty, font.Family);
				bindable.SetValue(FontSizeProperty, font.Size);
				bindable.SetValue(FontAttributesProperty, font.GetFontAttributes());
			}

			SetCancelEvents(bindable, false);
		}

		static bool OnChanged(BindableObject bindable)
		{
			if (GetCancelEvents(bindable))
				return false;

			ITextElement fontElement = (ITextElement)bindable;

			SetCancelEvents(bindable, true);
			bindable.SetValue(FontProperty, Font.OfSize(fontElement.FontFamily, fontElement.FontSize, enableScaling: false).WithAttributes(fontElement.FontAttributes));

			SetCancelEvents(bindable, false);
			return true;
		}

		static void OnFontFamilyChanged(BindableObject bindable, object oldValue, object newValue)
		{
			if (!OnChanged(bindable))
				return;

			((ITextElement)bindable).OnFontFamilyChanged((string)oldValue, (string)newValue);
		}

		static void OnFontSizeChanged(BindableObject bindable, object oldValue, object newValue)
		{
			if (!OnChanged(bindable))
				return;

			((ITextElement)bindable).OnFontSizeChanged((double)oldValue, (double)newValue);
		}

		static object FontSizeDefaultValueCreator(BindableObject bindable)
		{
			return ((ITextElement)bindable).FontSizeDefaultValueCreator();
		}

		static void OnFontAttributesChanged(BindableObject bindable, object oldValue, object newValue)
		{
			if (!OnChanged(bindable))
				return;

			((ITextElement)bindable).OnFontAttributesChanged((FontAttributes)oldValue, (FontAttributes)newValue);
		}

	}
}
