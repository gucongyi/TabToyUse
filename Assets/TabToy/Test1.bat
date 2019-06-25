cd /D %~dp0 
cd /D Excel
setlocal enabledelayedexpansion
set FILES=


set FILES=!FILES! ../GlobalExcel/Globals.xlsx


for /f "delims=" %%i in ('dir /b *.xl??') do (
set FILES=!FILES! %%i
echo !FILES!
)
rem cd ..
rem dir
REM ..\tabtoy.exe --mode=exportorv2 --lan=zh_cn --json_out=..\Bin\Config.json --combinename=Config %FILES%
..\tabtoy.exe --mode=exportorv2 --lan=zh_cn --binary_out=..\..\Resources\DataProxy\Bin\DataProxyConfig.bytes --combinename=Config %FILES%
rem ..\tabtoy.exe --mode=exportorv2 --lan=zh_cn --binary_out=..\..\..\..\..\G05NetCoreAIServer\HorseAIServer\HorseAIServer\TabToy\Bin\DataProxyConfig.bytes --combinename=Config %FILES%
pause 