dotnet build --configuration Release
dotnet pack C:\Users\e63550\source\repos\AceGenericClientCore_v.1\AceGenericClientCore\AceGenericClientCore\AceGenericClientCore.csproj --configuration Release --output .
nuget.exe push -source "https://pkgs.dev.azure.com/NbgNugets/NbgNugetsProject/_packaging/NbgNugetFeed/nuget/v3/index.json" -ApiKey AzureDevOps .\*.nupkg
del /f *.nupkg
pause