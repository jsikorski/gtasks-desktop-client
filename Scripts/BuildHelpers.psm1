function Write-Logo {
	param(
		[Parameter(Mandatory=$true)]
		[ValidateNotNullOrEmpty()]
		[string]
		$ProductName,
		
		[Parameter(Mandatory=$true)]
		[ValidateNotNullOrEmpty()]
		[string]
		$Version
	)

	Write-Host "-------------------------------------------------------"
	Write-Host "Building $ProductName v. $Version"
	Write-Host "-------------------------------------------------------"
}

function Get-SolutionDependencies {
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
		$NugetPackagesOutputDir
	)
	
	Exec { & $NugetPath install $NugetSlnConfig -o $NugetPackagesOutputDir }
}

function Get-ProjectsDependencies {
	param(
		[Parameter(Mandatory=$true)]
		[ValidateNotNullOrEmpty()]
		[string]
		$NugetPath,
		
		[Parameter(Mandatory=$true)]
		[ValidateNotNullOrEmpty()]
		[string]
		$NugetBaseFile,
		
		[Parameter(Mandatory=$true)]
		[ValidateNotNullOrEmpty()]
		[string]
		$NugetPackagesOutputDir
	)

	$nugetRepositoriesPaths = Get-NugetRepositoriesPaths -NugetBaseFile $NugetBaseFile
	foreach ($nugetRepositoryPath in $nugetRepositoriesPaths) {
		Exec { & $NugetPath install $nugetRepositoryPath -o $NugetPackagesOutputDir }
	}
}

function Get-NugetRepositoriesPaths {
	param(
		[Parameter(Mandatory=$true)]
		[ValidateNotNullOrEmpty()]
		[string]
		$NugetBaseFile
	)
	
	$nugetRepositories = ([xml](Get-Content $NugetBaseFile)).repositories.repository	
	$nugetRepositoriesPaths = @()
	foreach ($nugetRepository in $nugetRepositories) {
		$completeRepositoryPath = (Split-Path($NugetBaseFile)) + "\" + $nugetRepository.path
		$nugetRepositoriesPaths += @($completeRepositoryPath)
	}
	return $nugetRepositoriesPaths
}

function Start-Tests {
	param(
		[Parameter(Mandatory=$true)]
		[ValidateNotNullOrEmpty()]
		[string]
		$SlnFile,
	
		[Parameter(Mandatory=$true)]
		[ValidateNotNullOrEmpty()]
		[string]
		$NUnitPath, 
		
		[Parameter(Mandatory=$true)]
		[ValidateNotNullOrEmpty()]
		[string]
		$BuildDir,
		
		[string]
		$TestsResultsFile = "TestsResults.xml"
	)

	$allprojects = Get-AllProjects -SlnFile $SlnFile
	$testProjects = $allProjects | where { $_.Name.EndsWith(".Tests") }
	
	foreach ($testProject in $testProjects) {
		Write-Host "-------------------------------------------------------"
		Write-Host ("Running tests for project " + $testProject.Name)
		Write-Host "-------------------------------------------------------"
		
		$projectName = $testProject.Name
		$testProjectAssemblyPath = Join-Path $BuildDir "$projectName\$projectName.dll"

		Exec { & $NUnitPath $testProjectAssemblyPath /nologo /result=$TestsResultsFile }
	}
}

function Get-AllProjects {
	param(
		[Parameter(Mandatory=$true)]
		[ValidateNotNullOrEmpty()]
		[string]
		$SlnFile
	)
	
	$solutionDir = Split-Path $SlnFile
	$projects = @()
	Get-Content $SlnFile | 
		Select-String "Project\(" | 
			ForEach-Object {
				$projectParts = $_ -Split '[,=]' | ForEach-Object { $_.Trim('[ "{}]') };
				
				$projectName = $projectParts[1] 

				if ($projectName -eq ".nuget") {
					return
				}

				$projectRelativePath = Split-Path $projectParts[2]
				$projectAbsolutePAth = Join-Path $solutionDir $projectRelativePath

				$projects += @(New-Object PSObject -Property @{
					Name = $projectName
					Path = $projectAbsolutePAth
				})
			}

	return $projects
}

function Add-AssembliesInfos {
	param(
		[Parameter(Mandatory=$true)]
		[ValidateNotNullOrEmpty()]
		[string]
		$SlnFile, 

		[Parameter(Mandatory=$true)]
		[ValidateNotNullOrEmpty()]
		[string]
		$ProductName,

		[Parameter(Mandatory=$true)]
		[ValidateNotNullOrEmpty()]
		[string]
		$Version
	)

	$allProjects = Get-AllProjects -SlnFile $SlnFile
	foreach ($project in $allProjects) {
		$propertiesDir = Join-Path $project.Path "Properties"

		if (!(Test-Path $propertiesDir)) {
			New-Item -Path $propertiesDir -Type Directory | Out-Null
		}

		Add-AssemblyInfo -Project $Project -ProductName $ProductName -Version $Version
	}
}

function Add-AssemblyInfo {
	param(
		$Project,

		[Parameter(Mandatory=$true)]
		[ValidateNotNullOrEmpty()]
		[string]
		$ProductName, 

		[Parameter(Mandatory=$true)]
		[ValidateNotNullOrEmpty()]
		[string]
		$Version
	)

	$assemblyInfoDir = Join-Path $project.Path "Properties"
	$assemblyInfoPath = Join-Path $assemblyInfoDir "AssemblyInfo.cs"

	$projectName = $Project.Name

	@"
using System.Reflection;
using System.Runtime.InteropServices;

[assembly: AssemblyTitle("$projectName")]
[assembly: AssemblyProduct("$ProductName")]

[assembly: ComVisible(false)]

[assembly: AssemblyVersion("$Version")]
[assembly: AssemblyFileVersion("$Version")]
"@ | Out-File $assemblyInfoPath

}