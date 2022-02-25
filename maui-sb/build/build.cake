#addin nuget:?package=Cake.AppCenter&version=1.2.0
#addin nuget:?package=Cake.Xamarin&version=3.1.0
#addin nuget:?package=Cake.FileHelpers&version=3.3.0
#addin nuget:?package=Cake.Email&version=0.8.0&loaddependencies=true
#addin "NuGet.Core" 

#tool nuget:?package=NUnit.ConsoleRunner&version=3.4.0
#tool "nuget:?package=GitVersion.CommandLine&version=5.3.7"

using Cake.Email.Common;
using System.Diagnostics;
using System.Xml.Linq;
using System.IO;
using System.Text;
using System.Net;
using System.Text.RegularExpressions;
using Cake.Common.Tools.DotNetCore.Restore;
using Cake.Common.IO;
using Cake.Common.Tools.DotNetCore;
using Cake.Common.Tools.DotNetCore.Build;
using Cake.Common.Tools.DotNetCore.MSBuild;
using Cake.Common.Tools.MSBuild;
using Cake.Common.Tools.NuGet.Pack;
using Cake.Core;
using Cake.Core.Diagnostics;

using System.Xml;
using NuGet;

/////////////////////////////////////////////////////////////////////////
// ARGUMENTS
//////////////////////////////////////////////////////////////////////////

var target = Argument("target", "Default");
var apkLocation = "";
var configuration = Argument("configuration", "Release");
var API_TOKEN = Argument<string>("apitoken", "");
var targetBranch = Argument<string>("targetBranch", "");
var STUDIO_VERSION = Argument<string>("studioversion","");
var studio_version = Argument("studio_version", STUDIO_VERSION).Split('.'); 
var preReleaseNumber=Argument<string>("preReleaseNumber","");
var nugetserverurl=Argument<string>("nugetserverurl","http://nexus.syncfusion.com/repository/nuget-hosted/,https://api.nuget.org/v3/index.json");
var nugetapikey=Argument<string>("nugetapikey","");
DirectoryPath currentDirectory = MakeAbsolute(Directory("../"));
DirectoryInfo currentDirectoryInfo = new DirectoryInfo(currentDirectory.FullPath);
var machineIP = Argument("machineip", " ");
var isScheduler = Argument("isscheduler"," ");
var publishedDate =Argument("publishdate"," ");
var jobLink = Argument("joblink"," ");
var gitlabSourceBranch = Argument<string>("sourcebranch"," ");
string apkSharedLocation = machineIP + "\\com.syncfusion.SampleBrowser.Maui-Signed_"+publishedDate+".apk";
string ipaSharedLocation = machineIP + "\\SampleBrowser.Maui.iOS_"+publishedDate+".ipa";
List<string> nugetSource = new List<string>();

var nugetserverurls=nugetserverurl.Split(',');

foreach(var nugeturl in nugetserverurls)
{
    nugetSource.Add(nugeturl);
}

nugetserverurl =nugetserverurl.Split(',')[0];

Task("LoadlongPath")
.Does(() =>
{
var fileSystemType = Context.FileSystem.GetType();
Information(fileSystemType.ToString());
if (fileSystemType.ToString()=="Cake.LongPath.Module.LongPathFileSystem")
{
    Information("Suscessfully loaded {0}", fileSystemType.Assembly.Location);
}
else
{
    Error("Failed to load Cake.LongPath.Module");
}
})
.OnError(exception =>
{
	FileWriteText("../cakelog.txt", "Error on LoadlongPath task:" + exception.ToString());
	throw new Exception("Cake process Failed on LoadlongPath task");
});

string revisionName = studio_version[0] + "." + studio_version[1] + ".12." + "0";

