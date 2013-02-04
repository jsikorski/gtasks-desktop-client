# Parameters:
#	$baseDir 	
# 	$nugetPath
# 	$nugetSlnConfig
# 	$nugetPackagesOutputDir


Properties {
	$versionMajorNumber   = "0"
	$versionMinorNumber   = "0"
	$productName 	   	  = "GTasks Desktop Client"
	$buildDir 	  	   	  = "$baseDir\Build"
	$releaseDir    	   	  = "$baseDir\Release"
 	$slnFile 	   	   	  = "$baseDir\Source\GTasksDesktopClient.sln"
	$nugetBaseFile	   	  = "$baseDir\Packages\repositories.config"
	$toolsDir 	   	   	  = "$baseDir\Tools"
}

Task Default -Depends ShowLogo, Release

Task GetVersion {
	$script:version = "$versionMajorNumber.$versionMinorNumber"
}

Task ShowLogo -Depends GetVersion {
	Write-Logo -ProductName $productName -Version $script:version
}

Task Clean {
	Remove-Item $buildDir -Recurse -ErrorAction SilentlyContinue
	Remove-Item $releaseDir -Recurse -ErrorAction SilentlyContinue
}

Task Init -Depends Clean {
	New-Item -Type Directory $buildDir | Out-Null
	New-Item -Type Directory $releaseDir | Out-Null
}

Task GenerateAssembliesInfos -Depends GetVersion {
	Add-AssembliesInfos -SlnFile $slnFile -ProductName $productName -Version $script:version
}

Task DownloadDependencies {
	Get-SolutionDependencies `
		-NugetPath $nugetPath `
		-NugetSlnConfig $nugetSlnConfig `
		-NugetPackagesOutputDir $nugetPackagesOutputDir
	
	Get-ProjectsDependencies `
		-NugetPath $nugetPath `
		-NugetBaseFile $nugetBaseFile `
		-NugetPackagesOutputDir $nugetPackagesOutputDir
}

Task Compile -depends Init, DownloadDependencies {
	Exec { MSBuild $slnFile /property:Configuration=Release /verbosity:quiet /nologo }
}

Task Release -Depends Compile {
	Copy-Item "$buildDir\*" $releaseDir
	Get-ChildItem $releaseDir | foreach { Copy-Item ("$buildDir\" + $_.Name + "\*.exe") ("$releaseDir\" + $_.Name) }
	Get-ChildItem $releaseDir | foreach { Copy-Item ("$buildDir\" + $_.Name + "\*.dll") ("$releaseDir\" + $_.Name) }
	Get-ChildItem $releaseDir | foreach { Copy-Item ("$buildDir\" + $_.Name + "\*.exe.config") ("$releaseDir\" + $_.Name) }
}