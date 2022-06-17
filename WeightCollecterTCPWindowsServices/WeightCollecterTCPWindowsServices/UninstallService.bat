@ECHO OFF
if "%FrameworkDIR%"=="" set FrameworkDIR=%SystemRoot%\Microsoft.NET\Framework

REM Uninstall windows service of WeightCollecterWindowsService...
%FrameworkDIR%\v4.0.30319\installUtil /u WeightCollecterTCPWindowsServices.exe
if %errorlevel% neq 0 goto END
ECHO uninstall WeightCollecterWindowsService success!
GOTO END

:END
@ECHO ON
