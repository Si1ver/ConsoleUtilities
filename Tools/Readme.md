# (Mostly) Useful command line scripts.

This folder contains Windows command line scripts that help automate some development tasks
for ConsoleUtilities library.

These scripts comes without support from the author. They may or may not work for you.

Scripts should work on Windows 10.

All scripts must be executed with Tools directory as working directory, otherwise unexpected
things may happen.

## CleanSolution.cmd

This script cleans up working directory. Compiled projects and temp files will be removed.
Generated documentation and code coverage data and reports also will be removed.

Please take your time to read script before executing it to make sure it will work as you
are expecting.

## CreateArchive.cmd

This script will create an archive of the project using free 7-Zip archiver.

Some files are excluded from archive. Ignore filter list is stored in '.7zignore' file in the
project root folder.

This script is useful when you've done some progress on the project and have to move
from one pc to another but you are not ready to commit and push your changes yet.

7-Zip is supposed to be found in it's default install location.

## GenerateDocs.cmd

This script will call Doxygen and create code documentation.

Doxygen is supposed to be found in it's default install location.

## TestCodeCoverage.cmd

This script will produce code coverage report using OpenCover and ReportGenerator (from local
NuGet package cache).

Generated report can be found at Coverage\Report\index.htm

To make sure that needed nuget packages are installed you may need to build
ConsoleUtilitiesTests project or manually execute nuget restore on tests project.
