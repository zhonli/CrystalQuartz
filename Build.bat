cd ./tools
".nuget/NuGet.exe" install Rosalia -ExcludeVersion -OutputDirectory "./packages"
"./packages/Rosalia/tools/Rosalia.exe" /wd="../src" /task=BuildPackages "../tools/CrystalQuartz.Build/CrystalQuartz.Build.csproj"
pause