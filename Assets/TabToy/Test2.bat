cd /D %~dp0 
cd /D Excel
setlocal enabledelayedexpansion
set FILES=

set FILES=!FILES! ../GlobalExcel/Globals.xlsx

for /f "delims=" %%i in ('dir /b *.xl??') do (
set FILES=!FILES! %%i
echo !FILES!
)
..\tabtoy.exe --mode=exportorv2 --lan=zh_cn --csharp_out=..\..\..\Hotfix\TabToy\Bin\Config.cs --combinename=Config %FILES%

pause 