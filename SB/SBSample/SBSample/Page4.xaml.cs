using Syncfusion.Maui.Gauges;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SBSample;

public partial class Page4 : ContentPage, INotifyPropertyChanged
{
	private double size;

	public double Size
	{
		get { return size; }
		set
		{
			size = value;
			NotifyPropertyChanged();
		}
	}

	protected override void OnSizeAllocated(double width, double height)
	{
		base.OnSizeAllocated(width, height);

		Size = width / 4;
	}

	public Page4()
	{
		InitializeComponent();
	}




	public event PropertyChangedEventHandler PropertyChanged;

	// This method is called by the Set accessor of each property.  
	// The CallerMemberName attribute that is applied to the optional propertyName  
	// parameter causes the property name of the caller to be substituted as an argument.  
	private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
	{
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}

}

