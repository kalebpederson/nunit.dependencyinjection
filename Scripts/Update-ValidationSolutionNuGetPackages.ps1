# Generates NuGet packages from the main code, then validates that the
# validation tests can run against the generated NuGet packages.
param(
  [Parameter(Mandatory=$true)]
  [string]$basedir,
  [switch]$singleNuGetDir=$false,
  [string]$nugetDir="",
  [string]$configuration="debug"
  )

if (-not (test-path -PathType Container $basedir)) {
  write-error "basedir ($basedir) must be a valid directory"
  exit 1
}

if ($singleNuGetDir -and -not (test-path -pathtype Container $nugetDir)) {
  write-error "nugetDir ($nugetDir) must be a valid directory"
  exit 2
}

$validationDir = (join-path $basedir -ChildPath "Validation")
$mainDir = (join-path $basedir -ChildPath "Main")
$scriptDir = (join-path $basedir -ChildPath "Scripts")
$solutionFile = (join-path $mainDir -ChildPath "NUnit.Extension.DependencyInjection.sln")

if (-not $singleNuGetDir) {
  $idLocationMap = @{
    'NUnit.Extension.DependencyInjection' = join-path $mainDir -ChildPath "NUnit.Extension.DependencyInjection\bin\$configuration"
    'NUnit.Extension.DependencyInjection.Unity' = join-path $mainDir -ChildPath "NUnit.Extension.DependencyInjection.Unity\bin\$configuration"
  }
} 
else
{
  $idLocationMap = @{
    'NUnit.Extension.DependencyInjection' = (resolve-path $nugetDir).Path
    'NUnit.Extension.DependencyInjection.Unity' = (resolve-path $nugetDir).Path
  }
}

$testProjects = @(
  @{
    ProjectFile = (join-path $validationDir -ChildPath "ConventionMappingTypeDiscovererTests\ConventionMappingTypeDiscovererTests.csproj")
    RequiredPkgs = @('NUnit.Extension.DependencyInjection', 'NUnit.Extension.DependencyInjection.Unity')
    },
  @{
    ProjectFile = (join-path $validationDir -ChildPath "IocRegistrarTypeDiscovererTests\IocRegistrarTypeDiscovererTests.csproj")
    RequiredPkgs = @('NUnit.Extension.DependencyInjection', 'NUnit.Extension.DependencyInjection.Unity')
    }
  )
$validationSolution = (join-path $validationDir -ChildPath "ValidationTests.sln")

# update the existing projects to use the recently-built NuGet packages
$testProjects | % {
  $project = $_
  $projectFile = $project.ProjectFile
  $project.RequiredPkgs | % {
    $id = $_
    $projectName = (get-item $projectFile).Name
    Write-Host "Updating package $id in $projectName..." 
    . $scriptDir\Update-NuGetPackageInProject.ps1 -id $id -source $idLocationMap[$id] -installProject $projectFile
  }
}


