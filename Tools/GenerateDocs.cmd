@echo off
rem Copyright (c) Anton Vasiliev. All rights reserved.
rem Licensed under the MIT license.
rem See the License.md file in the project root for full license information.

rem This script will call Doxygen and create code documentation.

set doxygen="%ProgramFiles%\doxygen\bin\doxygen.exe"

set doxyfile="Docs\Doxyfile.cfg"

rem Test that Doxygen exists at estimated path.
if not exist %doxygen% goto no_doxygen

rem Change directory to the project root.
cd ..

rem Run Doxygen tool.
%doxygen% %doxyfile%

goto :eof

:no_doxygen
rem Display message that Doxygen is not found.
echo Error: Doxygen executable is not found at path: %doxygen%
