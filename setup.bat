"C:\Program Files (x86)\Inno Setup 5\iscc.exe" setup\x86\setup.iss
"C:\Program Files (x86)\Inno Setup 5\iscc.exe" setup\x64\setup.iss
COPY setup\x86\setup.exe tmm_setup_ia32.exe /y 
COPY setup\x64\setup.exe tmm_setup_amd64.exe /y 