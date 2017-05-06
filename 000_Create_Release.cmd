@echo off
cls
echo ***********************************************
echo *** Achordeon release/setup packaging...          **
echo *** This will create a NUGET and SQUIRREL package **
echo ***                                               **
echo *** Attention!                                    **
echo *** =========                                    **
echo *** Please make sure you have all folders and     **
echo *** and tools in place, and restored all          **
echo *** referenced packages (use VS/restore packages) **
echo ***                                               **
echo *** Subfolders required:                          **
echo *** 003_NUGET - NUGET package is created here     **
echo *** 004_RELEASE - this is where the release goes  **
echo *** 005_TOOLS - several tools (extract ZIP file)  **
echo ***********************************************
echo.
echo.
echo Press any key to continue or CTRL-C to abort...
echo.
pause
if not exist "%~dp0\003_NUGET" mkdir  "%~dp0\003_NUGET"
if not exist "%~dp0\004_RELEASE" mkdir  "%~dp0\004_RELEASE"
if not exist "%~dp0\005_TOOLS" goto error_tools_missing
echo Creating NUGET package...
rem  Build the whole solution and create a NUGET package
005_TOOLS\nuget pack Achordeon.Shell.Wpf\Achordeon.Shell.Wpf.csproj -Build -OutputDirectory 003_NUGET -IncludeReferencedProjects
echo.
echo.
rem fetch Version (required for Squirrel input file name)
for /F "tokens=1" %%F in ('005_TOOLS\filever.exe "%~dp0\000_BIN\Achordeon.exe"') do (set RELVERSION=%%F)
echo Detected version: "%RELVERSION%"
echo.
echo Creating SQUIRREL release...
echo.
rem now releasify the NUGET package and create the release
packages\squirrel.windows.1.6.0\tools\squirrel.com --releaseDir=004_RELEASE --releasify "003_NUGET\Achordeon.%RELVERSION%.nupkg" --packagesDir=packages --icon=Achordeon.Shell.Wpf\logo.ico --setupIcon=Achordeon.Shell.Wpf\logo.ico --no-msi
rem rename the setup to the release file name
move "004_RELEASE\Setup.exe" "004_RELEASE\Achordeon.%RELVERSION%.exe"
echo.
echo Finished. The release now should be here: 
echo "%~dp0\004_RELEASE\Achordeon.%RELVERSION%.exe"
echo.
echo ************************
echo *** end of conversation **
echo ************************
goto end
:error_tools_missing
echo.
echo.
echo **************************************************
echo *** ERROR: TOOLS ARE MISSING!                        **
echo *** Please unzip the tools to 005_TOOLS folder first **
echo **************************************************
echo.
echo.
:end
pause