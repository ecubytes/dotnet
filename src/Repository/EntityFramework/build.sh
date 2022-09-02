cd src
outdir=$HOME/artifacts/ecubytes/packages/Debug 
projectName=$(find *.csproj)
packageName=$(basename -s .csproj $projectName)
read -p "Enter Package Version: " version
read -p "Enter Version Suffix: " suffix
packageFile=$outdir/$packageName.$version-$suffix.nupkg
if [$version == ""]; then
    version=1.0.0
fi
if [ -f "$packageFile" ]; then
    rm $packageFile    
fi
dotnet pack -c Debug -o $outdir /p:Version=$version-$suffix