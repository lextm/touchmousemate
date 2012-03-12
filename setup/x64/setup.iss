#define MyAppID "{9F282176-6DBB-468B-BA65-6ABDE4AC74B0}"
#define MyAppCopyright "Copyright (C) 2011-2012 Lex Li and other contributors"
#define MyAppName "Touch Mouse Mate"
#define MyAppVersion GetFileVersion("..\..\bin\TouchMouseMate.exe")
#pragma message "Detailed version info: " + MyAppVersion

[Setup]
AppName={#MyAppName}
AppVerName={#MyAppName}
AppPublisher=Lex Li (lextm)
AppPublisherURL=http://lextm.com
AppSupportURL=http://lextm.com
AppUpdatesURL=http://touchmousemate.codeplex.com
DefaultDirName={pf}\{#MyAppName}
DefaultGroupName={#MyAppName}
OutputDir=.
SolidCompression=true
AppCopyright={#MyAppCopyright}
VersionInfoVersion={#MyAppVersion}
VersionInfoCompany=LeXtudio
VersionInfoDescription={#MyAppName} {#MyAppVersion} Setup
VersionInfoTextVersion={#MyAppVersion}
InternalCompressLevel=max
VersionInfoCopyright={#MyAppCopyright}
Compression=zip
PrivilegesRequired=admin
ShowLanguageDialog=yes
WindowVisible=false
AppVersion={#MyAppVersion}
AppID={{#MyAppID}
UninstallDisplayName={#MyAppName}
SetupIconFile=mouse.ico
UninstallDisplayIcon={app}\mouse.ico
ArchitecturesInstallIn64BitMode=x64

[Languages]
Name: english; MessagesFile: compiler:Default.isl
[Types]
Name: Full; Description: All components are installed; Languages: 
Name: Custom; Description: Custom; Flags: iscustom
[Components]
Name: TMM; Description: Touch Mouse Mate components; Types: Custom Full; Languages: 
[Files]
; NOTE: Don't use "Flags: ignoreversion" on any shared system files

; dll used to check running notepad at install time
Source: "processviewer.exe"; Flags: dontcopy
;psvince is installed in {app} folder, so it will be
;loaded at uninstall time ;to check if notepad is running
Source: "processviewer.exe"; DestDir: "{app}"

Source: "..\..\bin\TouchMouseMate.exe"; DestDir: "{app}"; Flags: ignoreversion; Components: TMM
Source: "..\..\bin\TouchMouseMate.exe.config"; DestDir: "{app}"; Flags: ignoreversion; Components: TMM
Source: "..\..\bin\log4net.dll"; DestDir: "{app}"; Flags: ignoreversion; Components: TMM
Source: "TouchMouseSensor.dll"; DestDir: "{app}"; Components: TMM
Source: "Microsoft.Research.TouchMouseSensor.dll"; DestDir: "{app}"; Components: TMM
Source: "mouse.ico"; DestDir: "{app}"; Components: TMM

[Icons]
Name: "{group}\{cm:UninstallProgram,{#MyAppName}}"; Filename: "{uninstallexe}"; Components: TMM
Name: "{group}\Author's Blog"; Filename: "http://lextm.com"; Components: TMM
Name: "{group}\Report A Bug"; Filename: "http://touchmousemate.codeplex.com/workitem/list/basic"; Components: TMM
Name: "{group}\Homepage"; Filename: "http://touchmousemate.codeplex.com"; Components: TMM
Name: "{group}\Touch Mouse Mate"; Filename: "{app}\TouchMouseMate.exe"; WorkingDir: "{app}"; IconFilename: "{app}\mouse.ico"

[Run]
Filename: {win}\Microsoft.NET\Framework\v4.0.30319\ngen.exe; Parameters: "install ""{app}\TouchMouseMate.exe"""; WorkingDir: {app}; StatusMsg: Optimising Performance; Flags: runhidden skipifdoesntexist

[UninstallRun]
Filename: {win}\Microsoft.NET\Framework\v4.0.30319\ngen.exe; Parameters: "uninstall ""{app}\TouchMouseMate.exe"""; WorkingDir: {app}; StatusMsg: Optimising Performance; Flags: runhidden skipifdoesntexist

;[Registry]
;Root: "HKLM"; Subkey: "SOFTWARE\Microsoft\Windows\CurrentVersion\Run"; ValueType: string; ValueName: "Touch Mouse Mate"; ValueData: """{app}\TouchMouseMate.exe"""; Flags: uninsdeletevalue

[Code]
// =======================================
// Testing if under Windows safe mode
// =======================================
function GetSystemMetrics( define: Integer ): Integer; external
'GetSystemMetrics@user32.dll stdcall';

Const SM_CLEANBOOT = 67;

function IsSafeModeBoot(): Boolean;
begin
  // 0 = normal boot, 1 = safe mode, 2 = safe mode with networking
 Result := ( GetSystemMetrics( SM_CLEANBOOT ) <> 0 );
end;

// ======================================
// Testing version number string
// ======================================
function GetNumber(var temp: String): Integer;
var
  part: String;
  pos1: Integer;
begin
  if Length(temp) = 0 then
  begin
    Result := -1;
    Exit;
  end;
  pos1 := Pos('.', temp);
  if (pos1 = 0) then
  begin
    Result := StrToInt(temp);
  temp := '';
  end
  else
  begin
  part := Copy(temp, 1, pos1 - 1);
    temp := Copy(temp, pos1 + 1, Length(temp));
    Result := StrToInt(part);
  end;
end;

function CompareInner(var temp1, temp2: String): Integer;
var
  num1, num2: Integer;
begin
  num1 := GetNumber(temp1);
  num2 := GetNumber(temp2);
  if (num1 = -1) or (num2 = -1) then
  begin
    Result := 0;
    Exit;
  end;
  if (num1 > num2) then
  begin
  Result := 1;
  end
  else if (num1 < num2) then
  begin
  Result := -1;
  end
  else
  begin
  Result := CompareInner(temp1, temp2);
  end;
end;

function CompareVersion(str1, str2: String): Integer;
var
  temp1, temp2: String;
begin
  temp1 := str1;
  temp2 := str2;
  Result := CompareInner(temp1, temp2);
end;

function ProductRunning(): Boolean;
var
  ResultCode: Integer;
begin  
  ExtractTemporaryFile('processviewer.exe');
  if Exec(ExpandConstant('{tmp}\processviewer.exe'), 'touchmousemate', '', SW_HIDE,
     ewWaitUntilTerminated, ResultCode) then
  begin
    Result := ResultCode > 0;
    Exit;    
  end;  
  
  MsgBox('failed to check process', mbError, MB_OK);
end;

function ProductRunningU(): Boolean;
var
  ResultCode: Integer;
begin  
  if Exec(ExpandConstant('{app}\processviewer.exe'), 'touchmousemate', '', SW_HIDE,
     ewWaitUntilTerminated, ResultCode) then
  begin
    Result := ResultCode > 0;
    Exit;    
  end;  
  
  MsgBox('failed to check process.', mbError, MB_OK);
end;

function ProductInstalled(): Boolean;
begin
  Result := RegKeyExists(HKEY_LOCAL_MACHINE,
  'SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\{#MyAppID}_is1');
end;

function VCRuntimeInstalled(): Boolean;
var 
  installed: Cardinal;
begin
  if not (RegQueryDWordValue(HKEY_LOCAL_MACHINE,
  'SOFTWARE\Wow6432Node\Microsoft\VisualStudio\10.0\VC\VCRedist\x64', 'Installed', installed)) then
  begin
    Result := False;
    Exit;
  end;

  Result := installed = 1;
end;

function InitializeSetup(): Boolean;
var
  oldVersion: String;
  uninstaller: String;
  ErrorCode: Integer;
  compareResult: Integer;
begin
  if IsSafeModeBoot then
  begin
    MsgBox('Cannot install under Windows Safe Mode.', mbError, MB_OK);
    Result := False;
    Exit;
  end;
  
  if not VCRuntimeInstalled then
  begin
    MsgBox('{#MyAppName} needs Microsoft Visual C++ 2010 runtime to be installed (x64)', mbInformation, MB_OK);
    Result := False;
    Exit;
  end;

  if (not RegKeyExists(HKLM, 'Software\Microsoft\.NETFramework\policy\v4.0')) then
  begin
    MsgBox('{#MyAppName} needs Microsoft .NET Framework 4.0 to be installed', mbInformation, MB_OK);
    Result := False;
    Exit;
  end;

  if ProductRunning then
  begin
    MsgBox( '{#MyAppName} is running, please close it and run setup again.',
             mbError, MB_OK );
    Result := False;
    Exit;
  end;

  if not ProductInstalled then
  begin
    Result := True;
    Exit;
  end;

  RegQueryStringValue(HKEY_LOCAL_MACHINE,
    'SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\{#MyAppID}_is1',
    'DisplayVersion', oldVersion);
  compareResult := CompareVersion(oldVersion, '{#MyAppVersion}');
  if (compareResult > 0) then
  begin
    MsgBox('Version ' + oldVersion + ' of {#MyAppName} is already installed. It is newer than {#MyAppVersion}. This installer will exit.',
    mbInformation, MB_OK);
    Result := False;
    Exit;
  end
  else if (compareResult = 0) then
  begin
    if (MsgBox('{#MyAppName} ' + oldVersion + ' is already installed. Do you want to repair it now?',
    mbConfirmation, MB_YESNO) = IDNO) then
  begin
    Result := False;
    Exit;
    end;
  end
  else
  begin
    if (MsgBox('{#MyAppName} ' + oldVersion + ' is already installed. Do you want to override it with {#MyAppVersion} now?',
    mbConfirmation, MB_YESNO) = IDNO) then
  begin
    Result := False;
    Exit;
    end;
  end;
  // remove old version
  RegQueryStringValue(HKEY_LOCAL_MACHINE,
  'SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\{#MyAppID}_is1',
  'UninstallString', uninstaller);
  ShellExec('runas', uninstaller, '/SILENT', '', SW_HIDE, ewWaitUntilTerminated, ErrorCode);
  if (ErrorCode <> 0) then
  begin
  MsgBox( 'Failed to uninstall {#MyAppName} version ' + oldVersion + '. Please restart Windows and run setup again.',
   mbError, MB_OK );
  Result := False;
  Exit;
  end;

  Result := True;
end;

function InitializeUninstall(): Boolean;
begin
  if IsSafeModeBoot then
  begin
    MsgBox( 'Cannot uninstall under Windows Safe Mode.', mbError, MB_OK);
    Result := False;
    Exit;
  end;

  // check if notepad is running
  if ProductRunningU then
  begin
    MsgBox( '{#MyAppName} is running, please close it and uninstall again.',
             mbError, MB_OK );
    Result := false;
    Exit;
  end
  else
  begin
    Result := true;
  end;
end;
