@echo off
cls
echo ************************************************************************
echo *** This will create a NUGET package **
echo ************************************************************************
echo.
echo.
echo Press any key to continue or CTRL-C to abort...
echo.
pause
005_TOOLS\nuget pack Achordeon.Shell.Wpf\Achordeon.Shell.Wpf.csproj -Build -OutputDirectory 003_NUGET -IncludeReferencedProjects

for /F "tokens=1" %%F in ('005_TOOLS\filever.exe "%~dp0\000_BIN\Achordeon.exe"') do (set RELVERSION=%%F)
echo Detected version: "%RELVERSION%"
packages\squirrel.windows.1.6.0\tools\squirrel --releaseDir=004_RELEASE --releasify "003_NUGET\Achordeon.%RELVERSION%.nupkg" --packagesDir=packages --icon=Achordeon.Shell.Wpf\logo.ico --setupIcon=Achordeon.Shell.Wpf\logo.ico

 
echo.
echo.
echo.
echo **************
echo *** Done! **
echo **************


pause
