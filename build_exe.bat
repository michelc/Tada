@echo off
cd c:\Mvc\Tada
dotnet publish --output "c:/mvc/tada/exe" --runtime win-x64 --configuration Release -p:PublishSingleFile=true --self-contained false
echo.
pause
