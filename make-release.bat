@echo off
chcp 65001 >nul
setlocal enableextensions

for /f "tokens=1-4 delims=/-. " %%a in ("%date% %time%") do (
    set yy=%%a
    set mm=%%b
    set dd=%%c
    set hh=%%d
)
set hh=%time:~0,2%
set mn=%time:~3,2%
set ss=%time:~6,2%

if "%hh:~0,1%"==" " set hh=0%hh:~1,1%

set timestamp=%yy%%mm%%dd%_%hh%%mn%%ss%
set zipname=행복한 마인크래프트 %timestamp%.zip

if exist "dist" (
    rd /s /q "dist"
)
mkdir "dist"

copy ".\bin\Debug\happy-squirrel-mc.exe" ".\dist\행복한 마인크래프트.exe" >nul

xcopy ".\bin\Debug\bin" ".\dist\bin" /E /I /Y >nul

".\7z.exe" a -tzip "%zipname%" ".\dist\*" >nul
if errorlevel 1 (
    echo 압축 실패
    goto end
)

:end
endlocal
pause
