@ECHO OFF

IF NOT EXIST %systemdrive%\TMouse\x86\bin\Win32\Release\TouchMouseSensor.dll (
  ECHO "Microsoft Touch Mouse SDK 32 bit must be installed to %systemdrive%\TMouse"
  GOTO EOF
 )

IF NOT EXIST setup\x86\TouchMouseSensor.dll (
  XCOPY %systemdrive%\TMouse\x86\bin\Win32\Release\TouchMouseSensor.dll setup\x86\
  XCOPY %systemdrive%\TMouse\x86\bin\Win32\Release\Microsoft.Research.TouchMouseSensor.dll setup\x86\
  XCOPY %systemdrive%\TMouse\x86\bin\Win32\Release\TouchMouseSensor.dll bin\
  XCOPY %systemdrive%\TMouse\x86\bin\Win32\Release\Microsoft.Research.TouchMouseSensor.dll lib\
)

IF NOT EXIST setup\x86\processviewer.exe (
  XCOPY processviewer\processviewer.exe setup\x86\
)

:EOF