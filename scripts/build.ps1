dotnet publish ..\src\MigratorLogParser\MigratorLogParser.csproj -c Release -o ..\dist\win-x64\ -p:PublishSingleFile=true -p:PublishTrimmed=true -r win-x64 -f 'net6.0' --self-contained