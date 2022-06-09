@ECHO OFF
set path=%~dp0bin;%path%>> %SystemRoot%\System32\autoexec.nt

if "%FrameworkDIR%"=="" set FrameworkDIR=%SystemRoot%\Microsoft.NET\Framework

REM Install windows service of WeightCollecterWindowsService...
%FrameworkDIR%\v4.0.30319\installUtil WeightCollecterWindowsService.exe
if %errorlevel% neq 0 goto END
ECHO install WeightCollecterWindowsService success!
GOTO END

:END
@ECHO ON
