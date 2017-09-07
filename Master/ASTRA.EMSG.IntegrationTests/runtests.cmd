@pushd %~dp0

setlocal

set exitcode=0

..\packages\SpecRun.Runner.1.5.2\tools\SpecRun.exe %*

set exitcode=%errorlevel%

@popd

exit /b %exitcode%
