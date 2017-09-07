@echo off
IF EXIST interlisBaseDir goto installService




rem #######################################################################################
rem INSTALL SERVICE
rem #######################################################################################

:installService
echo Installing Service...
echo.
echo please give the password for user administrator and press enter:
@runas /user:administrator "ASTRA.EMSG.AxisImportService.exe --install" > nul
echo.

if %ERRORLEVEL% NEQ 0 goto installFailed
echo "Installation successfully done."





rem #######################################################################################
rem START SERVICE
rem #######################################################################################

:startService
echo.
echo Starting windows service...
REM net start EMSGAxisImportService
if %ERRORLEVEL% NEQ 0 goto startFailed





rem #######################################################################################
rem INSTALL FAILED
rem #######################################################################################

:installFailed
echo !!! Service Installation failed.
echo.
echo this Script needs administrative privileges for installing the windows service.
echo automatic gathering failed, please run this script as admin
echo.
goto finish





rem #######################################################################################
rem START FAILED
rem #######################################################################################
:startFailed
echo.
echo !!! Service start failed
echo.
echo The service could not be installed. Please check the Windows Event Log.
echo.
goto finish


rem #######################################################################################
rem FINISHED
rem #######################################################################################

:finish
echo Setup done.