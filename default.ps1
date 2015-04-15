Properties {
    ### Directories
    $base_directory = resolve-path .
	$src_directory = "$base_directory\src"
	$output_directory = "$base_directory\build"
	$dist_directory = "$base_directory\distribution"
    $tests_dir = "$base_directory\src\tests"
    $package_dir = "$base_directory\packages"

    ### Tools
    $nuget = "$base_directory\.nuget\nuget.exe"
    $nunit = "$package_dir\nunit.runners*\tools\nunit-console.exe"

    ### AppVeyor-related
    $appVeyorConfig = "$base_directory\appveyor.yml"
    $appVeyor = $env:APPVEYOR

    ### Project information
	$solution = "Hepsi.CommandProcessor.sln"
    $sln_file = "$base_directory\$solution"
    $target_config = "Release"

	### Build information
	$buildNumber = 0;
	$version = "1.0.0.0"
	$preRelease = $null
}

## Tasks

Task default -Depends CreateNuGetPackage
Task appVeyor -Depends CreateNuGetPackage

Task Restore {
    "Restoring NuGet packages for '$solution'..."

    Exec { .$nuget restore $solution }
}

Task Clean {
	rmdir $output_directory -ea SilentlyContinue -recurse
	rmdir $dist_directory -ea SilentlyContinue -recurse
	exec { msbuild /nologo /verbosity:quiet $sln_file /p:Configuration=$target_config /t:Clean /p:Outdir="$output_directory" }
}

task UpdateVersion {
	"Updating version number..."

	$vSplit = $version.Split('.')
	if($vSplit.Length -ne 4)
	{
		throw "Version number is invalid. Must be in the form of 0.0.0.0"
	}
	$major = $vSplit[0]
	$minor = $vSplit[1]
	$patch = $vSplit[2]
	$assemblyFileVersion =  "$major.$minor.$patch.$buildNumber"
	$assemblyVersion = "$major.$minor.0.0"
	$versionAssemblyInfoFile = "$src_directory/Shared/VersionAssemblyInfo.cs"
	"using System.Reflection;" > $versionAssemblyInfoFile
	"" >> $versionAssemblyInfoFile
	"[assembly: AssemblyVersion(""$assemblyVersion"")]" >> $versionAssemblyInfoFile
	"[assembly: AssemblyFileVersion(""$assemblyFileVersion"")]" >> $versionAssemblyInfoFile
}

Task Compile -Depends Clean, Restore, UpdateVersion {
    "Compiling '$solution'..."

	exec { msbuild /nologo /verbosity:q $sln_file /p:Configuration=$target_config /p:TargetFrameworkVersion=v4.5 /p:Outdir="$output_directory" }
}

Task RunTests -Depends Compile {
	"Runing Unit Tests for '$solution'..."

	$project = "Hepsi.CommandProcessor.UnitTests"
	.$nunit "$output_directory\$project.dll" /noxml
	mkdir $dist_directory -ea SilentlyContinue
}

Task CreateNuGetPackage -Depends RunTests {
	"Creating nuget package..."

	$vSplit = $version.Split('.')
	if($vSplit.Length -ne 4)
	{
		throw "Version number is invalid. Must be in the form of 0.0.0.0"
	}
	$major = $vSplit[0]
	$minor = $vSplit[1]
	$patch = $vSplit[2]
	$packageVersion =  "$major.$minor.$patch"
	if($preRelease){
		$packageVersion = "$packageVersion-$preRelease"
	}
	
	if ($buildNumber -ne 0){
		$packageVersion = $packageVersion + "-build" + $buildNumber.ToString().PadLeft(5,'0')
	}

	copy-item $src_directory\Hepsi.CommandProcessor\Hepsi.CommandProcessor.nuspec $dist_directory
}