Task("Build")
.Does(()=>
{
    
	if(IsRunningOnWindows())
	{
		if(!System.IO.Directory.Exists("../packages"))
			System.IO.Directory.CreateDirectory("../packages");

        //SetVersionCode(true); 
		
		EnsureDirectoryExists("../cireports/warnings/");
		EnsureDirectoryExists("../cireports/errorlogs/");
 
		MSBuild(@"..\SampleBrowser.Maui.sln", GetMSBuildSettings().
		WithRestore().WithProperty("configuration","release").
						AddFileLogger(new MSBuildFileLogger { LogFile = "../cireports/warnings/SampleBrowser.Maui.txt",  MSBuildFileLoggerOutput = MSBuildFileLoggerOutput.WarningsOnly }).
						AddFileLogger(new MSBuildFileLogger { LogFile = "../cireports/errorlogs/SampleBrowser.Maui.txt", MSBuildFileLoggerOutput = MSBuildFileLoggerOutput.ErrorsOnly}));
	    
		if(FileExists("../cireports/errorlogs/SampleBrowser.Maui.txt"))
			DeleteFile("../cireports/errorlogs/SampleBrowser.Maui.txt");
			
		StartProcess("cmd" , new ProcessSettings { Arguments = "/c adb kill-server" });
		
	}
	else
	{
		
	}
})
.OnError(exception =>
{
	FileWriteText("../cakelog.txt", "Error on Build task:" + exception.ToString());
	throw new Exception("Cake process Failed on Build task");
});


MSBuildSettings GetMSBuildSettings()
{
	var buildSettings =  new MSBuildSettings {
        MSBuildPlatform = Cake.Common.Tools.MSBuild.MSBuildPlatform.x64,
        Configuration = configuration,
    };

			var vsInstallation =
            VSWhereLatest(new VSWhereLatestSettings { Requires = "Microsoft.Component.MSBuild", IncludePrerelease = true })
            ?? VSWhereLatest(new VSWhereLatestSettings { Requires = "Microsoft.Component.MSBuild" });

        if (vsInstallation != null)
        {
            buildSettings.ToolPath = vsInstallation.CombineWithFilePath(@"MSBuild\Current\Bin\MSBuild.exe");
            if (!FileExists(buildSettings.ToolPath))
                buildSettings.ToolPath = vsInstallation.CombineWithFilePath(@"MSBuild\15.0\Bin\MSBuild.exe");
        }
	return buildSettings;	
}

public string SetVersionCode(bool isBuild)
{
    HttpWebRequest webRequest = HttpWebRequest.CreateHttp("https://api.appcenter.ms/v0.1/apps/xamarincore/MAUI-SB-Android/releases?published_only=true");
    webRequest.Headers.Add("X-API-Token", "54a3e5a1bac5f7defca0538b2eb7089c8e6debf6");
    Stream stream = webRequest.GetResponse().GetResponseStream();
    string result;
    using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
    {
        result = reader.ReadToEnd().Substring(28,4);
    }
    webRequest.Abort();

     int version= Convert.ToInt32(result) + 1;

    //if (isBuild)
    //{
	   // XNamespace androidNamespace =  "http://schemas.android.com/apk/res/android";
    //     XDocument xmlFile = XDocument.Load("../SB.Droid/Properties/AndroidManifest.xml");
    //     var query = from c in xmlFile.Elements("manifest") select c;
    //     foreach (XElement element in query)
    //     {
    //         element.Attribute(androidNamespace+"versionCode").Value = (version).ToString();
    //         element.Attribute(androidNamespace+"versionName").Value = revisionName;
		  //  element.Attribute("package").Value = "com.syncfusion.sb";
    //     }
	
	   // xmlFile.Save("../SB.Droid/Properties/AndroidManifest.xml");

	   // xmlFile = XDocument.Load("../SB.Droid/Properties/AndroidManifest.xml");
	   // var applicationElements = xmlFile.Element("manifest") .Elements("application");
	
	   // foreach (XElement element in applicationElements)
    //     {
		  //  element.Attribute(androidNamespace+"label").Value = "XF SB";
    //     }
	
    //     xmlFile.Save("../SB.Droid/Properties/AndroidManifest.xml");
	
	   // string text = System.IO.File.ReadAllText("../SB.Droid/SplashScreenActivity.cs");
    //     text = text.Replace("MainActivity", "MainActivityExt");
    //     System.IO.File.WriteAllText("../SB.Droid/SplashScreenActivity.cs", text);
   // }

    return version.ToString();
}



Task("CodeViolation").Does(() =>
{
	if(IsRunningOnWindows())
	{
		 
	}
})
.OnError(exception =>
{
	FileWriteText("../cakelog.txt", "Error on CodeViolation task:" + exception.ToString());
	throw new Exception("Cake process Failed on CodeViolation task");
});


