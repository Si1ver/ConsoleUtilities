@echo off
rem Copyright (c) Anton Vasiliev. All rights reserved.
rem Licensed under the MIT license.
rem See the License.md file in the project root for full license information.

rem This script will clean up working directory. Compiled projects and temp files will be removed.
rem Generated documentation and code coverage data and reports also will be removed.

rem Change directory to the project root.
cd ..

rem Remove all 'bin' and 'obj' folders.
for /f %%i in ('dir /a:d /s /b obj') do rd /s /q %%i
for /f %%i in ('dir /a:d /s /b bin') do rd /s /q %%i

rem Remove coverage data and reports.
rd /s /q .\Coverage

rem Remove generated documentation and logs.
rd /s /q .\Docs\html
del /s /q .\Docs\Warnings.log
