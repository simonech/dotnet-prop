dotnet restore
dotnet build
cd dotnet-prop
dotnet pack -o ../nupkgs
cd ..

Remove-Item C:\Users\user\.nuget\packages\dotnet-prop
Remove-Item C:\Users\user\.nuget\packages\.tools

cd consumerapp
dotnet restore
cd ..