#addin "Cake.HockeyApp"
#addin "Cake.Xamarin"
#addin "Cake.FileHelpers"
using System.Diagnostics;
using System.Xml.Linq;
using System.IO;
using System.Text;
using System.Net;
using System.Text.RegularExpressions;

//////////////////////////////////////////////////////////////////////
// ARGUMENTS
//////////////////////////////////////////////////////////////////////

var target = Argument("target", "UpdateNugets");

Task("UpdateNugets")
.Does(()=>
{ 
		if(!System.IO.Directory.Exists("../packages"))
			System.IO.Directory.CreateDirectory("../packages");
 
	   	NuGetInstall("Newtonsoft.Json", new NuGetInstallSettings{ OutputDirectory = "../packages"});

		NuGetUpdate(@"..\SampleBrowser.Maui.sln" , new NuGetUpdateSettings {
		Source = new List<string>{"http://nexus.syncfusion.com/repository/nuget-hosted/"}	
		
		});
		
});


Task("Default")
.IsDependentOn("UpdateNugets");

RunTarget(target);
