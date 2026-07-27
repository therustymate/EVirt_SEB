dotnet publish -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -p:IncludeNativeLibrariesForSelfExtract=true -p:EnableWindowsTargeting=true

cp ./bin/Release/net10.0/win-x64/publish/EVirtPatch.exe ../build/EVirtPatch.exe