Task("Publish")
.Does(()=>
{	
	if(IsRunningOnWindows())
	{
        if(isScheduler == "true")
			System.IO.File.Copy("../SampleBrowser.Maui/bin/Release/net6.0-android/com.syncfusion.SampleBrowser.Maui-Signed.apk",machineIP+@"\com.syncfusion.SampleBrowser.Maui-Signed_"+publishedDate+".apk");

        if(!System.IO.Directory.Exists("../cireports/releasenotes"))
			System.IO.Directory.CreateDirectory("../cireports/releasenotes");

        FileWriteText("../cireports/releasenotes/releasenotes.txt", "");

		AppCenterDistributeRelease(new AppCenterDistributeReleaseSettings
        	{
            		File = @"..\SampleBrowser.Maui\bin\Release\net6.0-android\com.syncfusion.SampleBrowser.Maui-Signed.apk",
            		Token ="54a3e5a1bac5f7defca0538b2eb7089c8e6debf6",
            		App = "xamarincore/MAUI-SB-Android",
                    ReleaseNotesFile = $"../cireports/releasenotes/releasenotes.txt",
            		Group = "All-users-of-MAUI-SB-Android"
        	});
	}
	else
	{

	}
})
.OnError(exception =>
{
	FileWriteText("../cakelog.txt", "Error on Publish task:" + exception.ToString());
	throw new Exception("Cake process Failed on Publish task");
});

Task("SendEmail")
    .WithCriteria(isScheduler == "true")
    .Does(() =>
{ 
   string publishStatus="failed";
   if((System.IO.File.Exists(machineIP+@"\com.syncfusion.SampleBrowser.Maui-Signed_"+publishedDate+".apk")))
   {
   string repositoryName = "maui-samplebrowser";
   string emailTemplateLocation = "./EmailTemplates/MAUIAPKSuccess.htm";
   string successMailContents = System.IO.File.ReadAllText(emailTemplateLocation);   
   if(System.IO.File.Exists(machineIP+@"\com.syncfusion.SampleBrowser.Maui-Signed_"+publishedDate+".apk"))
   {
     successMailContents = successMailContents.Replace("##APKLOCATION##",apkSharedLocation);   
	 publishStatus = "success";
   }
   else
   {
	  successMailContents = successMailContents.Replace("##APKLOCATION##","<a href="+jobLink+">APK file not available due to CI failure</a>"); 
   }

   successMailContents = successMailContents.Replace("##CURRENTTIME##",DateTime.Now.ToString("dd-MM-yyyy    hh:mm:ss tt IST"));
   successMailContents = successMailContents.Replace("##SOURCEBRANCH##",gitlabSourceBranch);
   System.IO.File.WriteAllText(emailTemplateLocation, successMailContents);
   string mailResult = System.IO.File.ReadAllText(emailTemplateLocation);
   
   try
	{
	   var result = Email.SendEmail(
                senderName: "Jenkins CI", 
                senderAddress: "buildautomation@syncfusion.com",
                recipients: new[]
                {
                    new MailAddress("citeam@syncfusion.com", "CI Team"),
                    new MailAddress("xamarin@syncfusion.com", "Xamarin Team"),
                    
                },
                subject: "CI - "+ repositoryName + " - publish "+publishStatus,
                htmlContent: mailResult,
                textContent: "",
                attachments: null,
                settings: new EmailSettings 
                {
                    SmtpHost = "smtp.office365.com",
                    Port = 587,
                    EnableSsl = false,
                    Username = "buildautomation@syncfusion.com",
                    Password = "Coolcomp299"
                }
        );
    if (result.Ok)
    {
        Information("Email succcessfully sent");
    }
    else
    {
        Error("Failed to send email: {0}", result.Error);
    }
	}
	catch(Exception ex)
	{
	Error("{0}", ex);
	}
   }
})
.OnError(exception =>
{
	FileWriteText("../cakelog.txt", "Error on SendEmail task:" + exception.ToString());
	throw new Exception("Cake process Failed on SendEmail task");
});




Task("Default")
.IsDependentOn("Publish");

RunTarget(target);
