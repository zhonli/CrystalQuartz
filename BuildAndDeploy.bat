cd ./tools
".nuget/NuGet.exe" install Rosalia -ExcludeVersion -OutputDirectory "./"
"./Rosalia/tools/Rosalia.exe" /wd="../src" /hold /task="PushPackages" "../tools/CrystalQuartz.Build/CrystalQuartz.Build.csproj"
pause