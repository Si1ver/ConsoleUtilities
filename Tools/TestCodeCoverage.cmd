@echo off
rem Copyright (c) Anton Vasiliev. All rights reserved.
rem Licensed under the MIT license.
rem See the License.md file in the project root for full license information.

rem This script will produce code coverage report using OpenCover and ReportGenerator (from local NuGet package cache).

set open_cover="%userprofile%\.nuget\packages\opencover\4.6.519\tools\OpenCover.Console.exe"
set report_generator="%userprofile%\.nuget\packages\reportgenerator\3.0.2\tools\ReportGenerator.exe"

set coverage_target="dotnet.exe"
set coverage_targetargs="test ConsoleUtilitiesTests\ConsoleUtilitiesTests.csproj /p:DebugType=full"
set coverage_filter="+[ConsoleUtilities*]* -[xunit*]* -[ConsoleUtilitiesTests*]*"
set coverage_output=Coverage\Data\Coverage.xml
set report_targetdir=Coverage\Report
set report_historydir=Coverage\History

rem Test that OpenCover exists at estimated path.
if not exist %open_cover% goto no_open_cover

rem Test that ReportGenerator exists at estimated path.
if not exist %report_generator% goto no_report_generator

rem Change directory to the project root.
cd ..

rem Ensure the needed directories are created.
rem Raw data from OpenCover.
mkdir Coverage\Data
rem History data for ReportGenerator.
mkdir Coverage\History
rem Report from ReportGenerator.
mkdir Coverage\Report

rem Clean solutions to ensure we will have pdb files with full debug info.
dotnet clean .\ConsoleUtilities\ConsoleUtilities.csproj
dotnet clean .\ConsoleUtilitiesTests\ConsoleUtilitiesTests.csproj

rem Run tools.
%open_cover% -register:user -target:%coverage_target% -targetargs:%coverage_targetargs% -filter:%coverage_filter% -output:%coverage_output% -oldstyle
%report_generator% -reports:%coverage_output% -targetdir:%report_targetdir% -historydir:%report_historydir%

goto :eof

:no_open_cover
rem Display message that OpenCover is not found.
echo Error: OpenCover executable is not found at path: %open_cover%

goto :eof

:no_report_generator
rem Display message that ReportGenerator is not found.
echo Error: ReportGenerator executable is not found at path: %report_generator%
