function Get-Psake {
    param(
        [Parameter(Mandatory=$true)]
        [ValidateNotNullOrEmpty()]
        [string]
        $NugetPath,
        
        [Parameter(Mandatory=$true)]
        [ValidateNotNullOrEmpty()]
        [string]
        $NugetSlnConfig,

        [Parameter(Mandatory=$true)]
        [ValidateNotNullOrEmpty()]
        [string]
        $NugetPackageOutputDir
    )

    Write-Host (Get-PsakePackage -NugetPath $NugetPath -NugetSlnConfig $NugetSlnConfig -NugetPackageOutputDir $NugetPackageOutputDir)
    return (Get-ChildItem $NugetPackageOutputDir -Recurse -Include psake.ps1)
}

function Get-PsakePackage {
    param (
        [Parameter(Mandatory=$true)]
        [ValidateNotNullOrEmpty()]
        [string]
        $NugetPath,

        [Parameter(Mandatory=$true)]
        [ValidateNotNullOrEmpty()]
        [string]
        $NugetSlnConfig,

        [Parameter(Mandatory=$true)]
        [ValidateNotNullOrEmpty()]
        [string]
        $NugetPackageOutputDir
    )

	$configXml = [xml](Get-Content $NugetSlnConfig)
	$psakeVersion = $configXml.SelectSingleNode("//package[@id='psake']").version
    Write-Host $psakeVersion
	& $NugetPath install psake -Version $psakeVersion -OutputDir $NugetPackageOutputDir
}