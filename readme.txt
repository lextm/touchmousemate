Touch Mouse Mate

(C) 2011-2012 Lex Li and other contributors

Project home page is at http://touchmousemate.codeplex.com

Source code is at http://github/lextm/touchmousemate

Released under MIT/X11 license.

Note that Touch Mouse SDK is available under a non-commercial license, so that Touch Mouse Mate as a derived product cannot be used for commercial purposes.

*Hints on the code base*

To compile the code, make sure that Windows 7 x64 is used.

1. Install Microsoft Touch Mouse 32 bit SDK to %systemdrive%\TMouse

http://research.microsoft.com/en-us/downloads/8ca8f8d1-c0b8-43a3-a519-0276195a6eec/

2. Install Microsoft Touch Mouse 64 bit SDK to %systemdrive%\TMouse

http://research.microsoft.com/en-us/downloads/8e2847f1-0e2d-48d3-b924-71400b358c17/

3. Execute prepare.bat so that SDK files are copied to the expected locations

4. Open the solution file and compile the project.

The default output in bin folder is for Windows 7 x64.

Installers can be created using Inno Setup. The iss scripts are in either x64 or x86 folder.