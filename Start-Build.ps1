param(
	[string]
	$task = "Default"
)

Import-Module .\Scripts\PsakeHelpers.psm1
Import-Module .\Scripts\BuildHelpers.psm1

$baseDir = Split-Path $MyInvocation.MyCommand.Path
$visualStudioDir = "$baseDir\Sources\VisualStudio"

$nugetPath = "$baseDir\Tools\NuGet\NuGet.exe"
$nugetSlnConfig = "$baseDir\Source\.nuget\packages.config" 
$nugetPackagesOutputDir = "$baseDir\Packages"

$psake = Get-Psake `
    -NugetPath $nugetPath `
    -NugetSlnConfig $nugetSlnConfig `
    -NugetPackageOutputDir $nugetPackagesOutputDir
    
& $psake .\Scripts\Build.ps1 `
	-Task $task `
	-Framework "4.0" `
	-Parameters @{
		"baseDir"=$baseDir;
		"nugetPath"=$nugetPath;
		"nugetSlnConfig"=$nugetSlnConfig;
		"nugetPackagesOutputDir"=$nugetPackagesOutputDir;
	}

Remove-Module psake
Remove-Module BuildHelpers
Remove-Module PsakeHelpers