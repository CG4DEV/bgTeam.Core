@echo off

set Filters=+[bgTeam*]* -[Test*]* -[*Tests]*

set DotNet=C:\Program Files
set OpenCover=opencover\4.6.519
set ReportGenerator=reportgenerator\3.1.2

if exist "%~dp0reports" rmdir "%~dp0reports" /s /q
if exist "%~dp0coverage" rmdir "%~dp0coverage" /s /q
mkdir "%~dp0reports"
mkdir "%~dp0coverage"

call :RunOpenCover

if %errorlevel% equ 0 (
 call :RunReportGenerator
)
exit /b %errorlevel%

:RunOpenCover
for /d %%i in (.\tests\*) do (
for %%f in (%%i) do (
"%USERPROFILE%\.nuget\packages\%OpenCover%\tools\OpenCover.Console.exe" ^
-oldstyle ^
-register:user ^
-target:"%DotNet%\dotnet\dotnet.exe" ^
-targetargs:"test ./tests\%%~nxf" ^
-filter:"%Filters%" ^
-mergebyhash ^
-skipautoprops ^
-excludebyattribute:"System.CodeDom.Compiler.GeneratedCodeAttribute" ^
-output:"%~dp0reports\%%~nxf.xml"
)
)
exit /b %errorlevel%

:RunReportGenerator
"%USERPROFILE%\.nuget\packages\%ReportGenerator%\tools\ReportGenerator.exe" ^
-reports:"%~dp0reports\*.xml" ^
-targetdir:"%~dp0coverage\"
exit /b %errorlevel%
