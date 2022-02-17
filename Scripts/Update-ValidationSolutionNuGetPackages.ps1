#!/usr/bin/env pwsh
# Generates NuGet packages from the main code, then validates that the
# validation tests can run against the generated NuGet packages.
param(
  [Parameter(Mandatory=$true)]
  [string]$basedir,
  [switch]$singleNuGetDir=$false,
  [string]$nugetDir="",
  [string]$configuration="Debug"
  )

if (-not (test-path -PathType Container $basedir)) {
  write-error "basedir ($basedir) must be a valid directory"
  exit 1
}

if ($singleNuGetDir -and -not (test-path -pathtype Container $nugetDir)) {
  write-error "nugetDir ($nugetDir) must be a valid directory"
  exit 2
}

$validationDir = resolve-path (join-path $basedir -ChildPath "Validation")
$mainDir = resolve-path (join-path $basedir -ChildPath "Main")
$scriptDir = resolve-path (join-path $basedir -ChildPath "Scripts")
$solutionFile = resolve-path (join-path $mainDir -ChildPath "NUnit.Extension.DependencyInjection.sln")

if (-not $singleNuGetDir) {
  $idLocationMap = @{
    'NUnit.Extension.DependencyInjection.Abstractions' = resolve-path (join-path $mainDir -ChildPath src/NUnit.Extension.DependencyInjection.Abstractions -AdditionalChildPath bin,$configuration)
    'NUnit.Extension.DependencyInjection' = resolve-path (join-path $mainDir -ChildPath "src/NUnit.Extension.DependencyInjection/bin/$configuration")
    'NUnit.Extension.DependencyInjection.Unity' = resolve-path (join-path $mainDir -ChildPath "src/NUnit.Extension.DependencyInjection.Unity/bin/$configuration")
  }
} 
else
{
  $idLocationMap = @{
    'NUnit.Extension.DependencyInjection.Abstractions' = (resolve-path $nugetDir).Path
    'NUnit.Extension.DependencyInjection' = (resolve-path $nugetDir).Path
    'NUnit.Extension.DependencyInjection.Unity' = (resolve-path $nugetDir).Path
  }
}

$testProjects = @()
dir $validationDir/*Tests/*.csproj | % {
  $testProjects += @{
    ProjectFile = $_.FullName
    RequiredPkgs = @('NUnit.Extension.DependencyInjection', 'NUnit.Extension.DependencyInjection.Unity', 'NUnit.Extension.DependencyInjection.Abstractions')
    }
}
$validationSolution = (join-path $validationDir -ChildPath "ValidationTests.sln")

# update the nuget.config file so we can pick up the NuGet files
copy-item -force $validationDir/nuget.config.empty $validationDir/nuget.config
dotnet nuget add source https://api.nuget.org/v3/index.json --name nuget.org --configfile $validationDir/nuget.config
if ($singleNuGetDir) {
  $nugetDirPath = (resolve-path $nugetDir).Path
  dotnet nuget add source $nugetDirPath --name Solution --configfile $validationDir/nuget.config
} else {
  $idLocationMap.Keys | % {
    dotnet nuget add source $idLocationMap[$_] --name $_ --configfile $validationDir/nuget.config
  }
}

# do a restore of all the current packages before attempting to update
$validationSolution = (resolve-path $validationDir/ValidationTests.sln).Path
dotnet restore $validationSolution

# update the existing projects to use the recently-built NuGet packages
$testProjects | % {
  $project = $_
  $projectFile = $project.ProjectFile
  $project.RequiredPkgs | % {
    $id = $_
    $projectName = (get-item $projectFile).Name
    Write-Host "Updating package $id in $projectName using source '$($idLocationMap[$id])' ..." 
    . $scriptDir/Update-NuGetPackageInProject.ps1 -id $id -source $idLocationMap[$id] -installProject $projectFile
  }
}
