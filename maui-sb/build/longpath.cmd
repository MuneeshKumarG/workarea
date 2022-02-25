@ECHO OFF
pushd %~dp0
IF NOT EXIST "%~dp0\tools" (md "tools")
IF NOT EXIST "%~dp0\tools\modules" (md "tools\modules")
IF NOT EXIST "%~dp0\tools\nuget.exe" (@powershell -NoProfile -ExecutionPolicy Bypass -Command "(New-Object System.Net.WebClient).DownloadFile('https://dist.nuget.org/win-x86-commandline/latest/nuget.exe','tools/nuget.exe')")
IF NOT EXIST "%~dp0\tools\Cake" (tools\nuget.exe install Cake -ExcludeVersion -OutputDirectory "Tools" -Source https://www.nuget.org/api/v2/)
IF NOT EXIST "%~dp0\tools\modules\Cake.LongPath.Module" (tools\nuget.exe install Cake.LongPath.Module -PreRelease -Version 0.5.0 -ExcludeVersion -OutputDirectory "%~dp0\tools\modules" -Source https://www.nuget.org/api/v2/)
%~dp0\tools\Cake\Cake.exe build.cake -target="LoadlongPath" 