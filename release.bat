set msBuildDir=%WINDIR%\Microsoft.NET\Framework\v4.0.30319
call %MSBuildDir%\msbuild TouchMouseMate.sln /p:Configuration=Release /p:Platform="Any CPU"
@IF %ERRORLEVEL% NEQ 0 PAUSE