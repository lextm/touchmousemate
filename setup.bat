"C:\Program Files (x86)\Inno Setup 5\iscc.exe" setup\x86\setup.iss
"C:\Program Files (x86)\Inno Setup 5\iscc.exe" setup\x64\setup.iss
XCOPY setup\x86\setup.exe .\setup_ia32.exe /y
XCOPY setup\x64\setup.exe .\setup_amd64.exe /y