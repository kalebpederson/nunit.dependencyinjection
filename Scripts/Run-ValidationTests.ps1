# Generates NuGet packages from the main code, then validates that the
# validation tests can run against the generated NuGet packages.
param(
  [Parameter(Mandatory=$true)]
  [string]$basedir,
  [string]$configuration="debug"
  )

if (-not (test-path $basedir)) {
  write-error 'basedir must be a valid directory'
  return
}

$validationDir = (join-path $basedir -ChildPath "Validation")
$mainDir = (join-path $basedir -ChildPath "Main")
$solutionFile = (join-path $mainDir -ChildPath "NUnit.Extension.DependencyInjection.sln")
$validationSolution = (join-path $validationDir -ChildPath "NUnit.Extension.DependencyInjection.ValidationTests.sln")
$validationTestDir = (join-path $validationDir -ChildPath "NUnit.Extension.DependencyInjection.Unity.IntegrationTests")
$dependencyInjectionProjDir = (join-path $mainDir -ChildPath "NUnit.Extension.DependencyInjection")
$dependencyInjectionUnityProjDir = (join-path $mainDir -ChildPath "NUnit.Extension.DependencyInjection.Unity")
$dependencyInjectionNupkgDir = (join-path $dependencyInjectionProjDir -ChildPath "bin\$configuration")
$dependencyInjectionUnityNupkgDir = (join-path $dependencyInjectionUnityProjDir -ChildPath "bin\$configuration")

# build the NuGet packages to use as the source files
Write-Host "Building updated NuGet packages..."
dotnet pack $solutionFile > $null

# update the existing project to use the just-built NuGet packages
try
{
  pushd $validationTestDir > $null 2>&1
  Write-Host "Updating NUnit.Extension.DependencyInjection in $($dependencyInjectionNupkgDir)..." 
  dotnet add package NUnit.Extension.DependencyInjection -s $dependencyInjectionNupkgDir > $null
  Write-Host "Updating NUnit.Extension.DependencyInjection.Unity in $($dependencyInjectionUnityNupkgDir)..." 
  dotnet add package NUnit.Extension.DependencyInjection.Unity -s $dependencyInjectionUnityNupkgDir > $null
}
finally
{
  popd > $null 2>&1
}

try
{
  pushd $validationDir > $null 2>&1
  Write-Host "Building and executing tests..."
  dotnet test
  $exitcode = $LASTEXITCODE
  if ($exitcode -ne 0) {
    throw "Validation tests failed with code ($exitcode)"
  }
}
finally
{
  popd > $null 2>&1
}

