dotnet build --configuration Release
dotnet pack C:\Users\apeppas\source\Personal_Repos\GenericAceClient\AceGenericClientCore_v.3\AceGenericClientCore\AceGenericClientCore\Nbg.NetCore.Services.Ace.Http.csproj --configuration Release --output .
nuget.exe push -source "https://pkgs.dev.azure.com/NBGCICD/_packaging/NBGUploads/nuget/v3/index.json" -ApiKey AzureDevOps "C:\Users\apeppas\source\Personal_Repos\GenericAceClient\AceGenericClientCore_v.3\AceGenericClientCore\AceGenericClientCore\*.nupkg
del /f *.nupkg
pause