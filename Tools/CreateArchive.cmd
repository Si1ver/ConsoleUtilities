@echo off
rem Copyright (c) Anton Vasiliev. All rights reserved.
rem Licensed under the MIT license.
rem See the License.md file in the project root for full license information.

rem This script will create an archive of the project.
rem Ignore filter list is stored in '.7zignore' file.

set seven_zip="%programfiles%\7-zip\7z.exe"

rem Test that 7-Zip exists at estimated path.
if not exist %seven_zip% goto no_seven_zip

rem Change directory to the project root.
cd ..

rem Get current date and time. The code is taken from this answer on Stack Overflow:
rem https://stackoverflow.com/questions/203090/how-do-i-get-current-datetime-on-the-windows-command-line-in-a-suitable-format#answer-203116
for /F "usebackq tokens=1,2 delims==" %%i in (`wmic os get LocalDateTime /VALUE 2^>NUL`) do if '.%%i.'=='.LocalDateTime.' set ldt=%%j
set ldt=%ldt:~0,4%-%ldt:~4,2%-%ldt:~6,2%_%ldt:~8,2%-%ldt:~10,2%-%ldt:~12,2%

rem Get parent directory name.
for %%* in (.) do set project=%%~n*

rem Compress files.
%seven_zip% a -x@.7zignore %project%_%ldt% .\*

goto :eof

:no_seven_zip
rem Display message that 7-Zip is not found.
echo Error: 7-Zip executable is not found at path: %seven_zip%
